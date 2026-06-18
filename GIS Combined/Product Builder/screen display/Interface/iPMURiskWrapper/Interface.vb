Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name    : Interface
    ' File Name     : Interface.cls
    ' Date          : 24-10-2002
    ' Author        : Ram Chandrabose
    ' Description   : Wrapper Interface Component which is used for iPMURisk.dll
    '
    ' Edit History  :
    ' RAM20021024   : Created
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer

    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lProductId As Integer
    Private m_lRiskId As Integer
    Private m_lRiskTypeId As Integer
    Private m_lScreenId As Integer

    Private oRisk As Object

    ' Public instance of the object manager.
    Public g_oObjectManager As bObjectManager.ObjectManager

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public WriteOnly Property InsuranceFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property ProductId() As Integer
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

    Public WriteOnly Property RiskId() As Integer
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

    Public WriteOnly Property RiskTypeId() As Integer
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    Public WriteOnly Property ScreenId() As Integer
        Set(ByVal Value As Integer)
            m_lScreenId = Value
        End Set
    End Property

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



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)
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
            Me.disposedValue = True
            If disposing Then
                If oRisk IsNot Nothing Then
                    oRisk.Dispose()
                    oRisk = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oRisk = CreateLateBoundObject("iPMURisk.Interface_Renamed")

            ' Launch New Instance
            'lReturn = CType(oRisk, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            lReturn = oRisk.Initialise()
            If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error.
                iPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="New instance of iPMURisk - initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            lReturn = oRisk.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oRisk.Dispose()
                oRisk = Nothing
                Return result
            End If

            With oRisk

                .InsuranceFolderCnt = m_lInsuranceFolderCnt
                .InsuranceFileCnt = m_lInsuranceFileCnt
                .ProductId = m_lProductId
                .RiskId = m_lRiskId
                .RiskTypeId = m_lRiskId
                .ScreenId = m_lScreenId
                .ShowModeLessForm = True
                'developer guide no.83
                lReturn = .Start()

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function SwithTo() As Integer

        Dim result As Integer = 0
        Dim lReturn, lWinHand As Integer


        'developer guide no.32

        result = gPMConstants.PMEReturnCode.PMTrue

        ' lReturn = oRisk.SwithTo

        ' Determine if another instance is running, switch to it if that is the case
        lWinHand = MainModule.FindWindow(Nothing, "Policy Details")

        If lWinHand <> 0 Then

            ' Check if iPMURisk is running, switch to it if that is the case
            Try
                oRisk = System.Runtime.InteropServices.Marshal.GetActiveObject("iPMURisk.Interface")

                ' Trap error if no reference can be found

            Catch excep As System.Exception
                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error.
                iPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Found instance of iPMURisk - but getobject failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SwithTo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result
            End Try

            ' Resume normal error handling after inline error handler


            ' Show Window
            lReturn = MainModule.SetWindowPos(lWinHand, HWND_TOP, 0, 0, 0, 0, FLAGS)
            Application.DoEvents()
            lReturn = ShowWindow(lWinHand, SW_SHOWNORMAL)
            MainModule.SetForegroundWindow(lWinHand)
            lReturn = SetFocus(lWinHand)

        End If

        Return result

Err_SwithTo:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Activate the Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="SwithTo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
End Class
