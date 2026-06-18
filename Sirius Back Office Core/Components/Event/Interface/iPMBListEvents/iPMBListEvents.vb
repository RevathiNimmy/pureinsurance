Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 08/09/1998
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    'developer guide no. 50
    Dim frmInterface As frmInterface
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_lPartyCnt As Integer
    Private m_lClaimCnt As Integer
    Private m_lBaseClaimId As Integer
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_sInsuranceRef As String = ""
    Private m_sClaimRef As String = ""

    ' AccountKey
    Private m_lAccountKey As Integer
    Private m_sEventGroupCode As String = ""
    Private m_bDisableEventGroupLookup As Boolean

    Private m_bEnableDefaultedFields As Boolean ' S4B Claim Enhancements R&D 2005
    Private m_bShowNonNotes As Boolean
    Private m_bShowNotes As Boolean
    Private m_bRTFNotes As Boolean
    Private m_lCaseID As Integer
    Private m_sCaseNumber As String = ""
    Private m_lBaseCaseID As Integer
    Private m_lKeepOnTop As Integer
    Private m_bShowAllClaimVersionEvents As Boolean

    Public WriteOnly Property AccountKey() As Integer
        Set(ByVal Value As Integer)
            m_lAccountKey = Value
        End Set
    End Property
    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceRef() As String
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property
    Public WriteOnly Property ClaimRef() As String
        Set(ByVal Value As String)
            m_sClaimRef = Value
        End Set
    End Property
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    Public WriteOnly Property ClaimCnt() As Integer
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
        End Set
    End Property
    Public WriteOnly Property BaseClaimId() As Integer
        Set(ByVal Value As Integer)
            m_lBaseClaimId = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property
    Public WriteOnly Property EventGroupCode() As String
        Set(ByVal Value As String)
            m_sEventGroupCode = Value
        End Set
    End Property
    Public WriteOnly Property DisableEventGroupLookup() As Boolean
        Set(ByVal Value As Boolean)
            m_bDisableEventGroupLookup = Value
        End Set
    End Property
    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get
            Return m_iTask
        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
    End Property
    Public ReadOnly Property StepStatus() As String
        Get
            Return m_sStepStatus.Value
        End Get
    End Property

    Public Property EnableDefaultedFields() As Boolean
        Get
            Return m_bEnableDefaultedFields
        End Get
        Set(ByVal Value As Boolean)
            m_bEnableDefaultedFields = Value
        End Set
    End Property

    Public Property ShowNonNotes() As Boolean
        Get
            Return m_bShowNonNotes
        End Get
        Set(ByVal Value As Boolean)
            m_bShowNonNotes = Value
        End Set
    End Property

    Public Property ShowNotes() As Boolean
        Get
            Return m_bShowNotes
        End Get
        Set(ByVal Value As Boolean)
            m_bShowNotes = Value
        End Set
    End Property

    Public Property RTFNotes() As Boolean
        Get
            Return m_bRTFNotes
        End Get
        Set(ByVal Value As Boolean)
            m_bRTFNotes = Value
        End Set
    End Property


    Public Property CaseID() As Integer
        Get
            Return m_lCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lCaseID = Value
        End Set
    End Property

    Public Property CaseNumber() As String
        Get
            Return m_sCaseNumber
        End Get
        Set(ByVal Value As String)
            m_sCaseNumber = Value
        End Set
    End Property

    Public Property BaseCaseID() As Integer
        Get
            Return m_lBaseCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lBaseCaseID = Value
        End Set
    End Property
    Public Property ShowAllClaimVersionEvents() As Boolean
        Get
            Return m_bShowAllClaimVersionEvents
        End Get
        Set(ByVal Value As Boolean)
            m_bShowAllClaimVersionEvents = Value
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
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise
        Dim result As Integer = 0
        Dim ACApp As Object = Nothing
        Dim g_oObjectManager As bObjectManager.ObjectManager

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=CStr(ACApp))

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.

                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            '    g_iLanguageID% = .LanguageID
            '    g_iSourceID% = .SourceID

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

            'Claim Enhancements
            m_bShowNonNotes = True
            m_bShowNotes = True

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
            End If
        End If
        Me.disposedValue = True
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
        Dim ACApp As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}



                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameKeepWindowOnTop

                        m_lKeepOnTop = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameShowAllClaimVersionEvents
                        m_bShowAllClaimVersionEvents = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
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
        Dim ACApp As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameID
            '    vKeyArray(PMKeyValue, 0) = m_iNameID%

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As Object = Nothing

        Try


            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            '    ReDim vKeyArray(1, 0)
            '
            '    ' Assign the key array with the parameter members.
            '    vSummaryArray(PMKeyName, 0) = PMKeyNameNavigatorTitle1
            '    vSummaryArray(PMKeyValue, 0) = m_sNavigatorTitle$

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACApp As Object = Nothing

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

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer
        Dim result As Integer = 0
        Dim ACApp As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer
        Dim result As Integer = 0
        Dim ACApp As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

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
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer
        Dim result As Integer = 0
        Dim ACApp As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue
        'Instance of the object has to be created.
        frmInterface = New frmInterface

        ' Assign the parameters to the interface properties.
        With frmInterface
            .PartyCnt = m_lPartyCnt
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .InsuranceRef = m_sInsuranceRef
            .ClaimCnt = m_lClaimCnt
            .BaseClaimId = m_lBaseClaimId
            .ClaimRef = m_sClaimRef
            .InsuranceFolderCnt = m_lInsuranceFolderCnt
            .AccountKey = m_lAccountKey
            .DisableEventGroupLookup = m_bDisableEventGroupLookup
            .EventGroupCode = m_sEventGroupCode
            .TransactionType = m_sTransactionType
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            '.ProcessMode = m_lProcessMode&
            .EffectiveDate = m_dtEffectiveDate
            .EnableDefaultedFields = m_bEnableDefaultedFields
            .CaseID = m_lCaseID
            .CaseNumber = m_sCaseNumber
            .RTFNotes = frmInterface.uctListEvents1.RTFNotes
            .ShowNotes = m_bShowNotes
            .ShowNonNotes = m_bShowNonNotes
            .BaseCaseID = m_lBaseCaseID
            .ShowAllClaimVersionEvents = m_bShowAllClaimVersionEvents
        End With

        ' Load the instance of the interface into memory.
        Dim tempLoadForm As frmInterface = frmInterface

        ' Check if we have had an error so far.
        If frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = frmInterface.ErrorNumber
        End If

        ' Set the status in the interface.
        '    m_lReturn& = frmInterface.SetStatus( _
        ''        sProcessStatus:=m_sProcessStatus$, _
        ''        sMapStatus:=m_sMapStatus$, _
        ''        sStepStatus:=m_sStepStatus$)
        '
        '    ' Check for errors.
        '    If (m_lReturn& <> PMTrue) Then
        '        ' Failed to set the status.
        '        LoadInterface = PMFalse
        '    End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer
        Dim result As Integer = 0
        Dim ACApp As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With frmInterface
            m_lStatus = .Status
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        frmInterface.Close()
        frmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lKeepOnTop = 1 Then
            m_lReturn = CType(iPMFunc.SetWindowPlacement(frmInterface.Handle.ToInt32(), True), gPMConstants.PMEReturnCode)
        End If


        ' Display the interface.
        VB6.ShowForm(frmInterface, lDisplayState)

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

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error Message

        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=CStr(ACApp), vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

