Imports System.Activities
Imports Microsoft.TeamFoundation.Build.Client
Imports System.IO
Imports System.Text
Imports System.Linq

<BuildActivity(HostEnvironmentOption.All)>
Public NotInheritable Class AssemblyVersionInfo
    Inherits CodeActivity

    <RequiredArgument()> _
    Public Property FileName As InArgument(Of String)

    <RequiredArgument()> _
    Public Property TfsBuildNumber As InArgument(Of String)

    Public Property NewAssemblyFileVersion As OutArgument(Of String)

    Protected Overrides Sub Execute(ByVal context As CodeActivityContext)
        Dim solutionVersionFile As String = Me.FileName.Get(context)

        'Ensure File is Writable
        Dim fileAttributes = File.GetAttributes(solutionVersionFile)
        File.SetAttributes(solutionVersionFile, fileAttributes - FileAttribute.ReadOnly)

        Dim majorMinor = GetAssemblyMajorMinorVersionBasedOnExisting(solutionVersionFile)
        Dim newBuildNumber = GetNewBuildNumber(Me.TfsBuildNumber.Get(context))
        Dim newAssemblyVersion = String.Format("{0}.{1}.0.0", majorMinor.Item1, majorMinor.Item2)
        Dim newAssemblyFileVersion = String.Format("{0}.{1}.{2}.0", majorMinor.Item1, majorMinor.Item2, newBuildNumber)
        Me.NewAssemblyFileVersion.Set(context, newAssemblyFileVersion)

        'Normal File
        Dim contents = Me.GetFileContents(newAssemblyVersion, newAssemblyFileVersion, False)
        File.WriteAllText(solutionVersionFile, contents)
        File.SetAttributes(solutionVersionFile, fileAttributes)

        'Com Visible File
        Dim comVisibleVersionFile As String = Path.Combine(Path.GetDirectoryName(solutionVersionFile), _
                                             "ComVisible" & Path.GetFileName(solutionVersionFile))

        'Only if exists, not in all branches yet
        If File.Exists(comVisibleVersionFile) Then
            fileAttributes = File.GetAttributes(comVisibleVersionFile)
            File.SetAttributes(comVisibleVersionFile, fileAttributes - FileAttribute.ReadOnly)

            contents = Me.GetFileContents(newAssemblyVersion, newAssemblyFileVersion, True)
            File.WriteAllText(comVisibleVersionFile, contents)

            File.SetAttributes(comVisibleVersionFile, fileAttributes)
        End If
    End Sub

    Private Function GetFileContents(ByVal newAssemblyVersion As String, ByVal newAssemblyFileVersion As String, ByVal comVisible As Boolean) As String
        Dim cs As New StringBuilder()

        cs.AppendLine("Imports System")
        cs.AppendLine("Imports System.Reflection")
        cs.AppendLine("Imports System.Runtime.InteropServices")
        cs.AppendLine("")
        cs.AppendLine("<Assembly: AssemblyTitle("""")> ")
        cs.AppendLine("<Assembly: AssemblyDescription("""")> ")
        cs.AppendLine("<Assembly: AssemblyCompany(""SSP"")> ")
        cs.AppendLine("<Assembly: AssemblyProduct(""SSP Pure Insurance"")> ")
        cs.AppendFormat("<Assembly: AssemblyCopyright(""Copyright SSP © {0}"")>{1}", DateTime.Now.Year, Environment.NewLine)
        cs.AppendLine("<Assembly: AssemblyTrademark("""")> ")
        cs.AppendFormat("<Assembly: AssemblyVersion(""{0}"")>{1}", newAssemblyVersion, Environment.NewLine)
        cs.AppendFormat("<Assembly: AssemblyFileVersion(""{0}"")>{1}", newAssemblyFileVersion, Environment.NewLine)

        If comVisible Then
            cs.AppendLine("<Assembly: ComVisible(True)>")
            cs.AppendLine("<Assembly: Guid(""8ce52483-7e64-4da5-b928-03c1abf41a09"")>")
        End If

        Return cs.ToString()
    End Function

    Private Function GetAssemblyMajorMinorVersionBasedOnExisting(ByVal filePath As String) As Tuple(Of String, String)
        Dim lines = File.ReadAllLines(filePath)
        Dim versionLine As String = ""
        For Each line In lines
            If line.Contains("AssemblyVersion") Then
                versionLine = line
                Exit For
            End If
        Next
        Return ExtractMajorMinor(versionLine)
    End Function

    Private Function ExtractMajorMinor(ByVal versionLine As String) As Tuple(Of String, String)
        Dim firstQuote = versionLine.IndexOf("""") + 1
        Dim secondQuote = versionLine.IndexOf("""", firstQuote)
        Dim version = versionLine.Substring(firstQuote, secondQuote - firstQuote)
        Dim versionParts = version.Split(".")
        Return New Tuple(Of String, String)(versionParts(0), versionParts(1))
    End Function

    Private Function GetNewBuildNumber(ByVal buildName As String) As String
        Return buildName.Substring(buildName.LastIndexOf(".") + 1)
    End Function

End Class
