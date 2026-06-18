Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Imports System.Threading
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("StartControl_NET.StartControl")> _
Public NotInheritable Class StartControl
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 02/12/1998
    '
    ' Description: Main Public Creatable Class.
    '
    ' Edit History:
    ' DAK151099 - Get keys from called function
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    Private WithEvents m_fMainForm As frmStartComponent
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""


    Private m_sComponent As String = ""
    Private m_lPMAuthorityLevel As Integer
    'Private m_vSetKeyArray As String = ""
    Private m_vSetKeyArray As Object
    ' GetKeyArray
    Private m_vGetKeyArray As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' {* USER DEFINED CODE (Begin) *}
    ' Finished Event
    Public Event Finished(ByVal v_bComplete As Boolean)
    ' {* USER DEFINED CODE (End) *}

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property GetKeyArray() As Object
        Get
            Return m_vGetKeyArray
        End Get
        Set(ByVal Value As Object)


            m_vGetKeyArray = Value
        End Set
    End Property

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then

                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                End If

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Load the Form
            m_fMainForm = New frmStartComponent()


            Return result

        Catch excep As System.Exception



            ' Error Section.

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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If Not (m_fMainForm Is Nothing) Then
                    ' Destroy the Form as we do not need it anymore.
                    m_fMainForm.Close()
                    m_fMainForm = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: StartComponent
    '
    ' Description: Set the Properties of the component to start, Load
    '              the timer form and return control immediately.
    ' ***************************************************************** '
    'developer guide no. 17
    Public Function StartComponent(ByVal v_sComponent As String, ByVal v_lPMAuthorityLevel As Integer, Optional ByVal v_vSetKeyArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Component To Call
            m_sComponent = v_sComponent

            ' Authority Level
            m_lPMAuthorityLevel = v_lPMAuthorityLevel


            If Not Information.IsNothing(v_vSetKeyArray) Then
                If Information.IsArray(v_vSetKeyArray) Then
                    m_vSetKeyArray = v_vSetKeyArray
                Else
                    m_vSetKeyArray = ""
                End If
            Else
                m_vSetKeyArray = ""
            End If

            ' Enable the Timer
            m_fMainForm.tmrStartComponent.Enabled = True

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to StartComponent the object", vApp:=ACApp, vClass:=ACClass, vMethod:="StartComponent", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StartSynchronous
    '
    ' Description: Set the Properties of the component to start, Load
    '              the timer form and return control immediately.
    ' ***************************************************************** '
    ' developer guide no. 17
    Public Function StartSynchronous(ByVal v_sComponent As String, ByVal v_lPMAuthorityLevel As Integer, Optional ByVal v_vSetKeyArray As Object = Nothing, Optional ByRef o_aGetKeyArray As Object = Nothing) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMFalse
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Component To Call
            m_sComponent = v_sComponent

            ' Authority Level
            m_lPMAuthorityLevel = v_lPMAuthorityLevel

            If Information.IsArray(v_vSetKeyArray) Then
                m_vSetKeyArray = v_vSetKeyArray
            Else
                m_vSetKeyArray = ""
            End If

            ' Do not use the form as we are not working asyncronously
            Start()

            o_aGetKeyArray = m_vGetKeyArray

            Return nResult

        Catch ex As System.Exception



            ' Error Section.

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to StartSynchronous the object", vApp:=ACApp, vClass:=ACClass, vMethod:="StartSynchronous", excep:=ex)

            Return nResult

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to Start its processing.
    '
    ' ***************************************************************** '
    Private Sub Start()
        'Developer Guide (As Per VB6 Code)
        'Dim oComponent, oNav3 As aPMNav.NavigatorV3

        Dim oComponent As Object
        Dim oNav3 As aPMNav.NavigatorV3
        Dim lStatus As gPMConstants.PMEReturnCode



        ' Get Instance of Component

        m_lReturn = g_oObjectManager.GetInstance(oobject:=oComponent, sclassname:=m_sComponent, vinstancemanager:=gPMConstants.PMGetLocalInterface)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseEvent Finished(False)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Get Instance of Component - " & m_sComponent, vApp:=ACApp)
            Exit Sub
        End If

        oNav3 = oComponent

        ' Set the Calling App
        oNav3.CallingAppName = ACApp

        ' Set the Authority Level
        oNav3.PMAuthorityLevel = m_lPMAuthorityLevel

        ' Call the Set Keys if needed
        If Information.IsArray(m_vSetKeyArray) Then
            m_lReturn = oNav3.SetKeys(m_vSetKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseEvent Finished(False)
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Set Keys for Component - " & m_sComponent, vApp:=ACApp)
                Exit Sub
            End If
        End If

        ' Start
        m_lReturn = oNav3.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseEvent Finished(False)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Component - " & m_sComponent, vApp:=ACApp)
            oNav3 = Nothing
            Exit Sub
        End If

        ' Get the OK/Cancel Status
        lStatus = oNav3.Status

        'DAK151099
        m_lReturn = oNav3.GetKeys(m_vGetKeyArray)

        ' Terminate the Component

        oComponent.Dispose()
        oComponent = Nothing
        oNav3 = Nothing

        ' Raise the Finished Event
        If lStatus = gPMConstants.PMEReturnCode.PMOK Then
            RaiseEvent Finished(True)
        Else
            RaiseEvent Finished(False)
        End If



    End Sub
    ' PRIVATE Methods (End)
    Private Sub m_fMainForm_StartComponent() Handles m_fMainForm.StartComponent


        Dim newthread As System.Threading.Thread

        newthread = New Thread(AddressOf openapplication)

        newthread.SetApartmentState(ApartmentState.STA)

        newthread.IsBackground = True
        'newthread.Start(newapp)
        newthread.Start()

        ' Start the Component
        'Start()

    End Sub

    Sub openapplication()
        Initialise()
        Start()
        'developer guide no. 106 (Guide)
        Application.Run()
    End Sub
End Class

