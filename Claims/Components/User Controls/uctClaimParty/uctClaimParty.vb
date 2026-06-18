Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
'Developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctClaimParty_NET.uctClaimParty")> _
Partial Public Class uctClaimParty
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event ClaimChange()
    Public Event PartyTypeCodeChange()
    Public Event PartyTypeChange()
    Public Event EffectiveDateChange()
    Public Event TransactionTypeChange()
    Public Event ProcessModeChange()
    Public Event NavigateChange()
    Public Event TaskChange()
    Public Event CallingAppNameChange()

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctClaimPartyControl"

    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0

    'Default Property Values:
    Const m_def_BackColor As Integer = 0
    Const m_def_ForeColor As Integer = 0
    Const m_def_Enabled As Integer = 0
    Const m_def_BackStyle As Integer = 0
    Const m_def_BorderStyle As Integer = 0
    Const m_def_PartyType As Integer = 0
    Const m_def_PartyTypeCode As String = ""
    Const m_def_ClaimId As Integer = 0

    'Property Variables:
    Dim m_BackColor As Integer
    Dim m_ForeColor As Integer
    Dim m_Enabled As Boolean
    Dim m_Font As Font
    Dim m_BackStyle As Integer
    Dim m_BorderStyle As Integer
    Dim m_lPartyType As Integer
    Dim m_sPartyTypeCode As String = ""
    Dim m_lClaimId As Integer

    Private m_lRiskTypeId As Integer
    Private m_lPerilTypeId As Integer

    Const ACDriver As String = "OTDRIVER"

    ' Keys

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    'Property variables
    Private m_sUnderwritingOrAgency As String = ""

    Const m_iFirstRecord As Integer = 0
    ' Variables
    Private m_lReturn As Integer

    'Collections
    'Property Variables:

    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs)
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs)
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs)
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs)
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs)
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs)
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs)

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRPartyPC.Business

    Private m_oFormFields As iPMFormControl.FormFields

    Private m_vPartyArray(,) As Object
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object
    'For Event Traping
    Private m_PartyDetail As Object
    Private m_bDataChanged As Boolean
    <Browsable(False)> _
    Public ReadOnly Property Controls_Renamed() As Object
        Get
            Return Me.Controls_Renamed
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    <Browsable(False)> _
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value
            RaiseEvent CallingAppNameChange()

        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    <Browsable(True)> _
    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value
            RaiseEvent TaskChange()

        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value
            RaiseEvent NavigateChange()

        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value
            RaiseEvent ProcessModeChange()

        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value
            RaiseEvent TransactionTypeChange()

        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value
            RaiseEvent EffectiveDateChange()

        End Set
    End Property
    ' {* USER DEFINED CODE (Begin) *}


    <Browsable(True)> _
    Public Property PartyType() As Integer
        Get
            Return m_lPartyType
        End Get
        Set(ByVal Value As Integer)
            m_lPartyType = Value
            RaiseEvent PartyTypeChange()
        End Set
    End Property
    <Browsable(True)> _
    Public Property PartyTypeCode() As String
        Get
            Return m_sPartyTypeCode
        End Get
        Set(ByVal Value As String)
            m_sPartyTypeCode = Value
            RaiseEvent PartyTypeCodeChange()
        End Set
    End Property
    <Browsable(True)> _
    Public Property ClaimId() As Integer
        Get
            Return m_lClaimId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimId = Value
            RaiseEvent ClaimChange()
        End Set
    End Property

    <Browsable(True)> _
    Public Property RiskTypeId() As Integer
        Get
            Return m_lRiskTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property PerilTypeId() As Integer
        Get
            Return m_lPerilTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lPerilTypeId = Value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property PartyCount() As Integer
        Get
            If Information.IsArray(m_vPartyArray) Then

                Return m_vPartyArray.GetUpperBound(1) + 1
            Else
                Return 0
            End If

        End Get
    End Property

    ' {* USER DEFINED CODE (End) *}

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)

    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: CancelClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function CancelClick() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return CancelParty()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CancelParty
    '
    ' Description: Called when we wish to cancel any changes
    '
    ' ***************************************************************** '
    Private Function CancelParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                'Me.Hide
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the party", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Private Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try


            ' Get the lookup values.

            '    m_lReturn& = GetLookupValues()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '    ' Get all of the lookup details.
            '
            '    ' {* USER DEFINED CODE (Begin) *}
            '    'SP090998
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=SIRLookupArea, _
            ''        ctlLookup:=cboArea)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '

    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control, Optional ByRef bSecondary As Boolean = False) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow, lRow2 As Integer
    'Dim bFoundMatch As Boolean
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    ' Check if this is the selected index.
    'If bSecondary Then
    'lRow2 = lRow + 1
    'Else
    'lRow2 = lRow
    'End If
    'If CStr(m_vLookupValues(ACValueID, lRow2)) <> "" Then
    'If CDbl(m_vLookupValues(ACValueID, lRow2)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    '
    'End If
    '
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    'If CStr(m_vLookupValues(ACValueID, lRow2)) = "" Then

    'ctlLookup.ListIndex = -1
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '

    'Private Function GetLookupValues() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Gets all of the lookup values.
    '
    ' Check the task.
    'Select Case (m_iTask)
    'Case gPMConstants.PMEComponentAction.PMAdd
    ' Get all of the lookup values.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMEdit
    ' Get all of the lookup values with the correct
    ' effective date.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMView
    ' Get lookup values for viewing only.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    'End Select
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
    '
    'Return result
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetParties
    '
    ' Description: Gets the interface details and sets the appropriate
    '              style.
    '
    ' ***************************************************************** '
    Public Function GetParties() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            If (Task = gPMConstants.PMEComponentAction.PMEdit Or Task = gPMConstants.PMEComponentAction.PMView) Or m_lClaimId > 0 Then
                ' Get the interface details from the
                ' business object.
                m_lReturn = GetBusiness()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business object
                ' to the interface.
                m_lReturn = BusinessToInterface()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Display all of the lookup details.
            m_lReturn = DisplayLookupDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the task.
            If Task = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                m_lReturn = DisableForm(lDisabled:=True)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the parties", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Static bIsInitialised As Boolean

        Dim sTitle, sMessage As String

        Dim sHelpFile As String = ""
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

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

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserId = .UserID
            End With

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                ''Developer Guide No. 39 (No Solution Found)
                'App.HelpFile = sHelpFile
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMClaimParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.
                '        sTitle = iPMFunc.GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFailTitle, _
                'iDataType:=PMResString)

                '        sMessage = iPMFunc.GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFail, _
                'iDataType:=PMResString)

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If


            m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency

            'SP130199 - Add locking

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' hold Initialised status
            bIsInitialised = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadControl
    '
    ' Description: Does all the extra stuff that initialise doesn't
    '
    ' ***************************************************************** '
    Public Function LoadControl() As Integer

        Dim result As Integer = 0

        ' Forms load event.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.ClaimId = m_lClaimId

            m_oBusiness.RiskTypeId = m_lRiskTypeId

            m_oBusiness.PerilTypeId = m_lPerilTypeId
            ' {* USER DEFINED CODE (End) *}

            m_oFormFields = New iPMFormControl.FormFields()

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()


            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load control", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Refresh
    '
    ' Description: Refresh the list of Other Parties
    '
    ' ***************************************************************** '
    Public Overrides Sub Refresh()

        'S4B Claim Enhancements R&D 2005
        GetParties()

    End Sub

    ' ***************************************************************** '
    ' Name: SaveParty
    '
    ' Description: Saves the displayed party details
    '
    ' ***************************************************************** '
    Private Function SaveParty() As Integer

        ' Click event of the OK button.

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Validate some address stuff
            m_lReturn = ValidateOK()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the party", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If m_oFormFields IsNot Nothing Then
                    m_oFormFields.Dispose()
                    m_oFormFields = Nothing
                End If

                m_vPartyArray = Nothing
                
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: UnloadControl
    '
    ' Description: Cleans up then unloads the control
    '
    ' ***************************************************************** '
    Public Function UnLoadControl(ByRef Cancel As Integer, ByRef UnloadMode As Integer) As Integer

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Function
                End If
            End If

            Dispose()
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: ValidateParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function ValidateParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return ValidateOK()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidatePartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)
    ' PRIVATE Methods (Begin)

    '*********************************************************************
    'Name       : AddPartyDetails
    'Description: Open the Find Party Screen
    '             If Ok was pressed on that screen then add the Party_Id to the collection
    '             of newly add Parties
    '             Check if Claim Id has been already choosen i.e. in the List View. If yes,
    '             then do not add the Party to the list view,nor the collAddedParties
    '*********************************************************************
    Private Function AddPartyDetails() As Integer

        Dim result As Integer = 0

        'Developer Guide No. 88
        Dim oFindParty As Object
        Dim vKeyArray(,) As Object
        Dim lTemp, lPartyCnt, lArrayBound As Integer

        Dim sTitle, sMessage As String

        Dim vPartyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "special_party"

            If m_sUnderwritingOrAgency = "A" And m_sPartyTypeCode = "" Then

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = gSIRLibrary.SIRPartyTypeOther
            Else

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = PartyTypeCode
            End If

            If g_oObjectManager Is Nothing Then g_oObjectManager = New bObjectManager.ObjectManager() 'PN-69385

            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oFindParty.SetKeys(vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oFindParty.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then

                oFindParty.Dispose()
                oFindParty = Nothing
                Return result
            End If


            m_lReturn = oFindParty.GetKeys(vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn = oFindParty.Terminate

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set oFindParty = Nothing


            For lTemp = ACFirstRow To vKeyArray.GetUpperBound(1)
                Select Case (vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lTemp))
                    Case PMKeyNamePartyCnt

                        lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lTemp))
                End Select
            Next lTemp

            'Check if Party Cnt has been already chosen

            If Information.IsArray(m_vPartyArray) Then

                For lTemp = m_vPartyArray.GetLowerBound(1) To m_vPartyArray.GetUpperBound(1)

                    If CDbl(m_vPartyArray(ACColPartyCnt, lTemp)) = lPartyCnt Then

                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddPartyTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyAlreadyPresentMessage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
                        Return result
                    End If
                Next lTemp
            End If
            'PN-69385 Start
            If m_oBusiness Is Nothing Then
                Dim temp_m_oBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMClaimParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oBusiness = temp_m_oBusiness
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If
            'PN-69385 End

            m_lReturn = m_oBusiness.GetSingleParty(v_lParty:=lPartyCnt, r_vDetails:=vPartyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lArrayBound = IIf(m_sUnderwritingOrAgency = "U", ACCOL_INDEX_UPPER_U, ACCOL_INDEX_UPPER_A)

            If Not Information.IsArray(m_vPartyArray) Then
                lTemp = 0
                ReDim m_vPartyArray(lArrayBound, lTemp)
            Else

                lTemp = m_vPartyArray.GetUpperBound(1) + 1
                ReDim Preserve m_vPartyArray(lArrayBound, lTemp)
            End If

            'AJM 14/08/2001 - Prevent subscript out of range error

            '    m_vPartyArray(ACColPartyCnt, lTemp) = vPartyArray(ACColPartyCnt, lTemp)
            '    m_vPartyArray(ACColName, lTemp) = vPartyArray(ACColName, lTemp)
            '    m_vPartyArray(ACColAddressLine1, lTemp) = vPartyArray(ACColAddressLine1, lTemp)
            '    m_vPartyArray(ACColPhone, lTemp) = vPartyArray(ACColPhone, lTemp)
            '    m_vPartyArray(ACColLicenseNumber, lTemp) = vPartyArray(ACColLicenseNumber, lTemp)
            '    m_vPartyArray(ACColDOB, lTemp) = vPartyArray(ACColDOB, lTemp)
            '    m_vPartyArray(ACColGender, lTemp) = vPartyArray(ACColGender, lTemp)
            '    m_vPartyArray(ACColStatus, lTemp) = vPartyArray(ACColStatus, lTemp)


            m_vPartyArray(ACColPartyCnt, lTemp) = vPartyArray(ACColPartyCnt, 0)


            m_vPartyArray(ACColName, lTemp) = vPartyArray(ACColName, 0)


            m_vPartyArray(ACColAddressLine1, lTemp) = vPartyArray(ACColAddressLine1, 0)


            m_vPartyArray(ACColPhone, lTemp) = vPartyArray(ACColPhone, 0)


            m_vPartyArray(ACColLicenseNumber, lTemp) = vPartyArray(ACColLicenseNumber, 0)


            m_vPartyArray(ACColDOB, lTemp) = vPartyArray(ACColDOB, 0)


            m_vPartyArray(ACColGender, lTemp) = vPartyArray(ACColGender, 0)


            m_vPartyArray(ACColStatus, lTemp) = vPartyArray(ACColStatus, 0)

            'S4B Claim Enhancements R&D 2005
            If m_sUnderwritingOrAgency = "A" Then


                m_vPartyArray(ACColContactName, lTemp) = vPartyArray(ACColContactName, 0)


                m_vPartyArray(ACColContactTelephone, lTemp) = vPartyArray(ACColContactTelephone, 0)


                m_vPartyArray(ACColPartyTypeCode, lTemp) = vPartyArray(ACColPartyTypeCode, 0)


                m_vPartyArray(ACColPartyTypeDescription, lTemp) = vPartyArray(ACColPartyTypeDescription, 0)
            End If


            vPartyArray = Nothing

            m_lReturn = PopulateParties()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TN20010829 - start
            If PartyTypeCode = "OTDRIVER" Then
                lvwParty.FocusedItem = lvwParty.Items.Item(lTemp)
            End If
            'TN20010829 - end

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPartyDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            m_vPartyArray = m_oBusiness.PartyArray


            m_PartyDetail = m_vPartyArray
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Private Function BusinessToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.


            'Fill the parties list view
            PopulateParties()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function DeletePartyDetails() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object
        Dim lThisOne, lTemp2 As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vPartyArray) Then
                Return result
            End If


            lThisOne = Convert.ToString(lvwParty.FocusedItem.Tag)


            vArray = Nothing


            For lTemp As Integer = m_vPartyArray.GetLowerBound(1) To m_vPartyArray.GetUpperBound(1)
                If lTemp <> lThisOne Then
                    If Not Information.IsArray(vArray) Then
                        lTemp2 = 0

                        ReDim vArray(m_vPartyArray.GetUpperBound(0), lTemp2)
                    Else

                        lTemp2 = vArray.GetUpperBound(1) + 1

                        ReDim Preserve vArray(m_vPartyArray.GetUpperBound(0), lTemp2)
                    End If



                    vArray(ACColPartyCnt, lTemp2) = m_vPartyArray(ACColPartyCnt, lTemp)


                    vArray(ACColName, lTemp2) = m_vPartyArray(ACColName, lTemp)


                    vArray(ACColAddressLine1, lTemp2) = m_vPartyArray(ACColAddressLine1, lTemp)


                    vArray(ACColPhone, lTemp2) = m_vPartyArray(ACColPhone, lTemp)


                    vArray(ACColLicenseNumber, lTemp2) = m_vPartyArray(ACColLicenseNumber, lTemp)


                    vArray(ACColDOB, lTemp2) = m_vPartyArray(ACColDOB, lTemp)


                    vArray(ACColGender, lTemp2) = m_vPartyArray(ACColGender, lTemp)


                    vArray(ACColStatus, lTemp2) = m_vPartyArray(ACColStatus, lTemp)

                    'S4B Claim Enhancements R&D 2005
                    If m_sUnderwritingOrAgency = "A" Then


                        vArray(ACColContactName, lTemp2) = m_vPartyArray(ACColContactName, lTemp)


                        vArray(ACColContactTelephone, lTemp2) = m_vPartyArray(ACColContactTelephone, lTemp)


                        vArray(ACColPartyTypeCode, lTemp2) = m_vPartyArray(ACColPartyTypeCode, lTemp)


                        vArray(ACColPartyTypeDescription, lTemp2) = m_vPartyArray(ACColPartyTypeDescription, lTemp)
                    End If

                End If
            Next lTemp



            m_vPartyArray = vArray


            vArray = Nothing

            m_lReturn = PopulateParties()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lvwParty.Items.Count <= 0 Then
                cmdEditParty.Enabled = False
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePartyDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set all of the forms controls to the disable state.
            For Each ctlFormControl As Control In Controls_Renamed
                ' Check the type of the control.
                If TypeOf ctlFormControl Is TextBox Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                End If
            Next ctlFormControl

            cmdAddParty.Enabled = Not lDisabled

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            '    Me.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACInterfaceTitle, _
            'iDataType:=PMResString)

            ' Check for an error.
            '    If (Me.Caption = "") Then
            ' Failed to get data from the resource file.
            '        DisplayCaptions = PMFalse

            ' Log Error.
            '        LogMessage _
            'iType:=PMLogError, _
            'sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
            '"Please check the file exists and the correct captions are available", _
            'vApp:=ACApp, _
            'vClass:=ACClass, _
            'vMethod:="DisplayCaptions"

            '        Exit Function
            '    End If


            cmdAddParty.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEditParty.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDeleteParty.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************
    'Name       : EditPartyDetails
    'Description : This function brings up the Party screen. If OK is press on that
    '              screen, then update the selected item's info.
    '********************************************************************

    Private Function EditPartyDetails() As Integer

        Dim result As Integer = 0

        'Developer Guide No. 88
        Dim oPartyOT As Object
        Dim vKeyArray(,) As Object
        Dim lTemp As Integer
        Dim vPartyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lvwParty.Items.Count > 0 Then

                lTemp = Convert.ToString(lvwParty.FocusedItem.Tag)

                ReDim vKeyArray(1, 1)

                ' Assign the key array with the parameter members.

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMKeyNamePartyCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_vPartyArray(ACColPartyCnt, lTemp)


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMKeyNamePartyOther


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_vPartyArray(ACColPartyTypeCode, lTemp)

                Dim temp_oPartyOT As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oPartyOT, sClassName:="iPMBPartyOT.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oPartyOT = temp_oPartyOT

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = oPartyOT.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = oPartyOT.SetKeys(vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = oPartyOT.Start

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If oPartyOT.Status = gPMConstants.PMEReturnCode.PMCancel Then

                    oPartyOT.Dispose()
                    oPartyOT = Nothing
                    Return result
                End If


                oPartyOT.Dispose()

                

                oPartyOT = Nothing


                m_lReturn = m_oBusiness.GetSingleParty(v_lParty:=m_vPartyArray(ACColPartyCnt, lTemp), r_vDetails:=vPartyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'AJM 14/08/2001 - prevent subscript out of range error


                m_vPartyArray(ACColName, lTemp) = vPartyArray(ACColName, 0)


                m_vPartyArray(ACColAddressLine1, lTemp) = vPartyArray(ACColAddressLine1, 0)


                m_vPartyArray(ACColPhone, lTemp) = vPartyArray(ACColPhone, 0)


                m_vPartyArray(ACColLicenseNumber, lTemp) = vPartyArray(ACColLicenseNumber, 0)


                m_vPartyArray(ACColDOB, lTemp) = vPartyArray(ACColDOB, 0)


                m_vPartyArray(ACColGender, lTemp) = vPartyArray(ACColGender, 0)


                m_vPartyArray(ACColStatus, lTemp) = vPartyArray(ACColStatus, 0)

                'S4B Claim Enhancements R&D 2005
                If m_sUnderwritingOrAgency = "A" Then


                    m_vPartyArray(ACColContactName, lTemp) = vPartyArray(ACColContactName, 0)


                    m_vPartyArray(ACColContactTelephone, lTemp) = vPartyArray(ACColContactTelephone, 0)


                    m_vPartyArray(ACColPartyTypeCode, lTemp) = vPartyArray(ACColPartyTypeCode, 0)


                    m_vPartyArray(ACColPartyTypeDescription, lTemp) = vPartyArray(ACColPartyTypeDescription, 0)
                End If

                '    m_vPartyArray(ACColName, lTemp) = vPartyArray(ACColName, lTemp)
                '    m_vPartyArray(ACColAddressLine1, lTemp) = vPartyArray(ACColAddressLine1, lTemp)
                '    m_vPartyArray(ACColPhone, lTemp) = vPartyArray(ACColPhone, lTemp)
                '    m_vPartyArray(ACColLicenseNumber, lTemp) = vPartyArray(ACColLicenseNumber, lTemp)
                '    m_vPartyArray(ACColDOB, lTemp) = vPartyArray(ACColDOB, lTemp)
                '    m_vPartyArray(ACColGender, lTemp) = vPartyArray(ACColGender, lTemp)
                '    m_vPartyArray(ACColStatus, lTemp) = vPartyArray(ACColStatus, lTemp)


                vPartyArray = Nothing

                m_lReturn = PopulateParties()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Edit Party Details", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_oBusiness.ClaimId = m_lClaimId

            m_oBusiness.PartyTypeCode = m_sPartyTypeCode

            m_oBusiness.RiskTypeId = m_lRiskTypeId

            m_oBusiness.PerilTypeId = m_lPerilTypeId


            m_lReturn = m_oBusiness.GetDetails()
            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_oBusiness.PartyArray = m_vPartyArray

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Dim sMsg As String = ""


        Try


            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}
            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: OKClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function OKClick() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = SaveParty()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateParties
    '
    ' Description: Fills the grid control with contact details
    '
    ' ***************************************************************** '
    Private Function PopulateParties() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwParty.Items.Clear()

            If Not Information.IsArray(m_vPartyArray) Then
                Return result
            End If

            ' Assign the details to the interface.

            For i As Integer = m_vPartyArray.GetLowerBound(1) To m_vPartyArray.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1 - Name

                oListItem = lvwParty.Items.Add(CStr(m_vPartyArray(ACColName, i)).Trim())

                ' Assign details to other the columns

                If m_sUnderwritingOrAgency = "A" And m_sPartyTypeCode = "" Then

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.ToSafeString(CStr(m_vPartyArray(ACColAddressLine1, i))).Trim() 'Address

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.ToSafeString(CStr(m_vPartyArray(ACColContactName, i))).Trim() 'Contact Name

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.ToSafeString(CStr(m_vPartyArray(ACColContactTelephone, i))).Trim() 'Contact No.

                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.ToSafeString(CStr(m_vPartyArray(ACColPartyTypeDescription, i))).Trim() 'Party Type
                Else
                    If PartyTypeCode <> ACDriver Then

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vPartyArray(ACColAddressLine1, i)).Trim() ' Column 2 - Address

                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vPartyArray(ACColPhone, i)).Trim() ' Column 3 - Phone
                    Else
                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_vPartyArray(ACColDOB, i))

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vPartyArray(ACColLicenseNumber, i)).Trim() ' Column 2 - License
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtDate.Text ' Column 3 - date of birth

                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vPartyArray(ACColGender, i)).Trim() ' Column 4 - Gender

                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vPartyArray(ACColStatus, i)).Trim() ' Column 5 - Status
                    End If
                End If

                ' Store the array position
                oListItem.Tag = CStr(i)
                ' {* USER DEFINED CODE (End) *}

            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateParties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String
        Dim lPartyCnt, iCount As Integer
        Dim sEventDescription As String = ""
        'Const AC_COL_CLAIMPERIL_DESC As Integer = 3
        Const AC_EVENT_TYPE_UPDATECLAIM As Integer = 6

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                Select Case Task
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
                        ' Update the business from the interface.
                        m_lReturn = InterfaceToBusiness()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update business.
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                End Select

            End If

            ' Check the task.
            Select Case Task
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' Form hasn't been cancelled, so we just go
                        ' ahead and add the details.

                        ' Add the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details (m_oBusiness.Update)", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    End If

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then

                        ' Check the details havn't changed.
                        '                m_lReturn& = m_oBusiness.Cancel()

                        'MH Request - Always confirm cancellation
                        '                If (m_lReturn& = PMDataChanged) Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        '                End If
                    Else

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    End If
            End Select
            'For Event Entry
            iCount = 0
            If Information.IsArray(m_PartyDetail) And Information.IsArray(m_vPartyArray) Then


                If m_vPartyArray.GetUpperBound(1) = m_PartyDetail.GetUpperBound(1) Then

                    For iCounter As Integer = m_vPartyArray.GetLowerBound(1) To m_vPartyArray.GetUpperBound(1)

                        lPartyCnt = CInt(gPMFunctions.ToSafeString(CStr(m_vPartyArray(ACColPartyCnt, iCounter))).Trim())

                        For jCounter As Integer = m_PartyDetail.GetLowerBound(1) To m_PartyDetail.GetUpperBound(1)

                            If lPartyCnt = StringsHelper.ToDoubleSafe(CStr(gPMFunctions.ToSafeInteger(CStr(m_PartyDetail(ACColPartyCnt, jCounter)))).Trim()) Then
                                iCount += 1
                                Exit For
                            End If
                        Next
                    Next

                    If iCount <> m_vPartyArray.GetUpperBound(1) + 1 Then
                        m_bDataChanged = True
                    End If
                Else
                    m_bDataChanged = True
                End If
            ElseIf Not Information.IsArray(m_PartyDetail) And Information.IsArray(m_vPartyArray) Then
                m_bDataChanged = True
            ElseIf Information.IsArray(m_PartyDetail) And Not Information.IsArray(m_vPartyArray) Then
                m_bDataChanged = True
            End If
            If m_bDataChanged Then
                sEventDescription = Interaction.InputBox("Enter the Event Description", "Event Log", sEventDescription)

                m_lReturn = m_oBusiness.CreateEvent(AC_EVENT_TYPE_UPDATECLAIM, sEventDescription)
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '

    'Private Function SelectParty(ByRef vPartyCnt As Object, ByRef vShortName As Object, Optional ByRef vName As Object = Nothing, Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As Object = Nothing) As Integer
    '
    '
    'Dim result As Integer = 0

    'Dim oFindParty As ClassInterface
    'Dim vKeyArray As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Dim temp_oFindParty As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
    'oFindParty = temp_oFindParty
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Set appropriate key if agent only


    'If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then
    '
    ''ReDim vKeyArray(1, 0)

    'vKeyArray(0, 0) = "special_party"

    'vKeyArray(1, 0) = vSpecialParty
    '

    'm_lReturn = oFindParty.SetKeys(vKeyArray)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    '

    'm_lReturn = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lReturn = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'oFindParty.CallingAppName = ACApp
    '

    'm_lReturn = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lReturn = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'oFindParty.NotEditable = 1
    '

    'm_lReturn = oFindParty.Start()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lReturn = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
    '


    'vPartyCnt = oFindParty.PartyCnt


    'vShortName = oFindParty.ShortName

    'If Not Information.IsNothing(vName) Then


    'vName = oFindParty.LongName
    'End If


    'vResolvedName = oFindParty.ResolvedName
    'Else

    'If oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then
    'result = gPMConstants.PMEReturnCode.PMCancel
    'Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    '

    'm_lReturn = oFindParty.Terminate()
    '
    'oFindParty = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectPartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            '    ReDim m_ctlTabFirstLast(1, 4)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************
            '
            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cmdAddParty.Enabled = False
            End If

            ' Set the status of the Navigate button.
            cmdEditParty.Enabled = False
            cmdDeleteParty.Enabled = False

            m_lReturn = SetFirstLastControls()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwParty.Handle.ToInt32(), v_vShowRowSelect:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            lvwParty.Items.Clear()
            lvwParty.Columns.Clear()

            If m_sUnderwritingOrAgency = "A" And m_sPartyTypeCode = "" Then

                lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNameParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRES_CONTACTNAME, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRES_CONTACTNUMBER, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRES_PARTYTYPE, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)
            Else
                If PartyTypeCode <> ACDriver Then
                    ' Set Column headers from the resource strings

                    lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNameParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                    lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                    lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPhoneParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)
                Else
                    ' Set Column headers from the resource strings

                    lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNameParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                    lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLicenseNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                    lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDOB, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                    lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSex, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                    lvwParty.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)
                End If
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UserControlListViewAutoSize
    '
    ' Description: Resizes the column widths of a list view so that
    '              all information is visible.
    '
    '              If [bSizeHeaders] is true then it will also include
    '              the column headers in the sizing.
    '
    '              You might not want to resize the last column if you
    '              have a hidden date in it.
    '
    '              Note 1: It uses the control's parent's font to calculate
    '                      font width. So, if your control has the Verdanna
    '                      font, for example, then the form must have that
    '                      font too otherwise the sizes will be slightly out.
    '
    ' ***************************************************************** '
    Private Function UserControlListViewAutoSize(ByRef lvwList As ListView, Optional ByRef bSizeHeaders As Boolean = True, Optional ByRef bResizeLastColumn As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim vArray As Object
        Dim lWidth As Integer
        Dim iOffset, iUpper As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lvwList.Columns.Count = 0 Then
                Return result
            End If

            ' Make an array to store the widths in
            ReDim vArray(lvwList.Columns.Count - 1)

            ' Initialise the array

            iUpper = vArray.GetUpperBound(0)
            For iLoop1 As Integer = 1 To iUpper
                If bSizeHeaders Then




                    ''Developer Guide No: 10001
                    'vArray.SetValue(Parent.TextWidth(lvwList.Columns.Item(iLoop1 - 1).Text), iLoop1)


                Else

                    vArray(iLoop1) = -1
                End If
            Next iLoop1

            ' Go across each header and find the biggest item
            For iLoop1 As Integer = 1 To lvwList.Items.Count

                ' Do the first column



                'Developer Guide No: 10001
                'lWidth = Parent.TextWidth(lvwList.Items.Item(iLoop1 - 1).Text)



                If lWidth > CDbl(vArray(1)) Then

                    vArray(1) = lWidth
                End If

            Next iLoop1

            ' Add a little extra for the icon !


            vArray(1) = CDbl(vArray(1)) + 40

            ' Now do the subitems (other columns)

            For iLoop1 As Integer = 1 To vArray.GetUpperBound(0) - 1

                For iLoop2 As Integer = 1 To lvwList.Items.Count




                    'Developer Guide No: 10001
                    'lWidth = Parent.TextWidth(ListViewHelper.GetListViewSubItem(lvwList.Items.Item(iLoop2 - 1), iLoop1).Text)



                    If lWidth > CDbl(vArray(iLoop1 + 1)) Then

                        vArray(iLoop1 + 1) = lWidth
                    End If
                Next iLoop2

            Next iLoop1

            ' Dont do the last one if not wanted
            If bResizeLastColumn Then
                iOffset = 0
            Else
                iOffset = 1
            End If

            ' Now set the column header widths

            For iLoop1 As Integer = 1 To vArray.GetUpperBound(0) - iOffset


                lvwList.Columns.Item(iLoop1 - 1).Width = CInt(VB6.TwipsToPixelsX(CDbl(vArray(iLoop1))))
            Next iLoop1
            If iOffset = 1 Then



                lvwList.Columns.Item(vArray.GetUpperBound(0) - 1).Width = CInt(VB6.TwipsToPixelsX(CDbl(vArray(vArray.GetUpperBound(0)))))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UserControlListViewAutoSize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UserControlListViewAutoSize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateOK
    '
    '
    ' ***************************************************************** '
    Private Function ValidateOK() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdAddParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddParty.Click
        m_lReturn = AddPartyDetails()

        'TN20010829 - start
        If PartyTypeCode = "OTDRIVER" Then
            m_lReturn = EditPartyDetails()
        End If
        'TN20010829 - end
    End Sub

    Private Sub cmdDeleteParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteParty.Click
        m_lReturn = DeletePartyDetails()
    End Sub

    Private Sub cmdEditParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditParty.Click
        m_lReturn = EditPartyDetails()
    End Sub

    Private Sub lvwParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwParty.Click
        If lvwParty.FocusedItem Is Nothing Then
            If lvwParty.Items.Count > 0 Then
                lvwParty.FocusedItem = lvwParty.Items.Item(-1)

                'To Do Sanjay
                'lvwParty.DropHighlight = lvwParty.FocusedItem
                lvwParty.FocusedItem = lvwParty.FocusedItem
                Application.DoEvents()
            End If
        End If
    End Sub

    Private Sub lvwParty_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwParty.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        If lvwParty.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditParty.Enabled = False
            cmdDeleteParty.Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEditParty.Enabled = True
                cmdDeleteParty.Enabled = True
            End If
        End If

    End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()
        m_BackColor = m_def_BackColor
        m_ForeColor = m_def_ForeColor
        m_Enabled = m_def_Enabled

        'Developer Guide No 3
        m_Font = MyBase.Font
        m_BackStyle = m_def_BackStyle
        m_BorderStyle = m_def_BorderStyle
        m_sPartyTypeCode = m_def_PartyTypeCode
    End Sub

    '*************************************************************************************
    'Name        :UserControl_Resize
    'Description :Resize & repostion the controls on resize of the UserControl
    '*************************************************************************************
    Private Sub uctClaimParty_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        Const ACTwice As Integer = 2
        Const ACThrice As Integer = 3
        Const ACFormMargin As Integer = 45
        Const ACLeftMargin As Integer = 1
        Const ACHeightDifference As Integer = 8

        'Set minimum width
        If VB6.PixelsToTwipsX(Width) < ((ACTwice * ACCommandButtonWidth) + ACFormMargin) Then
            Width = VB6.TwipsToPixelsX((ACTwice * ACCommandButtonWidth) + ACFormMargin)
        End If
        ' Set minimum height
        If VB6.PixelsToTwipsY(Height) < ((ACCommandButtonHeight * ACThrice) + (ACTwice * ACVerticalGapBetweenTwoButtons) + ACcmdAddPartyTop) Then
            Height = VB6.TwipsToPixelsY((ACCommandButtonHeight * ACThrice) + (ACTwice * ACVerticalGapBetweenTwoButtons) + ACcmdAddPartyTop)
        End If

        ' Set buttons top,left,width & height with respect to cmdAddParty
        cmdAddParty.Top = VB6.TwipsToPixelsY(ACcmdAddPartyTop)
        cmdAddParty.Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
        cmdAddParty.Height = VB6.TwipsToPixelsY(ACCommandButtonHeight)
        cmdAddParty.Left = Width - cmdAddParty.Width

        cmdEditParty.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdAddParty.Top) + VB6.PixelsToTwipsY(cmdAddParty.Height) + ACVerticalGapBetweenTwoButtons)
        cmdEditParty.Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
        cmdEditParty.Height = VB6.TwipsToPixelsY(ACCommandButtonHeight)
        cmdEditParty.Left = cmdAddParty.Left

        cmdDeleteParty.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdEditParty.Top) + VB6.PixelsToTwipsY(cmdEditParty.Height) + ACVerticalGapBetweenTwoButtons)
        cmdDeleteParty.Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
        cmdDeleteParty.Height = VB6.TwipsToPixelsY(ACCommandButtonHeight)
        cmdDeleteParty.Left = cmdAddParty.Left

        ' Set list view's tops, left, height & width
        lvwParty.Top = VB6.TwipsToPixelsY(ACListViewTop)
        lvwParty.Left = VB6.TwipsToPixelsX(ACLeftMargin)
        lvwParty.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - ACHeightDifference)
        lvwParty.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - (VB6.PixelsToTwipsX(cmdAddParty.Width) + ACFormMargin))
        UserControlListViewAutoSize(lvwParty, True, True)
    End Sub

    '*************************************************************************************
    'Name        :UserControl_ReadProperties
    'Description :Load property values from storage Prperty bag,
    '             for persistance of data.
    '*************************************************************************************


    'Developer Guide No. 1
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))


        m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))


        m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))


        'Developer Guide No 3
        m_Font = PropBag.ReadProperty("Font", MyBase.Font)


        m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


        m_BorderStyle = CInt(PropBag.ReadProperty("BorderStyle", m_def_BorderStyle))


        m_sPartyTypeCode = CStr(PropBag.ReadProperty("PartyTypeCode", m_def_PartyTypeCode))

    End Sub

    '*************************************************************************************
    'Name        :UserControl_WriteProperties
    'Description :Write property values to storage Porperty bag,
    '             for persistance of data.
    '*************************************************************************************


    'Developer Guide No. 1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

        PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

        PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)


        'Developer Guide No 3
        PropBag.WriteProperty("Font", m_Font, MyBase.Font)

        PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

        PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

        PropBag.WriteProperty("PartyTypeCode", m_sPartyTypeCode, m_def_PartyTypeCode)
    End Sub
End Class
