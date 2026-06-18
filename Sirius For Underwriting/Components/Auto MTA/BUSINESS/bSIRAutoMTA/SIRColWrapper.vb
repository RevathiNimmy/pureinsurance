Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SSP.Shared
Imports System.Collections.ObjectModel
Friend NotInheritable Class ColWrapper

    Private Const ACClass As String = "ObjectKeys"

    'Private _m_cCollection As Collection(Of Object) = Nothing    'Saj

    Private _m_cCollection As Dictionary(Of Object, Integer) = Nothing


    Private Property m_cCollection() As Dictionary(Of Object, Integer)
        Get
            If _m_cCollection Is Nothing Then
                _m_cCollection = New Dictionary(Of Object, Integer)
            End If
            Return _m_cCollection
        End Get
        Set(ByVal Value As Dictionary(Of Object, Integer))
            _m_cCollection = Value
        End Set
    End Property


    'Private Property m_cCollection() As Collection(Of Object)
    '    Get
    '        If _m_cCollection Is Nothing Then
    '            _m_cCollection = New Collection(Of Object)
    '        End If
    '        Return _m_cCollection
    '    End Get
    '    Set(ByVal Value As Collection(Of Object))
    '        _m_cCollection = Value
    '    End Set
    'End Property

    ' ************************************************
    ' Added to replace global variables 26/11/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description:
    '
    ' History: 01/11/2000 sj - Created.
    '
    ' ***************************************************************** '
    Public Function Add(ByVal v_vItem As Object, Optional ByVal v_vKey As Object = Nothing, Optional ByRef r_vExists As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(v_vKey) Then
                m_cCollection.Add(v_vKey, v_vItem)
                'Else
                '    m_cCollection.Add(v_vItem)
            End If

            Return result

        Catch excep As System.Exception



            If Informations.Err().Number = 457 Then

                If Not Informations.IsNothing(r_vExists) Then
                    r_vExists = True
                End If
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description:
    '
    ' History: 01/11/2000 sj - Created.
    '
    ' ***************************************************************** '
    Public Function Item(ByVal v_vKey As Object, Optional ByRef r_vItem As Object = Nothing, Optional ByRef r_vExists As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If (m_cCollection.ContainsKey(v_vKey)) Then
                r_vItem = m_cCollection(v_vKey)
                r_vExists = True
                Else
                    r_vExists = False
                End If


                Return result

        Catch excep As System.Exception



            If Informations.Err().Number = 5 Or Informations.Err().Number = 9 Then

                If Not Informations.IsNothing(r_vExists) Then
                    r_vExists = False
                End If
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Item Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Remove
    '
    ' Description:
    '
    ' History: 01/11/2000 sj - Created.
    '
    ' ***************************************************************** '
    Public Function Remove(ByVal v_vKey As Object, Optional ByRef r_vExists As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(r_vExists) Then
                r_vExists = True
            End If

            m_cCollection.Remove(v_vKey)

            Return result

        Catch excep As System.Exception



            If Informations.Err().Number = 5 Or Informations.Err().Number = 9 Then

                If Not Informations.IsNothing(r_vExists) Then
                    r_vExists = False
                End If
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Remove Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Remove", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description:
    '
    ' History: 01/11/2000 sj - Created.
    '
    ' ***************************************************************** '
    Public Function Count(ByRef r_lCount As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lCount = m_cCollection.Count

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Protected Overrides Sub Finalize()
        m_cCollection = Nothing
    End Sub

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class