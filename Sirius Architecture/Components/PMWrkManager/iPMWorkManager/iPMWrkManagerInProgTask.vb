Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles

Friend NotInheritable Class InProgTask 
	  Implements IDisposable
' ***************************************************************** '
	' Class Name: InProgTask
	'
	' Date: 11/11/1998
	'
	' Description: An instance of Navigator
	'
	'
	' Edit History:
	' ***************************************************************** '
	
	' Constant for the functions to identify which class this is.
Private Const ACClass As String = "InProgTask" 
	
	Private lReturn As Integer

    Private WithEvents m_oNavigator As iPMNavigator.NavigateControl
    ' RDC 15012003
    Private WithEvents m_oNavigatorXM As iPMNavigatorXM.Interface_Renamed
    Private WithEvents m_oComponent As iPMWrkComponentStarter.StartControl
    Private m_oParent As PMWorkManager.InProgTasks
    Private m_lPMWrkTaskInstanceCnt As Integer
    Private m_bIsNavigatorInstance As Boolean
    Private m_bStatusUpdated As Boolean
    Private m_sKey As String = ""
    ' RDC 14012003
    Private m_sNavXMLfile As String = ""
    Private m_bFromSchedule As Boolean 'AR20050428 - PN7388

    'AR20050428 - PN7388

    Public Property FromSchedule() As Boolean
        Get
            Return m_bFromSchedule
        End Get
        Set(ByVal Value As Boolean)
            m_bFromSchedule = Value
        End Set
    End Property

    Public Property Navigator() As iPMNavigator.NavigateControl
        Get
            Return m_oNavigator
        End Get
        Set(ByVal Value As iPMNavigator.NavigateControl)
            'If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is String) Then
            m_oNavigator = Value
            'Else
            'm_oNavigator = Value
            'End If
        End Set
    End Property

    ' RDC 15012003
    Public Property NavigatorXM() As iPMNavigatorXM.Interface_Renamed
        Get
            Return m_oNavigatorXM
        End Get
        Set(ByVal Value As iPMNavigatorXM.Interface_Renamed)
            'TODO
            'If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is string) Then
            '	m_oNavigatorXM = Value
            'Else
            m_oNavigatorXM = Value
            'End If
        End Set
    End Property

    Public Property Component() As iPMWrkComponentStarter.StartControl
        Get
            Return m_oComponent
        End Get
        Set(ByVal Value As iPMWrkComponentStarter.StartControl)
            'If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is String) Then
            '    m_oComponent = Value
            'Else
            m_oComponent = Value
            'End If
        End Set
    End Property

    Public Property Parent() As PMWorkManager.InProgTasks
        Get
            Return m_oParent
        End Get
        Set(ByVal Value As PMWorkManager.InProgTasks)
            'Tarun modified
            'If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is string) Then
            '	m_oParent = Value
            'Else
            '	m_oParent = Value
            'End If
            m_oParent = Value

        End Set
    End Property

    Public Property PMWrkTaskInstanceCnt() As Integer
        Get
            Return m_lPMWrkTaskInstanceCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPMWrkTaskInstanceCnt = Value
        End Set
    End Property

    Public Property IsNavigatorInstance() As Boolean
        Get
            Return m_bIsNavigatorInstance
        End Get
        Set(ByVal Value As Boolean)
            m_bIsNavigatorInstance = Value
        End Set
    End Property

    Public Property StatusUpdated() As Boolean
        Get
            Return m_bStatusUpdated
        End Get
        Set(ByVal Value As Boolean)
            m_bStatusUpdated = Value
        End Set
    End Property

    Public Property Key() As String
        Get
            Return m_sKey.Trim()
        End Get
        Set(ByVal Value As String)
            m_sKey = Value.Trim()
        End Set
    End Property

    ' RDC 14012003
    Public Property NavXMLfile() As String
        Get
            Return m_sNavXMLfile
        End Get
        Set(ByVal Value As String)
            m_sNavXMLfile = Value
        End Set
    End Property
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal v_bIsNavigatorInstance As Boolean) As Integer

        Dim result As Integer = 0
        Dim sWhatFailed As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            IsNavigatorInstance = v_bIsNavigatorInstance
            StatusUpdated = False

            ' Is this is a Navigator Instance
            If Not v_bIsNavigatorInstance Then

                ' No, so Create Component Starter
                sWhatFailed = "Component Starter"

                Navigator = Nothing
                Component = New iPMWrkComponentStarter.StartControl()

                lReturn = Component.Initialise()

                Component.CallingAppName = ACApp

            Else

                ' RDC 140012003
                'Tracy Richards 23/10/03 Trim the Nav file as it can be a padded blank string
                If NavXMLfile.Trim() = "" Then
                    ' it's an old-style Navigator
                    sWhatFailed = "Navigator"

                    Component = Nothing

                    Navigator = New iPMNavigator.NavigateControl()

                    lReturn = Navigator.Initialise()

                    Navigator.CallingAppName = ACApp
                Else
                    sWhatFailed = "Navigator XM"

                    Component = Nothing
                    NavigatorXM = New iPMNavigatorXM.Interface_Renamed()

                    lReturn = NavigatorXM.Initialise()

                    NavigatorXM.CallingAppName = ACApp
                End If

            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Initialise : " & sWhatFailed, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to create an instance of : " & sWhatFailed, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' History    : Kevin Renshaw (CMG) - Terminate correct NavigatorXM
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

                Parent = Nothing

                ' Terminate Navigator/Component
                If IsNavigatorInstance Then
                    'Tracy Richards 23/10/03 Trim the Nav file as it can be a padded blank string
                    If NavXMLfile.Trim() = "" Then
                        Navigator.Dispose()
                        Navigator = Nothing
                    Else
                        NavigatorXM.Dispose()
                        NavigatorXM = Nothing
                    End If
                Else
                    Component.Dispose()
                    Component = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
	End Sub


	Protected Overrides Sub Finalize()
		Dispose(False)
	End Sub

	
	Private Sub m_oComponent_Finished(ByVal v_bComplete As Boolean) Handles m_oComponent.Finished
		
		'AR20050428 - PN7388 If component called from scheduled tasks prompt to see if complete
		'RKS PN28020
		If m_bFromSchedule And (Not v_bComplete) Then
			v_bComplete = MessageBox.Show("Has the task been completed?", ACMainFormCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes
		End If
		
		' The Process is either Complete or Incomplete.
		Parent.UpdateStatus(PMWrkTaskInstanceCnt, v_bComplete)
		
		' So we know that we have updated the Task Status for this Instance.
		StatusUpdated = True
		
		' Delete Myself
		Parent.Delete(Key)
		
	End Sub
    'TODO
    Private Sub m_oNavigator_NavigatorClose() Handles m_oNavigator.NavigatorClose

        Dim sKey As String = ""

        ' Delete Myself
        Parent.Delete(Key)

    End Sub
    'TODO
    Private Sub m_oNavigator_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavigator.SetProcessStatus

        ' The Process is either Complete or Incomplete.
        Parent.UpdateStatus(PMWrkTaskInstanceCnt, v_bProcessComplete)

        ' So we know that we have updated the Task Status for this Instance.
        StatusUpdated = True

    End Sub
	
	' RDC 15012003
	Private Sub m_oNavigatorXM_NavigatorClose() Handles m_oNavigatorXM.NavigatorClose
		
		Dim sKey As String = ""
		
		' Delete Myself
		Parent.Delete(Key)
		
	End Sub
	
	Private Sub m_oNavigatorXM_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavigatorXM.SetProcessStatus
		
		' The Process is either Complete or Incomplete.
		Parent.UpdateStatus(PMWrkTaskInstanceCnt, v_bProcessComplete)
		
		' So we know that we have updated the Task Status for this Instance.
		StatusUpdated = True
		
	End Sub
End Class
