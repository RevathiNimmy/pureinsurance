Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
Imports System.Collections.Generic

<System.Runtime.InteropServices.ProgId("PMMessageV2_NET.PMMessageV2")> _
Public NotInheritable Class PMMessageV2
    ' ***************************************************************** '
    ' Class Name: PMMessageV2
    '
    ' Date: 04 September 1996
    '
    ' Description: Main class containing all of the business methods.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the methods to identify
    ' which class this is.
    Private Const ACClass As String = "PMMessage"

    ' ***************************************************************** '
    '
    ' Name: LogMessage
    '
    ' Description:
    '
    ' History: 18/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function LogMessage(ByVal iType As Integer, ByVal sMsg As String, Optional ByVal vApp As String = "", Optional ByVal vClass As String = "", _
                               Optional ByVal vMethod As String = "", Optional ByVal vErrNo As String = "", Optional ByVal vErrDesc As String = "", _
                               Optional ByVal bSilent As Boolean = False, Optional excep As Exception = Nothing, Optional oDicParms As Dictionary(Of String, Object) = Nothing) As Integer

        Dim result As Integer = 0
        Dim oForm As frmError
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim sErrUniqueId As String = gPMFunctions.GenerateUniqueSSPExceptionRef(gPMConstants.ERROR_NO_LENGTH)

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=iType, sMsg:=sMsg, sErrUniqueId:=sErrUniqueId, vApp:=vApp, vClass:=vClass, _
                                          vMethod:=vMethod, excep:=excep, oDicParms:=oDicParms)

            If Not (bSilent) Then

                ' new instance of the form
                oForm = New frmError()
                SetWindowPlacement(oForm.Handle.ToInt32(), True)
                ' load it

                ' Set the values
                With oForm
                    ' Default to fatal error
                    .ErrType = IIf(iType = 0, gPMConstants.PMELogLevel.PMLogFatal, iType)
                    .Message = sMsg
                    .Application = vApp
                    .Module_Renamed = vClass
                    .Method = vMethod
                    .ErrUniqueId = sErrUniqueId
                    .VBErrNo = gPMFunctions.NullToLong(vErrNo)
                    .VBErrMessage = gPMFunctions.NullToString(vErrDesc)
                End With

                ' Show em
                lReturn = oForm.PropertiesToInterface()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Show the form
                oForm.ShowDialog()

                ' Unload the form
                oForm.Close()

                oForm = Nothing

            End If

            Return result

        Catch excep2 As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LogMessage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LogMessage", excep:=excep2)

            Return result

        End Try
    End Function
End Class
