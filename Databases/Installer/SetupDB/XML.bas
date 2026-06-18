Attribute VB_Name = "MXML"
' Module:   Standard XML utility functions
' Shared:   Yes
' Needs:    MSXML 3.0 or MSXML 4.0
'
' Conditional compilation constants recognised in this module:
' * MSXML4 = 1  activates support for XML Schemas (requires msxml4.dll)
'
Option Explicit

' All new Sirius namespaces must start with this.
Public Const ksNamespaceCompanyStartWith = "http://www.siriusfs.com/Schemas/"

' Common XML standard namespaces.
Public Const ksNamespaceXMLSchema = "http://www.w3.org/2001/XMLSchema"
Public Const ksNamespaceXMLSchemaDataTypes = "http://www.w3.org/2001/XMLSchema-datatypes"
Public Const ksNamespaceXMLSchemaInstance = "http://www.w3.org/2001/XMLSchema-instance"
Public Const ksNamespaceXSLT = "http://www.w3.org/1999/XSL/Transform"

Public Const ksNameSpaceXMLSchemaDataTypesPrefix = "dt"
Public Const ksNameSpaceXMLSchemaInstancePrefix = "xsi"

' Error declarations.
Private Const ksErrSource = "MSXML"

' Create a new empty document for writing to.
Public Function XMLCreateFile(Optional ByVal sEncoding As String = "", _
    Optional ByVal sSelectionNamespaces As String = "") As DOMDocument

    Dim docFile As DOMDocument
    Dim sXMLDeclaration As String

    #If MSXML4 Then
        Set docFile = New DOMDocument40
        docFile.setProperty "NewParser", True
    #Else
        Set docFile = New DOMDocument
    #End If
    docFile.setProperty "SelectionLanguage", "XPath"
    If Len(sSelectionNamespaces) <> 0 Then
        docFile.setProperty "SelectionNamespaces", sSelectionNamespaces
    End If

    If Len(sEncoding) <> 0 Then
        sXMLDeclaration = "version=""1.0"" encoding=""" & sEncoding & """"
    Else
        sXMLDeclaration = "version=""1.0"""
    End If
    docFile.appendChild docFile.createProcessingInstruction("xml", sXMLDeclaration)

    Set XMLCreateFile = docFile

End Function

' Create a document and load an XML file into it.
Public Function XMLOpenFile(ByVal sFile As String, _
    Optional ByVal sSelectionNamespaces As String = "", _
    Optional ByVal schSchemaCache As XMLSchemaCache = Nothing) As DOMDocument

    Dim docFile As DOMDocument
    Dim errParse As IXMLDOMParseError

    #If MSXML4 Then
        Set docFile = New DOMDocument40
        docFile.setProperty "NewParser", True
    #Else
        Set docFile = New DOMDocument
    #End If
    docFile.setProperty "SelectionLanguage", "XPath"
    If sSelectionNamespaces <> "" Then
        docFile.setProperty "SelectionNamespaces", sSelectionNamespaces
    End If

    If Not schSchemaCache Is Nothing Then
        Set docFile.Schemas = schSchemaCache
    End If

    docFile.async = False
    If Not docFile.Load(sFile) Then
        Set errParse = docFile.parseError
        If errParse Is Nothing Then
            ' Throw last resort error.
            Err.Raise vbObjectError, ksErrSource, "Unknown error while loading/parsing XML file."
        ElseIf errParse.errorCode = -2146697210 Then
            ' Throw proper "file not found" instead of silly "system error".
            Err.Raise 53, ksErrSource, "File not found: " & sFile
        Else
            ' Throw the actual parsing error.
            Err.Raise errParse.errorCode, ksErrSource, XMLErrorDescription(errParse)
        End If
    End If

    Set XMLOpenFile = docFile

End Function

' Create a document and load an XML string into it.
Public Function XMLOpenString(ByVal sXML As String, _
    Optional ByVal sSelectionNamespaces As String = "", _
    Optional ByVal schSchemaCache As XMLSchemaCache = Nothing) As DOMDocument

    Dim docFile As DOMDocument
    Dim errParse As IXMLDOMParseError

    #If MSXML4 Then
        Set docFile = New DOMDocument40
        docFile.setProperty "NewParser", True
    #Else
        Set docFile = New DOMDocument
    #End If
    docFile.setProperty "SelectionLanguage", "XPath"
    If sSelectionNamespaces <> "" Then
        docFile.setProperty "SelectionNamespaces", sSelectionNamespaces
    End If

    If Not schSchemaCache Is Nothing Then
        Set docFile.Schemas = schSchemaCache
    End If

    docFile.async = False
    If Not docFile.loadXML(sXML) Then
        Set errParse = docFile.parseError
        If errParse Is Nothing Then
            ' Throw last resort error.
            Err.Raise vbObjectError, ksErrSource, "Unknown error while loading/parsing XML string."
        Else
            ' Throw the actual parsing error.
            Err.Raise errParse.errorCode, ksErrSource, XMLErrorDescription(errParse)
        End If
    End If

    Set XMLOpenString = docFile

End Function

' Add a new element to any parent element or the document object.
Public Function XMLAddElement(ByVal elmParent As IXMLDOMNode, _
    ByVal sTagName As String, _
    Optional ByVal sText As String = "", _
    Optional ByVal bCDATA As Boolean = False, _
    Optional ByVal sNamespaceURI As String = "") As IXMLDOMElement

    Dim docFile As DOMDocument
    Dim elmItem As IXMLDOMElement

    Set docFile = elmParent.ownerDocument
    If docFile Is Nothing Then
        Set docFile = elmParent
    End If

    Set elmItem = docFile.createNode(NODE_ELEMENT, sTagName, sNamespaceURI)
    If Len(sText) <> 0 Then
        If bCDATA Then
            elmItem.appendChild docFile.createCDATASection(sText)
        Else
            elmItem.Text = sText
        End If
    End If
    elmParent.appendChild elmItem

    Set XMLAddElement = elmItem

End Function

' Return user-readable text for the last XML parsing error.
Public Function XMLErrorText(ByVal docFile As DOMDocument) As String

    Dim errParse As IXMLDOMParseError

    XMLErrorText = ""
    If docFile Is Nothing Then Exit Function

    Set errParse = docFile.parseError
    If errParse Is Nothing Then Exit Function
    If errParse.errorCode = 0 Then Exit Function

    XMLErrorText = XMLErrorDescription(errParse) & " (" & errParse.errorCode & ") (" & ksErrSource & ")"

End Function

Private Function XMLErrorDescription(ByVal errParse As IXMLDOMParseError) As String

    XMLErrorDescription = RemoveChars(errParse.reason, vbCrLf)

    If errParse.Line <> 0 Or errParse.linepos <> 0 Or Len(errParse.srcText) <> 0 Then
        XMLErrorDescription = XMLErrorDescription & vbCrLf & _
            "Text at line " & errParse.Line & ", col " & errParse.linepos & ": " & Trim$(errParse.srcText)
    End If

End Function
