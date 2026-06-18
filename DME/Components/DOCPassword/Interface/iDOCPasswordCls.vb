Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name:  Interface
    '
    ' Date:        {17/2/98}
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    'Private objfrmInterface As New frmInterface
    Private objfrmInterface As frmInterface
    Private objFrmVerify As FrmVerify
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lreturn As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name:        Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef bStandAlone As Boolean, ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bIsStandAlone = bStandAlone

            sCurrentUserName = sUsername
            iCurrentLogLevel = iLogLevel

            If bStandAlone Then
                ' Create for stand alone object
                m_oDOCPassword = New bDOCPassword.Form()

                m_lreturn = m_oDOCPassword.Initialise("sa", "", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=iCurrentLogLevel, sCallingAppName:="iDOCPassword", bStandAlone:=True)

            Else

                ' Create an instance of the object manager.
#If PD_EARLYBOUND = 1 Then

				Set g_oObjectManager = New bObjectManager.ObjectManager
#Else
                g_oObjectManager = New bObjectManager.ObjectManager()
#End If

                ' Call the initialise method.
                m_lreturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

                ' Check for errors.
                If m_lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to call the initialise method.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the object manager to nothing.
                    g_oObjectManager = Nothing

                    ' Log Error.
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("iUserID", iUserID)
                    oDict.Add("iSourceID", iSourceID)
                    oDict.Add("iLanguageID", iLanguageID)
                    oDict.Add("iCurrencyID", iCurrencyID)
                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", oDicParms:=oDict)

                    Return result
                End If

                ' Store the language ID from the object manager
                ' to the public variables, to enable us to use
                ' them throughout the object.

                Dim temp_m_oDOCPassword As Object
                m_lreturn = g_oObjectManager.GetInstance(temp_m_oDOCPassword, "bDOCPassword.Form", vInstanceManager:="ClientManager")
                m_oDOCPassword = temp_m_oDOCPassword

            End If

            If m_lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to initialise the object.", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMError
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("iUserID", iUserID)
            oDict.Add("iSourceID", iSourceID)
            oDict.Add("iLanguageID", iLanguageID)
            oDict.Add("iCurrencyID", iCurrencyID)
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:        Terminate (Standard Method)
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
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name:        Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '



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


    ' ***************************************************************** '
    ' Name:        AddPassword
    '
    ' Description: Display Password interface, modally. Return
    '              the enrypted password if stand alone
    '
    ' ***************************************************************** '
    Public Function AddPassword(Optional ByRef lNodeNum As Integer = 0, Optional ByRef iNodeLevel As Integer = 0, Optional ByRef sEncryptedPassword As String = "") As Integer


        Dim result As Integer = 0
        Try

            'set return value to true
            result = gPMConstants.PMEReturnCode.PMTrue

            'If parameter isn't missing save value in a variable
            If Not False Then
                lCurrentNum = lNodeNum
            End If

            'If parameter isn't missing save value in a variable
            If Not False Then
                iCurrentLevel = iNodeLevel
            End If

            'show AddPassword form modally
            objfrmInterface = New frmInterface()
            objfrmInterface.TxtPass.Text = ""
            objfrmInterface.ShowDialog()

            'if parameter isn't missing save value in variable
            If Not False Then
                sEncryptedPassword = sEncPassword
            End If

            'sets return value
            If iVerify <> gPMConstants.PMEReturnCode.PMOK Then
                result = iVerify
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Password failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPassword", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:        VerifyPassword
    '
    ' Description: Display verify password interface modally,
    '              including sNodeName in title bar.
    '              Return value to be PMCancel or PMOK,
    '              according to interface
    ' ***************************************************************** '

    Public Function VerifyPassword(ByRef lNodeNum As Integer, ByRef iNodeLevel As Integer, ByRef sNodeName As String) As Integer

        Dim result As Integer = 0
        Try

            'sets the value to true
            result = gPMConstants.PMEReturnCode.PMTrue

            'store parameters in Global variables
            lCurrentNum = lNodeNum
            iCurrentLevel = iNodeLevel
            objFrmVerify = New FrmVerify
            'Sets the Form title as document or Folder
            If iNodeLevel = DOCNode_Folder Then
                objFrmVerify.FraVerify.Text = "Folder - " & sNodeName
            Else
                objFrmVerify.FraVerify.Text = "Document - " & sNodeName
            End If

            'Show VerifyPassword Form
            objFrmVerify.ShowDialog()

            'sets return value

            Return iVerify

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Verify Password Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="VerifyPassword", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function
End Class