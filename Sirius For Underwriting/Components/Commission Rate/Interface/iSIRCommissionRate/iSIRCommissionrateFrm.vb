Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
'Friend Partial Class frmInterface
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 5th September 2000
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '
    'replaced iPMFunc.GetResData with GetResData in the whole document

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPartyTypeId As Integer
    Private m_lPartyId As Integer
    Private m_lProductId As Integer
    Private m_lRiskTypeId As Integer
    Private m_lCommissionBandId As Integer
    ' CMG / PB 23072002 New Commission Grouping functionality
    Private m_lCommissionGroupId As Integer
    ' CMG End
    Private m_lTransactionTypeId As Integer
    Private m_lTaxGroupID As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSirCommissionRate.Business
    Private m_oLookup As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    'Variables to store the party details
    Private m_vPartyDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast() As Control

    'Define an instance of formfields
    Private m_oFormfields As iPMFormControl.FormFields

    'Private m_oInterface As ClassInterface
    Private m_oInterface As Interface_Renamed
    'SAGICOR WPR14
    Private m_lCommissionLevelID As Integer
    'SAGICOR WPR14
    Private m_lCommissionLevel As Integer

    ' {* USER DEFINED CODE (Begin) *}

    Public WriteOnly Property PartyTypeId() As Integer
        Set(ByVal Value As Integer)

            ' Set the objects parameter value.
            m_lPartyTypeId = Value

        End Set
    End Property

    Public WriteOnly Property PartyId() As Integer
        Set(ByVal Value As Integer)

            'Set the objects parameter value
            m_lPartyId = Value

        End Set
    End Property

    Public WriteOnly Property ProductId() As Integer
        Set(ByVal Value As Integer)

            'set the objects parameter value
            m_lProductId = Value

        End Set
    End Property

    Public WriteOnly Property RiskTypeId() As Integer
        Set(ByVal Value As Integer)

            'Set the objects parameter value
            m_lRiskTypeId = Value

        End Set
    End Property
    Public WriteOnly Property CommisionBandId() As Integer
        Set(ByVal Value As Integer)
            'Set the objects parameter value
            m_lCommissionBandId = Value

        End Set
    End Property
    ' CMG / PB 23072002 New Commission Grouping functionality
    Public WriteOnly Property CommissionGroupId() As Integer
        Set(ByVal Value As Integer)
            'Set the objects parameter value
            m_lCommissionGroupId = Value

        End Set
    End Property

    Public WriteOnly Property TransactionTypeId() As Integer
        Set(ByVal Value As Integer)
            'Set the objects parameter value
            m_lTransactionTypeId = Value

        End Set
    End Property
    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property
 
    ''' <summary>
    ''' SetInterfaceDefaults : Sets all of the interface default values.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            Return nResult

        Catch excep As System.Exception

            ' Error Section.

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, _
                               vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

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
            'ReDim m_ctlTabFirstLast(1, )

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

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeletebutton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            lblPartyType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyTypeLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblParty.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblProduct.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProductLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblRiskType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskTypeLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblCommissionband.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCommissionBandLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' CMG / PB 23072002 New Commission Grouping functionality

            lblCommissionGroup.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCommissionGroupLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblTransactionType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTransactionTypeLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get all of the lookup values.

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=m_vLookupDetails)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
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
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox, Optional ByVal bShowAll As Boolean = False, Optional ByVal bShowNone As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)) = sLookupTable Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            'Clear the contents of the control
            ctlLookup.Items.Clear()

            If bShowNone Then
                'Add "None" as the default item
                m_lReturn = CType(AddNonetoCombo(ctlLookup), gPMConstants.PMEReturnCode)
            Else
                'Add "All" as the default item
                m_lReturn = CType(AddAlltoCombo(ctlLookup, bShowAll), gPMConstants.PMEReturnCode)
            End If

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)

                'start
                Dim ctlLookup_NewIndex As Integer = -1
                If sLookupTable = "Transaction_type" Then
                    Select Case CInt(m_vLookupDetails(ACDetailKey, lCntr))
                        Case ACTTNewBusiness, ACTTCancelPolicy, ACTTMTA, ACTTRenewals
                            ' Add the details to the control.                            
                            ctlLookup_NewIndex = ctlLookup.Items.Add(New VB6.ListBoxItem(CStr(m_vLookupDetails(ACDetailDesc, lCntr)), CInt(m_vLookupDetails(ACDetailKey, lCntr))))
                    End Select
                Else
                    ' Add the details to the control.
                    ctlLookup_NewIndex = ctlLookup.Items.Add(New VB6.ListBoxItem(CStr(m_vLookupDetails(ACDetailDesc, lCntr)), CInt(m_vLookupDetails(ACDetailKey, lCntr))))

                End If

                ' Check if this is the selected index.
                
                If Convert.ToString(m_vLookupValues(ACValueID, lRow)).Equals(m_vLookupDetails(ACDetailKey, lCntr)) Then
                    ctlLookup.SelectedIndex = ctlLookup_NewIndex
                End If
                'end
            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.

            'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then
            If Convert.ToString(m_vLookupValues(ACValueID, lRow)) = "" Then
                ctlLookup.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboCommissionband_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCommissionband.SelectedIndexChanged

        'Set the property and populate the listview again
        CommisionBandId = VB6.GetItemData(cboCommissionband, cboCommissionband.SelectedIndex)

        m_lReturn = CType(FilterCommissionRatings(), gPMConstants.PMEReturnCode)

    End Sub

    ' CMG / PB 23072002 New Commission Grouping functionality
    Private Sub cboCommissionGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCommissionGroup.SelectedIndexChanged

        'CMG/PB 12/07/2002 Set the property and populate the listview again
        CommissionGroupId = VB6.GetItemData(cboCommissionGroup, cboCommissionGroup.SelectedIndex)

        m_lReturn = CType(FilterCommissionRatings(), gPMConstants.PMEReturnCode)

    End Sub

    'SAGICOR WPR14
    Private Sub cboCommissionLevel_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboCommissionLevel.SelectedIndexChanged

        'Set the property and populate the listview again
        If cboCommissionLevel.SelectedIndex > 0 Then
            m_lCommissionLevelID = VB6.GetItemData(cboCommissionLevel, cboCommissionLevel.SelectedIndex)
        Else
            m_lCommissionLevelID = -1
        End If

        'Get the newly selected party type
        PartyTypeId = VB6.GetItemData(cboPartyType, cboPartyType.SelectedIndex)

        m_lReturn = MainModule.frmInterface.PopulatePartyCombo(cboParty, m_lPartyTypeId, True, m_lCommissionLevelID)
        If cboParty.SelectedIndex < 0 Then
            cboParty.SelectedIndex = 0
        End If

        m_lReturn = FilterCommissionRatings()
    End Sub
    'SAGICOR WPR14
    Private Sub cboCommissionLevel_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboCommissionLevel.TextChanged

        If cboCommissionLevel.SelectedIndex <> 0 Then
            're-populate party combo that should filter over selected commission level.
            SetComboItem(cboPartyType, -1)
        End If

    End Sub

    Private isInitializingComponent As Boolean

    Private Sub cboParty_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboParty.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        '
        '        'Set the property and populate the listview again
        '        PartyId = cboParty.ItemData(cboParty.ListIndex)
        '
        '        m_lReturn = FilterCommissionRatings()
        'SAGICOR WPR14 :cboCommissionLevel enabled when no party is selected

        If cboParty.SelectedIndex = 0 Then
            cboCommissionLevel.Enabled = gPMConstants.PMEReturnCode.PMTrue
        Else
            cboCommissionLevel.Enabled = gPMConstants.PMEReturnCode.PMFalse
        End If

    End Sub

    Private Sub cboParty_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboParty.SelectedIndexChanged
        'Set the property and populate the listview again
        PartyId = VB6.GetItemData(cboParty, cboParty.SelectedIndex)

        m_lReturn = CType(FilterCommissionRatings(), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cboPartyType_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPartyType.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        '        'Get the newly selected party type
        '        PartyTypeId = cboPartyType.ItemData(cboPartyType.ListIndex)
        '
        '        m_lReturn = PopulatePartyCombo(cboParty, PartyTypeId)
        '
        '        If m_lReturn = PMTrue Then
        '
        '            m_lReturn = FilterCommissionRatings()
        '
        '        End If

    End Sub

    Private Sub cboPartyType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPartyType.SelectedIndexChanged

        'CMG / PB 15/7/2002 Only enable the commission group if party type selected
        If cboPartyType.SelectedIndex = 0 Then
            'Reset the group, no longer allowed
            SetComboItem(cboCommissionGroup, -1)
            cboCommissionGroup.Enabled = gPMConstants.PMEReturnCode.PMFalse
        Else
            cboCommissionGroup.Enabled = gPMConstants.PMEReturnCode.PMTrue
        End If

        'Get the newly selected party type
        PartyTypeId = VB6.GetItemData(cboPartyType, cboPartyType.SelectedIndex)

        m_lReturn = CType(PopulatePartyCombo(cboParty, m_lPartyTypeId, True, m_lCommissionLevelID), gPMConstants.PMEReturnCode)
        If cboParty.SelectedIndex < 0 Then
            cboParty.SelectedIndex = 0
        End If

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

            m_lReturn = CType(FilterCommissionRatings(), gPMConstants.PMEReturnCode)

        End If
        cmdFindParty.Enabled = cboPartyType.SelectedIndex > 1
    End Sub

    Private Sub cboProduct_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProduct.SelectedIndexChanged

        ProductId = VB6.GetItemData(cboProduct, cboProduct.SelectedIndex)

        m_lReturn = CType(FilterCommissionRatings(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cboRiskType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRiskType.SelectedIndexChanged
        RiskTypeId = VB6.GetItemData(cboRiskType, cboRiskType.SelectedIndex)

        m_lReturn = CType(FilterCommissionRatings(), gPMConstants.PMEReturnCode)
    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cboTaxGroupID_Change) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cboTaxGroupID_Change()
    '
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cboTaxGroupID_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cboTaxGroupID_Click()
    'Set the property and populate the listview again
    'm_lTaxGroupID = VB6.GetItemData(cboTaxGroup, cboTaxGroup.SelectedIndex)
    '
    'm_lReturn = CType(FilterCommissionRatings(), gPMConstants.PMEReturnCode)
    'End Sub

    Private Sub cboTaxGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxGroup.SelectedIndexChanged

        m_lTaxGroupID = VB6.GetItemData(cboTaxGroup, cboTaxGroup.SelectedIndex)

        m_lReturn = CType(FilterCommissionRatings(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cboTransactionType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTransactionType.SelectedIndexChanged

        TransactionTypeId = VB6.GetItemData(cboTransactionType, cboTransactionType.SelectedIndex)

        m_lReturn = CType(FilterCommissionRatings(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        MainModule.frmDetail.Mode = gPMConstants.PMEComponentAction.PMAdd

        'Set the Default values in the combos of Detail form
        m_lReturn = CType(SetComboItem(MainModule.frmDetail.cboPartyType, VB6.GetItemData(cboPartyType, cboPartyType.SelectedIndex)), gPMConstants.PMEReturnCode)
        m_lReturn = CType(PopulatePartyCombo(MainModule.frmDetail.cboParty, VB6.GetItemData(cboPartyType, cboPartyType.SelectedIndex)), gPMConstants.PMEReturnCode)
        m_lReturn = CType(SetComboItem(MainModule.frmDetail.cboParty, VB6.GetItemData(cboParty, cboParty.SelectedIndex)), gPMConstants.PMEReturnCode)
        m_lReturn = CType(SetComboItem(MainModule.frmDetail.cboProduct, VB6.GetItemData(cboProduct, cboProduct.SelectedIndex)), gPMConstants.PMEReturnCode)
        m_lReturn = CType(SetComboItem(MainModule.frmDetail.cboRiskType, VB6.GetItemData(cboRiskType, cboRiskType.SelectedIndex)), gPMConstants.PMEReturnCode)
        m_lReturn = CType(SetComboItem(MainModule.frmDetail.cboTransactionType, VB6.GetItemData(cboTransactionType, cboTransactionType.SelectedIndex)), gPMConstants.PMEReturnCode)
        m_lReturn = CType(SetComboItem(MainModule.frmDetail.cboCommissionband, VB6.GetItemData(cboCommissionband, cboCommissionband.SelectedIndex)), gPMConstants.PMEReturnCode)
        ' CMG / PB 23072002 New Commission Grouping functionality
        m_lReturn = CType(SetComboItem(MainModule.frmDetail.cboCommissionGroup, VB6.GetItemData(cboCommissionGroup, cboCommissionGroup.SelectedIndex)), gPMConstants.PMEReturnCode)

        'Set the default values for the controls
        m_lReturn = MainModule.frmDetail.m_oFormfields.FormatControl(MainModule.frmDetail.txtRate, "0.0")
        'Changed to line to view the date as per VB Application.
        'm_lReturn = MainModule.frmDetail.m_oFormfields.FormatControl(MainModule.frmDetail.txtEffectiveDate, DateTime.Today)
        m_lReturn = MainModule.frmDetail.m_oFormfields.FormatControl(MainModule.frmDetail.txtEffectiveDate, CDate(DateTime.Today).ToString("dd MMMM yyyy"))
        'Start - Renuka - (WPR64 Paralleling)
        m_lReturn = MainModule.frmDetail.m_oFormfields.FormatControl(MainModule.frmDetail.txtMaximumRate, "")
        'End - Renuka - (WPR64 Paralleling)
        MainModule.frmDetail.chkIsvalue.CheckState = CheckState.Unchecked
        m_lReturn = CType(SetComboItem(MainModule.frmDetail.cboTaxGroup, VB6.GetItemData(cboTaxGroup, cboTaxGroup.SelectedIndex)), gPMConstants.PMEReturnCode)
        'SAGICOR WPR14
        If cboCommissionLevel.SelectedIndex <> -1 Then
            m_lReturn = CType(SetComboItem((MainModule.frmDetail.cboCommissionLevel), VB6.GetItemData(cboCommissionLevel, cboCommissionLevel.SelectedIndex)), gPMConstants.PMEReturnCode)
        End If
        iPMFunc.CenterForm(MainModule.frmDetail)

        'Show the Details screen in New mode
        MainModule.frmDetail.ShowDialog()
        m_lReturn = PopulateCommissionlevelCombo(cboCombo:=cboCommissionLevel, bShowAll:=True)

    End Sub

    ' PRIVATE Methods (End)
    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        'Delete the selected item in the listview
        'Added the check to check if a particular row is selected or not
        'start
        Dim nItem As Integer

        If Information.IsNothing(lvwCommissionRate.FocusedItem) Then
            nItem = lvwCommissionRate.Items.Item(0).Index + 1
        Else
            nItem = lvwCommissionRate.FocusedItem.Index + 1
        End If
        'If lvwCommissionRate.SelectedItem.Ghosted Then
        If Information.IsNothing(lvwCommissionRate.FocusedItem) Then
            If lvwCommissionRate.Items.Item(0).ForeColor.Equals(Color.Gray) Then
                m_lReturn = CType(UnDeleteCommissionRate(nItem), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(DeleteCommissionRate(nItem), gPMConstants.PMEReturnCode)
            End If

        Else

            If lvwCommissionRate.FocusedItem.ForeColor.Equals(Color.Gray) Then
                m_lReturn = CType(UnDeleteCommissionRate(nItem), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(DeleteCommissionRate(nItem), gPMConstants.PMEReturnCode)
            End If
        End If
        'lvwCommissionRate.ListItems.Remove (nItem)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

            'Repopulate the list view
            m_lReturn = CType(FilterCommissionRatings(), gPMConstants.PMEReturnCode)

        End If

        'end
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        Dim lvItem As ListViewItem
        If Information.IsNothing(lvwCommissionRate.FocusedItem) Then
            lvItem = lvwCommissionRate.Items.Item(0)
        Else
            lvItem = lvwCommissionRate.FocusedItem
        End If
        'Set the controls
        SetComboItem(MainModule.frmDetail.cboPartyType, CInt(ListViewHelper.GetListViewSubItem(lvItem, ACPartyTypeCol).Text))
        SetComboItem(MainModule.frmDetail.cboProduct, CInt(ListViewHelper.GetListViewSubItem(lvItem, ACProductCol).Text))
        SetComboItem(MainModule.frmDetail.cboRiskType, CInt(ListViewHelper.GetListViewSubItem(lvItem, ACRiskTypeCol).Text))
        SetComboItem(MainModule.frmDetail.cboTransactionType, CInt(ListViewHelper.GetListViewSubItem(lvItem, ACTransactionTypeCol).Text))
        SetComboItem(MainModule.frmDetail.cboCommissionband, CInt(ListViewHelper.GetListViewSubItem(lvItem, ACCommissionBandCol).Text))
        ' CMG / PB 23072002 New Commission Grouping functionality
        SetComboItem(MainModule.frmDetail.cboCommissionGroup, CInt(ListViewHelper.GetListViewSubItem(lvItem, ACCommissionGroupCol).Text))

        MainModule.frmDetail.chkIsvalue.CheckState = ListViewHelper.GetListViewSubItem(lvItem, ACIsValueCol).Text

        If MainModule.frmDetail.chkIsvalue.CheckState = CheckState.Checked Then
            'frmDetail.m_oFormfields.Item("TxtRate").FieldFormat = PMFormatCurrency

            'start
            'frmDetail.m_oFormfields.Item(CStr(1)).set_FieldFormat(gPMConstants.PMEFormatStyle.PMFormatCurrency)
            MainModule.frmDetail.m_oFormfields.Item(1).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
            'Start - Renuka - (WPR64 Paralleling)
            'frmDetail.m_oFormfields.Item(CStr(3)).set_FieldFormat(gPMConstants.PMEFormatStyle.PMFormatCurrency)
            MainModule.frmDetail.m_oFormfields.Item(3).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
            'End - Renuka - (WPR64 Paralleling)
        Else
            'frmDetail.m_oFormfields.Item("TxtRate").FieldFormat = PMFormatPercent
            'frmDetail.m_oFormfields.Item(CStr(1)).set_FieldFormat(gPMConstants.PMEFormatStyle.PMFormatPercent)
            MainModule.frmDetail.m_oFormfields.Item(1).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
            'Start - Renuka - (WPR64 Paralleling)
            'frmDetail.m_oFormfields.Item(CStr(3)).set_FieldFormat(gPMConstants.PMEFormatStyle.PMFormatPercent)
            MainModule.frmDetail.m_oFormfields.Item(3).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
            'end
            'End - Renuka - (WPR64 Paralleling)
        End If
        'm_lReturn = frmDetail.m_oFormfields.FormatControl(frmDetail.txtRate, lvitem.SubItems(ACRateCol))
        'm_lReturn = frmDetail.m_oFormfields.FormatControl(frmDetail.txtEffectiveDate, lvitem.SubItems(ACEffectiveDateCol))
        'Since the data is already formatted in the listview, no need to format again
        MainModule.frmDetail.txtRate.Text = ListViewHelper.GetListViewSubItem(lvItem, ACRateCol).Text
        MainModule.frmDetail.txtEffectiveDate.Text = ListViewHelper.GetListViewSubItem(lvItem, ACEffectiveDateCol).Text

        'Start - Renuka - (WPR64 Paralleling)
        MainModule.frmDetail.txtMaximumRate.Text = ListViewHelper.GetListViewSubItem(lvItem, ACMaximumRateCol).Text
        Dim sCommisionrate As String = ""
        If ListViewHelper.GetListViewSubItem(lvItem, ACRateCol).Text.IndexOf("%"c) >= 0 Then
            sCommisionrate = ListViewHelper.GetListViewSubItem(lvItem, ACRateCol).Text

            sCommisionrate = CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatString, sCommisionrate))
        Else
            sCommisionrate = ListViewHelper.GetListViewSubItem(lvItem, ACRateCol).Text
        End If
        MainModule.frmDetail.txtMaximumRate.Enabled = Not (sCommisionrate.Length <= 0 Or sCommisionrate <= "0" Or sCommisionrate <= "0.00")
        'End - Renuka - (WPR64 Paralleling)
        'PSL 29/07/2003 5616 need old date and new date if they change it
        MainModule.frmDetail.dtOldDate = CDate(ListViewHelper.GetListViewSubItem(lvItem, ACEffectiveDateCol).Text)

        SetComboItem(MainModule.frmDetail.cboTaxGroup, CInt(ListViewHelper.GetListViewSubItem(lvItem, ACTaxGroupID).Text))
        'SAGICOR WPR14
        'If lvItem.SubItems(ACCommissionLevel) <> "" And lvItem.SubItems(ACCommissionLevel) <> "ALL" Then
        SetComboItem(MainModule.frmDetail.cboCommissionLevel, CInt(ListViewHelper.GetListViewSubItem(lvItem, ACCommissionLevelID).Text))
        SetComboItem(MainModule.frmDetail.cboParty, CInt(ListViewHelper.GetListViewSubItem(lvItem, ACPartyCol).Text), ListViewHelper.GetListViewSubItem(lvItem, 1).Text)

        'End If
        MainModule.frmDetail.Mode = gPMConstants.PMEComponentAction.PMEdit

        iPMFunc.CenterForm(MainModule.frmDetail)

        'Show the Details form in Edit mode
        MainModule.frmDetail.ShowDialog()
        m_lReturn = PopulateCommissionlevelCombo(cboCombo:=cboCommissionLevel, bShowAll:=True)

    End Sub

    Private Sub cmdFindParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindParty.Click
        Dim vCnt As Integer
        Dim vShortName As String = ""
        Dim vName As Object
        Dim lRows As Integer
        Dim iPartyTypeID As Integer
        Dim vCommissionLevelID As Object

        Try

            iPartyTypeID = VB6.GetItemData(cboPartyType, cboPartyType.SelectedIndex)

            If iPartyTypeID = -1 Then
                iPartyTypeID = 0
            End If

            'SAGICOR WPR14
            If cboCommissionLevel.SelectedIndex <> 0 Then
                vCommissionLevelID = VB6.GetItemData(cboCommissionLevel, cboCommissionLevel.SelectedIndex)
            Else
                vCommissionLevelID = -1
            End If

            m_lReturn = CType(m_oInterface.SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=CStr(vName), vPartyTypeID:=iPartyTypeID, vCommissionLevelID:=vCommissionLevelID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            lRows = cboParty.Items.Count - 1
            If vShortName <> "" Then
                For lRow As Integer = 0 To lRows
                    If vCnt = CInt(VB6.GetItemData(cboParty, lRow)) Then
                        cboParty.SelectedIndex = lRow
                        Exit For
                    End If
                Next lRow
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdFindParty_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' call help

        'm_lReturn = CType(PMHelpFunc.ShowHelp(dlgHelp:=dlgHelp, lContextID:=ScreenhelpID), gPMConstants.PMEReturnCode)
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenhelpID), gPMConstants.PMEReturnCode)
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSirCommissionRate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            'Get an instance of the lookup object
            Dim temp_m_oLookup As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oLookup, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oLookup = temp_m_oLookup

            'Set the product familiy of lookup

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            'Fill the Lookup array

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ''' <summary>
    ''' Interface Load Event
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.
        Dim vResultArray(,) As Object

        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwCommissionRate.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            'Add the controls in the Detail form to the formfields collection
            m_oFormfields = New iPMFormControl.FormFields()

            'm_oInterface = New ClassInterface()
            m_oInterface = New Interface_Renamed()

            m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=txtRate1, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, _
                                                      lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=CType(False, gPMConstants.PMEMandatoryStatus))

            m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=txtDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, _
                                                      lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=CType(False, gPMConstants.PMEMandatoryStatus))

            'Start - Renuka - (WPR64 Paralleling)
            m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=txtMaximumRate1, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, _
                                                      lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=CType(False, gPMConstants.PMEMandatoryStatus))
            'End - Renuka - (WPR64 Paralleling)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", _
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            'Get the values of Commission Maintenance from the database

            m_lReturn = m_oBusiness.GetAllCommissionArrangement(vResultArray:=vResultArray)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'Fill the retrieved data into the List view
                lvwCommissionRate.Items.Clear()

                If Information.IsArray(vResultArray) Then

                    m_lReturn = CType(PopulateCommissionListview(vResultArray), gPMConstants.PMEReturnCode)

                End If
            End If

            ' CMG / PB 23072002 New Commission Grouping lookup
            ReDim m_vLookupValues(3, 7)

            m_vLookupValues(0, 0) = "Party_Agent_Type"
            m_vLookupValues(0, 1) = "Product"
            m_vLookupValues(0, 2) = "Risk_type"
            m_vLookupValues(0, 3) = "Transaction_type"
            m_vLookupValues(0, 4) = "commission_band"
            m_vLookupValues(0, 5) = "commission_grouping"
            m_vLookupValues(0, 6) = "tax_group"
            'SAGICOR WPR14
            m_vLookupValues(0, 7) = "commission_level"

            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            'Fill the lookup values in the detail form
            MainModule.frmDetail.Hide()

            m_lReturn = CType(AddAlltoCombo(cboParty, True), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddAlltoCombo(MainModule.frmDetail.cboParty), gPMConstants.PMEReturnCode)

            RemoveHandlers()
            cboParty.SelectedIndex = 0
            MainModule.frmDetail.cboParty.SelectedIndex = 0

            m_lReturn = CType(PopulatePartyAgentType(r_cboControl:=cboPartyType), gPMConstants.PMEReturnCode)
            m_lReturn = CType(PopulatePartyAgentType(r_cboControl:=MainModule.frmDetail.cboPartyType, bShowAll:=True), gPMConstants.PMEReturnCode)

            m_lReturn = CType(GetLookupDetails("Product", cboProduct, True), gPMConstants.PMEReturnCode)
            m_lReturn = CType(GetLookupDetails("Product", MainModule.frmDetail.cboProduct), gPMConstants.PMEReturnCode)

            m_lReturn = CType(GetLookupDetails("Risk_type", cboRiskType, True), gPMConstants.PMEReturnCode)
            m_lReturn = CType(GetLookupDetails("Risk_type", MainModule.frmDetail.cboRiskType), gPMConstants.PMEReturnCode)

            m_lReturn = CType(GetLookupDetails("Transaction_type", cboTransactionType, True), gPMConstants.PMEReturnCode)
            m_lReturn = CType(GetLookupDetails("Transaction_type", MainModule.frmDetail.cboTransactionType), gPMConstants.PMEReturnCode)

            m_lReturn = CType(GetLookupDetails("commission_band", cboCommissionband, True), gPMConstants.PMEReturnCode)
            m_lReturn = CType(GetLookupDetails("commission_band", MainModule.frmDetail.cboCommissionband), gPMConstants.PMEReturnCode)

            m_lReturn = CType(GetLookupDetails("commission_grouping", cboCommissionGroup, True), gPMConstants.PMEReturnCode)
            m_lReturn = CType(GetLookupDetails("commission_grouping", MainModule.frmDetail.cboCommissionGroup), gPMConstants.PMEReturnCode)

            m_lReturn = CType(GetLookupDetails("tax_group", cboTaxGroup, True), gPMConstants.PMEReturnCode)
            m_lReturn = CType(GetLookupDetails("tax_group", MainModule.frmDetail.cboTaxGroup, , True), gPMConstants.PMEReturnCode)
            'SAGICOR WPR14
            m_lReturn = CType(GetLookupDetails("commission_level", MainModule.frmDetail.cboCommissionLevel, , True), gPMConstants.PMEReturnCode)

            m_lReturn = PopulateCommissionlevelCombo(cboCommissionLevel, vResultArray, True, m_lCommissionLevelID)

            AddHandlers()
            ' {* USER DEFINED CODE (End) *}

            m_lReturn = m_oBusiness.GetallParties(m_vPartyDetails)

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            'm_lReturn& = BusinessToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            'SAGICOR WPR14  - cboCommissionLevel enabled only when no party is selected.

            If cboParty.SelectedIndex = 0 Then
                cboCommissionLevel.Enabled = True
            Else
                cboCommissionLevel.Enabled = False
            End If

            cmdFindParty.Enabled = cboPartyType.SelectedIndex > 1

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            MainModule.frmDetail.Close()
            'Added the following code to end the instance of m_oFormfields
            'start
            m_oFormfields.Dispose()

            m_oFormfields = Nothing
            'end

            ' Terminate the business object
            '    m_lReturn& = m_oBusiness.Terminate()

            ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse

            ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to terminate the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            'vMethod:="Form_QueryUnload"
            'End If

            ' Destroy the instance of the business object
            ' from memory.
            '    Set m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            m_oInterface = Nothing
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Call the
            m_lStatus = ProcessCommand()

            Me.Hide()

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            'm_lReturn& = m_oGeneral.ProcessCommand()

            ' Check the return value.
            '   If (m_lReturn& = PMTrue) Then
            ' Everything OK, so we can hide the interface.
            Me.Hide()
            '  End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Events (End)

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Perform the process for the command button pressed by the user.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            'Set the Return Value
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            'If We are in EDIT mode ,then delete the existing Rating Section and Perils
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

            End If

            'Set the return Value

            Return m_lReturn

        Catch excep As System.Exception

            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Add 'All' and 'Show All' to combo.
    ''' </summary>
    ''' <param name="cboCombo"></param>
    ''' <param name="bShowAll"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddAlltoCombo(ByRef cboCombo As ComboBox, Optional ByVal bShowAll As Boolean = False) As Integer
        Const sAllString As String = "All"
        Const sShowAllString As String = "<Show All>"

        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        'start
        Dim cboCombo_NewIndex As Integer = -1
        If bShowAll Then

            'Add into the combo           
            cboCombo_NewIndex = cboCombo.Items.Add(New VB6.ListBoxItem(sShowAllString, -1))

        End If

        'Add into the combo
        If cboCombo.Name <> "cboTaxGroup" Then
            cboCombo_NewIndex = cboCombo.Items.Add(New VB6.ListBoxItem(sAllString, 0))
        End If
        'end

        Return m_lReturn

    End Function

    ''' <summary>
    ''' Add 'None' to Combo
    ''' </summary>
    ''' <param name="cboCombo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddNonetoCombo(ByRef cboCombo As ComboBox) As Integer

        Const sShowNoneString As String = "<None>"

        'Add into the combo
        Dim cboCombo_NewIndex As Integer = -1
        cboCombo_NewIndex = cboCombo.Items.Add(New VB6.ListBoxItem(sShowNoneString, 0))

        Return m_lReturn

    End Function
    ''' <summary>
    ''' Populate the party combo with the parties of the given party type.
    ''' </summary>
    ''' <param name="cboCombo"></param>
    ''' <param name="v_lPartytypeId"></param>
    ''' <param name="bShowAll"></param>
    ''' <param name="v_lCommissionLevelID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PopulatePartyCombo(ByRef cboCombo As ComboBox, _
                                       ByVal v_lPartytypeId As Integer, _
                                       Optional ByVal bShowAll As Boolean = False, _
                                       Optional ByVal v_lCommissionLevelID As Integer = 0) As Integer
        Dim nResult As Integer = 0
        Try
            Const nColPartytype As Integer = 0
            Const nColPartyCnt As Integer = 1
            Const nColShortname As Integer = 2
            Const nColCommissionlevel As Integer = 3

            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)

            cboCombo.Items.Clear()

            'Add the "All" as default
            m_lReturn = CType(AddAlltoCombo(cboCombo, bShowAll), PMEReturnCode)
            Dim bMatchFound As Boolean = False

            If v_lPartytypeId > 0 Then

                'Browse thro the party details array and add the parties that belong to the given party type
                For nCount As Integer = m_vPartyDetails.GetLowerBound(1) To m_vPartyDetails.GetUpperBound(1)
                    If ToSafeInteger(m_vPartyDetails(nColPartytype, nCount)) = v_lPartytypeId AndAlso _
                        (ToSafeLong(m_vPartyDetails(nColCommissionlevel, nCount)) = v_lCommissionLevelID OrElse (v_lCommissionLevelID < 1 AndAlso bShowAll = True)) Then
                        Dim cboCombo_NewIndex As Integer = -1
                        cboCombo_NewIndex = cboCombo.Items.Add(New VB6.ListBoxItem(CStr(m_vPartyDetails(nColShortname, nCount)), _
                                                                                   CInt(m_vPartyDetails(nColPartyCnt, nCount))))
                        bMatchFound = True
                    End If

                Next

            End If

            If bMatchFound = False Then
                'Set the first item as default
                cboCombo.SelectedIndex = 0
            End If

            nResult = m_lReturn


            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="PopulatePartyCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulatePartyCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        Finally
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
        End Try
    End Function
    ' Function : FiltercommissionRatings
    ' Purpose : Get the only commission ratings which satisfies the items selected in the combo boxes
    '                and display them in the listview
    ' Person : S.Rajan
    ' Date   : 7th September 2000
    '----------------------------------------------------------------------
    Public Function FilterCommissionRatings() As Integer
        Dim result As Integer = 0
        Try

            Dim vntResult(,) As Object

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Get the Array of commission ratings from the business object
            ' CMG / PB 23072002 New Commission Grouping functionality

            m_lReturn = m_oBusiness.GetCommissionArrangement(m_lPartyTypeId, m_lPartyId, m_lRiskTypeId, m_lProductId, m_lTransactionTypeId, m_lCommissionBandId, m_lCommissionGroupId, m_lTaxGroupID, vntResult, m_lCommissionLevelID)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'Populate the listview using the array

                m_lReturn = CType(PopulateCommissionListview(vntResult), gPMConstants.PMEReturnCode)

                If lvwCommissionRate.Items.Count = 0 Then

                    cmdEdit.Enabled = False
                    cmdDelete.Enabled = False

                Else

                    cmdEdit.Enabled = True
                    cmdDelete.Enabled = True

                End If

            End If
            result = m_lReturn

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception

            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FilterCommissionRatings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FilterCommissionRatings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        End Try
    End Function
    ' Function : AddCommissionArrangement
    ' Purpose :  Add the new commission arragement
    ' Person : S.Rajan
    ' Date   : 8th September 2000
    '
    '
    ' History
    ' CMG / PB 23072002 New Commission Grouping functionality
    'Start - Renuka - (WPR64 Paralleling)
    'Added a optional parameter v_cMaximumRate
    'End - Renuka - (WPR64 Paralleling)
    '---------------------------------------------------------------------------------------------------------
    Public Function AddcommissionArrangement(ByVal v_lPartytypeId As Integer, ByVal v_lPartyId As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_lProductId As Integer, ByVal v_lTransactionTypeId As Integer, ByVal v_lCommissionBandId As Integer, ByVal v_lCommissionGroupId As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_cRate As Double, ByVal v_bIsrate As Integer, ByVal v_lTaxGroupID As Integer, Optional ByVal v_cMaximumRate As Decimal = 0, Optional ByVal v_lCommissionLevelID As Integer = 0, Optional ByVal v_sUniqueId As String = "") As Integer

        Dim result As Integer = 0
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Add the commission Rate into the database
            'Start - Renuka - (WPR64 Paralleling)

            If v_cMaximumRate.Equals(0) Then

                m_lReturn = m_oBusiness.AddCommissionArrangement(v_lPartytypeId, v_lPartyId, v_lRiskTypeId, v_lProductId, v_lTransactionTypeId, v_lCommissionBandId, v_lCommissionGroupId, v_dtEffectiveDate, v_cRate, v_bIsrate, v_lTaxGroupID, 0, v_lCommissionLevelID, v_sUniqueId)
            Else

                m_lReturn = m_oBusiness.AddCommissionArrangement(v_lPartytypeId, v_lPartyId, v_lRiskTypeId, v_lProductId, v_lTransactionTypeId, v_lCommissionBandId, v_lCommissionGroupId, v_dtEffectiveDate, v_cRate, v_bIsrate, v_lTaxGroupID, v_cMaximumRate, v_lCommissionLevelID, v_sUniqueId)

            End If
            'End - Renuka - (WPR64 Paralleling)

            'Return the result
            result = m_lReturn

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception

            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddcommissionArrangement  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddcommissionArrangement ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        End Try
    End Function

    ' Function : EditCommissionArrangement
    ' Purpose :  Edit the selected Commission arrangemtn
    ' Person : S.Rajan
    ' Date   : 8th September 2000
    '
    ' History
    ' CMG / PB 23072002 New Commission Grouping functionality
    'Start - Renuka - (WPR64 Paralleling)
    'Added a optional parameter v_cMaximumRate
    'End - Renuka - (WPR64 Paralleling)
    '--------------------------------------------------------------------------------------------------------------------
    Public Function EditCommissionArrangement(ByVal v_lPartytypeId As Integer, ByVal v_lPartyId As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_lProductId As Integer, ByVal v_lTransactionTypeId As Integer, ByVal v_lCommissionBandId As Integer, ByVal v_lCommissionGroupId As Integer, ByVal v_cRate As Double, ByVal v_bIsValue As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_dtOldDate As Date, ByVal v_lTaxGroupID As Integer, Optional ByVal v_cMaximumRate As Decimal = 0, Optional ByVal v_lCommissionLevelID As Integer = 0, Optional ByVal v_sUniqueId As String = "") As Integer
        Dim result As Integer = 0
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Call the method from the business object
            'PSL 29/07/2003 Need old date to update using compound key
            'Start - Renuka - (WPR64 Paralleling)

            If v_cMaximumRate.Equals(0) Then

                m_lReturn = m_oBusiness.EditCommissionArrangement(v_lPartytypeId, v_lPartyId, v_lRiskTypeId, v_lProductId, v_lTransactionTypeId, v_lCommissionBandId, v_lCommissionGroupId, v_cRate, v_bIsValue, v_dtEffectiveDate, v_lTaxGroupID, v_dtOldDate, , v_lCommissionLevelID, v_sUniqueId)
            Else

                m_lReturn = m_oBusiness.EditCommissionArrangement(v_lPartytypeId, v_lPartyId, v_lRiskTypeId, v_lProductId, v_lTransactionTypeId, v_lCommissionBandId, v_lCommissionGroupId, v_cRate, v_bIsValue, v_dtEffectiveDate, v_lTaxGroupID, v_dtOldDate, v_cMaximumRate, v_lCommissionLevelID, v_sUniqueId)
            End If
            'End - Renuka - (WPR64 Paralleling)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'Return the result

            Return m_lReturn

        Catch excep As System.Exception

            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditCommissionArrangement   Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditCommissionArrangement  ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' PopulateCommissionListview : Populate the list view with the given data
    ''' </summary>
    ''' <param name="vntArray"></param>
    ''' <returns></returns>
    ''' <remarks>CMG / PB 23072002 New Commission Grouping functionality</remarks>
    Private Function PopulateCommissionListview(ByRef vntArray(,) As Object) As Integer
        Dim nResult As Integer = 0
        Dim sListItem() As String
        Dim oListItemArr() As ListViewItem
        Try

            Dim lvItem As ListViewItem

            'Set the return value
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Clear the listview
            lvwCommissionRate.Items.Clear()

            If Information.IsArray(vntArray) Then
                ReDim sListItem(22)
                ReDim oListItemArr(vntArray.GetUpperBound(1))

                ReDim sListItem(22)
                ReDim oListItemArr(vntArray.GetUpperBound(1))

                For iCount As Integer = vntArray.GetLowerBound(1) To vntArray.GetUpperBound(1)

                    sListItem(0) = CStr(vntArray(0, iCount)) 'Party Type
                    sListItem(1) = CStr(vntArray(1, iCount)) 'Party
                    sListItem(2) = CStr(vntArray(2, iCount)) 'Product
                    sListItem(3) = CStr(vntArray(3, iCount)) 'Risk Type
                    sListItem(4) = CStr(vntArray(4, iCount)) 'Transaction type
                    sListItem(5) = CStr(vntArray(5, iCount)) 'Commission band
                    sListItem(6) = CStr(vntArray(6, iCount)) 'Commission group

                    If CDbl(vntArray(8, iCount)) = 0 Then

                        m_oFormfields.Item(1).FieldType = gPMConstants.PMEDataType.PMDouble
                        m_oFormfields.Item(1).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
                        m_oFormfields.Item(1).DecimalPlaces = 10
                        m_oFormfields.Item(3).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatPercent

                    Else

                        m_oFormfields.Item(1).FieldType = gPMConstants.PMEDataType.PMCurrency
                        m_oFormfields.Item(1).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
                        m_oFormfields.Item(1).DecimalPlaces = 0
                        m_oFormfields.Item(3).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency

                    End If
                    'Set the Value in the text box
                    m_lReturn = m_oFormfields.FormatControl(ctlControl:=txtRate1, vControlValue:=vntArray(7, iCount))
                    m_lReturn = m_oFormfields.FormatControl(ctlControl:=txtMaximumRate1, vControlValue:=vntArray(20, iCount))

                    sListItem(7) = txtRate1.Text
                    sListItem(8) = CStr(vntArray(8, iCount)) 'Is Value

                    'Format the date before copying into the listview
                    m_lReturn = m_oFormfields.FormatControl(ctlControl:=txtDate, vControlValue:=vntArray(9, iCount))

                    sListItem(9) = ToSafeDate(txtDate.Text).ToShortDateString() 'Date
                    sListItem(10) = CStr(vntArray(10, iCount)) 'Party Type id
                    sListItem(11) = CStr(vntArray(11, iCount)) 'Party id
                    sListItem(12) = CStr(vntArray(12, iCount)) 'ProductId
                    sListItem(13) = CStr(vntArray(13, iCount)) 'Risk Type id
                    sListItem(14) = CStr(vntArray(14, iCount)) 'Transaction type id
                    sListItem(15) = CStr(vntArray(15, iCount)) 'Commission bandid
                    sListItem(16) = CStr(vntArray(16, iCount)) 'Commission groupid
                    sListItem(17) = CStr(vntArray(18, iCount)) 'Tax group id
                    sListItem(18) = CStr(vntArray(19, iCount)) 'Tax group
                    sListItem(19) = txtMaximumRate1.Text.Trim  'Maximum Rate

                    'Change the format of the Rate according to the value
                    'SAGICOR WPR14
                    sListItem(20) = CStr(vntArray(21, iCount))
                    sListItem(21) = CStr(vntArray(22, iCount))
                    'Change the format of the Rate according to the value
                    lvItem = New ListViewItem(sListItem)
                    lvItem.ForeColor = IIf((CDbl(vntArray(17, iCount)) = 1), Color.Gray, Color.Black)
                    lvItem.ImageIndex = 0
                    oListItemArr(iCount) = lvItem
                    If lvwCommissionRate.Items.Count = 1 Then
                        Dim currentSortOrder As SortOrder = lvwCommissionRate.Sorting
                        lvwCommissionRate.Sorting = If(currentSortOrder = SortOrder.Ascending, SortOrder.Descending, If(currentSortOrder = SortOrder.Descending, SortOrder.None, SortOrder.Ascending))
                        lvwCommissionRate.Sort()
                        lvwCommissionRate.FullRowSelect = True
                    End If
                Next
                lvwCommissionRate.Items.AddRange(oListItemArr)
            End If

            nResult = m_lReturn
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return nResult

        Catch excep As System.Exception
            'Error
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateCommissionListview Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateCommissionListview  ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return nResult

        End Try
    End Function

    Public Function SetComboItem(ByRef cboCombo As ComboBox, ByVal v_lItemData As Integer, Optional ByVal v_sItemName As String = "") As Integer

        'CMG/PB 12022002 Bug 1712 Default to all
        ' PW190203 - check if anything in combo list
        ' ISS2335 / ISS2293 / ISS2289
        If cboCombo.Items.Count > 0 Then
            cboCombo.SelectedIndex = 0
        End If
        'End CMG
        If cboCombo.Name = "cboParty" And Not (v_sItemName = "") Then
            cboCombo.Items.Add(v_sItemName)
            VB6.SetItemData(cboCombo, cboCombo.Items.Count - 1, v_lItemData)
        End If
        For nCount As Integer = 0 To cboCombo.Items.Count - 1

            If CInt(VB6.GetItemData(cboCombo, nCount)) = v_lItemData Then

                cboCombo.SelectedIndex = nCount

            End If
        Next

    End Function

    Private Sub lvwCommissionRate_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwCommissionRate.DoubleClick
        'If some item is selected in the listview
        If Not (lvwCommissionRate.FocusedItem Is Nothing) Then

            'If Not lvwCommissionRate.SelectedItem.Ghosted Then
            If Not lvwCommissionRate.FocusedItem.ForeColor.Equals(Color.Gray) Then
                'Call the edit method
                cmdEdit_Click(cmdEdit, New EventArgs())
            End If
        End If
    End Sub

    ' CMG / PB 23072002 New Commission Grouping functionality
    Private Function DeleteCommissionRate(ByVal v_nSelectedItem As Integer) As Integer
        Dim result As Integer = 0
        Try

            'Define local variables to store the values
            Dim lPartyTypeId, lPartyId, lProductId, lRiskTypeId, lTransactionTypeId, lCommissionBandId, lCommissionGroupId As Integer
            Dim lvItem As ListViewItem
            Dim dtEffDate As Date
            Dim lCommissionlevelID As Integer
            Dim m_suniqueId As String = ""
            Dim m_sscreenHeirarchy As String = ""

            'Set the Return value
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Get the selected item
            lvItem = lvwCommissionRate.Items.Item(v_nSelectedItem - 1)

            lPartyTypeId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACPartyTypeCol).Text)
            lPartyId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACPartyCol).Text)
            lProductId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACProductCol).Text)
            lRiskTypeId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACRiskTypeCol).Text)
            lTransactionTypeId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACTransactionTypeCol).Text)
            lCommissionBandId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACCommissionBandCol).Text)
            lCommissionGroupId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACCommissionGroupCol).Text)
            ' Alix - 21/01/2003 - PN9808
            ' Effective date is part of making a record unique
            dtEffDate = ToSafeDate(ListViewHelper.GetListViewSubItem(lvItem, ACEffectiveDateCol).Text)
            ' /Alix

            lCommissionlevelID = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACCommissionLevelID).Text)
            If lCommissionlevelID < 0 Then
                lCommissionlevelID = 0
            End If
            'Call the Business object method to delete the commission rate entry
            If m_suniqueId = "" Then
                m_suniqueId = GetUniqueID()
            End If
            m_sScreenHeirarchy = $"Commission Maintenance ({lvItem.SubItems(0).Text}, {lvItem.SubItems(1).Text}, {lvItem.SubItems(2).Text}, {lvItem.SubItems(3).Text}, {lvItem.SubItems(4).Text}, {lvItem.SubItems(5).Text}, {lvItem.SubItems(6).Text})"
            m_lReturn = m_oBusiness.DeleteCommissionArrangement(lPartyTypeId, lPartyId, lProductId, lRiskTypeId, lTransactionTypeId, lCommissionBandId, lCommissionGroupId, dtEffDate, lCommissionlevelID, m_suniqueId, m_sscreenHeirarchy)
            result = m_lReturn

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception

            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteCommissionRate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteCommissionRate ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        End Try
    End Function

    ' CMG / PB 23072002 New Commission Grouping functionality
    Private Function UnDeleteCommissionRate(ByVal v_nSelectedItem As Integer) As Integer
        Dim result As Integer = 0
        Try

            'Define local variables to store the values
            Dim lPartyTypeId, lPartyId, lProductId, lRiskTypeId, lTransactionTypeId, lCommissionBandId, lCommissionGroupId As Integer
            Dim lvItem As ListViewItem
            Dim dtEffDate As Date
            Dim lCommissionlevelID As Integer
            Dim m_suniqueId As String = ""
            Dim m_sscreenHeirarchy As String = ""
            'Set the Return value
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Get the selected item
            lvItem = lvwCommissionRate.Items.Item(v_nSelectedItem - 1)

            lPartyTypeId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACPartyTypeCol).Text)
            lPartyId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACPartyCol).Text)
            lProductId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACProductCol).Text)
            lRiskTypeId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACRiskTypeCol).Text)
            lTransactionTypeId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACTransactionTypeCol).Text)
            lCommissionBandId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACCommissionBandCol).Text)
            lCommissionGroupId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACCommissionGroupCol).Text)
            ' Alix - 21/01/2003 - PN9808
            ' Effective date is part of making a record unique
            dtEffDate = CDate(ListViewHelper.GetListViewSubItem(lvItem, ACEffectiveDateCol).Text)
            ' /Alix

            lCommissionlevelID = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACCommissionLevelID).Text)
            If lCommissionlevelID < 0 Then
                lCommissionlevelID = 0
            End If

            'Call the Business object method to delete the commission rate entry
            If m_suniqueId = "" Then
                m_suniqueId = GetUniqueID()
            End If
            m_sscreenHeirarchy = $"Commission Maintenance ({lvItem.SubItems(0).Text}, {lvItem.SubItems(1).Text}, {lvItem.SubItems(2).Text}, {lvItem.SubItems(3).Text}, {lvItem.SubItems(4).Text}, {lvItem.SubItems(5).Text}, {lvItem.SubItems(6).Text})"
            m_lReturn = m_oBusiness.UnDeleteCommissionArrangement(lPartyTypeId, lPartyId, lProductId, lRiskTypeId, lTransactionTypeId, lCommissionBandId, lCommissionGroupId, dtEffDate, lCommissionlevelID, m_suniqueId, m_sscreenHeirarchy)

            result = m_lReturn

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception

            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnDeleteCommissionRate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnDeleteCommissionRate ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        End Try
    End Function

    Private Sub lvwCommissionRate_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwCommissionRate.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwCommissionRate.GetItemAt(x, y) Is Nothing Then
                cmdDelete.Enabled = False
                cmdEdit.Enabled = False
            Else
                cmdDelete.Enabled = True
                cmdEdit.Enabled = True
            End If
        End If

    End Sub

    Private Sub lvwCommissionRate_ItemClick(ByVal Item As ListViewItem)

        'If Item.Ghosted Then
        If Item.ForeColor.Equals(Color.Gray) Then

            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUndeletebutton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEdit.Enabled = False

        Else

            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeletebutton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEdit.Enabled = True

        End If
    End Sub

    ' CMG / PB 23072002 New Commission Grouping functionality
    'Thinh Nguyen 01/07/2003 - add effective date
    Public Function IsDuplicateExists(ByVal v_lPartytypeId As Integer, ByVal v_lPartyId As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_lProductId As Integer, ByVal v_lTransactionTypeId As Integer, ByVal v_lCommissionBandId As Integer, ByVal v_lCommissionGroupId As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_lCommissionLevelID As Integer) As Integer

        Dim lPartyTypeId, lPartyId, lRiskTypeId, lProductId, lTransactionTypeId, lCommissionBandId, lCommissionGroupId As Integer
        Dim vntResult(,) As Object
        Dim lCommissionlevelID As Integer
        'Assume that Duplicate not exist
        Dim lReturn As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        'Get all the Commission Arrangements

        m_lReturn = m_oBusiness.GetAllCommissionArrangement(vResultArray:=vntResult)

        If Information.IsArray(vntResult) Then

            For nCount As Integer = vntResult.GetLowerBound(1) To vntResult.GetUpperBound(1)

                'Thinh Nguyen 01/07/2003 (start) - only check if record is not deleted and check for effective date as well

                ' The Check is commented as when we try to Add the commission with similar values
                ' as deleted commisions have then it will log Primary Key Constraint Error
                'If vntResult(ACIsDeletedCol, nCount) <> 1 Then
                'Get the values from the Listview

                lPartyTypeId = CInt(vntResult(ACPartyTypeCol, nCount))

                lPartyId = CInt(vntResult(ACPartyCol, nCount))

                lProductId = CInt(vntResult(ACProductCol, nCount))

                lRiskTypeId = CInt(vntResult(ACRiskTypeCol, nCount))

                lTransactionTypeId = CInt(vntResult(ACTransactionTypeCol, nCount))

                lCommissionBandId = CInt(vntResult(ACCommissionBandCol, nCount))

                lCommissionGroupId = CInt(vntResult(ACCommissionGroupCol, nCount))

                lCommissionlevelID = vntResult(ACCommissionLevelDESC, nCount)
                'Check for the duplicates

                If (lPartyTypeId = v_lPartytypeId) And (lPartyId = v_lPartyId) And (lRiskTypeId = v_lRiskTypeId) And (lProductId = v_lProductId) And (lTransactionTypeId = v_lTransactionTypeId) And (lCommissionBandId = v_lCommissionBandId) And (lCommissionGroupId = v_lCommissionGroupId) And (CDate(vntResult(ACEffectiveDateCol, nCount)) = v_dtEffectiveDate And (lCommissionlevelID = v_lCommissionLevelID)) Then

                    'Duplicate found.
                    lReturn = gPMConstants.PMEReturnCode.PMTrue

                    'Exit the process
                    Exit For

                End If
                'End If
                'Thinh Nguyen 01/07/2003 (end) - only check if record is not deleted and check for effective date as well

            Next

        End If

        'Return the result
        Return lReturn

    End Function

    '******************************************************************
    ' Name : PopulatePartyAgentType
    '
    ' Desc : populate party agent type
    '
    ' History
    ' CMG / PB 23072002 New Commission Grouping functionality
    '******************************************************************
    Public Function PopulatePartyAgentType(ByRef r_cboControl As ComboBox, Optional ByVal bShowAll As Boolean = True) As Integer

        Dim result As Integer = 0
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetLookupDetails("Party_Agent_Type", r_cboControl, bShowAll), gPMConstants.PMEReturnCode)

            'VB 17/03/2005 PN924 Removed.
            '    'add in reinsurer
            '    r_cboControl.AddItem "Reinsurer"
            '    r_cboControl.ItemData(r_cboControl.NewIndex) = 4

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulatePartyAgentType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulatePartyAgentType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        End Try
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If e.Alt And e.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
    End Sub

    Private Sub lvwCommissionRate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwCommissionRate.SelectedIndexChanged
        'If Item.Ghosted Then
        'If lvwCommissionRate.FocusedItem.ForeColor.Equals(Color.Gray) Then
        If lvwCommissionRate.SelectedItems.Count > 0 Then
            If lvwCommissionRate.SelectedItems.Item(0).ForeColor.Equals(Color.Gray) Then
                cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUndeletebutton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                cmdEdit.Enabled = False

            Else

                cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeletebutton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                cmdEdit.Enabled = True

            End If
        End If

        If lvwCommissionRate.SelectedItems.Count = 1 Then
            MainModule.frmDetail.Commissionlevel = CInt(ListViewHelper.GetListViewSubItem(lvwCommissionRate.SelectedItems.Item(0), ACCommissionLevelID).Text)
        End If

    End Sub
    Public Function SetCommissionLevel(ByRef cboCombo As System.Windows.Forms.ComboBox, ByVal v_lPartyId As Integer) As Integer

        Dim ncount As Short
        Dim i As Integer
        'Const nCol_Partytype As Short = 0
        Const nCol_PartyCnt As Short = 1
        'Const nCol_Shortname As Short = 2
        Const nCol_Commissionlevel As Short = 3
        Try
            If v_lPartyId > 0 Then

                'Browse thro the party details array and add the parties that belong to the given party type
                For ncount = LBound(m_vPartyDetails, 2) To UBound(m_vPartyDetails, 2)

                    If m_vPartyDetails(nCol_PartyCnt, ncount) = v_lPartyId Then
                        If ToSafeLong(m_vPartyDetails(nCol_Commissionlevel, ncount)) > 0 Then
                            For i = 0 To cboCombo.Items.Count - 1
                                If VB6.GetItemData(cboCombo, i) = m_vPartyDetails(nCol_Commissionlevel, ncount) Then
                                    cboCombo.SelectedIndex = i
                                End If
                            Next

                            Exit For
                        Else
                            cboCombo.SelectedIndex = 0
                        End If
                    End If

                Next

            End If

            'Return the result
            SetCommissionLevel = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set commission level", vApp:=ACApp, vClass:=ACClass, vMethod:="SetCommissionLevel", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
            SetCommissionLevel = gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' Function : PopulateCommissionlevelCombo
    ' Purpose : Populate the CommissionLevelcombo with the commissionlevels used to create the Commission
    ' Person : G.Kapoor
    ' Date   : 19th Aug 2010
    '----------------------------------------------------------------------
    Public Function PopulateCommissionlevelCombo(ByRef cboCombo As System.Windows.Forms.ComboBox, Optional ByRef vntArray As Object = Nothing, Optional ByVal bShowAll As Boolean = False, Optional ByVal v_lCommissionLevelID As Integer = 0) As Integer

        Dim iCount As Short
        Dim vArray(,) As Object

        Try
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Clear the Commissionlevel combo
            cboCombo.Items.Clear()

            'Add the "All" as default
            m_lReturn = AddAlltoCombo(cboCombo, bShowAll)

            m_lReturn = m_oBusiness.GetConfiguredCommissionLevel(vArray)
            If IsArray(vArray) Then

                'Browse thro the array and add the Commissionlevel to the given Commissionlevel Combo
                For iCount = LBound(vArray, 2) To UBound(vArray, 2)
                    cboCombo.Items.Add(New VB6.ListBoxItem(vArray(1, iCount), vArray(0, iCount)))
                Next
            End If

            'Set the first item as default
            cboCombo.SelectedIndex = 0

            'Return the result
            PopulateCommissionlevelCombo = m_lReturn

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch ex As System.Exception
            'Error
            PopulateCommissionlevelCombo = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateCommissionlevelCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateCommissionlevelCombo", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Function

    Private Sub AddHandlers()

        RemoveHandlers()
        AddHandler Me.cboParty.SelectedIndexChanged, AddressOf Me.cboParty_SelectedIndexChanged
        AddHandler Me.cboPartyType.SelectedIndexChanged, AddressOf Me.cboPartyType_SelectedIndexChanged
        AddHandler Me.cboProduct.SelectedIndexChanged, AddressOf Me.cboProduct_SelectedIndexChanged
        AddHandler Me.cboRiskType.SelectedIndexChanged, AddressOf Me.cboRiskType_SelectedIndexChanged
        AddHandler Me.cboTransactionType.SelectedIndexChanged, AddressOf Me.cboTransactionType_SelectedIndexChanged
        AddHandler Me.cboCommissionband.SelectedIndexChanged, AddressOf Me.cboCommissionband_SelectedIndexChanged
        AddHandler Me.cboCommissionGroup.SelectedIndexChanged, AddressOf Me.cboCommissionGroup_SelectedIndexChanged
        AddHandler Me.cboTaxGroup.SelectedIndexChanged, AddressOf Me.cboTaxGroup_SelectedIndexChanged
        AddHandler Me.cboCommissionLevel.SelectedIndexChanged, AddressOf Me.cboCommissionLevel_SelectedIndexChanged
        MainModule.frmDetail.AddHandlers()

    End Sub

    Private Sub RemoveHandlers()

        RemoveHandler Me.cboParty.SelectedIndexChanged, AddressOf Me.cboParty_SelectedIndexChanged
        RemoveHandler Me.cboPartyType.SelectedIndexChanged, AddressOf Me.cboPartyType_SelectedIndexChanged
        RemoveHandler Me.cboProduct.SelectedIndexChanged, AddressOf Me.cboProduct_SelectedIndexChanged
        RemoveHandler Me.cboRiskType.SelectedIndexChanged, AddressOf Me.cboRiskType_SelectedIndexChanged
        RemoveHandler Me.cboTransactionType.SelectedIndexChanged, AddressOf Me.cboTransactionType_SelectedIndexChanged
        RemoveHandler Me.cboCommissionband.SelectedIndexChanged, AddressOf Me.cboCommissionband_SelectedIndexChanged
        RemoveHandler Me.cboCommissionGroup.SelectedIndexChanged, AddressOf Me.cboCommissionGroup_SelectedIndexChanged
        RemoveHandler Me.cboTaxGroup.SelectedIndexChanged, AddressOf Me.cboTaxGroup_SelectedIndexChanged
        RemoveHandler Me.cboCommissionLevel.SelectedIndexChanged, AddressOf Me.cboCommissionLevel_SelectedIndexChanged
        MainModule.frmDetail.RemoveHandlers()

    End Sub

End Class
