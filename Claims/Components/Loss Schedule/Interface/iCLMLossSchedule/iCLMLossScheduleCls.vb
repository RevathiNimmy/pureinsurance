Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Modified by Sudhanshu Behera on 6/24/2010 11:27:55 AM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface

    Private Const ACClass As String = "Interface"

    Private m_bLossSchedule As Boolean
    Private m_lLossScheduleTypeId As Integer
    Private m_lStatus As Integer

    Private m_lReturn As Integer


    Public Property LossSchedule() As Boolean
        Get
            Return m_bLossSchedule
        End Get
        Set(ByVal Value As Boolean)
            m_bLossSchedule = Value
        End Set
    End Property


    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property


    Public Property LossScheduleTypeId() As Integer
        Get
            Return m_lLossScheduleTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lLossScheduleTypeId = Value
        End Set
    End Property


    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise



        ' ***************************************************************** '
        '
        ' Name: Initialise
        '
        ' Description:
        '
        ' History: 16092002 CMG/PB - Created.
        '
        ' ***************************************************************** '

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
            End With

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bObjectManager.ObjectManager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



        End Try
    End Function


    Public Function Start() As Integer
        ' ***************************************************************** '
        '
        ' Name: Start
        '
        ' Description:
        '
        ' History: 26/10/1999 CTAF - Created.
        '
        ' ***************************************************************** '

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ProcessInterface()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



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

                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    Private Function ProcessInterface() As Integer
        ' ***************************************************************** '
        '
        ' Name: ProcessInterface
        '
        ' Description:
        '
        ' History: 16092002 CMG/PB - Created.
        '
        ' ***************************************************************** '

        Dim result As Integer = 0
        Dim oInterface As frmInterface



        result = gPMConstants.PMEReturnCode.PMTrue

        oInterface = New frmInterface()

        With oInterface
            .LossSchedule = m_bLossSchedule
            .LossScheduleTypeId = m_lLossScheduleTypeId
        End With

        ' Load the form

        'Modified by Sudhanshu Behera on 6/24/2010 11:28:37 AM refer developer guide no. 68
        'Load(oInterface)

        ' Show the form
        oInterface.ShowDialog()

        ' Remove the instance
        oInterface.Close()
        oInterface = Nothing

        Return result

    End Function

    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer
        ' ***************************************************************** '
        '
        ' Name: SetKeys
        '
        ' Description:
        '
        ' History: 20/9/2002 CMG/PB Created
        '
        ' ***************************************************************** '

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vKeyArray) Then
                Return result
            End If

            For iLoop1 As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                Select Case vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)
                    Case PMNavKeyConst.PMKeyNameLossSchedule

                        m_bLossSchedule = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case PMNavKeyConst.PMKeyNameLossScheduleTypeId

                        m_lLossScheduleTypeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                End Select

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

