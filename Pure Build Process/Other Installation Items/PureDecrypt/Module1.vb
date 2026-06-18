Imports System.Security.Cryptography
Imports System.Text
Imports System.IO
Imports System.Reflection

Module Module1
    Private logFile As String = Path.Combine(Path.GetTempPath(), "PureDecrypt.log")

    Sub Main(args As String())
        Log("Console mode started")
        Log("Args count: " & args.Length)

        If args.Length < 1 Then
            Console.WriteLine("Usage: PureDecrypt_Console.exe <encryptedtext> [outputFile]")
            Log("No arguments provided. Exiting.")
            Return
        End If

        Try
            Dim encryptedText As String = args(0)
            Log("Encrypted text: " & encryptedText)

            Dim decryptedText As String = DecryptText(encryptedText)

            ' If output file provided, write there, otherwise default
            Dim outputFilePath As String
            If args.Length >= 2 Then
                outputFilePath = Path.GetFullPath(args(1))
            Else
                outputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "decrypted.txt")
            End If

            ' Create directory if it doesn't exist
            Dim outputDirectory As String = Path.GetDirectoryName(outputFilePath)
            If Not Directory.Exists(outputDirectory) Then
                Directory.CreateDirectory(outputDirectory)
                Log("Created directory: " & outputDirectory)
            End If

            File.WriteAllText(outputFilePath, decryptedText, Encoding.UTF8)
            Log("File written: " & outputFilePath)

            ' Print to console too
            Console.WriteLine("Encrypted: " & encryptedText)
            Console.WriteLine("Decrypted: " & decryptedText)
            Console.WriteLine("Saved to: " & outputFilePath)

        Catch ex As Exception
            Log("FATAL ERROR: " & ex.ToString())
            Console.WriteLine("ERROR: " & ex.Message)
        End Try
    End Sub

    Private Function DecryptText(sEncryptedText As String) As String
        Const kScope As DataProtectionScope = DataProtectionScope.LocalMachine

        If String.IsNullOrEmpty(sEncryptedText) Then
            Throw New ArgumentNullException("encryptedText", "Encrypted text cannot be null or empty")
        End If

        Dim bKeys As Byte() = Encoding.ASCII.GetBytes("SiriusArchitecture")

        Try
            ' Convert from Base64 to bytes
            Dim aEncrypted As Byte() = Convert.FromBase64String(sEncryptedText)

            ' Decrypt
            Dim aDecrypted As Byte() = ProtectedData.Unprotect(aEncrypted, bKeys, kScope)

            Log("Decryption completed successfully")

            ' Return Unicode string
            Return Encoding.Unicode.GetString(aDecrypted)

        Catch ex As CryptographicException
            Throw New InvalidOperationException("Failed to decrypt data: " & ex.Message, ex)
        Catch ex As FormatException
            Throw New ArgumentException("Invalid base64 format in encrypted text", ex)
        End Try
    End Function

    Private Sub Log(message As String)
        Try
            Using sw As StreamWriter = New StreamWriter(logFile, True, Encoding.UTF8)
                sw.WriteLine("[" & DateTime.Now.ToString("dd MMM yyyy HH:mm:ss") & "] " & message)
            End Using
        Catch
        End Try
    End Sub
End Module