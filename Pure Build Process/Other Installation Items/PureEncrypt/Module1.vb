Imports System.Security.Cryptography
Imports System.Text
Imports System.IO
Imports System.Reflection

Module Module1
    Private logFile As String = Path.Combine(Path.GetTempPath(), "PureEncrypt.log")

    Sub Main(args As String())
        Log("Console mode started")
        Log("Args count: " & args.Length)

        If args.Length < 1 Then
            Console.WriteLine("Usage: PureEncrypt.exe <plaintext> [outputFile]")
            Log("No arguments provided. Exiting.")
            Return
        End If

        Try
            Dim plainText As String = args(0)
            Log("Plain text length: " & plainText.Length)

            Dim encryptedText As String = EncryptText(plainText)

            ' If output file provided, write there, otherwise default
            Dim outputFilePath As String
            If args.Length >= 2 Then
                outputFilePath = Path.GetFullPath(args(1))
            Else
                outputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "encrypted.txt")
            End If

            ' Write the Base64 encrypted text to file
            File.WriteAllText(outputFilePath, encryptedText, Encoding.UTF8)
            Log("File written: " & outputFilePath)

            ' Print to console
            Console.WriteLine("Plain: " & plainText)
            Console.WriteLine("Encrypted (Base64): " & encryptedText)
            Console.WriteLine("Saved to: " & outputFilePath)
            Console.WriteLine("Length of encrypted text: " & encryptedText.Length)

        Catch ex As Exception
            Log("FATAL ERROR: " & ex.ToString())
            Console.WriteLine("ERROR: " & ex.Message)
        End Try
    End Sub

    Private Function EncryptText(sPlainText As String) As String
        Const kScope As DataProtectionScope = DataProtectionScope.LocalMachine

        If String.IsNullOrEmpty(sPlainText) Then
            Throw New ArgumentNullException("plainText", "Plain text cannot be null or empty")
        End If

        Try
            ' Use the original salt/entropy (ASCII encoding for key)
            Dim bKeys As Byte() = Encoding.ASCII.GetBytes("SiriusArchitecture")

            ' Convert plain text to bytes using Unicode (as in original)
            Dim aPlain As Byte() = Encoding.Unicode.GetBytes(sPlainText)

            ' Encrypt the data
            Dim aEncrypted As Byte() = ProtectedData.Protect(aPlain, bKeys, kScope)

            ' Convert to Base64 string (this ensures printable characters only)
            Dim base64Result As String = Convert.ToBase64String(aEncrypted)

            Log("Encryption completed successfully. Base64 length: " & base64Result.Length)

            Return base64Result

        Catch ex As Exception
            Log("Encryption error: " & ex.ToString())
            Throw New Exception("Encryption failed: " & ex.Message, ex)
        End Try
    End Function

    ' Optional: Add a decrypt function for testing
    Private Function DecryptText(sEncryptedText As String) As String
        Const kScope As DataProtectionScope = DataProtectionScope.LocalMachine

        If String.IsNullOrEmpty(sEncryptedText) Then
            Throw New ArgumentNullException("encryptedText", "Encrypted text cannot be null or empty")
        End If

        Try
            ' Use the same salt/entropy as encryption (ASCII)
            Dim bKeys As Byte() = Encoding.ASCII.GetBytes("SiriusArchitecture")

            ' Convert from Base64 back to bytes
            Dim aEncrypted As Byte() = Convert.FromBase64String(sEncryptedText)

            ' Decrypt the data
            Dim aDecrypted As Byte() = ProtectedData.Unprotect(aEncrypted, bKeys, kScope)

            ' Convert back to string using Unicode (matching encryption)
            Dim result As String = Encoding.Unicode.GetString(aDecrypted)

            Log("Decryption completed successfully")

            Return result

        Catch ex As Exception
            Log("Decryption error: " & ex.ToString())
            Throw New Exception("Decryption failed: " & ex.Message, ex)
        End Try
    End Function

    Private Sub Log(message As String)
        Try
            Using sw As StreamWriter = New StreamWriter(logFile, True, Encoding.UTF8)
                sw.WriteLine("[" & DateTime.Now.ToString("dd MMM yyyy HH:mm:ss") & "] " & message)
            End Using
        Catch
            ' Silently ignore logging errors
        End Try
    End Sub
End Module