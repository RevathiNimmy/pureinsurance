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
    ' Date: 07/11/2002
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_sStepStatus As String = ""

    Private m_lPMAuthorityLevel As Integer

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPartyCnt As Integer
    Private m_lLoyaltySchemeId As Integer
    Private m_lPartyLoyaltySchemeID As Integer
    Private m_sLoyaltySchemeName As String = ""
    Private m_sMemberNumber As String = ""
    Private m_sMainMemberNumber As String = ""
    Private m_sOtherRef As String = ""
    Private m_dtStartDate As Date
    Private m_dtEndDate As Date
    Private m_iIsActive As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Private m_frmInterface As frmInterface

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' Party Cnt
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    ' Loyalty Scheme ID
    Public Property LoyaltySchemeID() As Integer
        Get
            Return m_lLoyaltySchemeId
        End Get
        Set(ByVal Value As Integer)
            m_lLoyaltySchemeId = Value
        End Set
    End Property

    ' Party Loyalty Scheme ID
    Public Property PartyLoyaltySchemeID() As Integer
        Get
            Return m_lPartyLoyaltySchemeID
        End Get
        Set(ByVal Value As Integer)
            m_lPartyLoyaltySchemeID = Value
        End Set
    End Property

    ' Loyalty Scheme Name
    Public ReadOnly Property LoyaltySchemeName() As String
        Get
            Return m_sLoyaltySchemeName
        End Get
    End Property
    ' Membership Number
    Public ReadOnly Property MemberNumber() As String
        Get
            Return m_sMemberNumber
        End Get
    End Property
    ' Main Membership Number
    Public ReadOnly Property MainMemberNumber() As String
        Get
            Return m_sMainMemberNumber
        End Get
    End Property
    ' Other Ref
    Public ReadOnly Property OtherRef() As String
        Get
            Return m_sOtherRef
        End Get
    End Property
    ' Start Date
    Public ReadOnly Property StartDate() As Date
        Get
            Return m_dtStartDate
        End Get
    End Property
    ' End Date
    Public ReadOnly Property EndDate() As Date
        Get
            Return m_dtEndDate
        End Get
    End Property
    ' IsActive
    Public ReadOnly Property IsActive() As Integer
        Get
            Return m_iIsActive
        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

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
        Const ACMethod As String = "Initialise"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            MainModule.g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = MainModule.g_oObjectManager.Initialise(MainModule.ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                MainModule.g_oObjectManager = Nothing

                ' Log & Display Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod)

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With MainModule.g_oObjectManager
                g_iLanguageID = .LanguageID
                MainModule.g_iSourceID = .SourceID
                MainModule.g_sUserName = .UserName
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, excep:=excep)

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
                If MainModule.g_oObjectManager IsNot Nothing Then
                    MainModule.g_oObjectManager.Dispose()
                    MainModule.g_oObjectManager = Nothing
                End If
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
        Const ACMethod As String = "SetKeys"


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

                '        Select Case Trim$(CStr(vKeyArray(PMKeyName, lRow&)))
                '            Case PMKeyNameID
                '                m_iNameID% = CInt(vKeyArray(PMKeyValue, lRow&))

                '        End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Const ACMethod As String = "GetKeys"

        Try


            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            '    ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameID
            '    vKeyArray(PMKeyValue, 0) = m_iNameID%

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function GetSummary(ByRef vSummaryArray As String) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "GetSummary"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            ' Assign the key array with the parameter members.
            '    vSummaryArray(PMKeyName, 0) = PMKeyNameNavigatorTitle1
            '    vSummaryArray(PMKeyValue, 0) = m_sNavigatorTitle$
            vSummaryArray = ""

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Const ACMethod As String = "SetProcessModes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Const ACMethod As String = "Start"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Const ACMethod As String = "ProcessInterface"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = LoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' RAW 18/11/02 : PS005 : Added
        ' Display the interface - if not a delete
        If m_iTask <> gPMConstants.PMEComponentAction.PMDelete Then
            m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to display the inteface.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' do not exit here - need to unload interface
            End If
        End If
        ' RAW 18/11/02 : PS005 : End

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
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
        Const ACMethod As String = "LoadInterface"



        result = gPMConstants.PMEReturnCode.PMTrue

        m_frmInterface = New frmInterface()

        'frmInterface = Nothing
        ' Assign the parameters to the interface properties.
        With m_frmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}

            .PartyLoyaltySchemeID = m_lPartyLoyaltySchemeID
            .PartyCnt = m_lPartyCnt

            ' {* USER DEFINED CODE (End) *}

        End With

        ' Load the instance of the interface into memory.

        'developer guide no. 68
        'Load(m_frmInterface)
        If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMDelete Then
            m_frmInterface.FrmInterfaceLoad()
        End If
        ' Check if we have had an error so far.
        If m_frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            Return m_frmInterface.ErrorNumber
        End If

        '    ' Set the status in the interface.
        '    m_lReturn& = m_frmInterface.SetStatus( _
        ''        sProcessStatus:=m_sProcessStatus$, _
        ''        sMapStatus:=m_sMapStatus$, _
        ''        sStepStatus:=m_sStepStatus$)
        '
        '    ' Check for errors.
        '    If (m_lReturn& <> PMTrue) Then
        '        ' Failed to set the status.
        '        LoadInterface = PMFalse
        '    End If
        '
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
        Const ACMethod As String = "ShowInterface"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(m_frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_frmInterface.ErrorNumber <> 0 Then
                Return m_frmInterface.ErrorNumber
            End If
        End If

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
        Const ACMethod As String = "UnLoadInterface"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_frmInterface
            m_lStatus = .Status
            m_sStepStatus = .StepStatus

            ' {* USER DEFINED CODE (Begin) *}

            m_lPartyCnt = .PartyCnt
            m_lLoyaltySchemeId = .LoyaltySchemeID
            m_lPartyLoyaltySchemeID = .PartyLoyaltySchemeID

            m_sLoyaltySchemeName = .LoyaltySchemeName
            m_sMemberNumber = .MemberNumber
            m_sMainMemberNumber = .MainMemberNumber
            m_sOtherRef = .OtherRef
            m_dtStartDate = .StartDate
            m_dtEndDate = .EndDate
            m_iIsActive = .IsActive

            ' {* USER DEFINED CODE (End) *}

        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        'frmInterface.Close()
        m_frmInterface = Nothing

        Return result

    End Function

    'PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'Const ACMethod As String = "Class_Initialize"

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

