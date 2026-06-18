Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.IO
Imports System.Xml.Serialization
Imports Aspose.Cells
Imports Sirius.SBO.Import.Excel_Import_Library
Imports System.Configuration
Imports SharedFiles

Friend NotInheritable Class Premium_BDX_Import : Inherits ImportBase

#Region "Private variables"

    Private m_sImportedPath As String = String.Empty
    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer
    Private m_sBatchStatus As String
    Public Const KSAMBDXCalling As Integer = -88
    Private Class PoliciesAllocationType
        Private amountTobeAllocatedField As Decimal = 0
        Private documentRefField As String = String.Empty
        Private insuranceFileKeyField As Integer = 0
        Private writeOffReasonKeyField As Integer
        Private writeOffAmountField As Decimal
        Private TransDetailIDField As Integer

        Public Property AmountTobeAllocated() As Decimal
            Get
                Return amountTobeAllocatedField
            End Get
            Set(ByVal value As Decimal)
                amountTobeAllocatedField = value
            End Set
        End Property
        Public Property DocumentRef() As String
            Get
                Return documentRefField
            End Get
            Set(ByVal value As String)
                documentRefField = value
            End Set
        End Property
        Public Property InsuranceFileKey() As Integer
            Get
                Return insuranceFileKeyField
            End Get
            Set(ByVal value As Integer)
                insuranceFileKeyField = value
            End Set
        End Property

        Public Property WriteOffReasonKey() As Integer
            Get
                Return Me.writeOffReasonKeyField
            End Get
            Set(ByVal value As Integer)
                Me.writeOffReasonKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property WriteOffAmount() As Decimal
            Get
                Return Me.writeOffAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.writeOffAmountField = value
            End Set
        End Property

         Public Property TransDetailID() As Integer
            Get
                Return Me.TransDetailIDField
            End Get
            Set(ByVal value As Integer)
                Me.TransDetailIDField = value
            End Set
        End Property



    End Class

#End Region

#Region "Public Properties"
    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "BDXPREM"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Bordereaux Premium Import"
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

    ' BuildPolicyProcessRequest - commented out, empty stub referencing WCF types not used in Premium import
    ' Private Sub BuildPolicyProcessRequest(ByRef PolicyProcessRequest As PureMessagingService.PolicyProcessRequestType, ByRef PolicyProcessResponse As PureMessagingService.PolicyProcessResponseType, ByVal matchFound As Boolean, ByRef riskCnt As Integer)
    '
    ' End Sub

    ' This runs the custom Premium SQL as specified in the Configuration file
          Private Sub LookupPremium(ByVal PremiumMatchingElement As Xml.XmlElement, ByRef PremiumAllocations As List(Of PoliciesAllocationType), ByVal Worksheet As Worksheet, ByVal RowCnt As Integer, ByVal ListOfErrors As listOfErrorsType, ByVal CertRef As String, ByRef RunningTotal As Double, ByVal Username As String) 

        ' Retireve the SQL Element
        Static originalSqlScript As String = String.Empty
        Static sqlScriptLocation As String = PremiumMatchingElement.GetElementsByTagName("SQL").Item(0).InnerText.ToString

        If PremiumMatchingElement Is Nothing OrElse Worksheet Is Nothing Then
            Return
        End If

        ' If we don't already have it read in the SQL file
        If originalSqlScript = String.Empty Then
            sqlScriptLocation = PremiumMatchingElement.GetElementsByTagName("SQL").Item(0).InnerText.ToString
            If Dir(sqlScriptLocation) = String.Empty Then

            Else
                Dim sr As StreamReader = File.OpenText(sqlScriptLocation) 
                originalSqlScript = sr.ReadToEnd()
            End If
        End If

        ' Get all of the parameters defined in the configuration file
        Dim parametersElement As XmlElement = PremiumMatchingElement.GetElementsByTagName("Parameters").Item(0) 

        ' Setup the parameter array for the string format.  The number of parameters in the SQL has to 
        ' match the number of parameters in the condif.
        Dim sqlParamArray(parametersElement.GetElementsByTagName("Parameter").Count - 1) As Object 

        ' For each parameter in the config go and fetch the value out of the spreadsheet
        For Each parameter As Xml.XmlElement In PremiumMatchingElement.Item("Parameters").GetElementsByTagName("Parameter")

            Dim thisCellName As String = parameter.Attributes(STR_SourceValue).Value 
            Dim thisCellPosition As Integer = Convert.ToInt32(parameter.Attributes(STR_Position).Value) 
            Dim thisCell As Cell 
            Dim thisRowCellValue As Object = 2 

            If thisCellName.StartsWith("$") Then
                thisCellName = thisCellName.Substring(1).Trim & (RowCnt).ToString
                thisCell = Worksheet.Cells(thisCellName)
                If thisCell IsNot Nothing Then
                    thisRowCellValue = thisCell.Value
                End If
            Else
                thisRowCellValue = thisCellName
            End If

            If thisRowCellValue IsNot Nothing Then
                ' Check the validity of the data item before passing it into the SQL
                If ValidateAndConvertDatatype(parameter, thisRowCellValue, thisCellName, ListOfErrors, thisCellName, RowCnt) = False Then
                Else
                    If thisCellPosition <= sqlParamArray.GetUpperBound(0) Then
                        ' Add the value into the correct location in the string paramarry
                        If parameter.Attributes("Description").Value = "UserName" Then
                            sqlParamArray(thisCellPosition) = Username
                        Else
                            sqlParamArray(thisCellPosition) = thisRowCellValue
                        End If
                    End If
                End If
            End If

        Next

        ' Use string format to to set the parameters in the SQL string
        Dim sqlScript As String = [String].Format(originalSqlScript, sqlParamArray) 

        Dim resultObject(,) As Object = Nothing 
        ' Execute sql
        Dim iReturn As Integer = m_oDatabase.SQLSelect(sqlScript, "BDX Lookup Premium", False, vResultArray:=resultObject) 
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'BDX Lookup Premium' with the following SQL - " & sqlScript)
        End If

        If IsArray(resultObject) Then
            Dim resultArray As System.Array = resultObject

            If PremiumAllocations Is Nothing Then
                PremiumAllocations = New List(Of PoliciesAllocationType)
            End If


            If resultArray.Rank = 2 AndAlso resultArray.GetUpperBound(0) > 6 Then
                For icnt As Integer = resultArray.GetLowerBound(1) To resultArray.GetUpperBound(1)
                    ' If we have an error then 
                    If resultArray(6, 0).ToString <> String.Empty Then
                        ListOfErrors.Add("The import of Row " & RowCnt & "  failed. " & resultArray(6, 0).ToString, "", RowCnt)
                    Else

                        Dim oPoliciesAllocationType = New PoliciesAllocationType


                        oPoliciesAllocationType.AmountTobeAllocated = resultArray(0, icnt)
                        oPoliciesAllocationType.DocumentRef = resultArray(1, icnt)
                        oPoliciesAllocationType.InsuranceFileKey = resultArray(2, icnt)
                        RunningTotal = Convert.ToDouble(resultArray(7, icnt))
                        If resultArray(4, 0).ToString <> String.Empty AndAlso resultArray(4, 0) <> 0 Then
                            oPoliciesAllocationType.WriteOffAmount = resultArray(5, icnt)
                        End If

                        If resultArray.GetUpperBound(0) > 7 Then
                            oPoliciesAllocationType.TransDetailID = resultArray(8, icnt)
                        End If


                        PremiumAllocations.Add(oPoliciesAllocationType)
                    End If

                Next

            Else
                ListOfErrors.Add("The results returned from the Premium matching SQL did not return the correct number of items in the select list.  Please review the SQL contained in the file - " & PremiumMatchingElement.GetElementsByTagName("SQL").Item(0).InnerText.ToString)
            End If


        End If

    End Sub


    Private Function CreateManualAllocation(ByVal oPremiumAllocations As List(Of PoliciesAllocationType),
                                        ByVal iTransDetailKey As Integer,
                                        ByVal iCashListKey As String,
                                        ByRef oAllocationManual As bACTAllocationManual.Business) As Integer



        Dim vKeys(,) As Object
        Dim vResultArray(,) As Object
        Dim vMatchTrans(oPremiumAllocations.Count - 1) As String
        Dim dNetAllocatedAmount As Double
        Dim dWriteOffAmount As Double
        Dim iWriteOffReasonKey As Integer
        ' Build match transaction array
        For lMatchRow As Integer = 0 To oPremiumAllocations.Count - 1
            vMatchTrans(lMatchRow) = gPMFunctions.ToSafeString(oPremiumAllocations(lMatchRow).TransDetailID) & "|" & gPMFunctions.ToSafeDouble(oPremiumAllocations(lMatchRow).AmountTobeAllocated)
            dNetAllocatedAmount = dNetAllocatedAmount + gPMFunctions.ToSafeDouble(oPremiumAllocations(lMatchRow).AmountTobeAllocated)

            dWriteOffAmount = dWriteOffAmount + gPMFunctions.ToSafeDouble(oPremiumAllocations(lMatchRow).WriteOffAmount)

            If oPremiumAllocations(lMatchRow).WriteOffReasonKey > 0 Then

                iWriteOffReasonKey = oPremiumAllocations(lMatchRow).WriteOffReasonKey
            End If

        Next


        ' Initialize the keys array with appropriate dimensions
        If dWriteOffAmount <> 0 Then
            ReDim vKeys(1, 6)

        Else
            ReDim vKeys(1, 3)
        End If
        Dim bCurrencyWriteOff As Boolean = False
        ' Populate key names
        vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID
        vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs
        vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameCashListItemId

        ' Populate key values
        vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = gPMFunctions.ToSafeString(iTransDetailKey) & "|" & (dNetAllocatedAmount * -1)
        vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans
        vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = iCashListKey

        If dWriteOffAmount <> 0 Then
            ' Write Off Reason

            vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameWriteOffReasonId

            vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = iWriteOffReasonKey

            'WriteOff difference

            vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameWriteOffAmount

            vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = IIf(bCurrencyWriteOff, 0, dWriteOffAmount)

            'Currency difference

            vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.ACTKeyNameCurrencyDifference

            vKeys(SharedFiles.gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = IIf(bCurrencyWriteOff, dWriteOffAmount, 0)
        End If

        Dim m_lReturn As Integer

        m_lReturn = oAllocationManual.SetProcessModes(vTask:=SharedFiles.gPMConstants.PMEComponentAction.PMEdit)
        If m_lReturn <> SharedFiles.gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(m_lReturn.ToString() + ", CreateManualAllocation, m_oAllocationManual.SetProcessModes failed.")
        End If

        ' Set the keys
        m_lReturn = oAllocationManual.SetKeys(vKeyArray:=vKeys)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(m_lReturn.ToString() + ", CreateManualAllocation, " + "m_oAllocationManual.SetKeys " &
                               "failed.")
        End If

        ' Start it
        m_lReturn = oAllocationManual.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(m_lReturn.ToString() + ", CreateManualAllocation, " + "m_oAllocationManual.Start " &
                               "failed.")
        End If

        Dim oInsFileCntList As New List(Of Integer)

        For iTmpCnt As Integer = 0 To oPremiumAllocations.Count - 1
            If Not oInsFileCntList.Contains(oPremiumAllocations(iTmpCnt).InsuranceFileKey) Then
                oInsFileCntList.Add(oPremiumAllocations(iTmpCnt).InsuranceFileKey)

                With m_oDatabase
                    .Parameters.Clear()
                    .Parameters.Add("insurance_file_cnt", CStr(oPremiumAllocations(iTmpCnt).InsuranceFileKey), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("cashlistitem_id", CStr(iCashListKey), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    'Developer Guide No. 39
                    m_lReturn = .SQLAction("spu_ACT_Update_PolicyCashListItem", "Update_PolicyCashListItem", True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, spu_ACT_Update_PolicyCashListItem failed.")
                    End If
                End With
            End If
        Next

        Return m_lReturn

    End Function


    ''' <summary>
    ''' ProcessImport
    ''' </summary>
    ''' <param name="ConfigFile"></param>
    ''' <param name="WorkbookFile"></param>
    ''' <remarks></remarks>
    Friend Sub ProcessImport(ByVal ConfigFile As FileInfo, ByVal WorkbookFile As FileInfo)

        Dim fsFileStream As FileStream = Nothing
        Dim xdocXmlDoc As New Xml.XmlDocument
        Dim errorWorkbook As Workbook
        Dim sErrorWorkbookFilename As String = String.Empty
        Dim sBrokerCode As String = String.Empty
        Dim crTotalAmount As Decimal = 0
        Dim nTotalTransactions As Integer = 0
        Dim crRejectAmount As Decimal = 0
        Dim nRejectTransactions As Integer = 0
        Dim nTotalPolicies As Integer = 0
        Dim sAccountSortCode As String = ""
        Dim bManualAllocation As Boolean = False
        Dim m_lReturn As Object
        ' Define the REST API client

        Dim _apiClient As ApiClient

        Try
            OutputLine("Creating REST API client...")
            Dim restApiUrl As String = ConfigurationManager.AppSettings("RestAPIUrl")
            If String.IsNullOrWhiteSpace(restApiUrl) Then
                Throw New ConfigurationErrorsException("Missing required appSetting 'RestAPIUrl'.")
            End If
            _apiClient = New ApiClient(restApiUrl)

            OutputLine("Validating XML mapping...")
            m_oXML.Validate()

            OutputLine("Creating Batch...")
            CreateBatch()

            xdocXmlDoc.Load(ConfigFile.FullName)

            'Creating a file stream containing the Excel file to be opened
            fsFileStream = New FileStream(WorkbookFile.FullName, FileMode.Open)

            'Instantiate a Workbook object that represents the existing Excel file
            Dim workbook As Workbook = New Workbook(fsFileStream)

            ' Retrieve the Import Header Element
            Dim workbookElement As Xml.XmlElement = xdocXmlDoc.Item(STR_ImportHeader)

            sBrokerCode = workbookElement.Attributes(STR_Broker_Code).Value

            OutputLine("Processing data file...")
            ' Loop around the WorkSheet elements in the Configuration File
            For Each worksheetElement As Xml.XmlElement In workbookElement.ChildNodes

                ' Check the name of the child element identifies it as a WorkSheet
                If worksheetElement.Name = STR_Worksheet Then

                    ' Get the Worksheet name
                    Dim worksheetName As String = worksheetElement.Attributes(STR_Name).Value

                    Dim license As License = New License()

                    'Set the license of Aspose.Cells to avoid the evaluation limitations
                    license.SetLicense("Aspose.Total.lic")

                    'Instantiate a new Workbook object that represents the existing Excel file
                    Dim worksheet As Worksheet = workbook.Worksheets(worksheetName)

                    ' Create an Error file ready to report any errors that may occur
                    sErrorWorkbookFilename = WorkbookFile.Directory.FullName & "\" & WorkbookFile.Name.Replace(WorkbookFile.Extension, " - Errors(" & Now.ToFileTime & ")" & WorkbookFile.Extension)
                    errorWorkbook = New Workbook
                    Dim errorworksheet As Worksheet = errorWorkbook.Worksheets(0)
                    errorworksheet.Name = worksheetName

                    ' Grab the child Elemenets for this worksheet from the configuration file
                    Dim xeMappingElement As Xml.XmlElement = worksheetElement.Item("Mapping")
                    Dim xeRowSortingElement As Xml.XmlElement = worksheetElement.Item("RowSorting")
                    Dim xeRowMappingElement As Xml.XmlElement = worksheetElement.Item("RowMapping")
                    Dim xePremiumMappingElement As Xml.XmlElement = worksheetElement.Item("PremiumMatching")
                    Dim nStartRow As Integer = CInt(xeMappingElement.Attributes("StartingRow").Value)
                    Dim sCertRefColumn As String = xeMappingElement.Attributes(STR_Certificate_Ref_Column).Value
                    Dim sCoverHolderNameColumn As String = xeMappingElement.Attributes(STR_CoverHoldername_Column).Value
                    sCertRefColumn = sCertRefColumn.Substring(1).Trim()
                    sCoverHolderNameColumn = sCoverHolderNameColumn.Substring(1).Trim()

                    bManualAllocation = ToSafeBoolean(xeMappingElement.Attributes("ManualAllocation").Value)

                    Dim oAllocationManual As bACTAllocationManual.Business

                    If bManualAllocation Then
                        oAllocationManual = New bACTAllocationManual.Business
                        m_lReturn = oAllocationManual.Initialise(sUsername:="", sPassword:="", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=26, iLogLevel:=PMELogLevel.PMLogError, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create object, bACTAllocationManual.Business")
                        End If
                    End If



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

                    ' Validate the header structure
                    If (HeaderStructureIsValid(worksheetElement, worksheet, oGlobalListOfErrors) = False) Then
                        OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, 0, oGlobalListOfErrors, "Premium_BDX_Import", , sBrokerCode)
                    Else
                        'Obtain the DataSorter object in the workbook
                        Dim sorter As DataSorter = workbook.DataSorter

                        If xeRowSortingElement.Attributes("AmountLocation") IsNot Nothing Then
                            Dim sAmountRow As String = xeRowSortingElement.Attributes("AmountLocation").Value.ToString
                            sAmountRow = sAmountRow.Substring(1) & nStartRow.ToString

                            Dim nPaymentReceiptColumn As Integer = worksheet.Cells.MaxDataColumn + 1
                            'Inserting a column into the worksheet at 2nd position
                            worksheet.Cells.InsertColumn(nPaymentReceiptColumn)
                            worksheet.Cells(nStartRow - 1, nPaymentReceiptColumn).SetSharedFormula("=IF(" & sAmountRow & "<0,1,0)", worksheet.Cells.MaxDataRow - nStartRow + 2, 1)

                            sorter.AddKey(nPaymentReceiptColumn, SortOrder.Descending)

                            ' Calculate the newly added formula
                            workbook.CalculateFormula()

                        End If

                        ' Sort the Spreadsheet
                        For Each xeSortField As XmlElement In xeRowSortingElement
                            If xeSortField.Attributes IsNot Nothing Then

                                Dim sLocation As String = xeSortField.Attributes("Location").Value.ToString
                                Dim bAscending As Boolean = CBool(xeSortField.Attributes("Ascending").Value.ToString)
                                sLocation = sLocation.Substring(1) & nStartRow.ToString

                                Dim iColIndex As Integer = worksheet.Cells.Item(sLocation).Column
                                Dim thisSortOrder As SortOrder = IIf(bAscending = True, SortOrder.Ascending, SortOrder.Descending)

                                sorter.AddKey(iColIndex, thisSortOrder)

                            End If
                        Next

                        'Create a cells area (range).
                        Dim ca As New CellArea()
                        'Specify the start row index.
                        ca.StartRow = nStartRow - 1
                        'Specify the start column index.
                        ca.StartColumn = 0
                        'Specify the last row index.
                        ca.EndRow = worksheet.Cells.MaxDataRow
                        'Specify the last column index.
                        ca.EndColumn = worksheet.Cells.MaxDataColumn

                        'Sort data in the specified data range (A2:C10)
                        sorter.Sort(worksheet.Cells, ca)

                        ' Setup the new Request structure
                        Dim createReceiptCashListWithItemsRequestType As CreateReceiptCashListWithItemsCommand = New CreateReceiptCashListWithItemsCommand
                        Dim createReceiptCashListWithItemsResponseType As CreateReceiptCashListWithItemsCommandResponse = New CreateReceiptCashListWithItemsCommandResponse
                        ' Setup the new Request structure
                        Dim createPaymentCashListWithItemsRequestType As CreatePaymentCashListWithItemsCommand = New CreatePaymentCashListWithItemsCommand
                        Dim createPaymentCashListWithItemsResponseType As CreatePaymentCashListWithItemsCommandResponse = New CreatePaymentCashListWithItemsCommandResponse

                        Dim oCashListItemTypeRTPolicies() As BaseReceiptCashListItemTypePolicies
                        Dim oCashListItemTypePTPolicies() As BasePaymentCashListItemTypePolicies

                        Dim crPolicyRunningTotal As Decimal = 0
                        Dim oPremiumAllocations As List(Of PoliciesAllocationType)

                        ' For each Row in the worksheet
                        For iCnt As Integer = nStartRow To worksheet.Cells.MaxDataRow + 1

                            Dim listOfErrors As New listOfErrorsType

                            bValueIsValid = True
                            nTotalTransactions = nTotalTransactions + 1
                            Dim xnsmNsMgr As New Xml.XmlNamespaceManager(xdocXmlDoc.NameTable)
                            xnsmNsMgr.AddNamespace("ab", "http://www.siriusfs.com/SFI/Import/Premium_BDX_Import/20051005")

                            ' Loop around each field in the configuration mapping 
                            Dim xnField As Xml.XmlNode = Nothing
                            Dim oCellValue As Object = Nothing
                            Dim sDatatype As String = String.Empty

                            Dim sCashListItemBranchCodeValue As String = String.Empty
                            Dim sCashListBankAccountCodeValue As String = String.Empty
                            Dim sCashListCurrencyCodeValue As String = String.Empty
                            Dim sCashListInsuranceRefValue As String = String.Empty
                            Dim dCashListAmount As Double = 0
                            Dim sCashListReferenceValue As String = String.Empty
                            Dim sCashListStatusCodeValue As String = String.Empty
                            Dim sCashListTypeCodeValue As String = String.Empty
                            Dim sMediaTypeCodeValue As String = String.Empty
                            Dim sBankReference As String = String.Empty
                            Dim sOurReference As String = String.Empty
                            Dim sMediaReference As String = String.Empty
                            Dim sTheirReference As String = String.Empty
                            Dim sTransDate As String = String.Empty

                            Dim dNextCashListAmount As Double = 0
                            Dim sCertRef As String = worksheet.Cells(sCertRefColumn & iCnt.ToString).Value
                            Dim sCoverHolderName As String = worksheet.Cells(sCoverHolderNameColumn & iCnt.ToString).Value

                            nTotalTransactions = nTotalTransactions + 1
                            Dim dtTransDate As DateTime = Date.MinValue
                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='CashListItemBranchCode']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sCashListItemBranchCodeValue = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='CashListBankAccountCode']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sCashListBankAccountCodeValue = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='CashListSettlementCurrency']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sCashListCurrencyCodeValue = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='CashListInsuranceRef']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sCashListInsuranceRefValue = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='CashListAmount']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                dCashListAmount = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='CashListReference']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sCashListReferenceValue = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='CashListStatusCode']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sCashListStatusCodeValue = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='CashListTypeCode']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sCashListTypeCodeValue = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='MediaTypeCode']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sMediaTypeCodeValue = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='MediaReference']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sMediaReference = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='OurReference']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sOurReference = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='TheirReference']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sTheirReference = oCellValue
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='BankReference']", xnsmNsMgr)
                            ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                            If (bValueIsValid) Then
                                ' Process the value and map into the internal structure
                                sBankReference = oCellValue
                            End If

                            'set AccountShortCode if the same is mapped from excel file
                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='AccountShortCode']", xnsmNsMgr)

                            sAccountSortCode = sBrokerCode

                            If Not xnField Is Nothing Then
                                ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                                If (bValueIsValid = True) Then
                                    ' Process the value and map into the internal structure
                                    sAccountSortCode = oCellValue
                                End If
                            End If

                            xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='TransDate']", xnsmNsMgr)
                            If Not xnField Is Nothing Then
                                ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                                If (bValueIsValid) Then
                                    ' Process the value and map into the internal structure
                                    dtTransDate = oCellValue
                                End If
                            End If

                            If dtTransDate = Date.MinValue Then
                                dtTransDate = Date.Now
                            End If

                            'Get Amount for the next row
                            If iCnt < worksheet.Cells.MaxDataRow + 1 Then

                                xnField = xeMappingElement.SelectSingleNode("./ab:Field[@DestMapping='CashListAmount']", xnsmNsMgr)
                                ExtractAndValidateValue(xnsmNsMgr, worksheet, iCnt + 1, listOfErrors, xnField, bValueIsValid, oCellValue, sDatatype, m_oDatabase)
                                If (bValueIsValid = True) Then
                                    ' Process the value and map into the internal structure
                                    dNextCashListAmount = oCellValue
                                End If

                            End If

                            
                            crPolicyRunningTotal = dCashListAmount
                            ' Use the custom XML to match the policy record
                            LookupPremium(xePremiumMappingElement, oPremiumAllocations, worksheet, iCnt, listOfErrors, sCashListInsuranceRefValue, crPolicyRunningTotal, UserName)

                            If oPremiumAllocations IsNot Nothing And listOfErrors.Count = 0 Then
                                For Each oPrem As Premium_BDX_Import.PoliciesAllocationType In oPremiumAllocations
                                    If crPolicyRunningTotal > 0 Then
                                        If oPrem.AmountTobeAllocated = crPolicyRunningTotal Then
                                            ReDim Preserve oCashListItemTypeRTPolicies(nTotalPolicies)
                                            oCashListItemTypeRTPolicies(nTotalPolicies) = New BaseReceiptCashListItemTypePolicies
                                            oCashListItemTypeRTPolicies(nTotalPolicies).InsuranceFileKey = oPrem.InsuranceFileKey
                                            oCashListItemTypeRTPolicies(nTotalPolicies).AmountTobeAllocated = oPrem.AmountTobeAllocated
                                            oCashListItemTypeRTPolicies(nTotalPolicies).DocumentRef = oPrem.DocumentRef
                                            crPolicyRunningTotal -= oPrem.AmountTobeAllocated
                                            nTotalPolicies += 1
                                        ElseIf oPrem.AmountTobeAllocated < crPolicyRunningTotal Then
                                            ReDim Preserve oCashListItemTypeRTPolicies(nTotalPolicies)
                                            oCashListItemTypeRTPolicies(nTotalPolicies) = New BaseReceiptCashListItemTypePolicies
                                            oCashListItemTypeRTPolicies(nTotalPolicies).InsuranceFileKey = oPrem.InsuranceFileKey
                                            oCashListItemTypeRTPolicies(nTotalPolicies).AmountTobeAllocated = crPolicyRunningTotal
                                            oCashListItemTypeRTPolicies(nTotalPolicies).DocumentRef = oPrem.DocumentRef
                                            If oPrem.WriteOffAmount > (crPolicyRunningTotal - oPrem.AmountTobeAllocated) Then
                                                oPrem.WriteOffAmount = (oPrem.AmountTobeAllocated - crPolicyRunningTotal)
                                            End If
                                            oCashListItemTypeRTPolicies(nTotalPolicies).WriteOffAmount = oPrem.WriteOffAmount
                                            crPolicyRunningTotal = 0
                                            nTotalPolicies += 1
                                        Else
                                            ReDim Preserve oCashListItemTypeRTPolicies(nTotalPolicies)
                                            oCashListItemTypeRTPolicies(nTotalPolicies) = New BaseReceiptCashListItemTypePolicies
                                            oCashListItemTypeRTPolicies(nTotalPolicies).InsuranceFileKey = oPrem.InsuranceFileKey
                                            oCashListItemTypeRTPolicies(nTotalPolicies).AmountTobeAllocated = crPolicyRunningTotal
                                            oCashListItemTypeRTPolicies(nTotalPolicies).DocumentRef = oPrem.DocumentRef
                                            If oPrem.WriteOffAmount > (oPrem.AmountTobeAllocated - crPolicyRunningTotal) Then
                                                oPrem.WriteOffAmount = (oPrem.AmountTobeAllocated - crPolicyRunningTotal)
                                            End If
                                            oCashListItemTypeRTPolicies(nTotalPolicies).WriteOffAmount = oPrem.WriteOffAmount
                                            oCashListItemTypeRTPolicies(nTotalPolicies).WriteOffReasonKey = KSAMBDXCalling
                                            crPolicyRunningTotal = 0
                                            nTotalPolicies += 1
                                        End If
                                    ElseIf crPolicyRunningTotal < 0 Then
                                        If Math.Abs(oPrem.AmountTobeAllocated) <= Math.Abs(crPolicyRunningTotal) Then
                                            ReDim Preserve oCashListItemTypePTPolicies(nTotalPolicies)
                                            oCashListItemTypePTPolicies(nTotalPolicies) = New BasePaymentCashListItemTypePolicies
                                            oCashListItemTypePTPolicies(nTotalPolicies).InsuranceFileKey = oPrem.InsuranceFileKey
                                            oCashListItemTypePTPolicies(nTotalPolicies).AmountTobeAllocated = oPrem.AmountTobeAllocated
                                            oCashListItemTypePTPolicies(nTotalPolicies).DocumentRef = oPrem.DocumentRef
                                            crPolicyRunningTotal -= oPrem.AmountTobeAllocated
                                            nTotalPolicies += 1
                                        Else
                                            ReDim Preserve oCashListItemTypePTPolicies(nTotalPolicies)
                                            oCashListItemTypePTPolicies(nTotalPolicies) = New BasePaymentCashListItemTypePolicies
                                            oCashListItemTypePTPolicies(nTotalPolicies).InsuranceFileKey = oPrem.InsuranceFileKey
                                            oCashListItemTypePTPolicies(nTotalPolicies).AmountTobeAllocated = crPolicyRunningTotal
                                            oCashListItemTypePTPolicies(nTotalPolicies).DocumentRef = oPrem.DocumentRef

                                            oCashListItemTypePTPolicies(nTotalPolicies).WriteOffAmount = 0
                                            If Math.Abs(oPrem.WriteOffAmount) >= Math.Abs(oPrem.AmountTobeAllocated) - Math.Abs(crPolicyRunningTotal) Then
                                                oCashListItemTypePTPolicies(nTotalPolicies).WriteOffAmount = Math.Abs(crPolicyRunningTotal) - Math.Abs(oPrem.AmountTobeAllocated)
                                            End If

                                            crPolicyRunningTotal = 0
                                            nTotalPolicies += 1
                                        End If
                                    Else
                                        Exit For
                                    End If
                                Next

                                crTotalAmount += dCashListAmount
                            End If

                            ' Does the next row in the spreadsheet match this policy?
                            Dim bMatchFound As Boolean = False

                            If Math.Sign(dNextCashListAmount) <> Math.Sign(dCashListAmount) Then
                                bMatchFound = False
                            Else
                                MatchOnNextRow(xeRowMappingElement, worksheet, worksheet.Cells.MaxDataRow + 1, iCnt, bMatchFound)
                            End If

                            If listOfErrors.Count > 0 Then
                                ' Output the error row
                                OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, iCnt, listOfErrors, "Premium_BDX_Import", , sBrokerCode, sCoverHolderName)
                                oGlobalListOfErrors.Add("Failure recorded for Certificate Ref - " & sCertRef, sCertRefColumn, iCnt, sCertRef)
                                nRejectTransactions = nRejectTransactions + 1
                                dCashListAmount = 0
                            End If

                            Dim iCashListKey As Integer
                            Dim iTransDetailKey As Integer


                            ' If a match was found on the next row then prepare the request for the next risk
                            If bMatchFound = False Then

                                Try
                                    ' Call SAM to process the policy

                                    If crTotalAmount > 0 Then
                                        Dim oCashListReceipt As BaseReceiptCashListType
                                        Dim oCashListReceiptItem(0) As BaseReceiptCashListItemType

                                        createReceiptCashListWithItemsRequestType.BranchCode = sCashListItemBranchCodeValue

                                        oCashListReceipt = New BaseReceiptCashListType()
                                        oCashListReceipt.BankAccountCode = sCashListBankAccountCodeValue
                                        oCashListReceipt.CurrencyCode = sCashListCurrencyCodeValue
                                        oCashListReceipt.TypeCode = "R"
                                        oCashListReceipt.StatusCode = "E"
                                        oCashListReceipt.Reference = sCashListReferenceValue
                                        oCashListReceipt.ListDate = dtTransDate

                                        oCashListReceiptItem(0) = New BaseReceiptCashListItemType()
                                        oCashListReceiptItem(0).TypeCode = "STD"
                                        oCashListReceiptItem(0).StatusCode = "ADD"
                                        oCashListReceiptItem(0).Amount = crTotalAmount
                                        oCashListReceiptItem(0).TransactionDate = Date.Now
                                        oCashListReceiptItem(0).MediaTypeCode = sMediaTypeCodeValue
                                        oCashListReceiptItem(0).AccountShortCode = sAccountSortCode
                                        oCashListReceiptItem(0).AllocationStatusCode = "U"
                                        oCashListReceiptItem(0).OurReference = sOurReference
                                        oCashListReceiptItem(0).TheirReference = sTheirReference
                                        oCashListReceiptItem(0).MediaReference = sMediaReference


                                        If bManualAllocation <> True Then
                                        oCashListReceiptItem(0).Policies = New List(Of BaseReceiptCashListItemTypePolicies)(oCashListItemTypeRTPolicies)
                                        End If

                                        oCashListReceipt.ReceiptItem = New List(Of BaseReceiptCashListItemType)(oCashListReceiptItem)

                                        createReceiptCashListWithItemsRequestType.ReceiptCashList = oCashListReceipt
                                        createReceiptCashListWithItemsRequestType.LoginUserName = UserName

                                        createReceiptCashListWithItemsResponseType = _apiClient.PostAsync(Of CreateReceiptCashListWithItemsCommandResponse)("/accounts/receiptCashListWithItems", createReceiptCashListWithItemsRequestType).Result
                                        If createReceiptCashListWithItemsResponseType.Errors IsNot Nothing AndAlso createReceiptCashListWithItemsResponseType.Errors.Count > 0 Then

                                            For Each stsError As SAMErrors In createReceiptCashListWithItemsResponseType.Errors
                                                listOfErrors.Add("The import of Row " & iCnt & "  failed. " & stsError.SAMErrorMessage)
                                            Next

                                            OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, iCnt, listOfErrors, "Premium_BDX_Import", , sBrokerCode, sCoverHolderName)
                                            oGlobalListOfErrors.Add("Failure recorded for Certificate Ref - " & sCertRef, sCertRefColumn, iCnt, sCertRef)
                                            nRejectTransactions = nRejectTransactions + 1

                                        End If

                                        iCashListKey = createReceiptCashListWithItemsResponseType.CashListItem(0).CashListItemKey
                                        iTransDetailKey = createReceiptCashListWithItemsResponseType.CashListItem(0).TransDetailKey


                                    ElseIf crTotalAmount < 0 Then

                                        Dim oCashListPayment As BasePaymentCashListType
                                        Dim oCashListPaymentItem(0) As BasePaymentCashListItemType

                                        createPaymentCashListWithItemsRequestType.BranchCode = sCashListItemBranchCodeValue

                                        oCashListPayment = New BasePaymentCashListType()
                                        oCashListPayment.BankAccountCode = sCashListBankAccountCodeValue
                                        oCashListPayment.CurrencyCode = sCashListCurrencyCodeValue
                                        oCashListPayment.TypeCode = "P"
                                        oCashListPayment.StatusCode = "E"
                                        oCashListPayment.Reference = sCashListReferenceValue
                                        oCashListPayment.ListDate = Date.Now

                                        oCashListPaymentItem(0) = New BasePaymentCashListItemType()
                                        oCashListPaymentItem(0).TypeCode = "REFUND"
                                        oCashListPaymentItem(0).StatusCode = "ISS"
                                        oCashListPaymentItem(0).Amount = crTotalAmount
                                        oCashListPaymentItem(0).TransactionDate = Date.Now
                                        oCashListPaymentItem(0).MediaTypeCode = sMediaTypeCodeValue
                                        oCashListPaymentItem(0).AccountShortCode = sAccountSortCode
                                        oCashListPaymentItem(0).AllocationStatusCode = "U"


                                        If bManualAllocation <> True Then
                                        oCashListPaymentItem(0).Policies = New List(Of BasePaymentCashListItemTypePolicies)(oCashListItemTypePTPolicies)
                                        End If

                                        oCashListPayment.PaymentItem = New List(Of BasePaymentCashListItemType)(oCashListPaymentItem)

                                        createPaymentCashListWithItemsRequestType.PaymentCashList = oCashListPayment
                                        createPaymentCashListWithItemsRequestType.LoginUserName = UserName
                                        createPaymentCashListWithItemsResponseType = _apiClient.PostAsync(Of CreatePaymentCashListWithItemsCommandResponse)("/accounts/paymentCashListWithItems", createPaymentCashListWithItemsRequestType).Result

                                        If createPaymentCashListWithItemsResponseType.Errors IsNot Nothing AndAlso createPaymentCashListWithItemsResponseType.Errors.Count > 0 Then

                                            For Each stsError As SAMErrors In createPaymentCashListWithItemsResponseType.Errors
                                                listOfErrors.Add("The import of Row " & iCnt & "  failed. " & stsError.SAMErrorMessage)
                                            Next

                                            OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, iCnt, listOfErrors, "Premium_BDX_Import", , sBrokerCode, sCoverHolderName)
                                            oGlobalListOfErrors.Add("Failure recorded for Certificate Ref - " & sCertRef, sCertRefColumn, iCnt, sCertRef)
                                            nRejectTransactions = nRejectTransactions + 1

                                        End If
                                        iCashListKey = createPaymentCashListWithItemsResponseType.CashListItem(0).CashListItemKey
                                        iTransDetailKey = createPaymentCashListWithItemsResponseType.CashListItem(0).TransDetailKey


                                    End If

                                     If bManualAllocation AndAlso iCashListKey <> 0 Then

                                        m_lReturn = CreateManualAllocation(oPremiumAllocations, iTransDetailKey, iCashListKey, oAllocationManual)
                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Throw New System.Exception(m_lReturn.ToString() + ", CreateManualAllocation, " + "CreateManualAllocation Method " &
                                                                   "failed.")
                                        End If

                                    End If
                                    
                                    oPremiumAllocations = Nothing

                                    oCashListItemTypePTPolicies = Nothing
                                    oCashListItemTypeRTPolicies = Nothing
                                    crTotalAmount = 0
                                    nTotalPolicies = 0

                                Catch ex As Exception

                                    If createReceiptCashListWithItemsResponseType IsNot Nothing AndAlso createReceiptCashListWithItemsResponseType.Errors IsNot Nothing AndAlso createReceiptCashListWithItemsResponseType.Errors.Count > 0 Then

                                        For Each stsError As SAMErrors In createReceiptCashListWithItemsResponseType.Errors
                                            listOfErrors.Add("The import of Row " & iCnt & "  failed." & stsError.SAMErrorMessage)
                                        Next

                                        OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, iCnt, listOfErrors, "Premium_BDX_Import", , sBrokerCode, sCoverHolderName)
                                        oGlobalListOfErrors.Add("Failure recorded for Certificate Ref - " & sCertRef, sCertRefColumn, iCnt, sCertRef)
                                        nRejectTransactions = nRejectTransactions + 1

                                    ElseIf createPaymentCashListWithItemsResponseType IsNot Nothing AndAlso createPaymentCashListWithItemsResponseType.Errors IsNot Nothing AndAlso createPaymentCashListWithItemsResponseType.Errors.Count > 0 Then

                                        For Each stsError As SAMErrors In createPaymentCashListWithItemsResponseType.Errors
                                            listOfErrors.Add("The import of Row " & iCnt & "  failed. " & stsError.SAMErrorMessage)
                                        Next

                                        OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, iCnt, listOfErrors, "Premium_BDX_Import", , sBrokerCode, sCoverHolderName)
                                        oGlobalListOfErrors.Add("Failure recorded for Certificate Ref - " & sCertRef, sCertRefColumn, iCnt, sCertRef)
                                        nRejectTransactions = nRejectTransactions + 1

                                    Else
                                        listOfErrors.Add("The import of Row " & iCnt & "  failed. " & ex.Message)
                                    End If

                                    OutputErrorRow(m_oDatabase, worksheetName, worksheet, sErrorWorkbookFilename, errorWorkbook, iCnt, listOfErrors, "Premium_BDX_Import", , sBrokerCode, sCoverHolderName)
                                    oGlobalListOfErrors.Add("Failure recorded for Certificate Ref - " & sCertRef, sCertRefColumn, iCnt, sCertRef)
                                    nRejectTransactions = nRejectTransactions + 1

                                End Try

                            End If

                        Next
                        If bManualAllocation Then
                            oAllocationManual.Dispose()
                            oAllocationManual = Nothing
                        End If
                    End If
                End If
            Next
            m_sBatchStatus = kBatchStatusComplete
            If nRejectTransactions > 0 Then
                OutputLine("Process premium bordereaux completed. Total request processed - " & nTotalTransactions & ", Failed - " & nRejectTransactions & " See out file for details.")
            Else
                OutputLine("Process premium bordereaux completed. Total request processed - " & nTotalTransactions & " See out file for details.")
            End If
            'Closing the file streams to free all resources
            fsFileStream.Close()

        Catch NotExcelFileEx As Aspose.Cells.CellsException
            m_sBatchStatus = kBatchStatusFailed
            ' Create a work manager task to indicate this file failed
            CreateWorkManagerTask(String.Format("Premium BDX Import failed for the import file {0}.  The file was not recognised as a valid Excel Spreadsheet.", WorkbookFile.FullName))
        Catch ex As Exception
            m_sBatchStatus = kBatchStatusFailed
            Throw New Exception("Unable to add new book.", ex)
        Finally

            If fsFileStream IsNot Nothing Then
                fsFileStream.Close()
            End If

            ' Output the files to a specific Broker folder
            Dim sProcessedFolder As String = String.Empty
            If sBrokerCode <> String.Empty Then
                sProcessedFolder = ImportedPath & "\" & sBrokerCode
                If Directory.Exists(sProcessedFolder) = False Then
                    Directory.CreateDirectory(sProcessedFolder)
                End If
            Else
                sProcessedFolder = ImportedPath
            End If

            ' If the Error file exists then 
            If sErrorWorkbookFilename <> String.Empty AndAlso IO.File.Exists(sErrorWorkbookFilename) Then
                Dim sProcessedFilename As String = String.Empty
                Try
                    Dim targetFileName As String = WorkbookFile.Name.Replace(WorkbookFile.Extension, " - Errors(" & Now.ToFileTime & ")" & WorkbookFile.Extension)
                    sProcessedFilename = sProcessedFolder & "\" & targetFileName

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
                    CreateWorkManagerTask(String.Format("Premium BDX Import failed for the import file {0}.  Please see the Premium BDX error report for further details", WorkbookFile.FullName))

                Catch ex As System.IO.IOException
                    ' Create a work manager task to indicate this file failed
                    CreateWorkManagerTask(String.Format("Premium BDX Import Failed but unable to move error file from {0} to {1}.", sErrorWorkbookFilename, sProcessedFilename))
                End Try
            End If

            ' Move the original file across tp processed
            If WorkbookFile.Name <> String.Empty Then
                Dim sProcessedFilename As String = String.Empty
                Try
                    Dim targetFileName As String = WorkbookFile.Name.Replace(WorkbookFile.Extension, " - (" & Now.ToFileTime & ")" & WorkbookFile.Extension)

                    sProcessedFilename = sProcessedFolder & "\" & targetFileName

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
                    CreateWorkManagerTask(String.Format("Premium BDX Import Failed but unable to move error file from {0} to {1}.", WorkbookFile.FullName, sProcessedFilename))
                End Try
            End If

            ' Take off the rejected transactions
            'nTotalTransactions = nTotalTransactions - nRejectTransactions

            ' Update the batch 
            UpdateBatch(BatchStatusCode:=m_sBatchStatus, TotalAmount:=crTotalAmount, TotalTransactions:=nTotalTransactions, RejectAmount:=crRejectAmount, RejectTransactions:=nRejectTransactions, ImportFilename:=WorkbookFile.FullName)
        End Try
    End Sub

    ' BuildStsErrorString - commented out, no longer needed with REST API (SAMErrors has no subtypes)
    ' Private Function BuildStsErrorString(ByVal stsError As PureService.SAMError) As String
    '
    '     Dim sErr As String
    '
    '     sErr = stsError.ToString
    '
    '     If Right(stsError.ToString, 19) = "SAMErrorInvalidData" Then
    '
    '         Dim sInvalidData As PureService.SAMErrorInvalidData = stsError
    '
    '         sErr = sErr & ", Reason: " & sInvalidData.Reason.ToString & ", Error Code: " & sInvalidData.Code & ", Description: " & sInvalidData.Description & ", Field Name: " & sInvalidData.FieldName & ", Supplied Value: " & sInvalidData.SuppliedValue
    '     End If
    '
    '     Return sErr
    ' End Function

#End Region

#Region "Creator"
    Public Sub New(ByVal oXML As XmlDocument)
        MyBase.New(oXML)
    End Sub
#End Region
End Class
