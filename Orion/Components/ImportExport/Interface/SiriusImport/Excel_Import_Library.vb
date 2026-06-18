Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.IO
Imports System.Xml.Serialization
Imports Aspose.Cells
Imports SharedFiles

Friend NotInheritable Class Excel_Import_Library 

#Region "Contstants"

    Friend Const STR_Validation As String = "Validation"
    Friend Const STR_Lookup As String = "Lookup"
    Friend Const STR_List As String = "List"
    Friend Const STR_DestValue As String = "DestValue"
    Friend Const STR_GreaterThan As String = "GreaterThan"
    Friend Const STR_Description As String = "Description"
    Friend Const STR_Rule As String = "Rule"
    Friend Const STR_LessThan As String = "LessThan"
    Friend Const STR_SourceValue As String = "SourceValue"
    Friend Const STR_Location As String = "Location"
    Friend Const STR_ImportHeader As String = "IMPORT_HEADER"
    Friend Const STR_Worksheet As String = "Worksheet"
    Friend Const STR_Name As String = "name"
    Friend Const STR_HeaderStructure As String = "HeaderStructure"
    Friend Const STR_Column As String = "Column"
    Friend Const STR_Length As String = "Length"
    Friend Const STR_Position As String = "Position"
    Friend Const STR_Broker_Code As String = "BrokerCode"
    Friend Const STR_Certificate_Ref_Column As String = "CertificateRefColumn"
    Friend Const STR_CoverHoldername_Column As String = "CoverHolderNameColumn"
    Friend Const STR_Claim_Ref_Column As String = "ClaimRefColumn"
    Friend Const STR_TPAname_Column As String = "TPAColumn"
    Friend Const STR_ReserveTypeCode As String = "TypeCode"
    Friend Const STR_RevisionAmount As String = "RevisionAmount"
    Friend Const STR_PaymentAmount As String = "PaymentAmount"
    Friend Const STR_IsReserveToDate As String = "IsReserveToDate"
    Friend Const STR_IsPaidToDate As String = "IsPaymentToDate"
    Friend Const STR_PaymentCurrencyCode As String = "PaymentCurrencyCode"
    Friend Const STR_PaymentBankCode As String = "PaymentBankCode"
    Friend Const STR_PaymentMediaTypeCode As String = "PaymentMediaTypeCode"
    Friend Const STR_PaymentPartyType As String = "PaymentPartyType"
    Friend Const STR_PaymentPartyCode As String = "PaymentPartyCode"
    Friend Const STR_RecoveryTypeCode As String = "TypeCode"
    Friend Const STR_ReceiptAmount As String = "ReceiptAmount"
    Friend Const STR_IsRecoverToDate As String = "IsRecoverToDate"
    Friend Const STR_IsReceiptToDate As String = "IsReceiptToDate"
    Friend Const STR_ReceiptCurrencyCode As String = "ReceiptCurrencyCode"
    Friend Const STR_ReceiptBankCode As String = "ReceiptBankCode"
    Friend Const STR_ReceiptMediaTypeCode As String = "ReceiptMediaTypeCode"
    Friend Const STR_ReceiptPartyType As String = "ReceiptPartyType"
    Friend Const STR_ReceiptPartyCode As String = "ReceiptPartyCode"
    Friend Const STR_IsSalvageRecoveryCode As String = "IsSalvageRecovery"
    Friend Const STR_ProcessRecoveryOnlyWhenPaymentMade As String = "ProcessRecoveryOnlyWhenPaymentMade"
    Friend Const STR_PartyType As String = "PartyType"
    Friend Const STR_ProcessedFolder As String = "ProcessedFolder"
    Friend Const STR_UpdateParty As String = "UpdateParty"
    Friend Const STR_Equation As String = "Equation"
    Friend Const STR_BranchCode As String = "BranchCode"
    Friend Const STR_RecoveryPartyCode As String = "RecoveryPartyCode"
    Friend Const STR_RecoveryPartyTypeCode As String = "RecoveryPartyTypeCode"


#End Region

#Region "Public Structures"

    Public Class listOfErrorsType
        Inherits List(Of errorType)

        Public Overloads Sub Add(ByVal message As String)
            Dim errorItem As New errorType(message)
            Me.Add(errorItem)
        End Sub

        Public Overloads Sub Add(ByVal message As String, ByVal column As String)
            Dim errorItem As New errorType(message, column)
            Me.Add(errorItem)
        End Sub

        Public Overloads Sub Add(ByVal message As String, ByVal column As String, ByVal row As Integer)
            Dim errorItem As New errorType(message, column, row)
            Me.Add(errorItem)
        End Sub

        Public Overloads Sub Add(ByVal message As String, ByVal column As String, ByVal row As Integer, ByVal certRef As String)
            Dim errorItem As New errorType(message, column, row, certRef)
            Me.Add(errorItem)
        End Sub


    End Class

    Public NotInheritable Class errorType 

        Private columnValue As String
        Private rowValue As Integer
        Private messageValue As String
        Private certRefValue As String

        Public Sub New(ByVal message As String)
            messageValue = message
        End Sub

        Public Sub New(ByVal message As String, ByRef column As String)
            messageValue = message
            columnValue = column
        End Sub
        Public Sub New(ByVal message As String, ByRef column As String, ByVal row As Integer)
            messageValue = message
            columnValue = column
            rowValue = row
        End Sub
        Public Sub New(ByVal message As String, ByRef column As String, ByVal row As Integer, ByVal CertRef As String)
            messageValue = message
            columnValue = column
            rowValue = row
            certRefValue = CertRef
        End Sub

        Public Property Column() As String
            Get
                Return columnValue
            End Get
            Set(ByVal value As String)
                columnValue = value
            End Set
        End Property
        Public Property Row() As Integer
            Get
                Return rowValue
            End Get
            Set(ByVal value As Integer)
                rowValue = value
            End Set
        End Property
        Public Property Message() As String
            Get
                Return messageValue
            End Get
            Set(ByVal value As String)
                messageValue = value
            End Set
        End Property
        Public Property CertRef() As String
            Get
                Return certRefValue
            End Get
            Set(ByVal value As String)
                certRefValue = value
            End Set
        End Property
    End Class

    Public NotInheritable Class certFinderPredicate 
        Private m_lookingForCert As String
        Public Sub New(ByVal lookingForCert As String)
            m_lookingForCert = lookingForCert
        End Sub
        Public Function Match(ByVal item As errorType) As Boolean
            Return m_lookingForCert = item.CertRef
        End Function
    End Class

#End Region

#Region "Methods"

    Friend Shared Sub CreateWorkManagerTask(ByVal sDescription As String)
        Dim iReturn As PMEReturnCode
        Dim oDatabase As dPMDAO.Database = Nothing

        Try
            ' Connect to db
            DBConnect(oDatabase)

            ' Add parameters
            AddParameterLite(oDatabase, "pmwrk_task_instance_cnt", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong, True)
            AddParameterLite(oDatabase, "pmwrk_task_group_id", 5, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "pmwrk_task_id", 18, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "customer", "System adminstration", PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(oDatabase, "task_due_date", DateTime.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(oDatabase, "pmuser_group_id", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "user_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "description", sDescription, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(oDatabase, "task_status", 0, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "is_urgent", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "date_created", DateTime.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(oDatabase, "created_by_id", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "last_modified", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(oDatabase, "modified_by_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "is_visible", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "workflow_information", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

            ' Execute command
            iReturn = oDatabase.SQLAction("spe_PMWrk_Task_Instance_add", "Create WMTask", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spe_PMWrk_Task_Instance_add'")
            End If
        Catch ex As Exception
            Throw New Exception("Unable to create work manager task", ex)
        Finally
            ' Disconnect from db
            DBDisconnect(oDatabase)
        End Try
    End Sub

    Friend Shared Function ValidateAndConvertDatatype(ByVal field As Xml.XmlElement, ByRef compareValue As Object, ByVal cell As String, ByRef listOfErrors As listOfErrorsType, ByVal cellColumn As String, ByVal cnt As Integer) As Boolean
        Dim bMultiply As Boolean = True

        Dim datatype As String = field.Attributes("Datatype").Value
        If field.HasAttribute("MultiplyPercent") Then
            bMultiply = CBool(field.Attributes("MultiplyPercent").Value)
        End If


        ValidateAndConvertDatatype = True

        Select Case datatype
            Case "Date"
                If (IsDate(compareValue) = False) Then
                    listOfErrors.Add("The value for Cell """ & cell & """, in the original file, cannot be converted to the correct datatype.  The expected datatype is " & datatype, cellColumn, cnt)
                    ValidateAndConvertDatatype = False
                Else
                    compareValue = Convert.ToDateTime(compareValue).ToString("yyyy-MM-dd")
                End If
            Case "Currency"
                If (IsNumeric(compareValue) = False) Then
                    listOfErrors.Add("The value for Cell """ & cell & """, in the original file, cannot be converted to the correct datatype.  The expected datatype is " & datatype, cellColumn, cnt)
                    ValidateAndConvertDatatype = False
                Else
                    compareValue = Convert.ToDouble(compareValue)
                End If
            Case "Percent"
                If (IsNumeric(compareValue) = False) Then
                    listOfErrors.Add("The value for Cell """ & cell & """, in the original file, cannot be converted to the correct datatype.  The expected datatype is " & datatype, cellColumn, cnt)
                    ValidateAndConvertDatatype = False
                Else
                    compareValue = Convert.ToDouble(compareValue) * IIf(bMultiply, 100, 1)
                    If (Convert.ToDouble(compareValue) > 100) Then
                        listOfErrors.Add("The value for Cell """ & cell & """ cannot be converted to Percent. It not should be more than 100", cellColumn, cnt)
                        ValidateAndConvertDatatype = False
                    End If
                End If
            Case "String"
                If field.Attributes(STR_Length) IsNot Nothing Then
                    If compareValue.ToString.Length > Convert.ToInt32(field.Attributes(STR_Length).Value.ToString) Then
                        listOfErrors.Add("The value for Cell """ & cell & """, in the original file, is longer than the maximum length specified which is " & field.Attributes(STR_Length).Value, cellColumn, cnt)
                        ValidateAndConvertDatatype = False
                    Else
                        compareValue = compareValue.ToString.Trim
                    End If
                End If
            Case "Integer"
                If (IsNumeric(compareValue) = False) Then
                    listOfErrors.Add("The value for Cell """ & cell & """, in the original file, cannot be converted to the correct datatype.  The expected datatype is " & datatype, cellColumn, cnt)
                    ValidateAndConvertDatatype = False
                Else
                    compareValue = Convert.ToInt32(compareValue)
                End If
        End Select
    End Function

    Friend Shared Function HeaderStructureIsValid(ByVal worksheetElement As Xml.XmlElement, ByVal worksheet As Worksheet, ByRef listOfErrors As listOfErrorsType) As Boolean
        ' Check the structure of the spreadsheet. Reject it if it doesn't match what we expect.
        Dim headerStructureValid As Boolean = True
        Dim headerStructure As Xml.XmlElement = worksheetElement.Item(STR_HeaderStructure)
        For Each xmlElement As Xml.XmlElement In headerStructure.GetElementsByTagName(STR_Column)
            If worksheet.Cells(xmlElement.Attributes(STR_Location).Value).Value <> (xmlElement.Attributes(STR_Description).Value) Then
                headerStructureValid = False
                listOfErrors.Add("The header """ & xmlElement.Attributes(STR_Description).Value & """ was expected in Cell " & xmlElement.Attributes(STR_Location).Value & " but was not present")
            End If
        Next

        Return headerStructureValid

    End Function
    Friend Shared Sub GetMatchingRowKey(ByVal xeRowMappingElement As Xml.XmlElement, ByVal worksheet As Worksheet, ByVal rowCnt As Integer, ByRef sMatchKey As String, ByRef sMatchKeyColumns As String)
        If xeRowMappingElement Is Nothing OrElse worksheet Is Nothing Then
            sMatchKey = ""
            Return
        End If


        Dim thisRowCellValue As String = ""
        Dim thisColumnName As String = ""

        For Each column As Xml.XmlElement In xeRowMappingElement.GetElementsByTagName("Column")

            Dim thisCellName As String = column.Attributes(STR_Location).Value

            Dim thisCell As Cell

            If thisCellName.StartsWith("$") Then
                thisCellName = thisCellName.Substring(1).Trim & (rowCnt).ToString
                thisCell = worksheet.Cells(thisCellName)

                If thisCell IsNot Nothing Then

                    If thisRowCellValue = "" Then
                        thisRowCellValue = Convert.ToString(thisCell.Value)
                    Else
                        thisRowCellValue = thisRowCellValue & ", " & Convert.ToString(thisCell.Value)
                    End If

                    If thisColumnName = "" Then
                        thisColumnName = thisCellName
                    Else
                        thisColumnName = thisColumnName & ", " & thisCellName
                    End If
                End If
            End If

        Next

        sMatchKey = thisRowCellValue
        sMatchKeyColumns = thisColumnName
    End Sub

    Friend Shared Sub MatchOnNextRow(ByVal rowMappingElement As Xml.XmlElement, ByVal worksheet As Worksheet, ByVal lastRow As Integer, ByVal rowCnt As Integer, ByRef matchFound As Boolean)

        If rowMappingElement Is Nothing OrElse worksheet Is Nothing Then
            matchFound = False
            Return
        End If

        matchFound = True

        If rowCnt < lastRow Then
            For Each column As Xml.XmlElement In rowMappingElement.GetElementsByTagName("Column")

                Dim nextCellName As String = column.Attributes(STR_Location).Value
                Dim thisCellName As String = column.Attributes(STR_Location).Value
                Dim nextCell As Cell
                Dim thisCell As Cell
                Dim nextRowCellValue As Object = 1
                Dim thisRowCellValue As Object = 2

                If nextCellName.StartsWith("$") Then
                    nextCellName = nextCellName.Substring(1).Trim & (rowCnt + 1).ToString
                    nextCell = worksheet.Cells(nextCellName)
                    If nextCell IsNot Nothing Then
                        nextRowCellValue = nextCell.Value
                    End If
                End If

                If thisCellName.StartsWith("$") Then
                    thisCellName = thisCellName.Substring(1).Trim & (rowCnt).ToString
                    thisCell = worksheet.Cells(thisCellName)
                    If thisCell IsNot Nothing Then
                        thisRowCellValue = thisCell.Value
                    End If
                End If

                If nextRowCellValue <> thisRowCellValue Then
                    matchFound = False
                End If
            Next
        Else
            matchFound = False
        End If

    End Sub

    Friend Shared Function GetXNode(ByRef xDoc As Xml.XmlDocument, ByVal xpath As String, ByVal nsManager As XmlNamespaceManager) As XmlNode
        Dim xNode As XmlNode
        Dim previousElementLocation As Integer = xpath.LastIndexOfAny(New [Char]() {"/"c})
        Dim newXpath As String = xpath.Substring(0, previousElementLocation)
        Dim newElement As String = xpath.Substring(previousElementLocation + 1)

        ' Select the node using the XPath
        xNode = xDoc.SelectSingleNode(xpath, nsManager)
        If xNode Is Nothing Then
            xNode = GetXNode(xDoc, newXpath, nsManager)
        End If
        If (xDoc.SelectSingleNode(xpath, nsManager)) Is Nothing Then
            Dim newNode As XmlNode = xDoc.CreateNode(XmlNodeType.Element, newElement, "http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")
            Dim inserted As Boolean = False
            For Each refNode As XmlNode In xNode.ChildNodes
                If refNode.Name > newNode.Name Then
                    xNode.InsertBefore(newNode, refNode)
                    inserted = True
                    Exit For
                End If
            Next
            If inserted = False Then
                xNode.AppendChild(newNode)
            End If
        End If

        xNode = xDoc.SelectSingleNode(xpath, nsManager)

        Return xNode
    End Function

    Friend Shared Sub OutputErrorRow(ByVal dbConn As dPMDAO.Database, ByVal worksheetName As String, ByVal worksheet As Worksheet, ByVal errorWorkbookFilename As String, ByVal errorWorkbook As Workbook, ByVal cnt As Integer, ByVal listOfErrors As listOfErrorsType, ByVal ClassName As String, Optional ByVal startRow As Integer = 0, Optional ByVal BrokerCode As String = "", Optional ByVal CoverHolder As String = "")
        Dim errorCells As Cells = errorWorkbook.Worksheets(worksheetName).Cells
        Dim newRow As Integer = errorCells.MaxRow + 1

        If cnt > 0 Then
            Try
                errorCells.CopyRows(worksheet.Cells, cnt - 1, newRow, 1)
            Catch ex As Exception
                ' I cant get rid of the exception triggered by the CopyRows method but the row still
                ' gets copied so for now we'll just ignore the exception
            End Try
        End If

        If errorWorkbook.Worksheets(worksheetName & " - Errors") Is Nothing Then
            errorWorkbook.Worksheets.Add(worksheetName & " - Errors")
        End If

        Dim errorMsgCells As Cells = errorWorkbook.Worksheets(worksheetName & " - Errors").Cells
        For Each errorMsg As errorType In listOfErrors
            Dim newMsgRow As Integer = errorMsgCells.GetLastDataRow(1) + 1
            errorMsgCells.InsertRow(newMsgRow)
            If (cnt > 0) And (errorMsg.Column <> String.Empty) Then
                Dim hyperlinkCell As String = errorMsg.Column & (newRow + 1).ToString
                errorWorkbook.Worksheets(worksheetName & " - Errors").Hyperlinks.Add(newMsgRow, 0, 1, 1, "'" & worksheetName & "'!" & errorCells.Item(hyperlinkCell).Name)
            ElseIf (cnt > 0) Then
                errorWorkbook.Worksheets(worksheetName & " - Errors").Hyperlinks.Add(newMsgRow, 0, 1, 1, "'" & worksheetName & "'!" & errorCells.Item(newRow, 0).Name)
            Else
                errorMsgCells(newMsgRow, 0).Value = ""
            End If
            errorMsgCells(newMsgRow, 1).Value = errorMsg.Message

            ' Log the message to the PMMessage Table
            Dim newMessageId As Integer = 0
            AddParameterLite(dbConn, "username", "Sirius", PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(dbConn, "calling_app_name", ACApp, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(dbConn, "text", errorMsg.Message, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(dbConn, "err_description", "Policy BDX Import Failed. (Agent:" & BrokerCode & ") <CoverHolder:" & CoverHolder & "> The errors were logged in - " & errorWorkbookFilename, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(dbConn, "app_name", "SiriusImport", PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(dbConn, "class_name", ClassName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(dbConn, "method_name", "ProcessFile", PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            AddParameterLite(dbConn, "source_id", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(dbConn, "message_type", 15, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(dbConn, "err_number", 15, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(dbConn, "log_date", Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)

            AddParameterLite(dbConn, "message_id", newMessageId, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

            ' Execute sql
            Dim ret As Integer = dbConn.SQLAction("spu_add_pmmessage", "Add PMMessage entry for SiriusImport error", True)
            ' If the SQL has errored then the error will be logged.  No need to fail the process due to this.

        Next

        errorWorkbook.Save(errorWorkbookFilename)

    End Sub

    Friend Shared Sub ExtractAndValidateValue(ByVal nsMgr As Xml.XmlNamespaceManager, ByVal worksheet As Worksheet, ByVal cnt As Integer, ByVal listOfErrors As listOfErrorsType, ByVal field As Xml.XmlElement, ByRef valueIsValid As Boolean, ByRef cellValue As Object, ByRef datatype As String, ByVal oDatabase As dPMDAO.Database)
        Dim cellName As String = field.Attributes(STR_SourceValue).Value
        Dim cellColumn As String = String.Empty
        Dim cell As Cell


        ' If the Cellname begins with $ then it means it's a reference to a cell in the spreadsheet
        If cellName.StartsWith("$") Then
            cellColumn = cellName.Substring(1).Trim
            cellName = cellName.Substring(1).Trim & cnt.ToString
            cell = worksheet.Cells(cellName)
            cellValue = cell.Value
        Else
            ' Else it's a static value
            cellValue = cellName
        End If

        ' Check the mandatory value
        If (CBool(field.Attributes("Required").Value) = True) And (cellValue Is Nothing) Then
            valueIsValid = False
            'listOfErrors.Add("The value for Column """ & field.Attributes(STR_SourceValue).Value & """ was expected but is blank in Row " & cnt, cellColumn, cnt)
            If cellName = "N6" Then
                listOfErrors.Add("No Insured name")
            Else
                listOfErrors.Add("The value for Column """ & field.Attributes(STR_SourceValue).Value & """ was expected but is blank in Row " & cnt, cellColumn, cnt)
            End If
        Else
            If cellValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(cellValue) Then

                datatype = field.Attributes("Datatype").Value

                Dim errorMessage As String = String.Empty

                ' Validate the item against it's defined datatype
                If ValidateAndConvertDatatype(field, cellValue, cellName, listOfErrors, cellColumn, cnt) = False Then
                    valueIsValid = False
                Else

                    ' Apply any validation rules that may apply
                    If (field.Item(STR_Lookup) IsNot Nothing) Then
                        Dim LookupValue As String = ""
                        GetLookupValue(oDatabase, field.Item(STR_Lookup), worksheet, cnt, LookupValue, listOfErrors)
                        If LookupValue Is Nothing Or String.IsNullOrEmpty(LookupValue) Then
                            listOfErrors.Add("The value for Column """ & field.Attributes(STR_SourceValue).Value & """ was not found in lookup table (" & field.Item(STR_Lookup).Item("Parameters").GetElementsByTagName("Parameter").Item(0).Attributes(STR_SourceValue).Value & ")", cellColumn, cnt)
                        Else
                            cellValue = LookupValue
                        End If
                    ElseIf (field.Item(STR_Validation) IsNot Nothing) Then
                        ' Validate the list items
                        If (field.Item(STR_Validation).Item(STR_List) IsNot Nothing) Then

                            ' Search for the value in the list using xpath
                            Dim list As Xml.XmlElement = field.Item(STR_Validation).Item(STR_List)
                            If (list.SelectSingleNode("./ab:Item[@SourceValue='" & cellValue.ToString & "']", nsMgr) IsNot Nothing) Then

                                Dim item As Xml.XmlElement = CType(list.SelectSingleNode("./ab:Item[@SourceValue='" & cellValue.ToString & "']", nsMgr), XmlElement)
                                Dim listItemValue As String = String.Empty

                                'If no item is found then the value is invalid
                                If item Is Nothing Then
                                    listOfErrors.Add("The value for Column """ & field.Attributes(STR_SourceValue).Value & """ in Row " & cnt & ", in the original file, is not a valid list item", cellColumn, cnt)
                                    valueIsValid = False
                                Else
                                    If (item.Attributes(STR_DestValue) IsNot Nothing) Then
                                        cellValue = item.Attributes(STR_DestValue).Value.ToString
                                    Else
                                        cellValue = cellValue.ToString
                                    End If
                                End If
                            Else
                                If (CBool(field.Attributes("Required").Value) = False AndAlso cellValue.ToString() = "") Then
                                    'Dont throw an error
                                Else
                                    listOfErrors.Add("The value for Column """ & field.Attributes(STR_SourceValue).Value & """ in Row " & cnt & ", in the original file, is not a valid list item", cellColumn, cnt)
                                    valueIsValid = False
                                End If
                            End If

                            ' Validate the rules
                        ElseIf (field.Item(STR_Validation).Item(STR_Rule) IsNot Nothing) Then

                            ' If the rule is a "GreaterThan" rule then
                            Dim rule As Xml.XmlElement = field.Item(STR_Validation).Item(STR_Rule)
                            If (rule.Attributes(STR_GreaterThan) IsNot Nothing) Then

                                ' Identify the value we're comparing against
                                Dim compareLocation As String = rule.Attributes(STR_GreaterThan).Value.ToString
                                Dim compareLocationColumn As String = String.Empty
                                Dim compareValue As Object = String.Empty
                                If compareLocation.StartsWith("$") Then
                                    compareLocationColumn = compareLocation.Substring(1).Trim
                                    compareLocation = compareLocationColumn & cnt.ToString
                                    Dim compareCell As Cell = worksheet.Cells(compareLocation)
                                    compareValue = compareCell.Value.ToString
                                Else
                                    compareValue = rule.Attributes(STR_GreaterThan).Value.ToString
                                End If

                                If ValidateAndConvertDatatype(field, compareValue, compareLocation, listOfErrors, compareLocationColumn, cnt) = False Then
                                    valueIsValid = False
                                Else
                                    If cellValue < compareValue Then
                                        listOfErrors.Add("The Value in Column " & field.Attributes(STR_SourceValue).Value & " in Row " & cnt & ", in the original file, is less than the value in Cell" & compareLocation & " and the validation rules stipulate it should be greater.", cellColumn, cnt)
                                        valueIsValid = False
                                    End If
                                End If

                                ' If the rule is a "GreaterThan" rule then
                            ElseIf (rule.Attributes(STR_LessThan) IsNot Nothing) Then

                                ' Identify the value we're comparing against
                                Dim compareLocation As String = rule.Attributes(STR_LessThan).Value.ToString
                                Dim compareLocationColumn As String = String.Empty
                                Dim compareValue As Object = String.Empty
                                If compareLocation.StartsWith("$") Then
                                    compareLocationColumn = compareLocation.Substring(1).Trim
                                    compareLocation = compareLocationColumn & cnt.ToString
                                    Dim compareCell As Cell = worksheet.Cells(compareLocation)
                                    compareValue = compareCell.Value
                                Else
                                    compareValue = rule.Attributes(STR_LessThan).Value.ToString
                                End If

                                If ValidateAndConvertDatatype(field, compareValue, compareLocation, listOfErrors, compareLocationColumn, cnt) = False Then
                                    valueIsValid = False
                                Else
                                    If cellValue > compareValue Then
                                        listOfErrors.Add("The Value in Column " & field.Attributes(STR_SourceValue).Value & " in Row " & cnt & ", in the original file, is greater than the value in Cell" & compareLocation & " and the validation rules stipulate it should be less.", cellColumn, cnt)
                                        valueIsValid = False
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If

            End If
        End If
    End Sub


    Friend Shared Function GetCellvalue(ByVal thisCellName As String, ByVal worksheet As Worksheet, ByVal rowCnt As Integer) As String
        Dim ThisCellValue As String = ""
        If thisCellName.StartsWith("$") Then
            thisCellName = thisCellName.Substring(1).Trim & (rowCnt).ToString
            If worksheet.Cells(thisCellName) IsNot Nothing AndAlso worksheet.Cells(thisCellName).Value IsNot Nothing Then
                ThisCellValue = worksheet.Cells(thisCellName).Value.ToString()
            Else
                ThisCellValue = String.Empty
            End If

        Else
            ThisCellValue = thisCellName.ToString()
        End If
        Return ThisCellValue
    End Function


    Friend Shared Sub GetLookupValue(ByVal m_oDatabase As dPMDAO.Database, ByVal LookupElement As Xml.XmlElement, ByVal worksheet As Worksheet, ByVal rowCnt As Integer, ByRef LookupValue As String, ByVal listOfErrors As listOfErrorsType) 

        Static originalSqlScript As String = String.Empty

        If LookupElement Is Nothing OrElse worksheet Is Nothing Then
            Return
        End If

        ' If we don't already have it read in the SQL file
        If originalSqlScript = String.Empty Then
            Dim sqlScriptLocation As String = LookupElement.Attributes("SQL").Value.ToString 
            If Dir(sqlScriptLocation) = String.Empty Then
                listOfErrors.Add("The Lookup Matching SQL file was not found.  Please check the path in the configuration file")
                Return
            Else
                Dim sr As StreamReader = File.OpenText(sqlScriptLocation) 
                originalSqlScript = sr.ReadToEnd()
            End If
        End If

        ' Get all of the parameters defined in the configuration file
        Dim parametersElement As XmlElement = LookupElement.GetElementsByTagName("Parameters").Item(0) 

        ' Setup the parameter array for the string format.  The number of parameters in the SQL has to 
        ' match the number of parameters in the condif.
        Dim sqlParamArray(parametersElement.GetElementsByTagName("Parameter").Count - 1) As Object 

        ' For each parameter in the config go and fetch the value out of the spreadsheet
        For Each parameter As Xml.XmlElement In LookupElement.Item("Parameters").GetElementsByTagName("Parameter")

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
            Else
                thisRowCellValue = thisCellName.ToString()
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
        Dim iReturn As Integer = m_oDatabase.SQLSelect(sqlScript, "BDX Lookup Matching", False, vResultArray:=resultObject) 
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'BDX Lookup Matching' with the following SQL - " & sqlScript)
        End If

        If IsArray(resultObject) Then
            Dim resultArray As System.Array = resultObject
            LookupValue = resultArray(0, 0)
        End If
    End Sub

    Friend Shared Function GetEquationValue(ByVal thisEquation As String, ByVal worksheet As Worksheet, ByVal rowCnt As Integer) As Decimal

        Dim sNewEquationString As String = ""
        Dim sCurrentCell As String = ""
        Dim sCurrentEquationString As String = ""
        Dim sCurrentCellValue As String = ""
        Dim dCalculatedValue As Decimal = 0
        Dim sTempEquation As String = thisEquation
        Try
            For icnt As Integer = 0 To sTempEquation.Length - 1
                If sTempEquation.Substring(icnt, 1) = "$" Then
                    sCurrentEquationString = sTempEquation.Substring(icnt + 1, 2)
                    sCurrentCell = "$"
                    If (sCurrentEquationString.Substring(1, 1).ToString.ToUpper >= "A") And (sCurrentEquationString.Substring(1, 1).ToString.ToUpper <= "Z") Then
                        sCurrentCell = sCurrentCell + sCurrentEquationString
                        icnt = icnt + 2
                    Else
                        sCurrentCell = sCurrentCell + sTempEquation.Substring(icnt + 1, 1)
                        icnt = icnt + 1
                    End If
                    sCurrentCellValue = GetCellvalue(sCurrentCell, worksheet, rowCnt)
                    If String.IsNullOrEmpty(sCurrentCellValue) Then sCurrentCellValue = "0"
                    thisEquation = thisEquation.Replace(sCurrentCell, sCurrentCellValue)
                End If
            Next
            If String.IsNullOrEmpty(thisEquation) = False Then
                Dim dt As New Data.DataTable
                dCalculatedValue = ToSafeDecimal(dt.Compute(thisEquation, ""), 0)
                dt = Nothing
            End If
        Catch
            dCalculatedValue = 0
        End Try
        Return dCalculatedValue
    End Function


#End Region

End Class

