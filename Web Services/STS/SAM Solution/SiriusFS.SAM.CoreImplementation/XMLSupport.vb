Option Strict Off

Imports System.Xml
Imports SSP.Shared
Imports SSP.Shared.gPMConstants

Friend Class XMLSupport

    Friend Const PMXMLTooManyDimensions As Short = 2101
    Friend Const PMXMLNotEnoughRows As Short = 2102
    Friend Const PMXMLNotEnoughColumns As Short = 2103
    Friend Const PMXMLParseError As Short = 2104
    Friend Const PMBackOfficeError As Short = 3000
    Friend Const PMBusinessRuleError As Short = 3001
    Friend Const PMLeadClientOnInstalments As Short = 4001

    'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1016"'
    'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1016"'
    'Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef dest As Any, ByRef source As Any, ByVal bytes As Integer)

    Friend Structure PMXMLWarningMsgStruct
        Dim MethodName As String
        Dim Number As Integer
        Dim Title As String
        Dim message As String
    End Structure

    Friend Structure PMXMLWarningMsg
        Dim List() As PMXMLWarningMsgStruct
        Dim Count As Integer
    End Structure

    Friend Structure PMXMLErrorInvalidDataStruct
        Dim MethodName As String
        Dim FieldName As String
        'UPGRADE_ISSUE: PMEReturnCode object was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2068"'
        Dim Code As PMEReturnCode
        Dim Description As String
        Dim SuppliedValue As Object
    End Structure

    Friend Structure PMXMLErrorBusinessRuleStruct
        Dim MethodName As String
        'UPGRADE_ISSUE: PMEReturnCode object was not upgraded. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2068"'
        Dim Code As PMEReturnCode
        Dim Description As String
        Dim Detail As Object
    End Structure

    Friend Structure PMXMLErrorBackOfficeStruct
        Dim MethodName As String
        Dim ReturnValue As Integer
        Dim Description As String
    End Structure

    Friend Structure PMXMLErrorInternalExceptionStruct
        Dim MethodName As String
        Dim Description As String
    End Structure

    Friend Structure PMXMLErrorInvalidData
        Dim Details() As PMXMLErrorInvalidDataStruct
        Dim Count As Integer
    End Structure

    Friend Structure PMXMLErrorBusinessRule
        Dim Detail As PMXMLErrorBusinessRuleStruct
        Dim Count As Integer
    End Structure

    Friend Structure PMXMLErrorBackOffice
        Dim Detail As PMXMLErrorBackOfficeStruct
        Dim Count As Integer
    End Structure

    Friend Structure PMXMLErrorInternalException
        Dim Detail As PMXMLErrorInternalExceptionStruct
        Dim Count As Integer
    End Structure

    Friend Structure PMXMLErrorTypes
        Dim InvalidData As PMXMLErrorInvalidData
        Dim BusinessRule As PMXMLErrorBusinessRule
        Dim BackOffice As PMXMLErrorBackOffice
        Dim InternalException As PMXMLErrorInternalException
    End Structure

    '---------------------------------------------------------------------------------------------------------------

    Private Structure SAFEARRAYBOUND
        Dim cElements As Integer ' # of elements in the array dimension
        Dim lLbound As Integer ' lower bounds of the array dimension
    End Structure

    Private Structure SAFEARRAY
        Dim cDims As Short ' Count of dimensions in this array.
        Dim fFeatures As Short ' Flags used by the SAFEARRAY routines documented
        ' below.
        Dim cbElements As Integer ' Size of an element of the array.
        Dim cLocks As Integer ' Number of times the array has been
        ' locked without corresponding unlock.
        Dim pvData As Integer ' Pointer to the data.
        <VBFixedArray(60)> Dim rgsabound() As SAFEARRAYBOUND ' One bound for each dimension.
        ' An array can have max 60 dimensions, only the first cDims items will be
        ' used
        ' note that rgsabound elements are in reverse order,
        '  e.g. for a 2-dimensional
        ' array, rgsabound(1) holds info about columns, and rgsabound(2) about rows

    End Structure

    Private Const VT_BYREF As Integer = &H4000

    '---------------------------------------------------------------------------------------------------------------

    ' Maximum number of array dimensions allowed i.e. can have dim fred(1) or dim fred(1,1) but not dim fred(1,1,1)
    Private Const cMaxNumberArrayDimensions As Short = 2

    ' ***************************************************************** '
    ' Name:         GetArrayInfo
    '
    ' Description:  Fills a SAFEARRAY structure for the supplied array.
    '               The information contained in the SAFEARRAY structure allows
    '               the caller to identify the number of dimensions and the
    '               number of elements for each dimension (among other things).
    '               Element information for each dimension is stored in a
    '               one-based sub-array of SAFEARRAYBOUND structures (rgsabound).
    '
    '               TheArray        The array to get information on.
    '               ArrayInfo       The output SAFEARRAY structure.
    '
    '               RETURNS         The number of dimensions of the array
    '                               or zero if the array isn't dimensioned
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Private Function GetArrayInfo(ByRef TheArray As System.Array, ByRef ArrayInfo As SAFEARRAY) As Short

        Dim lp As Integer ' work pointer variable
        Dim VType As Short ' the VARTYPE member of the VARIANT structure]
        Dim iCnt As Integer

        If Not IsArray(TheArray) Then Exit Function

        ArrayInfo.cDims = TheArray.Rank

        ReDim ArrayInfo.rgsabound(ArrayInfo.cDims)

        If ArrayInfo.cDims > 1 Then
            ArrayInfo.rgsabound(1).cElements = TheArray.GetLength(1)
            ArrayInfo.rgsabound(1).lLbound = TheArray.GetLowerBound(1)
            ArrayInfo.rgsabound(2).cElements = TheArray.GetLength(0)
            ArrayInfo.rgsabound(2).lLbound = TheArray.GetLowerBound(0)
        Else
            ArrayInfo.rgsabound(1).cElements = TheArray.GetLength(0)
            ArrayInfo.rgsabound(1).lLbound = TheArray.GetLowerBound(0)
        End If

        GetArrayInfo = ArrayInfo.cDims

        '' Exit if no array supplied
        'If Not IsArray(TheArray) Then Exit Function

        'With ArrayInfo
        '    ' Get the VARTYPE value from the first 2 bytes of the VARIANT structure
        '    'UPGRADE_WARNING: Couldn't resolve default property of object TheArray. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
        '    CopyMemory(VType, TheArray, 2)

        '    ' Get the pointer to the array descriptor (SAFEARRAY structure)
        '    ' NOTE: A Variant's descriptor, padding & union take up 8 bytes.
        '    'UPGRADE_ISSUE: VarPtr function is not supported. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1040"'
        '    CopyMemory(lp, VarPtr(TheArray) + 8, 4)

        '    ' Test if lp is a pointer or a pointer to a pointer.
        '    If (VType And VT_BYREF) <> 0 Then
        '        ' Get real pointer to the array descriptor (SAFEARRAY structure)
        '        CopyMemory(lp, lp, 4)
        '    End If

        '    ' Fill the SAFEARRAY structure with the array info
        '    ' NOTE: The fixed part of the SAFEARRAY structure is 16 bytes.
        '    CopyMemory(ArrayInfo.cDims, lp, 16)

        '    ' Ensure the array has been dimensioned before getting SAFEARRAYBOUND
        '    ' Information
        '    If ArrayInfo.cDims > 0 Then
        '        ' Fill the SAFEARRAYBOUND structures with the array info
        '        'UPGRADE_WARNING: Couldn't resolve default property of object ArrayInfo.rgsabound(). Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
        '        CopyMemory(.rgsabound(1), lp + 16, ArrayInfo.cDims * Len(.rgsabound(1)))

        '        ' So caller knows there is information available for the array in
        '        ' output SAFEARRAY
        '        GetArrayInfo = ArrayInfo.cDims
        '    End If
        '



    End Function

    ' ***************************************************************** '
    ' Name:         StoreItemArray
    '
    ' Description:  Set a value in the property bag
    '
    ' History:      27/05/2004  RVH Created
    '               27/07/2004  RVH Modified to use new Append method
    '                               in an attempt to speed up this sub
    ' ***************************************************************** '
    Friend Sub StoreItemArray(ByRef vDataStore As Object, ByRef lLastPos As Integer, ByVal v_sItemName As String, ByVal v_vItemData As Object)


        Dim sDataType As String = String.Empty
        'UPGRADE_WARNING: Arrays in structure uArrayInfo may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1063"'
        Dim uArrayInfo As SAFEARRAY = Nothing
        Dim iArrayDims As Short
        Dim uArrayDimInfo As SAFEARRAYBOUND
        Dim sDims As String = String.Empty
        Dim iDims As Short
        Dim lColumns As Integer
        Dim lRows As Integer
        Dim lRow As Integer
        Dim lColumn As Integer
        Dim lRowLowBound As Integer
        Dim lColumnLowBound As Integer
        Dim vArrayItem As Object = Nothing
        Dim sTypeName As String = String.Empty
        Dim sVarContents As String = String.Empty
        Dim vValue As Object = Nothing
        Dim sArrayBase As String = String.Empty

        If IsArray(v_vItemData) = False Then
            'UPGRADE_WARNING: Couldn't resolve default property of object GetFormattedValue(). Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
            vValue = GetFormattedValue(v_vItemData, sTypeName, sVarContents)

            'RVH 27/07/2004: Modified to use new Append method
            'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
            Append(vDataStore, lLastPos, "<" & v_sItemName & " value='" & vValue & "' datatype='" & LCase(sTypeName) & "'" & IIf(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>"))
            Exit Sub
        End If

        iArrayDims = GetArrayInfo(v_vItemData, uArrayInfo)

        iDims = uArrayInfo.cDims

        ' Check number of dimensions - if 0 then this is not really an array
        If iDims = 0 Then
            StoreItem(vDataStore, lLastPos, v_sItemName, v_vItemData)
            Exit Sub
        End If

        ' Not allowed to have more than a specific number of dimensions
        If iDims > cMaxNumberArrayDimensions Then
            'UPGRADE_WARNING: Couldn't resolve default property of object PMXMLTooManyDimensions. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
            RaiseError("StoreItemArray", "Error: Passed array has more than 2 dimensions", PMXMLTooManyDimensions)
            Exit Sub
        End If

        ' Get array row information
        If iDims > 1 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object uArrayDimInfo. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
            uArrayDimInfo = uArrayInfo.rgsabound(2)
        Else
            'UPGRADE_WARNING: Couldn't resolve default property of object uArrayDimInfo. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
            uArrayDimInfo = uArrayInfo.rgsabound(1)
        End If

        lRows = uArrayDimInfo.cElements
        lRowLowBound = uArrayDimInfo.lLbound

        If lRowLowBound = 0 Then
            lRows = lRows - 1
        End If

        ' Get array column information
        If iDims > 1 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object uArrayDimInfo. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
            uArrayDimInfo = uArrayInfo.rgsabound(1)
            lColumns = uArrayDimInfo.cElements
            lColumnLowBound = uArrayDimInfo.lLbound

            If lColumnLowBound = 0 Then
                lColumns = lColumns - 1
            End If

            sDims = lRows & "," & lColumns
            sArrayBase = lRowLowBound & "," & lColumnLowBound
        Else
            sDims = CStr(lRows)
            sArrayBase = CStr(lRowLowBound)
        End If

        'UPGRADE_WARNING: TypeName has a new behavior. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1041"'
        sDataType = TypeName(v_vItemData)

        'RVH 27/07/2004: Modified to use new Append method
        Append(vDataStore, lLastPos, "<" & v_sItemName & " type='" & sDataType & "' arraydimensions='" & sDims & "' arraybase='" & sArrayBase & "'>")

        '   Store row and column data
        For lRow = lRowLowBound To lRows
            'RVH 27/07/2004: Modified to use new Append method
            Append(vDataStore, lLastPos, "<ROW>")

            If iDims > 1 Then
                For lColumn = lColumnLowBound To lColumns
                    'UPGRADE_WARNING: Couldn't resolve default property of object GetFormattedValue(). Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object vArrayItem. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                    vArrayItem = GetFormattedValue(v_vItemData(lRow, lColumn), sTypeName, sVarContents)

                    'RVH 27/07/2004: Modified to use new Append method
                    'RVH 27/08/2004: Modified to allow store of arrays embedded in array elements
                    If Right(sTypeName, 2) = "()" Then
                        Append(vDataStore, lLastPos, "<COL value='' datatype='" & sTypeName & "'>")
                        StoreItemArray(vDataStore, lLastPos, "subArray", v_vItemData(lRow, lColumn))
                        Append(vDataStore, lLastPos, "</COL>")
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object vArrayItem. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                        Append(vDataStore, lLastPos, "<COL value='" & vArrayItem & "' datatype='" & sTypeName & "'" & IIf(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>"))
                    End If
                    '                Append vDataStore, lLastPos, "<COL value='" & vArrayItem & "' datatype='" & sTypeName & "'" & IIf(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>")
                Next lColumn
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object GetFormattedValue(). Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vArrayItem. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                vArrayItem = GetFormattedValue(v_vItemData(lRow), sTypeName, sVarContents)
                'RVH 27/07/2004: Modified to use new Append method
                'RVH 27/08/2004: Modified to allow store of arrays embedded in array elements
                If Right(sTypeName, 2) = "()" Then
                    Append(vDataStore, lLastPos, "<COL value='' datatype='" & sTypeName & "'>")
                    StoreItemArray(vDataStore, lLastPos, "subArray", v_vItemData(lRow))
                    Append(vDataStore, lLastPos, "</COL>")
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object vArrayItem. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                    Append(vDataStore, lLastPos, "<COL value='" & vArrayItem & "' datatype='" & sTypeName & "'" & IIf(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>"))
                End If
            End If

            'RVH 27/07/2004: Modified to use new Append method
            Append(vDataStore, lLastPos, "</ROW>")
        Next lRow

        'RVH 27/07/2004: Modified to use new Append method
        Append(vDataStore, lLastPos, "</" & v_sItemName & ">")

        Exit Sub


    End Sub

    ' ***************************************************************** '
    ' Name:         StoreItem
    '
    ' Description:  Set a value in the property bag
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Friend Sub StoreItem(ByRef vDataStore As Object, ByRef lLastPos As Integer, ByVal v_sItemName As String, ByVal v_vItemData As Object)


        Dim sTypeName As String = String.Empty
        Dim vValue As Object = Nothing
        Dim sVarContents As String = String.Empty

        'UPGRADE_WARNING: Couldn't resolve default property of object GetFormattedValue(). Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
        vValue = GetFormattedValue(v_vItemData, sTypeName, sVarContents)

        'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
        Append(vDataStore, lLastPos, "<" & v_sItemName & " value='" & vValue & "' datatype='" & LCase(sTypeName) & "'" & IIf(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>"))

        Exit Sub


    End Sub

    ' ***************************************************************** '
    ' Name:         ExportItemArray
    '
    ' Description:  Re-assemble and export stored array of data
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Public Function ExportItemArray(ByVal oNode As System.Xml.XmlNode) As Object


        Dim vArray As Object = Nothing
        Dim sArrayDims As String = String.Empty
        Dim vArrayDims As Object = Nothing
        Dim lRows As Integer
        Dim lCols As Integer
        Dim oRow As System.Xml.XmlNode
        Dim oCol As System.Xml.XmlNode

        Dim lColCount As Integer
        Dim lRowCount As Integer
        Dim vColData As Object = Nothing

        Dim oColAttribute As System.Xml.XmlAttribute

        Dim oArrayAttribute As System.Xml.XmlAttribute ' IXMLDOMAttribute

        Dim oArrayBaseAttribute As System.Xml.XmlAttribute ' IXMLDOMAttribute
        Dim lMultiDimension As Integer

        Dim oAttributeVarContents As System.Xml.XmlAttribute ' IXMLDOMAttribute

        Dim oAttributeType As System.Xml.XmlAttribute ' IXMLDOMAttribute

        Dim oAttributeValue As System.Xml.XmlAttribute ' IXMLDOMAttribute
        Dim sArrayBase As String = String.Empty
        Dim vArrayBaseDims As Object = Nothing
        Dim lRowBase As Integer
        Dim lColBase As Integer
        Dim lRowBaseModifier As Integer
        Dim lColBaseModifier As Integer
        Dim lColStartCount As Integer
        Dim lRowStartCount As Integer

        Dim oSubArray As System.Xml.XmlNode

        'get attribute for array dimensions
        Try

            oArrayAttribute = oNode.SelectSingleNode("@arraydimensions")

            'check to see if the value we are working with is actually an array
            If Not oArrayAttribute Is Nothing Then
                'if yes, get the array dimensions
                sArrayDims = oArrayAttribute.Value
            Else
                'if no, treat it as a normal non-array attribute
                ExportItemArray = ExportItem(oNode)
                Exit Function
            End If

            'get attribute for array dimension base (i.e. the start point for each array element)
            'UPGRADE_WARNING: Untranslated statement in ExportItemArray. Please check source code.
            oArrayBaseAttribute = oNode.SelectSingleNode("@arraybase")

            'check attribute that tells us the base of the array dimensions
            If Not oArrayBaseAttribute Is Nothing Then
                sArrayBase = oArrayBaseAttribute.Value
            End If

            ' See if this is a single or multi dimension array
            lMultiDimension = Informations.inStr(sArrayDims, ",")

            If lMultiDimension > 0 Then
                vArrayDims = Split(sArrayDims, ",")

                'check indicated number of array dimensions
                If Microsoft.VisualBasic.UBound(vArrayDims) > 1 Then
                    Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLTooManyDimensions.ToString() + ", ExportItemArray, Error: Stored array has more than 2 dimensions")
                    Exit Function
                End If

                'check to see if the "arraybase" attribute was present or not - we need it, so complain if it isn't
                If sArrayBase.Trim = "" Then
                    Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLTooManyDimensions.ToString() + ", ExportItemArray, Error: 'arraybase' attribute is missing, cannot determine base for array dimensions")
                    Exit Function
                End If

                vArrayBaseDims = sArrayBase.Split(","c)

                'check indicated number of array dimensions - "arraybase" attribute should have one for each of the array dimensions
                If vArrayBaseDims.GetUpperBound(0) > 1 Then
                    Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLTooManyDimensions.ToString() + ", ExportItemArray, Error: 'arraybase' attribute indicates array has more than 2 dimensions")
                    Exit Function
                End If

                'check indicated number of array dimensions - "arraybase" attribute should have one for each of the array dimensions
                If vArrayBaseDims.GetUpperBound(0) <> vArrayDims.GetUpperBound(0) Then
                    Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLTooManyDimensions.ToString() + ", ExportItemArray, Error: 'arraybase' attribute does not have same number of dimensions as the 'arraydimensions' attribute")
                    Exit Function
                End If


                ' Get array dimensions

                lRows = CInt(vArrayDims(0))
                lCols = CInt(vArrayDims(1))
                lRowBase = CInt(vArrayBaseDims(0))
                lColBase = CInt(vArrayBaseDims(1))
                ReDim vArray(lRows, lCols)
            Else
                lRows = CInt(sArrayDims)
                lCols = 0
                lRowBase = CInt(sArrayBase)
                lColBase = 0

                ReDim vArray(lRows)
            End If

            lColStartCount = 1
            lRowStartCount = 1

            'check array "row" start point
            If lRowBase = 0 Then
                lRowBaseModifier = 1
                lRowStartCount = 0
            End If

            'check array "column" start point
            If lColBase = 0 Then
                lColBaseModifier = 1
                lColStartCount = 0
            End If

            ' Check if there are any ROWs
            If oNode.SelectNodes("ROW").Count = 0 Then
                Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLNotEnoughRows.ToString() + ", ExportItemArray, " + "Error: No row elements defined - expected " & lRows & " rows")
                Exit Function
            End If

            ' Check if we found correct number of rows
            If (oNode.SelectNodes("ROW").Count - lRowBaseModifier) <> lRows Then
                Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLNotEnoughRows.ToString() + ", ExportItemArray, " + "Error: Stored rows does not match stored array dimensions - expected " & lRows & " rows, but found " & CStr(oNode.SelectNodes("ROW").Count) & " instead.")
                Exit Function
            End If

            lRowCount = lRowStartCount

            ' Loop through all rows

            oRow = oNode.FirstChild

            While Not (oRow Is Nothing)
                ' Check if there are any COLs
                If oRow.ChildNodes.Count = 0 Then
                    Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLNotEnoughRows.ToString() + ", ExportItemArray, " + "Error: No COL elements defined for row - expected " & lCols & " columns")
                    Exit Function
                End If

                ' Check if we found correct number of columns
                If (oRow.ChildNodes.Count - lColBaseModifier) <> lCols Then
                    Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLNotEnoughRows.ToString() + ", ExportItemArray, " + "Error: Stored columns does not match stored array dimensions - expected " & lCols & " columns, but found " & CStr(oRow.ChildNodes.Count) & " COL nodes instead.")
                    Exit Function
                End If

                lColCount = lColStartCount

                ' Loop through all columns

                oCol = oRow.FirstChild

                While Not (oCol Is Nothing)

                    oAttributeVarContents = oCol.SelectSingleNode("@varcontents")
                    oAttributeType = oCol.SelectSingleNode("@datatype")
                    oAttributeValue = oCol.SelectSingleNode("@value")

                    'attribute for "value" should always be present...
                    If Not oAttributeValue Is Nothing Then
                        vColData = oAttributeValue.Value
                    End If

                    'RVH 27/8/2004: Rehydrate arrays stored inside other array elements
                    If Not (oAttributeType Is Nothing) Then
                        'developer guide no. 280
                        If oAttributeType.InnerText.EndsWith("()") Or oAttributeType.InnerText.EndsWith("[]") Or oAttributeType.InnerText.EndsWith("(,)") Or oAttributeType.InnerText.EndsWith("[,]") Then
                            oSubArray = oCol.SelectSingleNode("subArray")

                            'developer guide no. 98
                            vColData = ExportItemArray(oSubArray)
                        End If
                    End If


                    'check attribute which tells us about the variable contents
                    If Not oAttributeVarContents Is Nothing Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object oAttributeVarContents.Text. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                        Select Case oAttributeVarContents.InnerText.ToUpper()
                            Case "EMPTY"
                                vColData = Nothing
                            Case "NULL"
                                vColData = System.DBNull.Value
                        End Select
                    End If

                    ' Store data
                    If lMultiDimension > 0 Then
                        vArray(lRowCount, lColCount) = vColData
                    Else
                        vArray(lRowCount) = vColData
                    End If


                    oAttributeVarContents = Nothing

                    oAttributeType = Nothing

                    oAttributeValue = Nothing
                    lColCount = lColCount + 1


                    oCol = oCol.NextSibling
                End While
                lRowCount = lRowCount + 1

                oRow = oRow.NextSibling
            End While

            ' Return array
            ExportItemArray = vArray


        Catch excep As System.Exception
            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportItemArray, " + "Error: " & Information.Err().Number & " - " & excep.Message)
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         ExportMessageArray
    '
    ' Description:  Convert xml warning nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Public Function ExportMessageArray(ByVal oNodes As System.Xml.XmlNodeList, ByRef bHasMessages As Boolean) As PMXMLWarningMsg


        Dim oMessageList() As PMXMLWarningMsgStruct
        Dim oMessage As PMXMLWarningMsg = Nothing

        Dim oNode As XmlNode
        Dim lCount As Integer

        bHasMessages = False
        Try

            If Not oNodes Is Nothing Then

                If oNodes.Count > 0 Then
                    bHasMessages = True

                    ReDim oMessageList(oNodes.Count - 1)
                    lCount = 0

                    For Each oNode In oNodes
                        oMessageList(lCount).MethodName = oNode.SelectSingleNode("@method").Value
                        oMessageList(lCount).Number = oNode.SelectSingleNode("@number").Value
                        oMessageList(lCount).Title = oNode.SelectSingleNode("@title").Value
                        oMessageList(lCount).message = oNode.SelectSingleNode("@message").Value
                        lCount = lCount + 1
                    Next oNode

                    oMessage.List = oMessageList
                    oMessage.Count = lCount

                End If
            End If

            Return oMessage

        Catch excep As System.Exception


            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportMessageArray, " + "Error: " & Information.Err().Number & " - " & excep.Message)

            Return oMessage
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         ExportErrorArray
    '
    ' Description:  Convert xml error nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Public Function ExportErrorArray(ByVal oNodes As System.Xml.XmlNodeList, ByRef bHasErrors As Boolean, ByRef bHasInvalidDataErrors As Boolean, _
                                     ByRef bHasBusinessRuleErrors As Boolean, ByRef bHasBackOfficeErrors As Boolean, _
                                     ByRef bHasInternalExceptionErrors As Boolean) As PMXMLErrorTypes


        Dim oErrors As PMXMLErrorTypes = Nothing
        Dim oInvalidData() As PMXMLErrorInvalidDataStruct
        Dim oBusinessRule As PMXMLErrorBusinessRuleStruct
        Dim oBackOffice As PMXMLErrorBackOfficeStruct
        Dim oInternalException As PMXMLErrorInternalExceptionStruct

        Dim oErrorNodes As System.Xml.XmlNodeList

        Dim oErrorNode As System.Xml.XmlNode
        Dim lErrorCount As Integer

        Try
            bHasErrors = False

            If Not oNodes Is Nothing Then

                If oNodes.Item(0).ChildNodes.Count > 0 Then

                    oErrorNodes = oNodes.Item(0).SelectNodes("INVALIDDATA/INVALIDDATAERROR")
                    oInvalidData = ExportInvalidDataError(oErrorNodes, bHasInvalidDataErrors, lErrorCount)

                    oErrors.InvalidData.Details = oInvalidData
                    oErrors.InvalidData.Count = lErrorCount


                    oErrorNode = oNodes.Item(0).SelectSingleNode("BUSINESSRULE")

                    oBusinessRule = ExportBusinessRuleError(oErrorNode, bHasBusinessRuleErrors, lErrorCount)

                    oErrors.BusinessRule.Detail = oBusinessRule
                    oErrors.BusinessRule.Count = lErrorCount


                    oErrorNode = oNodes.Item(0).SelectSingleNode("BACKOFFICE")
                    oBackOffice = ExportBackOfficeError(oErrorNode, bHasBackOfficeErrors, lErrorCount)


                    oErrors.BackOffice.Detail = oBackOffice
                    oErrors.BackOffice.Count = lErrorCount


                    oErrorNode = oNodes.Item(0).SelectSingleNode("INTERNALEXCEPTION")

                    oInternalException = ExportInternalExceptionError(oErrorNode, bHasInternalExceptionErrors, lErrorCount)

                    oErrors.InternalException.Detail = oInternalException
                    oErrors.InternalException.Count = lErrorCount

                    If bHasInvalidDataErrors Or bHasBusinessRuleErrors Or bHasBackOfficeErrors Or bHasInternalExceptionErrors Then
                        bHasErrors = True
                    End If
                End If
            End If

            Return oErrors

        Catch ex As Exception
            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportErrorArray, " + "Error: " & Information.Err().Number & " - " & ex.Message)
        End Try

    End Function

    ' ***************************************************************** '
    ' Name:         ExportInvalidDataError
    '
    ' Description:  Convert xml invalid data nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Private Function ExportInvalidDataError(ByVal oNodes As XmlNodeList, ByRef bHasErrors As Boolean, ByRef lCount As Integer) As PMXMLErrorInvalidDataStruct()


        Dim oNode As XmlNode
        Dim oInvalidData() As PMXMLErrorInvalidDataStruct = Nothing

        lCount = 0
        Try

            If Not oNodes Is Nothing Then
                If oNodes.Count > 0 Then
                    bHasErrors = True
                    ReDim oInvalidData(oNodes.Count - 1)
                    For Each oNode In oNodes
                        oInvalidData(lCount).MethodName = oNode.SelectSingleNode("@method").Value
                        oInvalidData(lCount).Code = oNode.SelectSingleNode("@code").Value
                        oInvalidData(lCount).Description = oNode.SelectSingleNode("@description").Value
                        oInvalidData(lCount).FieldName = oNode.SelectSingleNode("@fieldname").Value
                        oInvalidData(lCount).SuppliedValue = oNode.SelectSingleNode("@suppliedvalue").Value
                        lCount = lCount + 1
                    Next oNode
                End If

            End If

            Return oInvalidData

        Catch excep As Exception
            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportInvalidDataError, " + "Error: " & Information.Err().Number & " - " & excep.Message)
            Return oInvalidData
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         ExportBusinessRuleError
    '
    ' Description:  Convert xml business rule error nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Private Function ExportBusinessRuleError(ByVal oNode As XmlNode, ByRef bHasErrors As Boolean, ByRef lCount As Integer) As PMXMLErrorBusinessRuleStruct

        Dim oBusinessRule As PMXMLErrorBusinessRuleStruct = Nothing
        Try
            lCount = 0

            If Not oNode Is Nothing Then
                If oNode.Attributes.Count > 0 Then
                    bHasErrors = True
                    oBusinessRule.MethodName = oNode.SelectSingleNode("@method").Value

                    oBusinessRule.Code = oNode.SelectSingleNode("@code").Value
                    oBusinessRule.Description = oNode.SelectSingleNode("@description").Value
                    oBusinessRule.Detail = oNode.SelectSingleNode("@detail").Value
                    lCount = 1
                End If

            End If

            Return oBusinessRule

        Catch excep As System.Exception

            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportBusinessRuleError, " + "Error: " & Information.Err().Number & " - " & excep.Message)

            Return oBusinessRule
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         ExportBackOfficeError
    '
    ' Description:  Convert xml back office error nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Private Function ExportBackOfficeError(ByVal oNode As XmlNode, ByRef bHasErrors As Boolean, ByRef lCount As Integer) As PMXMLErrorBackOfficeStruct

        Dim oBackOffice As PMXMLErrorBackOfficeStruct = Nothing

        lCount = 0
        Try

            If Not oNode Is Nothing Then
                If oNode.Attributes.Count > 0 Then
                    bHasErrors = True
                    oBackOffice.MethodName = oNode.SelectSingleNode("@method").Value
                    oBackOffice.Description = oNode.SelectSingleNode("@description").Value

                    oBackOffice.ReturnValue = oNode.SelectSingleNode("@returnvalue").Value
                    lCount = 1
                End If


            End If

            Return oBackOffice

        Catch excep As System.Exception
            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportBackOfficeError, " + "Error: " & Information.Err().Number & " - " & excep.Message)

            Return oBackOffice
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         ExportInternalExceptionError
    '
    ' Description:  Convert xml internal exception error nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Private Function ExportInternalExceptionError(ByVal oNode As XmlNode, ByRef bHasErrors As Boolean, ByRef lCount As Integer) As PMXMLErrorInternalExceptionStruct

        Dim oInternalException As PMXMLErrorInternalExceptionStruct = Nothing

        Try
            lCount = 0

            If Not oNode Is Nothing Then

                If oNode.Attributes.Count > 0 Then
                    bHasErrors = True
                    oInternalException.MethodName = oNode.SelectSingleNode("@method").Value
                    oInternalException.Description = oNode.SelectSingleNode("@description").Value
                    lCount = 1
                End If

            End If

            Return oInternalException

        Catch excep As System.Exception
            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportInternalExceptionError, " + "Error: " & Information.Err().Number & " - " & excep.Message)
            Return oInternalException
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         FormatDateCanonical
    '
    ' Description:  Format a date
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Public Function FormatDateCanonical(ByVal v_dtDate As Date) As String


        FormatDateCanonical = v_dtDate.ToString("yyyy-MM-dd HH:mm:ss")

        Exit Function


    End Function

    ' ***************************************************************** '
    ' Name:         FormatSafeXMLString
    '
    ' Description:  Format a string for XML
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Public Function FormatSafeXMLString(ByVal v_sString As String) As String


        ' mark any strings that have already been made safe so that the original formatting will not be
        ' lost when we unformatted it later
        v_sString = Replace(v_sString, "&lt;", "##LT##") ' RAW 06/09/2004 : added
        v_sString = Replace(v_sString, "&gt;", "##GT##") ' RAW 06/09/2004 : added
        v_sString = Replace(v_sString, "&amp;", "##AMP##") ' RAW 06/09/2004 : added
        v_sString = Replace(v_sString, "&apos;", "##APOS##") ' RAW 06/09/2004 : added
        v_sString = Replace(v_sString, "&quot;", "##QUOT##") ' RAW 06/09/2004 : added

        v_sString = Replace(v_sString, "<", "&lt;")
        v_sString = Replace(v_sString, ">", "&gt;")
        v_sString = Replace(v_sString, "&", "&amp;")
        v_sString = Replace(v_sString, "'", "&apos;")
        v_sString = Replace(v_sString, Chr(34), "&quot;")

        FormatSafeXMLString = v_sString

        Exit Function


    End Function

    ' ***************************************************************** '
    ' Name:         UnformatSafeXMLString
    '
    ' Description:  UnFormat a string previously made safe for XML
    '
    ' History:      27/08/2004  RVH Created
    ' ***************************************************************** '
    Public Function UnformatSafeXMLString(ByVal v_sString As String) As String


        v_sString = Replace(v_sString, "&lt;", "<")
        v_sString = Replace(v_sString, "&gt;", ">")
        v_sString = Replace(v_sString, "&amp;", "&")
        v_sString = Replace(v_sString, "&apos;", "'")
        v_sString = Replace(v_sString, "&quot;", Chr(34))

        ' now unformat any strings that are marked as being already been made safe
        v_sString = Replace(v_sString, "##LT##", "&lt;") ' RAW 06/09/2004 : added
        v_sString = Replace(v_sString, "##GT##", "&gt;") ' RAW 06/09/2004 : added
        v_sString = Replace(v_sString, "##AMP##", "&amp;") ' RAW 06/09/2004 : added
        v_sString = Replace(v_sString, "##APOS##", "&apos;") ' RAW 06/09/2004 : added
        v_sString = Replace(v_sString, "##QUOT##", "&quot;") ' RAW 06/09/2004 : added

        UnformatSafeXMLString = v_sString

        Exit Function


    End Function

    ' ***************************************************************** '
    ' Name:         ExportItem
    '
    ' Description:  Export a data item from the xml
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Public Function ExportItem(ByVal oNode As System.Xml.XmlNode) As Object


        Dim oAttributeVarContents As XmlAttribute ' IXMLDOMAttribute
        Dim oAttributeType As XmlAttribute 'IXMLDOMAttribute
        Dim oAttributeValue As XmlAttribute 'IXMLDOMAttribute
        Dim retValue As Object = Nothing
        Try

            If Not oNode Is Nothing Then

                oAttributeVarContents = oNode.SelectSingleNode("@varcontents")
                oAttributeType = oNode.SelectSingleNode("@datatype")
                oAttributeValue = oNode.SelectSingleNode("@value")

                'attribute for "value" should always be present...
                If Not (oAttributeValue Is Nothing) Then

                    'developer guide no 249.
                    'retValue = MSXMLHelper.GetNodeTypedValue(oAttributeValue)
                    retValue = oAttributeValue.Value
                    'RVH 27/08/2004: Need to reconstitute any XML tokens removed as part of the serialize process
                    If oAttributeType.InnerText.ToUpper() = "STRING" Then
                        retValue = UnformatSafeXMLString(retValue)
                    End If
                End If

                'check attribute which tells us about the variable contents
                If Not (oAttributeVarContents Is Nothing) Then
                    Select Case oAttributeVarContents.InnerText.ToUpper()
                        Case "EMPTY"
                            retValue = String.Empty
                        Case "NULL"

                            retValue = Nothing
                    End Select
                End If

                oAttributeVarContents = Nothing
                oAttributeType = Nothing
                oAttributeValue = Nothing
            End If
            Return retValue

        Catch excep As System.Exception
            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportItem, " + "Error: " & Information.Err().Number & " - " & excep.Message)
        End Try

    End Function

    ' ***************************************************************** '
    ' Name:         GetFormattedValue
    '
    ' Description:  Get formatted data and information about that data
    '
    ' History:      27/05/2004  RVH Created
    ' ***************************************************************** '
    Private Function GetFormattedValue(ByVal v_vValue As Object, ByRef r_sDataType As String, ByRef r_sVarContents As String) As Object

        Dim sTypeName As String = String.Empty
        Dim vValue As Object = Nothing

        'UPGRADE_WARNING: TypeName has a new behavior. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1041"'
        sTypeName = TypeName(v_vValue)
        r_sDataType = sTypeName
        r_sVarContents = ""

        If Right(sTypeName, 2) = "()" Then
            sTypeName = Left(sTypeName, Len(sTypeName) - 2)
        End If

        'check data type of passed data
        Select Case UCase(sTypeName)
            Case "DATE"
                'UPGRADE_WARNING: Couldn't resolve default property of object v_vValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                vValue = FormatDateCanonical(v_vValue)
            Case "STRING"
                'UPGRADE_WARNING: Couldn't resolve default property of object v_vValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                vValue = FormatSafeXMLString(v_vValue)
            Case Else
                'UPGRADE_WARNING: Couldn't resolve default property of object v_vValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                vValue = v_vValue
        End Select

        'check contents of passed data
        'UPGRADE_WARNING: IsEmpty was upgraded to IsNothing and has a new behavior. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1041"'
        If IsNothing(v_vValue) Then
            r_sVarContents = "empty"
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1049"'
        ElseIf IsDBNull(v_vValue) Then
            r_sVarContents = "null"
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
        'UPGRADE_WARNING: Couldn't resolve default property of object GetFormattedValue. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
        GetFormattedValue = vValue



    End Function

    ' ***************************************************************** '
    ' Name:         Append
    '
    ' Description:  Use array buffer to store data, rather than VB's
    '               slow string append...
    '
    ' History:      27/07/2004  RVH Created
    ' ***************************************************************** '
    Friend Sub Append(ByRef vDataStore As Object, ByRef LastPos As Integer, ByVal AppendData As String)


        Const cExtendArray As Short = 1000

        If IsArray(vDataStore) = False Then
            ReDim vDataStore(cExtendArray)
        Else
            If LastPos > Microsoft.VisualBasic.UBound(vDataStore) Then
                ReDim Preserve vDataStore(LastPos + cExtendArray)
            End If
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object vDataStore(). Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
        vDataStore(LastPos) = AppendData

        LastPos = LastPos + 1

        Exit Sub


    End Sub

End Class

