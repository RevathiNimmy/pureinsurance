Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 4/2/98
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private objfrmInterface As frmInterface

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef bStandAlone As Boolean) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bIsStandAlone = bStandAlone

            ' Create an instance of the object manager.
#If PD_EARLYBOUND = 1 Then

			Set g_oObjectManager = New bObjectManager.ObjectManager
#Else
            g_oObjectManager = New bObjectManager.ObjectManager()
#End If

            ' Call the initialise method.
            m_lReturn = CType(g_oObjectManager.Initialise(sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUsername = .UserName
                g_sPassword.Value = .Password
                g_iUserID = .UserID
                g_iLogLevel = .LogLevel
                g_iCurrencyID = .CurrencyID
            End With

            sCurrentUserName = g_sUsername
            iCurrentLogLevel = g_iLogLevel

            ' Get the instance via object manager...
            '    m_lReturn& = g_oObjectManager.GetInstance( _
            ''        oObject:=g_oViewBatch, _
            ''        sClassName:="bDOCViewBatch.Form", _
            ''        vInstanceManager:=PMGetLocalBusiness)
            '    If (m_lReturn <> PMTrue) Then
            '        Initialise = PMFalse
            '        Exit Function
            '    End If

            g_oViewBatch = New bDOCViewBatch.Form()

            ' ...but initialise it ourselves because of the extra parameter

            m_lReturn = g_oViewBatch.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:="iDOCViewBatch", bStandAlone:=bStandAlone)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DN 14/02/01 - Get instance the viewer if not already done so
            If g_oViewer Is Nothing Then

                g_oViewer = New iDOCViewer.Interface_Renamed()

                'initialise and pass instance of myself
                m_lReturn = g_oViewer.Initialise(Me)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    g_oViewer.Dispose()
                    g_oViewer = Nothing
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise viewer", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                    Return result
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ViewBatch() as Long
    '
    ' Description: Displays the view batch form.
    '
    ' ***************************************************************** '

    Public Function ViewBatch() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Load the form
            objfrmInterface = New frmInterface()

            ' Display it
            objfrmInterface.ShowDialog()

            ' unload the form and destroy it
            objfrmInterface.Close()

            objfrmInterface = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ViewBatch", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate() as Long
    '
    ' Description: Standard exit point
    '
    ' ***************************************************************** '

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oViewBatch IsNot Nothing Then
                    g_oViewBatch.Dispose()
                    g_oViewBatch = Nothing
                End If
                If g_oViewer IsNot Nothing Then
                    g_oViewer.Dispose()
                    g_oViewer = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

End Class
