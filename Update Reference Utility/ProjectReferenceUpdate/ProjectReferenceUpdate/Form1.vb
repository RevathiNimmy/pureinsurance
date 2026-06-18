Imports System.IO

Public Class Form1

    Private Sub btnUpdateReference_Click(sender As Object, e As EventArgs) Handles btnUpdateReference.Click
        Dim vbproj As XDocument = Nothing
        Dim sDllName As String = ""
        Dim vbprojPath As String = txtFolderPath.Text

        ' Load the .vbproj XML file
        Try
            vbproj = XDocument.Load(vbprojPath)
        Catch ex As Exception
            MsgBox("Please select a valid file ")
            Exit Sub
        End Try


        Dim projectsName = GetAllProjectsName(vbproj)

        Dim projectToDllMapping As New Dictionary(Of String, String)

        If projectsName.Any() Then
            For Each projectName In projectsName
                sDllName = projectName
                If projectName = "Sirius.Document.Utility" Then
                    sDllName = "SiriusDocumentUtility"
                ElseIf projectName = "Data" Then
                    sDllName = "Sirius.Architecture.Data"
                ElseIf projectName = "Utility" Then
                    sDllName = "Sirius.Architecture.Utility"
                ElseIf projectName = "Configuration.Local" Then
                    sDllName = "Sirius.Architecture.Configuration.Local"
                ElseIf projectName = "Data.BackOffice" Then
                    sDllName = "Sirius.Architecture.Data.BackOffice"
                End If
                projectToDllMapping.Add(projectName, "C:\Pure\Application\" & sDllName & ".dll")
            Next
        Else
            MsgBox("No project references found.")
            Exit Sub
        End If

        ' Replace all project references with DLL references
        ReplaceMultipleProjectReferencesWithDll(vbproj, projectToDllMapping)

        ' Save the updated .vbproj file
        vbproj.Save(vbprojPath)
        MsgBox("All project references updated to DLL references successfully.")
    End Sub

    Function GetAllProjectsName(vbproj As XDocument) As List(Of String)
        ' List to hold all project names
        Dim projectNameList As New List(Of String)

        ' Find all <ProjectReference> elements in the .vbproj file
        Dim projectReferences = vbproj.Descendants().Where(Function(e) e.Name.LocalName = "ProjectReference")

        ' Loop through each <ProjectReference> element
        For Each reference In projectReferences
            ' Get the "Include" attribute, which contains the path to the referenced project
            Dim includeAttr = reference.Attribute("Include")
            If includeAttr IsNot Nothing Then
                ' Extract the project name (filename without path and extension)
                Dim projectPath As String = includeAttr.Value
                Dim projectName As String = Path.GetFileNameWithoutExtension(projectPath)

                ' Add the project name to the list
                projectNameList.Add(projectName)
            End If
        Next

        ' Return the list of project names
        Return projectNameList
    End Function

    ' Function to replace multiple project references with DLL references
    Sub ReplaceMultipleProjectReferencesWithDll(vbproj As XDocument, projectToDllMapping As Dictionary(Of String, String))
        ' Find all project references in the .vbproj file
        Dim projectReferences = vbproj.Descendants().Where(Function(e) e.Name.LocalName = "ProjectReference").ToList()

        ' Iterate through all project references
        For Each projectReference In projectReferences
            Dim includeAttr = projectReference.Attribute("Include")
            If includeAttr IsNot Nothing Then
                Dim projectNameOrPath = includeAttr.Value

                ' Check if the project reference matches any in the dictionary
                For Each kvp In projectToDllMapping
                    Dim projectName = kvp.Key
                    Dim dllPath = kvp.Value

                    ' If the project reference contains the project name (or partial path), replace it
                    If projectNameOrPath.Contains(projectName) Then
                        ' Remove the project reference
                        projectReference.Remove()

                        ' Create a new DLL reference element
                        Dim dllReference As New XElement("Reference",
                                                         New XAttribute("Include", System.IO.Path.GetFileNameWithoutExtension(dllPath)),
                                                         New XElement("HintPath", dllPath))

                        ' Find the first <ItemGroup> to insert the new DLL reference
                        Dim itemGroup = vbproj.Descendants().Where(Function(e) e.Name.LocalName = "ItemGroup").FirstOrDefault()

                        ' Add the DLL reference to the <ItemGroup>
                        If itemGroup IsNot Nothing Then
                            itemGroup.Add(dllReference)

                        Else
                            ' If no <ItemGroup> exists, create one and add the reference
                            vbproj.Root.Add(New XElement("ItemGroup", dllReference))

                        End If

                        ' Exit the loop once the reference is replaced
                        Exit For
                    End If
                Next
            End If
        Next
    End Sub

End Class
