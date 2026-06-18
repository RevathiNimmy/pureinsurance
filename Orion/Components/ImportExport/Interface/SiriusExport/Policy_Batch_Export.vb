Option Strict On

Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports System.Xml
Imports System.Xml.Schema
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.IO
Imports System.Xml.XPath
Imports SharedFiles

Public NotInheritable Class Policy_Batch_Export : Inherits ExportBase

    Friend Enum GetAddressColumns As Integer
        Address1 = 3
        Address2 = 4
        Address3 = 5
        Address4 = 6
        PostalCode = 7
        CountryCode = 15
    End Enum

    Friend Enum GisPropertySpecialType As Integer
        SumInsured = 4
        StandardWording = 5
    End Enum

    Friend dataModelCache As New StringDictionary

    Const ClaimsBulderEnabledProductOption As Integer = 12

    Private m_nBatchId As Integer = 0
    Public Overrides Property BatchId() As Integer
        Get
            Return m_nBatchId
        End Get
        Set(ByVal value As Integer)
            m_nBatchId = value
        End Set
    End Property

    Private _dStartDate As Nullable(Of DateTime) = Nothing

    Public Property StartDate() As Nullable(Of DateTime)
        Get
            Return _dStartDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dStartDate = value
        End Set
    End Property

    Private _dEndDate As Nullable(Of DateTime) = Nothing
    Private _sXSLTFileName As String = String.Empty
    Private _sXSLTDestFolder As String = String.Empty
    Private _sXSLTFileExt As String = String.Empty

    Public Property EndDate() As Nullable(Of DateTime)
        Get
            Return _dEndDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dEndDate = value
        End Set
    End Property

    Public Property sXSLTFileName() As String
        Get
            Return _sXSLTFileName
        End Get
        Set(ByVal value As String)
            _sXSLTFileName = value
        End Set
    End Property

    Public Property sXSLTDestFolder() As String
        Get
            Return _sXSLTDestFolder
        End Get
        Set(ByVal value As String)
            _sXSLTDestFolder = value
        End Set
    End Property

    Public Property sXSLTFileExt() As String
        Get
            Return _sXSLTFileExt
        End Get
        Set(ByVal value As String)
            _sXSLTFileExt = value
        End Set
    End Property

#Region "Public Properties"

    ' Builds the export filename for this interface
    Public Overrides ReadOnly Property Filename() As String
        Get
            If (m_sFilename.Length = 0) Then
                m_sFilename = String.Format("{0}_{1}_{2}.xml", InterfaceName, Me.BatchId, Now.ToString("yyyyMMddhhmm"))
            End If
            Return m_sFilename
        End Get
    End Property

    ' Interface name
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Policy_Batch_Export"
        End Get
    End Property

#End Region

#Region "Public Methods"

    ' Display help for this interface
    Public Overrides Sub DisplayHelp()
        ' Write syntax help for this command
        OutputLine("Example call : - SIRIUSEXPORT Policy_Batch_Export StartDate=""01/01/2008"" EndDate=""01/01/2009""")
        OutputLine()
        OutputLine("batch_id    - (optional) batch id to re-export a batch")
        OutputLine("start_date  - (optional) optional date filter")
        OutputLine("end_date    - (optional) optional date filter")
    End Sub

    ' Process the command line for this interface
    Public Overrides Sub ProcessCommandLine(ByVal cArgs As Collection(Of String))

        If cArgs.Count >= 0 Then

            Dim NoofCommandLineArgs As Integer
            Dim lItem As Integer = 0
            Dim sArg As String
            Dim sArgValues() As String

            ' get the number of command line arguments passed
            NoofCommandLineArgs = cArgs.Count - 1

            ' for each command line argument passed
            For lItem = 0 To NoofCommandLineArgs

                ' get the argument (should be in the format (argument_name = argument_value)
                sArg = cArgs(lItem).ToString()

                ' split the argument into argument name / argument value
                sArgValues = sArg.Split(CChar("="))

                ' The key should have a value in key=value i.e. 
                ' an array of length 2
                If sArgValues.Length = 1 Then
                    Throw New ArgumentException("Invalid argument - Value missing for " & sArgValues(0), sArgValues(0))
                End If

                Try

                    ' determine which argument we are looking at
                    Select Case sArgValues(0).ToUpper

                        Case "BATCH_ID"
                            ' Try and grab a batch id
                            Me.BatchId = CInt(sArgValues(1))

                        Case "START_DATE"
                            ' Try and grab a start date
                            Me.StartDate = Convert.ToDateTime(sArgValues(1))

                        Case "END_DATE"
                            ' Try and grab an end date
                            Me.EndDate = Convert.ToDateTime(sArgValues(1))

                        Case "XSLT_FILE_NAME"
                            ' Try and grab the Xslt File Name
                            Me.sXSLTFileName = CStr(sArgValues(1))

                        Case "DEST_FILE_EXTN"
                            ' Try and grab the Export File Extension
                            Me.sXSLTFileExt = CStr(sArgValues(1))

                        Case "DEST_FOLDER"
                            ' Try and grab the Destination Folder
                            Me.sXSLTDestFolder = CStr(sArgValues(1))

                        Case "OVERRIDE_BATCH_SELECT"
                            ' Not usefull in this case
                        Case Else
                            ' Throw an error on finding an unrecognisable param
                            Throw New ArgumentException()
                    End Select

                Catch ex As Exception
                    Throw New ArgumentException("Invalid argument", "batchid", ex)
                End Try

            Next

        End If
    End Sub

    ' Process the export
    Public Overrides Sub ProcessExport()

        ' Write status line
        OutputLine(String.Format("Policy Batch Export start_date={0} end_date={1}", _dStartDate, _dEndDate))
        OutputLine()

        ' Existing batch or new one?
        If m_nBatchId > 0 Then
            OutputLine(String.Format("Recreating batch {0}", m_nBatchId))

            ' Export the old batch 
            ExportBatch(False)
        Else
            Output("Creating new batch...")

            ' Create a new batch
            CreateBatch()
            OutputLine(String.Format("Created batch {0}", m_nBatchId))

            'Export the new batch
            ExportBatch(True)
            UpdateBatchTask(kBatchStatusComplete, CInt(Me.BatchId), Filename, 0, 0)
        End If
    End Sub


#End Region

#Region "Private Methods"

    Private Sub CreateBatch()

        Dim nReturnValue As Integer = PMEReturnCode.PMTrue

        Try

            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "StartDate", StartDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "EndDate", EndDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)

            ' Execute command
            nReturnValue = m_oDatabase.SQLAction("spu_PolicyBatchExport_CreateBatch", "Create Batch", True)
            If nReturnValue <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_CLM_ClaimsExport_CreateBatch'")
            End If

            ' Get batch id
            Me.BatchId = Util.ToSafeInt(m_oDatabase.Parameters.Item("batch_id").Value, 0)

        Catch ex As Exception

            Throw New Exception("Unable to create new export batch", ex)

        End Try

    End Sub

    Public Sub ExportBatch(ByVal bIsNew As Boolean)

        Dim iReturn As Long
        Dim oXML As New XmlDocument ' MSXML2.DOMDocument = New MSXML2.DOMDocument

        Dim oXMLReaderSettings As XmlReaderSettings = New XmlReaderSettings()
        Dim oXMLReader As XmlReader = Nothing

        Dim vResult As Object = String.Empty
        Dim obGis As bGIS.Application = Nothing
        Dim sXMLDatasetDef As Object = String.Empty
        Dim sXMLDataset As Object = String.Empty
        Dim v_sGisDataModelCode As Object = String.Empty

        Dim sFileName As String

        obGis = New bGIS.Application

        ' Get batch XML

        'Me.BatchId = 0
        Output("Retrieving batch...")

        Dim comReturnValue As Integer

        ' Increase the query timeout
        m_oDatabase.QueryTimeout = 80000

        ' Add parameters
        AddParameterLite(m_oDatabase, "batch_id", m_nBatchId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "new_batch", bIsNew, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

        If _dStartDate.HasValue Then
            AddParameterLite(m_oDatabase, "start_date", _dStartDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
        End If
        If _dEndDate.HasValue Then
            AddParameterLite(m_oDatabase, "end_date", _dEndDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
        End If

        comReturnValue = m_oDatabase.SQLSelectForXML("spu_PolicyBatchExport_XML_Select", True, oXML)

        If comReturnValue <> PMEReturnCode.PMTrue Then
            Throw New Exception("spu_PolicyBatchExport_XML_Select Failed")
        End If

        Dim xmlDoc As XmlDocument = New XmlDocument
        xmlDoc.LoadXml(oXML.InnerXml)

        Try

            Dim sUsername As String = "sirius"
            Dim sPassword As String = "sirius"
            Dim iUserID As Short = 1
            Dim sCallingAppName As String = ACApp
            Dim iSourceID As Short = 1
            Dim iLanguageID As Short = 1
            Dim iCurrencyID As Short = 26
            Dim iLogLevel As Short = 6
            'Modified as comReturnValue
            'comReturnValue = obGis.Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, vDatabase:=CObj(m_oDatabase))
            comReturnValue = CInt(obGis.Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, vDatabase:=CObj(m_oDatabase)))

            If comReturnValue <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("bGis.Application.Initialise Failed")
            End If

            Dim namespaceManager As New XmlNamespaceManager(xmlDoc.NameTable)
            namespaceManager.AddNamespace("rss", "http://www.siriusfs.com/SFI/Export/Policy_Batch_Export/20060419")

            comReturnValue = ProcessPartyBuilderElements(obGis, xmlDoc, namespaceManager)
            If comReturnValue <> PMEReturnCode.PMTrue Then
                If comReturnValue <> PMEReturnCode.PMNotFound Then
                    Throw New ApplicationException("ProcessPartyBuilderElements Failed")
                End If
            End If

            comReturnValue = ProcessRiskBuilderElements(obGis, xmlDoc, namespaceManager)
            If comReturnValue <> PMEReturnCode.PMTrue Then
                If comReturnValue <> PMEReturnCode.PMNotFound Then
                    Throw New ApplicationException("ProcessRiskBuilderElements Failed")
                End If
            End If

        Catch ex As Exception

            Throw New ApplicationException(ex.Message.ToString())

        Finally

            'destroycominterop
            '            DestroyCOMInterop(CObj(obGis), False)
            obGis.Dispose()
            obGis = Nothing
        End Try

        ' Check for xml
        If xmlDoc.OuterXml.Length = 0 Then

            OutputLine(String.Format("Batch {0} does not exist", Me.BatchId))

        Else

            OutputLine("Done")
            Output(String.Format("Writing output file...{0}...", Filename))

            Dim processingInstruction As XmlProcessingInstruction
            ' Create the processing instruction (header) for this file.
            processingInstruction = xmlDoc.CreateProcessingInstruction("xml", "version=""1.0""")
            xmlDoc.InsertBefore(processingInstruction, xmlDoc.FirstChild)

            ' Tidy up the header wrapper
            GenerateSchemaHeader(oXML)

            ' Save XML
            sFileName = FullPath
            xmlDoc.Save(sFileName)
            OutputLine("Done")

            Try

                OutputLine("Validating Exported XML File Format")

                oXMLReaderSettings.Schemas.Add("http://www.siriusfs.com/SFI/Export/Policy_Batch_Export/20060419", System.AppDomain.CurrentDomain.BaseDirectory() & "\" & "Policy_Batch_Export.xsd")
                oXMLReaderSettings.ValidationType = ValidationType.Schema
                oXMLReader = XmlReader.Create(sFileName, oXMLReaderSettings)

                While oXMLReader.Read()
                End While
                oXMLReader.Close()

                OutputLine("Validation Completed")

            Catch ex As XmlException
                UpdateBatchTask(kBatchStatusFailed, ToSafeInteger(Me.BatchID), Filename, 0, 0)
                OutputLine("Invalid XML File Format")
                Throw New ApplicationException("Export file has an invalid XML File Format", ex)

            Catch ex As Exception
                Throw New ApplicationException("Export file has an invalid XML File Format", ex)

            Finally
                oXMLReader = Nothing
                oXMLReaderSettings = Nothing
            End Try

        End If
    End Sub

    Private Function ProcessPartyBuilderElements(ByVal obGis As bGIS.Application, ByRef xmlDoc As XmlDocument, ByVal namespaceManager As XmlNamespaceManager) As Integer
        Dim comReturnValue As Integer = PMEReturnCode.PMTrue
        Dim partyNodes As XmlNodeList = xmlDoc.SelectNodes("/rss:EXPORT_HEADER/rss:PARTY", namespaceManager)
        Debug.WriteLine(partyNodes.Count.ToString())

        If partyNodes Is Nothing Then
            Return comReturnValue
        End If

        For Each partyNode As XmlNode In partyNodes

            Dim partyCnt As Integer = Util.ToSafeInt(partyNode.Attributes.GetNamedItem("party_cnt").Value, 0)

            ' each claim node will have a cDataSection as its first and only child node
            Dim childNodes As XmlNodeList = partyNode.ChildNodes

            Dim xmlDataSetDef As String = String.Empty
            Dim xmlDataSet As String = String.Empty
            Dim gisDataModelCode As String = String.Empty

            comReturnValue = obGis.LoadPartyFromDB( _
                        r_sXMLDataSetDef:=xmlDataSetDef, _
                        r_sXMLDataset:=xmlDataSet, _
                        r_sGISDataModelCode:=gisDataModelCode, _
                        v_lPartyCnt:=partyCnt)

            If comReturnValue <> PMEReturnCode.PMTrue Then
                If comReturnValue <> PMEReturnCode.PMNotFound Then
                    Throw New ApplicationException("bGis.Application.LoadPartyFromDB Failed")
                End If
            End If

            If Not String.IsNullOrEmpty(xmlDataSet) Then
                Dim errorElement As String = String.Empty

                xmlDataSet = TransformDatasetPBToRelease(xmlDataSet, errorElement)

                Dim PartyData As XmlNode = partyNode.Item("PARTYBUILDER")

                'JP 24/03/2010 check if PartyData node is not nothing
                If PartyData IsNot Nothing Then
                    PartyData.ChildNodes(0).Value = xmlDataSet
                End If

                ' if we have retrieved some xmldataset data
                ' for this particular claim
                'For Each childNode As XmlNode In childNodes

                '    If childNode.NodeType = XmlNodeType.CDATA Then

                '        Dim cDataSection As XmlCDataSection

                '        cDataSection = TryCast(childNode, XmlCDataSection)

                '        If cDataSection IsNot Nothing Then
                '            'Debug.Print(cDataSection.Value)
                '            cDataSection.Value = xmlDataSet
                '        End If

                '    End If

                'Next

            End If
        Next
        Return comReturnValue
    End Function

    Private Function ProcessRiskBuilderElements(ByVal obGis As bGIS.Application, ByRef xmlDoc As XmlDocument, ByVal namespaceManager As XmlNamespaceManager) As Integer
        Dim comReturnValue As Integer = PMEReturnCode.PMTrue
        Dim riskNodes As XmlNodeList = xmlDoc.SelectNodes("/rss:EXPORT_HEADER/rss:PARTY/rss:POLICY/rss:RISK", namespaceManager)
        Debug.WriteLine(riskNodes.Count.ToString())

        If riskNodes Is Nothing Then
            Return comReturnValue
        End If

        For Each riskNode As XmlNode In riskNodes

            Dim riskCnt As Integer = Util.ToSafeInt(riskNode.Attributes.GetNamedItem("risk_cnt").Value, 0)
            Dim insuranceFolderCnt As Integer = Util.ToSafeInt(riskNode.ParentNode.Attributes.GetNamedItem("insurance_folder_cnt").Value, 0)

            ' each claim node will have a cDataSection as its first and only child node
            Dim xmlDataSetDef As String = String.Empty
            Dim xmlDataSet As String = String.Empty
            Dim gisDataModelCode As String = String.Empty

            comReturnValue = obGis.LoadRiskFromDB( _
                        r_sXMLDataSetDef:=xmlDataSetDef, _
                        r_sXMLDataset:=xmlDataSet, _
                        r_sGISDataModelCode:=gisDataModelCode, _
                        v_lRiskID:=riskCnt, _
                        v_lInsuranceFileCnt:=insuranceFolderCnt)

            If comReturnValue <> PMEReturnCode.PMTrue Then
                If comReturnValue <> PMEReturnCode.PMNotFound Then
                    Throw New ApplicationException("bGis.Application.LoadRiskFromDB Failed")
                End If
            End If

            If Not String.IsNullOrEmpty(xmlDataSet) Then
                Dim errorElement As String = String.Empty

                xmlDataSet = TransformDatasetPBToRelease(xmlDataSet, errorElement)

                ' if we have retrieved some xmldataset data
                ' for this particular claim
                'Dim childNodes As XmlNodeList = riskNode.ChildNodes

                Dim riskData As XmlNode = riskNode.Item("RISKDATA")

                'JP 24/03/2010 check if riskData node is not nothing
                If riskData IsNot Nothing Then
                    riskData.ChildNodes(0).Value = xmlDataSet
                Else

                    'Create a new node.
                    Dim elem As XmlNode = xmlDoc.CreateNode(XmlNodeType.Element, "RISKDATA", "")

                    Dim cDataSection As XmlCDataSection

                    cDataSection = TryCast(xmlDoc.CreateNode(XmlNodeType.CDATA, "RISKDATA", ""), XmlCDataSection)

                    If cDataSection IsNot Nothing Then
                        'Debug.Print(cDataSection.Value)
                        cDataSection.Value = xmlDataSet
                    End If

                    elem.AppendChild(cDataSection)
                    'Add the node to the document.
                    riskNode.AppendChild(elem)

                End If

                'For Each childNode As XmlNode In childNodes

                '    If childNode.NodeType = XmlNodeType.CDATA Then

                '        Dim cDataSection As XmlCDataSection

                '        cDataSection = TryCast(childNode, XmlCDataSection)

                '        If cDataSection IsNot Nothing Then
                '            'Debug.Print(cDataSection.Value)
                '            cDataSection.Value = xmlDataSet
                '        End If

                '    End If

                'Next

            End If
        Next
        Return comReturnValue
    End Function
    Private Sub GetSourceDetails(ByRef sourceDetails As Object(,))

        Dim vResults As Object = Nothing

        m_oDatabase.Parameters.Clear()
        ' Execute sql 
        Dim dbreturnCode As Integer
        dbreturnCode = m_oDatabase.SQLSelect("spu_PM_SelAll_Source", "spu_PM_SelAll_Source", True, vResultArray:=vResults)
        If dbreturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("spu_PM_SelAll_Source failed")
        End If

        sourceDetails = DirectCast(vResults, Object(,))

    End Sub

    Private Function GetAddressXPathAttributes(ByVal DataModelCode As String) As String

        Dim xPathAttributes As String = String.Empty
        Dim CacheKey As String

        Const NoAddressAttributesFound As String = "NoAddressAttributesFound"

        ' Generate a Cache Key using the DataModelCode as we may have two DataModels
        CacheKey = "Address_XPathAttributes_" & DataModelCode

        ' Try to get the Full List from the Cache
        If dataModelCache.ContainsKey(CacheKey) Then
            xPathAttributes = dataModelCache.Item(CacheKey)
        End If

        Dim resultArray As Object = Nothing
        Dim dbReturnCode As Integer
        Dim noOfRecords As Integer
        Dim addressXPathDetails As Object(,)

        ' if there wasnt a value in the cache
        If String.IsNullOrEmpty(xPathAttributes) Then

            AddParameterLite(m_oDatabase, "GisDataModelCode", DataModelCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)

            dbReturnCode = m_oDatabase.SQLSelect("spu_SAM_Get_Dataset_Address_Properties", "spu_SAM_Get_Dataset_Address_Properties", True, noOfRecords, resultArray)
            If dbReturnCode <> PMEReturnCode.PMTrue Then
                Throw New Exception("spu_SAM_Get_Dataset_Address_Properties failed")
            End If

            If resultArray IsNot Nothing And noOfRecords <> 0 Then

                addressXPathDetails = DirectCast(resultArray, Object(,))

                ' get array bounds
                Dim lBound As Integer = addressXPathDetails.GetLowerBound(1)
                Dim UBound As Integer = addressXPathDetails.GetUpperBound(1)

                ' if the option is set on any branch assume claims builder is enabled for all
                For xPathAttribute As Integer = lBound To UBound

                    Dim addressXPathAttribute As String = addressXPathDetails(1, xPathAttribute).ToString()

                    If String.IsNullOrEmpty(xPathAttributes) Then
                        xPathAttributes = xPathAttributes & addressXPathAttribute
                    Else
                        xPathAttributes += " | " & addressXPathAttribute
                    End If
                Next
            End If
        End If

        ' if there are no attributes add value to cache stating no attributes
        If String.IsNullOrEmpty(xPathAttributes) Then
            xPathAttributes = NoAddressAttributesFound
        End If

        If dataModelCache.ContainsKey(CacheKey) = False Then
            ' add entry to the cache
            dataModelCache.Add(CacheKey, xPathAttributes)
        End If

        ' if there are no attributes 
        If xPathAttributes = NoAddressAttributesFound Then
            Return String.Empty
        Else
            Return xPathAttributes
        End If

    End Function

    Private Function GetSumInsuredAttributes( _
    ByVal dataModelCode As String) As String

        Dim xPathAttributes As String = String.Empty

        Const NoSumInsuredAttributesFound As String = "NoSumInsuredFound"

        ' Generate a Cache Key using the DataModelCode as we may have two DataModels
        Dim CacheKey As String = "SumInsurd_XPathAttributes_" + dataModelCode

        ' Try to get the Full List from the Cache
        If dataModelCache.ContainsKey(CacheKey) Then
            xPathAttributes = dataModelCache.Item(CacheKey)
        End If

        ' if there wasnt a value in the cache
        If String.IsNullOrEmpty(xPathAttributes) Then

            Dim dbReturnCode As Integer
            Dim resultArray As Object = Nothing
            Dim noOfRecords As Integer

            ' get special property type xpath attributes for data model
            AddParameterLite(m_oDatabase, "gisdatamodelcode", dataModelCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)

            dbReturnCode = m_oDatabase.SQLSelect("spu_SAM_Get_SumInsured_Properties", "spu_SAM_Get_SumInsured_Properties", True, noOfRecords, resultArray)
            If dbReturnCode <> PMEReturnCode.PMTrue Then
                Throw New Exception("spu_SAM_Get_SumInsured_Properties failed")
            End If

            If resultArray IsNot Nothing And noOfRecords <> 0 Then

                Dim sumInsuredXPathDetails As Object(,) = DirectCast(resultArray, Object(,))

                ' get array bounds
                Dim lBound As Integer = sumInsuredXPathDetails.GetLowerBound(1)
                Dim uBound As Integer = sumInsuredXPathDetails.GetUpperBound(1)

                ' if the option is set on any branch assume claims builder is enabled for all
                For xPathAttribute As Integer = lBound To uBound

                    ' combine elements into string 
                    Dim specialPropertyTypeXPathAttribute As String = sumInsuredXPathDetails(0, xPathAttribute).ToString() + "*|*" + _
                                sumInsuredXPathDetails(1, xPathAttribute).ToString() + "*|*" + _
                                sumInsuredXPathDetails(2, xPathAttribute).ToString()

                    If String.IsNullOrEmpty(xPathAttributes) Then
                        xPathAttributes = xPathAttributes & specialPropertyTypeXPathAttribute.Trim
                    Else
                        xPathAttributes += " | " & specialPropertyTypeXPathAttribute.Trim
                    End If

                Next

            End If

        End If

        ' if there are no attributes add value to cache stating no attributes
        If String.IsNullOrEmpty(xPathAttributes) Then
            xPathAttributes = NoSumInsuredAttributesFound
        End If

        If dataModelCache.ContainsKey(CacheKey) = False Then
            ' add entry to the cache
            dataModelCache.Add(CacheKey, xPathAttributes)
        End If

        ' if there are no attributes 
        If xPathAttributes = NoSumInsuredAttributesFound Then
            Return String.Empty
        Else
            Return xPathAttributes
        End If

    End Function

    Private Sub IncludeSumInsuredElements( _
    ByRef xmlDataSet As String, _
    ByVal dataModelCode As String, _
    ByVal policyLinkId As Integer)

        Const gisObjectName As Integer = 0
        Const gisPropertyName As Integer = 1
        Const specialsTypeReference As Integer = 2

        Dim xPaths As String = GetSumInsuredAttributes(dataModelCode)

        Dim oXML As Xml.XmlDocument = New Xml.XmlDocument

        If Not String.IsNullOrEmpty(xPaths) Then

            Dim document As XmlDocument = New XmlDocument
            document.Load(New StringReader(xmlDataSet))
            Dim navigator As XPathNavigator = document.CreateNavigator()

            navigator.MoveToRoot()

            Dim arrayOfXPaths As String() = Split(xPaths, " | ", )

            If arrayOfXPaths IsNot Nothing Then

                Dim lBound As Integer = arrayOfXPaths.GetLowerBound(0)
                Dim uBound As Integer = arrayOfXPaths.GetUpperBound(0)

                For xPathItem As Integer = lBound To uBound

                    Dim arrayofXPathAttributes As String() = Split(arrayOfXPaths(xPathItem), "*|*")

                    Dim xPath As String = "//" + arrayofXPathAttributes(gisObjectName)
                    Dim tagName As String = arrayofXPathAttributes(gisPropertyName)
                    Dim sumInsuredType As Integer = Util.ToSafeInt(arrayofXPathAttributes(specialsTypeReference), 0)

                    Dim nodes As XPathNodeIterator = navigator.Select(xPath)

                    While nodes.MoveNext

                        nodes.Current.MoveToAttribute("OI", "")
                        navigator.MoveToId(nodes.Current.Value)

                        Dim resultArray As Object = Nothing

                        AddParameterLite(m_oDatabase, "gis_datamodel_code", dataModelCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
                        AddParameterLite(m_oDatabase, "gis_policy_link_id", policyLinkId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                        AddParameterLite(m_oDatabase, "sum_insured_type_id", sumInsuredType, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                        AddParameterLite(m_oDatabase, "tagname", tagName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

                        m_oDatabase.SQLSelectForXML("spu_SAM_Get_Sum_Insured", True, oXML) 'CType(CType(oXML, MSXML2.IXMLDOMDocument2), MSXML2.DOMDocument40))

                        If Not String.IsNullOrEmpty(oXML.InnerXml) Then
                            navigator.AppendChild(oXML.InnerXml)
                        End If

                    End While

                    navigator.MoveToRoot()

                Next

            End If

            xmlDataSet = CType(navigator.UnderlyingObject, XmlDocument).OuterXml.ToString()

        End If

    End Sub

    ' You cannot currently add standard wordings to claims - it blows up all over the shop.
    Private Sub IncludeStandardWordingElements( _
        ByRef xmlDataSet As String, _
        ByVal dataModelCode As String, _
        ByVal policyBinderId As Integer)

        'Dim resultArray As Object = Nothing
        'Dim noOfRecords As Integer
        'Dim dbReturnCode As Integer

        'AddParameterLite(m_oDatabase, "gis_datamodel_code", dataModelCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
        'AddParameterLite(m_oDatabase, "gis_policy_binder_id", policyBinderId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

        'dbReturnCode = m_oDatabase.SQLSelect("spu_SAM_Get_Standard_Wording_Values", "spu_SAM_Get_Standard_Wording_Values", True, noOfRecords, resultArray)
        'If dbReturnCode <> PMEReturnCode.PMTrue Then
        '    Throw New Exception("spu_SAM_Get_Standard_Wording_Values Failed")
        'End If

        'If resultArray IsNot Nothing AndAlso noOfRecords <> 0 Then

        '    Dim standardWordingAttributes As Object(,) = DirectCast(resultArray, Object(,))

        '    Dim lBound As Integer = standardWordingAttributes.GetLowerBound(0)
        '    Dim uBound As Integer = standardWordingAttributes.GetUpperBound(0)

        '    Dim documentCode As String = ""
        '    Dim childObject As Integer = 0

        '    Dim document As New XmlDocument

        '    document.LoadXml(xmlDataSet)

        '    Dim navigator As XPathNavigator = document.CreateNavigator()

        '    navigator.MoveToRoot()

        '    For standardWording As Integer = lBound To uBound


        '        '//GENERAL[@QBENZ_GENERAL_ID="700133539"]
        '        If Cast.DefaultIfNull(oRow.Item("child"), 0) = 1 Then
        '            Dim IdColumnName As String = dataModelCode & "_" & oRow.Item("object_name").ToString() & "_id"
        '            sXPath = "//" & oRow.Item("object_name").ToString.ToUpper & "[@" & IdColumnName.ToUpper & "=" & oRow.Item(IdColumnName).ToString & "]"
        '        Else
        '            sXPath = "//" & oRow.Item("object_name").ToString()
        '        End If

        '        documentCode = RTrim(oRow.Item("DocumentCode").ToString)

        '        Dim nodes As XPathNodeIterator = navigator.Select(sXPath)

        '        If nodes.MoveNext = True Then

        '            sName = oRow.Item("property_name").ToString.ToUpper.Trim

        '            nodes.Current.MoveToAttribute("OI", "")
        '            navigator.MoveToId(nodes.Current.Value)

        '            sChildNode = "<" & sName & " CODE=""" & documentCode & """/>"
        '            navigator.AppendChild(sChildNode)

        '        End If

        '    Next

        '    navigator.MoveToRoot()

        '    xmlDataSet = navigator.UnderlyingObject.outerxml.ToString

        'End If




        'Dim dt As DataTable
        'Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Standard_Wording_Values")
        '    cmd.AddInParameter("@gis_datamodel_code", SqlDbType.VarChar, 30).Value = DataModelCode
        '    cmd.AddInParameter("@gis_policy_binder_id", SqlDbType.Int).Value = PolicyBinderID
        '    dt = con.ExecuteDataTable(cmd)
        'End Using

        'If dt IsNot Nothing Then
        '    If dt.Rows.Count = 0 Then
        '        oSWProps = Nothing
        '    Else
        '        oSWProps = dt.Rows
        '    End If
        'End If

        'If oSWProps Is Nothing = False Then

        '    Dim DocumentCode As String = ""
        '    Dim ChildObject As Integer = 0
        '    Dim document As New XmlDocument

        '    document.LoadXml(XMLDataset$)

        '    Dim navigator As XPathNavigator = document.CreateNavigator()

        '    navigator.MoveToRoot()

        '    For Each oRow As DataRow In oSWProps
        '        '//GENERAL[@QBENZ_GENERAL_ID="700133539"]
        '        If Cast.DefaultIfNull(oRow.Item("child"), 0) = 1 Then
        '            Dim IdColumnName As String = DataModelCode & "_" & oRow.Item("object_name").ToString() & "_id"
        '            sXPath = "//" & oRow.Item("object_name").ToString.ToUpper & "[@" & IdColumnName.ToUpper & "=" & oRow.Item(IdColumnName).ToString & "]"
        '        Else
        '            sXPath = "//" & oRow.Item("object_name").ToString()
        '        End If

        '        DocumentCode = RTrim(oRow.Item("DocumentCode").ToString)

        '        Dim nodes As XPathNodeIterator = navigator.Select(sXPath)

        '        If nodes.MoveNext = True Then

        '            sName = oRow.Item("property_name").ToString.ToUpper.Trim

        '            nodes.Current.MoveToAttribute("OI", "")
        '            navigator.MoveToId(nodes.Current.Value)

        '            sChildNode = "<" & sName & " CODE=""" & DocumentCode & """/>"
        '            navigator.AppendChild(sChildNode)

        '        End If

        '    Next

        '    navigator.MoveToRoot()

        '    XMLDataset = navigator.UnderlyingObject.outerxml.ToString

        'End If


    End Sub

    Private Sub IncludeAddressElements( _
    ByRef xmlDataset As String, _
    ByVal dataModelCode As String)

        Dim sXPath As String = GetAddressXPathAttributes(dataModelCode)

        If Not String.IsNullOrEmpty(sXPath) Then

            Dim document As XmlDocument = New XmlDocument
            document.Load(New StringReader(xmlDataset))
            Dim navigator As XPathNavigator = document.CreateNavigator()
            Dim nodes As XPathNodeIterator = navigator.Select(sXPath)

            While nodes.MoveNext

                Dim sName As String = String.Empty
                Dim addressCnt As Integer = 0
                Dim addressLine1 As String = String.Empty
                Dim addressLine2 As String = String.Empty
                Dim addressLine3 As String = String.Empty
                Dim addressLine4 As String = String.Empty
                Dim postCode As String = String.Empty
                Dim countryCode As String = String.Empty

                sName = nodes.Current.Name
                addressCnt = nodes.Current.ValueAsInt

                nodes.Current.MoveToParent()
                nodes.Current.MoveToAttribute("OI", "")
                navigator.MoveToId(nodes.Current.Value)

                ' if get address returns a valid address
                If GetAddress(addressCnt, addressLine1, addressLine2, addressLine3, addressLine4, postCode, countryCode) Then

                    ' create the address element
                    navigator.AppendChildElement(navigator.Prefix, sName, navigator.LookupNamespace(navigator.Prefix), "")
                    navigator.MoveToChild(sName, navigator.LookupNamespace(navigator.Prefix))

                    ' append address node to dataset
                    Dim Attributes As XmlWriter = navigator.CreateAttributes
                    Attributes.WriteAttributeString("US", "0")
                    Attributes.WriteAttributeString("ADDRESS_LINE1", addressLine1)
                    Attributes.WriteAttributeString("ADDRESS_LINE2", addressLine2)
                    Attributes.WriteAttributeString("ADDRESS_LINE3", addressLine3)
                    Attributes.WriteAttributeString("ADDRESS_LINE4", addressLine4)
                    Attributes.WriteAttributeString("POSTCODE", postCode)
                    Attributes.WriteAttributeString("COUNTRYCODE", countryCode)
                    Attributes.Close()

                End If

            End While

            navigator.MoveToRoot()

            xmlDataset = CType(navigator.UnderlyingObject, XmlDocument).OuterXml.ToString()

        End If

    End Sub

    Friend Function TransformDatasetPBToRelease( _
    ByVal xmlDataset As String, _
    ByRef errorElement As String) As String

        Dim sName As String = String.Empty
        Dim iValue As Integer = 0

        If xmlDataset <> "" Then

            Dim xmldoc As New XmlDocument
            xmldoc.LoadXml(xmlDataset)

            Dim DataModelCode As String = xmldoc.SelectSingleNode("DATA_SET").Attributes("DataModelCode").Value
            Dim PolicyLinkID As Integer = Util.ToSafeInt(xmldoc.SelectSingleNode("DATA_SET").Item("RISK_OBJECTS").Item(DataModelCode.ToUpper & "_POLICY_BINDER").GetAttribute("GIS_POLICY_LINK_ID", ""), -1)
            Dim PolicyBinderID As Integer = Util.ToSafeInt(xmldoc.SelectSingleNode("DATA_SET").Item("RISK_OBJECTS").Item(DataModelCode.ToUpper & "_POLICY_BINDER").GetAttribute(DataModelCode.ToUpper & "_POLICY_BINDER_ID", ""), 0)

            If DataModelCode = String.Empty Then
                errorElement += "TransformDatasetPBtoSAM Failed. Failed to retrieve the DataModelCode attribute from the DATA_SET Element in the XMLDataset." + vbCr
            ElseIf PolicyLinkID = -1 Then
                errorElement += "TransformDatasetPBtoSAM Failed. Failed to retrieve the GIS_POLICY_LINK_ID attribute from the " + DataModelCode.ToUpper + "_POLICY_BINDER Element in the XMLDataset." + vbCr
            End If

            ' Include Any Address Elements
            IncludeAddressElements(xmlDataset, DataModelCode)

            ' Include Any Sum Insured Elements
            IncludeSumInsuredElements(xmlDataset, DataModelCode, PolicyLinkID)

            ' standard wordings cannot currently be included on claims 
            ' so this code has been left incomplete

        End If

        Return xmlDataset

    End Function

    Private Function GetAddress( _
    ByVal addressCnt As Integer, _
    ByRef addressLine1 As String, _
    ByRef addressLine2 As String, _
    ByRef addressLine3 As String, _
    ByRef addressLine4 As String, _
    ByRef postCode As String, _
    ByRef countryCode As String) As Boolean

        Dim dbReturnCode As Integer
        Dim noOfRecords As Integer
        Dim resultArray As Object = Nothing
        Dim bReturnValue As Boolean = False

        ' address_cnt
        AddParameterLite(m_oDatabase, "address_cnt", addressCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)

        ' execute sql select
        dbReturnCode = m_oDatabase.SQLSelect("spe_Address_sel", "spe_Address_sel", True, noOfRecords, resultArray)
        If dbReturnCode <> PMEReturnCode.PMTrue Then
            Throw New Exception("spu_SAM_Get_Dataset_Address_Properties failed")
        End If

        If resultArray IsNot Nothing And noOfRecords <> 0 Then

            ' get results
            Dim address As Object(,) = DirectCast(resultArray, Object(,))

            If address IsNot Nothing And noOfRecords <> 0 Then

                addressLine1 = address(GetAddressColumns.Address1, 0).ToString().Trim()
                addressLine2 = address(GetAddressColumns.Address2, 0).ToString().Trim()
                addressLine3 = address(GetAddressColumns.Address3, 0).ToString().Trim()
                addressLine4 = address(GetAddressColumns.Address4, 0).ToString().Trim()
                postCode = address(GetAddressColumns.PostalCode, 0).ToString().Trim()
                countryCode = address(GetAddressColumns.CountryCode, 0).ToString().Trim()

            End If

            bReturnValue = True

        End If

        Return bReturnValue

    End Function

    Public Overrides Sub CleanUpInterops()

        ' clean up the database interop
        '        DestroyCOMInterop(CObj(m_oDatabase), True)
        m_oDatabase = Nothing

    End Sub

#End Region

End Class








