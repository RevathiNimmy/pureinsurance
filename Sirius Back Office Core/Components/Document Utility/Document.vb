Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Xml
Imports Aspose.Words
Imports Aspose.Words.Fonts
Imports Aspose.Words.Saving
Imports SSP.Shared
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Linq





Public NotInheritable Class Document


#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "110C8DCD-7B7E-4ed9-90ED-28B8EE0DB889"
    Public Const InterfaceId As String = "4A5B34AA-4309-4110-B1A7-29E8AC785EBD"
    Public Const EventsId As String = "62E6F92F-76BC-4ec5-A418-A2C41488B2E6"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
#Region " Constructors "
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal sDoc As System.IO.Stream, ByVal sfilePath As String)
        Dim oDoc As Aspose.Words.Document
        Dim oLic As Aspose.Words.License

        'Load Document
        oLic = New Aspose.Words.License
        oLic.SetLicense("Aspose.Totalfor.NET.lic")
        oDoc = New Aspose.Words.Document(sDoc)
        oLic.SetLicense("Aspose.Totalfor.NET.lic")
        oDoc.Save(sfilePath)

        oDoc = Nothing
        oLic = Nothing
    End Sub
    Public Sub Close()
        'Destroy any left resources
    End Sub
#End Region

#Region " Declarations "

#End Region

    Private Const PMTrue As Integer = 1
    Private Const PMError As Integer = 11

#Region " Public Methods "
    'TODO: UpdateToc
    Dim m_sSubDocFilePath As String
    'Public Sub InsertSubDoc(ByVal v_sFilePath As String, _
    '                            ByVal v_sStringToReplace As String, _
    '                            ByVal v_sSubDocFilePath As String)

    '    'Check Both Files Exist
    '    If Not (System.IO.File.Exists(v_sFilePath)) Then Exit Sub
    '    If Not (System.IO.File.Exists(v_sSubDocFilePath)) Then Exit Sub

    '    m_sSubDocFilePath = v_sSubDocFilePath

    '    'Open Destination Document
    '    Dim oMainDocument As Aspose.Words.Document
    '    Dim oLic As Aspose.Words.License
    '    oLic = New Aspose.Words.License
    '    oLic.SetLicense("Aspose.Words.lic")
    '    oMainDocument = New Aspose.Words.Document(v_sFilePath)
    '    oLic.SetLicense("Aspose.Words.lic")

    '    'Find Existances in Destination Document
    '    Dim oSection As Aspose.Words.Section
    '    For Each oSection In oMainDocument.Sections
    '        oSection.Range.Replace(New System.Text.RegularExpressions.Regex(v_sStringToReplace), New Aspose.Words.ReplaceEvaluator(AddressOf CustomSubDocReplacer), False)
    '    Next

    '    'Save Destination
    '    oMainDocument.Save(v_sFilePath)
    '    oMainDocument = Nothing

    'End Sub

    'Private Function CustomSubDocReplacer(ByVal sender As Object, ByVal e As Aspose.Words.ReplaceEvaluatorArgs) As Aspose.Words.ReplaceAction
    '    Dim oSubDocument As Aspose.Words.Document
    '    Dim oLic As Aspose.Words.License
    '    oLic = New Aspose.Words.License
    '    oLic.SetLicense("Aspose.Words.lic")
    '    oSubDocument = New Aspose.Words.Document(m_sSubDocFilePath)
    '    oLic.SetLicense("Aspose.Words.lic")
    '    Dim oImporter As New Aspose.Words.NodeImporter(oSubDocument.Document, e.MatchNode.Document, Aspose.Words.ImportFormatMode.KeepSourceFormatting)
    '    Dim oAppendNode As Aspose.Words.Node = oImporter.ImportNode(oSubDocument.GetChild(Aspose.Words.NodeType.Section, 0, True), True)
    '    Dim oParentNode As Aspose.Words.Node = e.MatchNode.ParentNode.ParentNode.ParentNode
    '    e.MatchNode.Document.InsertBefore(oAppendNode, oParentNode)
    '    e.Replacement = ""
    '    e.MatchNode.Remove()
    'End Function

    Public Sub UpdateToc(ByVal v_sFilename As String)
    End Sub

    Public Sub AppendBreakToDocument(ByVal v_DocumentToAppendTo As String)
        Dim oDoc As Aspose.Words.Document
        Dim oLic As Aspose.Words.License

        'Check File Exists
        If Not (System.IO.File.Exists(v_DocumentToAppendTo)) Then Exit Sub

        'Load Document
        oLic = New Aspose.Words.License
        oLic.SetLicense("Aspose.Totalfor.NET.lic")
        oDoc = New Aspose.Words.Document(v_DocumentToAppendTo)
        oLic.SetLicense("Aspose.Totalfor.NET.lic")

        Dim oBuilder As New Aspose.Words.DocumentBuilder(oDoc)
        oBuilder.InsertBreak(Aspose.Words.BreakType.SectionBreakNewPage)
        oBuilder.Document.Save(v_DocumentToAppendTo)
        oBuilder = Nothing

        oDoc = Nothing
        oLic = Nothing
    End Sub


    Public Sub AppendDocument(ByVal v_DocumentToAppendTo As String, ByVal v_DocumentToAppend As String, ByVal bAppendBreak As Boolean)
        Dim oDoc As Aspose.Words.Document
        Dim oAppendDoc As Aspose.Words.Document
        Dim oLic As Aspose.Words.License

        'Check File Exists
        If Not (System.IO.File.Exists(v_DocumentToAppendTo)) Then Exit Sub
        If Not (System.IO.File.Exists(v_DocumentToAppend)) Then Exit Sub

        'Load Document
        oLic = New Aspose.Words.License
        oLic.SetLicense("Aspose.Totalfor.NET.lic")
        oDoc = New Aspose.Words.Document(v_DocumentToAppendTo)
        oAppendDoc = New Aspose.Words.Document(v_DocumentToAppend)
        oLic.SetLicense("Aspose.Totalfor.NET.lic")

        oDoc.AppendDocument(oAppendDoc, Aspose.Words.ImportFormatMode.KeepSourceFormatting)

        If bAppendBreak Then
            Dim oBuilder As New Aspose.Words.DocumentBuilder(oDoc)
            oBuilder.MoveToDocumentEnd()
            oBuilder.InsertBreak(Aspose.Words.BreakType.SectionBreakNewPage)
            oBuilder.Document.Save(v_DocumentToAppendTo)
            oBuilder = Nothing
        Else
            oDoc.Save(v_DocumentToAppendTo)
        End If

        oDoc = Nothing
        oAppendDoc = Nothing
        oLic = Nothing
    End Sub

    Public Sub AddToc(ByVal v_sDocument As String, ByVal v_lSectionNumber As Integer)
        Dim oDoc As Aspose.Words.Document
        Dim oLic As Aspose.Words.License

        'Add When Document Finished

        'Check File Exists
        If Not (System.IO.File.Exists(v_sDocument)) Then Exit Sub

        'Load Document
        oLic = New Aspose.Words.License
        oLic.SetLicense("Aspose.Totalfor.NET.lic")
        oDoc = New Aspose.Words.Document(v_sDocument)
        oLic.SetLicense("Aspose.Totalfor.NET.lic")
        Dim oBuilder As New Aspose.Words.DocumentBuilder(oDoc)

        'Goto all sections and reset
        Dim lCount As Long
        For lCount = 0 To oDoc.Sections.Count - 1
            oBuilder.MoveToSection(lCount)
            oDoc.Sections(lCount).Range.UpdateFields()
        Next lCount

        'Goto Sections and add TOC plus page break
        oBuilder.MoveToSection(v_lSectionNumber)
        oBuilder.InsertTableOfContents("\o ""1-3"" \h \z \u")
        oBuilder.InsertBreak(Aspose.Words.BreakType.PageBreak)

        'Save Document
        oBuilder.Document.Save(v_sDocument)
        oBuilder = Nothing
        oDoc = Nothing
        oLic = Nothing

        UpdateToc(v_sDocument)

    End Sub

    Dim mDocument As Aspose.Words.Document = Nothing
    Dim mDocumentLicense As Aspose.Words.License
    Dim mFilename As String
    Public Sub GetDocumentPageAsImage(ByVal v_sDocumentFilename As String,
        ByVal v_DestinationFilename As String, ByVal v_lPageCount As Integer, ByRef r_lMaxPages As Integer)

        If Not (System.IO.File.Exists(v_sDocumentFilename)) Then Exit Sub

        If mFilename <> v_sDocumentFilename Then
            If mDocument IsNot Nothing Then
                mDocument = Nothing
            End If
            mDocumentLicense = New Aspose.Words.License
            mDocumentLicense.SetLicense("Aspose.Totalfor.NET.lic")
            mDocument = New Aspose.Words.Document(v_sDocumentFilename)
            mDocumentLicense.SetLicense("Aspose.Totalfor.NET.lic")
            mFilename = v_sDocumentFilename
        End If
        r_lMaxPages = mDocument.PageCount

        If v_lPageCount <= r_lMaxPages Then
            mDocument.Save(v_DestinationFilename)
        End If

    End Sub

    Public Function GenerateWordMLForPDFWithImages(wordmlXml As String, tempDirectory As String) As MemoryStream
        Dim wordDocXml As New XmlDocument()
        wordDocXml.LoadXml(wordmlXml)

        ' Namespace manager
        Dim nsmgr As New XmlNamespaceManager(wordDocXml.NameTable)
        nsmgr.AddNamespace("w", "http://schemas.microsoft.com/office/word/2003/wordml")
        nsmgr.AddNamespace("v", "urn:schemas-microsoft-com:vml")

        ' temp directory 
        If Not Directory.Exists(tempDirectory) Then
            Directory.CreateDirectory(tempDirectory)
        End If

        ' Cache mapping
        Dim cacheImage As New Dictionary(Of String, String)()


        For Each binData As XmlNode In wordDocXml.SelectNodes("//w:binData", nsmgr)
            Dim nameAttr = binData.Attributes("w:name")
            If nameAttr IsNot Nothing Then
                Dim wordmlName As String = nameAttr.Value
                Dim fileName As String = Path.Combine(tempDirectory, Path.GetFileName(wordmlName))

                Dim base64Data As String = Regex.Replace(binData.InnerText, "\s+", "")
                Dim bytesConversion As Byte() = System.Convert.FromBase64String(base64Data)

                ' Write to file
                File.WriteAllBytes(fileName, bytesConversion)

                cacheImage(wordmlName) = fileName
            End If
        Next

        ' 2. Rewrite <v:imagedata src>
        For Each img As XmlNode In wordDocXml.SelectNodes("//v:imagedata", nsmgr)
            Dim srcAttribute = img.Attributes("src")
            If srcAttribute IsNot Nothing AndAlso cacheImage.ContainsKey(srcAttribute.Value) Then
                srcAttribute.Value = cacheImage(srcAttribute.Value)
            End If
        Next

        ' 3. Return as MemoryStream
        Dim msXmlSave As New MemoryStream()
        wordDocXml.Save(msXmlSave)
        msXmlSave.Position = 0
        Return msXmlSave
    End Function

    Public Function Convert(ByVal sSourceFile As String, ByVal sDestinationFile As String, Optional ByVal sCCMDocumentName As String = "") As Integer
        Dim sTempDirectory As String

        Try

            'Check Source Type supported
            If Not ((System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".DOC") Or
                            (System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".HTM") Or
                            (System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".HTML") Or
                            (System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".XML") Or
                            (System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".DOCX")) Then
                'Not Supported
            End If

            If ISCCMEnabled() AndAlso System.IO.Path.GetExtension(sDestinationFile).ToUpper().EndsWith("PDF") AndAlso
                (Not IsKCMForSelectedDocument() OrElse sCCMDocumentName <> "") Then
                Try
                    ConvertWordToPDF(sSourceFile, sDestinationFile)
                    If File.Exists(sDestinationFile) = False Then
                        bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sDestinationFile + " file does not exists. Document.Convert Failed.")
                    End If
                    Return True
                Catch ex As Exception
                    bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sDestinationFile + " file does not exists. Document.Convert Failed from KCM.")
                End Try

            End If

            'Check Destination Type supported
            Dim oDocSaveType As Aspose.Words.SaveFormat
            If (System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".DOCX") Then
                oDocSaveType = Aspose.Words.SaveFormat.Docx
            ElseIf (System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".DOC") Then
                oDocSaveType = Aspose.Words.SaveFormat.Doc
            ElseIf ((System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".HTM") Or
                   (System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".HTML")) Then
                oDocSaveType = Aspose.Words.SaveFormat.Html
            ElseIf (System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".PDF") Then
                oDocSaveType = Aspose.Words.SaveFormat.Pdf
            ElseIf (System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".XML") Then
                oDocSaveType = Aspose.Words.SaveFormat.WordML
            ElseIf ((System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".TIF") Or
                 (System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".TIFF")) Then
                oDocSaveType = Aspose.Words.SaveFormat.Tiff
            ElseIf (System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".TXT") Then
                oDocSaveType = Aspose.Words.SaveFormat.Text
            ElseIf ((System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".TIF") Or
             (System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".TIFF")) Then
                oDocSaveType = Aspose.Words.SaveFormat.Tiff
            Else
                'Not supported

            End If

            'Create Temporary Folder
            sTempDirectory = ""
            Try
                sTempDirectory = System.IO.Path.GetTempPath() & System.Guid.NewGuid.ToString() & "\"
                System.IO.Directory.CreateDirectory(sTempDirectory)
            Catch ex As Exception
            End Try

            'Copy Document (and dependancy folder)
            Dim sLocalSourceFile As String = ""
            Dim bSourceHtml As Boolean = False

            If ((System.IO.Path.GetExtension(sSourceFile).ToUpper = ".HTM") Or
                            (System.IO.Path.GetExtension(sSourceFile).ToUpper = ".HTML")) Then
                bSourceHtml = True
                sLocalSourceFile = sTempDirectory & System.IO.Path.GetFileName(sSourceFile)

                'Open Source file (sSourceFile) and write to localfile (sLocalSourceFile) (replacing %20 to ' ')
                Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(sSourceFile)
                If sFileName.IndexOf(" ") + 1 > 0 Then
                    Dim sFindString As String = sFileName.Replace(" ", "%20")
                    Dim sReplaceString As String = sFileName
                    Dim SourceFile As New StreamReader(sSourceFile, System.Text.Encoding.Default)
                    Dim DestinationFile As New StreamWriter(sLocalSourceFile, False, System.Text.Encoding.Default)
                    Dim sTempLine As String
                    'SourceFile = System.IO.File.OpenText(sSourceFile)

                    While SourceFile.Peek <> -1
                        sTempLine = SourceFile.ReadLine()
                        sTempLine = sTempLine.Replace(sFindString, sReplaceString)
                        Dim lTemp As Long = sTempLine.IndexOf("<P ") + 1
                        If lTemp > 0 Then
                            If Mid(sTempLine, lTemp).IndexOf(">") + 1 = 0 Then
                                If SourceFile.Peek <> -1 Then
                                    Do
                                        sTempLine = sTempLine & SourceFile.ReadLine()
                                    Loop Until (Mid(sTempLine, lTemp).IndexOf(">") + 1 > 0) Or SourceFile.Peek <> -1
                                End If
                            End If
                            sTempLine = CorrectSpacing(sTempLine)
                        End If
                        DestinationFile.WriteLine(sTempLine)
                    End While

                    SourceFile.Close()
                    DestinationFile.Close()
                Else
                    sLocalSourceFile = sSourceFile
                End If

                'Copy Dependancies
                Dim sSourceDependanciesDir As String
                Dim sDestDependanciesDir As String
                sSourceDependanciesDir = System.IO.Path.GetDirectoryName(sSourceFile)
                If Right(Trim(sSourceDependanciesDir), 1) <> "\" Then sSourceDependanciesDir = sSourceDependanciesDir & "\"
                sSourceDependanciesDir = sSourceDependanciesDir & System.IO.Path.GetFileNameWithoutExtension(sSourceFile) & "_files\"
                sDestDependanciesDir = sTempDirectory & System.IO.Path.GetFileNameWithoutExtension(sSourceFile) & "_files\"
                If System.IO.Directory.Exists(sSourceDependanciesDir) Then
                    CopyFolder(sSourceDependanciesDir, sDestDependanciesDir, True)
                End If
            Else
                'Open Existing File
                sLocalSourceFile = sSourceFile
            End If

            'Convert Document
            ' Create Conversion Objects
            Dim oAsposeWordsLic As New Aspose.Words.License
            oAsposeWordsLic.SetLicense("Aspose.Totalfor.NET.lic")

            Dim oAsposeWords As Aspose.Words.Document
            Try
                oAsposeWords = New Aspose.Words.Document(sLocalSourceFile)
                oAsposeWordsLic.SetLicense("Aspose.Totalfor.NET.lic")
                If oDocSaveType = Aspose.Words.SaveFormat.WordML Then
                    oAsposeWords.RemoveSmartTags()
                End If
            Catch ex As Exception
                Dim htmlDoc As HtmlAgilityPack.HtmlDocument = New HtmlAgilityPack.HtmlDocument()
                htmlDoc.OptionFixNestedTags = True
                htmlDoc.OptionOutputOriginalCase = True
                htmlDoc.Load(sLocalSourceFile)
                htmlDoc.Save(sLocalSourceFile)
                oAsposeWords = New Aspose.Words.Document(sLocalSourceFile)
                oAsposeWordsLic.SetLicense("Aspose.Totalfor.NET.lic")
            End Try

            If System.IO.File.Exists(sDestinationFile) Then System.IO.File.Delete(sDestinationFile)

            If bSourceHtml Then
                Dim oAsposeDocBuilder As Aspose.Words.DocumentBuilder = New Aspose.Words.DocumentBuilder(oAsposeWords)

                'Handle Headers and Footers (excluded from aspose html solution).
                'Parse Source File (and add to Aspose.Words Document).

                Dim curSection As Aspose.Words.Section = oAsposeDocBuilder.CurrentSection
                Dim lCurrentSection As Long = 1
                If curSection IsNot Nothing Then
                    Do

                        'Debug.Print(curSection.ToTxt)

                        Dim lHeaderMargin As Long = 10
                        Dim lFooterMargin As Long = 10
                        Dim sHeaderUrl As String = ""
                        Dim sFooterUrl As String = ""
                        Dim sHeaderCode As String = ""
                        Dim sFooterCode As String = ""

                        'parse source html file and determine header/footer variables
                        Dim parseFile As New StreamReader(sLocalSourceFile, System.Text.Encoding.Default)
                        Dim sTempLine As String

                        While parseFile.Peek <> -1
                            sTempLine = parseFile.ReadLine()
                            If sTempLine = "@page Section" & Trim(lCurrentSection.ToString) Then
                                Dim bFinished As Boolean = False
                                While (parseFile.Peek <> -1) Xor (bFinished)
                                    sTempLine = parseFile.ReadLine()
                                    If sTempLine.StartsWith(ChrW(9)) Then
                                        If sTempLine.StartsWith(ChrW(9) & "mso-header-margin:") Then
                                            'lHeaderMargin = Val(sTempLine.Substring(sTempLine.LastIndexOf("mso-header-margin:") + 18))
                                        ElseIf sTempLine.StartsWith(ChrW(9) & "mso-footer-margin:") Then
                                            'lFooterMargin = Val(sTempLine.Substring(sTempLine.LastIndexOf("mso-footer-margin:") + 18))
                                        ElseIf sTempLine.StartsWith(ChrW(9) & "mso-header:url") Then
                                            sHeaderUrl = sTempLine.Substring(sTempLine.IndexOf("""") + 1, sTempLine.LastIndexOf("""") - (sTempLine.IndexOf("""") + 1))
                                            sHeaderCode = Trim(sTempLine.Substring(sTempLine.LastIndexOf(" "), sTempLine.LastIndexOf(";") - sTempLine.LastIndexOf(" ")))
                                        ElseIf sTempLine.StartsWith(ChrW(9) & "mso-footer:url") Then
                                            sFooterUrl = sTempLine.Substring(sTempLine.IndexOf("""") + 1, sTempLine.LastIndexOf("""") - (sTempLine.IndexOf("""") + 1))
                                            sFooterCode = Trim(sTempLine.Substring(sTempLine.LastIndexOf(" "), sTempLine.LastIndexOf(";") - sTempLine.LastIndexOf(" ")))
                                        End If
                                    Else
                                        bFinished = True
                                    End If
                                End While
                                Exit While
                            End If
                        End While

                        parseFile.Close()

                        Dim sBasePath As String = ""
                        sBasePath = System.IO.Path.GetDirectoryName(sLocalSourceFile)
                        If Right(Trim(sBasePath), 1) <> "\" Then sBasePath = sBasePath & "\"


                        'get header text values from specified File
                        Dim pgSetup As Aspose.Words.PageSetup = curSection.PageSetup

                        pgSetup.HeaderDistance = lHeaderMargin
                        If sHeaderUrl.Length > 0 Then

                            'Create Footer Section
                            oAsposeDocBuilder.MoveToHeaderFooter(Aspose.Words.HeaderFooterType.HeaderPrimary)

                            Dim sheaderHtml As String = ""
                            Dim sheaderTempLine As String = ""
                            If System.IO.File.Exists(sBasePath & sHeaderUrl) Then
                                Dim headerFile As New StreamReader(sBasePath & sHeaderUrl, System.Text.Encoding.Default)
                                While headerFile.Peek <> -1
                                    sheaderTempLine = headerFile.ReadLine()
                                    If sheaderTempLine = "<div style='mso-element:header' id=" & sHeaderCode & ">" Then
                                        'Get Lines until next </DIV>
                                        sheaderHtml = "" 'sFooterTempLine
                                        sheaderTempLine = ""
                                        While headerFile.Peek <> -1 Xor sheaderTempLine.StartsWith("</div>")
                                            If sheaderTempLine.Length > 0 Then
                                                sheaderTempLine = CorrectSpacing(sheaderTempLine)
                                                sheaderHtml = sheaderHtml & vbCrLf & sheaderTempLine.Replace("%20", " ")
                                            End If
                                            sheaderTempLine = headerFile.ReadLine()
                                        End While
                                        Exit While
                                    End If

                                End While
                                headerFile.Close()
                            End If
                            If sheaderHtml.Contains("v:shape") Then
                                Dim sTemp As String

                                'From last v:shape
                                sTemp = sheaderHtml.Substring(sheaderHtml.LastIndexOf("<v:shape "), sheaderHtml.IndexOf(">", sheaderHtml.LastIndexOf("<v:shape ")) - sheaderHtml.LastIndexOf("<v:shape "))
                                Dim dblImageHeight As Double = 0
                                If sTemp.Contains("height:") Then
                                    dblImageHeight = Val(sTemp.Substring(sTemp.IndexOf("height:") + 7))
                                End If
                                Dim dblImageWidth As Double = 0
                                If sTemp.Contains("width:") Then
                                    dblImageWidth = Val(sTemp.Substring(sTemp.IndexOf("width:") + 6))
                                End If

                                'From Initial v:shape
                                sTemp = sheaderHtml.Substring(sheaderHtml.IndexOf("<v:shape "), sheaderHtml.IndexOf(">", sheaderHtml.IndexOf("<v:shape ")) - sheaderHtml.IndexOf("<v:shape "))
                                Dim dblImageTop As Double = 0 'margin-top
                                If sTemp.Contains("margin-top:") Then
                                    dblImageTop = Val(sTemp.Substring(sTemp.IndexOf("margin-top:") + 11))
                                End If
                                Dim dblImageLeft As Double = 0 'margin-left
                                If sTemp.Contains("margin-left:") Then
                                    dblImageLeft = Val(sTemp.Substring(sTemp.IndexOf("margin-left:") + 12))
                                End If

                                Dim sImageFilename As String = ""
                                Dim sHeaderBasePath As String = ""
                                sHeaderBasePath = System.IO.Path.GetDirectoryName(sBasePath & sHeaderUrl)
                                If Right(Trim(sHeaderBasePath), 1) <> "\" Then sHeaderBasePath = sHeaderBasePath & "\"

                                sImageFilename = sheaderHtml.Substring(sheaderHtml.IndexOf("v:imagedata src=""") + 17, sheaderHtml.IndexOf("""", sheaderHtml.IndexOf("v:imagedata src=""") + 18) - (sheaderHtml.IndexOf("v:imagedata src=""") + 17))

                                oAsposeDocBuilder.InsertImage(sHeaderBasePath & sImageFilename, Aspose.Words.Drawing.RelativeHorizontalPosition.Page, dblImageLeft, Aspose.Words.Drawing.RelativeVerticalPosition.Page, dblImageTop, dblImageWidth, dblImageHeight, Aspose.Words.Drawing.WrapType.None)
                            End If

                            'AppendHtmlToDocBuild(oAsposeDocBuilder, sheaderHtml)
                            oAsposeDocBuilder.InsertHtml(sheaderHtml)

                            curSection.HeadersFooters.LinkToPrevious(False)

                        End If


                        'get Footer text values from specified File
                        pgSetup.FooterDistance = lFooterMargin
                        If sFooterUrl.Length > 0 Then

                            'Create Footer Section
                            oAsposeDocBuilder.MoveToHeaderFooter(Aspose.Words.HeaderFooterType.FooterPrimary)

                            Dim sFooterHtml As String = ""
                            Dim sFooterTempLine As String = ""
                            If System.IO.File.Exists(sBasePath & sFooterUrl) Then
                                Dim footerFile As New StreamReader(sBasePath & sFooterUrl, System.Text.Encoding.Default)
                                While footerFile.Peek <> -1
                                    sFooterTempLine = footerFile.ReadLine()

                                    If sFooterTempLine = "<div style='mso-element:footer' id=" & sFooterCode & ">" Then
                                        'Get Lines until next </DIV>
                                        sFooterHtml = "" 'sFooterTempLine
                                        sFooterTempLine = ""
                                        While footerFile.Peek <> -1 Xor sFooterTempLine.StartsWith("</div>")
                                            If sFooterTempLine.Length > 0 Then
                                                sFooterTempLine = CorrectSpacing(sFooterTempLine)
                                                sFooterHtml = sFooterHtml & vbCrLf & sFooterTempLine.Replace("%20", " ")
                                            End If

                                            sFooterTempLine = footerFile.ReadLine()
                                        End While

                                        Exit While
                                    End If

                                End While
                                footerFile.Close()
                            End If

                            If sFooterHtml.Contains("v:shape") Then
                                Dim sTemp As String

                                'From last v:shape
                                sTemp = sFooterHtml.Substring(sFooterHtml.LastIndexOf("<v:shape "), sFooterHtml.IndexOf(">", sFooterHtml.LastIndexOf("<v:shape ")) - sFooterHtml.LastIndexOf("<v:shape "))
                                Dim dblImageHeight As Double = 0
                                If sTemp.Contains("height:") Then
                                    dblImageHeight = Val(sTemp.Substring(sTemp.IndexOf("height:") + 7))
                                End If
                                Dim dblImageWidth As Double = 0
                                If sTemp.Contains("width:") Then
                                    dblImageWidth = Val(sTemp.Substring(sTemp.IndexOf("width:") + 6))
                                End If

                                'From Initial v:shape
                                sTemp = sFooterHtml.Substring(sFooterHtml.IndexOf("<v:shape "), sFooterHtml.IndexOf(">", sFooterHtml.IndexOf("<v:shape ")) - sFooterHtml.IndexOf("<v:shape "))
                                Dim dblImageTop As Double = 0 'margin-top
                                If sTemp.Contains("margin-top:") Then
                                    dblImageTop = Val(sTemp.Substring(sTemp.IndexOf("margin-top:") + 11))
                                End If
                                Dim dblImageLeft As Double = 0 'margin-left
                                If sTemp.Contains("margin-left:") Then
                                    dblImageLeft = Val(sTemp.Substring(sTemp.IndexOf("margin-left:") + 12))
                                End If

                                Dim sImageFilename As String = ""
                                Dim sHeaderBasePath As String = ""
                                sHeaderBasePath = System.IO.Path.GetDirectoryName(sBasePath & sHeaderUrl)
                                If Right(Trim(sHeaderBasePath), 1) <> "\" Then sHeaderBasePath = sHeaderBasePath & "\"

                                sImageFilename = sFooterHtml.Substring(sFooterHtml.IndexOf("v:imagedata src=""") + 17, sFooterHtml.IndexOf("""", sFooterHtml.IndexOf("v:imagedata src=""") + 18) - (sFooterHtml.IndexOf("v:imagedata src=""") + 17))

                                oAsposeDocBuilder.InsertImage(sHeaderBasePath & sImageFilename, Aspose.Words.Drawing.RelativeHorizontalPosition.Page, dblImageLeft, Aspose.Words.Drawing.RelativeVerticalPosition.Page, dblImageTop, dblImageWidth, dblImageHeight, Aspose.Words.Drawing.WrapType.None)
                            End If

                            'AppendHtmlToDocBuild(oAsposeDocBuilder, sFooterHtml)
                            oAsposeDocBuilder.InsertHtml(sFooterHtml)

                            curSection.HeadersFooters.LinkToPrevious(False)

                        End If

                        'Move to next Section
                        If lCurrentSection >= oAsposeWords.Sections.Count Then
                            curSection = Nothing
                        Else
                            oAsposeDocBuilder.MoveToSection(lCurrentSection)
                            lCurrentSection = lCurrentSection + 1
                            curSection = oAsposeDocBuilder.CurrentSection
                        End If
                    Loop Until curSection Is Nothing
                End If

                oAsposeDocBuilder.Document.Save(sDestinationFile, oDocSaveType)
            Else
                ' Convert Document (if to PDF use Aspose.PDf and intermediatery xml file)
                Dim fontSettings As New Aspose.Words.Fonts.FontSettings()
                Dim platformId As PlatformID
                Dim isWindows As Boolean
                oAsposeWords.FontSettings = new FontSettings()
                platformId = Environment.OSVersion.Platform
                isWindows = (platformId = PlatformID.Win32NT) OrElse (platformId = PlatformID.Win32S) OrElse _
                (platformId = PlatformID.Win32Windows) OrElse (platformId = PlatformID.WinCE)
                If isWindows Then
                    Const fontsPath As String = "C:\Windows\Fonts"
                    oAsposeWords.FontSettings.SetFontsFolder(fontsPath, True)
                End If
                If (oDocSaveType = Aspose.Words.SaveFormat.Pdf) AndAlso
                       (System.IO.Path.GetExtension(sLocalSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".XML") Then
                    Dim wordmlContent As String = File.ReadAllText(sLocalSourceFile)
                    Dim returnXmlStream As MemoryStream = GenerateWordMLForPDFWithImages(wordmlContent, sTempDirectory)
                    ' Load into Aspose.Words
                    Dim oAsposeWordDoc As New Aspose.Words.Document(returnXmlStream)
                    oAsposeWordDoc.FontSettings = New FontSettings()
                    oAsposeWordDoc.FontSettings.SetFontsFolder("C:\Windows\Fonts", True)

                    Dim pdfSaveOptions As New PdfSaveOptions()
                    pdfSaveOptions.EmbedFullFonts = True
                    pdfSaveOptions.UseCoreFonts = True
                    pdfSaveOptions.SaveFormat = SaveFormat.Pdf
                    pdfSaveOptions.FontEmbeddingMode = PdfFontEmbeddingMode.EmbedAll
                    oAsposeWordDoc.Save(sDestinationFile, pdfSaveOptions)

                Else
                    If (System.IO.Path.GetExtension(sDestinationFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".DOCX") Then
                        oAsposeWords.RemoveMacros()
                    End If

                    oAsposeWords.Save(sDestinationFile, oDocSaveType)
                End If
            End If

            'Tidy Up Created Files
            System.IO.Directory.Delete(sTempDirectory, True)
            Return PMTrue

        Catch ex As Exception
            Throw New Exception("Failed to Convert File " & ex.Message)
        End Try
    End Function

    Public Function PrintDocument(ByVal sSourceFile As String, ByVal sPrinterName As String) As Integer
        Try
            Dim iReturn As Integer
            Dim sMessage As String = ""
            'Check File Exists
            If Not (System.IO.File.Exists(sSourceFile)) Then
                Throw New Exception("File Does Not Exist")
            End If

            'Get Source Type
            If System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".PDF" Then
                'Print Using QuickPdf
                iReturn = PrintDocumentPDF(sSourceFile, sMessage)
                If iReturn <> PMTrue Then
                    Throw New Exception(sMessage)
                End If
            ElseIf ((System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".DOC") Or
                (System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".HTM") Or
                (System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".HTML") Or
                (System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".XML")) Then

                '''''''''''''''''''''
                Dim sTempDirectory As String

                'Create Temporary Folder
                sTempDirectory = ""
                Try
                    sTempDirectory = System.IO.Path.GetTempPath() & System.Guid.NewGuid.ToString() & "\"
                    System.IO.Directory.CreateDirectory(sTempDirectory)
                Catch ex As Exception
                End Try

                'Copy Document (and dependancy folder)
                Dim sLocalSourceFile As String = ""
                If ((System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".HTM") Or
                                (System.IO.Path.GetExtension(sSourceFile).ToUpper(System.Globalization.CultureInfo.CurrentCulture) = ".HTML")) Then

                    sLocalSourceFile = sTempDirectory & System.IO.Path.GetFileName(sSourceFile)

                    'Open Source file (sSourceFile) and write to localfile (sLocalSourceFile) (replacing %20 to ' ')
                    Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(sSourceFile)
                    If sFileName.IndexOf(" ") + 1 > 0 Then
                        Dim sFindString As String = sFileName.Replace(" ", "%20")
                        Dim sReplaceString As String = sFileName
                        Dim SourceFile As New StreamReader(sSourceFile, System.Text.Encoding.Default)
                        Dim DestinationFile As New StreamWriter(sLocalSourceFile, False, System.Text.Encoding.Default)
                        Dim sTempLine As String
                        'SourceFile = System.IO.File.OpenText(sSourceFile)

                        While SourceFile.Peek <> -1
                            sTempLine = SourceFile.ReadLine()
                            sTempLine = sTempLine.Replace(sFindString, sReplaceString)
                            Dim lTemp As Long = sTempLine.IndexOf("<P ") + 1
                            If lTemp > 0 Then
                                If Mid(sTempLine, lTemp).IndexOf(">") + 1 = 0 Then
                                    If SourceFile.Peek <> -1 Then
                                        Do
                                            sTempLine = sTempLine & SourceFile.ReadLine()
                                        Loop Until (Mid(sTempLine, lTemp).IndexOf(">") + 1 > 0) Or SourceFile.Peek <> -1
                                    End If
                                End If
                                sTempLine = CorrectSpacing(sTempLine)
                            End If
                            DestinationFile.WriteLine(sTempLine)
                        End While

                        SourceFile.Close()
                        DestinationFile.Close()
                    Else
                        sLocalSourceFile = sSourceFile
                    End If

                    'Copy Dependancies
                    Dim sSourceDependanciesDir As String
                    Dim sDestDependanciesDir As String
                    sSourceDependanciesDir = System.IO.Path.GetDirectoryName(sSourceFile)
                    If Right(Trim(sSourceDependanciesDir), 1) <> "\" Then sSourceDependanciesDir = sSourceDependanciesDir & "\"
                    sSourceDependanciesDir = sSourceDependanciesDir & System.IO.Path.GetFileNameWithoutExtension(sSourceFile) & "_files\"
                    sDestDependanciesDir = sTempDirectory & System.IO.Path.GetFileNameWithoutExtension(sSourceFile) & "_files\"
                    If System.IO.Directory.Exists(sSourceDependanciesDir) Then
                        CopyFolder(sSourceDependanciesDir, sDestDependanciesDir, True)
                    End If
                Else
                    'Open Existing File
                    sLocalSourceFile = sSourceFile
                End If

                Dim oAsposeWordsLic As New Aspose.Words.License
                oAsposeWordsLic.SetLicense("Aspose.Totalfor.NET.lic")
                Dim oAsposeWords As New Aspose.Words.Document(sLocalSourceFile)
                oAsposeWordsLic.SetLicense("Aspose.Totalfor.NET.lic")

                'Convert to PDF and open in default viewer
                Dim sTempPdf As String = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.Guid.NewGuid.ToString() & ".pdf")
                oAsposeWords.Save(sTempPdf, Aspose.Words.SaveFormat.Pdf)
                System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(sTempPdf) With {.UseShellExecute = True})

                'Tidy Up Created Files
                System.IO.Directory.Delete(sTempDirectory, True)

                oAsposeWordsLic = Nothing
                oAsposeWords = Nothing

            Else
                Throw New Exception("Unsupported Type")
            End If
            Return PMConst.PMTrue
        Catch ex As Exception
            Return PMConst.PMFalse
        End Try
    End Function

    Public Function PrintDocumentPDF( _
        ByVal v_sSourceFile As String, _
        ByRef r_sMessage As String, _
        Optional ByVal v_sPrinterName As String = "") As Integer

        'Print Using QuickPdf

        PrintDocumentPDF = PMTrue

        Try
            r_sMessage = ""
            Dim QP As Object
            Dim iReturn As Integer = PMTrue

            Try
                QP = CreateObject("QuickPDFAX0714.PDFLibrary")
            Catch ex As Exception
                r_sMessage = "Failed to get instance of QuickPdf"
                PrintDocumentPDF = PMError
                Exit Function
            End Try

            iReturn = QP.UnlockKey("jp6ax3959um6zj3ca3xr9eb3y")
            If iReturn <> PMTrue Then
                r_sMessage = "Failed to set QuickPdf License"
                PrintDocumentPDF = PMError
                Exit Function
            End If

            iReturn = QP.LoadFromFile(v_sSourceFile.ToString)
            If iReturn <> PMTrue Then
                r_sMessage = "Failed to open Pdf file " & v_sSourceFile
                PrintDocumentPDF = PMError
                Exit Function
            End If

            If v_sPrinterName.Length = 0 Then
                v_sPrinterName = QP.GetDefaultPrinterName
            End If

            iReturn = QP.PrintDocument(v_sPrinterName.ToString, 1, QP.pagecount.ToString, QP.PrintOptions(1, 0, v_sSourceFile.ToString).ToString)
            If iReturn <> PMTrue Then
                r_sMessage = "Failed to print Pdf file " & v_sSourceFile & " to printer " & v_sPrinterName
                PrintDocumentPDF = PMError
                Exit Function
            End If

            QP = Nothing
        Catch ex As Exception

        End Try

    End Function

    Sub CopyFolder(ByVal SourcePath As String, ByVal DestPath As String, Optional ByVal Overwrite As Boolean = False)
        Dim SourceDir As DirectoryInfo = New DirectoryInfo(SourcePath)
        Dim DestDir As DirectoryInfo = New DirectoryInfo(DestPath)

        ' the source directory must exist, otherwise throw an exception
        If SourceDir.Exists Then
            ' if destination SubDir's parent SubDir does not exist throw an exception
            If Not DestDir.Parent.Exists Then
                Throw New DirectoryNotFoundException _
                    ("Destination directory does not exist: " + DestDir.Parent.FullName)
            End If

            If Not DestDir.Exists Then
                DestDir.Create()
            End If

            ' copy all the files of the current directory
            Dim ChildFile As FileInfo
            For Each ChildFile In SourceDir.GetFiles()
                If Overwrite Then
                    ChildFile.CopyTo(Path.Combine(DestDir.FullName, ChildFile.Name), True)
                Else
                    ' if Overwrite = false, copy the file only if it does not exist
                    ' this is done to avoid an IOException if a file already exists
                    ' this way the other files can be copied anyway...
                    If Not File.Exists(Path.Combine(DestDir.FullName, ChildFile.Name)) Then
                        ChildFile.CopyTo(Path.Combine(DestDir.FullName, ChildFile.Name), False)
                    End If
                End If
            Next

            ' copy all the sub-directories by recursively calling this same routine
            Dim SubDir As DirectoryInfo
            For Each SubDir In SourceDir.GetDirectories()
                CopyFolder(SubDir.FullName, Path.Combine(DestDir.FullName, _
                    SubDir.Name), Overwrite)
            Next
        Else
            Throw New DirectoryNotFoundException("Source directory does not exist: " + SourceDir.FullName)
        End If
    End Sub

    Private Function CorrectSpacing(ByVal v_sTempLine As String) As String

        Dim sOtherTextBefore As String
        Dim sOtherTextAfter As String
        Dim sTag As String
        Dim lCursor As Long
        Dim lOldCursor As Long
        Dim sNewTag As String

        If v_sTempLine.IndexOf("<P") + 1 > 0 Then

            'Split Line Into sOtherTextBefore, sTag and sOtherTextAfter
            lOldCursor = 0
            sOtherTextBefore = ""
            Try
                lCursor = v_sTempLine.IndexOf("<P") + 1
                If lCursor > 0 Then
                    lCursor = lCursor + 1
                    sOtherTextBefore = Mid(v_sTempLine, lOldCursor + 1, lCursor)
                Else
                    sOtherTextBefore = ""
                End If
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try

            lOldCursor = lOldCursor + lCursor
            sTag = ""
            Try
                lCursor = Mid(v_sTempLine, lOldCursor).IndexOf(">") + 1
                sTag = Mid(v_sTempLine, lOldCursor + 1, lCursor - 2)
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try

            lOldCursor = lOldCursor + lCursor
            sOtherTextAfter = ""
            Try
                If v_sTempLine.Length > lOldCursor Then
                    sOtherTextAfter = Mid(v_sTempLine, lOldCursor - 1)
                Else
                    sOtherTextAfter = ""
                End If
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try

            'Correct sTag
            sNewTag = ""
            Try
                If sTag.IndexOf(" style='") + 1 = 0 Then
                    sNewTag = sTag & " style='margin-top:0pt;margin-bottom:0pt'"
                Else
                    Dim sStyleBefore As String
                    Dim sStyleTag As String
                    Dim sStyleAfter As String
                    Dim sNewStyleTag As String

                    'Get Style Sections
                    lOldCursor = 0
                    sStyleBefore = ""
                    Try
                        lCursor = sTag.IndexOf(" style='") + 8
                        If lCursor > 0 Then
                            sStyleBefore = Mid(sTag, lOldCursor + 1, lCursor)
                        Else
                            sStyleBefore = ""
                        End If
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try

                    lOldCursor = lOldCursor + lCursor + 1
                    sStyleTag = ""
                    Try
                        lCursor = Mid(sTag, lOldCursor).IndexOf("'") + 1
                        sStyleTag = Mid(sTag, lOldCursor, lCursor - 1)
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try

                    lOldCursor = lOldCursor + lCursor
                    sStyleAfter = ""
                    Try
                        If v_sTempLine.Length > lOldCursor Then
                            sStyleAfter = Mid(sTag, lOldCursor - 1)
                        Else
                            sStyleAfter = ""
                        End If
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try

                    sNewStyleTag = sStyleTag

                    'Check if margin-top exists
                    If sStyleTag.IndexOf("margin-top") + 1 = 0 Then
                        sNewStyleTag = sNewStyleTag & ";margin-top:0pt"
                    End If

                    'check if margin-bottom exists
                    If sStyleTag.IndexOf("margin-bottom") + 1 = 0 Then
                        sNewStyleTag = sNewStyleTag & ";margin-bottom:0pt"
                    End If

                    sNewTag = sStyleBefore & sNewStyleTag & sStyleAfter

                End If
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try

            CorrectSpacing = sOtherTextBefore & sNewTag & sOtherTextAfter
        Else
            CorrectSpacing = v_sTempLine
        End If

    End Function

    Private Sub AppendHtmlToDocBuild(ByRef oAsposeDocBuilder As Aspose.Words.DocumentBuilder, ByVal sheaderHtml As String)

        Dim sHtml As String()
        Dim lCount As Long

        sHtml = sheaderHtml.Split(vbCrLf)

        For lCount = LBound(sHtml) To UBound(sHtml)
            If sHtml(lCount).Contains("<span style='mso-field-code:"" PAGE ""'>") Or _
               sHtml(lCount).Contains("<span style='mso-field-code:"" NUMPAGES ""'>") Then

                'remove elements <span style='mso-no-proof:yes'>65</span>
                'oAsposeDocBuilder.Font.Size = 5
                'oAsposeDocBuilder.Write("Page ")
                'oAsposeDocBuilder.InsertField("PAGE", "")
                'oAsposeDocBuilder.Write(" of ")
                'oAsposeDocBuilder.InsertField("NUMPAGES", "")


                oAsposeDocBuilder.InsertHtml(sHtml(lCount))

                Debug.Print(sHtml(lCount))

            Else
                oAsposeDocBuilder.InsertHtml(sHtml(lCount))
            End If
        Next
    End Sub

    Public Function ClearDocumentTitlePropertyIfMergeCode(ByVal sFile As String) As Integer

        Dim sDocumentTitle As String

        Try

            Dim oAsposeWordsLic As New Aspose.Words.License
            oAsposeWordsLic.SetLicense("Aspose.Totalfor.NET.lic")
            Dim oAsposeWords As New Aspose.Words.Document(sFile)
            oAsposeWordsLic.SetLicense("Aspose.Totalfor.NET.lic")

            sDocumentTitle = oAsposeWords.BuiltInDocumentProperties("Title").Value
            If sDocumentTitle.IndexOf("&lt;@") + 1 > 0 Then
                oAsposeWords.BuiltInDocumentProperties("Title").Value = ""
            ElseIf sDocumentTitle.IndexOf("<@") + 1 > 0 Then
                oAsposeWords.BuiltInDocumentProperties("Title").Value = ""
            End If

            oAsposeWords.Save(sFile)

            Return PMConst.PMTrue
        Catch ex As Exception
            Throw New Exception("Failed to Convert File " & ex.Message)
        End Try
    End Function
    Private Function CreateObject(ByVal ProgId As String, ByVal Optional ServerName As String = "") As Object
        If ProgId.Length = 0 Then
            Return Nothing
        End If

        If ServerName Is Nothing OrElse ServerName.Length = 0 Then
            ServerName = Nothing
        ElseIf String.Compare(Environment.MachineName, ServerName, StringComparison.OrdinalIgnoreCase) = 0 Then
            ServerName = Nothing
        End If

        Try
            Dim type As Type = (If((ServerName IsNot Nothing), Type.GetTypeFromProgID(ProgId, ServerName, throwOnError:=True), Type.GetTypeFromProgID(ProgId)))
            Return Activator.CreateInstance(type)
        Catch ex As COMException

            If ex.ErrorCode = -2147023174 Then
                Return Nothing
            End If

            Throw ex
        Catch ex2 As StackOverflowException
            Throw ex2
        Catch ex3 As OutOfMemoryException
            Throw ex3
        Catch ex4 As ThreadAbortException
            Throw ex4
        Catch ex5 As Exception
            Throw ex5
        End Try
    End Function
    Public Function MergeWatermark( _
       ByVal v_sBaseFilePath As String, _
       ByVal v_sWaterMarkFilePath As String, _
       ByVal v_lFirstPage As Long, _
       ByVal v_lPageCount As Long, _
       ByRef v_sMergedFilePath As String, _
       ByRef r_sMessage As String) As Integer

        'Watermarking Using QuickPdf

        MergeWatermark = PMTrue
        Dim QP As Object = Nothing
        Try
            r_sMessage = ""

            Dim iReturn As Integer = PMTrue
            Dim lBaseFileId As Long
            Dim lWatermarkFileId As Long
            Dim lTotalPage As Long
            Dim lCapturedPageID As Long
            Dim iLoop1 As Integer
            Dim lPageHeight As Long
            Dim lPageWidth As Long

            Try
                QP = CreateObject("QuickPDFAX0718.PDFLibrary")
            Catch ex As Exception
                r_sMessage = "Failed to get instance of QuickPdf"
                MergeWatermark = PMError
                Exit Function
            End Try

            'unLocks the library for further use
            iReturn = QP.UnlockKey("jp6ax3959um6zj3ca3xr9eb3y")
            If iReturn <> PMTrue Then
                r_sMessage = "Failed to set QuickPdf License"
                MergeWatermark = PMError
                Exit Function
            End If

            'Loads a file from the specified path
            iReturn = QP.LoadFromFile(v_sBaseFilePath.ToString)
            If iReturn <> PMTrue Then
                r_sMessage = "Failed to open Pdf file " & v_sBaseFilePath
                MergeWatermark = PMError
                Exit Function
            End If

            'Returns the Id of the selected document
            lBaseFileId = QP.SelectedDocument

            'Decrypts the file
            QP.Decrypt()

            'Loads a file from the specified path
            iReturn = QP.LoadFromFile(v_sWaterMarkFilePath.ToString)
            If iReturn <> PMTrue Then
                r_sMessage = "Failed to open Pdf file " & v_sWaterMarkFilePath
                MergeWatermark = PMError
                Exit Function
            End If

            'Returns the Id of the selected document
            lWatermarkFileId = QP.SelectedDocument

            'selects a document
            QP.SelectDocument(CInt(lBaseFileId))

            'Joins the another document with the selected document.
            QP.MergeDocument(CInt(lWatermarkFileId))

            lTotalPage = QP.PageCount

            lCapturedPageID = QP.CapturePage(CLng(lTotalPage))
            QP.SelectDocument(CLng(lBaseFileId))

            If v_lPageCount > 0 Then
                'When PageCount is Specified and Total pages is less  than 
                'Specied page count then Use total page
                If lTotalPage > v_lFirstPage + v_lPageCount Then
                    lTotalPage = v_lFirstPage + v_lPageCount
                End If
            End If

            For iLoop1 = v_lFirstPage To lTotalPage - 1
                QP.SelectPage(CInt(iLoop1))
                lPageHeight = QP.PageHeight()
                lPageWidth = QP.PageWidth()

                ' Draw the captured page onto the currently selected page
                QP.DrawCapturedPage(CInt(lCapturedPageID), 0, CInt(lPageHeight), CInt(lPageWidth), CInt(lPageHeight))

            Next

            ' Save the stitched file to disk
            iReturn = QP.SaveToFile(v_sMergedFilePath.ToString)

            If iReturn <> PMTrue Then
                r_sMessage = "Failed to merge file"
                MergeWatermark = PMError
                Exit Function
            End If

        Catch ex As Exception

        Finally
            If Not QP Is Nothing Then
                QP = Nothing
            End If
        End Try

    End Function

    ''' <summary>
    ''' Call CCM componenet to convert to PDF
    ''' </summary>
    ''' <param name="sSourceFile"></param>
    ''' <param name="sDestinationFile"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConvertWordToPDF(ByVal sSourceFile As String, ByVal sDestinationFile As String) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        Dim oCCMDocumentProdBusiness As bCCMDocumentProduction.Business = New bCCMDocumentProduction.Business
        nResult = oCCMDocumentProdBusiness.Initialise("", "", 1, 1, 1, 26, 1, ACApp)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        nResult = oCCMDocumentProdBusiness.ConvertWordToPDFV1(sSourceFile, sDestinationFile)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If
        oCCMDocumentProdBusiness.Dispose()
        oCCMDocumentProdBusiness = Nothing
        Return nResult

    End Function

    ''' <summary>
    ''' Get system option for doc production
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ISCCMEnabled() As Boolean
        Dim nResult As Integer
        'Get system option CCMDocProduction
        Dim sCCMDocProduction As String = String.Empty
        nResult = bPMFunc.GetSystemOption(v_sUsername:="", v_sPassword:="", v_iUserID:=1, v_iMainSourceID:=1, v_iLanguageID:=1, _
                                         v_iCurrencyID:=26, v_iLogLevel:=1, v_sCallingAppName:=ACApp, v_iOptionNumber:=GeneralConst.kSystemOptionDocumentProductionSystem, _
                                         r_sOptionValue:=sCCMDocProduction)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            If sCCMDocProduction = "1" Then
                Return True
            Else
                Return False
            End If
        End If

    End Function

    ''' <summary>
    ''' Get system option for If KCM is applicable on seleced documents (5207)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsKCMForSelectedDocument() As Boolean
        Dim nResult As Integer
        'Get system option IsKCMApplicableForSelectedDocument
        Dim isKCMForSelectedDocs As String = String.Empty
        nResult = bPMFunc.GetSystemOption(v_sUsername:="", v_sPassword:="", v_iUserID:=1, v_iMainSourceID:=1, v_iLanguageID:=1,
                                         v_iCurrencyID:=26, v_iLogLevel:=1, v_sCallingAppName:=ACApp, v_iOptionNumber:=GeneralConst.KSystemOptionKCMForSelectedTemplate,
                                         r_sOptionValue:=isKCMForSelectedDocs)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            If isKCMForSelectedDocs = "1" Then
                Return True
            Else
                Return False
            End If
        End If

    End Function
#End Region

End Class

