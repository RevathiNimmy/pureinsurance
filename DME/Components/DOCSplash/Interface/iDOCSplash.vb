Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: {17/2/98}
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"
    Private objfrmInterface As iDOCSplash.frmInterface = New iDOCSplash.frmInterface

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""



    ' {* USER DEFINED CODE (Begin) *}
    Private m_sDMEDIR As String = ""

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lreturn As Integer
    ' PRIVATE Data Members (End)


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                objfrmInterface = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' Private Delegate Sub DlgCloseFrm()
    ' ***************************************************************** '
    ' Name: Show (Standard Method)
    '
    ' Description: Show the splash form. Tweak dimensions according to
    ' which type.
    '
    ' ***************************************************************** '
    Public Function Show(ByRef iSplashType As Integer, Optional ByRef sMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim lHeight, lWidth, lTextHeight As Integer
        Const THRESHOLD As Integer = 6000
        Dim fileAvi As IO.FileInfo

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the label back to normal size and properties
            'frmInterface.aniMain.Visible = True
            'objfrmInterface.aniMain.Visible = True

            'frmInterface.lblLabel.WordWrap = False
            'frmInterface.lblLabel.AutoSize = True

            'To do
            'objfrmInterface.lblLabel.WordWrap = False
            objfrmInterface.lblLabel.AutoSize = True
            'Get the DME install directory
            If m_sDMEDIR.Trim() = "" Then
                m_lreturn = GetDMEDIR(m_sDMEDIR)
            End If



            Select Case iSplashType
                Case DOCSplash_Copying
                    'copying stuff
                    fileAvi = New IO.FileInfo(m_sDMEDIR & DOCAVIDir & "\filecopy.avi")
                    If fileAvi.Exists Then
                        objfrmInterface.lblLabel.Text = "Copying Data ... Please Wait ..."
                        'objfrmInterface.aniMain.Open(m_sDMEDIR & DOCAVIDir & "\filecopy.avi")
                        'objfrmInterface.aniMain.AutoPlay = True
                        objfrmInterface.Height = VB6.TwipsToPixelsY(1710)
                        objfrmInterface.Width = VB6.TwipsToPixelsX(4225)
                    End If


                Case DOCSplash_Moving
                    'moving stuff
                    fileAvi = New IO.FileInfo(m_sDMEDIR & DOCAVIDir & "\filemove.avi")
                    If fileAvi.Exists Then
                        objfrmInterface.lblLabel.Text = "Moving Data ... Please Wait ..."
                        'objfrmInterface.aniMain.Open(m_sDMEDIR & DOCAVIDir & "\filemove.avi")
                        'objfrmInterface.aniMain.AutoPlay = True
                        objfrmInterface.Height = VB6.TwipsToPixelsY(1710)
                        objfrmInterface.Width = VB6.TwipsToPixelsX(4425)
                    End If
                Case DOCSplash_Deleting
                    'deleting stuff
                    fileAvi = New IO.FileInfo(m_sDMEDIR & DOCAVIDir & "\filedel.avi")
                    If fileAvi.Exists Then
                        objfrmInterface.lblLabel.Text = "Deleting Data ... Please Wait ..."
                        'objfrmInterface.aniMain.Open(m_sDMEDIR & DOCAVIDir & "\filedel.avi")
                        'objfrmInterface.aniMain.AutoPlay = True
                        'objfrmInterface.aniMain.Top = objfrmInterface.aniMain.Top
                        objfrmInterface.Height = VB6.TwipsToPixelsY(1800)
                        objfrmInterface.Width = VB6.TwipsToPixelsX(5225)

                    End If

                Case DOCSplash_Retrieving
                    'retrieving stuff
                    objfrmInterface.lblLabel.Text = "Retrieving Data ... Please Wait ..."
                    objfrmInterface.Height = VB6.TwipsToPixelsY(1000)
                    objfrmInterface.Width = VB6.TwipsToPixelsX(3400)

                Case DOCSplash_Message
                    ' generic message

                    'To Do
                    'objfrmInterface.lblLabel.WordWrap = False
                    objfrmInterface.lblLabel.Text = sMessage

                    lHeight = CInt(VB6.PixelsToTwipsY(objfrmInterface.lblLabel.Height))
                    lWidth = CInt(VB6.PixelsToTwipsX(objfrmInterface.lblLabel.Width))
                    lTextHeight = CInt(VB6.PixelsToTwipsY(objfrmInterface.lblLabel.Height))

                    If lWidth > THRESHOLD Then

                        While (lWidth > THRESHOLD)
                            lWidth -= THRESHOLD
                            lHeight += lTextHeight
                        End While

                        objfrmInterface.lblLabel.AutoSize = False

                        objfrmInterface.lblLabel.Width = VB6.TwipsToPixelsX(THRESHOLD)
                        objfrmInterface.lblLabel.Height = VB6.TwipsToPixelsY(lHeight)

                        ' The animation control gets in the way, so hide it
                        'objfrmInterface.aniMain.Visible = False

                    Else

                        'objfrmInterface.aniMain.Visible = True

                    End If

                    objfrmInterface.Width = objfrmInterface.lblLabel.Width + VB6.TwipsToPixelsX(750)
                    objfrmInterface.Height = objfrmInterface.lblLabel.Height + VB6.TwipsToPixelsY(750)

                Case Else
                    'generic
                    objfrmInterface.lblLabel.Text = "Processing ... Please Wait ..."
                    objfrmInterface.Height = VB6.TwipsToPixelsY(1000)
                    objfrmInterface.Width = VB6.TwipsToPixelsX(3000)

            End Select
            objfrmInterface.Show()
            Application.DoEvents()
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Show", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Hide (Standard Method)
    '
    ' Description: Hide the splash form.
    '
    ' ***************************************************************** '
    Public Function Hide() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'stop AVI and Hide the form
            'objfrmInterface.aniMain.AutoPlay = False
            objfrmInterface.Hide()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Hide", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error Message
        'LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
