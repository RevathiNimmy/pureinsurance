Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.IO
Imports System.Xml.Serialization
Imports Aspose.Cells
Imports Sirius.SBO.Import.Excel_Import_Library
Imports System.Runtime.Serialization
Imports System.Linq
Imports System.Configuration

Friend NotInheritable Class Policy_BDX_Import : Inherits ImportBase

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
            Return "BDXPOL"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Bordereaux Policy Import"
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

    Public Property UserName() As String

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
    Private Sub BuildPolicyProcessRequest(ByRef PolicyProcessRequest As PolicyProcessCommand, ByRef PolicyProcessResponse As PolicyProcessCommandResponse, ByVal matchFound As Boolean, ByRef riskCnt As Integer, ByVal sPartyType As String, ByRef oRiskDetails() As BaseQuoteRiskMsgTypeRisks)

        ' If this is a new request then reset everything
        If matchFound = False Then
            If sPartyType = "PC" Then
                Dim partyPC As New BasePartyPCType

                partyPC.Addresses = New List(Of BaseAddressType)

                PolicyProcessRequest.BasePartyPCType = partyPC

                PolicyProcessRequest.Policy = New BaseQuoteRiskMsgType

                ReDim oRiskDetails(0)

                riskCnt = 0

            Else
                Dim partyCC As New BasePartyCCType

                partyCC.Addresses = New List(Of BaseAddressType)

                PolicyProcessRequest.BasePartyCCType = partyCC

                PolicyProcessRequest.Policy = New BaseQuoteRiskMsgType

                ReDim oRiskDetails(0)
                riskCnt = 0
            End If

            ' Else this is an additional risk being added into the structure
        Else

            riskCnt = oRiskDetails.GetUpperBound(0) + 1
            ReDim Preserve oRiskDetails(riskCnt)

        End If

    End Sub

    ''' <summary>
    ''' This runs the custom Policy SQL as specified in the Configuration file
    ''' </summary>
    ''' <param name="xePolicyMatchingElement"></param>
    ''' <param name="PolicyProcessRequest"></param>
    ''' <param name="worksheet"></param>
    ''' <param name="nRowCnt"></param>
    ''' <param name="olistOfErrors"></param>
    ''' <param name="sCertRef"></param>
    ''' <remarks></remarks>
    Private Sub LookupPolicy(ByVal xePolicyMatchingElement As Xml.XmlElement, ByRef PolicyProcessRequest As PolicyProcessCommand, ByVal worksheet As Worksheet, ByVal nRowCnt As Integer, ByVal olistOfErrors As listOfErrorsType, ByVal sCertRef As String)

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
                olistOfErrors.Add("The PolicyMatching SQL file was not found.  Please check the path in the configuration file")
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
        For Each xeParameter As Xml.XmlElement In xePolicyMatchingElement.Item("Parameters").GetElementsByTagName("Parameter")

            Dim sThisCellName As String = xeParameter.Attributes(STR_SourceValue).Value
            Dim nThisCellPosition As Integer = Convert.ToInt32(xeParameter.Attributes(STR_Position).Value)
            Dim thisCell As Cell
            Dim oThisRowCellValue As Object = 2

            If sThisCellName.StartsWith("$") Then
                sThisCellName = sThisCellName.Substring(1).Trim & (nRowCnt).ToString
                thisCell = worksheet.Cells(sThisCellName)
                If thisCell IsNot Nothing Then
                    oThisRowCellValue = thisCell.Value
                End If
            Else

                oThisRowCellValue = sThisCellName

            End If

            If oThisRowCellValue IsNot Nothing Then
                ' Check the validity of the data item before passing it into the SQL
                If ValidateAndConvertDatatype(xeParameter, oThisRowCellValue, sThisCellName, olistOfErrors, sThisCellName, nRowCnt) = False Then
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
            Dim aResultArray As System.Array = oResultObject
            If aResultArray.Rank = 2 AndAlso aResultArray.GetUpperBound(0) > 3 Then
                ' If we have an error then 
                If aResultArray(4, 0).ToString <> String.Empty Then
                    olistOfErrors.Add(aResultArray(4, 0).ToString, "", nRowCnt)
                Else
                    If SharedFiles.ToSafeString(PolicyProcessRequest.Policy.QuoteRef).Trim() = SharedFiles.ToSafeString(aResultArray(0, 0)).Trim Then
                        PolicyProcessRequest.Policy.NewQuoteRef = aResultArray(0, 0)
                    Else
                        PolicyProcessRequest.Policy.NewQuoteRef = sCertRef
                    End If
                    PolicyProcessRequest.Policy.QuoteRef = aResultArray(0, 0)
                    PolicyProcessRequest.Policy.TransactionTypeCode = aResultArray(3, 0)
                    If aResultArray(1, 0) <> String.Empty Then
                        PolicyProcessRequest.Policy.CoverStartDate = aResultArray(1, 0)
                    End If
                    If aResultArray(2, 0) <> String.Empty Then
                        PolicyProcessRequest.Policy.CoverEndDate = aResultArray(2, 0)
                        If aResultArray.GetUpperBound(0) > 5 Then
                            PolicyProcessRequest.Policy.PartyKey = aResultArray(6, 0)
                        End If
                    End If
                    If (PolicyProcessRequest.AgentCode Is Nothing) OrElse (PolicyProcessRequest.AgentCode IsNot Nothing AndAlso Convert.ToString(PolicyProcessRequest.AgentCode).Trim.Length = 0) Then
                        If aResultArray.GetUpperBound(0) >= 5 AndAlso aResultArray(5, 0) IsNot Nothing AndAlso aResultArray(5, 0).ToString <> String.Empty Then
                            PolicyProcessRequest.AgentCode = aResultArray(5, 0)
                        Else
                            PolicyProcessRequest.AgentCode = ""
                        End If
                    End If
                    ' client code validation
                    If aResultArray.GetUpperBound(0) >= 6 AndAlso SharedFiles.ToSafeLong(aResultArray(6, 0)) > 0 Then
                        PolicyProcessRequest.ClientID = SharedFiles.ToSafeLong(aResultArray(6, 0))
                        PolicyProcessRequest.ClientCodeSpecified = True
                    Else
                        PolicyProcessRequest.ClientCodeSpecified = False
                    End If
                End If
            Else
                olistOfErrors.Add("The results returned from the Policy matching SQL did not return the correct number of items in the select list.  Please review the SQL contained in the file - " & xePolicyMatchingElement.GetElementsByTagName("SQL").Item(0).InnerText.ToString)
            End If
        End If

    End Sub

    ''' <summary>
    ''' This runs the custom Risk SQL as specified in the Configuration file
    ''' </summary>
    ''' <param name="riskMatchingElement"></param>
    ''' <param name="PolicyProcessRequest"></param>
    ''' <param name="worksheet"></param>
    ''' <param name="rowCnt"></param>
    ''' <param name="riskCnt"></param>
    ''' <param name="listOfErrors"></param>
    ''' <param name="certRef"></param>
    ''' <param name="oRiskDetails"></param>
    ''' <remarks></remarks>
    Private Sub LookupRisk(ByVal riskMatchingElement As Xml.XmlElement, ByRef PolicyProcessRequest As PolicyProcessCommand, ByVal worksheet As Worksheet, ByVal rowCnt As Integer, ByVal riskCnt As Integer, ByVal listOfErrors As listOfErrorsType, ByVal certRef As String, ByRef oRiskDetails() As BaseQuoteRiskMsgTypeRisks)

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
            If resultArray.Rank = 2 AndAlso resultArray.GetUpperBound(0) = 2 Then
                If resultArray(1, 0).ToString <> String.Empty Then
                    listOfErrors.Add(resultArray(1, 0).ToString, "", rowCnt)
                Else
                    oRiskDetails(riskCnt).RiskFolderKeySpecified = Integer.TryParse(resultArray(0, 0), oRiskDetails(riskCnt).RiskFolderKey)
                    oRiskDetails(riskCnt).OriginalRiskKeySpecified = Integer.TryParse(resultArray(2, 0), oRiskDetails(riskCnt).OriginalRiskKey)
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
        Dim xDocXmlDocument As New Xml.XmlDocument
        Dim wbErrorWorkbook As Workbook
        Dim sErrorWorkbookFilename As String = String.Empty
        Dim sBrokerCode As String = String.Empty
        Dim crTotalAmount As Decimal = 0
        Dim nTotalTransactions As Integer = 0
        Dim crRejectAmount As Decimal = 0
        Dim nRejectTransactions As Integer = 0
        Dim sPartyType As String = "CC"
        Dim sBusinessType As String = "AGENCY"
        Dim sProcessedFolder As String = ""
        Dim sbranchCode As String = String.Empty
        Dim bIsCoinsPlacementMissing As Boolean = False
        Dim _apiClient As ApiClient = Nothing

        Try
            ' Define the REST API client
            OutputLine("Creating REST API client...")
            _apiClient = New ApiClient(ConfigurationManager.AppSettings("RestAPIUrl"))
            OutputLine("Validating XML mapping...")
            m_oXML.Validate()

            OutputLine("Creating Batch...")
            CreateBatch()

            xDocXmlDocument.Load(ConfigFile.FullName)

            'Creating a file stream containing the Excel file to be opened
            fsFileStream = New FileStream(WorkbookFile.FullName, FileMode.Open)

            'Instantiate a Workbook object that represents the existing Excel file
            Dim workbook As Workbook = New Workbook(fsFileStream)

            ' Retrieve the Import Header Element
            Dim xeWorkbookElement As Xml.XmlElement = xDocXmlDocument.Item(STR_ImportHeader)

            If xeWorkbookElement.Attributes IsNot Nothing AndAlso xeWorkbookElement.Attributes.Count > 1 AndAlso xeWorkbookElement.Attributes(STR_ProcessedFolder) IsNot Nothing Then
                sProcessedFolder = xeWorkbookElement.Attributes(STR_ProcessedFolder).Value
                sProcessedFolder = sProcessedFolder.Trim
            End If
            sBrokerCode = xeWorkbookElement.Attributes(STR_Broker_Code).Value
            If xeWorkbookElement.Attributes(STR_Broker_Code) IsNot Nothing AndAlso (xeWorkbookElement.Attributes(STR_Broker_Code).Value).ToString.Trim.Length > 0 Then
                sBrokerCode = xeWorkbookElement.Attributes(STR_Broker_Code).Value
                sBusinessType = "AGENCY"
            Else
                sBusinessType = "DIRECT"
            End If

            OutputLine("Processing data file...")
            ' Loop around the WorkSheet elements in the Configuration File
            For Each worksheetElement As Xml.XmlElement In xeWorkbookElement.ChildNodes

                ' Check the name of the child element identifies it as a WorkSheet
                If worksheetElement.Name = STR_Worksheet Then

                    ' Get the Worksheet name
                    Dim sWorksheetName As String = worksheetElement.Attributes(STR_Name).Value

                    Dim license As License = New License()

                    'Set the license of Aspose.Cells to avoid the evaluation limitations
                    license.SetLicense("Aspose.Total.lic")

                    'Instantiate a new Workbook object that represents the existing Excel file
                    Dim worksheet As Worksheet = workbook.Worksheets(sWorksheetName)

                    ' Create an Error file ready to report any errors that may occur
                    sErrorWorkbookFilename = WorkbookFile.Directory.FullName & "\" & WorkbookFile.Name.Replace(WorkbookFile.Extension, " - Errors(" & Now.ToFileTime & ")" & WorkbookFile.Extension)
                    wbErrorWorkbook = New Workbook
                    Dim errorworksheet As Worksheet = wbErrorWorkbook.Worksheets(0)
                    errorworksheet.Name = sWorksheetName

                    ' Grab the child Elemenets for this worksheet from the configuration file
                    Dim xeMappingElement As Xml.XmlElement = worksheetElement.Item("Mapping")
                    Dim xeRowMappingElement As Xml.XmlElement = worksheetElement.Item("RowMapping")
                    Dim xePolicyMappingElement As Xml.XmlElement = worksheetElement.Item("PolicyMatching")
                    Dim xeRiskMappingElement As Xml.XmlElement = worksheetElement.Item("RiskMatching")
                    Dim nStartRow As Integer = CInt(xeMappingElement.Attributes("StartingRow").Value)
                    Dim sCertRefColumn As String = xeMappingElement.Attributes(STR_Certificate_Ref_Column).Value
                    Dim sCoverHolderNameColumn As String = xeMappingElement.Attributes(STR_CoverHoldername_Column).Value
                    Dim sbranchCodeColumn As String = String.Empty

                    sCertRefColumn = sCertRefColumn.Substring(1).Trim()
                    sCoverHolderNameColumn = sCoverHolderNameColumn.Substring(1).Trim()

                    For Each Node As XmlNode In xeMappingElement.ChildNodes
                        If (Node.NodeType = XmlNodeType.Element) Then
                            Dim wrkshtElement As XmlElement = Node
                            If (wrkshtElement.Attributes("Description").Value = STR_BranchCode) Then
                                sbranchCodeColumn = wrkshtElement.Attributes("SourceValue").Value
                                Exit For
                            End If
                        End If
                    Next
                    Dim bBranchFromSheet As Boolean = False

                    If sbranchCodeColumn.StartsWith("$") Then
                        sbranchCodeColumn = sbranchCodeColumn.Substring(1).Trim()
                        bBranchFromSheet = True
                    End If

                    Dim bValueIsValid As Boolean = True

                    ' Deserialize the XML from the implementation resultdataset into 
                    ' the correct messaginging format
                    Dim oGlobalListOfErrors As New listOfErrorsType

                    ' Copy across the Header Rows to the Error Workbook
                    Dim errorCells As Cells = wbErrorWorkbook.Worksheets(sWorksheetName).Cells
                    If nStartRow > 1 Then
                        Try
                            errorCells.CopyRows(worksheet.Cells, 0, 0, nStartRow - 1)
                        Catch ex As Exception
                            ' I cant get rid of the exception triggered by the CopyRows method but the row still
                            ' gets copied so for now we'll just ignore the exception
                        End Try
                    End If

                    If UCase(Trim(sBusinessType)) = "AGENCY" AndAlso ValidateAgent(sBrokerCode, oGlobalListOfErrors) = False Then
                        OutputErrorRow(m_oDatabase, sWorksheetName, worksheet, sErrorWorkbookFilename, wbErrorWorkbook, 0, oGlobalListOfErrors, "Policy_BDX_Import", 0, sBrokerCode)
                    ElseIf ValidateRiskData(xeMappingElement, oGlobalListOfErrors) = False Then
                        OutputErrorRow(m_oDatabase, sWorksheetName, worksheet, sErrorWorkbookFilename, wbErrorWorkbook, 0, oGlobalListOfErrors, "Policy_BDX_Import", 0, sBrokerCode)
                    Else

                        ' Validate the header structure
                        If (HeaderStructureIsValid(worksheetElement, worksheet, oGlobalListOfErrors) = False) Then
                            OutputErrorRow(m_oDatabase, sWorksheetName, worksheet, sErrorWorkbookFilename, wbErrorWorkbook, 0, oGlobalListOfErrors, "Policy_BDX_Import", 0, sBrokerCode)
                        Else
                            Dim PolicyProcessRequest As New PolicyProcessCommand
                            Dim PolicyProcessResponse As PolicyProcessCommandResponse = Nothing
                            Dim bMatchFound As Boolean = False
                            Dim nRiskCnt As Integer = 0
                            Dim bUpdateParty As Boolean = False
                            Dim oRiskDetails As BaseQuoteRiskMsgTypeRisks() = Nothing
                            Dim oPoilcyTax As BaseTaxesType() = Nothing
                            Dim oRiskTax As BaseTaxesType() = Nothing
                            Dim oPBData As BaseProductBuilderRiskType() = Nothing
                            Dim oCoInsurer As BaseUpdateCoinsuranceValuesRequestTypeRow() = Nothing
                            Dim oContacts As BaseContactType() = Nothing

                            ' For each Row in the worksheet
                            For cnt As Integer = nStartRow To worksheet.Cells.MaxDataRow + 1

                                Dim listOfErrors As New listOfErrorsType

                                ' Setup the new Request structure
                                If bMatchFound = False Then
                                    PolicyProcessRequest = New PolicyProcessCommand
                                    PolicyProcessResponse = Nothing
                                    nRiskCnt = 0
                                End If

                                If worksheet IsNot Nothing AndAlso worksheet.Cells IsNot Nothing AndAlso xeMappingElement.Attributes(STR_PartyType) IsNot Nothing AndAlso (xeMappingElement.Attributes(STR_PartyType).Value) <> String.Empty Then
                                    If xeMappingElement.Attributes(STR_PartyType).Value.StartsWith("$") Then
                                        sPartyType = worksheet.Cells((xeMappingElement.Attributes(STR_PartyType).Value).Substring(1).Trim & cnt.ToString).Value
                                    Else
                                        sPartyType = xeMappingElement.Attributes(STR_PartyType).Value
                                    End If
                                End If
                                'Changes made to achieve Existing functionality of arch.
                                bUpdateParty = True
                                If worksheet IsNot Nothing AndAlso worksheet.Cells IsNot Nothing AndAlso xeMappingElement.Attributes(STR_UpdateParty) IsNot Nothing AndAlso (xeMappingElement.Attributes(STR_UpdateParty).Value) <> String.Empty Then
                                    Try
                                        If xeMappingElement.Attributes(STR_UpdateParty).Value.StartsWith("$") Then

                                            bUpdateParty = Convert.ToBoolean(worksheet.Cells((xeMappingElement.Attributes(STR_UpdateParty).Value).Substring(1).Trim & cnt.ToString).Value)
                                        Else
                                            bUpdateParty = Convert.ToBoolean(xeMappingElement.Attributes(STR_UpdateParty).Value)
                                        End If

                                    Catch
                                        bUpdateParty = False
                                    End Try
                                End If
                                PolicyProcessRequest.UpdateParty = bUpdateParty
                                If bMatchFound = False Then
                                    BuildPolicyProcessRequest(PolicyProcessRequest, PolicyProcessResponse, False, nRiskCnt, sPartyType, oRiskDetails)
                                End If

                                bValueIsValid = True
                                nTotalTransactions = nTotalTransactions + 1

                                ' Get the Certificate ref from the Cell.
                                Dim sCertRef As String = worksheet.Cells(sCertRefColumn & cnt.ToString).Value
                                Dim sCoverHolderName As String = worksheet.Cells(sCoverHolderNameColumn & cnt.ToString).Value

                                If bBranchFromSheet Then
                                    sbranchCode = worksheet.Cells(sbranchCodeColumn & cnt.ToString).Value
                                Else
                                    sbranchCode = sbranchCodeColumn
                                End If

                                If ValidateBranch(sbranchCode, UserName, oGlobalListOfErrors) = False Then
                                    listOfErrors.Add("User - " & UserName & " doesn't have access on branch code " & sbranchCode & " in Row " & cnt & " therefore it is being rejected", sCertRefColumn, cnt)
                                    bValueIsValid = False
                                End If
                                ' Check that this certificate hasn't already resulted in an error
                                If oGlobalListOfErrors.Exists(AddressOf New certFinderPredicate(sCertRef).Match) Then

                                    listOfErrors.Add("The Certificate - " & sCertRef & " - referenced Value in Row " & cnt & " has errored on a previous row in this import therefore it is being rejected", sCertRefColumn, cnt)
                                    bValueIsValid = False

                                Else

                                    Dim xnsmNsMgr As New Xml.XmlNamespaceManager(xDocXmlDocument.NameTable)
                                    xnsmNsMgr.AddNamespace("ab", "http://www.siriusfs.com/SFI/Import/Policy_BDX_Import/20051005")
                                    Dim sParameterFullName As String = ""
                                    Dim oAddresses As BaseAddressType() = Nothing

                                    Dim oPartyPC As BasePartyPCType = Nothing
                                    Dim oPartyCC As BasePartyCCType = Nothing
                                    If sPartyType = "PC" Then
                                        oPartyPC = New BasePartyPCType
                                    Else
                                        oPartyCC = New BasePartyCCType
                                    End If

                                    ReDim oAddresses(0)
                                    oAddresses(0) = New BaseAddressType

                                    ReDim Preserve oRiskDetails(nRiskCnt)
                                    oRiskDetails(nRiskCnt) = New BaseQuoteRiskMsgTypeRisks

                                    ReDim oPoilcyTax(0)
                                    oPoilcyTax(0) = New BaseTaxesType

                                    ReDim oRiskTax(0)
                                    oRiskTax(0) = New BaseTaxesType

                                    Dim nPNCount As Integer = 0

                                    ' Loop around each field in the configuration mapping 
                                    For Each field As Xml.XmlElement In xeMappingElement.GetElementsByTagName("Field")

                                        Dim bSkipProcess As Boolean = False
                                        SkipProcessingRiskData(field, sPartyType, bSkipProcess)
                                        If bSkipProcess = False Then
                                            Dim oCellValue As String = String.Empty
                                            Dim sDatatype As String = String.Empty

                                            ' Extract the value from the spreadsheet
                                            ExtractAndValidateValue(xnsmNsMgr, worksheet, cnt, listOfErrors, field, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                                            Dim sParameterName As String = ""
                                            Dim nIndex As Integer = 0

                                            If (oCellValue Is Nothing AndAlso field.Attributes("Datatype").Value <> "Date") Then

                                                oCellValue = SharedFiles.ToSafeString(oCellValue)
                                            End If

                                            If (oCellValue IsNot Nothing) AndAlso (bValueIsValid) Then
                                                sParameterFullName = [String].Format(field.Attributes("DestMapping").Value, nRiskCnt + 1)
                                                If sParameterFullName = "/PolicyProcessRequestType/bt:BranchCode" Then
                                                    PolicyProcessRequest.BranchCode = oCellValue
                                                ElseIf sParameterFullName = "/PolicyProcessRequestType/bt:AgentCode" Then
                                                    PolicyProcessRequest.AgentCode = oCellValue
                                                ElseIf sParameterFullName = "/PolicyProcessRequestType/bt:CurrencyCode" Then
                                                    Dim eCurrencyType As CurrencyType
                                                    If [Enum].TryParse(oCellValue, eCurrencyType) Then
                                                        PolicyProcessRequest.CurrencyCode = eCurrencyType
                                                    End If
                                                ElseIf sPartyType = "PC" AndAlso (sParameterFullName.Contains("/PolicyProcessRequestType/bt:PersonalClient/bt:")) Then
                                                    ProcessPCDetails(sParameterFullName, oCellValue, oPartyPC, oAddresses)
                                                ElseIf sPartyType = "CC" AndAlso (sParameterFullName.Contains("/PolicyProcessRequestType/bt:CorporateClient/bt:")) Then
                                                    ProcessCCDetails(sParameterFullName, oCellValue, oPartyCC, oAddresses)
                                                ElseIf sParameterFullName.Contains("/PolicyProcessRequestType/bt:Policy") AndAlso (Not sParameterFullName.Contains("/PolicyProcessRequestType/bt:Policy/bt:Risk")) AndAlso (Not sParameterFullName.Contains("/PolicyProcessRequestType/bt:Policy/bt:Tax")) Then
                                                    ProcessPolicyData(sParameterFullName, oCellValue, PolicyProcessRequest.Policy)
                                                ElseIf sParameterFullName.Contains("/PolicyProcessRequestType/bt:Policy/bt:Risk") Then
                                                    ProcessRisksData(sParameterFullName, oCellValue, nRiskCnt, oRiskDetails)
                                                Else
                                                    If field.Attributes("Datatype").Value.Trim = "CoInsurers" Then
                                                        ProcessCoInsurers(field, oCoInsurer, listOfErrors, cnt, worksheet)
                                                    ElseIf field.Attributes("Datatype").Value.Trim = "Contacts" Then
                                                        ProcessContacts(field, oContacts, listOfErrors, cnt, worksheet)
                                                    Else
                                                        ProcessTaxesandPBData(field, PolicyProcessRequest, worksheet, cnt, nRiskCnt, nPNCount, oPBData, oRiskTax, oPoilcyTax, oCellValue)
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Next

                                    If PolicyProcessRequest.Policy.PolicyProcessType Is Nothing Then
                                        PolicyProcessRequest.Policy.PolicyProcessType = PolicyProcessTypes.Bind
                                    End If

                                    If sPartyType = "PC" Then
                                        oPartyPC.Addresses = New List(Of BaseAddressType)(oAddresses)
                                        If oContacts IsNot Nothing Then
                                            oPartyPC.Contacts = New List(Of BaseContactType)(oContacts)
                                        End If
                                        PolicyProcessRequest.BasePartyPCType = oPartyPC
                                    Else
                                        oPartyCC.Addresses = New List(Of BaseAddressType)(oAddresses)
                                        If oContacts IsNot Nothing Then
                                            oPartyCC.Contacts = New List(Of BaseContactType)(oContacts)
                                        End If
                                        PolicyProcessRequest.BasePartyCCType = oPartyCC
                                    End If

                                    If bValueIsValid = True Then
                                        If oPBData IsNot Nothing Then
                                            oRiskDetails(nRiskCnt).ProductBuilderDetail = New List(Of BaseProductBuilderRiskType)(oPBData)
                                        End If
                                        oRiskDetails(nRiskCnt).Taxes = New List(Of BaseTaxesType)(oRiskTax)
                                        PolicyProcessRequest.Policy.Taxes = New List(Of BaseTaxesType)(oPoilcyTax)
                                    End If
                                    oRiskDetails(nRiskCnt).Taxes = New List(Of BaseTaxesType)(oRiskTax)
                                    PolicyProcessRequest.Policy.Taxes = New List(Of BaseTaxesType)(oPoilcyTax)
                                    PolicyProcessRequest.Policy.IsBDXRequest = True
                                    PolicyProcessRequest.Policy.DeletePolicyUnderRenewal = 1
                                    PolicyProcessRequest.Policy.DoNotCopyRiskAtRenSelection = True
                                    If oCoInsurer IsNot Nothing Then
                                        PolicyProcessRequest.Policy.CoInsurers = New List(Of BaseUpdateCoinsuranceValuesRequestTypeRow)(oCoInsurer)
                                        ValidateCoInsurerFields(PolicyProcessRequest.Policy.CoInsurers, cnt, listOfErrors)
                                    Else
                                        PolicyProcessRequest.Policy.CoInsurancePlacement = Nothing
                                        PolicyProcessRequest.Policy.BusinessTypeCode = Nothing
                                    End If
                                End If

                                If bValueIsValid = False Then
                                    ' Output the error row
                                    OutputErrorRow(m_oDatabase, sWorksheetName, worksheet, sErrorWorkbookFilename, wbErrorWorkbook, cnt, listOfErrors, "Policy_BDX_Import", 0, sBrokerCode, sCoverHolderName)
                                    oGlobalListOfErrors.Add("Failure recorded for Certificate Ref - " & sCertRef, sCertRefColumn, cnt, sCertRef)
                                    nRejectTransactions = nRejectTransactions + 1
                                Else

                                    ' Does the next row in the spreadsheet match this policy?
                                    bMatchFound = False
                                    MatchOnNextRow(xeRowMappingElement, worksheet, worksheet.Cells.MaxDataRow + 1, cnt, bMatchFound)

                                    ' Use the custom XML to match the policy record
                                    LookupPolicy(xePolicyMappingElement, PolicyProcessRequest, worksheet, cnt, listOfErrors, sCertRef)

                                    If listOfErrors.Count > 0 Then
                                        ' Output the error row
                                        OutputErrorRow(m_oDatabase, sWorksheetName, worksheet, sErrorWorkbookFilename, wbErrorWorkbook, cnt, listOfErrors, "Policy_BDX_Import", 0, sBrokerCode, sCoverHolderName)
                                        oGlobalListOfErrors.Add("Failure recorded for Certificate Ref - " & sCertRef, sCertRefColumn, cnt, sCertRef)
                                        nRejectTransactions = nRejectTransactions + 1
                                    Else

                                        ' Use the custom XML to match the risk record
                                        LookupRisk(xeRiskMappingElement, PolicyProcessRequest, worksheet, cnt, nRiskCnt, listOfErrors, sCertRef, oRiskDetails)

                                        If PolicyProcessRequest.Policy.TransactionTypeCode <> "POLICY" AndAlso listOfErrors.Count > 0 Then
                                            ' Output the error row
                                            OutputErrorRow(m_oDatabase, sWorksheetName, worksheet, sErrorWorkbookFilename, wbErrorWorkbook, cnt, listOfErrors, "Policy_BDX_Import", 0, sBrokerCode, sCoverHolderName)
                                            oGlobalListOfErrors.Add("Failure recorded for Certificate Ref - " & sCertRef, sCertRefColumn, cnt, sCertRef)
                                            nRejectTransactions = nRejectTransactions + 1

                                        Else

                                            ' If a match was found on the next row then prepare the request for the next risk
                                            If bMatchFound Then

                                                BuildPolicyProcessRequest(PolicyProcessRequest, PolicyProcessResponse, bMatchFound, nRiskCnt, sPartyType, oRiskDetails)

                                            Else

                                                Try
                                                    ' Call SAM to process the policy
                                                    PolicyProcessRequest.Policy.Risks = New List(Of BaseQuoteRiskMsgTypeRisks)(oRiskDetails)
                                                    PolicyProcessRequest.Policy.DoNotCopyRiskAtRenSelection = True
                                                    PolicyProcessRequest.Policy.DeletePolicyUnderRenewal = 1
                                                    If (PolicyProcessRequest.Policy.BusinessTypeCode = "COIN LEAD" OrElse PolicyProcessRequest.Policy.BusinessTypeCode = "COIN FOLL") AndAlso
                                                          (PolicyProcessRequest.Policy.CoInsurancePlacement Is Nothing OrElse (PolicyProcessRequest.Policy.CoInsurancePlacement.ToUpper <> "GROSS" AndAlso
                                                           PolicyProcessRequest.Policy.CoInsurancePlacement.ToUpper <> "NETT")) Then
                                                        bIsCoinsPlacementMissing = True
                                                        Throw New Exception("The import of Row " & cnt & " failed because the valid co-insurer placement (GROSS or NETT) cannot be found")
                                                    End If
                                                    PolicyProcessResponse = _apiClient.PostAsync(Of PolicyProcessCommandResponse)("/messaging/policyProcess", PolicyProcessRequest).Result

                                                    If PolicyProcessResponse IsNot Nothing AndAlso PolicyProcessResponse.Policy IsNot Nothing AndAlso PolicyProcessResponse.Policy.Count > 0 Then

                                                        crTotalAmount = crTotalAmount + PolicyProcessResponse.Policy(0).PremiumDueGross

                                                    ElseIf PolicyProcessResponse IsNot Nothing AndAlso PolicyProcessResponse.Errors IsNot Nothing Then

                                                        For Each stsError As SAMErrors In PolicyProcessResponse.Errors
                                                            listOfErrors.Add("The import of Row " & cnt & "  failed. " & stsError.SAMErrorMessage)
                                                        Next

                                                        OutputErrorRow(m_oDatabase, sWorksheetName, worksheet, sErrorWorkbookFilename, wbErrorWorkbook, cnt, listOfErrors, "Policy_BDX_Import", 0, sBrokerCode, sCoverHolderName)
                                                        oGlobalListOfErrors.Add("Failure recorded for Certificate Ref - " & sCertRef, sCertRefColumn, cnt, sCertRef)
                                                        nRejectTransactions = nRejectTransactions + 1

                                                    End If

                                                Catch ex As Exception

                                                    If PolicyProcessResponse IsNot Nothing AndAlso PolicyProcessResponse.Errors IsNot Nothing Then
                                                        For Each stsError As SAMErrors In PolicyProcessResponse.Errors
                                                            listOfErrors.Add("The import of Row " & cnt & "  failed. " & stsError.SAMErrorMessage)
                                                        Next
                                                    ElseIf (bIsCoinsPlacementMissing) Then
                                                        listOfErrors.Add("The import of Row " & cnt & " failed because the valid co-insurer placement (GROSS or NETT) cannot be found")
                                                    Else
                                                        listOfErrors.Add("The import of Row " & cnt & "  failed. " & ex.Message)
                                                    End If

                                                    OutputErrorRow(m_oDatabase, sWorksheetName, worksheet, sErrorWorkbookFilename, wbErrorWorkbook, cnt, listOfErrors, "Policy_BDX_Import", 0, sBrokerCode, sCoverHolderName)
                                                    oGlobalListOfErrors.Add("Failure recorded for Certificate Ref - " & sCertRef, sCertRefColumn, cnt, sCertRef)
                                                    nRejectTransactions = nRejectTransactions + 1

                                                End Try

                                                ' Reset the request object
                                                PolicyProcessRequest = New PolicyProcessCommand
                                                PolicyProcessResponse = Nothing
                                                bMatchFound = False
                                                nRiskCnt = 0
                                                BuildPolicyProcessRequest(PolicyProcessRequest, PolicyProcessResponse, False, nRiskCnt, "CC", Nothing)

                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If
            Next

            'Closing the file streams to free all resources
            fsFileStream.Close()
            m_sBatchStatus = kBatchStatusComplete
            If nRejectTransactions > 0 Then
                OutputLine("Process policy bordereaux completed. Total request processed - " & nTotalTransactions & ", Failed - " & nRejectTransactions & " See out file for details.")
            Else
                OutputLine("Process policy bordereaux completed. Total request processed - " & nTotalTransactions & " See out file for details.")
            End If

        Catch NotExcelFileEx As Aspose.Cells.CellsException
            ' Create a work manager task to indicate this file failed
            m_sBatchStatus = kBatchStatusFailed
            CreateWorkManagerTask(String.Format("Policy BDX Import failed for the import file {0}.  The file was not recognised as a valid Excel Spreadsheet.", WorkbookFile.FullName))
        Catch ex As Exception
            m_sBatchStatus = kBatchStatusFailed
            Throw New Exception("Unable to add new book.", ex)
        Finally

            If fsFileStream IsNot Nothing Then
                fsFileStream.Close()
            End If
            Dim sIPProcessedFolder As String = ImportedPath
            Try
                ' Output the files to a specific Broker folder
                If sProcessedFolder <> String.Empty Then
                    sIPProcessedFolder = ImportedPath & "\" + sProcessedFolder
                    If Directory.Exists(sIPProcessedFolder) = False Then
                        Directory.CreateDirectory(sIPProcessedFolder)
                    End If
                Else
                    If sBrokerCode <> String.Empty Then
                        sIPProcessedFolder = ImportedPath & "\" & sBrokerCode
                        If Directory.Exists(sIPProcessedFolder) = False Then
                            Directory.CreateDirectory(sIPProcessedFolder)
                        End If
                    Else
                        sIPProcessedFolder = ImportedPath & "\DirectBusiness"
                        If Directory.Exists(sIPProcessedFolder) = False Then
                            Directory.CreateDirectory(sIPProcessedFolder)
                        End If
                    End If
                End If
            Catch exDirectory As Exception
                CreateWorkManagerTask(String.Format(exDirectory.Message, WorkbookFile.FullName))
            End Try

            ' If the Error file exists then 
            If sErrorWorkbookFilename <> String.Empty AndAlso IO.File.Exists(sErrorWorkbookFilename) Then
                Dim sProcessedFilename As String = String.Empty
                Try
                    Dim targetFileName As String = WorkbookFile.Name.Replace(WorkbookFile.Extension, " - Errors(" & Now.ToFileTime & ")" & WorkbookFile.Extension)
                    sProcessedFilename = sIPProcessedFolder & "\" & targetFileName

                    ' Move file to processed directory
                    File.Move(sErrorWorkbookFilename, sProcessedFilename)

                    If CloudHostingEnabled Then

                        Using fileStream As Stream = New FileStream(sProcessedFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                            Dim fileBytes(0 To fileStream.Length - 1) As Byte
                            fileStream.Read(fileBytes, 0, fileBytes.Length)

                            CloudRepository.UploadFile(Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudProcessedFolder & sProcessedFilename.Substring(ConfiguredImportedPath.Length).Replace("\", "/"),
                                                       fileBytes)
                        End Using

                    End If

                    ' Create a work manager task to indicate this file failed
                    CreateWorkManagerTask(String.Format("Policy BDX Import failed for the import file {0}.  Please see the Policy BDX error report for further details", WorkbookFile.FullName))

                Catch ex As System.IO.IOException
                    ' Create a work manager task to indicate this file failed
                    CreateWorkManagerTask(String.Format("Policy BDX Import Failed but unable to move error file from {0} to {1}.", sErrorWorkbookFilename, sProcessedFilename))
                End Try
            End If

            ' Move the original file across tp processed
            If WorkbookFile.Name <> String.Empty Then
                Dim sProcessedFilename As String = String.Empty
                Try
                    Dim targetFileName As String = WorkbookFile.Name.Replace(WorkbookFile.Extension, " - (" & Now.ToFileTime & ")" & WorkbookFile.Extension)
                    sProcessedFilename = sIPProcessedFolder & "\" & targetFileName

                    ' Move file to processed directory
                    File.Move(WorkbookFile.FullName, sProcessedFilename)

                    If CloudHostingEnabled Then
                        Dim result As Integer
                        Using fileStream As Stream = New FileStream(sProcessedFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                            Dim fileBytes(0 To fileStream.Length - 1) As Byte
                            fileStream.Read(fileBytes, 0, fileBytes.Length)

                            result = CloudRepository.UploadFile(Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudProcessedFolder & sProcessedFilename.Substring(ConfiguredImportedPath.Length).Replace("\", "/"),
                                                                           fileBytes).Result
                        End Using

                        If result = gPMConstants.PMEReturnCode.PMTrue Then
                            Dim s3ImportFile As String = Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudImportFolder & WorkbookFile.FullName.Substring(ConfiguredImportPath.Length).Replace("\", "/")
                            CloudRepository.DeleteFile(s3ImportFile)
                        End If
                    End If
                Catch ex As System.IO.IOException
                    ' Create a work manager task to indicate this file failed
                    CreateWorkManagerTask(String.Format("Policy BDX Import Failed but unable to move error file from {0} to {1}.", WorkbookFile.FullName, sProcessedFilename))
                End Try
            End If

            ' Take off the rejected transactions
            'nTotalTransactions = nTotalTransactions - nRejectTransactions

            ' Update the batch 
            UpdateBatch(BatchStatusCode:=m_sBatchStatus, TotalAmount:=crTotalAmount, TotalTransactions:=nTotalTransactions, RejectAmount:=crRejectAmount, RejectTransactions:=nRejectTransactions, ImportFilename:=WorkbookFile.FullName)
        End Try
    End Sub

    ''' <summary>
    ''' SkipProcessingRiskData
    ''' </summary>
    ''' <param name="oField"></param>
    ''' <param name="sPartyType"></param>
    ''' <param name="bSkip"></param>
    ''' <remarks></remarks>
    Private Sub SkipProcessingRiskData(ByVal oField As Xml.XmlElement, ByVal sPartyType As String, ByRef bSkip As Boolean)
        If oField.GetAttribute("DestMapping").StartsWith("/PolicyProcessRequestType/bt:CorporateClient") Then
            If sPartyType = "PC" Then
                bSkip = True
            Else
                bSkip = False
            End If
        ElseIf oField.GetAttribute("DestMapping").StartsWith("/PolicyProcessRequestType/bt:PersonalClient") Then
            If sPartyType = "CC" Then
                bSkip = True
            Else
                bSkip = False
            End If
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


    Private Function ValidateAgent(ByVal brokerCode As String, ByVal listOfErrors As listOfErrorsType) As Boolean

        Dim sqlScript As String
        Dim resultObject(,) As Object

        ValidateAgent = True

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

    Private Function GetAddressTypeID(ByVal v_sAddressTypeCode As String) As AddressTypeType
        Dim iRetVal As AddressTypeType

        Select Case v_sAddressTypeCode

            Case "3131 XBI"
                iRetVal = AddressTypeType.Item3131XBI
            Case "3131 XBA"
                iRetVal = AddressTypeType.Item3131XBA
            Case "3131 002"
                iRetVal = AddressTypeType.Item3131002
            Case "3131 XCO"
                iRetVal = AddressTypeType.Item3131XCO
            Case "3131 001"
                iRetVal = AddressTypeType.Item3131001
            Case "3131 XPR"
                iRetVal = AddressTypeType.Item3131XPR
            Case "3131 XRE"
                iRetVal = AddressTypeType.Item3131XRE
            Case "3131 XSA"
                iRetVal = AddressTypeType.Item3131XSA
            Case "3131 0XR"
                iRetVal = AddressTypeType.Item31310XR
        End Select

        Return iRetVal
    End Function

    ''' <summary>
    ''' ProcessPCDetails
    ''' </summary>
    ''' <param name="sParameterFullName"></param>
    ''' <param name="sCellValue"></param>
    ''' <param name="oPartyPC"></param>
    ''' <param name="oAddresses"></param>
    ''' <remarks></remarks>
    Private Sub ProcessPCDetails(ByVal sParameterFullName As String, ByVal sCellValue As String, ByRef oPartyPC As BasePartyPCType, ByRef oAddresses() As BaseAddressType)

        Dim sParameterName As String = ""
        Dim nIndex As Integer = 0

        If Not (sParameterFullName.Contains("/PolicyProcessRequestType/bt:PersonalClient/bt:Addresses/bt:")) Then
            nIndex = sParameterFullName.LastIndexOf("/bt:")
            sParameterName = sParameterFullName.Substring(nIndex + 4).ToString

            With oPartyPC
                Select Case (sParameterName)

                    Case "AccountExecutive"
                        .AccountExecutive = sCellValue

                    Case "AlternativeId"
                        .AlternativeId = sCellValue

                    Case "BranchCode"
                        .BranchCode = sCellValue

                    Case "Currency"
                        .Currency = sCellValue

                    Case "DateOfBirth"
                        .DateOfBirth = Convert.ToDateTime(sCellValue)

                    Case "EmployersBusinessCode"
                        .EmployersBusinessCode = sCellValue

                    Case "EmploymentStatusCode"
                        .EmploymentStatusCode = sCellValue

                    Case "FileCode"
                        .FileCode = sCellValue

                    Case "Forename"
                        .Forename = sCellValue

                    Case "GenderCode"
                        .GenderCode = sCellValue

                    Case "Initials"
                        .Initials = sCellValue

                    Case "MaritalStatusCode"
                        .MaritalStatusCode = sCellValue

                        .MaritalStatusCodeSpecified = True
                    Case "OccupationCode"
                        .OccupationCode = sCellValue

                    Case "Surname"
                        .Surname = sCellValue

                    Case "Title"
                        .Title = sCellValue


                    Case "TPIntroducer"
                        .TPIntroducer = sCellValue

                    Case "TPUserCode"
                        .TPUserCode = sCellValue

                    Case "NationalityCode"
                        .NationalityCode = sCellValue
                End Select

            End With

        ElseIf sParameterFullName.Contains("/PolicyProcessRequestType/bt:PersonalClient/bt:Addresses/bt:") Then
            nIndex = sParameterFullName.LastIndexOf("/bt:")
            sParameterName = sParameterFullName.Substring(nIndex + 4).ToString

            Dim iAddressIndex As Integer = 0

            If sParameterName.EndsWith("]") Then

                iAddressIndex = sParameterName.Substring(Len(sParameterName) - 2, 1)
                sParameterName = sParameterName.Substring(0, Len(sParameterName) - 3)



                If oAddresses.GetUpperBound(0) < iAddressIndex Then
                    ReDim Preserve oAddresses(iAddressIndex)
                    oAddresses(iAddressIndex) = New BaseAddressType
                End If
            End If


            With oAddresses(iAddressIndex)
                Select Case (sParameterName)

                    Case "AddressTypeCode"
                        If sCellValue = "" Then
                            .AddressTypeCode = AddressTypeType.Item3131XCO
                        Else
                            .AddressTypeCode = GetAddressTypeID(sCellValue)
                        End If
                    Case "AddressLine1"
                        .AddressLine1 = sCellValue

                    Case "AddressLine2"
                        .AddressLine2 = sCellValue

                    Case "AddressLine3"
                        .AddressLine3 = sCellValue

                    Case "AddressLine4"
                        .AddressLine4 = sCellValue

                    Case "CountryCode"
                        .CountryCode = sCellValue

                    Case "PostCode"
                        .PostCode = sCellValue
                End Select

            End With
        End If

    End Sub

    ''' <summary>
    ''' ProcessCCDetails
    ''' </summary>
    ''' <param name="sParameterFullName"></param>
    ''' <param name="sCellValue"></param>
    ''' <param name="oPartyCC"></param>
    ''' <param name="oAddresses"></param>
    ''' <remarks></remarks>
    Private Sub ProcessCCDetails(ByVal sParameterFullName As String, ByVal sCellValue As String, ByRef oPartyCC As BasePartyCCType, ByRef oAddresses() As BaseAddressType)
        Dim sParameterName As String = ""
        Dim nIndex As Integer = 0

        If Not (sParameterFullName.Contains("/PolicyProcessRequestType/bt:CorporateClient/bt:Addresses/bt:")) Then
            nIndex = sParameterFullName.LastIndexOf("/bt:")
            sParameterName = sParameterFullName.Substring(nIndex + 4).ToString

            With oPartyCC
                Select Case (sParameterName)

                    Case "AccountExecutive"
                        .AccountExecutive = sCellValue

                    Case "CompanyName"
                        .CompanyName = sCellValue

                    Case "BranchCode"
                        .BranchCode = sCellValue

                    Case "BusinessCode"
                        .BusinessCode = sCellValue

                    Case "MainContact"
                        .MainContact = sCellValue

                    Case "NumberOfEmployees"
                        .NumberOfEmployees = sCellValue

                    Case "FileCode"
                        .FileCode = sCellValue

                    Case "TPIntroducer"
                        .TPIntroducer = sCellValue

                    Case "TPUserCode"
                        .TPUserCode = sCellValue

                    Case "Currency"
                        .Currency = sCellValue
                End Select

            End With

        ElseIf sParameterFullName.Contains("/PolicyProcessRequestType/bt:CorporateClient/bt:Addresses/bt:") Then
            nIndex = sParameterFullName.LastIndexOf("/bt:")
            sParameterName = sParameterFullName.Substring(nIndex + 4).ToString

            With oAddresses(0)
                Select Case (sParameterName)

                    Case "AddressTypeCode"
                        .AddressTypeCode = AddressTypeType.Item3131XCO

                    Case "AddressLine1"
                        .AddressLine1 = sCellValue

                    Case "AddressLine2"
                        .AddressLine2 = sCellValue

                    Case "AddressLine3"
                        .AddressLine3 = sCellValue

                    Case "AddressLine4"
                        .AddressLine4 = sCellValue

                    Case "CountryCode"
                        .CountryCode = sCellValue

                    Case "PostCode"
                        .PostCode = sCellValue
                End Select

            End With
        End If

    End Sub

    ''' <summary>
    ''' ProcessPolicyData
    ''' </summary>
    ''' <param name="sParameterFullName"></param>
    ''' <param name="sCellValue"></param>
    ''' <param name="oPolicy"></param>
    ''' <remarks></remarks>
    Private Sub ProcessPolicyData(ByVal sParameterFullName As String, ByVal sCellValue As String, ByRef oPolicy As BaseQuoteRiskMsgType)
        Dim nIndex As Integer = 0
        Dim sParameterName As String = ""

        If (Not sParameterFullName.Contains("/PolicyProcessRequestType/bt:Policy/bt:Risk")) AndAlso (Not sParameterFullName.Contains("/PolicyProcessRequestType/bt:Policy/bt:Tax")) Then
            nIndex = sParameterFullName.LastIndexOf("/bt:")
            sParameterName = sParameterFullName.Substring(nIndex + 4).ToString
            With oPolicy

                Select Case (sParameterName)

                    Case "AlternateReference"
                        .AlternateReference = sCellValue

                    Case "PaymentTermCode"
                        .PaymentTermCode = sCellValue

                    Case "CollectionFrequencyCode"
                        .CollectionFrequencyCode = sCellValue

                    Case "AnalysisCode"
                        .AnalysisCode = sCellValue

                    Case "BranchCode"
                        .BranchCode = sCellValue

                    Case "BusinessTypeCode"
                        .BusinessTypeCode = sCellValue

                    Case "CoInsurancePlacement"
                        .CoInsurancePlacement = sCellValue

                    Case "CommissionRate"
                        .CommissionRate = Convert.ToDecimal(sCellValue)

                    Case "CommissionValue"
                        .CommissionValue = Convert.ToDecimal(sCellValue)

                    Case "CoverEndDate"
                        .CoverEndDate = Convert.ToDateTime(sCellValue)

                    Case "CoverStartDate"
                        .CoverStartDate = Convert.ToDateTime(sCellValue)

                    Case "CurrencyCode"
                        .CurrencyCode = sCellValue

                    Case "Description"
                        .Description = sCellValue

                    Case "InsuredName"
                        .InsuredName = sCellValue

                    Case "LastTransDescription"
                        .LastTransDescription = sCellValue

                    Case "MTAReasonCode"
                        .MTAReasonCode = sCellValue

                    Case "NewQuoteRef"
                        .NewQuoteRef = sCellValue

                    Case "OldPolicyNumber"
                        .OldPolicyNumber = sCellValue

                    Case "PolicyStatusCode"
                        .PolicyStatusCode = sCellValue

                    Case "ProductCode"
                        .ProductCode = sCellValue

                    Case "QuoteRef"
                        .QuoteRef = sCellValue

                    Case "TransactionTypeCode"
                        .TransactionTypeCode = sCellValue

                    Case "TransactionDueDate"
                        .TransactionDueDate = Convert.ToDateTime(sCellValue)

                    Case "UnderwritingYearCode"
                        .UnderwritingYearCode = sCellValue

                    Case "PolicyProcessType"
                        Dim ePolicyProcessType As PolicyProcessTypes
                        If String.IsNullOrEmpty(sCellValue) OrElse Not [Enum].TryParse(Of PolicyProcessTypes)(sCellValue, True, ePolicyProcessType) Then
                            ePolicyProcessType = PolicyProcessTypes.Bind
                        End If
                        .PolicyProcessType = ePolicyProcessType


                End Select
            End With

        End If

    End Sub

    ''' <summary>
    ''' ProcessRisksData
    ''' </summary>
    ''' <param name="sParameterFullName"></param>
    ''' <param name="sCellValue"></param>
    ''' <param name="nRiskCnt"></param>
    ''' <param name="orisk"></param>
    ''' <remarks></remarks>
    Private Sub ProcessRisksData(ByVal sParameterFullName As String, ByVal sCellValue As String, ByVal nRiskCnt As Integer, ByRef orisk() As BaseQuoteRiskMsgTypeRisks)
        Dim nIndex As Integer = 0
        Dim sParameterName As String = ""

        If sParameterFullName.Contains("/PolicyProcessRequestType/bt:Policy/bt:Risk") Then
            nIndex = sParameterFullName.LastIndexOf("/bt:")
            sParameterName = sParameterFullName.Substring(nIndex + 4).ToString

            With orisk(nRiskCnt)

                Select Case (sParameterName)

                    Case "DataModelCode"
                        .DataModelCode = sCellValue

                    Case "BranchCode"
                        .BranchCode = sCellValue

                    Case "RiskDescription"
                        .RiskDescription = sCellValue

                    Case "RiskFolderKey"
                        .RiskFolderKey = Convert.ToInt32(sCellValue)

                    Case "RiskTypeCode"
                        .RiskTypeCode = (sCellValue)

                    Case "RunDefaultRules"
                        .RunDefaultRules = Convert.ToBoolean(sCellValue)

                    Case "ScreenCode"
                        .ScreenCode = sCellValue

                    Case "XMLDataSet"
                        .XMLDataSet = sCellValue
                End Select

            End With

        End If
    End Sub

    ''' <summary>
    ''' ProcessTaxesandPBData
    ''' </summary>
    ''' <param name="xField"></param>
    ''' <param name="oPolicy"></param>
    ''' <param name="worksheet"></param>
    ''' <param name="rowCnt"></param>
    ''' <param name="RiskCnt"></param>
    ''' <param name="nPbCount"></param>
    ''' <param name="oPBData"></param>
    ''' <param name="oRiskTax"></param>
    ''' <param name="oPolicyTax"></param>
    ''' <param name="sCellValue"></param>
    ''' <remarks></remarks>
    Private Sub ProcessTaxesandPBData(ByVal xField As Xml.XmlElement, ByRef oPolicy As PolicyProcessCommand, ByVal worksheet As Worksheet, ByVal rowCnt As Integer, ByVal RiskCnt As Integer, ByRef nPbCount As Integer, ByRef oPBData() As BaseProductBuilderRiskType, ByRef oRiskTax() As BaseTaxesType, ByRef oPolicyTax() As BaseTaxesType, ByVal sCellValue As String)

        Dim crAmountorRate As Decimal = 0

        If xField.Attributes("DestMapping").Value.Contains("[PB]/DATA_SET/RISK_OBJECTS/") Then
            ReDim Preserve oPBData(nPbCount)
            Dim oProductBuilderData As BaseProductBuilderRiskTypeProductBuilderData
            oPBData(nPbCount) = New BaseProductBuilderRiskType()
            oProductBuilderData = New BaseProductBuilderRiskTypeProductBuilderData
            oProductBuilderData.ItemName = xField.Attributes("DestMapping").Value.Substring(4)
            oProductBuilderData.Value = sCellValue
            oPBData(nPbCount).ProductBuilderData = oProductBuilderData
            nPbCount = nPbCount + 1
        End If

        If xField.Attributes("Datatype").Value = "Tax" Then
            For Each item As Xml.XmlElement In xField.GetElementsByTagName("Tax")
                If item.Attributes("Type").Value = "Policy" Then
                    Dim iCount As Integer = 0
                    For Each TaxItem As Xml.XmlElement In item.GetElementsByTagName("TaxItem")
                        Dim thisCellName As String = ""
                        crAmountorRate = 0
                        ReDim Preserve oPolicyTax(iCount)
                        oPolicyTax(iCount) = New BaseTaxesType

                        thisCellName = TaxItem.Attributes("TaxBandCode").Value
                        oPolicyTax(iCount).TaxBandCode = GetCellvalue(thisCellName, worksheet, rowCnt)

                        thisCellName = TaxItem.Attributes("TaxAmount").Value

                        If GetCellvalue(thisCellName, worksheet, rowCnt) = String.Empty Then
                            crAmountorRate = 0
                        Else
                            crAmountorRate = GetCellvalue(thisCellName, worksheet, rowCnt)
                        End If

                        If TaxItem.Attributes("IsValue").Value = "true" Then
                            oPolicyTax(iCount).Amount = crAmountorRate
                        Else
                            oPolicyTax(iCount).TaxRate = crAmountorRate
                        End If
                        iCount += 1
                    Next

                ElseIf item.Attributes("Type").Value = "Risk" Then
                    Dim iCount As Integer = 0
                    For Each TaxItem As Xml.XmlElement In item.GetElementsByTagName("TaxItem")
                        Dim thisCellName As String = ""
                        crAmountorRate = 0
                        ReDim Preserve oRiskTax(iCount)
                        oRiskTax(iCount) = New BaseTaxesType

                        thisCellName = TaxItem.Attributes("TaxBandCode").Value
                        oRiskTax(iCount).TaxBandCode = GetCellvalue(thisCellName, worksheet, rowCnt)

                        thisCellName = TaxItem.Attributes("TaxAmount").Value

                        If GetCellvalue(thisCellName, worksheet, rowCnt) = String.Empty Then
                            crAmountorRate = 0
                        Else
                            crAmountorRate = GetCellvalue(thisCellName, worksheet, rowCnt)
                        End If

                        If TaxItem.Attributes("IsValue").Value = "true" Then
                            oRiskTax(iCount).Amount = crAmountorRate
                        Else
                            oRiskTax(iCount).TaxRate = crAmountorRate
                        End If
                        iCount += 1
                    Next

                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' ValidateBranch
    ''' </summary>
    ''' <param name="sbranchCode"></param>
    ''' <param name="UserName"></param>
    ''' <param name="listOfErrors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateBranch(ByVal sbranchCode As String, ByVal UserName As String, ByVal listOfErrors As listOfErrorsType) As Boolean

        Dim sSqlScript As String
        Dim oResultObject(,) As Object

        If sbranchCode = String.Empty Then
            Return PMEReturnCode.PMFalse
        End If
        sSqlScript = "SELECT COUNT(*) FROM   source s WHERE  source_id not IN (SELECT source_id FROM pmuser_source pms join Pmuser pmu on pms.user_id = pmu.user_id WHERE pmu.username = '" & UserName & "') AND is_deleted = 0 and code='" & sbranchCode & "'"

        Dim nReturn As Integer = m_oDatabase.SQLSelect(sSqlScript, "ValidateAgent", False, vResultArray:=oResultObject)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'ValidateBranch' with the following SQL - " & sSqlScript)
        End If

        If IsArray(oResultObject) Then
            If Convert.ToString(oResultObject(0, 0)) = "0" Then
                listOfErrors.Add("Import Failed because User " & UserName & " has no access on branch code " & sbranchCode)
                Return False
            End If
        End If
        Return True
    End Function

    ''' <summary>
    ''' Validate the Coinsurer and get the party_cnt.
    ''' </summary>
    ''' <param name="sparticipantcode"></param>
    ''' <param name="oListOfErrors"></param>
    ''' <param name="r_nPartycnt"></param>
    ''' <param name="nCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateCoInsurer(ByVal sparticipantcode As String, ByVal oListOfErrors As listOfErrorsType,
                                       ByRef r_nPartycnt As Integer, ByVal nCnt As Integer) As Boolean
        Dim sSqlScript As String = String.Empty
        Dim oResultObject(,) As Object

        Dim bResult As Boolean = True

        sSqlScript = "spu_ACT_Coinsurer_Party_Key_Sel"

        m_oDatabase.Parameters.Clear()
        AddParameterLite(m_oDatabase, "sShortCode", sparticipantcode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        Dim iReturn As Integer = m_oDatabase.SQLSelect(sSqlScript, "ValidateCoInsurer", True, vResultArray:=oResultObject)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'ValidateCoInsurer' with the following SQL - " & sSqlScript)
        End If

        If IsArray(oResultObject) Then
            r_nPartycnt = oResultObject(0, 0)
            If Convert.ToInt32(oResultObject(0, 0)) > 0 Then
                bResult = True
            End If
        Else
            bResult = False
            oListOfErrors.Add("The import of Row " & nCnt & " failed because the co-insurer code cannot be found")
        End If
        Return bResult
    End Function

    ''' <summary>
    ''' ValidateContacts
    ''' </summary>
    ''' <param name="sContactType"></param>
    ''' <param name="sContactNumber"></param>
    ''' <param name="oListOfErrors"></param>
    ''' <param name="nCnt"></param>
    ''' <param name="oWorksheet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateContacts(ByVal sContactType As String, ByVal sContactNumber As String,
                                      ByVal oListOfErrors As listOfErrorsType,
                                      ByVal nCnt As Integer,
                                      ByVal oWorksheet As Worksheet) As Boolean
        Dim sSqlScript As String = String.Empty
        Dim oResultObject(,) As Object = Nothing

        Dim bResult As Boolean = True
        If String.IsNullOrEmpty(sContactType) Then
            ValidateContacts = False
            oListOfErrors.Add("The policy at row " & nCnt & " failed because the contact type code is missing from the input.")
        ElseIf String.IsNullOrEmpty(sContactNumber) Then
            ValidateContacts = False
            oListOfErrors.Add("The policy at row " & nCnt & " failed because the number is missing from the input.")
        Else
            sSqlScript = "spu_ACT_Contact_Type_Sel"

            m_oDatabase.Parameters.Clear()
            AddParameterLite(m_oDatabase, "sContactType", sContactType, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            Dim iReturn As Integer = m_oDatabase.SQLSelect(sSqlScript, "ValidateContacts", True, vResultArray:=oResultObject)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'ValidateContacts' with the following SQL - " & sSqlScript)
            End If

            If Not IsArray(oResultObject) Then
                bResult = False
                oListOfErrors.Add("The import of Row " & nCnt & " failed because the contact code '" & sContactType & "' cannot be found.")
            End If
        End If
        Return bResult
    End Function

    ''' <summary>
    ''' ValidateCoInsurerFields
    ''' </summary>
    ''' <param name="oCoInsurer"></param>
    ''' <param name="nCnt"></param>
    ''' <param name="oListOfErrors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateCoInsurerFields(ByVal oCoInsurer As List(Of BaseUpdateCoinsuranceValuesRequestTypeRow),
                                             ByVal nCnt As Integer, ByVal oListOfErrors As listOfErrorsType) As Boolean
        Dim bResult As Boolean = True
        Dim dSharePercent As Double = 0
        Dim iTotalCount As Integer = 0
        Dim iDistinctCount As Integer = 0
        For Each CoInsurer As BaseUpdateCoinsuranceValuesRequestTypeRow In oCoInsurer
            dSharePercent += SharedFiles.ToSafeDouble(CoInsurer.SharePerc)
            If (SharedFiles.ToSafeDouble(CoInsurer.CommissionPerc) > 100) Then
                oListOfErrors.Add("The import of Row " & nCnt & " failed because the commission percentage for co-insurer is greater than 100%.")
                bResult = False
            End If
        Next
        iTotalCount = oCoInsurer.Count
        iDistinctCount = oCoInsurer.Select(Function(x) x.CoInsurerKey).Distinct().Count

        If iTotalCount <> iDistinctCount Then
            oListOfErrors.Add("The import of Row " & nCnt & " failed because the insurer code already have a share.")
            ValidateCoInsurerFields = False
        End If

        If SharedFiles.ToSafeInteger(dSharePercent) <> 100 Then
            oListOfErrors.Add("The import of Row " & nCnt & " failed because the total share percentage for co-insurers is not equals to 100%.")
            bResult = False
        End If
        Return bResult
    End Function

    ''' <summary>
    ''' ProcessCoInsurers
    ''' </summary>
    ''' <param name="xField"></param>
    ''' <param name="oCoInsurer"></param>
    ''' <param name="oListOfErrors"></param>
    ''' <param name="nCnt"></param>
    ''' <param name="oWorksheet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessCoInsurers(ByVal xField As Xml.XmlElement,
                                       ByRef oCoInsurer() As BaseUpdateCoinsuranceValuesRequestTypeRow,
                                       ByRef oListOfErrors As listOfErrorsType,
                                       ByVal nCnt As Integer,
                                       ByVal oWorksheet As Worksheet) As Boolean
        Dim bResult As Boolean = True
        If xField.Attributes("Datatype").Value = "CoInsurers" Then
            For Each item As Xml.XmlElement In xField.GetElementsByTagName("CoInsurers")
                Dim nCount As Integer = 0
                Dim nInsurerKey As Integer = 0
                For Each CoInsurerItem As Xml.XmlElement In item.GetElementsByTagName("CoInsurer")
                    If ValidateCoInsurer(GetCellvalue(SharedFiles.ToSafeString(GetAttribute(CoInsurerItem, "Co-InsCode")), oWorksheet, nCnt), oListOfErrors, nInsurerKey, nCnt) Then
                        ReDim Preserve oCoInsurer(nCount)
                        oCoInsurer(nCount) = New BaseUpdateCoinsuranceValuesRequestTypeRow
                        oCoInsurer(nCount).CoInsurerKey = nInsurerKey
                        If Not String.IsNullOrEmpty(GetCellvalue(SharedFiles.ToSafeString(GetAttribute(CoInsurerItem, "Co-InsShare")), oWorksheet, nCnt)) Then
                            oCoInsurer(nCount).SharePerc = SharedFiles.ToSafeDouble(GetCellvalue(SharedFiles.ToSafeString(GetAttribute(CoInsurerItem, "Co-InsShare")), oWorksheet, nCnt))
                        Else
                            oListOfErrors.Add("The import of Row " & nCnt & " failed because the co-insurer share percent cannot be blank")
                        End If
                        oCoInsurer(nCount).CommissionPerc = SharedFiles.ToSafeDouble(GetCellvalue(SharedFiles.ToSafeString(GetAttribute(CoInsurerItem, "Co-InsComm")), oWorksheet, nCnt))
                        nCount += 1
                    Else
                        bResult = False
                        Exit For
                    End If
                Next
            Next
        End If
        Return bResult
    End Function

    ''' <summary>
    ''' ProcessContacts
    ''' </summary>
    ''' <param name="xField"></param>
    ''' <param name="oContact"></param>
    ''' <param name="oListOfErrors"></param>
    ''' <param name="nCnt"></param>
    ''' <param name="oWorksheet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessContacts(ByVal xField As Xml.XmlElement,
                                     ByRef oContact() As BaseContactType,
                                     ByRef oListOfErrors As listOfErrorsType,
                                     ByVal nCnt As Integer,
                                     ByVal oWorksheet As Worksheet) As Boolean
        Dim bResult As Boolean = True
        If xField.Attributes("Datatype").Value = "Contacts" Then
            For Each item As Xml.XmlElement In xField.GetElementsByTagName("Contacts")
                Dim iCount As Integer = 0
                Dim oContactDetail As BaseContactDetailType = Nothing
                For Each xeContactItem As Xml.XmlElement In item.GetElementsByTagName("Contact")
                    Dim sContactNumber As String = GetCellvalue(SharedFiles.ToSafeString(GetAttributeValue(xeContactItem, "ContactNumber")), oWorksheet, nCnt)
                    Dim sContactType As String = GetCellvalue(SharedFiles.ToSafeString(GetAttributeValue(xeContactItem, "ContactType")), oWorksheet, nCnt).Trim.ToUpper

                    If ValidateContacts(sContactType, sContactNumber, oListOfErrors, nCnt, oWorksheet) Then
                        oContactDetail = New BaseContactDetailType
                        ReDim Preserve oContact(iCount)
                        oContact(iCount) = New BaseContactType

                        oContactDetail.Item = sContactNumber

                        Select Case sContactType
                            Case "E-MAIL", "EMAIL"
                                oContact(iCount).ContactTypeCode = ContactTypeType.EMAIL
                                oContactDetail.ItemElementName = ItemChoiceType.EmailAddress
                            Case "TELEPHONE", "HOMEPHONE"
                                oContact(iCount).ContactTypeCode = ContactTypeType.HOMEPHONE
                                oContactDetail.ItemElementName = ItemChoiceType.Number
                            Case "MOBILE"
                                oContact(iCount).ContactTypeCode = ContactTypeType.MOBILE
                                oContactDetail.ItemElementName = ItemChoiceType.Number
                            Case "FAX"
                                oContact(iCount).ContactTypeCode = ContactTypeType.FAX
                                oContactDetail.ItemElementName = ItemChoiceType.Number
                            Case "WEB"
                                oContact(iCount).ContactTypeCode = ContactTypeType.WEB
                                oContactDetail.ItemElementName = ItemChoiceType.EmailAddress
                            Case "MAIN"
                                oContact(iCount).ContactTypeCode = ContactTypeType.MAIN
                                oContactDetail.ItemElementName = ItemChoiceType.EmailAddress
                            Case "MEMAIL"
                                oContact(iCount).ContactTypeCode = ContactTypeType.MAINEMAILCONTACT
                                oContactDetail.ItemElementName = ItemChoiceType.EmailAddress
                        End Select
                        oContact(iCount).ContactDetail = oContactDetail

                        oContact(iCount).AreaCode = GetCellvalue(SharedFiles.ToSafeString(GetAttributeValue(xeContactItem, "AreaCode")), oWorksheet, nCnt)
                        iCount += 1
                    Else
                        bResult = False
                        Exit For
                    End If
                Next
            Next
        End If
        Return bResult
    End Function

    ''' <summary>
    ''' get the attribute value from excel file
    ''' </summary>
    ''' <param name="oElement"></param>
    ''' <param name="sAttributeName"></param>
    ''' <returns></returns>
    ''' <remarks>WPR 11</remarks>
    Private Function GetAttributeValue(ByRef oElement As XmlElement, ByVal sAttributeName As String) As String
        If oElement.Attributes(sAttributeName) IsNot Nothing Then
            Return oElement.Attributes(sAttributeName).Value
        Else
            Return String.Empty
        End If
    End Function
#End Region
#Region "Creator"
    Public Sub New(ByVal oXML As XmlDocument)
        MyBase.New(oXML)
    End Sub
#End Region
End Class
