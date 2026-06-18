Imports System.Web
Imports System.Web.Configuration
Imports System.Web.HttpContext
Imports Nexus.Library.Config
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Diagnostics
Imports System.Text

Public Class ErrorFormatter

    ''' <summary>
    ''' Takes an exception and outputs a text string of useful information for logging
    ''' </summary>
    ''' <param name="oException"></param>
    ''' <returns>A string</returns>
    ''' <remarks></remarks>
    Public Shared Function FormatErrorAsText(ByVal oException As Exception)

        Dim sbTextErrorMessage As New StringBuilder()
        With sbTextErrorMessage

            oException = IIf(oException.InnerException IsNot Nothing, oException.InnerException, oException)

            Dim oAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
            Dim oVersion As System.Version = oAssembly.GetName().Version()

            .AppendLine("Version: " & oVersion.Major & "." & oVersion.Minor & " b" & oVersion.Build & "." & oVersion.Revision & vbLf)

            .AppendLine("Path: " & Current.Request.Path & vbCrLf)

            If Current.Session Is Nothing Then
                .Append("SessionID: Nothing" & vbCrLf)
            Else
                .Append("SessionID: " & Current.Session.SessionID & vbCrLf)
            End If

            .AppendLine("Message: " & oException.Message & vbCrLf)
            .AppendLine("Source: " & oException.Source & vbCrLf)
            .AppendLine("Stack Trace: " & oException.StackTrace & vbCrLf)
            .AppendLine("Data: " & vbCrLf)

            Dim oDictionary As IDictionary = oException.Data
            For Each sDataKey As String In oDictionary.Keys
                .AppendLine(sDataKey & " : " & oException.Data.Item(sDataKey).ToString() & vbCrLf)
            Next
            oDictionary = Nothing

            Select Case True
                Case TypeOf oException Is NexusProvider.NexusException
                    .AppendLine("SAM Errors: " & vbCrLf)

                    For Each oError As NexusProvider.NexusError In CType(oException, NexusProvider.NexusException).Errors

                        .AppendLine(oError.NexusCode.ToString() & vbCr)
                        .AppendLine("Code : " & oError.Code & vbLf)
                        .AppendLine("Description : " & oError.Description & vbCr)
                        .AppendLine("Detail : " & oError.Detail & vbCr)

                    Next
                Case Else
            End Select

            .AppendLine("Session: " & vbCrLf & GetSessionAsText())

        End With

        Return sbTextErrorMessage.ToString

    End Function


    Public Shared Function FormatErrorAsHtml(ByVal oException As Exception) As String
        Return FormatErrorAsHtml(oException, "")
    End Function

    ''' <summary>
    ''' Takes an exception and outputs HTML as a string of useful information for logging
    ''' </summary>
    ''' <param name="oException"></param>
    ''' <returns>A string</returns>
    ''' <remarks></remarks>
    Public Shared Function FormatErrorAsHtml(ByVal oException As Exception, ByVal sSAMStackTrace As String) As String
        Dim sbHtmlErrorMessage As New StringBuilder()
        With sbHtmlErrorMessage

            oException = IIf(oException.InnerException IsNot Nothing, oException.InnerException, oException)

            .AppendLine("<table>" & vbCr)

            Dim oAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
            Dim oVersion As System.Version = oAssembly.GetName().Version()

            .AppendLine("<tr>" & vbCr)
            .AppendLine("<th colspan='2'>Debug Information</th>" & vbCr)
            '.AppendLine("<td class='info'>" & oVersion.Major & "." & oVersion.Minor & " b" & oVersion.Build & "." & oVersion.Revision & "</td>" & vbCr)
            .AppendLine("</tr>" & vbCr)

            .AppendLine("<tr>" & vbCr)
            .AppendLine("<td>Version</td>" & vbCr)
            .AppendLine("<td class='info'>" & oVersion.Major & "." & oVersion.Minor & " b" & oVersion.Build & "." & oVersion.Revision & "</td>" & vbCr)
            .AppendLine("</tr>" & vbCr)

            .AppendLine("<tr>" & vbCr)
            .AppendLine("<td></td>" & vbCr)
            .AppendLine("<td class='info'>" & Current.Server.MachineName & " : " & Now & "</td>" & vbCr)
            .AppendLine("</tr>" & vbCr)

            .AppendLine("<tr>" & vbCr)
            .AppendLine("<td>Path</td>" & vbCr)
            .AppendLine("<td class='info'>" & Current.Request.Path & "</td>" & vbCr)
            .AppendLine("</tr>" & vbCr)

            .AppendLine("<tr>" & vbCr)
            .AppendLine("<td>SessionID</td>" & vbCr)
            .AppendLine("<td class='info'>")

            If Current.Session Is Nothing Then
                .Append("<i>Nothing</i>")
            Else
                .Append(Current.Session.SessionID)
            End If

            .Append("</td>" & vbCr)
            .AppendLine("</tr>" & vbCr)

            .AppendLine("<tr>" & vbCr)
            .AppendLine("<td>Message</td>" & vbCr)
            .AppendLine("<td class='info'>" & oException.Message & "</td>" & vbCr)
            .AppendLine("</tr>" & vbCr)

            .AppendLine("<tr>" & vbCr)
            .AppendLine("<td>Source</td>" & vbCr)
            .AppendLine("<td class='info'>" & oException.Source & "</td>" & vbCr)
            .AppendLine("</tr>" & vbCr)

            If Not oException.StackTrace Is Nothing Then
                .AppendLine("<tr>" & vbCr)
                .AppendLine("<td>Stack Trace</td>" & vbCr)
                .AppendLine("<td class='info'>" & oException.StackTrace.Replace(vbCr, "<br />").Replace(vbLf, "<br />").Replace(vbTab, "&tab;") & "</td>" & vbCr).Replace("&", "&amp;")
                .AppendLine("</tr>" & vbCr)
            End If

            .AppendLine("<tr>" & vbCr)
            .AppendLine("<td>Data</td>" & vbCr)
            .AppendLine("<td class='info'>")

            Dim oDictionary As IDictionary = oException.Data
            For Each sDataKey As String In oDictionary.Keys
                .AppendLine(sDataKey & " : " & oException.Data.Item(sDataKey).ToString() & "<br />")
            Next
            oDictionary = Nothing

            .AppendLine("</td>" & vbCr)
            .AppendLine("</tr>" & vbCr)

            Select Case True
                Case TypeOf oException Is NexusProvider.NexusException
                    .AppendLine("<tr>" & vbCr)
                    .AppendLine("<td>SAM Errors</td>" & vbCr)
                    .AppendLine("<td class='info'>")
                    .AppendLine("<table cellspacing='2' cellpadding='5' border='0' class='session'>" & vbCr)

                    For Each oError As NexusProvider.NexusError In CType(oException, NexusProvider.NexusException).Errors

                        .AppendLine("<tr>" & vbCr)
                        .AppendLine("<td>" & oError.NexusCode.ToString() & "</td>" & vbCr)
                        .AppendLine("<td class='info'>")
                        .AppendLine("Code : " & oError.Code & "<br />")
                        .AppendLine("Description : " & oError.Description & "<br />")
                        .AppendLine("Detail : " & oError.Detail & "<br />")
                        .AppendLine("</td>")
                        .AppendLine("</tr>" & vbCr)

                    Next

                    .AppendLine("</table>")
                    .AppendLine("</td>" & vbCr)
                    .AppendLine("</tr>" & vbCr)
                Case Else
            End Select

            .AppendLine("<tr>" & vbCr)
            .AppendLine("<td>Session</td>" & vbCr)
            .AppendLine("<td class='info'>")

            .AppendLine(GetSessionAsHtml)


            .AppendLine("</td>" & vbCr)
            .AppendLine("</tr>" & vbCr)
            .AppendLine("</table>" & vbCr)

        End With

        Return sbHtmlErrorMessage.ToString
    End Function


    ''' <summary>
    ''' Generate Text output of all the current users session values formatted for easy display
    ''' </summary>
    ''' <returns>A string</returns>
    ''' <remarks>This is used to format data to add to log files when an unhandled exception is raised</remarks>
    Public Shared Function GetSessionAsText() As String

        Dim sbSession As New StringBuilder()

        With sbSession

            If Current.Session IsNot Nothing Then

                For Each sKey As String In Current.Session.Keys

                    .AppendLine(sKey & ": ")

                    Select Case True
                        Case TypeOf Current.Session(sKey) Is NexusProvider.PersonalParty
                            .AppendLine(Replace((CType(Current.Session(sKey), NexusProvider.PersonalParty).Print()), "<br />", vbCrLf))

                        Case TypeOf Current.Session(sKey) Is NexusProvider.CorporateParty
                            .AppendLine(Replace(CType(Current.Session(sKey), NexusProvider.CorporateParty).Print(), "<br />", vbCrLf))

                        Case TypeOf Current.Session(sKey) Is NexusProvider.Quote
                            .AppendLine(Replace(CType(Current.Session(sKey), NexusProvider.Quote).Print(), "<br />", vbCrLf))

                        Case TypeOf Current.Session(sKey) Is NexusProvider.UserDetails
                            .AppendLine(Replace(CType(Current.Session(sKey), NexusProvider.UserDetails).Print(), "<br />", vbCrLf))

                        Case TypeOf Current.Session(sKey) Is NexusProvider.PartySearchCriteria
                            .AppendLine(Replace(CType(Current.Session(sKey), NexusProvider.PartySearchCriteria).Print(), "<br />", vbCrLf))

                        Case TypeOf Current.Session(sKey) Is System.Xml.XmlElement
                            .AppendLine(CType(Current.Session(sKey), System.Xml.XmlElement).OuterXml)

                        Case TypeOf Current.Session(sKey) Is Hashtable

                            Dim oHashTable As Hashtable = CType(Current.Session(sKey), Hashtable)

                            If oHashTable.Count > 0 Then
                                For Each sHashKey As String In oHashTable.Keys
                                    .AppendLine(sHashKey & " : ")
                                    If oHashTable.Item(sHashKey) IsNot Nothing Then
                                        .AppendLine(oHashTable.Item(sHashKey).ToString())
                                    End If
                                    .AppendLine(vbCrLf)
                                Next
                            Else
                                .AppendLine("Nothing")
                            End If

                            oHashTable = Nothing

                        Case TypeOf Current.Session(sKey) Is Collections.Stack

                            Dim oStack As Collections.Stack = CType(Current.Session(sKey), Collections.Stack)

                            If oStack.Count > 0 Then
                                Dim i As IEnumerator = oStack.GetEnumerator()
                                While i.MoveNext
                                    .AppendLine(i.Current.ToString() & vbCrLf)
                                End While
                            Else
                                .AppendLine("Nothing")
                            End If

                        Case Else
                            If Current.Session(sKey) IsNot Nothing Then
                                .AppendLine(Current.Session(sKey).ToString())
                            End If
                    End Select

                    .AppendLine(vbCr)

                Next

            End If

        End With

        Return sbSession.ToString()

    End Function

    ''' <summary>
    ''' Generate HTML table of all the current users session values
    ''' </summary>
    ''' <returns>A string of HTML</returns>
    ''' <remarks>This is used as part of the debug information display when an exception is raised</remarks>
    Public Shared Function GetSessionAsHtml() As String

        Dim sbSession As New StringBuilder()

        With sbSession

            .AppendLine("<table cellspacing='2' cellpadding='5' border='0' class='session'>" & vbCr)

            If Current.Session IsNot Nothing Then

                For Each sKey As String In Current.Session.Keys

                    .AppendLine("<tr>" & vbCr)
                    .AppendLine("<td>" & sKey & "</td>" & vbCr)
                    .AppendLine("<td class='info'>")

                    Select Case True
                        Case TypeOf Current.Session(sKey) Is NexusProvider.PersonalParty
                            .AppendLine(CType(Current.Session(sKey), NexusProvider.PersonalParty).Print())

                        Case TypeOf Current.Session(sKey) Is NexusProvider.CorporateParty
                            .AppendLine(CType(Current.Session(sKey), NexusProvider.CorporateParty).Print())

                        Case TypeOf Current.Session(sKey) Is NexusProvider.Quote
                            .AppendLine(CType(Current.Session(sKey), NexusProvider.Quote).Print())

                        Case TypeOf Current.Session(sKey) Is NexusProvider.UserDetails
                            .AppendLine(CType(Current.Session(sKey), NexusProvider.UserDetails).Print())

                        Case TypeOf Current.Session(sKey) Is NexusProvider.PartySearchCriteria
                            .AppendLine(CType(Current.Session(sKey), NexusProvider.PartySearchCriteria).Print())

                        Case TypeOf Current.Session(sKey) Is System.Xml.XmlElement
                            .AppendLine(NexusProvider.ProviderHelper.PrettyFormatXMLToHTML( _
                                CType(Current.Session(sKey), System.Xml.XmlElement).OuterXml))

                        Case TypeOf Current.Session(sKey) Is Hashtable

                            Dim oHashTable As Hashtable = CType(Current.Session(sKey), Hashtable)

                            If oHashTable.Count > 0 Then
                                For Each sHashKey As String In oHashTable.Keys
                                    .AppendLine(sHashKey & " : ")
                                    If oHashTable.Item(sHashKey) IsNot Nothing Then
                                        .AppendLine(oHashTable.Item(sHashKey).ToString())
                                    End If
                                    .AppendLine("<br />")
                                Next
                            Else
                                .AppendLine("<i>Nothing</i>")
                            End If

                            oHashTable = Nothing

                        Case TypeOf Current.Session(sKey) Is Collections.Stack

                            Dim oStack As Collections.Stack = CType(Current.Session(sKey), Collections.Stack)

                            If oStack.Count > 0 Then
                                Dim i As IEnumerator = oStack.GetEnumerator()
                                While i.MoveNext
                                    .AppendLine(i.Current.ToString() & "<br />")
                                End While
                            Else
                                .AppendLine("<i>Nothing</i>")
                            End If

                        Case Else
                            If Current.Session(sKey) IsNot Nothing Then
                                .AppendLine(Current.Session(sKey).ToString())
                            End If
                    End Select

                    .AppendLine("</td>" & vbCr)
                    .AppendLine("</tr>" & vbCr)

                Next

            End If

            .AppendLine("</table>")

        End With

        Return sbSession.ToString()

    End Function

End Class
