Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'Developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed

    Implements IDisposable
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' Date :15/07/2000
    '
    ' Edit History: SK
    ' ***************************************************************** '

    Private Const ACClass As String = "Interface"

    ' Private instance of the interface form.
    Private m_frmInterface As frmInterface
    Dim frmInterface As frmInterface

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer
    Private m_lReturn As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    'Jes 02 October 2002 ---
    'added to replace the user defined type below also used in bGISGroupImport
    Private Const ACABICode As Integer = 0
    Private Const ACGroupCode As Integer = 1
    'Developer guide no. 50
    Dim objfrmInterface As frmInterface

    Public Structure GroupRecord
        Dim ABICode As String
        Dim GroupCode As String
        Public Shared Function CreateInstance() As GroupRecord
            Dim result As New GroupRecord
            result.ABICode = String.Empty
            result.GroupCode = String.Empty
            Return result
        End Function
    End Structure

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            m_sCallingAppName = Value

        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Function Initialise() As Integer




        Dim result As Integer = 0
        Dim sMessage, sTitle As String

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

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bGisGroupImport.Business", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.
                sTitle = "Business Object"

                sMessage = "Failed to create business object"

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            ' Store the instance of the form into the member.
            'Developer guide no. 50

            m_frmInterface = objfrmInterface
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
    ' Date :15/07/2000
    '
    ' Edit History:SK
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
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                m_frmInterface = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' Date :15/07/2000
    '
    ' Edit History :SK
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            ' Set the process modes for the business object.
            If Not (g_oBusiness Is Nothing) Then

                m_lReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' Date :15/07/2000
    '
    ' Edit History :SK
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Carry on without defaults set
            End If

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' Date :15/07/2000
    '
    ' Edit History:SK
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = LoadInterface()


        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' Date :15/07/2000
    '
    ' Edit History:SK
    '***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.
        With m_frmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

        End With

        ' Load the instance of the interface into memory.
        'Developer guide no. 50
        Dim tempLoadForm As frmInterface = objfrmInterface
        objfrmInterface.InterfaceObj = Me

        ' Check if we have had an error so far.
        If m_frmInterface.ErrorNumber <> gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_frmInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' Date :15/07/2000
    '
    ' Edit History :SK
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        'Developer guide no. 50

        With objfrmInterface

            m_lStatus = .Status

        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        objfrmInterface.Close()
        objfrmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' Date :15/07/2000
    '
    ' Edit History :SK
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        'Developer guide no. 50

        VB6.ShowForm(objfrmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If frmInterface.ErrorNumber <> 0 Then
                result = frmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function
    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub




    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' Date :15/07/2000
    '
    ' Edit History:SK
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.

            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
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
    ' Date :15/07/2000
    '
    ' Edit History:SK
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(0, 0)


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function DoImport() As Boolean

        Dim result As Boolean = False
        Try

            Dim vReturn As Object
            Dim str As String = ""
            Dim lFileNumber As Integer

            'Jes 02 October 2002 ---
            'Dim Rows() As GroupRecord

            Dim lListTypeID, lSchemeID As Integer

            Dim Rows(1, 0) As Object

            'Get the List Type ID and The SCheme ID from the litems selected in the comboboxes
            'Developer guide no. 50
            lListTypeID = VB6.GetItemData(objfrmInterface.cmbListType, objfrmInterface.cmbListType.SelectedIndex)
            lSchemeID = VB6.GetItemData(objfrmInterface.cmbVersion, objfrmInterface.cmbVersion.SelectedIndex)
            lFileNumber = FileSystem.FreeFile()

            'Developer guide no. 50

            FileSystem.FileOpen(lFileNumber, objfrmInterface.txtFileName.Text, OpenMode.Input) ' Open file for input.

            'g_oBusiness.BeginTrans

            'Delete existing gis_list_grouping_items for this scheme version
            'developer guide no. 50

            objfrmInterface.lblProgress.Text = "Removing old grouping items..."
            Application.DoEvents()

            'Delete all the group listing items that are going to be replaced
            ' CTAF add lListTypeID


            vReturn = g_oBusiness.DeleteGroups(lSchemeID, lListTypeID)

            'developer guide no. 50

            objfrmInterface.lblProgress.Text = "Reading input file..."
            Application.DoEvents()

            'Loop through each row in the file getting the ABI code and Group Code
            Do Until FileSystem.EOF(lFileNumber)

                ReDim Preserve Rows(1, Rows.GetUpperBound(1) + 1)
                FileSystem.Input(lFileNumber, Rows(ACABICode, Rows.GetUpperBound(1)))
                FileSystem.Input(lFileNumber, Rows(ACGroupCode, Rows.GetUpperBound(1)))

            Loop

            'Add records to gis_list_grouping_items, creating any new groups on the way
            'developer guide no. 50

            objfrmInterface.lblProgress.Text = "Updating grouping items..."
            Application.DoEvents()
            'Pass all rows in the file to the business object in one go to save network traffic


            vReturn = g_oBusiness.ImportGroups(lListTypeID, lSchemeID, Rows)

            'Remove gis_list_grouping records that are not in new list
            'developer guide no. 50
            objfrmInterface.lblProgress.Text = "clearing up groups..."

            Application.DoEvents()


            vReturn = g_oBusiness.ClearupGroups(lSchemeID)

            FileSystem.FileClose(lFileNumber)


            g_oBusiness.CommitTrans()

            'developer guide no. 50

            objfrmInterface.lblProgress.Text = "Complete."
            Application.DoEvents()

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            g_oBusiness.RollbackTrans()

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Import Group Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DoImport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
End Class

