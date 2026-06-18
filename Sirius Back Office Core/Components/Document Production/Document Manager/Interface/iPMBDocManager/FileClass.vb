Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Imports System.IO
Imports System.Text


<System.Runtime.InteropServices.ProgId("FileClass_NET.FileClass")>
Public NotInheritable Class FileClass

    Private Const ACClass As String = "FileClass"
    Private Const cDefaultStartTag As String = "&LT;@"

    Private m_sFileName As String = ""
    Private m_vFileArray() As Object
    Private m_lCurrentFileLine As Integer
    Private m_lTotalFileLines As Integer
    Private m_sUsername As String = ""
    Private m_sStartTag As String = cDefaultStartTag
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

        Dim oDoc As Xml.XmlDocument
        Dim oBodyNodes As Xml.XmlNodeList
        Dim newXmlDeclaration As Xml.XmlDeclaration
        Dim result As Integer = 0


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
                sPreBodyFragment = Split(oDoc.OuterXml, "<w:body>")

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

            m_lTotalFileLines = UBound(m_vFileArray)
            m_lCurrentFileLine = 0
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to open and read file", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenAndReadFileXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        Finally
            oBodyNodes = Nothing
            oDoc = Nothing
        End Try
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

            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get current line No.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentLineNo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed getting line", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLine", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                iPageMgrPos = InStr(1, sLine, v_sTag)

                If iPageMgrPos > 0 Then
                    iPageMgrEndPos = InStr(iPageMgrPos, sLine, ">")
                    sPageMgrTag = Mid(sLine, iPageMgrPos, iPageMgrEndPos - iPageMgrPos + 1)
                    sLine = Replace(sLine, sPageMgrTag, v_sValue)

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

            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed while getting next file line", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextLine", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CheckNameSpaces(ByRef r_vXML As String) As Integer
        Dim sNSSmartTag As String
        Dim sDoubleQuote As String
        Dim lPos As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        sDoubleQuote = Chr(34)

        sNSSmartTag = " xmlns:st0=" & sDoubleQuote & "urn:schemas-microsoft-com:office:smarttags" & sDoubleQuote
        sNSSmartTag = sNSSmartTag & " xmlns:st1=" & sDoubleQuote & "urn:schemas-microsoft-com:office:smarttags" & sDoubleQuote
        If InStr(1, r_vXML, "urn:schemas-microsoft-com:office:smarttags") = 0 Then
            lPos = InStr(1, r_vXML, "wordDocument")

            r_vXML = Left$(r_vXML, lPos + Len("wordDocument") - 1) & sNSSmartTag & " " & Mid$(r_vXML, lPos + Len("wordDocument") + 1)
        End If
        Return result
    End Function
    Public Function AddNodesInXML(ByVal v_sParentNode As String, ByVal v_sNodes As String, ByRef r_vXML As String) As Integer
        Dim lPos As Integer
        Dim lPosEndMark As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            lPos = InStr(1, r_vXML, v_sParentNode)

            If lPos > 0 Then
                lPosEndMark = InStr(lPos, r_vXML, ">")
                r_vXML = Left$(r_vXML, lPosEndMark) & v_sNodes & Mid$(r_vXML, lPosEndMark + 1)
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed while getting next file line", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextLine", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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

            lPos = InStr(1, v_sXML, v_sStartingNode)

            If lPos > 0 Then
                lEndPos = InStrRev(v_sXML, v_sEndingNode) + Len(v_sEndingNode)
            End If

            r_vNodeXML = Mid$(v_sXML, lPos, lEndPos - lPos)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed while getting next file line", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextLine", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    Private Function AddItemInFileArray(ByVal v_vValue As Object) As Integer

        Dim lReturn As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        If Not IsArray(m_vFileArray) Then
            ReDim m_vFileArray(0)
        Else
            ReDim Preserve m_vFileArray(UBound(m_vFileArray) + 1)
        End If

        lReturn = RemoveInvalidCharacters(v_vValue)

        m_vFileArray(UBound(m_vFileArray)) = v_vValue
    End Function


End Class

