Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.IO
Imports System.Xml.Serialization
Imports Aspose.Cells
Imports Sirius.SBO.Import.Excel_Import_Library
Imports SharedFiles
Imports System.Configuration


Friend NotInheritable Class Claim_BDX_Import : Inherits ImportBase

#Region "Private variables"

    Private m_sImportedPath As String = String.Empty
    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer
    Private m_sBatchStatus As String

#End Region

#Region "Public Properties"
    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "BDXCLM"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Bordereaux Claim Import"
        End Get
    End Property

    Public ReadOnly Property ImportedPath() As String
        Get
            ' If we haven't got the path yet, get it
            If (m_sImportedPath.Length = 0) Then
                m_sImportedPath = GetSystemOption(ACImportedPathOption)
            End If

            ' Check it exists
            If Not Directory.Exists(m_sImportedPath) Then
                ' If we can create it do so, else raise error
                Directory.CreateDirectory(m_sImportedPath)
            End If

            ' If we made it this far return the path
            Return m_sImportedPath
        End Get
    End Property

    Private userNameField As String = String.Empty
    Private passwordField As String = String.Empty

    Public Property UserName() As String
        Get
            Return userNameField
        End Get
        Set(ByVal value As String)
            userNameField = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return passwordField
        End Get
        Set(ByVal value As String)
            passwordField = value
        End Set
    End Property

    Public Property ClaimPaymentAmount() As Integer = 0

    Public Property ProcessRecoveryOnlyWhenPaymentMade() As Boolean = False

    ''' <summary>
    ''' Specifies the number of records in batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfTotalRecords() As Integer
        Get
            Return m_nNoOfTotalRecords
        End Get
        Set(ByVal value As Integer)
            m_nNoOfTotalRecords = value
        End Set
    End Property

    ''' <summary>
    ''' Specifies the no of rejected records in the batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfRejections() As Integer
        Get
            Return m_nNoOfRejections
        End Get
        Set(ByVal value As Integer)
            m_nNoOfRejections = value
        End Set
    End Property

#End Region

#Region "Methods"

    Protected Overrides Sub ProcessElement()

    End Sub

    ' Prepare the Request structure for the serialisation.  This means it's easier to assign values if the elements are already there.
    Private Sub BuildClaimProcessRequest(ByRef ClaimProcessRequest As ProcessClaimCommand, ByRef ClaimProcessResponse As ProcessClaimCommandResponse, ByRef riskCnt As Integer)

        ClaimProcessRequest.Claim = New BaseClaimProcessType

    End Sub

    ' Process the risk data.  This injects the valus from the spreadsheet into the Request XML structure using XPath
    Private Sub ProcessRiskData(ByRef xDoc As Xml.XmlDocument, ByVal xpath As String, ByVal value As String, ByVal datatype As String, ByRef listOfErrors As listOfErrorsType, ByRef valueIsValid As Boolean)

        Dim xNode As XmlNode
        Dim xmlValue As String

        If value IsNot Nothing Then

            Dim nsManager As New XmlNamespaceManager(xDoc.NameTable)
            nsManager.AddNamespace("bt", "http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")

            ' Convert to a safe date format
            '    If datatype = "Date" Then
            'Dim dt As Date = Convert.ToDateTime(value)
            'xmlValue = XmlConvert.ToString(dt, Xml.XmlDateTimeSerializationMode.Local)
            'Else
            xmlValue = value.ToString
            'End If

            ' If this is not a product builder field then just set the value
            If xpath.StartsWith("[PB]") = False Then

                xNode = GetXNode(xDoc, xpath, nsManager)
                xNode.InnerText = xmlValue

            Else

                ' Get the XPath excluding the "[PB]" prefix
                xpath = xpath.Substring(4)

                Dim navigator As XPath.XPathNavigator = xDoc.CreateNavigator()

                ' Validate the XPath value
                Try
                    Dim xpathExp As XPath.XPathExpression = navigator.Compile(xpath)
                Catch ex As XPath.XPathException
                    listOfErrors.Add("The PB xPath value """ & xpath & """ is not a valid xPath query")
                    valueIsValid = False
                    Exit Sub
                End Try

                ' Get the correct Risk to which to add this product builder value
                Dim pbXpath As String = "/ProcessClaimCommand/bt:Claim"
                xNode = GetXNode(xDoc, pbXpath, nsManager)

                Dim pbdNode As XmlNode = xDoc.CreateNode(XmlNodeType.Element, "bt", "ClaimBuilderDetail", "http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")

                ' Set the ItemName
                Dim pbNode As XmlNode = xDoc.CreateNode(XmlNodeType.Element, "bt", "ClaimBuilderData", "http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")
                If pbNode.Attributes("ItemName") Is Nothing Then
                    pbNode.Attributes.Append(xDoc.CreateAttribute("ItemName"))
                End If
                pbNode.Attributes("ItemName").Value = xpath

                ' Set the Value
                If pbNode.Attributes("Value") Is Nothing Then
                    pbNode.Attributes.Append(xDoc.CreateAttribute("Value"))
                End If
                pbNode.Attributes("Value").Value = xmlValue

                ' Append the Child
                pbdNode.AppendChild(pbNode)
                xNode.AppendChild(pbdNode)

            End If
        End If
    End Sub

    ''' <summary>
    ''' This runs the custom Policy SQL as specified in the Configuration file
    ''' </summary>
    ''' <param name="xePolicyMatchingElement"></param>
    ''' <param name="oClaimProcessRequest"></param>
    ''' <param name="worksheet"></param>
    ''' <param name="nRowCnt"></param>
    ''' <param name="oListOfErrors"></param>
    ''' <param name="sCertRef"></param>
    ''' <remarks></remarks>
    Private Sub LookupPolicy(ByVal xePolicyMatchingElement As Xml.XmlElement, ByRef oClaimProcessRequest As ProcessClaimCommand, ByVal worksheet As Worksheet, ByVal nRowCnt As Integer, ByVal oListOfErrors As listOfErrorsType, ByVal sCertRef As String)

        ' Retireve the SQL Element
        Static sOriginalSqlScript As String = String.Empty
        Static sSqlScriptLocation As String = xePolicyMatchingElement.GetElementsByTagName("SQL").Item(0).InnerText.ToString

        If xePolicyMatchingElement Is Nothing OrElse worksheet Is Nothing Then
            Return
        End If

        ' If we don't already have it read in the SQL file
        If sOriginalSqlScript = String.Empty Then
            sSqlScriptLocation = xePolicyMatchingElement.GetElementsByTagName("SQL").Item(0).InnerText.ToString
            If Dir(sSqlScriptLocation) = String.Empty Then
                oListOfErrors.Add("The PolicyMatching SQL file was not found.  Please check the path in the configuration file")
                Return
            Else
                Dim sr As StreamReader = File.OpenText(sSqlScriptLocation)
                sOriginalSqlScript = sr.ReadToEnd()
            End If
        End If

        ' Get all of the parameters defined in the configuration file
        Dim xeParametersElement As XmlElement = xePolicyMatchingElement.GetElementsByTagName("Parameters").Item(0)

        ' Setup the parameter array for the string format.  The number of parameters in the SQL has to 
        ' match the number of parameters in the condif.
        Dim oSqlParamArray(xeParametersElement.GetElementsByTagName("Parameter").Count - 1) As Object

        ' For each parameter in the config go and fetch the value out of the spreadsheet
        For Each parameter As Xml.XmlElement In xePolicyMatchingElement.Item("Parameters").GetElementsByTagName("Parameter")

            Dim sThisCellName As String = parameter.Attributes(STR_SourceValue).Value
            Dim nThisCellPosition As Integer = Convert.ToInt32(parameter.Attributes(STR_Position).Value)
            Dim thisCell As Cell
            Dim oThisRowCellValue As Object = 2

            If sThisCellName.StartsWith("$") Then
                sThisCellName = sThisCellName.Substring(1).Trim & (nRowCnt).ToString
                thisCell = worksheet.Cells(sThisCellName)
                If thisCell IsNot Nothing Then
                    oThisRowCellValue = thisCell.Value
                End If
            End If

            If oThisRowCellValue IsNot Nothing Then
                ' Check the validity of the data item before passing it into the SQL
                If ValidateAndConvertDatatype(parameter, oThisRowCellValue, sThisCellName, oListOfErrors, sThisCellName, nRowCnt) = False Then
                Else
                    If nThisCellPosition <= oSqlParamArray.GetUpperBound(0) Then
                        ' Add the value into the correct location in the string paramarry
                        oSqlParamArray(nThisCellPosition) = oThisRowCellValue
                    End If
                End If
            End If

        Next

        ' Use string format to to set the parameters in the SQL string
        Dim sSqlScript As String = [String].Format(sOriginalSqlScript, oSqlParamArray)

        Dim oResultObject(,) As Object = Nothing
        ' Execute sql
        Dim nReturn As Integer = m_oDatabase.SQLSelect(sSqlScript, "BDX Lookup Policy", False, vResultArray:=oResultObject)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'BDX Lookup Policy' with the following SQL - " & sSqlScript)
        End If

        If IsArray(oResultObject) Then
            Dim resultArray As System.Array = oResultObject
            If resultArray.Rank = 2 AndAlso (resultArray.GetUpperBound(0) = 2 OrElse resultArray.GetUpperBound(0) = 3) Then
                ' If we have an error then 
                If resultArray(2, 0).ToString <> String.Empty Then
                    oListOfErrors.Add(resultArray(2, 0).ToString, "", nRowCnt)
                Else
                    oClaimProcessRequest.Claim.InsuranceFileKey = resultArray(0, 0)
                    oClaimProcessRequest.Claim.BaseClaimKey = resultArray(1, 0)
                    If resultArray.Length = 4 Then
                        ClaimPaymentAmount = resultArray(3, 0)
                    End If
                End If
            Else
                oListOfErrors.Add("The results returned from the Policy matching SQL did not return the correct number of items in the select list.  Please review the SQL contained in the file - " & xePolicyMatchingElement.GetElementsByTagName("SQL").Item(0).InnerText.ToString)
            End If
        End If

    End Sub

    ' This runs the custom Risk SQL as specified in the Configuration file
    Private Sub LookupRisk(ByVal riskMatchingElement As Xml.XmlElement, ByRef ClaimProcessRequest As ProcessClaimCommand, ByVal worksheet As Worksheet, ByVal rowCnt As Integer, ByVal riskCnt As Integer, ByVal listOfErrors As listOfErrorsType, ByVal certRef As String)

        Static originalSqlScript As String = String.Empty

        If riskMatchingElement Is Nothing OrElse worksheet Is Nothing Then
            Return
        End If

        ' If we don't already have it read in the SQL file
        If originalSqlScript = String.Empty Then
            Dim sqlScriptLocation As String = riskMatchingElement.GetElementsByTagName("SQL").Item(0).InnerText.ToString
            If Dir(sqlScriptLocation) = String.Empty Then
                listOfErrors.Add("The RiskMatching SQL file was not found.  Please check the path in the configuration file")
                Return
            Else
                Dim sr As StreamReader = File.OpenText(sqlScriptLocation)
                originalSqlScript = sr.ReadToEnd()
            End If
        End If

        ' Get all of the parameters defined in the configuration file
        Dim parametersElement As XmlElement = riskMatchingElement.GetElementsByTagName("Parameters").Item(0)

        ' Setup the parameter array for the string format.  The number of parameters in the SQL has to 
        ' match the number of parameters in the condif.
        Dim sqlParamArray(parametersElement.GetElementsByTagName("Parameter").Count - 1) As Object

        ' For each parameter in the config go and fetch the value out of the spreadsheet
        For Each parameter As Xml.XmlElement In riskMatchingElement.Item("Parameters").GetElementsByTagName("Parameter")

            Dim thisCellName As String = parameter.Attributes(STR_SourceValue).Value
            Dim thisCellPosition As Integer = Convert.ToInt32(parameter.Attributes(STR_Position).Value)
            Dim thisCell As Cell
            Dim thisRowCellValue As Object = 2

            If thisCellName.StartsWith("$") Then
                thisCellName = thisCellName.Substring(1).Trim & (rowCnt).ToString
                thisCell = worksheet.Cells(thisCellName)
                If thisCell IsNot Nothing Then
                    thisRowCellValue = thisCell.Value
                End If
            End If

            If thisRowCellValue IsNot Nothing Then
                ' Check the validity of the data item before passing it into the SQL
                If ValidateAndConvertDatatype(parameter, thisRowCellValue, thisCellName, listOfErrors, thisCellName, rowCnt) = False Then
                Else
                    If thisCellPosition <= sqlParamArray.GetUpperBound(0) Then
                        ' Add the value into the correct location in the string paramarry
                        sqlParamArray(thisCellPosition) = thisRowCellValue
                    End If
                End If
            End If

        Next

        ' Use string format to to set the parameters in the SQL string
        Dim sqlScript As String = [String].Format(originalSqlScript, sqlParamArray)

        Dim resultObject(,) As Object = Nothing
        ' Execute sql
        Dim iReturn As Integer = m_oDatabase.SQLSelect(sqlScript, "BDX Lookup Risk", False, vResultArray:=resultObject)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'BDX Lookup Risk' with the following SQL - " & sqlScript)
        End If

        If IsArray(resultObject) Then
            Dim resultArray As System.Array = resultObject
            If resultArray.Rank = 2 AndAlso resultArray.GetUpperBound(0) = 1 Then
                If resultArray(1, 0).ToString <> String.Empty Then
                    listOfErrors.Add(resultArray(1, 0).ToString, "", rowCnt)
                Else
                    ClaimProcessRequest.Claim.RiskKey = resultArray(0, 0)
                End If
            Else
                listOfErrors.Add("The results returned from the Risk matching SQL did not return the correct number of items in the select list.  Please review the SQL contained in the file - " & riskMatchingElement.GetElementsByTagName("SQL").Item(0).InnerText.ToString)
            End If
        End If

    End Sub
    ''' <summary>
    ''' Process Import
    ''' </summary>
    ''' <param name="ConfigFile"></param>
    ''' <param name="WorkbookFile"></param>
    ''' <remarks></remarks>
    Friend Sub ProcessImport(ByVal ConfigFile As FileInfo, ByVal WorkbookFile As FileInfo)

        Dim fsFileStream As FileStream = Nothing
        Dim xDocxXmlDoc As New Xml.XmlDocument
        Dim errorWorkbook As Workbook
        Dim sErrorWorkbookFilename As String = String.Empty
        Dim sBrokerCode As String = String.Empty
        Dim crTotalAmount As Decimal = 0
        Dim nTotalTransactions As Integer = 0
        Dim crRejectAmount As Decimal = 0
        Dim nRejectTransactions As Integer = 0
        ' Define the REST API client
        OutputLine("Creating REST API client...")
        Dim _apiClient As New ApiClient(ConfigurationManager.AppSettings("RestAPIUrl"))
        Dim sProcessedFolderName As String = ""

        Dim sMatchKey As String
        Dim sMatchKeyColumns As String = ""
        Dim sFailureMsg As String = ""

        Try
            OutputLine("Validating XML mapping...")
            m_oXML.Validate()

            OutputLine("Creating Batch...")
            CreateBatch()

            xDocxXmlDoc.Load(ConfigFile.FullName)

            'Creating a file stream containing the Excel file to be opened
            fsFileStream = New FileStream(WorkbookFile.FullName, FileMode.Open)

            'Instantiate a Workbook object that represents the existing Excel file
            Dim workbook As Workbook = New Workbook(fsFileStream)

            ' Retrieve the Import Header Element
            Dim workbookElement As Xml.XmlElement = xDocxXmlDoc.Item(STR_ImportHeader)

            sBrokerCode = workbookElement.Attributes(STR_Broker_Code).Value

            If workbookElement.Attributes(sProcessedFolderName) IsNot Nothing Then
                sProcessedFolderName = workbookElement.Attributes(sProcessedFolderName).Value
            End If

            OutputLine("Processing data file...")
            ' Loop around the WorkSheet elements in the Configuration File
            For Each worksheetElement As Xml.XmlElement In workbookElement.ChildNodes
                Dim errorworksheet As Worksheet
                Dim worksheet As Worksheet
                ' Check the name of the child element identifies it as a WorkSheet
                If worksheetElement.Name = STR_Worksheet Then

                    ' Get the Worksheet name
                    Dim worksheetName As String = worksheetElement.Attributes(STR_Name).Value

                    Dim license As License = New License()

                    'Set the license of Aspose.Cells to avoid the evaluation limitations
                    license.SetLicense("Aspose.Total.lic")

                    'Instantiate a new Workbook object that represents the existing Excel file
                    worksheet = workbook.Worksheets(worksheetName)

                    ' Create an Error file ready to report any errors that may occur
                    sErrorWorkbookFilename = WorkbookFile.Directory.FullName & "\" & WorkbookFile.Name.Replace(WorkbookFile.Extension, " - Errors(" & Now.ToFileTime & ")" & WorkbookFile.Extension)
                    errorWorkbook = New Workbook
                    errorworksheet = errorWorkbook.Worksheets(0)
                    errorworksheet.Name = worksheetName

                    ' Grab the child Elemenets for this worksheet from the configuration file
                    Dim xeMappingElement As Xml.XmlElement = worksheetElement.Item("Mapping")
                    Dim xeRowMappingElement As Xml.XmlElement = worksheetElement.Item("RowMapping")
                    Dim xePolicyMappingElement As Xml.XmlElement = worksheetElement.Item("PolicyMatching")
                    Dim xeRiskMappingElement As Xml.XmlElement = worksheetElement.Item("RiskMatching")
                    Dim nStartRow As Integer = CInt(xeMappingElement.Attributes("StartingRow").Value)
                    Dim sClaimRefColumn As String = ToSafeString(xeMappingElement.Attributes(STR_Claim_Ref_Column).Value)
                    Dim sTPANameColumn As String = ToSafeString(xeMappingElement.Attributes(STR_TPAname_Column).Value)
                    Dim sCoverHolderColumn As String = ToSafeString(xeMappingElement.Attributes(STR_CoverHoldername_Column).Value)
                    If sClaimRefColumn.Trim() <> "" Then
                        sClaimRefColumn = sClaimRefColumn.Substring(1).Trim()
                    End If
                    If sCoverHolderColumn.Trim() <> "" Then
                        sCoverHolderColumn = sCoverHolderColumn.Substring(1).Trim()
                    End If

                    Dim nTPAPartyKey As Integer = 0
                    Dim bTPASettleDirectly As Boolean = False

                    Dim bValueIsValid As Boolean = True

                    ' Deserialize the XML from the implementation resultdataset into 
                    ' the correct messaginging format
                    Dim oGlobalListOfErrors As New listOfErrorsType

                    ' Copy across the Header Rows to the Error Workbook
                    Dim errorCells As Cells = errorWorkbook.Worksheets(worksheetName).Cells
                    If nStartRow > 1 Then
                        Try
                            errorCells.CopyRows(worksheet.Cells, 0, 0, nStartRow - 1)
                        Catch ex As Exception
                            ' I cant get rid of the exception triggered by the CopyRows method but the row still
                            ' gets copied so for now we'll just ignore the exception
                        End Try
                    End If

                    If ValidateAgent(sBrokerCode, oGlobalListOfErrors) = False Then
                        OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, 0, oGlobalListOfErrors, "Claim_BDX_Import", 0, sBrokerCode)
                    ElseIf ValidateRiskData(xeMappingElement, oGlobalListOfErrors) = False Then
                        OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, 0, oGlobalListOfErrors, "Claim_BDX_Import", 0, sBrokerCode)
                    Else

                        ' Validate the header structure
                        If (HeaderStructureIsValid(worksheetElement, worksheet, oGlobalListOfErrors) = False) Then
                            OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, 0, oGlobalListOfErrors, "Claim_BDX_Import", 0, sBrokerCode)
                        Else

                            ' Setup the new Request structure
                            Dim ClaimProcessRequest As New ProcessClaimCommand
                            Dim ClaimProcessResponse As ProcessClaimCommandResponse = Nothing
                            Dim oClaimPeril() As BaseClaimProcessPerilType = Nothing
                            Dim nRiskCnt As Integer = 0
                            Dim bIsMaintainClm As Boolean = False
                            Dim oPayee As BaseClaimPayeeType
                            Dim oReceiptPayee As BaseClaimReceiptPayeeType
                            BuildClaimProcessRequest(ClaimProcessRequest, ClaimProcessResponse, nRiskCnt)

                            ' For each Row in the worksheet
                            For nCnt As Integer = nStartRow To worksheet.Cells.MaxDataRow + 1
                                sMatchKey = ""
                                GetMatchingRowKey(xeRowMappingElement, worksheet, nCnt, sMatchKey, sMatchKeyColumns)

                                If sMatchKey <> "" Then
                                    sFailureMsg = " Failure record as per matching columns - " & sMatchKeyColumns & " and values - " & sMatchKey & " respectively"
                                End If
                                Dim oListOfErrors As New listOfErrorsType

                                Dim xmlSerializer As New Serialization.XmlSerializer(GetType(ProcessClaimCommand))
                                Dim nsManager As New Serialization.XmlSerializerNamespaces
                                nsManager.Add("bt", "http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")

                                Dim sw As New IO.StringWriter()

                                ' Serialize the Class into XML
                                xmlSerializer.Serialize(sw, ClaimProcessRequest, nsManager)
                                xmlSerializer = Nothing
                                Dim ClaimProcessRequestXML As New Xml.XmlDocument
                                ClaimProcessRequestXML.LoadXml(sw.ToString())

                                Dim ClaimProcessRequestXMLNew As New Xml.XmlDocument
                                ClaimProcessRequestXMLNew.LoadXml(sw.ToString())

                                Dim oxmlnode As Xml.XmlNode
                                oxmlnode = ClaimProcessRequestXMLNew.SelectSingleNode("/ProcessClaimCommand")
                                If oxmlnode.HasChildNodes Then
                                    oxmlnode.RemoveAll()
                                End If

                                bValueIsValid = True
                                nTotalTransactions = nTotalTransactions + 1

                                ' Get the Certificate ref from the Cell.
                                Dim sClaimRef As String = String.Empty
                                If sClaimRefColumn.Trim() <> "" Then
                                    sClaimRef = worksheet.Cells(String.Format("{0}{1}", sClaimRefColumn, nCnt.ToString)).Value
                                End If
                                Dim sCoverHolderRef As String = String.Empty
                                If sCoverHolderColumn.Trim() <> "" Then
                                    sCoverHolderRef = worksheet.Cells(String.Format("{0}{1}", sCoverHolderColumn, nCnt.ToString)).Value
                                End If
                                Dim sTPAName As String = ""
                                Dim sLocation As String = Nothing
                                If sTPANameColumn.StartsWith("$") Then
                                    sTPAName = worksheet.Cells(sTPANameColumn.Substring(1) & nCnt.ToString).Value
                                Else
                                    sTPAName = sTPANameColumn
                                End If
                                ' Check that this certificate hasn't already resulted in an error
                                If oGlobalListOfErrors.Exists(AddressOf New certFinderPredicate(sMatchKey).Match) = True Then
                                    oListOfErrors.Add("An error on earlier Row matches as per configured columns - " & sMatchKeyColumns & " and values - " & sMatchKey & " respectively occured, therefore this row is being rejected", sClaimRefColumn, nCnt)
                                    bValueIsValid = False
                                ElseIf ValidateTPA(sTPAName, nTPAPartyKey, bTPASettleDirectly, oGlobalListOfErrors) = False Then
                                    OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, 0, oGlobalListOfErrors, "Claim_BDX_Import", 0, sTPAName, sCoverHolderRef)
                                Else

                                    Dim iPerilCount As Integer = 0
                                    Dim xnsmNsMgr As New Xml.XmlNamespaceManager(xDocxXmlDoc.NameTable)
                                    xnsmNsMgr.AddNamespace("ab", "http://www.siriusfs.com/SFI/Import/Claim_BDX_Import/20051005")

                                    Dim oCellValue As Object = Nothing
                                    Dim sDatatype As String = String.Empty

                                    Dim sTaxCode As String = String.Empty

                                    ' Loop around each field in the configuration mapping 
                                    For Each xeField As Xml.XmlElement In xeMappingElement.GetElementsByTagName("Field")
                                        If xeField.Attributes("Datatype").Value <> "ClaimPeril" Then
                                            oCellValue = Nothing
                                            sDatatype = String.Empty

                                            ' Extract the value from the spreadsheet
                                            ExtractAndValidateValue(xnsmNsMgr, worksheet, nCnt, oListOfErrors, xeField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)

                                            If xeField.Attributes("Datatype").Value = "Tax" Then
                                                For Each item As Xml.XmlElement In xeField.GetElementsByTagName("Tax")
                                                    If item.Attributes("Type").Value = "Risk" Then
                                                        Dim nCount As Integer = 0
                                                        For Each TaxItem As Xml.XmlElement In item.GetElementsByTagName("TaxItem")
                                                            Dim thisCellName As String = ""
                                                            thisCellName = TaxItem.Attributes("TaxGroupCode").Value

                                                            If GetCellvalue(thisCellName, worksheet, nCnt) = String.Empty Then
                                                                sTaxCode = thisCellName
                                                            Else
                                                                sTaxCode = GetCellvalue(thisCellName, worksheet, nCnt)
                                                            End If
                                                            nCount += 1
                                                        Next
                                                    End If
                                                Next

                                                If iPerilCount > 0 AndAlso oClaimPeril IsNot Nothing AndAlso oClaimPeril(iPerilCount - 1) IsNot Nothing AndAlso oClaimPeril(iPerilCount - 1).Reserve IsNot Nothing Then
                                                    For Each reserve As BaseClaimProcessPerilReserveType In oClaimPeril(iPerilCount - 1).Reserve
                                                        reserve.TaxGroupCode = sTaxCode
                                                    Next
                                                End If

                                            Else
                                                If (oCellValue IsNot Nothing) AndAlso (Not String.IsNullOrEmpty(oCellValue)) AndAlso (bValueIsValid = True) Then
                                                    If (xeField.Attributes("Description") IsNot Nothing) AndAlso (xeField.Attributes("Description").Value = "Is Maintain Claim") Then
                                                        bIsMaintainClm = Convert.ToBoolean(oCellValue)
                                                    End If
                                                    If (xeField.Attributes("Datatype") IsNot Nothing AndAlso xeField.Attributes("Datatype").Value.ToUpper <> "PAYEE") Then
                                                        ' Process the value and map into the internal structure
                                                        ProcessRiskData(ClaimProcessRequestXMLNew, [String].Format(xeField.Attributes("DestMapping").Value, nRiskCnt + 1), oCellValue, sDatatype, oListOfErrors, bValueIsValid)
                                                    End If
                                                End If

                                            End If
                                        Else

                                            oCellValue = Nothing
                                            sDatatype = String.Empty

                                            ' Extract the value from the spreadsheet
                                            ExtractAndValidateValue(xnsmNsMgr, worksheet, nCnt, oListOfErrors, xeField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)

                                            ReDim Preserve oClaimPeril(iPerilCount)
                                            oClaimPeril(iPerilCount) = New BaseClaimProcessPerilType
                                            oClaimPeril(iPerilCount).TypeCode = oCellValue
                                            ProcessClaimPeril(xeField, oClaimPeril(iPerilCount), worksheet, nCnt, oListOfErrors, sClaimRef)

                                            If ProcessRecoveryOnlyWhenPaymentMade AndAlso (oClaimPeril(iPerilCount).Recovery(0).RecoveryAmount = 0 OrElse oClaimPeril(iPerilCount).Reserve(0).PaymentAmount > 0) Then
                                                ProcessRecoveryOnlyWhenPaymentMade = False
                                            End If

                                            If oListOfErrors.Count > 0 Then
                                                bValueIsValid = False
                                                Exit For
                                            End If
                                            iPerilCount += 1

                                        End If
                                        If xeField.Attributes("Description").Value.ToUpper = "LOCATION" Then
                                            sLocation = oCellValue
                                        End If
                                    Next

                                End If

                                Dim sString As String = ""
                                sString = ClaimProcessRequestXMLNew.InnerXml.ToString.Replace("bt:", "")
                                ClaimProcessRequestXML.LoadXml(sString)

                                If bValueIsValid = False Then
                                    ' Output the error row
                                    OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, nCnt, oListOfErrors, "Claim_BDX_Import", 0, sTPAName, sCoverHolderRef)
                                    oGlobalListOfErrors.Add(sFailureMsg, sClaimRefColumn, nCnt, sMatchKey)
                                    nRejectTransactions = nRejectTransactions + 1
                                Else

                                    ' Does the next row in the spreadsheet match this policy?

                                    xmlSerializer = New Serialization.XmlSerializer(GetType(ProcessClaimCommand))
                                    Dim oXMLContext As New XmlParserContext(Nothing, Nothing, Nothing, XmlSpace.None)
                                    Dim oXMLReader As New XmlTextReader(ClaimProcessRequestXML.OuterXml, XmlNodeType.Document, oXMLContext)

                                    ' Deserialize the XML back into a Class
                                    ClaimProcessRequest = Nothing
                                    ClaimProcessRequest = xmlSerializer.Deserialize(oXMLReader)
                                    xmlSerializer = Nothing
                                    Dim oxmlnodes As XmlNodeList = ClaimProcessRequestXML.SelectNodes("/ProcessClaimCommand/Claim/ClaimBuilderDetail")
                                    Dim oClaimBuilderDetail As BaseClaimProcessBuilderRiskType() = Nothing
                                    Dim iCount As Integer = 0
                                    For Each xnXmno As XmlNode In oxmlnodes

                                        If xnXmno.HasChildNodes Then
                                            ReDim Preserve oClaimBuilderDetail(iCount)
                                            oClaimBuilderDetail(iCount) = New BaseClaimProcessBuilderRiskType
                                            Dim oClaimBuilderData As BaseClaimProcessBuilderRiskTypeClaimBuilderData = Nothing
                                            For Each oChildNode As XmlNode In xnXmno.ChildNodes
                                                oClaimBuilderData = New BaseClaimProcessBuilderRiskTypeClaimBuilderData
                                                oClaimBuilderData.ItemName = oChildNode.Attributes("ItemName").Value
                                                oClaimBuilderData.Value = oChildNode.Attributes("Value").Value
                                            Next
                                            oClaimBuilderDetail(iCount).ClaimBuilderData = oClaimBuilderData
                                            iCount = iCount + 1
                                        End If
                                    Next

                                    If oClaimBuilderDetail IsNot Nothing Then
                                        ClaimProcessRequest.Claim.ClaimBuilderDetail = New List(Of BaseClaimProcessBuilderRiskType)(oClaimBuilderDetail)
                                    End If


                                    ' Use the custom XML to match the policy record
                                    LookupPolicy(xePolicyMappingElement, ClaimProcessRequest, worksheet, nCnt, oListOfErrors, sClaimRef)
                                    If ProcessRecoveryOnlyWhenPaymentMade Then
                                        If ClaimPaymentAmount <= 0 Then
                                            oListOfErrors.Add("claim recovery receipt (CLR) transaction can not be processed if there have been no claim payments on claim - " & sClaimRef, sClaimRefColumn, nCnt, sClaimRef)
                                        End If
                                    End If
                                    If oListOfErrors.Count > 0 Then
                                        ' Output the error row
                                        OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, nCnt, oListOfErrors, "Claim_BDX_Import", 0, sTPAName, sCoverHolderRef)
                                        oGlobalListOfErrors.Add(sFailureMsg, sClaimRefColumn, nCnt, sMatchKey)
                                        nRejectTransactions = nRejectTransactions + 1

                                    Else

                                        ' Use the custom XML to match the risk record
                                        LookupRisk(xeRiskMappingElement, ClaimProcessRequest, worksheet, nCnt, nRiskCnt, oListOfErrors, sClaimRef)

                                        oPayee = New BaseClaimPayeeType
                                        ProcessPayeeDetails(xeMappingElement, oPayee, worksheet, nCnt, oListOfErrors)

                                        If oListOfErrors.Count > 0 Then
                                            OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, nCnt, oListOfErrors, "Claim_BDX_Import", 0, sTPAName, sCoverHolderRef)
                                            oGlobalListOfErrors.Add(sFailureMsg, sClaimRefColumn, nCnt, sMatchKey)
                                            nRejectTransactions = nRejectTransactions + 1
                                        Else
                                            oReceiptPayee = New BaseClaimReceiptPayeeType
                                            ProcessReceiptPayeeDetails(xeMappingElement, oReceiptPayee, worksheet, nCnt, oListOfErrors)

                                            If oListOfErrors.Count > 0 Then
                                                OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, nCnt, oListOfErrors, "Claim_BDX_Import", 0, sTPAName, sCoverHolderRef)
                                                oGlobalListOfErrors.Add("Failure recorded for Claim Ref - " & sClaimRef, sClaimRefColumn, nCnt, sClaimRef)
                                                nRejectTransactions = nRejectTransactions + 1
                                            Else

                                                Try
                                                    ClaimProcessRequest.Claim.TPA = nTPAPartyKey
                                                    ClaimProcessRequest.Claim.IsTPASettleDirectly = bTPASettleDirectly
                                                    If oClaimPeril IsNot Nothing Then
                                                        ClaimProcessRequest.Claim.ClaimPeril = New List(Of BaseClaimProcessPerilType)(oClaimPeril)
                                                    Else
                                                        ClaimProcessRequest.Claim.ClaimPeril = New List(Of BaseClaimProcessPerilType)
                                                    End If
                                                    ClaimProcessRequest.Claim.Payee = oPayee
                                                    ClaimProcessRequest.Claim.ReceiptPayee = oReceiptPayee
                                                    ClaimProcessRequest.Claim.IgnoreWarnings = True
                                                    ClaimProcessRequest.Claim.Location = sLocation
                                                    ClaimProcessRequest.IsMaintainClaim = bIsMaintainClm

                                                    'Send True, SAM checks the system option
                                                    ClaimProcessRequest.ExclusiveLock = True
                                                    ClaimProcessRequest.SessionValue = "CLAIMBDXIMPORT"
                                                    ClaimProcessRequest.LoginUserName = UserName

                                                    ClaimProcessResponse = _apiClient.PostAsync(Of ProcessClaimCommandResponse)("/claims/processClaim", ClaimProcessRequest).Result

                                                    If ClaimProcessResponse.Errors IsNot Nothing AndAlso ClaimProcessResponse.Errors.Count > 0 Then
                                                        For Each stsError As SAMErrors In ClaimProcessResponse.Errors
                                                            oListOfErrors.Add("The import of Row " & nCnt & "  failed. The error is '" & stsError.SAMErrorMessage & "'")
                                                        Next
                                                        OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, nCnt, oListOfErrors, "Claim_BDX_Import", 0, sTPAName, sCoverHolderRef)
                                                        oGlobalListOfErrors.Add(sFailureMsg, sClaimRefColumn, nCnt, sMatchKey)
                                                        nRejectTransactions = nRejectTransactions + 1
                                                    End If

                                                Catch ex As Exception

                                                    If ClaimProcessResponse IsNot Nothing AndAlso ClaimProcessResponse.Errors IsNot Nothing AndAlso ClaimProcessResponse.Errors.Count > 0 Then
                                                        For Each stsError As SAMErrors In ClaimProcessResponse.Errors
                                                            oListOfErrors.Add("The import of Row " & nCnt & "  failed. The error is '" & stsError.SAMErrorMessage & "'")
                                                        Next
                                                    Else
                                                        oListOfErrors.Add("The import of Row " & nCnt & "  failed. " & ex.Message)
                                                    End If

                                                    OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, nCnt, oListOfErrors, "Claim_BDX_Import", 0, sTPAName, sCoverHolderRef)
                                                    oGlobalListOfErrors.Add(sFailureMsg, sClaimRefColumn, nCnt, sMatchKey)
                                                    nRejectTransactions = nRejectTransactions + 1

                                                End Try

                                                ' Reset the request object
                                                ClaimProcessRequest = New ProcessClaimCommand
                                                ClaimProcessResponse = Nothing
                                                nRiskCnt = 0
                                                oPayee = Nothing
                                                BuildClaimProcessRequest(ClaimProcessRequest, ClaimProcessResponse, nRiskCnt)

                                            End If
                                        End If
                                    End If
                                End If
                                ClaimProcessRequestXML = Nothing
                                ClaimProcessRequestXMLNew = Nothing
                            Next
                        End If
                    End If
                End If

                errorworksheet = Nothing
                errorWorkbook = Nothing
            Next
            m_sBatchStatus = kBatchStatusComplete
            'Closing the file streams to free all resources
            If nRejectTransactions > 0 Then
                OutputLine("Process claim bordereaux completed. Total request processed - " & nTotalTransactions & ", Failed - " & nRejectTransactions & " See out file for details.")
            Else
                OutputLine("Process claim bordereaux completed. Total request processed - " & nTotalTransactions & " See out file for details.")
            End If
            fsFileStream.Close()

        Catch NotExcelFileEx As Aspose.Cells.CellsException
            m_sBatchStatus = kBatchStatusFailed
            ' Create a work manager task to indicate this file failed
            CreateWorkManagerTask(String.Format("Claim BDX Import failed for the import file {0}.  The file was not recognised as a valid Excel Spreadsheet.", WorkbookFile.FullName))
        Catch ex As Exception
            m_sBatchStatus = kBatchStatusFailed
            OutputError(ex)
            Throw New Exception("Unable to add new book.", ex)
        Finally

            If fsFileStream IsNot Nothing Then
                fsFileStream.Close()
                fsFileStream = Nothing
            End If
            xDocxXmlDoc = Nothing

            ' Output the files to a specific Broker folder
            Dim processedFolder As String = String.Empty
            If sProcessedFolderName <> String.Empty Then
                processedFolder = ImportedPath & "\" & sProcessedFolderName
                If Directory.Exists(processedFolder) = False Then
                    Directory.CreateDirectory(processedFolder)
                End If
            ElseIf sBrokerCode <> String.Empty Then
                processedFolder = ImportedPath & "\" & sBrokerCode
                If Directory.Exists(processedFolder) = False Then
                    Directory.CreateDirectory(processedFolder)
                End If
            Else
                processedFolder = ImportedPath
            End If

            ' If the Error file exists then 
            If sErrorWorkbookFilename <> String.Empty AndAlso IO.File.Exists(sErrorWorkbookFilename) Then
                Dim processedFilename As String = String.Empty
                Try
                    Dim targetFileName As String = WorkbookFile.Name.Replace(WorkbookFile.Extension, " - Errors(" & Now.ToFileTime & ")" & WorkbookFile.Extension)
                    processedFilename = processedFolder & "\" & targetFileName

                    ' Move file to processed directory
                    File.Move(sErrorWorkbookFilename, processedFilename)
                    If CloudHostingEnabled Then
                        Using fileStream As Stream = New FileStream(processedFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                            Dim fileBytes(0 To fileStream.Length - 1) As Byte
                            fileStream.Read(fileBytes, 0, fileBytes.Length)

                            CloudRepository.UploadFile(Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudProcessedFolder & processedFilename.Substring(ConfiguredImportedPath.Length).Replace("\", "/"),
                                                       fileBytes)
                        End Using
                    End If
                    ' Create a work manager task to indicate this file failed
                    CreateWorkManagerTask(String.Format("Claim BDX Import failed for the import file {0}.  Please see the Policy BDX error report for further details", WorkbookFile.FullName))

                Catch ex As System.IO.IOException
                    ' Create a work manager task to indicate this file failed
                    CreateWorkManagerTask(String.Format("Claim BDX Import Failed but unable to move error file from {0} to {1}.", sErrorWorkbookFilename, processedFilename))
                End Try
            End If

            ' Move the original file across tp processed
            If WorkbookFile.Name <> String.Empty Then
                Dim processedFilename As String = String.Empty
                Try
                    Dim targetFileName As String = WorkbookFile.Name.Replace(WorkbookFile.Extension, " - (" & Now.ToFileTime & ")" & WorkbookFile.Extension)

                    processedFilename = processedFolder & "\" & targetFileName

                    ' Move file to processed directory
                    File.Move(WorkbookFile.FullName, processedFilename)

                    If CloudHostingEnabled Then
                        Dim result As Integer
                        Using fileStream As Stream = New FileStream(processedFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                            Dim fileBytes(0 To fileStream.Length - 1) As Byte
                            fileStream.Read(fileBytes, 0, fileBytes.Length)

                            result = CloudRepository.UploadFile(Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudProcessedFolder & processedFilename.Substring(ConfiguredImportedPath.Length).Replace("\", "/"),
                                                                fileBytes).Result
                        End Using

                        If result = gPMConstants.PMEReturnCode.PMTrue Then
                            Dim s3ImportFile As String = Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudImportFolder & WorkbookFile.FullName.Substring(ConfiguredImportPath.Length).Replace("\", "/")
                            CloudRepository.DeleteFile(s3ImportFile)
                        End If
                    End If
                Catch ex As System.IO.IOException
                    ' Create a work manager task to indicate this file failed
                    CreateWorkManagerTask(String.Format("Claim BDX Import Failed but unable to move error file from {0} to {1}.", WorkbookFile.FullName, processedFilename))
                End Try
            End If

            ' Take off the rejected transactions
            'nTotalTransactions = nTotalTransactions - nRejectTransactions

            ' Update the batch 
            UpdateBatch(BatchStatusCode:=m_sBatchStatus, TotalAmount:=crTotalAmount, TotalTransactions:=nTotalTransactions, RejectAmount:=crRejectAmount, RejectTransactions:=nRejectTransactions, ImportFilename:=WorkbookFile.FullName)
        End Try
    End Sub

    Private Sub ProcessPayeeDetails(ByVal xField As Xml.XmlElement, ByRef oPayee As BaseClaimPayeeType, ByVal worksheet As Worksheet, ByVal rowCnt As Integer, ByRef listOfErrors As listOfErrorsType)
        Dim bAddressPresent As String = False
        Dim oPayeeAddress As New BaseAddressType
        For Each field As Xml.XmlElement In xField.GetElementsByTagName("Field")
            If field.Attributes("Datatype").Value = "Payee" Then
                For Each item As Xml.XmlElement In xField.GetElementsByTagName("PayeeDetails")
                    Dim thisCellName As String = ""
                    thisCellName = item.Attributes("SourceValue").Value
                    Select Case item.Attributes("Name").Value
                        Case "BankCode"
                            oPayee.BankCode = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "BankName"
                            oPayee.BankName = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "BankNumber"
                            oPayee.BankNumber = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "MediaReference"
                            oPayee.MediaReference = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "MediaTypeCode"
                            oPayee.MediaTypeCode = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "Name"
                            oPayee.Name = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "TheirReference"
                            oPayee.TheirReference = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "Comments"
                            oPayee.Comments = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "PartyBankKey"
                            oPayee.PartyBankKey = ToSafeInteger(GetCellvalue(thisCellName, worksheet, rowCnt), 0)

                        Case "AddressLine1"
                            oPayeeAddress.AddressLine1 = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "AddressLine2"
                            oPayeeAddress.AddressLine2 = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "AddressLine3"
                            oPayeeAddress.AddressLine3 = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "AddressLine4"
                            oPayeeAddress.AddressLine4 = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "PostCode"
                            oPayeeAddress.PostCode = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "CountryCode"
                            oPayeeAddress.CountryCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                    End Select
                Next
            End If
        Next
        If Not String.IsNullOrEmpty(oPayeeAddress.CountryCode) Then
            oPayeeAddress.AddressTypeCode = AddressTypeType.Item3131XCO
            oPayee.Address = oPayeeAddress
        End If
    End Sub

    Private Function ValidateRiskData(ByVal mappingElement As Xml.XmlElement, ByVal listOfErrors As listOfErrorsType) As Boolean
        Dim XPath As String
        Dim DataModelCode As String
        Dim FieldName As String
        Dim ObjectName As String
        Dim vArray() As String
        Dim datatype As String
        Dim Description As String

        ValidateRiskData = True

        For Each field As Xml.XmlElement In mappingElement.GetElementsByTagName("Field")
            XPath = field.Attributes("DestMapping").Value
            datatype = field.Attributes("Datatype").Value
            Description = field.Attributes("Description").Value
            If XPath.StartsWith("[PB]") Then
                vArray = XPath.Split("/")
                For i As Integer = 0 To vArray.GetUpperBound(0)
                    If vArray(i).Contains("POLICY_BINDER") Then
                        DataModelCode = Mid(vArray(i), 1, InStr(vArray(i), "_POLICY_BINDER", CompareMethod.Text))
                        Exit For
                    End If
                Next
                FieldName = vArray(vArray.GetUpperBound(0)).Substring(1)
                ObjectName = DataModelCode & vArray(vArray.GetUpperBound(0) - 1).Substring(0)
                If CheckDataType(ObjectName, FieldName, datatype, Description, listOfErrors, field) = False Then
                    ValidateRiskData = False
                End If
            End If
        Next
    End Function

    Private Function CheckDataType(ByVal Objectname As String, ByVal FieldName As String, ByVal datatype As String, ByVal Description As String, ByVal listOfErrors As listOfErrorsType, ByVal field As XmlElement) As Boolean
        Dim sqlScript As String
        Dim resultObject(,) As Object

        CheckDataType = True

        If InStr(Objectname, "[", CompareMethod.Text) > 0 Then
            Objectname = Mid(Objectname, 1, InStr(Objectname, "[", CompareMethod.Text) - 1)
        End If
        sqlScript = "SELECT data_type FROM GIS_Object obj INNER JOIN GIS_Property prop " &
                    "ON obj.gis_object_id=prop.gis_object_id " &
                    "WHERE obj.table_name='" & Objectname & "' AND prop.property_name='" & FieldName & "'"

        Dim iReturn As Integer = m_oDatabase.SQLSelect(sqlScript, "CheckDataType", False, vResultArray:=resultObject)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'CheckDataType' with the following SQL - " & sqlScript)
        End If

        If IsArray(resultObject) Then
            Select Case resultObject(0, 0)
                Case 1 'Date in DB
                    If datatype <> "Date" Then
                        CheckDataType = False
                        listOfErrors.Add("""" & Description & """" & " is defined as a " & datatype & " in the mapping but is a Date in the Database. ")
                    End If
                Case 5 'String in DB
                    If datatype <> "String" And datatype <> "List" And datatype <> "Percent" And datatype <> "Lookup" Then
                        CheckDataType = False
                        listOfErrors.Add("""" & Description & """" & " is defined as a " & datatype & " in the mapping but is a String in the Database. ")
                    End If
                Case 2 'Integer in DB
                    If datatype <> "Integer" And datatype <> "List" And datatype <> "Lookup" Then
                        CheckDataType = False
                        listOfErrors.Add("""" & Description & """" & " is defined as a " & datatype & " in the mapping but is a Integer in the Database. ")
                    ElseIf datatype = "List" Then
                        If field.Item("Validation") IsNot Nothing AndAlso field.Item("Validation").Item("List") IsNot Nothing Then
                            For Each item As Xml.XmlElement In field.Item("Validation").Item("List").GetElementsByTagName("Item")
                                If IsNumeric(item.GetAttribute("DestValue")) = False Then
                                    CheckDataType = False
                                    listOfErrors.Add("""" & Description & """" & "'s  DestValue " & item.GetAttribute("DestValue") & " is invalid because is Integer in the Database.")
                                    Exit Function
                                End If
                            Next
                        End If
                    End If
                Case 20 'Boolean in DB
                    If datatype <> "Integer" And datatype <> "List" And datatype <> "Lookup" Then
                        CheckDataType = False
                        listOfErrors.Add("""" & Description & """" & " is defined as a " & datatype & " in the mapping but is an Integer in the Database. ")
                    ElseIf datatype = "List" Then
                        If field.Item("Validation") IsNot Nothing AndAlso field.Item("Validation").Item("List") IsNot Nothing Then
                            For Each item As Xml.XmlElement In field.Item("Validation").Item("List").GetElementsByTagName("Item")
                                If Not (item.GetAttribute("DestValue") = "0" Or item.GetAttribute("DestValue") = "1") Then
                                    CheckDataType = False
                                    listOfErrors.Add("""" & Description & """" & "'s  DestValue " & item.GetAttribute("DestValue") & " is invalid because is Integer in the Database.")
                                    Exit Function
                                End If
                            Next
                        End If
                    End If
                Case 21 'Currency in DB
                    If datatype <> "Currency" And datatype <> "Lookup" Then
                        CheckDataType = False
                        listOfErrors.Add("""" & Description & """" & " is defined as a " & datatype & " in the mapping but is a Currency in the Database. ")
                    End If
                Case 22 'Percent in DB
                    If datatype <> "Percent" And datatype <> "List" And datatype <> "Lookup" Then
                        CheckDataType = False
                        listOfErrors.Add("""" & Description & """" & " is defined as a " & datatype & " in the mapping but is a Percent in the Database. ")
                    ElseIf datatype = "List" Then
                        If field.Item("Validation") IsNot Nothing AndAlso field.Item("Validation").Item("List") IsNot Nothing Then
                            For Each item As Xml.XmlElement In field.Item("Validation").Item("List").GetElementsByTagName("Item")
                                If IsNumeric(item.GetAttribute("DestValue")) = False Then
                                    CheckDataType = False
                                    listOfErrors.Add("""" & Description & """" & "'s  DestValue " & item.GetAttribute("DestValue") & " is invalid because is Percent in the Database.")
                                    Exit Function
                                End If
                            Next
                        End If
                    End If
            End Select
        Else
            CheckDataType = False
            listOfErrors.Add("Either """ & FieldName & " or " & Objectname & """ could not be found in the Database for """ & Description & """.")
        End If
    End Function

    ''' <summary>
    ''' Process Claim Peril
    ''' </summary>
    ''' <param name="xField"></param>
    ''' <param name="oClaimPeril"></param>
    ''' <param name="worksheet"></param>
    ''' <param name="rowCnt"></param>
    ''' <param name="listOfErrors"></param>
    ''' <remarks></remarks>
    Private Sub ProcessClaimPeril(ByVal xField As Xml.XmlElement, ByRef oClaimPeril As BaseClaimProcessPerilType, ByVal worksheet As Worksheet, ByVal rowCnt As Integer, ByRef listOfErrors As listOfErrorsType, ByVal ClaimNumber As String)

        Dim reservesElement As Xml.XmlElement = xField.Item("Reserves")
        Dim PaymentDetailsElement As Xml.XmlElement = xField.Item("PaymentDetails")
        Dim oReserve() As BaseClaimProcessPerilReserveType
        Dim PartyType As String = ""
        Dim PartyCode As String = ""
        Dim iReserveCount As Integer = 0
        Dim ReserveTotal As Decimal = 0
        Dim PaymentTotal As Decimal = 0
        Dim RecoveryPartyCode As String = ""
        Dim RecoveryPartyTypeCode As String = ""
        For Each item As Xml.XmlElement In reservesElement.GetElementsByTagName("ReserveItem")
            Dim thisCellName As String = ""
            Dim bIsReserveToDate As Boolean
            Dim bIsPaidToDate As Boolean
            Dim sThisCellEquation As String = ""
            ReDim Preserve oReserve(iReserveCount)

            oReserve(iReserveCount) = New BaseClaimProcessPerilReserveType

            ReserveTotal = 0
            PaymentTotal = 0

            If item.Attributes(STR_Equation) IsNot Nothing AndAlso String.IsNullOrEmpty(item.Attributes(STR_Equation).Value) = False Then
                sThisCellEquation = item.Attributes(STR_Equation).Value
                bIsReserveToDate = False ' To be looked into again
            Else
                thisCellName = item.Attributes(STR_IsReserveToDate).Value
                bIsReserveToDate = GetCellvalue(thisCellName, worksheet, rowCnt)
            End If

            thisCellName = item.Attributes(STR_IsPaidToDate).Value
            bIsPaidToDate = GetCellvalue(thisCellName, worksheet, rowCnt)

            For Each subitem As Xml.XmlElement In item.GetElementsByTagName("ReserveSubItem")
                Dim sReserveSubItemEquation As String = ""
                thisCellName = subitem.Attributes(STR_ReserveTypeCode).Value
                oReserve(iReserveCount).TypeCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                If subitem.Attributes(STR_Equation) IsNot Nothing AndAlso String.IsNullOrEmpty(subitem.Attributes(STR_Equation).Value) = False Then
                    sReserveSubItemEquation = subitem.Attributes(STR_Equation).Value
                    bIsReserveToDate = False
                Else
                    thisCellName = item.Attributes(STR_IsReserveToDate).Value
                    oReserve(iReserveCount).IsReserveToDate = bIsReserveToDate
                End If

                If String.IsNullOrEmpty(sReserveSubItemEquation) = False Then
                    oReserve(iReserveCount).Amount = ToSafeDecimal(GetEquationValue(sReserveSubItemEquation, worksheet, rowCnt))
                Else
                    thisCellName = subitem.Attributes(STR_RevisionAmount).Value
                    oReserve(iReserveCount).Amount = ToSafeDecimal(GetCellvalue(thisCellName, worksheet, rowCnt), 0)
                End If

                ReserveTotal += oReserve(iReserveCount).Amount

                thisCellName = subitem.Attributes(STR_PaymentAmount).Value

                oReserve(iReserveCount).PaymentAmount = ToSafeDecimal(GetCellvalue(thisCellName, worksheet, rowCnt), 0)
                If oReserve(iReserveCount).PaymentAmount <> 0 Then
                    oReserve(iReserveCount).PaymentAmountSpecified = True
                    PaymentTotal += oReserve(iReserveCount).PaymentAmount
                End If
                oReserve(iReserveCount).IsPaidToDate = bIsPaidToDate
                oReserve(iReserveCount).PaymentDetails = New BaseClaimProcessPaymentDetailsType
                thisCellName = PaymentDetailsElement.Attributes(STR_PaymentBankCode).Value
                oReserve(iReserveCount).PaymentDetails.PaymentBankCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                thisCellName = PaymentDetailsElement.Attributes(STR_PaymentMediaTypeCode).Value
                oReserve(iReserveCount).PaymentDetails.PaymentMediaTypeCode = GetCellvalue(thisCellName, worksheet, rowCnt)

                thisCellName = PaymentDetailsElement.Attributes(STR_PaymentCurrencyCode).Value
                oReserve(iReserveCount).PaymentDetails.PaymentCurrencyCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                thisCellName = PaymentDetailsElement.Attributes(STR_PaymentPartyType).Value
                PartyType = UCase(GetCellvalue(thisCellName, worksheet, rowCnt))
                If PartyType = "CLIENT" Then
                    oReserve(iReserveCount).PaymentDetails.PaymentPartyType = ClaimPaymentPartyTypeType.CLIENT
                ElseIf PartyType = "AGENT" Then
                    oReserve(iReserveCount).PaymentDetails.PaymentPartyType = ClaimPaymentPartyTypeType.AGENT
                ElseIf PartyType = "TPA" Then
                    oReserve(iReserveCount).PaymentDetails.PaymentPartyType = ClaimPaymentPartyTypeType.PARTY
                ElseIf PartyType = "CLMPAYABLE" Then
                    oReserve(iReserveCount).PaymentDetails.PaymentPartyType = ClaimPaymentPartyTypeType.CLMPAYABLE
                ElseIf PartyType = "OTHER" Then
                    oReserve(iReserveCount).PaymentDetails.PaymentPartyType = ClaimPaymentPartyTypeType.PARTY
                End If

                thisCellName = PaymentDetailsElement.Attributes(STR_PaymentPartyCode).Value
                PartyCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                If PartyType = "OTHER" Or PartyType = "TPA" Then
                    If Not String.IsNullOrEmpty(PartyCode) Then
                        ValidateTPA(PartyCode, oReserve(iReserveCount).PaymentDetails.PaymentPartyKey, False, listOfErrors)
                    End If
                End If

                iReserveCount += 1
                ReDim Preserve oReserve(iReserveCount)
                oReserve(iReserveCount) = New BaseClaimProcessPerilReserveType

            Next

            thisCellName = item.Attributes(STR_ReserveTypeCode).Value
            oReserve(iReserveCount).TypeCode = GetCellvalue(thisCellName, worksheet, rowCnt)
            thisCellName = item.Attributes(STR_RevisionAmount).Value

            If String.IsNullOrEmpty(sThisCellEquation) = False Then
                oReserve(iReserveCount).Amount = ToSafeDecimal(GetEquationValue(sThisCellEquation, worksheet, rowCnt)) - ReserveTotal
            Else
                oReserve(iReserveCount).Amount = ToSafeDecimal(GetCellvalue(thisCellName, worksheet, rowCnt)) - ReserveTotal
            End If

            thisCellName = item.Attributes(STR_PaymentAmount).Value
            oReserve(iReserveCount).PaymentAmount = Math.Round(ToSafeDecimal(GetCellvalue(thisCellName, worksheet, rowCnt)) - PaymentTotal, 2)

            oReserve(iReserveCount).IsReserveToDate = bIsReserveToDate

            If oReserve(iReserveCount).PaymentAmount <> 0 Then
                oReserve(iReserveCount).PaymentAmountSpecified = True

                oReserve(iReserveCount).IsPaidToDate = bIsPaidToDate
                oReserve(iReserveCount).PaymentDetails = New BaseClaimProcessPaymentDetailsType
                thisCellName = PaymentDetailsElement.Attributes(STR_PaymentBankCode).Value
                oReserve(iReserveCount).PaymentDetails.PaymentBankCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                thisCellName = PaymentDetailsElement.Attributes(STR_PaymentMediaTypeCode).Value
                oReserve(iReserveCount).PaymentDetails.PaymentMediaTypeCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                thisCellName = PaymentDetailsElement.Attributes(STR_PaymentCurrencyCode).Value
                oReserve(iReserveCount).PaymentDetails.PaymentCurrencyCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                thisCellName = PaymentDetailsElement.Attributes(STR_PaymentPartyType).Value
                PartyType = UCase(GetCellvalue(thisCellName, worksheet, rowCnt))
                If PartyType = "CLIENT" Then
                    oReserve(iReserveCount).PaymentDetails.PaymentPartyType = ClaimPaymentPartyTypeType.CLIENT
                ElseIf PartyType = "AGENT" Then
                    oReserve(iReserveCount).PaymentDetails.PaymentPartyType = ClaimPaymentPartyTypeType.AGENT
                ElseIf PartyType = "TPA" Then
                    oReserve(iReserveCount).PaymentDetails.PaymentPartyType = ClaimPaymentPartyTypeType.PARTY
                ElseIf PartyType = "CLMPAYABLE" Then
                    oReserve(iReserveCount).PaymentDetails.PaymentPartyType = ClaimPaymentPartyTypeType.CLMPAYABLE
                ElseIf PartyType = "OTHER" Then
                    oReserve(iReserveCount).PaymentDetails.PaymentPartyType = ClaimPaymentPartyTypeType.PARTY
                End If
                thisCellName = PaymentDetailsElement.Attributes(STR_PaymentPartyCode).Value
                PartyCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                If PartyType = "OTHER" Or PartyType = "TPA" Then
                    If Not String.IsNullOrEmpty(PartyCode) Then
                        ValidateTPA(PartyCode, oReserve(iReserveCount).PaymentDetails.PaymentPartyKey, False, listOfErrors)
                    End If
                End If
            End If
            ValidateReserveAmount(oReserve(iReserveCount).TypeCode, listOfErrors, ClaimNumber, oClaimPeril.TypeCode, oReserve(iReserveCount).Amount)

            iReserveCount += 1
        Next

        If oReserve IsNot Nothing Then
            oClaimPeril.Reserve = New List(Of BaseClaimProcessPerilReserveType)(oReserve)
        Else
            oClaimPeril.Reserve = New List(Of BaseClaimProcessPerilReserveType)
        End If
        Dim RecoveryElement As Xml.XmlElement = xField.Item("Recovery")
        Dim ReceiptDetailsElement As Xml.XmlElement = xField.Item("ReceiptDetails")
        Dim oRecovery() As BaseClaimProcessPerilRecoveryType
        Dim iRecoveryCount As Integer = 0
        Dim RecoveryTotal As Decimal = 0
        Dim ReceiptTotal As Decimal = 0
        If Not RecoveryElement Is Nothing Then
            For Each item As Xml.XmlElement In RecoveryElement.GetElementsByTagName("RecoveryItem")
                Dim thisCellName As String = ""
                Dim bRecoverToDate As Boolean
                Dim bIsReceiptToDate As Boolean
                Dim sThisCellEquation As String = ""
                ReDim Preserve oRecovery(iRecoveryCount)
                oRecovery(iRecoveryCount) = New BaseClaimProcessPerilRecoveryType
                RecoveryTotal = 0
                ReceiptTotal = 0

                If item.Attributes(STR_Equation) IsNot Nothing AndAlso String.IsNullOrEmpty(item.Attributes(STR_Equation).Value) = False Then
                    sThisCellEquation = item.Attributes(STR_Equation).Value
                    bRecoverToDate = False ' To be looked into again
                Else
                    thisCellName = item.Attributes(STR_IsRecoverToDate).Value
                    bRecoverToDate = GetCellvalue(thisCellName, worksheet, rowCnt)
                End If

                thisCellName = item.Attributes(STR_IsReceiptToDate).Value
                bIsReceiptToDate = GetCellvalue(thisCellName, worksheet, rowCnt)

                For Each subitem As Xml.XmlElement In item.GetElementsByTagName("RecoverySubItem")
                    Dim sRecoverySubItemEquation As String = ""
                    thisCellName = subitem.Attributes(STR_RecoveryTypeCode).Value
                    oRecovery(iRecoveryCount).TypeCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                    If subitem.Attributes(STR_Equation) IsNot Nothing AndAlso String.IsNullOrEmpty(subitem.Attributes(STR_Equation).Value) = False Then
                        sRecoverySubItemEquation = subitem.Attributes(STR_Equation).Value
                        bRecoverToDate = False
                    Else
                        thisCellName = item.Attributes(STR_IsRecoverToDate).Value
                        oRecovery(iRecoveryCount).isRecoverToDate = bRecoverToDate
                    End If

                    If String.IsNullOrEmpty(sRecoverySubItemEquation) = False Then
                        oRecovery(iRecoveryCount).Amount = ToSafeDecimal(GetEquationValue(sRecoverySubItemEquation, worksheet, rowCnt))
                    Else
                        thisCellName = subitem.Attributes(STR_RevisionAmount).Value
                        oRecovery(iRecoveryCount).Amount = ToSafeDecimal(GetCellvalue(thisCellName, worksheet, rowCnt), 0)
                    End If
                    RecoveryTotal += oRecovery(iRecoveryCount).Amount
                    thisCellName = subitem.Attributes(STR_ReceiptAmount).Value
                    oRecovery(iRecoveryCount).RecoveryAmount = ToSafeDecimal(GetCellvalue(thisCellName, worksheet, rowCnt), 0)
                    If oRecovery(iRecoveryCount).RecoveryAmount <> 0 Then
                        oRecovery(iRecoveryCount).RecoveryAmountSpecified = True
                        ReceiptTotal += oRecovery(iRecoveryCount).RecoveryAmount
                    End If
                    oRecovery(iRecoveryCount).isReceiptToDate = bIsReceiptToDate
                    oRecovery(iRecoveryCount).RecoveryDetails = New BaseClaimProcessReceiptDetailsType
                    thisCellName = ReceiptDetailsElement.Attributes(STR_ReceiptBankCode).Value
                    oRecovery(iRecoveryCount).RecoveryDetails.ReceiptBankCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                    thisCellName = ReceiptDetailsElement.Attributes(STR_ReceiptMediaTypeCode).Value
                    oRecovery(iRecoveryCount).RecoveryDetails.ReceiptMediaTypeCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                    thisCellName = ReceiptDetailsElement.Attributes(STR_ReceiptCurrencyCode).Value
                    oRecovery(iRecoveryCount).RecoveryDetails.ReceiptCurrencyCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                    thisCellName = ReceiptDetailsElement.Attributes(STR_ProcessRecoveryOnlyWhenPaymentMade).Value
                    ProcessRecoveryOnlyWhenPaymentMade = GetCellvalue(thisCellName, worksheet, rowCnt)
                    thisCellName = ReceiptDetailsElement.Attributes(STR_ReceiptPartyType).Value
                    PartyType = UCase(GetCellvalue(thisCellName, worksheet, rowCnt))

                    If PartyType = "CLIENT" Then
                        oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyType = ClaimReceiptPartyTypeType.CLIENT
                    ElseIf PartyType = "AGENT" Then
                        oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyType = ClaimReceiptPartyTypeType.AGENT
                    ElseIf PartyType = "TPA" Then
                        oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyType = ClaimReceiptPartyTypeType.PARTY
                    ElseIf PartyType = "CLMRECEIVABLE" Then
                        oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyType = ClaimReceiptPartyTypeType.CLMRECEIVABLE
                    ElseIf PartyType = "OTHER" Then
                        oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyType = ClaimReceiptPartyTypeType.PARTY
                    End If

                    thisCellName = ReceiptDetailsElement.Attributes(STR_ReceiptPartyCode).Value
                    PartyCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                    If PartyType = "OTHER" Or PartyType = "TPA" Then
                        If Not String.IsNullOrEmpty(PartyCode) Then
                            Dim bDummy As Boolean = False
                            ValidateTPA(PartyCode, oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyKey, False, listOfErrors)
                            ValidateTPA(PartyCode, oRecovery(iRecoveryCount).RecoveryPartyKey, bDummy, listOfErrors)
                        End If
                    ElseIf PartyType = "AGENT" OrElse PartyType = "CLIENT" Then
                        ValidatePartyType(PartyCode, oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyKey, listOfErrors)
                        ValidatePartyType(PartyCode, oRecovery(iRecoveryCount).RecoveryPartyKey, listOfErrors)
                    End If

                    iRecoveryCount += 1
                    ReDim Preserve oRecovery(iRecoveryCount)
                    oRecovery(iRecoveryCount) = New BaseClaimProcessPerilRecoveryType
                Next

                thisCellName = item.Attributes(STR_RecoveryPartyCode).Value
                RecoveryPartyCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                ValidatePartyType(RecoveryPartyCode, oRecovery(iRecoveryCount).RecoveryPartyKey, listOfErrors)

                thisCellName = item.Attributes(STR_RecoveryPartyTypeCode).Value
                RecoveryPartyTypeCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                ValidateRecoveryPartyType(RecoveryPartyTypeCode, oRecovery(iRecoveryCount).RecoveryPartyTypeKey, listOfErrors)

                thisCellName = item.Attributes(STR_RecoveryTypeCode).Value
                oRecovery(iRecoveryCount).TypeCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                thisCellName = item.Attributes(STR_IsSalvageRecoveryCode).Value
                oRecovery(iRecoveryCount).IsSalvageRecovery = GetCellvalue(thisCellName, worksheet, rowCnt)
                thisCellName = item.Attributes(STR_RevisionAmount).Value

                If String.IsNullOrEmpty(sThisCellEquation) = False Then
                    oRecovery(iRecoveryCount).Amount = ToSafeDecimal(GetEquationValue(sThisCellEquation, worksheet, rowCnt)) - RecoveryTotal
                Else
                    oRecovery(iRecoveryCount).Amount = ToSafeDecimal(GetCellvalue(thisCellName, worksheet, rowCnt)) - RecoveryTotal
                End If

                thisCellName = item.Attributes(STR_ReceiptAmount).Value
                oRecovery(iRecoveryCount).RecoveryAmount = ToSafeDecimal(GetCellvalue(thisCellName, worksheet, rowCnt)) - ReceiptTotal
                oRecovery(iRecoveryCount).isRecoverToDate = bRecoverToDate
                If oRecovery(iRecoveryCount).RecoveryAmount <> 0 Then
                    oRecovery(iRecoveryCount).RecoveryAmountSpecified = True

                    oRecovery(iRecoveryCount).isReceiptToDate = bIsReceiptToDate
                    oRecovery(iRecoveryCount).RecoveryDetails = New BaseClaimProcessReceiptDetailsType
                    thisCellName = ReceiptDetailsElement.Attributes(STR_ReceiptBankCode).Value
                    oRecovery(iRecoveryCount).RecoveryDetails.ReceiptBankCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                    thisCellName = ReceiptDetailsElement.Attributes(STR_ReceiptMediaTypeCode).Value
                    oRecovery(iRecoveryCount).RecoveryDetails.ReceiptMediaTypeCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                    thisCellName = ReceiptDetailsElement.Attributes(STR_ReceiptCurrencyCode).Value
                    oRecovery(iRecoveryCount).RecoveryDetails.ReceiptCurrencyCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                    thisCellName = ReceiptDetailsElement.Attributes(STR_ProcessRecoveryOnlyWhenPaymentMade).Value
                    ProcessRecoveryOnlyWhenPaymentMade = GetCellvalue(thisCellName, worksheet, rowCnt)
                    thisCellName = ReceiptDetailsElement.Attributes(STR_ReceiptPartyType).Value
                    PartyType = UCase(GetCellvalue(thisCellName, worksheet, rowCnt))
                    If PartyType = "CLIENT" Then
                        oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyType = ClaimReceiptPartyTypeType.CLIENT
                    ElseIf PartyType = "AGENT" Then
                        oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyType = ClaimReceiptPartyTypeType.AGENT
                    ElseIf PartyType = "TPA" Then
                        oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyType = ClaimReceiptPartyTypeType.PARTY
                    ElseIf PartyType = "CLMRECEIVABLE" Then
                        oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyType = ClaimReceiptPartyTypeType.CLMRECEIVABLE
                    ElseIf PartyType = "OTHER" Then
                        oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyType = ClaimReceiptPartyTypeType.PARTY
                    End If

                    thisCellName = ReceiptDetailsElement.Attributes(STR_ReceiptPartyCode).Value
                    PartyCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                    If PartyType = "OTHER" Or PartyType = "TPA" Then
                        If Not String.IsNullOrEmpty(PartyCode) Then
                            Dim bDummy As Boolean = False
                            ValidateTPA(PartyCode, oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyKey, False, listOfErrors)
                            ValidateTPA(PartyCode, oRecovery(iRecoveryCount).RecoveryPartyKey, bDummy, listOfErrors)
                        End If
                    ElseIf PartyType = "AGENT" OrElse PartyType = "CLIENT" Then
                        ValidatePartyType(PartyCode, oRecovery(iRecoveryCount).RecoveryDetails.ReceiptPartyKey, listOfErrors)
                        ValidatePartyType(PartyCode, oRecovery(iRecoveryCount).RecoveryPartyKey, listOfErrors)
                    End If

                End If

                iRecoveryCount += 1

            Next

            If oRecovery IsNot Nothing Then
                oClaimPeril.Recovery = New List(Of BaseClaimProcessPerilRecoveryType)(oRecovery)
            Else
                oClaimPeril.Recovery = New List(Of BaseClaimProcessPerilRecoveryType)
            End If

        End If
    End Sub

    Private Function ValidateAgent(ByVal brokerCode As String, ByVal listOfErrors As listOfErrorsType) As Boolean

        Dim sqlScript As String
        Dim resultObject(,) As Object

        ValidateAgent = True

        If brokerCode = "" Then
            Exit Function
        End If

        sqlScript = "SELECT is_deleted from Party p JOIN Party_Agent pa on p.party_cnt=pa.party_cnt Where shortname='" & brokerCode & "'"

        Dim iReturn As Integer = m_oDatabase.SQLSelect(sqlScript, "ValidateAgent", False, vResultArray:=resultObject)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'ValidateAgent' with the following SQL - " & sqlScript)
        End If

        If IsArray(resultObject) Then
            If resultObject(0, 0).ToString() = "1" Then
                ValidateAgent = False
                listOfErrors.Add("Import Failed because Agent " & brokerCode & " is set as deleted.")
            End If
        Else
            ValidateAgent = False
            listOfErrors.Add("Import Failed because Agent " & brokerCode & " could not be found.")
        End If

    End Function

    Private Function ValidateTPA(ByVal TPACode As String, ByRef TPAPartyKey As Integer, ByRef TPASettleDirectly As Boolean, ByVal listOfErrors As listOfErrorsType) As Boolean

        Dim sqlScript As String
        Dim resultObject(,) As Object

        ValidateTPA = True

        If TPACode = "" Then
            Exit Function
        End If

        sqlScript = "SELECT is_deleted,po.party_cnt,ISNULL(is_TPA_settle_directly,0) from Party p JOIN Party_other po on p.party_cnt=po.party_cnt Where shortname='" & TPACode & "'"

        Dim iReturn As Integer = m_oDatabase.SQLSelect(sqlScript, "ValidateTPA", False, vResultArray:=resultObject)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'ValidateTPA' with the following SQL - " & sqlScript)
        End If

        If IsArray(resultObject) Then
            If resultObject(0, 0).ToString() = "1" Then
                ValidateTPA = False
                listOfErrors.Add("Import Failed because TPA " & TPACode & " is set as deleted.")
            End If
            TPAPartyKey = Convert.ToInt32(resultObject(1, 0))
            TPASettleDirectly = (resultObject(2, 0).ToString() = "1")
        Else
            ValidateTPA = False
            listOfErrors.Add("Import Failed because TPA " & TPACode & " could not be found.")
        End If

    End Function

    ''' <summary>
    ''' ProcessReceiptPayeeDetails
    ''' </summary>
    ''' <param name="xField"></param>
    ''' <param name="oReceiptPayee"></param>
    ''' <param name="worksheet"></param>
    ''' <param name="rowCnt"></param>
    ''' <param name="listOfErrors"></param>
    ''' <remarks></remarks>
    Private Sub ProcessReceiptPayeeDetails(ByVal xField As Xml.XmlElement, ByRef oReceiptPayee As BaseClaimReceiptPayeeType, ByVal worksheet As Worksheet, ByVal rowCnt As Integer, ByRef listOfErrors As listOfErrorsType)
        Dim bAddressPresent As String = False
        Dim oReceiptPayeeAddress As New BaseAddressType
        Dim bReceiptPayeeExist As Boolean = False

        For Each field As Xml.XmlElement In xField.GetElementsByTagName("Field")
            If field.Attributes("Datatype").Value = "Payee" Then
                For Each item As Xml.XmlElement In xField.GetElementsByTagName("ReceiptPayeeDetails")
                    Dim thisCellName As String = ""
                    bReceiptPayeeExist = True
                    thisCellName = item.Attributes("SourceValue").Value
                    Select Case item.Attributes("Name").Value
                        Case "BankCode"
                            oReceiptPayee.BankCode = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "BankName"
                            oReceiptPayee.BankName = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "BankNumber"
                            oReceiptPayee.BankNumber = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "MediaReference"
                            oReceiptPayee.MediaReference = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "MediaTypeCode"
                            oReceiptPayee.MediaTypeCode = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "Name"
                            oReceiptPayee.Name = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "TheirReference"
                            oReceiptPayee.TheirReference = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "Comments"
                            oReceiptPayee.Comments = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "PartyBankKey"
                            oReceiptPayee.PartyBankKey = ToSafeInteger(GetCellvalue(thisCellName, worksheet, rowCnt), 0)

                        Case "AddressLine1"
                            oReceiptPayeeAddress.AddressLine1 = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "AddressLine2"
                            oReceiptPayeeAddress.AddressLine2 = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "AddressLine3"
                            oReceiptPayeeAddress.AddressLine3 = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "AddressLine4"
                            oReceiptPayeeAddress.AddressLine4 = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "PostCode"
                            oReceiptPayeeAddress.PostCode = GetCellvalue(thisCellName, worksheet, rowCnt)

                        Case "CountryCode"
                            oReceiptPayeeAddress.CountryCode = GetCellvalue(thisCellName, worksheet, rowCnt)
                    End Select
                Next
            End If
        Next
        If bReceiptPayeeExist = False Then
            oReceiptPayee = Nothing
        End If
        If Not String.IsNullOrEmpty(oReceiptPayeeAddress.CountryCode) Then
            oReceiptPayeeAddress.AddressTypeCode = AddressTypeType.Item3131XCO
            oReceiptPayee.Address = oReceiptPayeeAddress
        End If

    End Sub

    ''' <summary>
    ''' ValidatePartyType
    ''' </summary>
    ''' <param name="PartyTypeCode"></param>
    ''' <param name="PartyTypePartyKey"></param>
    ''' <param name="listOfErrors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidatePartyType(ByVal PartyTypeCode As String, ByRef PartyTypePartyKey As Integer, ByVal listOfErrors As listOfErrorsType) As Boolean
        Dim sSqlScript As String
        Dim oResultObject(,) As Object
        Dim bResult As Boolean = True

        sSqlScript = "spe_Party_sel"

        m_oDatabase.Parameters.Clear()
        AddParameterLite(m_oDatabase, "sShortCode", PartyTypeCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        Dim iReturn As Integer = m_oDatabase.SQLSelect(sSqlScript, "ValidatePartyType", True, vResultArray:=oResultObject)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'ValidateTPA' with the following SQL - " & sSqlScript)
        End If

        If IsArray(oResultObject) Then
            If oResultObject(33, 0).ToString() = "1" Then
                bResult = False
                listOfErrors.Add("Import Failed because PartyTypeCode " & PartyTypeCode & " is set as deleted.")
            End If
            PartyTypePartyKey = Convert.ToInt32(oResultObject(0, 0))
        Else
            bResult = False
            listOfErrors.Add("Import Failed because PartyTypeCode " & PartyTypeCode & " could not be found.")
        End If
        Return bResult
    End Function

    Private Function ValidateRecoveryPartyType(ByVal RecoveryPartyTypeCode As String, ByRef RecoveryPartyTypeKey As Integer, ByVal listOfErrors As listOfErrorsType) As Boolean
        Dim sSqlScript As String
        Dim oResultObject(,) As Object
        Dim bResult As Boolean = True

        sSqlScript = "SELECT recovery_party_type_id FROM recovery_party_type WHERE code = '" & RecoveryPartyTypeCode & "'"

        Dim iReturn As Integer = m_oDatabase.SQLSelect(sSqlScript, "ValidRecoveryPartyType", False, vResultArray:=oResultObject)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'ValidRecoveryPartyType' with the following SQL - " & sSqlScript)
        End If

        If IsArray(oResultObject) Then
            RecoveryPartyTypeKey = Convert.ToInt32(oResultObject(0, 0))
        Else
            bResult = False
            listOfErrors.Add("Import Failed because RecoveryPartyTypeCode " & RecoveryPartyTypeCode & " could not be found.")
        End If
        Return bResult
    End Function

    Private Function ValidateReserveAmount(ByRef ReserveTypeCode As String, ByVal listOfErrors As listOfErrorsType, ByVal ClaimNumber As String, ByVal PerilCode As String, ByVal ReserveAmount As Decimal) As Boolean
        Dim AllowNegativeReservesoption As Integer
        Dim iReturn As Integer = PMEReturnCode.PMTrue
        Dim AllowNegativeReservesScript
        Dim resultAllowNegativeReserves(,) As Object
        AllowNegativeReservesScript = "Select allow_Negative_Reserve from Product inner join Insurance_File on Product.product_id=Insurance_File.product_id INNER JOIN Claim on Claim.Policy_id = Insurance_File.insurance_file_cnt  where Claim.Claim_id= (Select MAX(Claim_id) From Claim Where Claim_Number = '" & ClaimNumber & "' And is_dirty=0)"
        iReturn = m_oDatabase.SQLSelect(AllowNegativeReservesScript, "GetProductOption", False, vResultArray:=resultAllowNegativeReserves)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'GetProductOption' with the following SQL - " & AllowNegativeReservesScript)
        End If

        If IsArray(resultAllowNegativeReserves) Then
            AllowNegativeReservesoption = resultAllowNegativeReserves(0, 0)
        End If

        If AllowNegativeReservesoption = 0 Then

            Dim GetCurrentReservesqlScript As String
            Dim CurrentReservesqlScriptresultObject(,) As Object

            GetCurrentReservesqlScript = "SELECT ((ISNULL(Initial_reserve,0) + ISNULL(Revised_reserve,0)) - ISNULL(Paid_to_date,0)) FROM Reserve INNER JOIN Claim_Peril ON Reserve.claim_Peril_id = Claim_Peril.Claim_Peril_id  INNER JOIN Peril_Type on Claim_Peril.Peril_type_id = Peril_type.Peril_type_id INNER JOIN Claim ON Claim_Peril.Claim_id = Claim.Claim_id INNER JOIN Reserve_type on Reserve.Reserve_type_id =Reserve_type.Reserve_type_id WHERE Claim.Claim_id = (Select MAX(Claim_id) From Claim where Claim_Number= '" & ClaimNumber & "' and is_dirty=0) And Reserve_type.name='" & ReserveTypeCode & "'And Peril_Type.Code = '" & PerilCode & "'"

            iReturn = m_oDatabase.SQLSelect(GetCurrentReservesqlScript, "GetCurrentReserve", False, vResultArray:=CurrentReservesqlScriptresultObject)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'GetCurrentReserve' with the following SQL - " & GetCurrentReservesqlScript)
            End If
            If IsArray(CurrentReservesqlScriptresultObject) Then
                Dim CurrentReserve As Decimal = CurrentReservesqlScriptresultObject(0, 0)
                ReserveAmount = Convert.ToDecimal(ReserveAmount)
                Dim ActualAmount As Decimal = CurrentReserve + ReserveAmount

                If ActualAmount < 0 Then
                    listOfErrors.Add("Total Reserve cannot be negative")
                End If
            End If
        End If
        Return iReturn
    End Function


#End Region

#Region "Creator"
    Public Sub New(ByVal oXML As XmlDocument)
        MyBase.New(oXML)
    End Sub
#End Region
End Class

