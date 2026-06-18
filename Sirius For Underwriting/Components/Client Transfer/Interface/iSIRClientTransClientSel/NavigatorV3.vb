Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("NavigatorV3_NET.NavigatorV3")> _
Public NotInheritable Class NavigatorV3
    Implements IDisposable
    Implements aPMNav.NavigatorV3
    ' ***************************************************************** '
    ' Class Name: NavigatorV3
    '
    '
    ' Description: NavigatorV3 public class to control the Interface class.
    '
    ' Edit History:Saurabh
    ' ***************************************************************** '



    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "NavigatorV3"

    ' Stores the exit status
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Stores the component Inrterface object
    'Developer Guide No.88
    Private m_oInterface As Interface_Renamed

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
            Return m_oInterface.Status
        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
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
        Const kMethodName As String = "Initialise"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Create instance of the Interface class
            'Developer Guide No.88
            m_oInterface = New Interface_Renamed()
            'Developer Guide No.9
            m_lReturn = m_oInterface.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "g_oObjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
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

        Dim result As Integer = 0
        Dim lRow As Integer

        Const kMethodName As String = "NavigatorV3_SetKeys"
        Try
            result = gPMConstants.PMEReturnCode.PMTrue



            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Call the corresponding Interface function
            m_lReturn = CType(m_oInterface.SetKeys(vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to called Interface SetKeys", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_GetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV3.GetKeys

        Dim result As Integer = 0
        Dim lRow As Integer
        Const kMethodName As String = "NavigatorV3_GetKeys"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            ' Call the corresponding Interface function

            m_lReturn = CType(m_oInterface.GetKeys(vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to called Interface GetKeys", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer Implements aPMNav.NavigatorV3.GetSummary

        Dim result As Integer = 0
        Dim lRow As Integer
        Const kMethodName As String = "NavigatorV3_GetSummary"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            ' Call the corresponding Interface function
            m_lReturn = CType(m_oInterface.GetSummary(vSummaryArray:=vSummaryArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to called Interface GetSummary", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer Implements aPMNav.NavigatorV3.SetProcessModes

        Dim result As Integer = 0
        Const kMethodName As String = "NavigatorV3_SetProcessModes"
        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            ' Call the corresponding Interface function
            m_lReturn = CType(m_oInterface.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, "Failed to call SetProcessMode", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_Start() As Integer Implements aPMNav.NavigatorV3.Start

        Dim result As Integer = 0
        Const kMethodName As String = "NavigatorV3_Start"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            ' Call the corresponding Interface function
            m_lReturn = CType(m_oInterface.Start(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, "Failed to call NavigatorV3_Start", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function
    ' PUBLIC Methods (End)


    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class