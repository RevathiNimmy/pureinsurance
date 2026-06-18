Imports System.IO

Public Module FileMethods


    'Public Enum OpenMode

    '    Input = 1
    '    Output = 2
    '    Random = 4
    '    Append = 8
    '    Binary = &H20
    'End Enum

    'Public Enum FileAttribute

    '    Normal = 0
    '    [ReadOnly] = 1
    '    Hidden = 2
    '    System = 4
    '    Volume = 8
    '    Directory = 10
    '    Archive = 20
    'End Enum
    'Public Enum OpenShare
    '    [Default] = -1
    '    LockRead = 2
    '    LockReadWrite = 0
    '    LockWrite = 1
    '    [Shared] = 3
    'End Enum
    'Public Enum OpenAccess
    '    [Default] = -1
    '    Read = 1
    '    ReadWrite = 3
    '    Write = 2
    'End Enum

    'Public Function FreeFile() As Integer
    '    Dim fileNumber As Integer = 1
    '    ' Keep checking until a free file number is found
    '    While File.Exists($"File{fileNumber}.txt")
    '        fileNumber += 1
    '    End While
    '    ' Return the free file number
    '    Return fileNumber
    'End Function

    'Public Function LOF(ByVal filePath As String) As Long
    '    ' Dim filePath As String = "example.txt"
    '    ' Create a FileInfo object for the file
    '    Dim fileInfo As New FileInfo(filePath)
    '    ' Get the length of the file
    '    Dim fileLength As Long = fileInfo.Length
    '    Return fileLength
    'End Function

    'Public Function InputString(ByVal startIndex As Integer, ByVal length As Integer, ByVal sFullPath As String) As String
    '    ' Dim filePath As String
    '    Try
    '        If File.Exists(sFullPath) Then
    '            Using reader As StreamReader = New StreamReader(sFullPath)
    '                reader.BaseStream.Seek(startIndex, SeekOrigin.Begin)
    '                Dim buffer As Char() = New Char(length - 1) {}
    '                Dim bytesRead As Integer = reader.Read(buffer, 0, length)
    '                Return New String(buffer, 0, bytesRead)
    '            End Using
    '        Else
    '            Throw New FileNotFoundException("File not found", sFullPath)
    '        End If

    '    Catch ex As Exception
    '        Console.WriteLine("An error occurred: " & ex.Message)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Sub FileClose(ParamArray fileNumbers() As Integer)
    '    Try
    '        For Each fileNumber As Integer In fileNumbers
    '            If fileNumber > 0 Then
    '                ' Close the file handle
    '                FileClose(fileNumber)
    '            Else
    '                Throw New ArgumentException("Invalid file number")
    '            End If
    '        Next
    '    Catch ex As Exception
    '        ' Handle any exceptions
    '        Console.WriteLine("An error occurred: " & ex.Message)
    '        ' You can also rethrow the exception if you prefer
    '        ' Throw
    '    End Try
    'End Sub
    'Public Sub ChDrive(drive As String)

    '    If (drive = " " AndAlso drive.Length <> 0) Then

    '        ChDrive(drive.IndexOf(0))

    '    End If
    'End Sub
    'Public Function PrintLine(ByVal filePath As String, ByVal line As String)

    '    Using writer As New StreamWriter(filePath, True)
    '        writer.WriteLine(line)
    '    End Using


    'End Function

    'Public Function LineInput(ByRef m_sPath As String) As String
    '    Dim line As String = ""
    '    ' Dim fName As String = Path.GetFileName(m_sPath)
    '    Using reader As New StreamReader(m_sPath)
    '        ' Use True for buffering
    '        line = reader.ReadLine()
    '    End Using
    '    Return line
    'End Function
    'Public Function EOF(m_sPath As String) As Boolean

    '    Dim currentLineNumber As Integer = 0

    '    Using reader As New StreamReader(m_sPath)
    '        Dim line As String = reader.ReadLine()
    '        While line IsNot Nothing
    '            currentLineNumber += 1
    '            line = reader.ReadLine()
    '        End While
    '    End Using

    '    Return True
    'End Function
    'Public Sub CopyDirectory(sourceDir As String, destinationDir As String, recursive As Boolean)

    '    ' Get information about the source directory
    '    Dim dir As New DirectoryInfo(sourceDir)

    '    ' Check if the source directory exists
    '    If Not dir.Exists Then
    '        Throw New DirectoryNotFoundException($"Source directory not found: {dir.FullName}")
    '    End If

    '    ' Cache directories before we start copying
    '    Dim dirs As DirectoryInfo() = dir.GetDirectories()

    '    ' Create the destination directory
    '    Directory.CreateDirectory(destinationDir)

    '    ' Get the files in the source directory and copy to the destination directory
    '    For Each file As FileInfo In dir.GetFiles()
    '        Dim targetFilePath As String = Path.Combine(destinationDir, file.Name)
    '        file.CopyTo(targetFilePath)
    '    Next

    '    ' If recursive and copying subdirectories, recursively call this method
    '    If recursive Then
    '        For Each subDir As DirectoryInfo In dirs
    '            Dim newDestinationDir As String = Path.Combine(destinationDir, subDir.Name)
    '            CopyDirectory(subDir.FullName, newDestinationDir, True)
    '        Next
    '    End If

    'End Sub

    'Public Function GetDirectory(ByVal directoryPath As String) As String

    '    Dim directoryInfo As New DirectoryInfo(directoryPath)

    '    If Directory.Exists(directoryPath) Then
    '        Dim firstDirectory As DirectoryInfo = directoryInfo.EnumerateDirectories().FirstOrDefault()

    '        If firstDirectory IsNot Nothing Then
    '            ' Return the full path of the first directory
    '            Return firstDirectory.FullName
    '        Else
    '            ' No directory found, return an empty string or handle the case accordingly
    '            Return String.Empty
    '        End If
    '    Else
    '        Return String.Empty
    '    End If
    'End Function

    'Public Function GetFiles()
    '    String targetDirectory = @"C:\YourTargetDirectory"

    '    If (Directory.Exists(targetDirectory)) Then

    '        String[] files = Directory.GetFiles(targetDirectory);

    '        foreach(String file In files)

    '        Console.WriteLine($"Found file: {File}");


    '    Else
    '        Console.WriteLine($"Directory '{targetDirectory}' does not exist.");
    '    End If
    'End Function
End Module


