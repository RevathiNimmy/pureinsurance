Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Activex
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Imports System.Xml
'developer Guide no. 129
Imports SharedFiles
Module XMLSupport
	
	Public Structure PMXMLWarningMsgStruct
		Dim MethodName As String
		Dim Number As Integer
		Dim Title As String
		Dim message As String
		Public Shared Function CreateInstance() As PMXMLWarningMsgStruct
			Dim result As New PMXMLWarningMsgStruct
			result.MethodName = String.Empty
			result.Title = String.Empty
			result.message = String.Empty
			Return result
		End Function
	End Structure
	
	Public Structure PMXMLWarningMsg
		Dim List() As PMXMLWarningMsgStruct
		Dim Count As Integer
	End Structure
	
	Public Structure PMXMLErrorInvalidDataStruct
		Dim MethodName As String
		Dim FieldName As String
		Dim Code As gPMConstants.PMEReturnCode
		Dim Description As String
		Dim SuppliedValue As Object
		Public Shared Function CreateInstance() As PMXMLErrorInvalidDataStruct
			Dim result As New PMXMLErrorInvalidDataStruct
			result.MethodName = String.Empty
			result.FieldName = String.Empty
			result.Description = String.Empty
			Return result
		End Function
	End Structure
	
	Public Structure PMXMLErrorBusinessRuleStruct
		Dim MethodName As String
		Dim Code As gPMConstants.PMEReturnCode
		Dim Description As String
		Dim Detail As Object
		Public Shared Function CreateInstance() As PMXMLErrorBusinessRuleStruct
			Dim result As New PMXMLErrorBusinessRuleStruct
			result.MethodName = String.Empty
			result.Description = String.Empty
			Return result
		End Function
	End Structure
	
	Public Structure PMXMLErrorBackOfficeStruct
		Dim MethodName As String
		Dim ReturnValue As Integer
		Dim Description As String
		Public Shared Function CreateInstance() As PMXMLErrorBackOfficeStruct
			Dim result As New PMXMLErrorBackOfficeStruct
			result.MethodName = String.Empty
			result.Description = String.Empty
			Return result
		End Function
	End Structure
	
	Public Structure PMXMLErrorInternalExceptionStruct
		Dim MethodName As String
		Dim Description As String
		Public Shared Function CreateInstance() As PMXMLErrorInternalExceptionStruct
			Dim result As New PMXMLErrorInternalExceptionStruct
			result.MethodName = String.Empty
			result.Description = String.Empty
			Return result
		End Function
	End Structure
	
	Public Structure PMXMLErrorInvalidData
		Dim Details() As PMXMLErrorInvalidDataStruct
		Dim Count As Integer
	End Structure
	
	Public Structure PMXMLErrorBusinessRule
		Dim Detail As PMXMLErrorBusinessRuleStruct
		Dim Count As Integer
		Public Shared Function CreateInstance() As PMXMLErrorBusinessRule
			Dim result As New PMXMLErrorBusinessRule
			result.Detail = PMXMLErrorBusinessRuleStruct.CreateInstance()
			Return result
		End Function
	End Structure
	
	Public Structure PMXMLErrorBackOffice
		Dim Detail As PMXMLErrorBackOfficeStruct
		Dim Count As Integer
		Public Shared Function CreateInstance() As PMXMLErrorBackOffice
			Dim result As New PMXMLErrorBackOffice
			result.Detail = PMXMLErrorBackOfficeStruct.CreateInstance()
			Return result
		End Function
	End Structure
	
	Public Structure PMXMLErrorInternalException
		Dim Detail As PMXMLErrorInternalExceptionStruct
		Dim Count As Integer
		Public Shared Function CreateInstance() As PMXMLErrorInternalException
			Dim result As New PMXMLErrorInternalException
			result.Detail = PMXMLErrorInternalExceptionStruct.CreateInstance()
			Return result
		End Function
	End Structure
	
	Public Structure PMXMLErrorTypes
		Dim InvalidData As PMXMLErrorInvalidData
		Dim BusinessRule As PMXMLErrorBusinessRule
		Dim BackOffice As PMXMLErrorBackOffice
		Dim InternalException As PMXMLErrorInternalException
		Public Shared Function CreateInstance() As PMXMLErrorTypes
			Dim result As New PMXMLErrorTypes
			result.BusinessRule = PMXMLErrorBusinessRule.CreateInstance()
			result.BackOffice = PMXMLErrorBackOffice.CreateInstance()
			result.InternalException = PMXMLErrorInternalException.CreateInstance()
			Return result
		End Function
	End Structure
	
	'---------------------------------------------------------------------------------------------------------------
	
	Public Structure SAFEARRAYBOUND
		Dim cElements As Integer ' # of elements in the array dimension
		Dim lLbound As Integer ' lower bounds of the array dimension
	End Structure
	
	Public Structure SAFEARRAY
		Dim cDims As Integer ' Count of dimensions in this array.
		Dim fFeatures As Integer ' Flags used by the SAFEARRAY routines documented
		' below.
		Dim cbElements As Integer ' Size of an element of the array.
		Dim cLocks As Integer ' Number of times the array has been
		' locked without corresponding unlock.
		Dim pvData As Integer ' Pointer to the data.
		<VBFixedArray(59)> _
		Dim rgsabound() As SAFEARRAYBOUND ' One bound for each dimension.
		' An array can have max 60 dimensions, only the first cDims items will be
		' used
		' note that rgsabound elements are in reverse order,
		'  e.g. for a 2-dimensional
		' array, rgsabound(1) holds info about columns, and rgsabound(2) about rows
		Public Shared Function CreateInstance() As SAFEARRAY
			Dim result As New SAFEARRAY
			ReDim result.rgsabound(59)
			Return result
		End Function
	End Structure
	
	Private Declare Sub CopyMemory Lib "kernel32"  Alias "RtlMoveMemory"(ByVal dest As Integer, ByVal source As Integer, ByVal bytes As Integer)
	
	Private Const VT_BYREF As Integer = &H4000
	
	'---------------------------------------------------------------------------------------------------------------
	
	' Maximum number of array dimensions allowed i.e. can have dim fred(1) or dim fred(1,1) but not dim fred(1,1,1)
	Private Const cMaxNumberArrayDimensions As Integer = 2
	
	'******************************************************************************
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
	'******************************************************************************
    Private Function GetArrayInfo(ByRef TheArray As Object, ByRef ArrayInfo As SAFEARRAY) As Integer
        Dim result As Integer = 0
        

            'Dim lp As Integer ' work pointer variable
            'Dim VType As Integer ' the VARTYPE member of the VARIANT structure

            '' Exit if no array supplied
            'If Not Information.IsArray(TheArray) Then Return result

            'With ArrayInfo
            '             ' Get the VARTYPE value from the first 2 bytes of the VARIANT structure

            '             Dim handle As GCHandle = GCHandle.Alloc(VType, GCHandleType.Pinned)
            '             Dim handle2 As GCHandle = GCHandle.Alloc(TheArray, GCHandleType.Pinned)
            '	Try 
            '		Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()


            '		Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
            '		CopyMemory(tmpPtr, tmpPtr2, 2)
            '		VType = Marshal.ReadInt16(tmpPtr)
            '	Finally 
            '		handle.Free()
            '		handle2.Free()
            '	End Try

            '	' Get the pointer to the array descriptor (SAFEARRAY structure)
            '	' NOTE: A Variant's descriptor, padding & union take up 8 bytes.
            '	Dim handle3 As GCHandle = GCHandle.Alloc(lp, GCHandleType.Pinned)
            '                            
            '            			 Dim handle4 As GCHandle = GCHandle.Alloc(CInt(TheArray) + 8, GCHandleType.Pinned)
            '	'Dim handle4 As GCHandle = GCHandle.Alloc(VarPtr(TheArray) + 8, GCHandleType.Pinned)
            '	Try 
            '		Dim tmpPtr4 As IntPtr = handle4.AddrOfPinnedObject()

            '		Dim tmpPtr3 As IntPtr = handle3.AddrOfPinnedObject()
            '		CopyMemory(tmpPtr3, tmpPtr4, 4)
            '		lp = Marshal.ReadInt32(tmpPtr3)
            '	Finally 
            '		handle3.Free()
            '		handle4.Free()
            '	End Try

            '	'RVH 27/8/2004: Need to cater for arrays that have not been dimensioned yet - previous call returns
            '	'               zero for lp if the array has not been dimensions
            '	If lp = 0 Then
            '		Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLTooManyDimensions.ToString() + ", GetArrayInfo, Passed array has not been dimensioned")
            '	End If

            '	' Test if lp is a pointer or a pointer to a pointer.
            '	If (VType And VT_BYREF) <> 0 Then
            '		' Get real pointer to the array descriptor (SAFEARRAY structure)
            '		Dim handle5 As GCHandle = GCHandle.Alloc(lp, GCHandleType.Pinned)
            '		Dim handle6 As GCHandle = GCHandle.Alloc(lp, GCHandleType.Pinned)
            '		Try 
            '			Dim tmpPtr6 As IntPtr = handle6.AddrOfPinnedObject()
            '			Dim tmpPtr5 As IntPtr = handle5.AddrOfPinnedObject()
            '			CopyMemory(tmpPtr5, tmpPtr6, 4)
            '			lp = Marshal.ReadInt32(tmpPtr5)
            '		Finally 
            '			handle5.Free()
            '			handle6.Free()
            '		End Try
            '	End If

            '	' Fill the SAFEARRAY structure with the array info
            '	' NOTE: The fixed part of the SAFEARRAY structure is 16 bytes.
            '	Dim handle7 As GCHandle = GCHandle.Alloc(ArrayInfo.cDims, GCHandleType.Pinned)
            '	Dim handle8 As GCHandle = GCHandle.Alloc(lp, GCHandleType.Pinned)
            '	Try 
            '		Dim tmpPtr8 As IntPtr = handle8.AddrOfPinnedObject()
            '		Dim tmpPtr7 As IntPtr = handle7.AddrOfPinnedObject()
            '		CopyMemory(tmpPtr7, tmpPtr8, 16)
            '		ArrayInfo.cDims = Marshal.ReadInt16(tmpPtr7)
            '	Finally 
            '		handle7.Free()
            '		handle8.Free()
            '	End Try

            '	' Ensure the array has been dimensioned before getting SAFEARRAYBOUND
            '	' Information
            '	If ArrayInfo.cDims > 0 Then
            '		' Fill the SAFEARRAYBOUND structures with the array info
            '		Dim handle9 As GCHandle = GCHandle.Alloc(.rgsabound(0), GCHandleType.Pinned)
            '		Dim handle10 As GCHandle = GCHandle.Alloc(lp + 16, GCHandleType.Pinned)
            '		Try 

            '			Dim tmpPtr10 As IntPtr = handle10.AddrOfPinnedObject()
            '			Dim tmpPtr9 As IntPtr = handle9.AddrOfPinnedObject()

            '			CopyMemory(tmpPtr9, tmpPtr10, ArrayInfo.cDims * Marshal.SizeOf(.rgsabound(0)))
            '		Finally 
            '			handle9.Free()
            '			handle10.Free()
            '		End Try

            '		' So caller knows there is information available for the array in
            '		' output SAFEARRAY
            '		result = ArrayInfo.cDims
            '	End If
            '         End With


            '' Exit if no array supplied
            If Not Information.IsArray(TheArray) Then Return result

            ArrayInfo.cDims = TheArray.Rank
            If ArrayInfo.cDims > 1 Then
                ArrayInfo.rgsabound(0).cElements = TheArray.GetUpperBound(1) + 1
                ArrayInfo.rgsabound(0).lLbound = TheArray.GetLowerBound(1)
            Else
                ArrayInfo.rgsabound(0).cElements = TheArray.GetUpperBound(0) + 1
                ArrayInfo.rgsabound(0).lLbound = TheArray.GetLowerBound(0)
            End If

            ArrayInfo.rgsabound(1).cElements = TheArray.GetUpperBound(0) + 1
            ArrayInfo.rgsabound(1).lLbound = TheArray.GetLowerBound(0)
            result = ArrayInfo.cDims

            Return result

    End Function

    '******************************************************************************
    ' Name:         StoreItemArray
    '
    ' Description:  Set a value in the property bag
    '
    ' History:      27/05/2004  RVH Created
    '               27/07/2004  RVH Modified to use new Append method
    '                               in an attempt to speed up this sub
    '******************************************************************************
    Public Sub StoreItemArray(ByRef vDataStore() As Object, ByRef lLastPos As Integer, ByVal v_sItemName As String, ByVal v_vItemData As Object)
        Try

            Dim sDataType As String = ""

            Dim uArrayInfo As SAFEARRAY = SAFEARRAY.CreateInstance()
            Dim iArrayDims As Integer
            Dim uArrayDimInfo As New SAFEARRAYBOUND
            Dim sDims As String = ""
            Dim iDims As Integer
            Dim lColumns, lRows, lRowLowBound, lColumnLowBound As Integer
            Dim vArrayItem As Object
            Dim sTypeName, sVarContents As String
            Dim vValue As Object
            Dim sArrayBase As String = ""

            If Not Information.IsArray(v_vItemData) Then


                vValue = GetFormattedValue(v_vItemData, sTypeName, sVarContents)

                'RVH 27/07/2004: Modified to use new Append method

                Append(vDataStore, lLastPos, "<" & v_sItemName & " value='" & CStr(vValue) & "' datatype='" & sTypeName.ToLower() & "'" & (IIf(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>")))
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
                Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLTooManyDimensions.ToString() + ", StoreItemArray, Error: Passed array has more than 2 dimensions")
                Exit Sub
            End If

            ' Get array row information
            If iDims > 1 Then
                uArrayDimInfo = uArrayInfo.rgsabound(1)
            Else
                uArrayDimInfo = uArrayInfo.rgsabound(0)
            End If

            lRows = uArrayDimInfo.cElements
            lRowLowBound = uArrayDimInfo.lLbound

            If lRowLowBound = 0 Then
                lRows -= 1
            End If

            ' Get array column information
            If iDims > 1 Then
                uArrayDimInfo = uArrayInfo.rgsabound(0)
                lColumns = uArrayDimInfo.cElements
                lColumnLowBound = uArrayDimInfo.lLbound

                If lColumnLowBound = 0 Then
                    lColumns -= 1
                End If

                sDims = CStr(lRows) & "," & CStr(lColumns)
                sArrayBase = CStr(lRowLowBound) & "," & CStr(lColumnLowBound)
            Else
                sDims = CStr(lRows)
                sArrayBase = CStr(lRowLowBound)
            End If


            'Developer Guide No. 253
            sDataType = TypeName(v_vItemData).ToString

            'RVH 27/07/2004: Modified to use new Append method
            Append(vDataStore, lLastPos, "<" & v_sItemName & " type='" & sDataType & "' arraydimensions='" & sDims & "' arraybase='" & sArrayBase & "'>")

            '   Store row and column data
            For lRow As Integer = lRowLowBound To lRows
                'RVH 27/07/2004: Modified to use new Append method
                Append(vDataStore, lLastPos, "<ROW>")

                If iDims > 1 Then
                    For lColumn As Integer = lColumnLowBound To lColumns


                        vArrayItem = GetFormattedValue(v_vItemData(lRow, lColumn), sTypeName, sVarContents)

                        'RVH 27/07/2004: Modified to use new Append method
                        'RVH 27/08/2004: Modified to allow store of arrays embedded in array elements
                        'Developer Guide No. 280 
                        If sTypeName.EndsWith("()") Or sTypeName.EndsWith("[]") Or sTypeName.EndsWith("(,)") Or sTypeName.EndsWith("[,]") Then
                            Append(vDataStore, lLastPos, "<COL value='' datatype='" & sTypeName & "'>")
                            StoreItemArray(vDataStore, lLastPos, "subArray", v_vItemData(lRow, lColumn))
                            Append(vDataStore, lLastPos, "</COL>")
                        Else

                            Append(vDataStore, lLastPos, "<COL value='" & CStr(vArrayItem) & "' datatype='" & sTypeName & "'" & (IIf(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>")))
                        End If
                        '                Append vDataStore, lLastPos, "<COL value='" & vArrayItem & "' datatype='" & sTypeName & "'" & IIf(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>")
                    Next lColumn
                Else


                    vArrayItem = GetFormattedValue(v_vItemData(lRow), sTypeName, sVarContents)
                    'RVH 27/07/2004: Modified to use new Append method
                    'RVH 27/08/2004: Modified to allow store of arrays embedded in array elements
                    'Developer Guide No. 280
                    If sTypeName.EndsWith("()") Or sTypeName.EndsWith("[]") Or sTypeName.EndsWith("(,)") Or sTypeName.EndsWith("[,]") Then
                        Append(vDataStore, lLastPos, "<COL value='' datatype='" & sTypeName & "'>")
                        StoreItemArray(vDataStore, lLastPos, "subArray", v_vItemData(lRow))
                        Append(vDataStore, lLastPos, "</COL>")
                    Else

                        Append(vDataStore, lLastPos, "<COL value='" & CStr(vArrayItem) & "' datatype='" & sTypeName & "'" & (IIf(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>")))
                    End If
                End If

                'RVH 27/07/2004: Modified to use new Append method
                Append(vDataStore, lLastPos, "</ROW>")
            Next lRow

            'RVH 27/07/2004: Modified to use new Append method
            Append(vDataStore, lLastPos, "</" & v_sItemName & ">")

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", StoreItemArray, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try

    End Sub

    '******************************************************************************
    ' Name:         StoreItem
    '
    ' Description:  Set a value in the property bag
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Sub StoreItem(ByRef vDataStore() As Object, ByRef lLastPos As Integer, ByVal v_sItemName As String, ByVal v_vItemData As Object)
        Try

            Dim sTypeName As String = ""
            Dim vValue As Object
            Dim sVarContents As String = ""



            vValue = GetFormattedValue(v_vItemData, sTypeName, sVarContents)


            Append(vDataStore, lLastPos, "<" & v_sItemName & " value='" & CStr(vValue) & "' datatype='" & sTypeName.ToLower() & "'" & (IIf(sVarContents <> "", " varcontents='" & sVarContents & "'/>", "/>")))

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", StoreItem, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try

    End Sub

    '******************************************************************************
    ' Name:         ExportItemArray
    '
    ' Description:  Re-assemble and export stored array of data
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Function ExportItemArray(ByVal oNode As XmlNode) As Object
        Try

            Dim vArray As Object
            Dim sArrayDims As String = ""
            Dim vArrayDims As Object
            Dim lRows, lCols As Integer
            Dim oRow, oCol As XmlNode
            Dim lColCount, lRowCount As Integer
            Dim vColData As Object
            Dim oArrayAttribute, oArrayBaseAttribute As XmlAttribute
            Dim lMultiDimension As Integer
            Dim oAttributeVarContents, oAttributeType, oAttributeValue As XmlAttribute
            Dim sArrayBase As String = ""
            Dim vArrayBaseDims As Object
            Dim lRowBase, lColBase, lRowBaseModifier, lColBaseModifier, lColStartCount, lRowStartCount As Integer
            Dim oSubArray As XmlNode

            'get attribute for array dimensions
            oArrayAttribute = oNode.SelectSingleNode("@arraydimensions")

            'check to see if the value we are working with is actually an array
            If Not (oArrayAttribute Is Nothing) Then
                'if yes, get the array dimensions

                'Developer Guide No 249. 
                sArrayDims = oArrayAttribute.Value
            Else
                'if no, treat it as a normal non-array attribute

                Return ExportItem(oNode)
            End If

            'get attribute for array dimension base (i.e. the start point for each array element)
            oArrayBaseAttribute = oNode.SelectSingleNode("@arraybase")

            'check attribute that tells us the base of the array dimensions
            If Not (oArrayBaseAttribute Is Nothing) Then

                'Developer Guide No 249. 
                sArrayBase = oArrayBaseAttribute.Value
            End If

            ' See if this is a single or multi dimension array
            lMultiDimension = (sArrayDims.IndexOf(","c) + 1)

            If lMultiDimension > 0 Then

                vArrayDims = sArrayDims.Split(","c)

                'check indicated number of array dimensions

                If vArrayDims.GetUpperBound(0) > 1 Then
                    Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLTooManyDimensions.ToString() + ", ExportItemArray, Error: Stored array has more than 2 dimensions")
                    Exit Function
                End If

                'check to see if the "arraybase" attribute was present or not - we need it, so complain if it isn't
                If sArrayBase.Trim() = "" Then
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

                vArray = Array.CreateInstance(GetType(Object), New Integer() {lRows - lRowBase + 1, lCols - lColBase + 1}, New Integer() {lRowBase, lColBase})
            Else
                lRows = CInt(sArrayDims)
                lCols = 0
                lRowBase = CInt(sArrayBase)
                lColBase = 0

                vArray = Array.CreateInstance(GetType(Object), New Integer() {lRows - lRowBase + 1}, New Integer() {lRowBase})
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
                    If Not (oAttributeValue Is Nothing) Then


                        'Developer Guide No 249.
                        If oAttributeType.Value.ToLower() = "PMEReturnCode".ToLower() Then
                            If oAttributeValue.Value.ToLower() = "PMTrue".ToLower() Then
                                vColData = 1
                            ElseIf oAttributeValue.Value.ToLower() = "PMFalse".ToLower() Then
                                vColData = 0
                            Else
                                vColData = oAttributeValue.Value
                            End If
                        Else
                            vColData = oAttributeValue.Value
                        End If
                    End If

                    'RVH 27/8/2004: Rehydrate arrays stored inside other array elements
                    If Not (oAttributeType Is Nothing) Then
                        'Developer Guide No. 280
                        If oAttributeType.InnerText.EndsWith("()") Or oAttributeType.InnerText.EndsWith("[]") Or oAttributeType.InnerText.EndsWith("(,)") Or oAttributeType.InnerText.EndsWith("[,]") Then
                            oSubArray = oCol.SelectSingleNode("subArray")

                            'Developer Guide No. 98
                            vColData = ExportItemArray(oSubArray)
                        End If
                    End If

                    'check attribute which tells us about the variable contents
                    If Not (oAttributeVarContents Is Nothing) Then
                        Select Case oAttributeVarContents.InnerText.ToUpper()
                            Case "EMPTY"

                                vColData = String.Empty
                            Case "NULL"


                                vColData = Nothing
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
                    lColCount += 1

                    oCol = oCol.NextSibling
                End While
                lRowCount += 1
                oRow = oRow.NextSibling
            End While

            ' Return array

            Return vArray

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportItemArray, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    '******************************************************************************
    ' Name:         StoreMessageArray
    '
    ' Description:  Convert messages to XML
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Sub StoreMessageArray(ByRef vDataStore() As Object, ByRef lLastPos As Integer, ByRef oMessages As PMXMLWarningMsg)

        Dim lUBound As Integer
        Dim oMessageList() As PMXMLWarningMsgStruct = Nothing

        oMessageList = VB6.CopyArray(oMessages.List)

        Try
            lUBound = oMessageList.GetUpperBound(0)

        Catch
            lUBound = -1
        End Try


        If lUBound > 0 Then
            Append(vDataStore, lLastPos, "<WARNINGS>")
            For lCount As Integer = 0 To lUBound
                Append(vDataStore, lLastPos, "<WARNING method='" & FormatSafeXMLString(oMessageList(lCount).MethodName) & "' number='" & CStr(oMessageList(lCount).Number) & "' title='" & FormatSafeXMLString(oMessageList(lCount).Title) & "' message='" & FormatSafeXMLString(oMessageList(lCount).message) & "'/>")
            Next lCount
            Append(vDataStore, lLastPos, "</WARNINGS>")
        Else
            Append(vDataStore, lLastPos, "<WARNINGS/>")
        End If

        Exit Sub

Err_StoreMessageArray:

        Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", StoreMessageArray, " + "Error: " & Information.Err().Number & " - " & Information.Err().Description)

    End Sub

    '******************************************************************************
    ' Name:         StoreErrorArray
    '
    ' Description:  Convert errors to XML
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Sub StoreErrorArray(ByRef vDataStore() As Object, ByRef lLastPos As Integer, ByRef oErrors As PMXMLErrorTypes)
        Try

            If Not CheckErrors(oErrors) Then
                Append(vDataStore, lLastPos, "<ERRORS/>")
                Exit Sub
            End If

            Append(vDataStore, lLastPos, "<ERRORS>")
            StoreInvalidDataErrors(vDataStore, lLastPos, oErrors)
            StoreBusinessRuleErrors(vDataStore, lLastPos, oErrors)
            StoreBackOfficeErrors(vDataStore, lLastPos, oErrors)
            StoreInternalExceptionErrors(vDataStore, lLastPos, oErrors)
            Append(vDataStore, lLastPos, "</ERRORS>")

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", StoreErrorArray, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try

    End Sub

    '******************************************************************************
    ' Name:         StoreInvalidDataErrors
    '
    ' Description:  Store invalid data errors of any type
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Private Sub StoreInvalidDataErrors(ByRef vDataStore() As Object, ByRef lLastPos As Integer, ByRef oErrors As PMXMLErrorTypes)

        Dim oInvalidData() As PMXMLErrorInvalidDataStruct = Nothing
        Dim lUBound As Integer

        oInvalidData = VB6.CopyArray(oErrors.InvalidData.Details)


        
            If IsNothing(oInvalidData) Then

                Append(vDataStore, lLastPos, "<INVALIDDATA/>")
                Exit Sub
            End If
            lUBound = oInvalidData.GetUpperBound(0)
            Append(vDataStore, lLastPos, "<INVALIDDATA>")

            For lCount As Integer = 0 To lUBound

                Append(vDataStore, lLastPos, "<INVALIDDATAERROR method='" & FormatSafeXMLString(oInvalidData(lCount).MethodName) & "' fieldname='" & FormatSafeXMLString(oInvalidData(lCount).FieldName) & "' code='" & CStr(oInvalidData(lCount).Code) & "' description='" & FormatSafeXMLString(oInvalidData(lCount).Description) & "' suppliedvalue='" & FormatSafeXMLString(CStr(oInvalidData(lCount).SuppliedValue)) & "'/>")
            Next lCount

            Append(vDataStore, lLastPos, "</INVALIDDATA>")

            Exit Sub

Err_StoreInvalidDataErrors:

            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", StoreInvalidDataErrors, " + "Error: " & Information.Err().Number & " - " & Information.Err().Description)


    End Sub

    '******************************************************************************
    ' Name:         StoreBusinessRuleErrors
    '
    ' Description:  Store business rule errors of any type
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Private Sub StoreBusinessRuleErrors(ByRef vDataStore() As Object, ByRef lLastPos As Integer, ByRef oErrors As PMXMLErrorTypes)
        

            Dim oBusinessRule As PMXMLErrorBusinessRuleStruct = PMXMLErrorBusinessRuleStruct.CreateInstance()
            oBusinessRule = oErrors.BusinessRule.Detail

            If oErrors.BusinessRule.Count = 0 Then
                Append(vDataStore, lLastPos, "<BUSINESSRULE/>")
            Else

                Append(vDataStore, lLastPos, "<BUSINESSRULE method='" & FormatSafeXMLString(oBusinessRule.MethodName) & "' code='" & CStr(oBusinessRule.Code) & "' description='" & FormatSafeXMLString(oBusinessRule.Description) & "' detail='" & FormatSafeXMLString(CStr(oBusinessRule.Detail)) & "'/>")
            End If


    End Sub

    '******************************************************************************
    ' Name:         StoreBackOfficeErrors
    '
    ' Description:  Store back office errors of any type
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Private Sub StoreBackOfficeErrors(ByRef vDataStore() As Object, ByRef lLastPos As Integer, ByRef oErrors As PMXMLErrorTypes)
        

            Dim oBackOffice As PMXMLErrorBackOfficeStruct = PMXMLErrorBackOfficeStruct.CreateInstance()
            oBackOffice = oErrors.BackOffice.Detail

            If oErrors.BackOffice.Count = 0 Then
                Append(vDataStore, lLastPos, "<BACKOFFICE/>")
            Else
                Append(vDataStore, lLastPos, "<BACKOFFICE method='" & FormatSafeXMLString(oBackOffice.MethodName) & "' returnvalue='" & CStr(oBackOffice.ReturnValue) & "' description='" & FormatSafeXMLString(oBackOffice.Description) & "'/>")
            End If


    End Sub

    '******************************************************************************
    ' Name:         StoreInternalExceptionErrors
    '
    ' Description:  Store internal exception errors of any type
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Private Sub StoreInternalExceptionErrors(ByRef vDataStore() As Object, ByRef lLastPos As Integer, ByRef oErrors As PMXMLErrorTypes)
        

            Dim oInternalException As PMXMLErrorInternalExceptionStruct = PMXMLErrorInternalExceptionStruct.CreateInstance()

            oInternalException = oErrors.InternalException.Detail

            If oErrors.InternalException.Count = 0 Then
                Append(vDataStore, lLastPos, "<INTERNALEXCEPTION/>")
            Else
                Append(vDataStore, lLastPos, "<INTERNALEXCEPTION method='" & FormatSafeXMLString(oInternalException.MethodName) & "' description='" & FormatSafeXMLString(oInternalException.Description) & "'/>")
            End If


    End Sub

    '******************************************************************************
    ' Name:         CheckErrors
    '
    ' Description:  Check if there are any errors of any type
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Private Function CheckErrors(ByRef oErrors As PMXMLErrorTypes) As Boolean
        Dim result As Boolean = False
        

            If oErrors.InvalidData.Count > 0 Then
                Return True
            End If

            If oErrors.BusinessRule.Count > 0 Then
                Return True
            End If

            If oErrors.BackOffice.Count > 0 Then
                Return True
            End If

            If oErrors.InternalException.Count > 0 Then
                Return True
            End If

            Return result

    End Function

    '******************************************************************************
    ' Name:         ExportMessageArray
    '
    ' Description:  Convert xml warning nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Function ExportMessageArray(ByVal oNodes As XmlNodeList, ByRef bHasMessages As Boolean) As PMXMLWarningMsg
        Dim result As New PMXMLWarningMsg
        Try

            Dim oMessageList() As PMXMLWarningMsgStruct = Nothing
            Dim oMessage As New PMXMLWarningMsg
            Dim lCount As Integer

            bHasMessages = False

            If Not (oNodes Is Nothing) Then
                If oNodes.Count > 0 Then
                    bHasMessages = True
                    ReDim oMessageList(oNodes.Count - 1)
                    lCount = 0

                    For Each oNode As XmlNode In oNodes

                        'Developer Guide No 249.
                        oMessageList(lCount).MethodName = (oNode.SelectSingleNode("@method")).Value

                        'Developer Guide No 249.
                        oMessageList(lCount).Number = (oNode.SelectSingleNode("@number")).Value

                        'Developer Guide No 249.
                        oMessageList(lCount).Title = (oNode.SelectSingleNode("@title")).Value

                        'Developer Guide No 249.
                        oMessageList(lCount).message = (oNode.SelectSingleNode("@message")).Value
                        lCount += 1
                    Next oNode

                    oMessage.List = oMessageList
                    oMessage.Count = lCount

                    result = oMessage
                End If
            End If

            Return result

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportMessageArray, " + "Error: " & Information.Err().Number & " - " & excep.Message)

            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name:         ExportErrorArray
    '
    ' Description:  Convert xml error nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Function ExportErrorArray(ByVal oNodes As XmlNodeList, ByRef bHasErrors As Boolean, ByRef bHasInvalidDataErrors As Boolean, ByRef bHasBusinessRuleErrors As Boolean, ByRef bHasBackOfficeErrors As Boolean, ByRef bHasInternalExceptionErrors As Boolean) As PMXMLErrorTypes
        Dim result As PMXMLErrorTypes = PMXMLErrorTypes.CreateInstance()
        Try

            Dim oErrors As PMXMLErrorTypes = PMXMLErrorTypes.CreateInstance()
            Dim oInvalidData() As PMXMLErrorInvalidDataStruct = Nothing
            Dim oBusinessRule As PMXMLErrorBusinessRuleStruct = PMXMLErrorBusinessRuleStruct.CreateInstance()
            Dim oBackOffice As PMXMLErrorBackOfficeStruct = PMXMLErrorBackOfficeStruct.CreateInstance()
            Dim oInternalException As PMXMLErrorInternalExceptionStruct = PMXMLErrorInternalExceptionStruct.CreateInstance()
            Dim oErrorNodes As XmlNodeList
            Dim oErrorNode As XmlNode
            Dim lErrorCount As Integer

            bHasErrors = False

            If Not (oNodes Is Nothing) Then
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

                    result = oErrors

                    If bHasInvalidDataErrors Or bHasBusinessRuleErrors Or bHasBackOfficeErrors Or bHasInternalExceptionErrors Then
                        bHasErrors = True
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportErrorArray, " + "Error: " & Information.Err().Number & " - " & excep.Message)

            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name:         ExportInvalidDataError
    '
    ' Description:  Convert xml invalid data nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Private Function ExportInvalidDataError(ByVal oNodes As XmlNodeList, ByRef bHasErrors As Boolean, ByRef lCount As Integer) As PMXMLErrorInvalidDataStruct()
        Dim result() As PMXMLErrorInvalidDataStruct = Nothing
        

            Dim oInvalidData() As PMXMLErrorInvalidDataStruct = Nothing

            lCount = 0

            If Not (oNodes Is Nothing) Then
                If oNodes.Count > 0 Then
                    bHasErrors = True
                    ReDim oInvalidData(oNodes.Count - 1)
                    For Each oNode As XmlNode In oNodes

                        'Developer Guide No 249.
                        oInvalidData(lCount).MethodName = (oNode.SelectSingleNode("@method")).Value

                        'Developer Guide No 249.
                        oInvalidData(lCount).Code = (oNode.SelectSingleNode("@code")).Value

                        'Developer Guide No 249.
                        oInvalidData(lCount).Description = (oNode.SelectSingleNode("@description")).Value

                        'Developer Guide No 249.
                        oInvalidData(lCount).FieldName = (oNode.SelectSingleNode("@fieldname")).Value


                        'Developer Guide No 249.
                        oInvalidData(lCount).SuppliedValue = (oNode.SelectSingleNode("@suppliedvalue")).Value
                        lCount += 1
                    Next oNode
                End If
                result = oInvalidData
            End If

            Return result

    End Function

    '******************************************************************************
    ' Name:         ExportBusinessRuleError
    '
    ' Description:  Convert xml business rule error nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Private Function ExportBusinessRuleError(ByVal oNode As XmlNode, ByRef bHasErrors As Boolean, ByRef lCount As Integer) As PMXMLErrorBusinessRuleStruct
        Dim result As PMXMLErrorBusinessRuleStruct = PMXMLErrorBusinessRuleStruct.CreateInstance()
        

            Dim oBusinessRule As PMXMLErrorBusinessRuleStruct = PMXMLErrorBusinessRuleStruct.CreateInstance()

            lCount = 0

            If Not (oNode Is Nothing) Then
                If oNode.Attributes.Count > 0 Then
                    bHasErrors = True

                    'Developer Guide No 249.
                    oBusinessRule.MethodName = (oNode.SelectSingleNode("@method")).Value

                    'Developer Guide No 249.
                    oBusinessRule.Code = (oNode.SelectSingleNode("@code")).Value

                    'Developer Guide No 249.
                    oBusinessRule.Description = (oNode.SelectSingleNode("@description")).Value


                    'Developer Guide No 249.
                    oBusinessRule.Detail = (oNode.SelectSingleNode("@detail")).Value
                    lCount = 1
                End If
                result = oBusinessRule
            End If

            Return result

    End Function

    '******************************************************************************
    ' Name:         ExportBackOfficeError
    '
    ' Description:  Convert xml back office error nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Private Function ExportBackOfficeError(ByVal oNode As XmlNode, ByRef bHasErrors As Boolean, ByRef lCount As Integer) As PMXMLErrorBackOfficeStruct
        Dim result As PMXMLErrorBackOfficeStruct = PMXMLErrorBackOfficeStruct.CreateInstance()
        

            Dim oBackOffice As PMXMLErrorBackOfficeStruct = PMXMLErrorBackOfficeStruct.CreateInstance()

            lCount = 0

            If Not (oNode Is Nothing) Then
                If oNode.Attributes.Count > 0 Then
                    bHasErrors = True

                    'Developer Guide No 249.
                    oBackOffice.MethodName = (oNode.SelectSingleNode("@method")).Value

                    'Developer Guide No 249.
                    oBackOffice.Description = (oNode.SelectSingleNode("@description")).Value

                    'Developer Guide No 249.
                    oBackOffice.ReturnValue = (oNode.SelectSingleNode("@returnvalue")).Value
                    lCount = 1
                End If
                result = oBackOffice
            End If

            Return result

    End Function

    '******************************************************************************
    ' Name:         ExportInternalExceptionError
    '
    ' Description:  Convert xml internal exception error nodes and return an array
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Private Function ExportInternalExceptionError(ByVal oNode As XmlNode, ByRef bHasErrors As Boolean, ByRef lCount As Integer) As PMXMLErrorInternalExceptionStruct
        Dim result As PMXMLErrorInternalExceptionStruct = PMXMLErrorInternalExceptionStruct.CreateInstance()
        

            Dim oInternalException As PMXMLErrorInternalExceptionStruct = PMXMLErrorInternalExceptionStruct.CreateInstance()

            lCount = 0

            If Not (oNode Is Nothing) Then
                If oNode.Attributes.Count > 0 Then
                    bHasErrors = True

                    'Developer Guide No 249.
                    oInternalException.MethodName = (oNode.SelectSingleNode("@method")).Value

                    'Developer Guide No 249.
                    oInternalException.Description = (oNode.SelectSingleNode("@description")).Value
                    lCount = 1
                End If
                result = oInternalException
            End If

            Return result

    End Function

    '******************************************************************************
    ' Name:         FormatDateCanonical
    '
    ' Description:  Format a date
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Function FormatDateCanonical(ByVal v_dtDate As Date) As String
        Try


            Return v_dtDate.ToString("yyyy-MM-dd HH:mm:ss")

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", FormatDateCanonical, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    '******************************************************************************
    ' Name:         FormatSafeXMLString
    '
    ' Description:  Format a string for XML
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Function FormatSafeXMLString(ByVal v_sString As String) As String
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
            v_sString = v_sString.Replace(Strings.Chr(34).ToString(), "&quot;")


            Return v_sString

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", FormatSafeXMLString, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    '******************************************************************************
    ' Name:         UnformatSafeXMLString
    '
    ' Description:  UnFormat a string previously made safe for XML
    '
    ' History:      27/08/2004  RVH Created
    '******************************************************************************
    Public Function UnformatSafeXMLString(ByVal v_sString As String) As String
        Try

            v_sString = v_sString.Replace("&lt;", "<")
            v_sString = v_sString.Replace("&gt;", ">")
            v_sString = v_sString.Replace("&amp;", "&")
            v_sString = v_sString.Replace("&apos;", "'")
            v_sString = v_sString.Replace("&quot;", Strings.Chr(34).ToString())

            ' now unformat any strings that are marked as being already been made safe
            v_sString = v_sString.Replace("##LT##", "&lt;") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("##GT##", "&gt;") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("##AMP##", "&amp;") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("##APOS##", "&apos;") ' RAW 06/09/2004 : added
            v_sString = v_sString.Replace("##QUOT##", "&quot;") ' RAW 06/09/2004 : added


            Return v_sString

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", UnformatSafeXMLString, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    '******************************************************************************
    ' Name:         ExportItem
    '
    ' Description:  Export a data item from the xml
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Function ExportItem(ByVal oNode As XmlNode) As String
        Try

            Dim oAttributeVarContents, oAttributeType, oAttributeValue As XmlAttribute
            Dim retValue As String = ""

            If Not (oNode Is Nothing) Then
                oAttributeVarContents = oNode.SelectSingleNode("@varcontents")
                oAttributeType = oNode.SelectSingleNode("@datatype")
                oAttributeValue = oNode.SelectSingleNode("@value")

                'attribute for "value" should always be present...
                If Not (oAttributeValue Is Nothing) Then


                    'Developer Guide No 249.
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

    '******************************************************************************
    ' Name:         GetFormattedValue
    '
    ' Description:  Get formatted data and information about that data
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Private Function GetFormattedValue(ByVal v_vValue As Object, ByRef r_sDataType As String, ByRef r_sVarContents As String) As Object
        

            Dim sTypeName As String = ""
            Dim vValue As Object


            'Developer Guide No 253.
            sTypeName = TypeName(v_vValue).ToString()
            r_sDataType = sTypeName
            r_sVarContents = ""


            'Developer Guide No. 280
            If sTypeName.EndsWith("()") Then
                sTypeName = sTypeName.Substring(0, sTypeName.Length - 2)
            ElseIf sTypeName.EndsWith("[]") Then
                sTypeName = sTypeName.Substring(0, sTypeName.Length - 2)
            ElseIf sTypeName.EndsWith("(,)") Then
                sTypeName = sTypeName.Substring(0, sTypeName.Length - 3)
            ElseIf sTypeName.EndsWith("[,]") Then
                sTypeName = sTypeName.Substring(0, sTypeName.Length - 3)
            End If

            'check data type of passed data
            Select Case sTypeName.ToUpper()
                Case "DATE"


                    vValue = FormatDateCanonical(CDate(v_vValue))
                Case "STRING"


                    vValue = FormatSafeXMLString(CStr(v_vValue))
                Case Else


                    vValue = v_vValue
            End Select

            'check contents of passed data


            'Developer Guide No 269. 
            If Convert.IsDBNull(v_vValue) Or IsNothing(v_vValue) Then
                r_sVarContents = "null"
            End If


            Return vValue

    End Function

    '******************************************************************************
    ' Name:         AddInvalidDataError
    '
    ' Description:  Store an invalid data error message away
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Function AddInvalidDataError(ByVal MethodName As String, ByVal FieldName As String, ByVal Code As gPMConstants.PMEReturnCode, ByVal Description As String, ByVal SuppliedValue As Object, ByRef oInvalidData As PMXMLErrorInvalidData) As Object

        Dim oInvalidDataError() As PMXMLErrorInvalidDataStruct = Nothing
        Dim oMsg As PMXMLErrorInvalidDataStruct = PMXMLErrorInvalidDataStruct.CreateInstance()
        Dim lUBound As Integer

        oInvalidDataError = VB6.CopyArray(oInvalidData.Details)

        oMsg.MethodName = MethodName
        oMsg.FieldName = FieldName
        oMsg.Code = Code
        oMsg.Description = Description


        oMsg.SuppliedValue = SuppliedValue


        Dim resume2 As Boolean = True
        Try
            lUBound = oInvalidDataError.GetUpperBound(0)

            If Information.Err().Number <> 0 Then

                resume2 = False
                ReDim oInvalidDataError(0)
                oInvalidDataError(0) = oMsg
                oInvalidData.Count = 1
            Else

                resume2 = False
                ReDim Preserve oInvalidDataError(lUBound + 1)
                oInvalidDataError(lUBound + 1) = oMsg
                oInvalidData.Count = lUBound
            End If

            oInvalidData.Details = oInvalidDataError

            Exit Function

Err_AddInvalidDataError:

            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", AddInvalidDataError, " + "Error: " & Information.Err().Number & " - " & Information.Err().Description)

        Catch exc As System.Exception

            If Not resume2 Then
                Throw exc
            End If
        End Try
    End Function

    '******************************************************************************
    ' Name:         AddBusinessRuleError
    '
    ' Description:  Store an business rule error message away
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Function AddBusinessRuleError(ByVal MethodName As String, ByVal Code As gPMConstants.PMEReturnCode, ByVal Description As String, ByVal Detail As Object, ByRef oBusinessRule As PMXMLErrorBusinessRule) As Object
        Try

            Dim oMsg As PMXMLErrorBusinessRuleStruct = PMXMLErrorBusinessRuleStruct.CreateInstance()

            oMsg.MethodName = MethodName
            oMsg.Code = Code
            oMsg.Description = Description


            oMsg.Detail = Detail

            oBusinessRule.Detail = oMsg
            oBusinessRule.Count = 1

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", AddBusinessRuleError, " + "Error: " & Information.Err().Number & " - " & excep.Message)
        End Try

    End Function

    '******************************************************************************
    ' Name:         AddBackOfficeError
    '
    ' Description:  Store an back office error message away
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Function AddBackOfficeError(ByVal MethodName As String, ByVal ReturnValue As Integer, ByVal Description As String, ByRef oBackOffice As PMXMLErrorBackOffice) As Object
        Try

            Dim oMsg As PMXMLErrorBackOfficeStruct = PMXMLErrorBackOfficeStruct.CreateInstance()
            oMsg.MethodName = MethodName
            oMsg.ReturnValue = ReturnValue
            oMsg.Description = Description

            oBackOffice.Detail = oMsg
            oBackOffice.Count = 1

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", AddBackOfficeError, " + "Error: " & Information.Err().Number & " - " & excep.Message)
        End Try

    End Function

    '******************************************************************************
    ' Name:         AddInternalExceptionError
    '
    ' Description:  Store an internal exception error message away
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Function AddInternalExceptionError(ByVal MethodName As String, ByVal Description As String, ByRef oInternalException As PMXMLErrorInternalException) As Object
        Try

            Dim oMsg As PMXMLErrorInternalExceptionStruct = PMXMLErrorInternalExceptionStruct.CreateInstance()

            oMsg.MethodName = MethodName
            oMsg.Description = Description

            oInternalException.Detail = oMsg
            oInternalException.Count = 1

        Catch excep As System.Exception

            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", AddInternalExceptionError, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try

    End Function

    '******************************************************************************
    ' Name:         ComposeErrorString
    '
    ' Description:  Compose error string from error type
    '
    ' History:      27/05/2004  RVH Created
    '******************************************************************************
    Public Function ComposeErrorString(ByRef oErrors As PMXMLErrorTypes) As String
        Try

            Dim sError As String = ""

            sError = ""

            If oErrors.BackOffice.Count > 0 Then
                sError = sError & "[" & oErrors.BackOffice.Detail.MethodName & "] " & CStr(oErrors.BackOffice.Detail.ReturnValue) & " - " & oErrors.BackOffice.Detail.Description & Strings.Chr(10).ToString()
            End If

            If oErrors.BusinessRule.Count > 0 Then
                If sError.Length Then
                    sError = sError & Strings.Chr(10).ToString()
                End If

                sError = sError & "[" & oErrors.BusinessRule.Detail.MethodName & "] " & CStr(oErrors.BusinessRule.Detail.Code) & " - " & oErrors.BusinessRule.Detail.Description & " (" & CStr(oErrors.BusinessRule.Detail.Detail) & ")" & Strings.Chr(10).ToString()
            End If

            If oErrors.InternalException.Count > 0 Then
                If sError.Length Then
                    sError = sError & Strings.Chr(10).ToString()
                End If
                sError = sError & "[" & oErrors.InternalException.Detail.MethodName & "] " & oErrors.InternalException.Detail.Description & Strings.Chr(10).ToString()
            End If

            If oErrors.InvalidData.Count > 0 Then
                If sError.Length Then
                    sError = sError & Strings.Chr(10).ToString()
                End If
                For lError As Integer = 0 To oErrors.InvalidData.Count - 1

                    sError = sError & "[" & oErrors.InvalidData.Details(lError).MethodName & "] " & CStr(oErrors.InvalidData.Details(lError).Code) & " - " & oErrors.InvalidData.Details(lError).Description & " (" & oErrors.InvalidData.Details(lError).FieldName & ":" & CStr(oErrors.InvalidData.Details(lError).SuppliedValue) & ")" & Strings.Chr(10).ToString()
                Next lError
            End If

            If sError.Length Then
                sError = Mid(sError, 1, sError.Length - 1)
            End If


            Return sError

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ComposeErrorString, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    '******************************************************************************
    ' Name:         ComposeWarningString
    '
    ' Description:  Compose string of warnings
    '
    ' History:      27/07/2004  RVH Created
    '******************************************************************************
    Public Function ComposeWarningString(ByRef oWarnings As PMXMLWarningMsg) As String
        Try

            Dim sWarning As String = ""

            sWarning = ""

            If oWarnings.Count > 0 Then
                For lWarning As Integer = 0 To oWarnings.Count - 1
                    sWarning = sWarning & "[" & oWarnings.List(lWarning).MethodName & "] " & oWarnings.List(lWarning).Title & " - (" & CStr(oWarnings.List(lWarning).Number) & ") " & oWarnings.List(lWarning).message & Strings.Chr(10).ToString()
                Next lWarning
            End If

            If sWarning.Length Then
                sWarning = Mid(sWarning, 1, sWarning.Length - 1)
            End If


            Return sWarning

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", ComposeWarningString, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Sub AddWarning(ByVal MethodName As String, ByVal Number As Integer, ByVal Title As String, ByVal message As String, ByRef oType As PMXMLWarningMsg)

        
        Dim oMsg As PMXMLWarningMsgStruct = PMXMLWarningMsgStruct.CreateInstance()
        Dim lUBound As Integer

        oMsg.MethodName = MethodName
        oMsg.Number = Number
        oMsg.Title = Title
        oMsg.message = message


        Dim resume2 As Boolean = True
        Try

            lUBound = oType.List.GetUpperBound(0)

            If Information.Err().Number <> 0 Then

                resume2 = False
                ReDim oType.List(0)
                oType.List(0) = oMsg
            Else

                resume2 = False
                ReDim Preserve oType.List(lUBound + 1)
                oType.List(lUBound + 1) = oMsg
            End If

            Exit Sub

Err_AddWarning:

            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", AddWarning, " + "Error: " & Information.Err().Number & " - " & Information.Err().Description)

        Catch exc As System.Exception

            If Not resume2 Then
                Throw exc
            End If
        End Try

    End Sub
	
	Public Sub RaiseBackOfficeError(ByVal MethodName As String, ByVal ReturnValue As Integer, ByVal Description As String, ByRef oType As PMXMLErrorBackOffice)
		
		AddBackOfficeError(MethodName, ReturnValue, Description, oType)
		
		Throw New System.Exception(gPMConstants.PMEReturnCode.PMBackOfficeError.ToString() + ", RaiseBackOfficeError, Back Office Error")
		
	End Sub
	
	Public Sub RaiseBusinessRuleError(ByVal MethodName As String, ByVal Code As gPMConstants.PMEReturnCode, ByVal Description As String, ByVal Detail As Object, ByRef oType As PMXMLErrorBusinessRule)
		
		AddBusinessRuleError(MethodName, Code, Description, Detail, oType)
		
		Throw New System.Exception(gPMConstants.PMEReturnCode.PMBusinessRuleError.ToString() + ", RaiseBusinessRuleError, Business Rule Error")
		
	End Sub
	
	'******************************************************************************
	' Name:         Append
	'
	' Description:  Use array buffer to store data, rather than VB's
	'               slow string append...
	'
	' History:      27/07/2004  RVH Created
	'******************************************************************************
	Public Sub Append(ByRef vDataStore() As Object, ByRef LastPos As Integer, ByVal AppendData As String)
		Try 
			
			Const cExtendArray As Integer = 1000
			
			If Not Information.IsArray(vDataStore) Then
				ReDim vDataStore(cExtendArray)
			Else
				If LastPos > vDataStore.GetUpperBound(0) Then
					ReDim Preserve vDataStore(LastPos + cExtendArray)
				End If
			End If
			

			vDataStore(LastPos) = AppendData
			
			LastPos += 1
		
		Catch excep As System.Exception
			
			
			
			Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", Append, " + "Error: " & Information.Err().Number & " - " & excep.Message)
			
		End Try
		
	End Sub
End Module

