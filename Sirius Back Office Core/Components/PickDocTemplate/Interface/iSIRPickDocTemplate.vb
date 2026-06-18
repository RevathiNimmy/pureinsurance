Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    '********************************************************************************
    'Created By:Arul Stephen
    'Tech Spec:TechSpec WR6ClauseGrouping.doc
    '********************************************************************************

    Private Const ACClass As String = "iSIRPickDocTemplate"

    Private m_lSourceID As Integer
    Private m_lProductID As Integer
    Private m_lClauseId As Integer
    Private m_lRiskId As Integer
    Private m_lStatus As Integer
    Private m_lReturn As Integer
    Private m_lTask As gPMConstants.PMEComponentAction
    Private m_lKeepOnTop As Integer
    Private m_lParentHwnd As Integer

    'Start Arul -Bug Fixing PN 55217
    Private m_sColumnName As String = ""
    Private m_sPropertyName As String = ""
    'End Arul -Bug Fixing PN 55217

    Private m_vDocumentTemplate As Object
    'Start(Sriram P)PN60826
    Private m_vDefaultClauses As Object
    Private m_bSearchable As Boolean

    Public WriteOnly Property Searchable() As Boolean
        Set(ByVal Value As Boolean)
            m_bSearchable = Value
        End Set
    End Property
    Public Property DefaultClauses() As Object
        Get
            Return m_vDefaultClauses
        End Get
        Set(ByVal Value As Object)


            m_vDefaultClauses = Value
        End Set
    End Property
    'End(Sriram P)PN60826

    Private m_sCoverToDate As String
    Public Property CoverToDate() As String
        Get
            Return m_sCoverToDate
        End Get
        Set(ByVal Value As String)
            m_sCoverToDate = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property
    Public ReadOnly Property Task() As Integer
        Get
            Return m_lTask
        End Get
    End Property

    Public Property SourceId() As Integer
        Get
            Return m_lSourceID
        End Get
        Set(ByVal Value As Integer)
            m_lSourceID = Value
        End Set
    End Property

    Public Property ProductId() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property

    Public WriteOnly Property ClauseId() As Integer
        Set(ByVal Value As Integer)
            m_lClauseId = Value
        End Set
    End Property
    Public ReadOnly Property lClauseId() As Integer
        Get
            ClauseId = m_lClauseId
        End Get
    End Property

    Public Property RiskId() As Integer
        Get
            Return m_lRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

    Public Property DocumentTemplate() As Object
        Get
            Return m_vDocumentTemplate
        End Get
        Set(ByVal Value As Object)


            m_vDocumentTemplate = Value
        End Set
    End Property
    'Start Arul -Bug Fixing PN 55217
    Public Property PropertyName() As String
        Get
            Return m_sPropertyName
        End Get
        Set(ByVal Value As String)
            m_sPropertyName = Value
        End Set
    End Property
    Public Property ColumnName() As String
        Get
            Return m_sColumnName
        End Get
        Set(ByVal Value As String)
            m_sColumnName = Value
        End Set
    End Property
    'End Arul -Bug Fixing PN 55217
    'This method will create the instance for the
    'business "bSIRFindDocTemplate.Form"
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"
        Dim sMessage, sTitle As String
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            MainModule.g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = MainModule.g_oObjectManager.Initialise(MainModule.ACApp)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse
                'Set the object manager to nothing.
                MainModule.g_oObjectManager = Nothing
                gPMFunctions.RaiseError(kMethodName, "g_oObjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With MainModule.g_oObjectManager
                MainModule.g_iLanguageID = .LanguageID
                MainModule.g_iSourceID = .SourceID
                MainModule.g_iUserID = .UserID
            End With

            ' Initialise the process modes.
            m_lTask = gPMConstants.PMEComponentAction.PMView
            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object = Nothing
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRFindDocTemplate.Form", "ClientManager")
            MainModule.g_oBusiness = temp_g_oBusiness


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACBusinessFailTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACBusinessFail, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
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

    'This method will Terminate the object(s)
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If Not (MainModule.g_oBusiness Is Nothing) Then
                    ' Terminate the business object

                    MainModule.g_oBusiness.Dispose()
                     
                    MainModule.g_oBusiness = Nothing
                End If
                If MainModule.g_oObjectManager IsNot Nothing Then
                    MainModule.g_oObjectManager.Dispose()
                    MainModule.g_oObjectManager = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetProcessModes"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Information.IsNothing(vTask) Then

                m_lTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
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

    'This method will really start the business
    Public Function Start() As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "Start"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessInterface method Failed", gPMConstants.PMELogLevel.PMLogError)
                result = gPMConstants.PMEReturnCode.PMFalse
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
    'developer guide no.50
    Dim frmPickDocumentTemplate As frmPickDocumentTemplate
    'This method will do the load, show and unload the interface
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessInterface"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Load the interface into memory.
            m_lReturn = LoadInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "LoadInterface method Failed", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lKeepOnTop = 1 Then
                m_lReturn = iPMFunc.SetWindowPlacement(frmPickDocumentTemplate.Handle.ToInt32(), True)
            End If

            ' Display the interface.
            m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ShowInterface method Failed", gPMConstants.PMELogLevel.PMLogError)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Destroy the interface from memory.
            m_lReturn = UnLoadInterface()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UnLoadInterface method Failed", gPMConstants.PMELogLevel.PMLogError)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End Try
        Return result

    End Function


    'This method will load the page
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadInterface"
        Try
            'developer guide no. 69
            frmPickDocumentTemplate = New frmPickDocumentTemplate


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the parameters to the interface properties.
            With frmPickDocumentTemplate
                .Task = m_lTask
                .ProductId = m_lProductID
                .SourceId = m_lSourceID
                .RiskId = m_lRiskId
                .ClauseId = m_lClauseId
                .PropertyName = m_sPropertyName
                .ColumnName = m_sColumnName


                .DefaultClauses = m_vDefaultClauses
                .CoverToDate = m_sCoverToDate
                .Searchable = m_bSearchable
            End With

            ' Load the instance of the interface into memory.
            Dim tempLoadForm As frmPickDocumentTemplate = frmPickDocumentTemplate
            m_lParentHwnd = tempLoadForm.FindForm().Handle.ToInt32()
        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End Try
        Return result

    End Function

    'Once all the process is done then the interface will gets unloaded from this form
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UnLoadInterface"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the property members from the interface parameters.
            With frmPickDocumentTemplate
                m_lStatus = .Status
                m_lTask = .Task
            End With

            ' Unload and destroy the instance of the interface
            ' from memory.
            frmPickDocumentTemplate.Close()
            frmPickDocumentTemplate = Nothing

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End Try
        Return result

    End Function

    'This method will display the form
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Const kMethodName As String = "ShowInterface"
        Try
            Catch_Renamed = True



            result = gPMConstants.PMEReturnCode.PMTrue
            Dim vDocumentTempate As Object

            ' Display the interface.
            VB6.ShowForm(frmPickDocumentTemplate, lDisplayState)

            If lDisplayState = FormShowConstants.Modal Then
                ' Check for any form errors.
                If frmPickDocumentTemplate.ErrorNumber <> 0 Then
                    result = frmPickDocumentTemplate.ErrorNumber
                End If
            End If



            vDocumentTempate = frmPickDocumentTemplate.DocumentTemplate


            m_vDocumentTemplate = vDocumentTempate

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            If Catch_Renamed Then


                ' Do Not Call any functions before here or the error will be lost
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

                ' If you want to rollback a transaction or something, do it here

            End If
Finally_Renamed:
        End Try
    End Function


    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()

                    Case PMNavKeyConst.PMKeyNameKeepWindowOnTop

                        m_lKeepOnTop = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ReDim vKeyArray(1, 1)
            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
