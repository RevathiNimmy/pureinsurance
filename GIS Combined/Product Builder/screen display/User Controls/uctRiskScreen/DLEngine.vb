Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No. 129
Imports SharedFiles


<System.Runtime.InteropServices.ProgId("DLEngine_NET.DLEngine")> _
Public NotInheritable Class DLEngine

    Implements IDisposable
    ' This module provides the public interface to the RiskScreen control for calls from dynamic logic
    ' All methods and properties should delegate to the RiskScreen control for action

    ' RAW 09/07/2004 : JIT : created


    ' These constants should have binary values (eg 1,2,4,8,16 etc )
#Const m_klDebugOption_Normal = 1
#Const m_klDebugOption_Events = 2
#Const m_klDebugOption_Script = 4
#Const m_klDebugOption_DLCallBack = 8
#Const m_klDebugOption_JIT = 16


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctRiskScreenControl"

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_oRiskScreenControl As RiskScreen

    'PLICO14
    Private m_oPartyDataSet As Object 'The Party data Engine
    Private m_lPartyDataCnt As Integer

    Public ReadOnly Property RiskTypeId() As Integer
        Get
            ' delegate to the RiskScreen control
            Return m_oRiskScreenControl.RiskTypeId
        End Get
    End Property

    'PLICO14 Give access to the Party data
    Public ReadOnly Property PartyDataEngine(ByVal lPartyCnt As Integer) As Object
        Get

            Dim result As Object = Nothing
            If m_oPartyDataSet Is Nothing Or m_lPartyDataCnt <> lPartyCnt Then
                m_lReturn = CType(m_oRiskScreenControl.LoadPartyDataEngine(lPartyCnt, m_oPartyDataSet), gPMConstants.PMEReturnCode)
                If m_oPartyDataSet Is Nothing Or m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return Nothing
                End If
            End If


            result = m_oPartyDataSet.Risk

            m_lPartyDataCnt = lPartyCnt
            Return result
        End Get
    End Property



    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Friend Function Initialise(ByVal v_oRiskScreenControl As RiskScreen) As Integer

        Dim result As Integer = 0
        Static bIsInitialised As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If


            m_oRiskScreenControl = v_oRiskScreenControl

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result



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
                m_oRiskScreenControl = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    '
    ' Name: GetValue
    '
    ' Description: Gets the value of a control given the object.attribute name as a string
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function GetValue(ByVal sName As String) As Object

        Dim result As Object = Nothing
        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : GetValue - " & sName)
#End If

        Try

            ' delegate to the RiskScreen control
            result = m_oRiskScreenControl.GetValue(sName:=sName)



        Catch ex As Exception

            result = ""

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetValue Failed for " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetValue", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
		If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetCaption
    '
    ' Description: Sets the caption of a control given the object.attribute name as a string
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Sub SetCaption(ByVal sName As String, ByRef vValue As Object)

        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : SetCaption - " & sName)
#End If

        Try


            ' delegate to the RiskScreen control
            m_oRiskScreenControl.SetCaption(sName:=sName, vValue:=vValue)




        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetCaption Failed for " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="SetCaption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
		If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    ' ***************************************************************** '
    '
    ' Name: SetValue
    '
    ' Description: Sets the value of a control given the object.attribute name as a string
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Sub SetValue(ByVal sName As String, ByVal vValue As Object)

        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : SetValue - " & sName)
#End If

        Try


            ' delegate to the RiskScreen control
            m_oRiskScreenControl.SetValue(sName:=sName, vValue:=vValue, vQuiet:=gPMConstants.PMEReturnCode.PMFalse)




        Catch ex As Exception

            ' Log Error Message

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetValue Failed for " & sName & " value=" & CStr(vValue), vApp:=ACApp, vClass:=ACClass, vMethod:="SetValue", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
		If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    ' ***************************************************************** '
    '
    ' Name: SetFocus
    '
    ' Description: Sets the focus to a control given the object.attribute name as a string
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function SetFocus(ByVal sName As String) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : SetFocus - " & sName)
#End If

        Try



            ' delegate to the RiskScreen control
            result = m_oRiskScreenControl.SetFocus(sName:=sName)




        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFocus Failed for " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="SetFocus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
		If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetMandatory
    '
    ' Description: Sets the mandatory status of a control given the object.attribute name as a string
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function SetMandatory(ByVal sName As String, ByVal vMode As Object, Optional ByVal vOverride As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : SetMandatory - " & sName)
#End If

        Try


            ' delegate to the RiskScreen control


            result = m_oRiskScreenControl.SetMandatory(sName:=sName, vMode:=CBool(vMode), vOverride:=CBool(vOverride))




        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetMandatory Failed for " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="SetMandatory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
		If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetVisibility
    '
    ' Description: Sets the visibility of a control given the object.attribute name as a string
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function SetVisibility(ByVal sName As String, ByVal vMode As Object) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : SetVisibility - " & sName)
#End If

        Try


            ' delegate to the RiskScreen control

            result = m_oRiskScreenControl.SetVisibility(sName:=sName, vMode:=CBool(vMode))



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetVisibility Failed for " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="SetVisibility", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
		If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetReadOnly
    '
    ' Description: Sets the ReadOnly status of a control given the object.attribute name as a string
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function SetReadOnly(ByVal sName As String, ByVal vMode As Object, Optional ByRef vOverride As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : SetReadOnly - " & sName)
#End If

        Try


            ' delegate to the RiskScreen control


            result = m_oRiskScreenControl.SetReadOnly(sName:=sName, vMode:=(vMode), vOverride:=(vOverride))



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetReadOnly Failed for " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="SetReadOnly", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
		If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: SetMandatoryColor
    '
    ' Description: Sets the color used for the background of mandatory controls
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function SetMandatoryColor(ByRef iRed As Integer, ByRef iGreen As Integer, ByRef iBlue As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : SetMandatoryColor")
#End If

        Try


            ' delegate to the RiskScreen control
            result = m_oRiskScreenControl.SetMandatoryColor(iRed:=iRed, iGreen:=iGreen, iBlue:=iBlue)



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetColor Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetMandatoryColor", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
		If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function





    '*****************************************************************
    '
    ' Name: GetColumnTotal
    '
    ' Description: Gets the total value of a sepcified column of a list view
    '              given the object.attribute name as a string and the column
    '              as a 1 based value
    '
    ' History:
    ' ***************************************************************** '
    Public Function GetColumnTotal(ByVal sName As String, ByVal sColumnName As String) As Object

        'Dim result As String = String.Empty
        Dim result As Object
        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : GetColumnTotal - " & sName & "." & sColumnName)
#End If

        Try

            ' delegate to the RiskScreen control
            result = m_oRiskScreenControl.GetColumnTotal(sName:=sName, sColumnName:=sColumnName)




        Catch ex As Exception

            result = ""

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetColumnTotal Failed for " & sName & "." & sColumnName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetColumnTotal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: PassValueToChild
    '
    ' Description: Saves a value into an array that can then be passed to a
    '              child screen
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function PassValueToChild(ByVal v_sChildName As String, ByVal v_sValueName As String, ByVal v_vValue As Object) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : PassValueToChild - " & v_sChildName & "." & v_sValueName)
#End If

        Try


            ' delegate to the RiskScreen control

            result = m_oRiskScreenControl.PassValueToChild(v_sChildName:=v_sChildName, v_sValueName:=v_sValueName, v_vValue:=CStr(v_vValue))



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PassValueToChild Failed for " & v_sValueName, vApp:=ACApp, vClass:=ACClass, vMethod:="PassValueToChild", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
		If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function



    Public Function GetListViewValue(ByVal v_sName As String, ByVal v_sColumnName As String, ByVal v_lRow As Integer) As String
        Dim result As String = String.Empty
        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : GetListViewValue - " & v_sName & " " & v_sColumnName & " " & v_lRow)
#End If

        Try


            ' delegate to the RiskScreen control
            result = m_oRiskScreenControl.GetListViewValue(sName:=v_sName, sColumnName:=v_sColumnName, lRow:=v_lRow)



        Catch ex As Exception

            result = CStr(gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListViewValue Failed for " & v_sName & " " & v_sColumnName & " " & CStr(v_lRow), vApp:=ACApp, vClass:=ACClass, vMethod:="GetListViewValue", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
		If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: SetListViewColumnCaption
    '
    ' Description: Sets the caption (and optionally the width) of a list view column
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function SetListViewColumnCaption(ByVal v_sName As String, ByVal v_sColumnName As String, ByVal v_sValue As String, Optional ByVal v_vWidth As Object = Nothing) As String

        Dim result As String = String.Empty
        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_DLCallBack) = m_klDebugOption_DLCallBack Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="DLCALL : SetListViewColumnCaption - " & v_sColumnName)
#End If

        Try


            ' delegate to the RiskScreen control
            result = m_oRiskScreenControl.SetListViewColumnCaption(v_sName:=v_sName, v_sColumnName:=v_sColumnName, v_sValue:=v_sValue, v_vWidth:=v_vWidth)




        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetListViewColumnCaption Failed for " & v_sName & " " & v_sColumnName, vApp:=ACApp, vClass:=ACClass, vMethod:="SetListViewColumnCaption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
		If lDebugDepthCounter > 0 Then AddToDebug(r_lDepthCounter:=lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function
End Class
