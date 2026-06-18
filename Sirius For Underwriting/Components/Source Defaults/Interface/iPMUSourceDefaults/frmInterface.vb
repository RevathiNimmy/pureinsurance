Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No.129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    '
    ' PW301002 - Component created
    ' Based on iPMURenewalPreList
    '
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""
    Private m_SourceID As Integer

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUSourceDefaults.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lSourceID As Integer
    Private m_iBusinessType As Integer
    'Developer Guide No.101
    '    Private m_vAgentID As Integer
    Private m_vAgentID As Object
    Private m_lcboSourceIndex As Integer
    Private m_bSystemChange As Boolean
    Private m_bChanged As Boolean

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
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

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            m_lNavigate = Value

        End Set
    End Property

    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

        End Set
    End Property
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property



    Public Property SourceID() As Integer
        Get

            Return m_SourceID

        End Get
        Set(ByVal Value As Integer)

            m_SourceID = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            result = m_oFormFields.AddNewFormField(ctlControl:=cboSource, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oFormFields.AddNewFormField(ctlControl:=cboBusinessType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return m_oFormFields.AddNewFormField(ctlControl:=cboAgent, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function BusinessToInterface() As Integer


        Dim result As Integer = 0
        Return gPMConstants.PMEReturnCode.PMTrue



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim iDirectBusiness As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details from the interface to the data storage.
            result = InterfaceToData()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            '
            ' Save the defaults away to the Source Defaults table
            '
            iDirectBusiness = Math.Abs(CInt(m_iBusinessType = ACDirectBusinessValue))


            result = m_oBusiness.SaveSourceDefaults(v_lSourceID:=m_lSourceID, v_iDirectBusiness:=iDirectBusiness, v_vAgentId:=m_vAgentID)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to save the Branch Defaults", ACApp, MessageBoxButtons.OK)
            Else
                MessageBox.Show("Branch Defaults saved successfully", ACApp, MessageBoxButtons.OK)
            End If

            Return result

        Catch excep As System.Exception



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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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

            m_lSourceID = VB6.GetItemData(cboSource, cboSource.SelectedIndex)
            m_iBusinessType = VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex)
            If cboAgent.SelectedIndex < 0 Then
                cboAgent.SelectedIndex = 0
            End If
            If VB6.GetItemData(cboAgent, cboAgent.SelectedIndex) <= 0 Then

                m_vAgentID = Nothing
            Else
                m_vAgentID = VB6.GetItemData(cboAgent, cboAgent.SelectedIndex)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            'default to first tab
            SSTabHelper.SetSelectedIndex(Me.tabMainTab, 0)

            ' Display all language specific captions.
            m_lReturn = iPMForms.DisplayCaptions(Me, My.Resources.ResourceManager)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' Set up the source combo
            result = GetComboDetails(cboSource)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Set up the agent combo
            '    SetInterfaceDefaults = GetComboDetails(cboAgent)
            '    If SetInterfaceDefaults <> PMTrue Then Exit Function

            ' Set up the business type combo
            Dim cboBusinessType_NewIndex As Integer = -1

            cboBusinessType_NewIndex = cboBusinessType.Items.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDirectBusiness, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))
            VB6.SetItemData(cboBusinessType, cboBusinessType_NewIndex, ACDirectBusinessValue)

            cboBusinessType_NewIndex = cboBusinessType.Items.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgencyBusiness, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))
            VB6.SetItemData(cboBusinessType, cboBusinessType_NewIndex, ACAgencyBusinessValue)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 0)

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

            m_ctlTabFirstLast(ACControlStart, 0) = cboSource
            m_ctlTabFirstLast(ACControlEnd, 0) = cboAgent

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




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


            cmdExit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExitButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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


            lblSource.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSource, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBusinessType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAgent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateForm
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ValidateForm() As Integer

        Dim result As Integer = 0
        Try


            '    If cboUserGroup.ItemData(cboUserGroup.ListIndex) = -1 Then
            '        MsgBox "Please choose a User Group from the list.", vbOKOnly + vbExclamation, ACApp
            '        ValidateForm = PMFalse
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetComboDetails
    '
    ' Description: get details from user groups and add to combobox
    '
    '
    ' ***************************************************************** '
    Private Function GetComboDetails(ByRef r_cboControl As ComboBox) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim sType As String = ""
        Dim iSource_id As Integer

        result = gPMConstants.PMEReturnCode.PMFalse

        Try

            ' Get the combo type
            sType = r_cboControl.Name.Trim()
            sType = Mid(sType, 4)

            'make sure combobox is empty
            r_cboControl.Items.Clear()

            'add in non applicable value with ID of 0
            If sType = "Agent" Then
                Dim r_cboControl_NewIndex As Integer = -1
                r_cboControl_NewIndex = r_cboControl.Items.Add("")
                VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, 0)
                iSource_id = VB6.GetItemData(cboSource, cboSource.SelectedIndex) ''Agent Filtering
            Else
                iSource_id = 0
            End If


            If m_oBusiness.GetLookUp(v_sTable:=sType, r_vResultArray:=vResultArray, v_lSourceID:=iSource_id) = gPMConstants.PMEReturnCode.PMTrue Then ''Agent Filtering

                If Information.IsArray(vResultArray) Then
                    For iCount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                        'Developer Guide No.153 (Latest guide)
                        'r_cboControl_NewIndex = r_cboControl.Items.Add(vResultArray(1, iCount))

                        'VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, CInt(vResultArray(0, iCount)))
                        r_cboControl.Items.Add(New VB6.ListBoxItem((vResultArray(1, iCount)), CInt(vResultArray(0, iCount))))
                    Next
                End If

                result = gPMConstants.PMEReturnCode.PMTrue

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetComboDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function





    Private Sub cboAgent_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAgent.SelectedIndexChanged

        m_bChanged = True

    End Sub


    Private Sub cboBusinessType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBusinessType.SelectedIndexChanged
        '
        ' Enable or Disable the agent combo depending on the business type
        '
        Try

            If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = ACDirectBusinessValue Then
                cboAgent.SelectedIndex = 0
                cboAgent.Enabled = False
            Else
                cboAgent.Enabled = True
            End If

            m_bChanged = True

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Business Type combo click", vApp:=ACApp, vClass:=ACClass, vMethod:="cboBusinessType_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub


    Private Sub cboSource_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSource.SelectedIndexChanged
        '
        ' Get the details for the selected branch
        '
        Dim iBusinessType As Integer
        Dim lAgentID As Integer
        Dim vResultArray As String = "" ''Agent Filtering

        Try

            ' Don't do this if it's a system change
            If m_bSystemChange Then
                Exit Sub
            End If

            ' Check if anything has changed, but not saved
            If m_bChanged Then
                If MessageBox.Show("Changes have been made to the branch. Do you wish to discard the changes ?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                    m_bSystemChange = True
                    cboSource.SelectedIndex = m_lcboSourceIndex
                    m_bSystemChange = False
                    Exit Sub
                End If
            End If

            ''Agent Filtering
            'Populate Branch Specific Agents

            vResultArray = ""
            m_lReturn = CType(GetComboDetails(cboAgent), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to retrieve the Branch Agents for " & VB6.GetItemString(cboSource, cboSource.SelectedIndex).Trim(), ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                cboAgent.SelectedIndex = 0
                Exit Sub
            End If

            ''Agent Filtering


            ' Get the details from the business object

            m_lReturn = m_oBusiness.GetSourceDefaults(v_lSourceID:=VB6.GetItemData(cboSource, cboSource.SelectedIndex), r_iDirectBusiness:=iBusinessType, r_lAgentId:=lAgentID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to retrieve the Branch Defaults for " & VB6.GetItemString(cboSource, cboSource.SelectedIndex).Trim(), ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                cboBusinessType.SelectedIndex = 0
                Exit Sub
            End If

            ' Setup the business type
            If iBusinessType = 1 Then
                iBusinessType = ACDirectBusinessValue
            Else
                iBusinessType = ACAgencyBusinessValue
            End If

            ' Display the default business type for this branch
            If iBusinessType = 0 Then
                cboBusinessType.SelectedIndex = 0
            Else
                For i As Integer = 0 To cboBusinessType.Items.Count - 1
                    If VB6.GetItemData(cboBusinessType, i) = iBusinessType Then
                        cboBusinessType.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            ' Display the default agent for this branch, if applicable
            If iBusinessType = ACAgencyBusinessValue Then
                For i As Integer = 0 To cboAgent.Items.Count - 1
                    If VB6.GetItemData(cboAgent, i) = lAgentID Then
                        cboAgent.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            ' Store the current source id
            m_lcboSourceIndex = cboSource.SelectedIndex

            m_bChanged = False

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Branch combo click", vApp:=ACApp, vClass:=ACClass, vMethod:="cboSource_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub


    ' PRIVATE Methods (End)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            'g_oObjectManager = New bObjectManager.ObjectManager ' TEST
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMUSourceDefaults.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

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

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUSourceDefaults.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_bSystemChange = True
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            m_bSystemChange = False

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Initialise the combos
            cboSource.SelectedIndex = 0
            cboSource.Focus()
            cboSource.Select()
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




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

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Check for errors.

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object


            ' Terminate the form control object.
            m_oFormFields.Dispose()

            ' Check for errors.

            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With

        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check mandatory controls have been entered into.
            If m_oFormFields.CheckMandatoryControls() = gPMConstants.PMEReturnCode.PMTrue Then
                If ValidateForm() = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Process the next set of actions depending
                    ' upon the interface task etc.
                    m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
                End If
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            m_bChanged = False

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click

        ' Click event of the Cancel button.

        Try

            ' Check if anything has changed, but not saved
            If m_bChanged Then
                If MessageBox.Show("Changes have been made to the branch. Do you wish to discard the changes ?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            Me.Hide()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
End Class
