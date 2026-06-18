Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no. 129 (guide)
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 10/07/2000
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History: VB
    ' ***************************************************************** '




    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    'Variable for Underwriting/Broking
    Private m_lSiriusUnderWritingBroking As String = ""

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)
    'developer guide no. 50
    'developer guide no. 107
    <ThreadStatic()> _
    Dim frmInterface As frmInterface
    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer


    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Variables for Find Case
    'developer guide no.101
    Private m_vPartyCnt As Object
    Private m_sPartyCode As String = ""
    Private m_sPartyName As String = ""
    Private m_vAccountId As Object

    Private m_bLoadChildEdit As Boolean
    Private m_bLoadChildAdd As Boolean
    Private m_bLoadChildView As Boolean
    Private m_lBG_id As Integer
    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    'Start - Sankar - Bank Guarantee Bug Fixing
    Private m_bCallFromClientManager As Boolean
    'End - Sankar - Bank Guarantee Bug Fixing

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


    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property


    Public Property BGId() As Integer
        Get
            Return m_lBG_id
        End Get
        Set(ByVal Value As Integer)
            m_lBG_id = Value
        End Set
    End Property


    Public Property PartyCnt() As Integer
        Get
            Return m_vPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_vPartyCnt = Value
        End Set
    End Property


    Public Property PartyCode() As String
        Get
            Return m_sPartyCode
        End Get
        Set(ByVal Value As String)
            m_sPartyCode = Value
        End Set
    End Property


    Public Property PartyName() As String
        Get
            Return m_sPartyName
        End Get
        Set(ByVal Value As String)
            m_sPartyName = Value
        End Set
    End Property



    Public Property LoadChildEdit() As Boolean
        Get
            Return m_bLoadChildEdit
        End Get
        Set(ByVal Value As Boolean)
            m_bLoadChildEdit = Value
        End Set
    End Property


    Public Property LoadChildView() As Boolean
        Get
            Return m_bLoadChildView
        End Get
        Set(ByVal Value As Boolean)
            m_bLoadChildView = Value
        End Set
    End Property


    Public Property LoadChildAdd() As Boolean
        Get
            Return m_bLoadChildAdd
        End Get
        Set(ByVal Value As Boolean)
            m_bLoadChildAdd = Value
        End Set
    End Property



    Public Property CallFromClientManager() As Boolean
        Get
            Return m_bCallFromClientManager
        End Get
        Set(ByVal Value As Boolean)
            m_bCallFromClientManager = Value
        End Set
    End Property
    'End - Sankar - Bank Guarantee Bug Fixing


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' Date :18/06/2007
    '
    ' Edit History: VB
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

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
                g_iUserID = .UserID
            End With

            ' Initialise the process modes.
            'm_iTask% = PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object = Nothing
            'TODO_Milan: Revoke the changes once bObject is freshly build
            'm_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRBankGuarantee.Business", vInstanceManager:="ClientManager")
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRBankGuarantee.Business", "ClientManager")
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bCLMCase", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
            Dim temp_g_oBackofficelink As Object = Nothing
            'TODO_Milan: Revoke the changes once bObject is freshly build
            'm_lReturn = g_oObjectManager.GetInstance(temp_g_oBackofficelink, "bBackofficelink.bBOlink", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBackofficelink, "bBackOfficeLink.bBOLink", gPMConstants.PMGetViaClientManager)
            g_oBackofficelink = temp_g_oBackofficelink

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bBackOfficeLink.bBOLink", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

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
    ' Date :18/06/2007
    '
    ' Edit History:VB
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
            End If
        End If
        Me.disposedValue = True
    End Sub


    '' ***************************************************************** '
    '' Name: SetKeys (Standard Method)
    ''
    '' Description: Stores all of the parameter members with the key
    ''              array.
    ''
    '' Date :18/06/2007
    ''
    '' Edit History:VB
    '' ***************************************************************** '
    'Public Function SetKeys(vKeyArray(,) As(,) Variant ) As Long
    '
    'Dim lRow As Long
    '
    '    On Error GoTo Err_SetKeys
    '
    '    SetKeys = PMTrue
    '
    '    ' Check we have a valid array.
    '    If (IsArray(vKeyArray) = False) Then
    '        SetKeys = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Step through the key array.
    '    For lRow& = LBound(vKeyArray, 2) To UBound(vKeyArray, 2)
    '
    '        Select Case Trim$(CStr(vKeyArray(PMKeyName, lRow&)))
    '            Case PMKeyNameClaimCnt
    '               ' m_lClaimCnt& = CLng(vKeyArray(PMKeyValue, lRow&))
    '            Case "case_number"
    '                m_sCaseNumber = CLng(vKeyArray(PMKeyValue, lRow&))
    '            Case "progresss_tatus"
    '                m_lProgressStatusId = CLng(vKeyArray(PMKeyValue, lRow&))
    '            Case "case_open_date"
    '                m_dtCaseOpenDate = CLng(vKeyArray(PMKeyValue, lRow&))
    '            Case "claim_number"
    '                m_lClaimNumber = CLng(vKeyArray(PMKeyValue, lRow&))
    '            Case "risk_type"
    '                m_lRiskTypeId = CLng(vKeyArray(PMKeyValue, lRow&))
    '        End Select
    '
    '    Next lRow&
    '
    '    Exit Function
    '
    'Err_SetKeys:
    '
    '    SetKeys = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="SetKeys Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="SetKeys", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    '
    '' ***************************************************************** '
    '' Name: GetKeys (Standard Method)
    ''
    '' Description: Stores all of the key array with the parameter
    ''              members.
    ''
    '' Date :18/06/2007
    ''
    '' Edit History:VB
    '' ***************************************************************** '
    'Public Function GetKeys(vKeyArray(,) As(,) Variant ) As Long
    '
    'Dim lRow As Long
    '
    '    On Error GoTo Err_GetKeys
    '
    '    GetKeys = PMTrue
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '
    '    ' Initialise the key array with the number of
    '    ' keys needed to be returned.
    '    ' Note: Remember arrays are zero based.
    '    ReDim vKeyArray(1, 7)
    '
    '    ' Assign the key array with the parameter members.
    '    vKeyArray(PMKeyName, 0) = "case" '- -PMKeyNameClaimCnt
    '    vKeyArray(PMKeyValue, 0) = 0
    '    vKeyArray(PMKeyName, 1) = PMKeyNamePolicyID
    '    vKeyArray(PMKeyValue, 1) = 0
    '    vKeyArray(PMKeyName, 2) = PMKeyNameClaimReference
    '    vKeyArray(PMKeyValue, 2) = 0
    '
    '    ' {* USER DEFINED CODE (End) *}
    '
    '    Exit Function
    '
    'Err_GetKeys:
    '
    '    GetKeys = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetKeys Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetKeys", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' Date :18/06/2007
    '
    ' Edit History :VB
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
    ' Date :18/06/2007
    '
    ' Edit History :VB
    ' ***************************************************************** '
    Public Function Start() As Integer



        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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
    ' Date :18/06/2007
    '
    ' Edit History:VB
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


        If (LoadChildEdit Or LoadChildAdd Or LoadChildView) And frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
            ' Do Nothing
        Else
            ' Display the interface.
            m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to display the inteface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
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
    ' Date :18/06/2007
    '
    ' Edit History:VB
    '***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'developer guide no. added code
        frmInterface = New frmInterface()
        ' Assign the parameters to the interface properties.

        With frmInterface

            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .Task = m_iTask
            .TransactionType = m_sTransactionType
            .LoadChildAdd = m_bLoadChildAdd
            .LoadChildEdit = m_bLoadChildEdit
            .PartyCnt = m_vPartyCnt
            .BGId = m_lBG_id
            .LoadChildView = m_bLoadChildView
            .PartyCode = m_sPartyCode
            .PartyName = m_sPartyName
            .CallFromClientManager = m_bCallFromClientManager ' Sankar - Bank Guarantee Bug Fixing
        End With

        ' Load the instance of the interface into memory.

        Dim tempLoadForm As frmInterface = frmInterface


        ' Check if we have had an error so far.
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' Date :18/06/2007
    '
    ' Edit History :VB
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.

        With frmInterface

            'm_lStatus& = .Status
            'm_iTask% = .Task

        End With

        ' Unload and destroy the instance of the interface from memory.

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
    ' Date :18/06/2007
    '
    ' Edit History :VB
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        ' Display the interface.


        VB6.ShowForm(frmInterface, lDisplayState)


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

End Class

