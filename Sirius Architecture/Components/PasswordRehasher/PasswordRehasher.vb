Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports SharedFiles
Imports System.Text
Imports System.Security.Cryptography


Module PasswordRehasher
    Public Const ACGetUserDetailsStored As Boolean = True
    Public Const ACGetUserDetailsName As String = "GetUserDetails"
    Public Const ACGetUserDetailsSQL As String = "spu_get_user_details"
    Public m_oClientManager As Object = Nothing

#Region "Fields"
    ' Basic command details
    Private m_bIsRehasher As Boolean = False

    Private m_cArgs As System.Collections.ObjectModel.Collection(Of String) = Nothing
#End Region

    Sub Main()

        Dim nReturn As Integer = PMEReturnCode.PMFalse

        Try
            ProcessCommandLine()

            'Load an appropriate container for the pmuser records containing the following columns
            '    ID, Username, password
            Console.WriteLine("Processing...")
            If m_bIsRehasher Then
                'nReturn = Getuserdetails()
                'If nReturn <> PMEReturnCode.PMTrue Then
                '    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to Get user details",
                '                       vApp:=ACApp, vClass:=ACClass, vMethod:="Getuserdetails",
                '                       vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                'End If
            Else
                nReturn = DoEncryption()

            End If
            'Create the PUREInsuranceLog folder in Eventvwr if it does not exists.
            If Not EventLog.SourceExists("PUREApplicationLog") Then

                ' Create the source, if it does not already exist.
                ' An event log source should not be created and immediately used.
                ' There is a latency time to enable the source, it should be created
                ' prior to executing the application that uses the source.
                ' Execute this sample a second time to use the new source.
                EventLog.CreateEventSource("PUREApplicationLog", "PUREApplicationLog")
                Console.WriteLine("Creating EventSource")


            End If
        Catch ex As Exception
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="sirius", iType:=PMELogLevel.PMLogOnError, sMsg:="PasswordRehasher Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Getuserdetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        End Try

        Environment.Exit(0)
    End Sub

    ''' <summary>
    ''' Get user details and update  encrypted password to new column secure password
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function Getuserdetails() As Integer

        Dim nReturn As Integer = 0
        Dim dtResult As New DataTable
        Dim oDatabase As dPMDAO.Database
        Dim sOldPasswordDecrypted As String = String.Empty
        Dim sPasswordEncrypted As String = String.Empty
        Dim sNewPasswordEncrypted As String = String.Empty
        Dim sOldEncryptedPassword As String = String.Empty
        Dim sUserName As String = String.Empty
        Dim sSecurePassword As String = String.Empty
        Dim sIs_deleted As String = ""
        Dim nUserId As Integer = 0
        Dim nLanguageId As Integer = 0
        Dim bSystemUpgradeTempPwd As Boolean = False
        Try
            nReturn = gPMConstants.PMEReturnCode.PMTrue
            m_oClientManager = New bClientManager.ClientManager
            nReturn = CType(gPMComponentServices.NewDatabase("sirius", 1, 1,
                                                             v_lPMProductFamily:=
                                                                gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                             r_oDatabase:=oDatabase), 
                            gPMConstants.PMEReturnCode)

            If nReturn <> PMEReturnCode.PMTrue Then
                Return nReturn
            End If
            oDatabase.Parameters.Clear()

            nReturn = oDatabase.Parameters.Add(sName:="username", vValue:=DBNull.Value,
                                               iDirection:=PMEParameterDirection.PMParamInput,
                                               iDataType:=PMEDataType.PMString)
            nReturn = oDatabase.Parameters.Add(sName:="effective_date", vValue:=DBNull.Value,
                                               iDirection:=PMEParameterDirection.PMParamInput,
                                               iDataType:=PMEDataType.PMInteger)

            ' Execute SQL Statement

            nReturn = oDatabase.ExecuteDataTable(sSQL:=ACGetUserDetailsSQL, sSQLName:=ACGetUserDetailsName,
                                                 bStoredProcedure:=ACGetUserDetailsStored, oRecordset:=dtResult)
            If nReturn <> PMEReturnCode.PMTrue Then
                oDatabase.CloseDatabase()
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then

                For iCntr As Integer = 0 To dtResult.Rows.Count - 1
                    sSecurePassword = dtResult.Rows(iCntr)("secure_password").ToString()

                    If String.IsNullOrEmpty(sSecurePassword) Then
                        sUserName = dtResult.Rows(iCntr)("username").ToString()
                        nUserId = dtResult.Rows(iCntr)("user_id").ToString()
                        nLanguageId = dtResult.Rows(iCntr)("language_id").ToString()
                        sOldEncryptedPassword = dtResult.Rows(iCntr)("password").ToString()
                        sIs_deleted = dtResult.Rows(iCntr)("Is_deleted").ToString()
                        bSystemUpgradeTempPwd = True

                        nReturn = m_oClientManager.UpdateUser(vLanguageID:=nLanguageId,
                                                              vpassword:=sNewPasswordEncrypted,
                                                              vUsername:=sUserName,
                                                              vIsDeleted:=sIs_deleted,
                                                              vPasswordChangeDate:=DateTime.Now,
                                                              vUserID:=nUserId,
                                                              bSystemUpgradeTempPwd:=bSystemUpgradeTempPwd)
                        If nReturn <> PMEReturnCode.PMTrue Then
                            Return PMEReturnCode.PMFalse
                        End If
                      
                    End If
                Next
            End If
            Return nReturn

        Catch ex As Exception

            ' Error.
            nReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="sirius", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Getuserdetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Getuserdetails", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return nReturn
        End Try
    End Function


    ''' <summary>
    ''' Decrypts string passed and returns the result
    ''' </summary>
    ''' <param name="sPassword"></param>
    ''' <param name="r_sDecryptedPassword"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function OldDecrypt(ByVal sPassword As String, ByRef r_sDecryptedPassword As String) As Integer

        Dim nresult As Integer = 0
        Dim sAString As String = ""
        Dim sbString As New StringBuilder
        Dim iCntr As Integer = 0
        Dim sChar1 As New FixedLengthString(1)
        Dim sChar2 As New FixedLengthString(1)
        Dim iSn As Integer
        Dim sCodeString As String = ""
        Dim iClen As Integer = 0

        Try

            nresult = PMEReturnCode.PMTrue

            ' Decrypts the supplied string returning the Decrypted
            ' result. Decrypted string will always be 2 characters
            ' shorter than original
            '

            sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
            iClen = sCodeString.Length

            sAString = sPassword
            iCntr = sAString.Length - 2 'take 2 off as ignoring first and last characters in password

            If iCntr < 1 Then
                nresult = PMEReturnCode.PMFalse

                r_sDecryptedPassword = ""

                Return nresult
            End If

            iSn =
                ((Strings.Asc(sAString.Substring(sAString.Length - 1)(0)) + Strings.Asc(sAString.Substring(0, 1)(0))) Mod
                 iClen) + 1

            sChar1.Value = sAString.Substring(sAString.Length - 1)

            sChar2.Value = sAString.Substring(0, 1)

            sAString = sAString.Substring(1, sAString.Length - 2)

            Dim iPos, iTemp As Integer
            Dim sTemp As String = ""
            For iCntr2 As Integer = 1 To iCntr

                iPos = (sCodeString.IndexOf(sAString.Substring(iCntr2 - 1, 1)) + 1)
                iTemp = iPos - 1
                iTemp += (iClen * 2) 'this iClen * 2 is dodgy ! - could be * 1 or other???
                iTemp = iTemp - iSn - iCntr2

                If iTemp > 122 Then iTemp -= 56

                sTemp = Strings.Chr(iTemp).ToString()

                sbString.Append(sTemp)
            Next

            ' Return the result.
            r_sDecryptedPassword = sbString.ToString().Trim()

            Return nresult

        Catch ex As System.Exception

            nresult = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to decrypt the string", vApp:=ACApp, vClass:=ACClass, vMethod:="Encrypt", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return nresult

        End Try
    End Function


    ''' <summary>
    ''' Process command line for flags and commands
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessCommandLine()

        ' Check for parameters
        If My.Application.CommandLineArgs.Count = 0 Then
            ' No parameters so default to Password ReHasher
            m_bIsRehasher = True
        End If

        ' Create interface argument collection
        m_cArgs = New Collections.ObjectModel.Collection(Of String)()

        ' Process args
        For Each sArg As String In My.Application.CommandLineArgs

            '  First Param is LoginID and Second is Password
            m_cArgs.Add(sArg.Trim)
        Next
    End Sub

    ''' <summary>
    ''' Encrypt the UserName,Password and Save it in Registry
    ''' </summary>
    ''' <remarks></remarks>
    Private Function DoEncryption() As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMFalse
        
            Dim sLoginId As String = ""
            Dim sPassword As String = ""

            If m_bIsRehasher = False AndAlso m_cArgs.Count > 0 Then
                sLoginID = m_cArgs(0).ToString
                'If Password is Blank then only LoginID will be Supplied.
                If m_cArgs.Count > 1 Then
                    sPassword = m_cArgs(1).ToString
                End If
            End If

            Dim bKeys As Byte()
            bKeys = Encoding.ASCII.GetBytes(PMEncryptionEntropy) _
            'Optional Entropy. HardCoded Key for Additional Security. 

            'Encrypt
            Dim sLoginIdSecure As String = ""
            Dim sPasswordSecure As String = ""
            sLoginIDSecure = Encrypt(sLoginID, bKeys)
            sPasswordSecure = Encrypt(sPassword, bKeys)

            'Save the Value in Registry

            nResult = SetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine,
                                      v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture,
                                      v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLCommon,
                                      v_sSettingName:=PMSQLLoginId,
                                      v_sSettingValue:=sLoginIdSecure)
            If nResult <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to Execute SetPMRegSetting.")
            End If

            nResult = SetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine,
                                      v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture,
                                      v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLCommon,
                                      v_sSettingName:=PMSQLLoginPassword,
                                      v_sSettingValue:=sPasswordSecure)
            If nResult <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to Execute SetPMRegSetting.")
            End If
            Return nResult


    End Function

    ''' <summary>
    ''' Encrypt  the password with the help of key provided
    ''' </summary>
    ''' <param name="sText"></param>
    ''' <param name="bKeys"></param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Private Function Encrypt(sText As String, bKeys As Byte()) As String
        Const kScope As DataProtectionScope = DataProtectionScope.LocalMachine
        If sText Is Nothing Then
            Throw New ArgumentNullException("plainText")
        End If

        'encrypt data
        Dim aData = Encoding.Unicode.GetBytes(sText)
        Dim aEncrypted As Byte() = ProtectedData.Protect(aData, bKeys, kScope)

        'return as base64 string
        Return Convert.ToBase64String(aEncrypted)
    End Function

End Module

