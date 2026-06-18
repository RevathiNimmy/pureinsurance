Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class Step_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Step
    '
    ' Date: 01/09/1998
    '
    ' Description: Describes the attributes for a single Navigator Step.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Step"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' DataBase Attributes
    Private m_lMapID As Integer
    Private m_lStepID As Integer
    Private m_lComponentID As Integer
    Private m_sComponentType As New FixedLengthString(2)
    Private m_sObjectName As String = ""
    Private m_sClassName As String = ""
    Private m_iIsServerSide As Integer
    Private m_lSubMapID As Integer
    Private m_iIsSubMap As Integer
    Private m_lTask As Integer
    Private m_sOkAction As New FixedLengthString(2)
    Private m_sCancelAction As New FixedLengthString(2)
    Private m_lOkNoOfSteps As Integer
    Private m_lCancelNoOfSteps As Integer
    Private m_lOkProcessID As Integer
    Private m_lCancelProcessID As Integer
    Private m_lNavigateStatus As gPMConstants.PMENavigateButtonStatus
    Private m_sCaption As New FixedLengthString(255)
    Private m_iIsHidden As Integer
    Private m_iIsLogged As Integer
    Private m_sStepKey As String = ""
    Private m_lItemNumber As Integer
    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.11.2)

    Private m_iResourceId As Integer
    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.11.2)

    ' Set Keys (Read Only)
    Private m_oSetKeys As iPMNavigator.Keys
    ' Get Keys (Read Only)
    Private m_oGetKeys As iPMNavigator.Keys

    Private m_lReturn As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property MapID() As Integer
        Get
            Return m_lMapID
        End Get
        Set(ByVal Value As Integer)
            m_lMapID = Value
        End Set
    End Property

    Public Property StepID() As Integer
        Get
            Return m_lStepID
        End Get
        Set(ByVal Value As Integer)
            m_lStepID = Value
        End Set
    End Property

    Public Property ComponentID() As Integer
        Get
            Return m_lComponentID
        End Get
        Set(ByVal Value As Integer)

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(Value), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_lComponentID = CInt(Value)
            Else
                m_lComponentID = 0
            End If
        End Set
    End Property

    Public Property ComponentType() As String
        Get
            Return m_sComponentType.Value.Trim()
        End Get
        Set(ByVal Value As String)

            If Not (Convert.IsDBNull(Value) Or IsNothing(Value)) Then

                m_sComponentType.Value = CStr(Value).Trim()
            Else
                m_sComponentType.Value = ""
            End If
        End Set
    End Property

    Public Property ObjectName() As String
        Get
            Return m_sObjectName.Trim()
        End Get
        Set(ByVal Value As String)

            If Not (Convert.IsDBNull(Value) Or IsNothing(Value)) Then

                m_sObjectName = CStr(Value).Trim()
            Else
                m_sObjectName = ""
            End If
        End Set
    End Property

    Public Property ClassName() As String
        Get
            Return m_sClassName.Trim()
        End Get
        Set(ByVal Value As String)

            If Not (Convert.IsDBNull(Value) Or IsNothing(Value)) Then

                m_sClassName = CStr(Value).Trim()
            Else
                m_sClassName = ""
            End If
        End Set
    End Property

    Public Property IsServerSide() As Integer
        Get
            Return m_iIsServerSide
        End Get
        Set(ByVal Value As Integer)

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(Value), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_iIsServerSide = CInt(Value)
            Else
                m_iIsServerSide = 0
            End If
        End Set
    End Property

    Public Property SubMapID() As Integer
        Get
            Return m_lSubMapID
        End Get
        Set(ByVal Value As Integer)

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(Value), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_lSubMapID = CInt(Value)
            Else
                m_lSubMapID = 0
            End If
            ' Set the IsSubMap property
            If m_lSubMapID < 1 Then
                IsSubMap = gPMConstants.PMEReturnCode.PMFalse
            Else
                IsSubMap = gPMConstants.PMEReturnCode.PMTrue
            End If
        End Set
    End Property

    Public Property IsSubMap() As Integer
        Get
            Return m_iIsSubMap
        End Get
        Set(ByVal Value As Integer)
            m_iIsSubMap = Value
        End Set
    End Property

    Public Property Task() As Integer
        Get
            Return m_lTask
        End Get
        Set(ByVal Value As Integer)

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(Value), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_lTask = CInt(Value)
            Else
                m_lTask = 0
            End If
        End Set
    End Property

    Public Property OkAction() As String
        Get
            Return m_sOkAction.Value.Trim()
        End Get
        Set(ByVal Value As String)

            If Not (Convert.IsDBNull(Value) Or IsNothing(Value)) Then

                m_sOkAction.Value = CStr(Value).Trim()
            Else
                m_sOkAction.Value = ""
            End If
        End Set
    End Property

    Public Property CancelAction() As String
        Get
            Return m_sCancelAction.Value.Trim()
        End Get
        Set(ByVal Value As String)

            If Not (Convert.IsDBNull(Value) Or IsNothing(Value)) Then

                m_sCancelAction.Value = CStr(Value).Trim()
            Else
                m_sCancelAction.Value = ""
            End If
        End Set
    End Property

    Public Property OkNoOfSteps() As Integer
        Get
            Return m_lOkNoOfSteps
        End Get
        Set(ByVal Value As Integer)

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(Value), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_lOkNoOfSteps = CInt(Value)
            Else
                m_lOkNoOfSteps = 0
            End If
        End Set
    End Property

    Public Property CancelNoOfSteps() As Integer
        Get
            Return m_lCancelNoOfSteps
        End Get
        Set(ByVal Value As Integer)

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(Value), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_lCancelNoOfSteps = CInt(Value)
            Else
                m_lCancelNoOfSteps = 0
            End If
        End Set
    End Property

    Public Property OkProcessID() As Integer
        Get
            Return m_lOkProcessID
        End Get
        Set(ByVal Value As Integer)

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(Value), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_lOkProcessID = CInt(Value)
            Else
                m_lOkProcessID = 0
            End If
        End Set
    End Property

    Public Property CancelProcessID() As Integer
        Get
            Return m_lCancelProcessID
        End Get
        Set(ByVal Value As Integer)

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(Value), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_lCancelProcessID = CInt(Value)
            Else
                m_lCancelProcessID = 0
            End If
        End Set
    End Property

    Public Property NavigateStatus() As String
        Get
            Return CStr(m_lNavigateStatus)
        End Get
        Set(ByVal Value As String)

            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                m_lNavigateStatus = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            Else
                If Information.VarType(Value) = VariantType.String Then
                    ' Convert Navigate Status from String in DB to Long expected by Component

                    Select Case CStr(Value).Trim()
                        Case "00"
                            m_lNavigateStatus = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
                        Case "01"
                            m_lNavigateStatus = gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                        Case "02"
                            m_lNavigateStatus = gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                        Case Else
                            m_lNavigateStatus = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
                    End Select
                Else
                    m_lNavigateStatus = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
                End If
            End If
        End Set
    End Property

    Public Property Caption() As String
        Get
            Return m_sCaption.Value.Trim()
        End Get
        Set(ByVal Value As String)
            m_sCaption.Value = Value.Trim()
        End Set
    End Property
    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.11.2)
    Public Property ResourceId() As Integer
        Get
            Return m_iResourceId
        End Get
        Set(ByVal Value As Integer)
            m_iResourceId = Value
        End Set
    End Property
    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.11.2)

    Public Property IsHidden() As Integer
        Get
            Return m_iIsHidden
        End Get
        Set(ByVal Value As Integer)
            m_iIsHidden = Value
        End Set
    End Property

    Public Property IsLogged() As Integer
        Get
            Return m_iIsLogged
        End Get
        Set(ByVal Value As Integer)
            m_iIsLogged = Value
        End Set
    End Property

    Public Property StepKey() As String
        Get
            Return m_sStepKey.Trim()
        End Get
        Set(ByVal Value As String)
            m_sStepKey = Value.Trim()
        End Set
    End Property

    Public Property ItemNumber() As Integer
        Get
            Return m_lItemNumber
        End Get
        Set(ByVal Value As Integer)
            m_lItemNumber = Value
        End Set
    End Property


    Public Property SetKeys() As iPMNavigator.Keys
        Get
            Return m_oSetKeys
        End Get
        Set(ByVal Value As iPMNavigator.Keys)

            m_oSetKeys = Value

        End Set
    End Property

    Public Property GetKeys() As iPMNavigator.Keys
        Get
            Return m_oGetKeys
        End Get
        Set(ByVal Value As iPMNavigator.Keys)

            m_oGetKeys = Value

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

            ' Initialisation Code.

            ' New SetKeys Collection
            m_oSetKeys = New iPMNavigator.Keys()
            'developer guide no. 9
            m_lReturn = m_oSetKeys.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' New GetKeys Collection
            m_oGetKeys = New iPMNavigator.Keys()
            'developer guide no. 9
            m_lReturn = m_oGetKeys.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oSetKeys IsNot Nothing Then
                    m_oSetKeys.Dispose()
                    m_oSetKeys = Nothing
                End If
                If m_oGetKeys IsNot Nothing Then
                    m_oGetKeys.Dispose()
                    m_oGetKeys = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
