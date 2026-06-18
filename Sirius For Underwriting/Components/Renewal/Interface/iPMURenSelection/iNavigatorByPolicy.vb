Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("NavigatorByPolicy_NET.NavigatorByPolicy")>
Public NotInheritable Class NavigatorByPolicy
    Implements IDisposable
    Implements aPMNav.NavigatorV3

    ' ***************************************************************** '
    ' Class Name: NavigatorByPolicy
    '
    ' Date: 16/10/2002
    '
    ' Description: Used to set mode for NavigatorV3 class.
    '
    ' Edit History:
    ' ***************************************************************** '




    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "NavigatorByPolicy"

    ' Stores the exit status
    Private m_lStatus As Integer

    ' Stores the Navigator Cancel action
    Private m_sPMNavCancelAction As String = ""

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    'Developer Guide No. 50
    Dim frmInterface As frmInterface

    Private m_oNav As Object

    Private ReadOnly Property oNavObject() As aPMNav.NavigatorV3
        Get
            Return m_oNav
        End Get
    End Property

    Public WriteOnly Property NavigatorV3_CallingAppName() As String Implements aPMNav.NavigatorV3.CallingAppName
        Set(ByVal Value As String)
            oNavObject.CallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property NavigatorV3_PMAuthorityLevel() As Integer Implements aPMNav.NavigatorV3.PMAuthorityLevel
        Set(ByVal Value As Integer)
            oNavObject.PMAuthorityLevel = Value
        End Set
    End Property

    Public ReadOnly Property NavigatorV3_Status() As Integer Implements aPMNav.NavigatorV3.Status
        Get
            Return oNavObject.Status
        End Get
    End Property


    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No. 50
            m_oNav = New NavigatorV3()
            'Developer Guide No. 9
            m_lReturn = m_oNav.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the Interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                If m_oNav IsNot Nothing Then
                    m_oNav.Dispose()
                    m_oNav = Nothing
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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the corresponding Interface function
            m_lReturn = CType(oNavObject.SetKeys(vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.SetKeys failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = CType(oNavObject.GetKeys(vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.GetKeys failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = CType(oNavObject.GetSummary(vSummaryArray:=vSummaryArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.GetSummary failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer Implements aPMNav.NavigatorV3.SetProcessModes

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = CType(oNavObject.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.SetProcessModes failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_Start() As Integer Implements aPMNav.NavigatorV3.Start

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No.50
            objfrmInterface = New frmInterface
            'Show only the policy tab
            SSTabHelper.SetTabVisible(objfrmInterface.tabMainTab, 0, False)
            SSTabHelper.SetTabVisible(objfrmInterface.tabMainTab, 1, True)

            ' Call the corresponding Interface function
            m_lReturn = CType(oNavObject.Start(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.Start failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)

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
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
