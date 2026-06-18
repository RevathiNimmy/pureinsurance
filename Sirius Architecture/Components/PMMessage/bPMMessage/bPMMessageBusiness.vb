Option Strict Off
Option Explicit On
Imports System.Text
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 24th October 1996
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to manipulate
    '              a PMMessage.
    '
    ' Edit History:
    ' DAK080500 - check message text for appostrophe
    ' RDC 29072002 - all messaging now performed to the o/s event log
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PRIVATE Data Members (Begin)

    Private m_lError As gPMConstants.PMEReturnCode

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property LogLevel() As Integer
        Get
            ' Return the System Log Level
            Return m_iLogLevel
        End Get
    End Property
    ' RFC250398 - Product Family Property Get Added.
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property
    ' PUBLIC Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long 

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set LogLevel

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("iUserID", iUserID)
            oDict.Add("iSourceID", iSourceID)
            oDict.Add("iLanguageID", iLanguageID)
            oDict.Add("iCurrencyID", iCurrencyID)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep, oDicParms:=oDict)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Add a single message
    '
    ' Changes: RDC 29072002 all messaging now goes to event log
    ' ***************************************************************** '
    Public Function Add(ByRef lMessageID As Integer, ByRef iSourceID As Integer, ByRef sUsername As String, ByRef dtLogDate As Date, ByRef iMessageType As Integer, ByRef sCallingAppName As String, ByRef sText As String, ByRef lErrNumber As Integer, ByRef sErrDescription As String, ByRef sAppName As String, ByRef sClassName As String, ByRef sMethodName As String, ByRef vTimestamp As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lError = CType(MessageEventLog(iSourceID, sUsername, dtLogDate, iMessageType, sCallingAppName, sText, lErrNumber, sErrDescription, sAppName, sClassName, sMethodName), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("lMessageID", lMessageID)
            oDict.Add("iSourceID", iSourceID)
            oDict.Add("dtLogDate", dtLogDate)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", excep:=excep, oDicParms:=oDict)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:     MessageEventLog
    '
    ' Description:  record message in system event log when database is unavailable
    '
    ' Created:  RDC 22072002
    ' ***************************************************************** '
    Private Function MessageEventLog(ByRef iSourceID As Integer, ByRef sUsername As String, ByRef dtLogDate As Date, ByRef iMessageType As Integer, ByRef sCallingAppName As String, ByRef sText As String, ByRef lErrNumber As Integer, ByRef sErrDescription As String, ByRef sAppName As String, ByRef sClassName As String, ByRef sMethodName As String) As Integer

        Dim result As Integer = 0
        Dim sErrUniqueId As String = gPMFunctions.GenerateUniqueSSPExceptionRef(gPMConstants.ERROR_NO_LENGTH)

        result = gPMConstants.PMEReturnCode.PMFalse

        m_lError = CType(gPMFunctions.EventLogWrite(iType:=iMessageType, sMsg:=sText, oDicParms:=Nothing, sErrUniqueId:=sErrUniqueId, excep:=New Exception(sErrDescription), vUsername:=sUsername, vCallingApp:=sCallingAppName, vApp:=sAppName, vClass:=sClassName, vMethod:=sMethodName), gPMConstants.PMEReturnCode)


        Return gPMConstants.PMEReturnCode.PMTrue


    End Function

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: EncryptString
    '
    ' Description:
    '
    ' History: 18/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function EncryptString(ByVal v_sInString As String, ByRef r_sOutString As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sOutString = ""

            For Each sByte_Renamed As Char In v_sInString
                ' Get the next byte
                ' Encrypt it with the shaolin magic key
                sByte_Renamed = Strings.ChrW(Strings.AscW(sByte_Renamed) Xor &HCFS).ToString()
                ' Add it
                r_sOutString = r_sOutString & sByte_Renamed
            Next sByte_Renamed

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncryptString Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncryptString", excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: EncryptLogFile
    '
    ' Description: Update this in the interface and business
    '
    ' History: 18/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function EncryptLogFile(ByVal v_sLogFile As String, ByRef r_sEncryptedLogFile As String, Optional ByVal v_bSiriusLog As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim lFile As Integer
        Dim sLine As String = ""
        Dim sTemp As New StringBuilder
        Dim vTempArray As Object
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lFile = FileSystem.FreeFile()

            ' Load the last few lines
            FileSystem.FileOpen(lFile, v_sLogFile, OpenMode.Input)

            sTemp = New StringBuilder("")
            sLine = ""

            vTempArray = Nothing

            While Not FileSystem.EOF(lFile)

                sLine = FileSystem.LineInput(lFile)

                If v_bSiriusLog Then

                    ' RDC 01112001 search for date dd/mm/yy or dd/mm/yyyy
                    If (sLine.Substring(11, 1) = ":") And (sLine.Substring(14, 1) = ":") Or (sLine.Substring(13, 1) = ":") And (sLine.Substring(16, 1) = ":") Then
                        '            If (Mid$(sLine, 14, 1) = ":") And (Mid$(sLine, 17, 1) = ":") Then
                        ' New line
                        If Information.IsArray(vTempArray) Then
                            ' Store the old line


                            vTempArray(vTempArray.GetUpperBound(0)) = sTemp.ToString()
                            ' Increase the array

                            ReDim Preserve vTempArray(vTempArray.GetUpperBound(0) + 1)
                            ' Reset the line
                            sTemp = New StringBuilder("")
                        Else
                            ReDim vTempArray(0)
                        End If
                    End If

                    If sTemp.ToString() <> "" Then
                        sTemp.Append(Environment.NewLine & sLine)
                    Else
                        sTemp = New StringBuilder(sLine)
                    End If

                Else

                    ' New line
                    If Information.IsArray(vTempArray) Then
                        ' Store the old line


                        vTempArray(vTempArray.GetUpperBound(0)) = sTemp.ToString()
                        ' Increase the array

                        ReDim Preserve vTempArray(vTempArray.GetUpperBound(0) + 1)
                        ' Reset the line
                        sTemp = New StringBuilder("")
                    Else
                        ReDim vTempArray(0)
                    End If

                    If sTemp.ToString() <> "" Then
                        sTemp.Append(Environment.NewLine & sLine)
                    Else
                        sTemp = New StringBuilder(sLine)
                    End If

                End If

            End While

            ' PWF 11/08/2002 - Check we have an array
            If Not Information.IsArray(vTempArray) Then
                ReDim vTempArray(0)
            End If



            vTempArray(vTempArray.GetUpperBound(0)) = sTemp.ToString()

            FileSystem.FileClose(lFile)

            r_sEncryptedLogFile = ""

            ' Encrypt them

            For iLoop1 As Integer = vTempArray.GetUpperBound(0) To (vTempArray.GetUpperBound(0) - ACReturnLines) Step -1

                If iLoop1 < 0 Then
                    Exit For
                End If


                sTemp = New StringBuilder(CStr(vTempArray(iLoop1)) & Environment.NewLine)

                lReturn = EncryptString(v_sInString:=sTemp.ToString(), r_sOutString:=sLine)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Return the value
                r_sEncryptedLogFile = r_sEncryptedLogFile & sLine

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncryptLogFile failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncryptLogFile", excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetLogFile
    '
    ' Description: Gets the last X lines from sirius.log and encrypts them
    '
    ' History: 18/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetLogFile(ByRef r_sLogFile As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sFileName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the name of the log file
            lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="LogFileName", r_sSettingValue:=sFileName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If FileSystem.Dir(sFileName, FileAttribute.Normal) = "" Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Encrypt it
            lReturn = CType(EncryptLogFile(v_sLogFile:=sFileName, r_sEncryptedLogFile:=r_sLogFile), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLogFile failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLogFile", excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCobolLogFile
    '
    ' Description:
    '
    ' History: 19/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetCobolLogFile(ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByRef r_sLogFile As String) As Integer

        Dim result As Integer = 0
        Dim sLogFile, sLogPath As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the name of the logfile
            ' [userid]|msg|[sourceid]|.dat
            sLogFile = StringsHelper.Format(v_iUserID, "0000") & "msg" & CStr(v_iSourceID) & ".dat"

            ' Return not found if it doesnt exist
            If FileSystem.Dir(sLogFile, FileAttribute.Normal) = "" Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Get the registry setting location of the files
            m_lError = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFGeminiII, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="COBMSGPATH", r_sSettingValue:=sLogPath), gPMConstants.PMEReturnCode)

            If Not sLogPath.EndsWith("1") Then
                sLogPath = sLogPath & "\"
            End If

            ' Construct the full path
            sLogFile = sLogPath & sLogFile

            ' Encrypt and return it
            m_lError = CType(EncryptLogFile(v_sLogFile:=sLogFile, r_sEncryptedLogFile:=r_sLogFile, v_bSiriusLog:=False), gPMConstants.PMEReturnCode)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_iUserID", v_iUserID)
            oDict.Add("v_iSourceID", v_iSourceID)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCobolLogFile failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCobolLogFile", excep:=excep, oDicParms:=oDict)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSystemName
    '
    ' Description: used by iPMMessage to get server system name for
    '              event log functionality.
    '
    ' History: RDC 30072002 created
    '
    ' ***************************************************************** '
    Public Function GetSystemName(ByRef sSystemName As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lError = CType(GetSystemName(sSystemName), gPMConstants.PMEReturnCode)


            Return m_lError

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
End Class

