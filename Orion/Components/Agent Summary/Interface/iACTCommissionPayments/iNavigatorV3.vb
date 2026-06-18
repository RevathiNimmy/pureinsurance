Option Strict Off
Option Explicit On
Imports SharedFiles
Imports Microsoft.VisualBasic
Imports System
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic.Compatibility.VB6
Public Interface _NavigatorV3
    WriteOnly Property NavigatorV3_CallingAppName As String
    WriteOnly Property NavigatorV3_PMAuthorityLevel As Integer
    ReadOnly Property NavigatorV3_Status As Integer
    ReadOnly Property PMProductFamily As Integer
    WriteOnly Property PMNavCancelAction As String
    Function Initialise() As Integer
    Function Terminate() As Integer
    Function NavigatorV3_SetKeys(ByRef vKeyArray As Object) As Integer
    Function NavigatorV3_GetKeys(ByRef vKeyArray As Object) As Integer
    Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer
    Function NavigatorV3_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
    Function NavigatorV3_Start() As Integer
End Interface
<System.Runtime.InteropServices.ProgId("NavigatorV3_NET.NavigatorV3")> Public NotInheritable Class NavigatorV3
    Implements IDisposable
    Implements aPMNav.NavigatorV3
    ' ***************************************************************** '
    ' Class Name: NavigatorV3
    '
    ' Date: 16/11/1998
    '
    ' Description: NavigatorV3 public class to control the Interface class.
    '
    ' Edit History:
    ' ***************************************************************** '



    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "NavigatorV3"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)
    ' PRIVATE Data Members (Begin)

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status
    Private m_lStatus As Integer

    ' Stores the Navigator Cancel action
    Private m_sPMNavCancelAction As String

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Stores the component Interface object
    Private m_oInterface As Interface_Renamed
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public WriteOnly Property NavigatorV3_CallingAppName() As String Implements aPMNav.NavigatorV3.CallingAppName
        Set(ByVal Value As String)
            m_oInterface.CallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property NavigatorV3_PMAuthorityLevel() As Integer Implements aPMNav.NavigatorV3.PMAuthorityLevel
        Set(ByVal Value As Integer)

            m_oInterface.PMAuthorityLevel = Value

        End Set
    End Property

    Public ReadOnly Property NavigatorV3_Status() As Integer Implements aPMNav.NavigatorV3.Status
        Get
            NavigatorV3_Status = m_oInterface.Status
        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            PMProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMNavCancelAction() As String
        Set(ByVal Value As String)

            m_sPMNavCancelAction = Value

        End Set
    End Property
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

        Try

            Initialise = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of the Interface class
            m_oInterface = New Interface_Renamed

            m_lReturn = m_oInterface.Initialise()

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Initialise = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the Interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                Exit Function
            End If

            Exit Function


        Catch ex As Exception

            Initialise = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=ex)

            Exit Function

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
                If m_oInterface IsNot Nothing Then
                    m_oInterface.Dispose()
                    m_oInterface = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: NavigatorV3_SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_SetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV3.SetKeys

        Dim lRow As Integer

        Try

            NavigatorV3_SetKeys = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If (IsArray(vKeyArray) = False) Then
                NavigatorV3_SetKeys = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Call the corresponding Interface function
            m_lReturn = m_oInterface.SetKeys(vKeyArray:=vKeyArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.SetKeys failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys")

                NavigatorV3_SetKeys = gPMConstants.PMEReturnCode.PMFalse
                Exit Function

            End If

            Exit Function


        Catch ex As Exception

            NavigatorV3_SetKeys = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_SetKeys", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_GetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV3.GetKeys

        Dim lRow As Integer

        Try

            NavigatorV3_GetKeys = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = m_oInterface.GetKeys(vKeyArray:=vKeyArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.GetKeys failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys")

                NavigatorV3_GetKeys = gPMConstants.PMEReturnCode.PMFalse
                Exit Function

            End If

            Exit Function


        Catch ex As Exception

            NavigatorV3_GetKeys = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_GetKeys", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer Implements aPMNav.NavigatorV3.GetSummary

        Dim lRow As Integer

        Try

            NavigatorV3_GetSummary = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = m_oInterface.GetSummary(vSummaryArray:=vSummaryArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.GetSummary failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary")

                NavigatorV3_GetSummary = gPMConstants.PMEReturnCode.PMFalse
                Exit Function

            End If

            Exit Function


        Catch ex As Exception

            NavigatorV3_GetSummary = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_GetSummary", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer Implements aPMNav.NavigatorV3.SetProcessModes

        Try

            NavigatorV3_SetProcessModes = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = m_oInterface.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.SetProcessModes failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                NavigatorV3_SetProcessModes = gPMConstants.PMEReturnCode.PMFalse
                Exit Function

            End If

            Exit Function


        Catch ex As Exception

            NavigatorV3_SetProcessModes = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_SetProcessModes", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_Start() As Integer Implements aPMNav.NavigatorV3.Start

        Dim sMsg As String

        Try

            NavigatorV3_Start = gPMConstants.PMEReturnCode.PMTrue

            ' Loop to cater for Cancel action
            Do

                ' Call the corresponding Interface function
                m_lReturn = m_oInterface.Start()

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.Start failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")

                    NavigatorV3_Start = gPMConstants.PMEReturnCode.PMFalse
                    Exit Do
                    Exit Function

                End If

                ' Check Interface Exit status
                If (m_oInterface.Status <> gPMConstants.PMEReturnCode.PMCancel) Then
                    Exit Do
                End If

                Select Case m_sPMNavCancelAction
                    Case gPMConstants.PMNavActionExitMap, PMNavActionAbortProcess
                        sMsg = "About to abort the current Navigator Process."
                        sMsg = sMsg & "Are you sure you wish to cancel?"
                        If (MsgBox(sMsg, MsgBoxStyle.YesNo + MsgBoxStyle.Critical, "Cancel Process.") = MsgBoxResult.Yes) Then
                            Exit Do
                        End If
                    Case Else
                        Exit Do
                End Select

            Loop

            Exit Function


        Catch ex As Exception

            NavigatorV3_Start = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_Start", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)

    'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Sub Class_Initialize_Renamed()

        ' Class Initialise Event.


        Exit Sub

    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub
End Class