Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Imports System.Collections.Generic

Module MainModule
    ' ***************************************************************** '
    ' Module Name: Main
    '
    ' Date: 04 September 1996
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main global constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMMessage"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' CTAF 180701
    ' Number of lines to return encrypted to iPMMessage
    Public Const ACReturnLines As Integer = 20

    ' CTAF 190701
    ' Constants for temporary file names
    Public Const ACClientLog As String = "c:\~~client.txt"
    Public Const ACServerLog As String = "c:\~~server.txt"
    Public Const ACCobolLog As String = "c:\~~cobol.txt"
    Public Const ACRegistry As String = "C:\~~reg.txt"

    ' RDC 30072002 event log support START
    ' Constants for different types of events
    Public Const Event_Type_Info As String = "Information"
    Public Const Event_Type_Warning As String = "Warning"
    Public Const Event_Type_Error As String = "Error"
    Public Const Event_Type_Success_Audit As String = "Audit Success"
    Public Const Event_Type_Failure_Audit As String = "Audit Failure"

    Public Const EVENTLOG_ERROR_TYPE As Integer = &H1S
    Public Const EVENTLOG_WARNING_TYPE As Integer = &H2S
    Public Const EVENTLOG_INFORMATION_TYPE As Integer = &H4S
    Public Const EVENTLOG_AUDIT_SUCCESS As Integer = &H8S
    Public Const EVENTLOG_AUDIT_FAILURE As Integer = &H10S

    ' Constants for different types of event filters
    Public Const Filter_Type_None As Integer = 0
    Public Const Filter_Type_TimeBefore As Integer = 1
    Public Const Filter_Type_TimeAfter As Integer = 2
    Public Const Filter_Type_EventType As Integer = 3
    Public Const Filter_Type_Source As Integer = 4
    Public Const Filter_Type_Category As Integer = 5
    Public Const Filter_Type_Computer As Integer = 6
    Public Const Filter_Type_EventID As Integer = 7

    ' Error numbers possibly returned from EventLog Server
    Public Const ERR_LOGTYPE_NOT_SET As Integer = 1011 + Constants.vbObjectError
    Public Const ERR_SOURCENAME_NOT_SET As Integer = 1012 + Constants.vbObjectError
    Public Const ERR_BAD_INDEX As Integer = 1013 + Constants.vbObjectError
    Public Const ERR_FAILED_OPEN_REGISTRY_KEY As Integer = 1014 + Constants.vbObjectError
    Public Const ERR_FAILED_READ_REGISTRY_KEY As Integer = 1015 + Constants.vbObjectError
    Public Const ERR_RESOURCE_DATA_NOT_FOUND As Integer = 1016 + Constants.vbObjectError
    Public Const ERR_READING_EVENT_LOG As Integer = 1017 + Constants.vbObjectError
    Public Const ERR_LOG_NOT_OPENED As Integer = 1018 + Constants.vbObjectError
    Public Const ERR_FAILED_SET_LOG_TYPE As Integer = 1019 + Constants.vbObjectError
    ' RDC 30072002 event log support END

    Private m_lReturn As Integer
    Public Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer

    Public Const SWP_NOMOVE As Integer = &H2S
    Public Const SWP_NOSIZE As Integer = &H1S
    Public Const HWND_TOPMOST As Integer = -1
    Public Const HWND_TOP As Integer = 0
    Public Const HWND_NOTOPMOST As Integer = -2
    Public Const HWND_BOTTOM As Integer = 1

    Public Function SetWindowPlacement(ByVal lWindowHwnd As Integer, ByVal bKeepInFront As Boolean) As Integer

        Try

            If bKeepInFront Then
                m_lReturn = SetWindowPos(lWindowHwnd, HWND_TOPMOST, 1, 1, 1, 1, SWP_NOMOVE Or SWP_NOSIZE)
            Else
                m_lReturn = SetWindowPos(lWindowHwnd, HWND_NOTOPMOST, 1, 1, 1, 1, SWP_NOMOVE Or SWP_NOSIZE)
            End If



        Catch ex As Exception

            ' Log Error.
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the window position", vApp:=ACApp, vClass:=ACClass, vMethod:="SetWindowPlacement", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Finally

        End Try
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function
    ' ***************************************************************** '
    ' Name: LogMessage
    '
    ' Description: Wrapper function for LogMessage.
    '              This will be called by an error within iPMMessage
    '              therefore just popup on screen.
    ' ***************************************************************** '
    Public Sub LogMessage(ByVal iType As Integer, ByVal sMsg As String, Optional ByRef vApp As Object = Nothing, _
                          Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, _
                          Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing, _
                          Optional ByRef sUsername As String = "", Optional ByRef ex As Exception = Nothing, _
                          Optional ByRef oDicParms As Dictionary(Of String, Object) = Nothing)
        Try
            gPMFunctions.LogMessagePopup(iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), _
                                         vClass:=CStr(vClass), vMethod:=CStr(vMethod), _
                                         excep:=ex, oDicParms:=oDicParms)

        Catch
            ' Error Section.
            Exit Sub
        End Try


    End Sub

    ' ***************************************************************************************
    '
    ' Name: RetrieveSingleSystemOption
    '
    ' Description : This function should not be used. It is here because gPMFunctions.bas
    '               references it which in turn uses iPMFunc or bPMFunc
    '               Now, bObjectManager is a client side business object and although it's
    '               client side, it still needs to log errors to a file which is why
    '               it has it's own LogMessage function.
    '               If you include iPMFunc then you get a clash of LogMessages (and the wrong)
    '               one is in iPMFunc (it's the popup version). You can't use bPMFunc because
    '               this object isn't strictly a business object.
    '
    ' ***************************************************************************************
    Public Function RetrieveSingleSystemOption(ByRef v_sUsername As String, ByRef v_sPassword As String, ByRef v_iUserID As Integer, ByRef v_iMainSourceID As Integer, ByRef v_iLanguageID As Integer, ByRef v_iCurrencyID As Integer, ByRef v_iLogLevel As Integer, ByRef v_sCallingAppName As String, ByRef v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef v_iSourceID As Integer = 0) As Integer

        ' Please read comments above function
        Return gPMConstants.PMEReturnCode.PMError

    End Function
End Module
