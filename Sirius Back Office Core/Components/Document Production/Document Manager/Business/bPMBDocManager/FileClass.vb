Option Strict Off
Option Explicit On
Imports System.IO
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("FileClass_NET.FileClass")>
Public NotInheritable Class FileClass

    Private Const ACClass As String = "FileClass"
    Private Const cDocumentBodyStart As String = "<BODY"
    Private Const cDocumentPropertiesStart As String = "<O:DOCUMENTPROPERTIES>"
    Private Const cDocumentPropertiesEnd As String = "</O:DOCUMENTPROPERTIES>"
    Private Const cDocumentCustomPropertiesStart As String = "<O:CUSTOMDOCUMENTPROPERTIES>"
    Private Const cDocumentCustomPropertiesEnd As String = "</O:CUSTOMDOCUMENTPROPERTIES>"
    Private Const cDefaultStartTag As String = "&LT;@"

    Private m_sFileName As String = ""
    Private m_vFileArray() As Object
    Private m_lCurrentFileLine As Integer
    Private m_lTotalFileLines As Integer
    Private m_sUsername As String = ""
    Private m_sStartTag As String = cDefaultStartTag
    Private m_lDocPropsStartLineCount As Integer
    Private m_lDocPropsEndLineCount As Integer
    Private m_lDocCustPropsStartLineCount As Integer
    Private m_lDocCustPropsEndLineCount As Integer
    Private m_lDocBodyStartLineCount As Integer

    Public WriteOnly Property StartTag() As String
        Set(ByVal Value As String)
            m_sStartTag = Value
        End Set
    End Property

    Public ReadOnly Property FileName() As String
        Get
            Return m_sFileName
        End Get
    End Property

    Public ReadOnly Property EOF() As Boolean
        Get
            If m_lCurrentFileLine > m_lTotalFileLines Then
                EOF = True
            Else
                EOF = False
            End If
        End Get
    End Property
    Public Function OpenAndReadFileXML(ByVal sInputFileName As String,
                                   ByRef oTextStreamOut As Scripting.TextStream,
                                   ByVal sUsername As String,
                                   Optional ByVal v_iIsSubDoc As Integer = 0,
                                   Optional ByRef r_sPreBodyFragment As String = "") As Integer
        Dim sPreBodyFragment() As String
        Dim lReturn As Integer
        Dim sValue As String

        Dim sDoubleQuote As String
        Dim oDoc As Xml.XmlDocument
        Dim oBodyNodes As Xml.XmlNodeList
        Dim newXmlDeclaration As Xml.XmlDeclaration
        Dim result As Integer = 0

        sDoubleQuote = ChrW(34)

        oDoc = New Xml.XmlDocument

        result = gPMConstants.PMEReturnCode.PMTrue

        m_sFileName = sInputFileName
        m_sUsername = sUsername
        Try
            If v_iIsSubDoc = 0 Then
                lReturn = DocumentTitleCheckUsingSiriusDocumentUtility(m_sFileName)
            End If


            oDoc.PreserveWhitespace = False

            oDoc.Load(m_sFileName)

            If oDoc.FirstChild.NodeType = System.Xml.XmlNodeType.XmlDeclaration Then
                newXmlDeclaration = oDoc.CreateXmlDeclaration("1.0", String.Empty, "yes")
                oDoc.ReplaceChild(newXmlDeclaration, oDoc.FirstChild)
            End If

            oBodyNodes = oDoc.GetElementsByTagName("w:body")

            'm_vFileArray = ""
            If oBodyNodes IsNot Nothing AndAlso oBodyNodes.Count > 0 Then
                lReturn = BreakStringIntoArray("<w:p", "</w:p>", oBodyNodes(0).OuterXml, m_vFileArray, True)
            End If

            AddItemInFileArray("</w:wordDocument>")

            If oDoc.OuterXml <> "" Then
                sPreBodyFragment = oDoc.OuterXml.Split(New String() {"<w:body>"}, StringSplitOptions.None)

                sValue = sPreBodyFragment(0)

                lReturn = RemoveInvalidCharacters(sValue)

                lReturn = CheckNameSpaces(sValue)

                oTextStreamOut.Write(sValue)

                r_sPreBodyFragment = sValue & "<w:body><wx:sect>"

                If g_sDocPreBodyFragment = "" Then
                    g_sDocPreBodyFragment = sValue & "<w:body><wx:sect>"
                    g_sDocEndBodyFragment = "</wx:sect></w:body></w:wordDocument>"
                End If
            End If

            m_lTotalFileLines = m_vFileArray.GetUpperBound(0)
            m_lCurrentFileLine = 0

            oBodyNodes = Nothing
            oDoc = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to open and read file", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenAndReadFileXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function DocumentTitleCheckUsingSiriusDocumentUtility(ByVal v_sDocument As String) As Integer
        Dim result As Integer = 0
        Dim oConvert As Object
         Dim Content As String = ""
        Const kMethodName As String = "DocumentTitleCheckUsingSiriusDocumentUtility"

        Dim lInputFileNum, lFileLength As Integer
        Dim sAllFile As String = ""



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'lInputFileNum = FreeFile()
            'Using fs As FileStream = File.Open(v_sDocument, FileMode.Open, FileAccess.Read)
            '    Using sr As New StreamReader(fs)
            '        Content = sr.ReadToEnd()
            '    End Using
            'End Using
            ''File.Open(lInputFileNum, v_sDocument, OpenMode.Input)
            'lFileLength = LOF(v_sDocument)
            ''sAllFile = InputString(lInputFileNum, lFileLength)
            'sAllFile = InputString(lInputFileNum, lFileLength, v_sDocument)
            lInputFileNum = FileSystem.FreeFile()
            FileSystem.FileOpen(lInputFileNum, v_sDocument, OpenMode.Input)
            lFileLength = FileSystem.LOF(lInputFileNum)
            sAllFile = FileSystem.InputString(lInputFileNum, lFileLength)
            FileSystem.FileClose(lInputFileNum)

            If sAllFile.IndexOf("<o:Title>", StringComparison.CurrentCultureIgnoreCase) >= 0 Then

                oConvert = New SiriusDocumentUtility.Document()

                oConvert.ClearDocumentTitlePropertyIfMergeCode(ToSafeString(v_sDocument))

            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="")


        Finally
            oConvert = Nothing

            '        Return result

            ' This is for debugging only
            '        
            '        Return result
        End Try
        Return result
    End Function
    Public Function GetCurrentLineNo() As Integer
        Dim result As Integer = 0

        Try
            If Not EOF Then
                GetCurrentLineNo = m_lCurrentFileLine
            Else
                GetCurrentLineNo = 0
            End If

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get current line No.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentLineNo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetLine(ByRef r_sFileLine As String, ByVal v_lLineNo As Integer) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sFileLine = ""

            If Not EOF Then
                r_sFileLine = CStr(m_vFileArray(v_lLineNo))
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed getting line", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLine", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ReplaceTag(ByVal v_sTag As String, ByVal v_sValue As String) As Integer

        Dim iPageMgrPos As Integer
        Dim iPageMgrEndPos As Integer
        Dim sPageMgrTag As String

        Dim iTmpPos As Integer = m_lCurrentFileLine + 1
        Dim sLine As String = ""

        Try

            Do While True

                If iTmpPos > m_lTotalFileLines Then
                    Exit Do
                End If

                sLine = CStr(m_vFileArray(iTmpPos))

                 iPageMgrPos = sLine.IndexOf(v_sTag, 1) + 1

                If iPageMgrPos > 0 Then
                    iPageMgrEndPos = sLine.IndexOf(">", iPageMgrPos) + 1
                    sPageMgrTag = Mid(sLine, iPageMgrPos, iPageMgrEndPos - iPageMgrPos + 1)
                    sLine = sLine.Replace(sPageMgrTag, v_sValue)

                    m_vFileArray(iTmpPos) = sLine

                    Exit Do

                End If

                iTmpPos = iTmpPos + 1
            Loop
        Catch excep As System.Exception
            'dont throw any exception
        End Try

        Return gPMConstants.PMEReturnCode.PMTrue
    End Function
    Public Function GetNextLine(ByRef r_sFileLine As String) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not EOF Then
                r_sFileLine = CStr(m_vFileArray(m_lCurrentFileLine))
                m_lCurrentFileLine = m_lCurrentFileLine + 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed while getting next file line", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextLine", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function CheckNameSpaces(ByRef r_vXML As String) As Integer
        Dim sNSSmartTag As String
        Dim sDoubleQuote As String
        Dim lPos As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        sDoubleQuote = ChrW(34)

        sNSSmartTag = " xmlns:st0=" & sDoubleQuote & "urn:schemas-microsoft-com:office:smarttags" & sDoubleQuote
        sNSSmartTag = sNSSmartTag & " xmlns:st1=" & sDoubleQuote & "urn:schemas-microsoft-com:office:smarttags" & sDoubleQuote
        If r_vXML <> "" Then
        If r_vXML.IndexOf("urn:schemas-microsoft-com:office:smarttags", 1) + 1 = 0 Then
            lPos = r_vXML.IndexOf("wordDocument", 1) + 1

            r_vXML = Informations.Left$(r_vXML, lPos + Len("wordDocument") - 1) & sNSSmartTag & " " & Mid$(r_vXML, lPos + Len("wordDocument") + 1)
        End If
            End If
        Return result
    End Function
    Public Function AddNodesInXML(ByVal v_sParentNode As String, ByVal v_sNodes As String, ByRef r_vXML As String) As Integer
        Dim lPos As Integer
        Dim lPosEndMark As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            lPos = r_vXML.IndexOf(v_sParentNode, 1) + 1

            If lPos > 0 Then
                lPosEndMark = r_vXML.IndexOf(">", lPos) + 1
                r_vXML = Informations.Left$(r_vXML, lPosEndMark) & v_sNodes & Mid$(r_vXML, lPosEndMark + 1)
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed while getting next file line", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextLine", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try

    End Function
    Public Function GetNodeXML(ByVal v_sXML As String, ByVal v_sStartingNode As String, ByVal v_sEndingNode As String, ByRef r_vNodeXML As String) As Integer
        Dim lPos As Integer
        Dim lEndPos As Integer
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            r_vNodeXML = ""

            v_sStartingNode = GetTagProperName(v_sStartingNode, v_sXML)

            lPos = v_sXML.IndexOf(v_sStartingNode, 1) + 1

            If lPos > 0 Then
                lEndPos = v_sXML.LastIndexOf(v_sEndingNode) + 1 + Len(v_sEndingNode)
            End If

            r_vNodeXML = Mid$(v_sXML, lPos, lEndPos - lPos)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed while getting next file line", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextLine", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
        Return result
    End Function
    Private Function AddItemInFileArray(ByVal v_vValue As Object) As Integer

        Dim lReturn As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        If Not Informations.IsArray(m_vFileArray) Then
            ReDim m_vFileArray(0)
        Else
            ReDim Preserve m_vFileArray(m_vFileArray.GetUpperBound(0) + 1)
        End If

        lReturn = RemoveInvalidCharacters(v_vValue)

        m_vFileArray(m_vFileArray.GetUpperBound(0)) = v_vValue
        Return result
    End Function

End Class

