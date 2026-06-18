Option Strict Off

Imports SSP.Shared
Imports System.Xml

Friend Class XMLSupport

    Friend Const kPMXMLTooManyDimensions As Integer = 2101
    Friend Const kPMXMLNotEnoughRows As Integer = 2102
    Friend Const kPMXMLNotEnoughColumns As Integer = 2103
    Friend Const kPMXMLParseError As Integer = 2104
    Friend Const kPMBackOfficeError As Integer = 3000
    Friend Const kPMBusinessRuleError As Integer = 3001
    Friend Const kPMLeadClientOnInstalments As Integer = 4001

    Friend Structure PMXMLWarningMsgStruct
        Dim sMethodName As String
        Dim nNumber As Integer
        Dim sTitle As String
        Dim sMessage As String
    End Structure

    Friend Structure PMXMLWarningMsg
        Dim oList() As PMXMLWarningMsgStruct
        Dim nCount As Integer
    End Structure

    Friend Structure PMXMLErrorInvalidDataStruct
        Dim sMethodName As String
        Dim sFieldName As String
        Dim nCode As PMEReturnCode
        Dim sDescription As String
        Dim oSuppliedValue As Object
    End Structure

    Friend Structure PMXMLErrorBusinessRuleStruct
        Dim sMethodName As String
        Dim nCode As PMEReturnCode
        Dim sDescription As String
        Dim oDetail As Object
    End Structure

    Friend Structure PMXMLErrorBackOfficeStruct
        Dim sMethodName As String
        Dim nReturnValue As Integer
        Dim sDescription As String
    End Structure

    Friend Structure PMXMLErrorInternalExceptionStruct
        Dim sMethodName As String
        Dim sDescription As String
    End Structure

    Friend Structure PMXMLErrorInvalidData
        Dim oDetails() As PMXMLErrorInvalidDataStruct
        Dim nCount As Integer
    End Structure

    Friend Structure PMXMLErrorBusinessRule
        Dim oDetail As PMXMLErrorBusinessRuleStruct
        Dim nCount As Integer
    End Structure

    Friend Structure PMXMLErrorBackOffice
        Dim oDetail As PMXMLErrorBackOfficeStruct
        Dim nCount As Integer
    End Structure

    Friend Structure PMXMLErrorInternalException
        Dim oDetail As PMXMLErrorInternalExceptionStruct
        Dim nCount As Integer
    End Structure

    Friend Structure PMXMLErrorTypes
        Dim oInvalidData As PMXMLErrorInvalidData
        Dim oBusinessRule As PMXMLErrorBusinessRule
        Dim oBackOffice As PMXMLErrorBackOffice
        Dim oInternalException As PMXMLErrorInternalException
    End Structure

    Private Structure SAFEARRAYBOUND
        Dim nElements As Integer ' # of elements in the array dimension
        Dim nLbound As Integer ' lower bounds of the array dimension
    End Structure

    Private Structure SAFEARRAY
        Dim nDims As Integer ' Count of dimensions in this array.
        Dim nFeatures As Integer ' Flags used by the SAFEARRAY routines documented
        Dim nbElements As Integer ' Size of an element of the array.
        Dim nLocks As Integer ' Number of times the array has been
        Dim nPData As Integer ' Pointer to the data.
        Dim oGSAbound() As SAFEARRAYBOUND ' One bound for each dimension.

    End Structure

    Private Const kVT_BYREF As Integer = &H4000

    ' Maximum number of array dimensions allowed i.e. can have dim fred(1) or dim fred(1,1) but not dim fred(1,1,1)
    Private Const kMaxNumberArrayDimensions As Integer = 2
    ''' <summary>
    ''' Get Array Info
    ''' </summary>
    ''' <param name="r_aTheArray"></param>
    ''' <param name="r_oArrayInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetArrayInfo(ByRef r_aTheArray As System.Array, ByRef r_oArrayInfo As SAFEARRAY) As Integer

        Try
            If Not Informations.IsArray(r_aTheArray) Then Exit Function
            r_oArrayInfo.nDims = r_aTheArray.Rank

            ReDim r_oArrayInfo.oGSAbound(r_oArrayInfo.nDims)

            If r_oArrayInfo.nDims > 1 Then
                r_oArrayInfo.oGSAbound(1).nElements = r_aTheArray.GetLength(1)
                r_oArrayInfo.oGSAbound(1).nLbound = r_aTheArray.GetLowerBound(1)
                r_oArrayInfo.oGSAbound(2).nElements = r_aTheArray.GetLength(0)
                r_oArrayInfo.oGSAbound(2).nLbound = r_aTheArray.GetLowerBound(0)
            Else
                r_oArrayInfo.oGSAbound(1).nElements = r_aTheArray.GetLength(0)
                r_oArrayInfo.oGSAbound(1).nLbound = r_aTheArray.GetLowerBound(0)
            End If
        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "GetArrayInfo." & ex.Message)
        End Try
        GetArrayInfo = r_oArrayInfo.nDims
    End Function
    ''' <summary>
    ''' Store Item Array
    ''' </summary>
    ''' <param name="r_oDataStore"></param>
    ''' <param name="r_nLastPos"></param>
    ''' <param name="v_sItemName"></param>
    ''' <param name="v_oItemData"></param>
    ''' <remarks></remarks>
    Friend Sub StoreItemArray(ByRef r_oDataStore As Object, ByRef r_nLastPos As Integer, ByVal v_sItemName As String, ByVal v_oItemData As Object)
        Dim sDataType As String = String.Empty
        Dim oArrayInfo As SAFEARRAY = Nothing
        Dim nArrayDims As Integer
        Dim oArrayDimInfo As SAFEARRAYBOUND
        Dim sDims As String = String.Empty
        Dim nDims As Integer
        Dim nColumns As Integer
        Dim nRows As Integer
        Dim nRow As Integer
        Dim nColumn As Integer
        Dim nRowLowBound As Integer
        Dim nColumnLowBound As Integer
        Dim oArrayItem As Object = Nothing
        Dim sTypeName As String = String.Empty
        Dim sVarContents As String = String.Empty
        Dim oValue As Object = Nothing
        Dim sArrayBase As String = String.Empty
        Try
            If Informations.IsArray(v_oItemData) = False Then
                oValue = GetFormattedValue(v_oItemData, sTypeName, sVarContents)
                Append(r_oDataStore, r_nLastPos, "<" & v_sItemName & " value='" & oValue & "' datatype='" & sTypeName.ToUpper & "'" & If(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>"))
                Exit Sub
            End If

            nArrayDims = GetArrayInfo(v_oItemData, oArrayInfo)

            nDims = oArrayInfo.nDims

            If nDims = 0 Then
                StoreItem(r_oDataStore, r_nLastPos, v_sItemName, v_oItemData)
                Exit Sub
            End If

            ' Not allowed to have more than a specific number of dimensions
            If nDims > kMaxNumberArrayDimensions Then
                gPMFunctions.RaiseError(CStr(kPMXMLTooManyDimensions), "StoreItemArray,Error: Passed array has more than 2 dimensions")
                Exit Sub
            End If

            ' Get array row information
            If nDims > 1 Then
                oArrayDimInfo = oArrayInfo.oGSAbound(2)
            Else
                oArrayDimInfo = oArrayInfo.oGSAbound(1)
            End If

            nRows = oArrayDimInfo.nElements
            nRowLowBound = oArrayDimInfo.nLbound

            If nRowLowBound = 0 Then
                nRows = nRows - 1
            End If

            ' Get array column information
            If nDims > 1 Then
                oArrayDimInfo = oArrayInfo.oGSAbound(1)
                nColumns = oArrayDimInfo.nElements
                nColumnLowBound = oArrayDimInfo.nLbound

                If nColumnLowBound = 0 Then
                    nColumns = nColumns - 1
                End If

                sDims = nRows & "," & nColumns
                sArrayBase = nRowLowBound & "," & nColumnLowBound
            Else
                sDims = CStr(nRows)
                sArrayBase = CStr(nRowLowBound)
            End If

            sDataType = v_oItemData.GetType().ToString
            Append(r_oDataStore, r_nLastPos, "<" & v_sItemName & " type='" & sDataType & "' arraydimensions='" & sDims & "' arraybase='" & sArrayBase & "'>")

            '   Store row and column data
            For nRow = nRowLowBound To nRows
                Append(r_oDataStore, r_nLastPos, "<ROW>")

                If nDims > 1 Then
                    For nColumn = nColumnLowBound To nColumns
                        oArrayItem = GetFormattedValue(v_oItemData(nRow, nColumn), sTypeName, sVarContents)

                        If Right(sTypeName, 2) = "()" Then
                            Append(r_oDataStore, r_nLastPos, "<COL value='' datatype='" & sTypeName & "'>")
                            StoreItemArray(r_oDataStore, r_nLastPos, "subArray", v_oItemData(nRow, nColumn))
                            Append(r_oDataStore, r_nLastPos, "</COL>")
                        Else
                            Append(r_oDataStore, r_nLastPos, "<COL value='" & oArrayItem & "' datatype='" & sTypeName & "'" & If(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>"))
                        End If
                    Next nColumn
                Else
                    oArrayItem = GetFormattedValue(v_oItemData(nRow), sTypeName, sVarContents)
                    If Right(sTypeName, 2) = "()" Then
                        Append(r_oDataStore, r_nLastPos, "<COL value='' datatype='" & sTypeName & "'>")
                        StoreItemArray(r_oDataStore, r_nLastPos, "subArray", v_oItemData(nRow))
                        Append(r_oDataStore, r_nLastPos, "</COL>")
                    Else
                        Append(r_oDataStore, r_nLastPos, "<COL value='" & oArrayItem & "' datatype='" & sTypeName & "'" & If(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>"))
                    End If
                End If
                Append(r_oDataStore, r_nLastPos, "</ROW>")
            Next nRow
            Append(r_oDataStore, r_nLastPos, "</" & v_sItemName & ">")
        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "StoreItemArray" & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Store Item
    ''' </summary>
    ''' <param name="r_oDataStore"></param>
    ''' <param name="r_nLastPos"></param>
    ''' <param name="v_sItemName"></param>
    ''' <param name="v_oItemData"></param>
    ''' <remarks></remarks>
    Friend Sub StoreItem(ByRef r_oDataStore As Object, ByRef r_nLastPos As Integer, ByVal v_sItemName As String, ByVal v_oItemData As Object)
        Dim sTypeName As String = String.Empty
        Dim oValue As Object = Nothing
        Dim sVarContents As String = String.Empty
        Try

            oValue = GetFormattedValue(v_oItemData, sTypeName, sVarContents)
            Append(r_oDataStore, r_nLastPos, "<" & v_sItemName & " value='" & oValue & "' datatype='" & sTypeName.ToLower & "'" & If(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>"))

        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "StoreItem" & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Export ItemArray
    ''' </summary>
    ''' <param name="v_oNode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function ExportItemArray(ByVal v_oNode As XmlNode) As Object
        Dim oArray As Object = Nothing
        Dim sArrayDims As String = String.Empty
        Dim oArrayDims As Object = Nothing
        Dim nRows As Integer
        Dim nCols As Integer
        Dim oRow As XmlNode
        Dim oCol As XmlNode
        Dim nColCount As Integer
        Dim nRowCount As Integer
        Dim oColData As Object = Nothing
        Dim oArrayAttribute As XmlAttribute
        Dim oArrayBaseAttribute As XmlAttribute
        Dim nMultiDimension As Integer
        Dim oAttributeVarContents As XmlAttribute
        Dim oAttributeType As XmlAttribute
        Dim oAttributeValue As XmlAttribute
        Dim sArrayBase As String = String.Empty
        Dim oArrayBaseDims As Object = Nothing
        Dim nRowBase As Integer
        Dim nColBase As Integer
        Dim nRowBaseModifier As Integer
        Dim nColBaseModifier As Integer
        Dim nColStartCount As Integer
        Dim nRowStartCount As Integer
        Dim oSubArray As XmlNode
        Try
            oArrayAttribute = v_oNode.SelectSingleNode("@arraydimensions")

            'check to see if the value we are working with is actually an array
            If Not oArrayAttribute Is Nothing Then
                sArrayDims = oArrayAttribute.Value
            Else
                ExportItemArray = ExportItem(v_oNode)
                Exit Function
            End If

            'get attribute for array dimension base (i.e. the start point for each array element)
            oArrayBaseAttribute = v_oNode.SelectSingleNode("@arraybase")

            'check attribute that tells us the base of the array dimensions
            If Not oArrayBaseAttribute Is Nothing Then
                sArrayBase = oArrayBaseAttribute.Value
            End If

            ' See if this is a single or multi dimension array
            nMultiDimension = sArrayDims.IndexOf(",") + 1

            If nMultiDimension > 0 Then
                oArrayDims = sArrayDims.Split(",")

                If UBound(oArrayDims) > 1 Then
                    gPMFunctions.RaiseError(CStr(kPMXMLTooManyDimensions), "ExportItemArray, Error: Stored array has more than 2 dimensions")
                    Return Nothing
                End If

                'check to see if the "arraybase" attribute was present or not - we need it, so complain if it isn't
                If Trim(sArrayBase) = "" Then
                    gPMFunctions.RaiseError(CStr(kPMXMLTooManyDimensions), "ExportItemArray, Error: 'arraybase' attribute is missing, cannot determine base for array dimensions")
                    Return Nothing
                End If

                oArrayBaseDims = sArrayBase.Split(",")

                'check indicated number of array dimensions - "arraybase" attribute should have one for each of the array dimensions
                If UBound(oArrayBaseDims) > 1 Then
                    gPMFunctions.RaiseError(CStr(kPMXMLTooManyDimensions), "ExportItemArray, Error: 'arraybase' attribute indicates array has more than 2 dimensions")
                    Return Nothing
                End If

                If UBound(oArrayBaseDims) <> UBound(oArrayDims) Then
                    gPMFunctions.RaiseError(CStr(kPMXMLTooManyDimensions), "ExportItemArray, Error: 'arraybase' attribute does not have same number of dimensions as the 'arraydimensions' attribute")
                    Return Nothing
                End If

                ' Get array dimensions
                nRows = oArrayDims(0)
                nCols = oArrayDims(1)
                nRowBase = oArrayBaseDims(0)
                nColBase = oArrayBaseDims(1)

                ReDim oArray(nRows, nCols)
            Else
                nRows = CInt(sArrayDims)
                nCols = 0
                nRowBase = CInt(sArrayBase)
                nColBase = 0

                ReDim oArray(nRows)
            End If

            nColStartCount = 1
            nRowStartCount = 1

            'check array "row" start point
            If nRowBase = 0 Then
                nRowBaseModifier = 1
                nRowStartCount = 0
            End If

            'check array "column" start point
            If nColBase = 0 Then
                nColBaseModifier = 1
                nColStartCount = 0
            End If

            ' Check if there are any ROWs
            If v_oNode.SelectNodes("ROW").Count = 0 Then
                gPMFunctions.RaiseError(CStr(kPMXMLNotEnoughRows), "ExportItemArray, Error: No row elements defined - expected " & nRows & " rows")
                Return Nothing
            End If

            ' Check if we found correct number of rows
            If (v_oNode.SelectNodes("ROW").Count - nRowBaseModifier) <> nRows Then
                gPMFunctions.RaiseError(CStr(kPMXMLNotEnoughRows), "ExportItemArray, Error: Stored rows does not match stored array dimensions - expected " & nRows & " rows, but found " & v_oNode.SelectNodes("ROW").Count & " instead.")
                Return Nothing
            End If

            nRowCount = nRowStartCount
            ' Loop through all rows
            oRow = v_oNode.FirstChild

            While Not (oRow Is Nothing)
                ' Check if there are any COLs
                If oRow.ChildNodes.Count = 0 Then
                    gPMFunctions.RaiseError(CStr(kPMXMLNotEnoughRows), "ExportItemArray, Error: No COL elements defined for row - expected " & nCols & " columns")
                    Return Nothing
                End If

                ' Check if we found correct number of columns
                If (oRow.ChildNodes.Count - nColBaseModifier) <> nCols Then
                    gPMFunctions.RaiseError(CStr(kPMXMLNotEnoughRows), "ExportItemArray, Error: Stored columns does not match stored array dimensions - expected " & nCols & " columns, but found " & oRow.ChildNodes.Count & " COL nodes instead.")
                    Return Nothing
                End If

                nColCount = nColStartCount

                ' Loop through all columns
                oCol = oRow.FirstChild

                While Not (oCol Is Nothing)
                    oAttributeVarContents = oCol.SelectSingleNode("@varcontents")
                    oAttributeType = oCol.SelectSingleNode("@datatype")
                    oAttributeValue = oCol.SelectSingleNode("@value")

                    'attribute for "value" should always be present...
                    If Not oAttributeValue Is Nothing Then
                        oColData = oAttributeValue.Value
                    End If

                    If Not oAttributeType Is Nothing Then
                        If Right(oAttributeType.InnerText, 2) = "()" Then
                            oSubArray = oCol.SelectSingleNode("subArray")
                            oColData = ExportItemArray(oSubArray)
                        End If
                    End If

                    'check attribute which tells us about the variable contents
                    If Not oAttributeVarContents Is Nothing Then
                        Select Case oAttributeVarContents.InnerText.ToUpper
                            Case "EMPTY"
                                oColData = Nothing
                            Case "NULL"
                                oColData = System.DBNull.Value
                        End Select
                    End If

                    ' Store data
                    If nMultiDimension > 0 Then
                        oArray(nRowCount, nColCount) = oColData
                    Else
                        oArray(nRowCount) = oColData
                    End If

                    oAttributeVarContents = Nothing
                    oAttributeType = Nothing
                    oAttributeValue = Nothing
                    nColCount = nColCount + 1

                    oCol = oCol.NextSibling
                End While
                nRowCount = nRowCount + 1
                oRow = oRow.NextSibling
            End While

            ExportItemArray = oArray

        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "ExportItemArray." & ex.Message)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Export MessageArray
    ''' </summary>
    ''' <param name="v_oNodes"></param>
    ''' <param name="r_bHasMessages"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function ExportMessageArray(ByVal v_oNodes As XmlNodeList, ByRef r_bHasMessages As Boolean) As PMXMLWarningMsg
        Dim oMessageList() As PMXMLWarningMsgStruct
        Dim oMessage As PMXMLWarningMsg = Nothing
        Dim oNode As XmlNode
        Dim nCount As Integer

        r_bHasMessages = False
        Try
            If Not v_oNodes Is Nothing Then
                If v_oNodes.Count > 0 Then
                    r_bHasMessages = True
                    ReDim oMessageList(v_oNodes.Count - 1)
                    nCount = 0

                    For Each oNode In v_oNodes
                        oMessageList(nCount).sMethodName = oNode.SelectSingleNode("@method").InnerText
                        oMessageList(nCount).nNumber = gPMFunctions.ToSafeInteger(oNode.SelectSingleNode("@number").InnerText)
                        oMessageList(nCount).sTitle = oNode.SelectSingleNode("@title").InnerText
                        oMessageList(nCount).sMessage = oNode.SelectSingleNode("@message").InnerText
                        nCount = nCount + 1
                    Next oNode

                    oMessage.oList = oMessageList
                    oMessage.nCount = nCount

                End If
            End If

        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "ExportMessageArray." & ex.Message)
        End Try
        Return oMessage
    End Function
    ''' <summary>
    ''' Export ErrorArray
    ''' </summary>
    ''' <param name="v_oNodes"></param>
    ''' <param name="r_bHasErrors"></param>
    ''' <param name="r_bHasInvalidDataErrors"></param>
    ''' <param name="r_bHasBusinessRuleErrors"></param>
    ''' <param name="r_bHasBackOfficeErrors"></param>
    ''' <param name="r_bHasInternalExceptionErrors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function ExportErrorArray(ByVal v_oNodes As XmlNodeList, ByRef r_bHasErrors As Boolean, ByRef r_bHasInvalidDataErrors As Boolean, ByRef r_bHasBusinessRuleErrors As Boolean,
                                     ByRef r_bHasBackOfficeErrors As Boolean, ByRef r_bHasInternalExceptionErrors As Boolean) As PMXMLErrorTypes
        Dim oErrors As PMXMLErrorTypes = Nothing
        Dim oInvalidData() As PMXMLErrorInvalidDataStruct
        Dim oBusinessRule As PMXMLErrorBusinessRuleStruct
        Dim oBackOffice As PMXMLErrorBackOfficeStruct
        Dim oInternalException As PMXMLErrorInternalExceptionStruct
        Dim oErrorNodes As XmlNodeList
        Dim oErrorNode As XmlNode
        Dim nErrorCount As Integer

        r_bHasErrors = False
        Try
            If Not v_oNodes Is Nothing Then
                If v_oNodes.Item(0).ChildNodes.Count > 0 Then
                    oErrorNodes = v_oNodes.Item(0).SelectNodes("INVALIDDATA/INVALIDDATAERROR")
                    oInvalidData = ExportInvalidDataError(oErrorNodes, r_bHasInvalidDataErrors, nErrorCount)

                    oErrors.oInvalidData.oDetails = oInvalidData
                    oErrors.oInvalidData.nCount = nErrorCount

                    oErrorNode = v_oNodes.Item(0).SelectSingleNode("BUSINESSRULE")
                    oBusinessRule = ExportBusinessRuleError(oErrorNode, r_bHasBusinessRuleErrors, nErrorCount)

                    oErrors.oBusinessRule.oDetail = oBusinessRule
                    oErrors.oBusinessRule.nCount = nErrorCount

                    oErrorNode = v_oNodes.Item(0).SelectSingleNode("BACKOFFICE")
                    oBackOffice = ExportBackOfficeError(oErrorNode, r_bHasBackOfficeErrors, nErrorCount)

                    oErrors.oBackOffice.oDetail = oBackOffice
                    oErrors.oBackOffice.nCount = nErrorCount

                    oErrorNode = v_oNodes.Item(0).SelectSingleNode("INTERNALEXCEPTION")
                    oInternalException = ExportInternalExceptionError(oErrorNode, r_bHasInternalExceptionErrors, nErrorCount)

                    oErrors.oInternalException.oDetail = oInternalException
                    oErrors.oInternalException.nCount = nErrorCount

                    If r_bHasInvalidDataErrors Or r_bHasBusinessRuleErrors Or r_bHasBackOfficeErrors Or r_bHasInternalExceptionErrors Then
                        r_bHasErrors = True
                    End If
                End If
            End If
        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "ExportErrorArray." & ex.Message)
        End Try
        Return oErrors
    End Function
    ''' <summary>
    ''' Export InvalidData Error
    ''' </summary>
    ''' <param name="v_oNodes"></param>
    ''' <param name="r_bHasErrors"></param>
    ''' <param name="r_nCount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExportInvalidDataError(ByVal v_oNodes As XmlNodeList, ByRef r_bHasErrors As Boolean, ByRef r_nCount As Integer) As PMXMLErrorInvalidDataStruct()
        Dim oNode As XmlNode
        Dim oInvalidData() As PMXMLErrorInvalidDataStruct = Nothing
        r_nCount = 0
        Try
            If Not v_oNodes Is Nothing Then
                If v_oNodes.Count > 0 Then
                    r_bHasErrors = True
                    ReDim oInvalidData(v_oNodes.Count - 1)
                    For Each oNode In v_oNodes
                        oInvalidData(r_nCount).sMethodName = oNode.SelectSingleNode("@method").InnerText
                        oInvalidData(r_nCount).nCode = CType(oNode.SelectSingleNode("@code").InnerText, PMEReturnCode)
                        oInvalidData(r_nCount).sDescription = oNode.SelectSingleNode("@description").InnerText
                        oInvalidData(r_nCount).sFieldName = oNode.SelectSingleNode("@fieldname").InnerText
                        oInvalidData(r_nCount).oSuppliedValue = oNode.SelectSingleNode("@suppliedvalue")
                        r_nCount = r_nCount + 1
                    Next oNode
                End If
            End If
        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "ExportInvalidDataError." & ex.Message)
        End Try
        Return oInvalidData
    End Function
    ''' <summary>
    ''' Export BusinessRule Error
    ''' </summary>
    ''' <param name="v_oNode"></param>
    ''' <param name="r_bHasErrors"></param>
    ''' <param name="r_nCount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExportBusinessRuleError(ByVal v_oNode As XmlNode, ByRef r_bHasErrors As Boolean, ByRef r_nCount As Integer) As PMXMLErrorBusinessRuleStruct
        Dim oBusinessRule As PMXMLErrorBusinessRuleStruct = Nothing
        r_nCount = 0
        Try
            If Not v_oNode Is Nothing Then
                If v_oNode.Attributes.Count > 0 Then
                    r_bHasErrors = True
                    oBusinessRule.sMethodName = v_oNode.SelectSingleNode("@method").InnerText
                    oBusinessRule.nCode = CType(v_oNode.SelectSingleNode("@code").InnerText, PMEReturnCode)
                    oBusinessRule.sDescription = v_oNode.SelectSingleNode("@description").InnerText
                    oBusinessRule.oDetail = v_oNode.SelectSingleNode("@detail")
                    r_nCount = 1
                End If
            End If
        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "ExportBusinessRuleError." & ex.Message)
        End Try
        Return oBusinessRule
    End Function
    ''' <summary>
    ''' Export BackOffice Error
    ''' </summary>
    ''' <param name="v_oNode"></param>
    ''' <param name="r_bHasErrors"></param>
    ''' <param name="r_nCount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExportBackOfficeError(ByVal v_oNode As XmlNode, ByRef r_bHasErrors As Boolean, ByRef r_nCount As Integer) As PMXMLErrorBackOfficeStruct
        Dim oBackOffice As PMXMLErrorBackOfficeStruct = Nothing
        r_nCount = 0
        Try
            If Not v_oNode Is Nothing Then
                If v_oNode.Attributes.Count > 0 Then
                    r_bHasErrors = True
                    oBackOffice.sMethodName = v_oNode.SelectSingleNode("@method").InnerText
                    oBackOffice.sDescription = v_oNode.SelectSingleNode("@description").InnerText
                    oBackOffice.nReturnValue = gPMFunctions.ToSafeInteger(v_oNode.SelectSingleNode("@returnvalue").InnerText)
                    r_nCount = 1
                End If

            End If
        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "ExportBackOfficeError." & ex.Message)
        End Try
        Return oBackOffice
    End Function
    ''' <summary>
    ''' Export Internal Exception Error
    ''' </summary>
    ''' <param name="v_oNode"></param>
    ''' <param name="r_bHasErrors"></param>
    ''' <param name="r_nCount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExportInternalExceptionError(ByVal v_oNode As XmlNode, ByRef r_bHasErrors As Boolean, ByRef r_nCount As Integer) As PMXMLErrorInternalExceptionStruct
        Dim oInternalException As PMXMLErrorInternalExceptionStruct = Nothing
        r_nCount = 0
        Try
            If Not v_oNode Is Nothing Then
                If v_oNode.Attributes.Count > 0 Then
                    r_bHasErrors = True
                    oInternalException.sMethodName = v_oNode.SelectSingleNode("@method").InnerText
                    oInternalException.sDescription = v_oNode.SelectSingleNode("@description").InnerText
                    r_nCount = 1
                End If
            End If

        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "ExportInternalExceptionError." & ex.Message)
        End Try
        Return oInternalException
    End Function
    ''' <summary>
    ''' Format Date Canonical
    ''' </summary>
    ''' <param name="v_dtDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function FormatDateCanonical(ByVal v_dtDate As Date) As String
        Try
            FormatDateCanonical = v_dtDate.ToString("yyyy-MM-dd HH:mm:ss")
        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "FormatDateCanonical." & ex.Message)
            FormatDateCanonical = String.Empty
        End Try
    End Function
    ''' <summary>
    ''' Format Safe XMLString
    ''' </summary>
    ''' <param name="v_sString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function FormatSafeXMLString(ByVal v_sString As String) As String
        Try
            ' mark any strings that have already been made safe so that the original formatting will not be
            ' lost when we unformatted it later
            v_sString = v_sString.Replace("&lt;", "##LT##") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("&gt;", "##GT##") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("&amp;", "##AMP##") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("&apos;", "##APOS##") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("&quot;", "##QUOT##") ' RAW 06/09/2004 : added

            v_sString = v_sString.Replace("<", "&lt;")
            v_sString = v_sString.Replace(">", "&gt;")
            v_sString = v_sString.Replace("&", "&amp;")
            v_sString = v_sString.Replace("'", "&apos;")
            v_sString = v_sString.Replace(ChrW(34), "&quot;")
        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "FormatSafeXMLString." & ex.Message)
        End Try
        FormatSafeXMLString = v_sString
    End Function
    ''' <summary>
    ''' Unformat Safe XMLString
    ''' </summary>
    ''' <param name="v_sString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function UnformatSafeXMLString(ByVal v_sString As String) As String
        Try
            v_sString = v_sString.Replace("&lt;", "<")
            v_sString = v_sString.Replace("&gt;", ">")
            v_sString = v_sString.Replace("&amp;", "&")
            v_sString = v_sString.Replace("&apos;", "'")
            v_sString = v_sString.Replace("&quot;", ChrW(34))

            ' now unformat any strings that are marked as being already been made safe
            v_sString = v_sString.Replace("##LT##", "&lt;") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("##GT##", "&gt;") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("##AMP##", "&amp;") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("##APOS##", "&apos;") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("##QUOT##", "&quot;") ' RAW 06/09/2004 : added

        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "UnformatSafeXMLString." & ex.Message)
        End Try

        UnformatSafeXMLString = v_sString
    End Function
    ''' <summary>
    ''' Export Item
    ''' </summary>
    ''' <param name="v_oNode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function ExportItem(ByVal v_oNode As XmlNode) As Object
        Dim oAttributeVarContents As XmlAttribute
        Dim oAttributeType As XmlAttribute
        Dim oAttributeValue As XmlAttribute
        Dim oRetValue As Object = Nothing
        Try
            If Not v_oNode Is Nothing Then
                oAttributeVarContents = v_oNode.SelectSingleNode("@varcontents")
                oAttributeType = v_oNode.SelectSingleNode("@datatype")
                oAttributeValue = v_oNode.SelectSingleNode("@value")

                'attribute for "value" should always be present...
                If Not oAttributeValue Is Nothing Then
                    oRetValue = oAttributeValue.Value

                    If oAttributeType.InnerText.ToUpper = "STRING" Then
                        oRetValue = UnformatSafeXMLString(oRetValue)
                    End If
                End If

                'check attribute which tells us about the variable contents
                If Not oAttributeVarContents Is Nothing Then
                    Select Case oAttributeVarContents.InnerText.ToUpper
                        Case "EMPTY"
                            oRetValue = Nothing
                        Case "NULL"
                            oRetValue = System.DBNull.Value
                    End Select
                End If

                oAttributeVarContents = Nothing
                oAttributeType = Nothing
                oAttributeValue = Nothing
            End If
        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "ExportItem." & ex.Message)
        End Try
        ExportItem = oRetValue
    End Function
    ''' <summary>
    ''' GetFormattedValue
    ''' </summary>
    ''' <param name="v_oValue"></param>
    ''' <param name="r_sDataType"></param>
    ''' <param name="r_sVarContents"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFormattedValue(ByVal v_oValue As Object, ByRef r_sDataType As String, ByRef r_sVarContents As String) As Object
        Dim sTypeName As String = String.Empty
        Dim oValue As Object = Nothing
        Try
            sTypeName = v_oValue.GetType.ToString()
            r_sDataType = sTypeName
            r_sVarContents = ""

            If Right(sTypeName, 2) = "()" Then
                sTypeName = Informations.Left(sTypeName, Len(sTypeName) - 2)
            End If

            'check data type of passed data
            Select Case (sTypeName.ToUpper)
                Case "DATE"
                    oValue = FormatDateCanonical(v_oValue)
                Case "STRING"
                    oValue = FormatSafeXMLString(v_oValue)
                Case Else
                    oValue = v_oValue
            End Select

            'check contents of passed data
            If Informations.IsNothing(v_oValue) Then
                r_sVarContents = "empty"
            ElseIf informations.IsDBNull(v_oValue) Then
                r_sVarContents = "null"
            End If

        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "GetFormattedValue." & ex.Message)
        End Try

        GetFormattedValue = oValue
    End Function
    ''' <summary>
    ''' Append
    ''' </summary>
    ''' <param name="r_oDataStore"></param>
    ''' <param name="r_nLastPos"></param>
    ''' <param name="v_sAppendData"></param>
    ''' <remarks></remarks>
    Friend Sub Append(ByRef r_oDataStore As Object, ByRef r_nLastPos As Integer, ByVal v_sAppendData As String)
        Const KExtendArray As Integer = 1000
        Try

            If Informations.IsArray(r_oDataStore) = False Then
                ReDim r_oDataStore(KExtendArray)
            Else
                If r_nLastPos > UBound(r_oDataStore) Then
                    ReDim Preserve r_oDataStore(r_nLastPos + KExtendArray)
                End If
            End If

            r_oDataStore(r_nLastPos) = v_sAppendData

            r_nLastPos = r_nLastPos + 1

        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "Append" & ex.Message)
        End Try
    End Sub

End Class

