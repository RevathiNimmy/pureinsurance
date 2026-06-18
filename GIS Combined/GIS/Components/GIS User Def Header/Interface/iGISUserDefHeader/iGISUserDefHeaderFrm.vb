Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 05/05/1999
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
    Private Const ACClass As String = "frmInterface"
    'developer guide no. 7
    Private Const vbFormCode As Integer = 0
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_sStepStatus As String = ""
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_lLookupHeaderId As Integer
	Private m_sCode As String = ""
	Private m_sDescription As String = ""
	Private m_lParentId As Integer
	Private m_sParentCode As String = ""
	Private m_vAllowedParents( ,  ) As Object
	Private m_vExistingHeaders( ,  ) As Object
	
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iGISUserDefHeader.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	'Private m_oBusiness As bSIRIPTExtras.Business
	
	' Declare an instance of the Details object.

	Private m_oLookupDetail As iGISUserDefDetails.Interface_Renamed
	'Private m_oLookupDetail As iGISUserDefDetails.Interface
	
	' Declare an instance of the Rates (or indicators) object.

	Private m_oLookupHeaderRates As iGISUserDefHeaderRates.Interface_Renamed
	'Private m_oLookupHeaderRates As iGISUserDefHeaderRates.Interface
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues As Object
	Private m_vLookupDetails As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String


    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)
    Public Property LookupHeaderId() As Integer
		Get
			Return m_lLookupHeaderId
		End Get
		Set(ByVal Value As Integer)
			m_lLookupHeaderId = Value
		End Set
	End Property
	
	Public Property Code() As String
		Get
			Return m_sCode
		End Get
		Set(ByVal Value As String)
			m_sCode = Value
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
	
	Public Property ParentId() As Integer
		Get
			Return m_lParentId
		End Get
		Set(ByVal Value As Integer)
			m_lParentId = Value
		End Set
	End Property
	
	Public Property ParentCode() As String
		Get
			Return m_sParentCode
		End Get
		Set(ByVal Value As String)
			m_sParentCode = Value
		End Set
	End Property
    'developer guide no. 101
    Public WriteOnly Property AllowedParents() As Object
        Set(ByVal Value As Object)
            m_vAllowedParents = Value
        End Set
    End Property
    'developer guide no. 101
    Public WriteOnly Property ExistingHeaders() As Object
        Set(ByVal Value As Object)
            m_vExistingHeaders = Value
        End Set
    End Property

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

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
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

    Public Property UniqueId() As String
        Get

            Return m_sUniqueId

        End Get
        Set(ByVal Value As String)

            m_sUniqueId = Value

        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get

            Return m_sScreenHierarchy

        End Get
        Set(ByVal Value As String)

            m_sScreenHierarchy = Value

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

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}


            Return gPMConstants.PMEReturnCode.PMTrue

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


            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            '    m_lReturn& = m_oBusiness.GetDetails(vExtras:=m_vExtras)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to get details.
            '        GetBusiness = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to get details from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetBusiness"
            '
            '        Exit Function
            '    End If

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
        Dim lIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCode, vControlValue:=m_sCode)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sDescription)

            cboParent.Items.Clear()

            Dim cboParent_NewIndex As Integer = -1
            cboParent_NewIndex = cboParent.Items.Add("None")
            lIndex = 0

            If Information.IsArray(m_vAllowedParents) Then
                For lTemp As Integer = m_vAllowedParents.GetLowerBound(1) To m_vAllowedParents.GetUpperBound(1)
                    cboParent_NewIndex = cboParent.Items.Add(CStr(m_vAllowedParents(1, lTemp)))
                    If CDbl(m_vAllowedParents(0, lTemp)) = m_lParentId Then
                        lIndex = cboParent_NewIndex
                    End If
                Next lTemp
            End If

            cboParent.SelectedIndex = lIndex

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try


            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim sTemp As String = ""

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
            '    m_DDate = CDate(txtDescription.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            m_sCode = CStr(m_oFormFields.UnformatControl(txtCode))

            m_sDescription = CStr(m_oFormFields.UnformatControl(txtDescription))

            If cboParent.SelectedIndex = 0 Then
                m_lParentId = -1
                m_sParentCode = ""
            Else
                m_lParentId = CInt(m_vAllowedParents(0, cboParent.SelectedIndex - 1))
                m_sParentCode = cboParent.Text
            End If

            ' {* USER DEFINED CODE (End) *}

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                txtCode.Enabled = False
            End If

            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lLookupHeaderId = -1) Then
                cmdIndicators.Enabled = False
                cmdRates.Enabled = False
                cmdDetails.Enabled = False
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

            m_ctlTabFirstLast(ACControlStart, 0) = txtCode
            m_ctlTabFirstLast(ACControlEnd, 0) = cboParent

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


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


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


            lblCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionRate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblParent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
    ' History: 07/09/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateForm(ByVal v_vExistingHeaders(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage As String


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetByCode(m_lLookupHeaderId, txtCode.Text.Trim())

            ' Also check a passed-in array of header values as database as not necessarily
            ' been refreshed with recent changes on lookup header screen (which requires
            ' Apply button to be pressed to refresh database).  So there may be code values
            ' contained in the list view array that the database is not yet aware of.

            ' PW160902 - Need to check if the passed array IS an array.
            ' Otherwise, if it is a null string we get a type mismatch error
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                If Information.IsArray(v_vExistingHeaders) Then
                    For iLoop As Integer = 0 To v_vExistingHeaders.GetUpperBound(1)
                        If m_lLookupHeaderId > 0 Or m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                            ' Update being performed so check header id as well to avoid finding
                            ' the existing record to be updated.


                            If txtCode.Text.Trim() = CStr(v_vExistingHeaders(ACHCode, iLoop)) And m_lLookupHeaderId <> CDbl(v_vExistingHeaders(ACHLookupHeaderId, iLoop)) Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                Exit For
                            End If
                        Else
                            ' New record being added

                            If txtCode.Text.Trim().ToUpper() = CStr(v_vExistingHeaders(ACHCode, iLoop)).ToUpper() Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                Exit For
                            End If
                        End If
                    Next iLoop
                End If
            End If

            If (result <> gPMConstants.PMEReturnCode.PMTrue) Or (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACValidateTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCodeAlreadyExists, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'Due to late introduction of code checking we may have some duplicate
                'codes in Edit when txtCode is disabled.
                If txtCode.Enabled Then
                    txtCode.Focus()
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CallRatesOrIndicators
    '
    ' Description:
    '
    ' History: 30/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CallRatesOrIndicators(ByRef sRorI As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create address component if not already done so
            If m_oLookupHeaderRates Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oLookupHeaderRates As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oLookupHeaderRates, sClassName:="iGISUserDefHeaderRates.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oLookupHeaderRates = temp_m_oLookupHeaderRates

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Lookup Header component", vApp:=ACApp, vClass:=ACClass, vMethod:="CallRatesOrIndicators", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If

            m_lReturn = CType(m_oLookupHeaderRates.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            m_oLookupHeaderRates.LookupHeaderId = m_lLookupHeaderId

            m_oLookupHeaderRates.Header = m_sDescription

            m_oLookupHeaderRates.RatesOrIndicators = sRorI

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            m_oLookupHeaderRates.UniqueId = m_sUniqueId

            m_oLookupHeaderRates.ScreenHierarchy = m_sScreenHierarchy


            m_lReturn = m_oLookupHeaderRates.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallRatesOrIndicators Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallRatesOrIndicators", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CallDetails
    '
    ' Description:
    '
    ' History: 30/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CallDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create address component if not already done so
            If m_oLookupDetail Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oLookupDetail As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oLookupDetail, sClassName:="iGISUserDefDetails.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oLookupDetail = temp_m_oLookupDetail

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Lookup Detail component", vApp:=ACApp, vClass:=ACClass, vMethod:="CallDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If

            m_lReturn = CType(m_oLookupDetail.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            m_oLookupDetail.LookupHeaderId = m_lLookupHeaderId

            m_oLookupDetail.Header = m_sDescription

            If cboParent.SelectedIndex < 1 Then

                m_oLookupDetail.LookupParentId = -1
            Else

                m_oLookupDetail.LookupParentId = m_vAllowedParents(0, cboParent.SelectedIndex - 1)
            End If


            If m_oLookupDetail.LookupParentId = 0 Then

                m_oLookupDetail.LookupParentId = -1
            End If

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            m_oLookupDetail.UniqueId = m_sUniqueId

            m_oLookupDetail.ScreenHierarchy = m_sScreenHierarchy

            m_lReturn = m_oLookupDetail.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function


    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub cmdDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetails.Click
        m_sScreenHierarchy = $"LookUp Header({txtCode.Text.ToString().Trim()})"
        m_lReturn = CType(CallDetails(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdIndicators_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdIndicators.Click
        m_sScreenHierarchy = $"LookUp Header({txtCode.Text.ToString().Trim()})"
        m_lReturn = CType(CallRatesOrIndicators(sRorI:="I"), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdRates_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRates.Click

        m_lReturn = CType(CallRatesOrIndicators(sRorI:="R"), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub Form_Initialize_Renamed()


        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            '    m_lReturn& = g_oObjectManager.GetInstance( _
            'oObject:=m_oBusiness, _
            'sClassName:="bSIRIPTExtras.Business", _
            'vInstanceManager:=PMGetViaClientManager)

            ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to get an instance of the business object.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Display error stating the problem.
            '
            '        ' Get description from the resource file.
            '        sTitle$ = iPMFunc.GetResData( _
            ''            iLangID:=g_iLanguageID%, _
            ''            lID:=ACBusinessFailTitle, _
            ''            iDataType:=PMResString)
            '
            '        sMessage$ = iPMFunc.GetResData( _
            ''            iLangID:=g_iLanguageID%, _
            ''            lID:=ACBusinessFail, _
            ''            iDataType:=PMResString)
            '
            '        ' Display message.
            '        MsgBox sMessage$, vbCritical, sTitle$
            '
            '        Exit Sub
            '    End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iGISUserDefHeader.General()

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

            ' Set the process modes for the busines object.
            '    m_lReturn& = m_oBusiness.SetProcessModes( _
            'vTask:=CVar(m_iTask%), _
            'vNavigate:=CVar(m_lNavigate&), _
            'vProcessMode:=CVar(m_lProcessMode&), _
            'vTransactionType:=CVar(m_sTransactionType$), _
            'vEffectiveDate:=CVar(m_dtEffectiveDate))

            ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to process the interface.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error Message
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to set the process modes for the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_Load"
            '
            '        Exit Sub
            '    End If

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
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

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
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'developer guide no. 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            If Not (m_oLookupHeaderRates Is Nothing) Then
                ' Terminate the LookupHeader object

                m_oLookupHeaderRates.Dispose()
                ' Destroy the instance of the LookupHeader object
                ' from memory.
                m_oLookupHeaderRates = Nothing
            End If

            If Not (m_oLookupDetail Is Nothing) Then
                ' Terminate the LookupHeader object

                m_oLookupDetail.Dispose()
                ' Destroy the instance of the LookupHeader object
                ' from memory.
                m_oLookupDetail = Nothing
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            '    ' Terminate the business object
            '    m_lReturn& = m_oBusiness.Terminate()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to terminate the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_QueryUnload"
            '    End If
            '
            '    ' Destroy the instance of the business object
            '    ' from memory.
            '    Set m_oBusiness = Nothing

            ' Terminate the form control object.
            m_oFormFields.Dispose()
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
                    Case Keys.Escape
                        cmdCancel.Focus()
                        cmdCancel.PerformClick()
                End Select
            End With
            'developer guide no.293

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        '    With tabMainTab
        '        ' Set the default button.
        '        If (.Tab < cmdNext.Count) Then
        '            cmdNext(.Tab).Default = True
        '        Else
        '            cmdOK.Default = True
        '        End If
        ''
        '        ' Now I know this is crap, this goes against
        '        ' all my principles, but for some reason when
        '        ' using the mouse to select a tab the setfocus
        '        ' code below doesn't work. The cursor sticks,
        '        ' and you can't tab off. Therefore I've used
        '        ' this to get around the problem.
        '        DoEvents
        ''
        '        ' Set focus to the first control on the tab.
        '        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
        '            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
        '        End If
        '    End With
        '
        'Catch 
        '
        '
        '
        '
        '
        'tabMainTabPreviousTab = tabMainTab.SelectedIndex
        'End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' validate data entered.
            m_lReturn = CType(ValidateForm(m_vExistingHeaders), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




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
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                RemoveHandler MyBase.FormClosing, AddressOf frmInterface_FormClosing
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)
    End Sub

    ' PRIVATE Events (End)
End Class
