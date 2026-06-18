Imports System
Imports System.Globalization
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text


Public Module SecureHash
    ''' <summary>
    ''' Compute SHA256 hash of passed string.
    ''' </summary>
    ''' <seecref="http"/>
    ''' <seecref="http"/>
    ''' <seecref="https"/>
    ''' <seecref="https"/>
    ''' <remarks>
    ''' SHA256 provides stronger security than SHA1.
    ''' </remarks>
    ''' <example>
    ''' string inputString = "Hello, World!";
    ''' string sha256Hash = ComputeSHA256Hash(inputString);
    ''' </example>
    ''' <paramname="input">String to be hashed</param>
    ''' <returns>String of hex bytes representing the SHA256 hash</returns>
    Public Function ComputeSHA256Hash(input As String) As String
        '	ArgumentException.ThrowIfNullOrWhiteSpace(input);

        ' Convert the string into bytes
        Dim inputBytes = Encoding.UTF8.GetBytes(input)

        ' Hash the bytes
        Dim hashBytes As Byte() = New SHA256Managed().ComputeHash(inputBytes) '''SHA256.HashData(inputBytes);

        Return JoinBytes(hashBytes)
    End Function

    ''' <summary>
    ''' Compute the SHA256 hash of the passed file.
    ''' </summary>
    ''' <example>
    ''' string sha256Hash = ComputeSHA256HashFile(@"C:\Path\To\File.txt");
    ''' </example>
    ''' <paramname="pathFile">Path to the file to be hashed</param>
    ''' <returns>String of hex bytes representing the SHA256 hash</returns>
    Public Function ComputeSHA256HashFile(pathFile As String) As String
        'ArgumentException.ThrowIfNullOrWhiteSpace(pathFile);

        Dim fileInfo As FileInfo = New FileInfo(pathFile)


        Dim fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read)
        Try
            ' Create a fileStream for the file.
            ' Be sure it's positioned to the beginning of the stream.
            fileStream.Position = 0

            ' Hash the file stream
            Dim hashBytes As Byte() = New SHA256Managed().ComputeHash(fileStream) '''

            Return JoinBytes(hashBytes)
        Catch __unusedIOException1__ As IOException
            Return String.Empty
        Catch __unusedUnauthorizedAccessException2__ As UnauthorizedAccessException
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Join bytes into a string.
    ''' </summary>
    ''' <example>
    ''' [ 0x12, 0x34, 0x56 ] => "123456"
    ''' </example>
    ''' <paramname="inBytes">Array of bytes to join</param>
    ''' <returns>String consisting of appended bytes</returns>
    Public Function JoinBytes(inBytes As Byte()) As String
        Dim stringBuilder As StringBuilder = New StringBuilder()
        For Each b In inBytes
            stringBuilder.Append(b.ToString("x2", CultureInfo.InvariantCulture))
        Next
        Return stringBuilder.ToString()
    End Function
End Module
