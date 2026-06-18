Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Friend Partial Class frmTask
	Inherits System.Windows.Forms.Form
	' Edit History:
	' DAK200999 - Allow selection of task icon.
	' DAK061099 - Allow link to Client, Policy, Claim, etc. object.
	'             Also set whether task is view only or not.
	' DAK231199 - Check PrivilegeLevel to see what we can do
	' DAK011299 - More privilege levels
	' DAK211299 - Add Task Category
	' ***************************************************************** '
	
	'Declarations
	Private m_lTaskID As Integer
	Private m_lCaptionId As Integer
	Private m_sTaskCode As String = ""
	Private m_sDescription As String = ""
	Private m_iIsDeleted As gPMConstants.PMEReturnCode
	Private m_dtEffectiveDate As Date
	Private m_iIsSystemTask As CheckState
	Private m_iTypeOfTask As Integer
	Private m_lPMNavProcessId As Integer
	Private m_sComponentObjectName As String = ""
	Private m_sComponentClassName As String = ""
    Private m_lAutoDeleteAfterNumDays As Integer
    'Developer Guide no.50
    Dim frmInterface As frmInterface
	'DAK200999
	' DisplayIcon
	Private m_lDisplayIcon As Integer
	'DAK101999
	' IsViewOnlyTask
	Private m_iIsViewOnlyTask As Integer
	' LinkedObjectName
	Private m_sLinkedObjectName As String = ""
	' LinkedClassName
	Private m_sLinkedClassName As String = ""
	' LinkedCaption
	Private m_sLinkedCaption As String = ""
	' IsAvailableTask
	Private m_iIsAvailableTask As Integer
	'DAK211299
	' TaskCategoryID
	Private m_lTaskCategoryID As Integer
	
	Private m_oIcons As ImageList.ImageCollection
	
	Private m_lStatus As Integer
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_lFormMode As Integer
	
	' Declare an instance of the Form Control object.
	Private m_oFormFields As iPMFormControl.FormFields
	'DAK231199
	' PrivilegeLevel
	Private m_iPrivilegeLevel As Integer
	
	Private Const ACClass As String = "frmTask"
	
	
	Public Property TaskID() As Integer
		Get
			
			Return m_lTaskID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lTaskID = Value
			
		End Set
	End Property
	
	
	Public Property CaptionID() As Integer
		Get
			
			Return m_lCaptionId
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lCaptionId = Value
			
		End Set
	End Property
	
	
	Public Property TaskCode() As String
		Get
			
			Return m_sTaskCode
			
		End Get
		Set(ByVal Value As String)
			
			m_sTaskCode = Value
			
		End Set
	End Property
	
	
	Public Property Description() As String
		Get
			
			Return m_sDescription
			
		End Get
		Set(ByVal Value As String)
			
			m_sDescription = Value
			
		End Set
	End Property
	
	
	Public Property IsDeleted() As Integer
		Get
			
			Return m_iIsDeleted
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iIsDeleted = Value
			
		End Set
	End Property
	
	
	Public Property EffectiveDate() As Date
		Get
			
			Return m_dtEffectiveDate
			
		End Get
		Set(ByVal Value As Date)
			
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	
	Public Property IsSystemTask() As Integer
		Get
			
			Return m_iIsSystemTask
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iIsSystemTask = Value
			
		End Set
	End Property
	
	
	Public Property TypeOfTask() As Integer
		Get
			
			Return m_iTypeOfTask
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iTypeOfTask = Value
			
		End Set
	End Property
	
	
	Public Property PMNavProcessId() As Integer
		Get
			
			Return m_lPMNavProcessId
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lPMNavProcessId = Value
			
		End Set
	End Property
	
	
	Public Property ComponentObjectName() As String
		Get
			
			Return m_sComponentObjectName
			
		End Get
		Set(ByVal Value As String)
			
			m_sComponentObjectName = Value
			
		End Set
	End Property
	
	
	Public Property ComponentClassName() As String
		Get
			
			Return m_sComponentClassName
			
		End Get
		Set(ByVal Value As String)
			
			m_sComponentClassName = Value
			
		End Set
	End Property
	
	
	Public Property AutoDeleteAfterNumDays() As Integer
		Get
			
			Return m_lAutoDeleteAfterNumDays
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lAutoDeleteAfterNumDays = Value
			
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			
			Return m_lStatus
			
		End Get
	End Property
	
	
	Private Property FormMode() As Integer
		Get
			
			Return m_lFormMode
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lFormMode = Value
			
		End Set
	End Property
	
	'DAK200999
	Public Property DisplayIcon() As Integer
		Get
			Return m_lDisplayIcon
		End Get
		Set(ByVal Value As Integer)
			m_lDisplayIcon = Value
		End Set
	End Property
	
	'DAK061099
	Public Property IsViewOnlyTask() As Integer
		Get
			Return m_iIsViewOnlyTask
		End Get
		Set(ByVal Value As Integer)
			m_iIsViewOnlyTask = Value
		End Set
	End Property
	
	Public Property LinkedObjectName() As String
		Get
			Return m_sLinkedObjectName.Trim()
		End Get
		Set(ByVal Value As String)
			m_sLinkedObjectName = Value.Trim()
		End Set
	End Property
	
	Public Property LinkedClassName() As String
		Get
			Return m_sLinkedClassName.Trim()
		End Get
		Set(ByVal Value As String)
			m_sLinkedClassName = Value.Trim()
		End Set
	End Property
	
	Public Property LinkedCaption() As String
		Get
			Return m_sLinkedCaption.Trim()
		End Get
		Set(ByVal Value As String)
			m_sLinkedCaption = Value.Trim()
		End Set
	End Property
	
	Public Property IsAvailableTask() As Integer
		Get
			Return m_iIsAvailableTask
		End Get
		Set(ByVal Value As Integer)
			m_iIsAvailableTask = Value
		End Set
	End Property
	
	'DAK231199
	Public Property PrivilegeLevel() As Integer
		Get
			Return m_iPrivilegeLevel
		End Get
		Set(ByVal Value As Integer)
			m_iPrivilegeLevel = Value
		End Set
	End Property
	
	'DAK211299
	Public Property TaskCategoryID() As Integer
		Get
			Return m_lTaskCategoryID
		End Get
		Set(ByVal Value As Integer)
			m_lTaskCategoryID = Value
		End Set
	End Property
	
	'*************************************************************
	'
	' Function Name:ShowForm()
	'
	' Description: Shows form details which correspond with what
	'              the Task has selected from the previous form
	'*************************************************************
	
	Public Function ShowForm(ByRef lEditMode As Integer) As Integer
		
		Dim result As Integer = 0

		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			FormMode = lEditMode
			
			m_lReturn = PopCombo()
			
			m_lReturn = SetFieldValidation()
			
			txtTaskName.Text = m_sTaskCode.Trim()

            'Developer Guide no. 26
            lblEffectiveDate1.Text = StringsHelper.Format(m_dtEffectiveDate, "general date")
            txtDescription.Text = m_sDescription.Trim()
            chkIsSystemTask.CheckState = m_iIsSystemTask

            If m_lPMNavProcessId > 0 Then
                cboPMNavProcessId.ItemId = m_lPMNavProcessId
            Else
                cboPMNavProcessId.ListIndex = 0
            End If

            txtComponentObjectName.Text = m_sComponentObjectName
            txtComponentClassName.Text = m_sComponentClassName
            txtAutoDeleteAfterNumDays.Text = CStr(m_lAutoDeleteAfterNumDays)

            'DAK061099
            txtLinkedObjectName.Text = LinkedObjectName
            txtLinkedClassName.Text = LinkedClassName
            txtLinkedCaption.Text = LinkedCaption
            chkIsViewOnlyTask.CheckState = IsViewOnlyTask
            chkIsAvailableTask.CheckState = IsAvailableTask

            'DAK211299
            cboTaskCategory.ItemId = m_lTaskCategoryID
            cboTaskCategory.Refresh()

            'DAK231199
            If PrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions Then
                chkIsSystemTask.Enabled = False
                cboTypeOfTask.Enabled = False
                cboPMNavProcessId.Enabled = False
                txtComponentObjectName.Enabled = False
                txtComponentClassName.Enabled = False
                txtAutoDeleteAfterNumDays.Enabled = False
                txtLinkedObjectName.Enabled = False
                txtLinkedClassName.Enabled = False
                txtLinkedCaption.Enabled = False
                chkIsViewOnlyTask.Enabled = False
                chkIsAvailableTask.Enabled = False
                'DAK211299
                cboTaskCategory.Enabled = False
                'DAK011299
            ElseIf PrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly Then
                txtDescription.Enabled = False
                chkIsSystemTask.Enabled = False
                cboTypeOfTask.Enabled = False
                cboPMNavProcessId.Enabled = False
                txtComponentObjectName.Enabled = False
                txtComponentClassName.Enabled = False
                txtAutoDeleteAfterNumDays.Enabled = False
                txtLinkedObjectName.Enabled = False
                txtLinkedClassName.Enabled = False
                txtLinkedCaption.Enabled = False
                chkIsViewOnlyTask.Enabled = False
                chkIsAvailableTask.Enabled = False
                'DAK211299
                cboTaskCategory.Enabled = False
            Else
                chkIsSystemTask.Enabled = True
                cboTypeOfTask.Enabled = True
                cboPMNavProcessId.Enabled = True
                txtComponentObjectName.Enabled = True
                txtComponentClassName.Enabled = True
                txtAutoDeleteAfterNumDays.Enabled = True
                txtLinkedObjectName.Enabled = True
                txtLinkedClassName.Enabled = True
                txtLinkedCaption.Enabled = True
                chkIsViewOnlyTask.Enabled = True
                chkIsAvailableTask.Enabled = True
                'DAK211299
                cboTaskCategory.Enabled = True
            End If


            Select Case lEditMode
                Case USRAddTask
                    'Give some names
                    Me.Text = "Add Task"

                    pnlEffectiveDate.BackColor = SystemColors.Control

                Case USREditTask
                    'Give some names
                    Me.Text = "Edit Task"

                    'Set some defaults
                    txtTaskName.BackColor = SystemColors.Control
                    pnlEffectiveDate.BackColor = SystemColors.Control

                    txtTaskName.Enabled = False
                    pnlEffectiveDate.Enabled = False

                Case Else

            End Select

            cboTypeOfTask.SelectedIndex = m_iTypeOfTask

            'DAK200999 - display task icon
            m_oIcons = frmInterface.imgTask.Images
            If DisplayIcon < 1 Or DisplayIcon > m_oIcons.Count Then
                DisplayIcon = 1
            End If

            imgIcon.Image = m_oIcons.Item(DisplayIcon - 1)

            'Show the form
            Me.ShowDialog()

            Return result

        Catch excep As System.Exception



            'Error Section

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show Task Form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function PopCombo() As Integer

        Dim result As Integer = 0


        Select Case Information.Err().Number
            Case Is < 0
                Conversion.ErrorToString(5)
            Case 1
                GoTo err_PopCombo
        End Select

        result = gPMConstants.PMEReturnCode.PMTrue

        cboTypeOfTask.Items.Clear()

        cboTypeOfTask.Items.Add("Memo")
        cboTypeOfTask.Items.Add("Single Component")
        cboTypeOfTask.Items.Add("Navigator Process")

        Return result

err_PopCombo:

        'Error Section

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate the Type Of Task list box", vApp:=ACApp, vClass:=ACClass, vMethod:="PopCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oFormFields = New iPMFormControl.FormFields()

            m_oFormFields.LanguageID = g_iLanguageID

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTaskName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtComponentObjectName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtComponentClassName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAutoDeleteAfterNumDays, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK061099
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLinkedObjectName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLinkedClassName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLinkedCaption, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' Name: ValidateForm
    '
    ' Description: Validates the fields.
    '
    ' ***************************************************************** '
    Private Function ValidateForm(ByRef ctlTemp As Control) As Integer

        Dim result As Integer = 0
        Dim iRetval As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oNavV3Component As aPMNav.NavigatorV3
        Dim oObject As Object

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_sTaskCode = txtTaskName.Text.Trim()

            ' Check to see if a Task name has been entered
            If m_sTaskCode = "" Then

                MessageBox.Show("Task name not entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                ctlTemp = txtTaskName
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Check to see if a Type Of Task has been entered
            If cboTypeOfTask.SelectedIndex = -1 Then

                MessageBox.Show("Type Of Task not entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                ctlTemp = cboTypeOfTask
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Validate the screen input
            Select Case cboTypeOfTask.SelectedIndex
                Case 0
                    'Can't do anything, nothing to do
                Case 1
                    'Must enter a object and class

                    'DAK061099 - Error messages here clarified as "Component" error to differentiate
                    '            from "Linked" error
                    m_sComponentObjectName = txtComponentObjectName.Text.Trim()
                    m_sComponentClassName = txtComponentClassName.Text.Trim()

                    If m_sComponentObjectName = "" Then
                        MessageBox.Show("Component Object Name not entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        ctlTemp = txtComponentObjectName
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_sComponentClassName = "" Then
                        MessageBox.Show("Component Class Name not entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        ctlTemp = txtComponentClassName
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_sComponentObjectName = "iPMTaskMaintenance" Then
                        MessageBox.Show("Task Maintenance is not a valid object", Application.ProductName)
                        ctlTemp = txtComponentObjectName
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = g_oObjectManager.GetInstance(oObject:=oObject, sClassName:=m_sComponentObjectName & "." & m_sComponentClassName, vInstanceManager:=gPMConstants.PMGetLocalInterface)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("The component object does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        ctlTemp = txtComponentObjectName
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If oObject Is Nothing Then
                        MessageBox.Show("The component object does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        ctlTemp = txtComponentObjectName
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Check to see if this component is Navigator Version 3

                    Try
                        oNavV3Component = oObject
                        If Information.Err().Number = 0 Then
                        Else
                            Information.Err().Clear()
                        End If
                    Catch ex As Exception

                    End Try

                    oObject.Dispose()

                    oObject = Nothing
                    If oNavV3Component Is Nothing Then
                        MessageBox.Show("The component object is not Navigator Version 3 compliant.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        ctlTemp = txtComponentObjectName
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    oNavV3Component = Nothing

                Case 2
                    'Must enter a valid navigator process id
                    If cboPMNavProcessId.ListIndex = 0 Then
                        MessageBox.Show("Must enter navigator process id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        ctlTemp = cboPMNavProcessId
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

            End Select

            'DAK061099 - Validate Linked Object
            LinkedObjectName = txtLinkedObjectName.Text
            LinkedClassName = txtLinkedClassName.Text
            LinkedCaption = txtLinkedCaption.Text

            If LinkedObjectName <> "" Or LinkedClassName <> "" Or LinkedCaption <> "" Then

                If LinkedObjectName = "" Then
                    MessageBox.Show("LinkedObject Name not enetered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ctlTemp = txtLinkedObjectName
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If LinkedClassName = "" Then
                    MessageBox.Show("Linked Class Name not entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ctlTemp = txtLinkedClassName
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If LinkedCaption = "" Then
                    MessageBox.Show("Linked Caption not entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ctlTemp = txtLinkedCaption
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If LinkedObjectName = "iPMTaskMaintenance" Then
                    MessageBox.Show("Task Maintenance is not a valid object", Application.ProductName)
                    ctlTemp = txtLinkedObjectName
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = g_oObjectManager.GetInstance(oObject:=oObject, sClassName:=LinkedObjectName & "." & LinkedClassName, vInstanceManager:=gPMConstants.PMGetLocalInterface)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("The linked object does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ctlTemp = txtLinkedObjectName
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If oObject Is Nothing Then
                    MessageBox.Show("The linked object does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ctlTemp = txtLinkedObjectName
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Check to see if this component is Navigator Version 3

                Try
                    oNavV3Component = oObject
                    If Information.Err().Number = 0 Then
                    Else
                        Information.Err().Clear()
                    End If
                Catch ex As Exception

                End Try


                oObject.Dispose()

                oObject = Nothing

                If oNavV3Component Is Nothing Then
                    MessageBox.Show("The linked object is not Navigator Version 3 compliant.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ctlTemp = txtLinkedObjectName
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oNavV3Component = Nothing

            End If

            m_sDescription = txtDescription.Text.Trim()

            'Developer Guide no. 26
            m_dtEffectiveDate = CDate(lblEffectiveDate1.Text)

            m_iIsSystemTask = chkIsSystemTask.CheckState
            m_iTypeOfTask = cboTypeOfTask.SelectedIndex

            If cboPMNavProcessId.ListIndex = 0 Then
                m_lPMNavProcessId = 0
            Else
                m_lPMNavProcessId = cboPMNavProcessId.ItemId
            End If

            m_sComponentObjectName = txtComponentObjectName.Text.Trim()
            m_sComponentClassName = txtComponentClassName.Text.Trim()

            If txtAutoDeleteAfterNumDays.Text.Trim() = "" Then
                m_lAutoDeleteAfterNumDays = -1
            Else
                m_lAutoDeleteAfterNumDays = CInt(txtAutoDeleteAfterNumDays.Text)
            End If

            'DAK061099
            IsViewOnlyTask = chkIsViewOnlyTask.CheckState
            LinkedObjectName = txtLinkedObjectName.Text
            LinkedClassName = txtLinkedClassName.Text
            LinkedCaption = txtLinkedCaption.Text
            IsAvailableTask = chkIsAvailableTask.CheckState
            'DAK211299
            TaskCategoryID = cboTaskCategory.ItemId

            If FormMode = USRAddTask Then
                ' Check for Duplicate Tasknames
                lReturn = CType(frmInterface.CheckTaskname(m_sTaskCode), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Task name " & m_sTaskCode & _
                                    " already exists." & Strings.Chr(13) & Strings.Chr(10) & "Please Choose another Task name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtTaskName.Text = ""
                    ctlTemp = txtTaskName
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ValidateForm", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End Try

        Return result
    End Function
	
	'DAK211299
	Private Sub cboTaskCategory_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaskCategory.Click
		
		TaskCategoryID = cboTaskCategory.ItemId
		
	End Sub
	
	Private Sub cboTypeOfTask_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTypeOfTask.SelectionChangeCommitted
		
		'If Navigator Process..
		
		Select Case cboTypeOfTask.SelectedIndex
			Case 0
				lblComponentObjectName.Visible = False
				txtComponentObjectName.Visible = False
				lblComponentClassName.Visible = False
				txtComponentClassName.Visible = False
				lblPMNavProcessId.Visible = False
				cboPMNavProcessId.Visible = False
				
				
				lblPMNavProcessId.Font = VB6.FontChangeBold(lblPMNavProcessId.Font, False)
				cboPMNavProcessId.ListIndex = 0
				'        cboPMNavProcessId.Enabled = False
				
				lblComponentObjectName.Font = VB6.FontChangeBold(lblComponentObjectName.Font, False)
				txtComponentObjectName.Enabled = False
				txtComponentObjectName.Text = ""
				lblComponentClassName.Font = VB6.FontChangeBold(lblComponentClassName.Font, False)
				txtComponentClassName.Enabled = False
				txtComponentClassName.Text = ""
			Case 1
				lblComponentObjectName.Visible = True
				txtComponentObjectName.Visible = True
				lblComponentClassName.Visible = True
				txtComponentClassName.Visible = True
				lblPMNavProcessId.Visible = False
				cboPMNavProcessId.Visible = False
				
				
				lblPMNavProcessId.Font = VB6.FontChangeBold(lblPMNavProcessId.Font, False)
				cboPMNavProcessId.ListIndex = 0
				'        cboPMNavProcessId.Enabled = False
				cboPMNavProcessId.Visible = False
				lblComponentObjectName.Font = VB6.FontChangeBold(lblComponentObjectName.Font, True)
				txtComponentObjectName.Enabled = True
				'        txtComponentObjectName.SetFocus
				lblComponentClassName.Font = VB6.FontChangeBold(lblComponentClassName.Font, True)
				txtComponentClassName.Enabled = True
			Case 2
				lblComponentObjectName.Visible = False
				txtComponentObjectName.Visible = False
				txtComponentObjectName.Text = ""
				lblComponentClassName.Visible = False
				txtComponentClassName.Visible = False
				txtComponentClassName.Text = ""
				lblPMNavProcessId.Visible = True
				cboPMNavProcessId.Visible = True
				
				lblPMNavProcessId.Font = VB6.FontChangeBold(lblPMNavProcessId.Font, True)
				'        cboPMNavProcessId.Enabled = True
				'        cboPMNavProcessId.SetFocus
				lblComponentObjectName.Font = VB6.FontChangeBold(lblComponentObjectName.Font, False)
				txtComponentObjectName.Enabled = False
				txtComponentObjectName.Text = ""
				lblComponentClassName.Font = VB6.FontChangeBold(lblComponentClassName.Font, False)
				txtComponentClassName.Enabled = False
				txtComponentClassName.Text = ""
		End Select
		
	End Sub
	
	Private Sub chkIsAvailableTask_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsAvailableTask.CheckStateChanged
		
		'Check to see if this check box is checked, if it is
		'then display yes else display no
		
		If chkIsAvailableTask.CheckState = CheckState.Checked Then
			lblIsAvailableTask.Text = "Yes"
		Else
			lblIsAvailableTask.Text = "No"
		End If
		
	End Sub
	
	Private Sub chkIsSystemTask_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsSystemTask.CheckStateChanged
		
		'Check to see if this check box is checked, if it is
		'then display yes else display no
		
		If chkIsSystemTask.CheckState = CheckState.Checked Then
			lblIsSystemTask.Text = "Yes"
			cboTypeOfTask.SelectedIndex = 1
			cboTypeOfTask.Enabled = False
		Else
			lblIsSystemTask.Text = "No"
			If cboTypeOfTask.SelectedIndex = 1 Then
				cboTypeOfTask.Enabled = True
			End If
		End If
		
	End Sub
	
	Private Sub chkIsViewOnlyTask_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsViewOnlyTask.CheckStateChanged
		
		'Check to see if this check box is checked, if it is
		'then display yes else display no
		
		If chkIsViewOnlyTask.CheckState = CheckState.Checked Then
			lblIsViewOnlyTask.Text = "Yes"
		Else
			lblIsViewOnlyTask.Text = "No"
		End If
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()
		
	End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (cmdHelp_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub cmdHelp_Click()
		'
		'MessageBox.Show("There is no help associated with this screen", "Task Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
		'
	'End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Dim ctlTemp As Control
		
		Dim lReturn As gPMConstants.PMEReturnCode = CType(ValidateForm(ctlTemp:=ctlTemp), gPMConstants.PMEReturnCode)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			ctlTemp.Focus()
			Exit Sub
		End If
		
		'Set status to PmOK
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		'Enable the apply button
		frmInterface.cmdApply.Enabled = True
		'hide this form
		Me.Hide()
		
	End Sub
	
	Private Sub cmdSelectIcon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectIcon.Click
		
		Dim fIconLookup As frmIconLookup
		
		
		Try 
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			fIconLookup = New frmIconLookup()
			fIconLookup.DisplayIcon = m_lDisplayIcon

			fIconLookup.ShowDialog()
			
			If DisplayIcon <> fIconLookup.DisplayIcon Then
				DisplayIcon = fIconLookup.DisplayIcon
				imgIcon.Image = m_oIcons.Item(DisplayIcon - 1)
			End If
		
		Catch 
			
			
			
			m_lReturn = gPMConstants.PMEReturnCode.PMError
		End Try
		
		
	End Sub
	
	Private Sub frmTask_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			With uctPMResizer1
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("tabTask", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
			End With
			
		End If
	End Sub
	

	Private Sub frmTask_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		With uctPMResizer1
			.NoResizeByDefault = True
			'DAK211299
			.FormMinHeight = 8700
			.FormMinWidth = 9405
		End With
		
	End Sub
	
	Private Sub txtAutoDeleteAfterNumDays_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAutoDeleteAfterNumDays.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtAutoDeleteAfterNumDays)
		
	End Sub
	
	Private Sub txtAutoDeleteAfterNumDays_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAutoDeleteAfterNumDays.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtAutoDeleteAfterNumDays)
		
	End Sub
	
	Private Sub txtComponentClassName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComponentClassName.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtComponentClassName)
		
	End Sub
	
	Private Sub txtComponentClassName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComponentClassName.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtComponentClassName)
		
	End Sub
	
	Private Sub txtComponentObjectName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComponentObjectName.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtComponentObjectName)
		
	End Sub
	
	Private Sub txtComponentObjectName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComponentObjectName.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtComponentObjectName)
		
	End Sub
	
	Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtDescription)
		
	End Sub
	
	Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtDescription)
		
	End Sub
	
	Private Sub txtLinkedCaption_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLinkedCaption.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtLinkedCaption)
		
	End Sub
	
	Private Sub txtLinkedCaption_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLinkedCaption.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtLinkedCaption)
		
	End Sub
	
	Private Sub txtLinkedClassName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLinkedClassName.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtLinkedClassName)
		
	End Sub
	
	Private Sub txtLinkedClassName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLinkedClassName.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtLinkedClassName)
		
	End Sub
	
	Private Sub txtLinkedObjectName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLinkedObjectName.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtLinkedObjectName)
		
	End Sub
	
	Private Sub txtLinkedObjectName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLinkedObjectName.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtLinkedObjectName)
		
	End Sub
	
	Private Sub txtTaskName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaskName.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtTaskName)
		
	End Sub
	
	Private Sub txtTaskName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaskName.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtTaskName)
		
	End Sub

    
    Private Sub frmTask_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabTask.SelectedIndex = 0
        End If
    End Sub
End Class
