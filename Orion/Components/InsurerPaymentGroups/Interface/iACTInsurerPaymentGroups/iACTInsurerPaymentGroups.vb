Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
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
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
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
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_lPartyCnt As Integer
	Private m_lAccountID As Integer
	Private m_vPaymentGroups As Object
	' {* USER DEFINED CODE (End) *}
    'Developer guide No.7
    Private Const vbFormCode As Integer = 0
	' Declare an instance of the general interface object.
	Private m_oGeneral As iACTInsurerPaymentGroups.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object

	' Declare an instance of the edit interface.
    'developer guide no.108
    Private m_oPaymentGroup As Object

	
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
	Private m_ctlTabFirstLast( ,  ) As Control
	
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	Public Property PartyCnt() As Integer
		Get
			Return m_lPartyCnt
		End Get
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}

			m_lReturn = m_oBusiness.GetAccountId(lPartyCnt:=m_lPartyCnt, lAccountID:=m_lAccountID)
			

			m_lReturn = m_oBusiness.GetDetails(lAccountID:=m_lAccountID, vPaymentGroups:=m_vPaymentGroups)
			
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
		Dim oListItem As ListViewItem
		
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
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			lvwPaymentGroups.Items.Clear()
			
			If Not Information.IsArray(m_vPaymentGroups) Then
				Return result
			End If
			
			' Assign the details to the interface.

			For iTemp As Integer = m_vPaymentGroups.GetLowerBound(1) To m_vPaymentGroups.GetUpperBound(1)
				
				' {* USER DEFINED CODE (Begin) *}
				
				' Assign the details to the first column.
				' Column 1


                oListItem = lvwPaymentGroups.Items.Add(CStr(m_vPaymentGroups(0, iTemp)), "")
                ' Assign details to other the columns
                ' Column 2

                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vPaymentGroups(1, iTemp))
                ' Column 3 Not visible

                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(CInt(m_vPaymentGroups(2, iTemp)))
                ' Column 4 not visible

                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(CInt(m_vPaymentGroups(3, iTemp)))
                ' {* USER DEFINED CODE (End) *}

            Next iTemp

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
            ' Assign the details from the interface to the data storage.


            m_lReturn = m_oBusiness.Update(lAccountID:=m_lAccountID, vPaymentGroups:=m_vPaymentGroups)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
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
        Dim oListItem As ListViewItem
        Dim i As Integer
        Dim bFirst As Boolean

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




            'Loop round the listview
            i = 1
            bFirst = True

            m_vPaymentGroups = Nothing

            Do
                If i > lvwPaymentGroups.Items.Count Then
                    Exit Do
                End If

                oListItem = lvwPaymentGroups.Items.Item(i - 1)

                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim m_vPaymentGroups(3, i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve m_vPaymentGroups(3, i - 1)
                    End If


                    m_vPaymentGroups(0, i - 1) = oListItem.Text

                    m_vPaymentGroups(1, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 1).Text

                    m_vPaymentGroups(2, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 2).Text

                    m_vPaymentGroups(3, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 3).Text

                End If
                i += 1
            Loop

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


            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwPaymentGroups.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cmdDelete.Enabled = False
            cmdEdit.Enabled = False

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

            m_ctlTabFirstLast(ACControlStart, 0) = lvwPaymentGroups
            m_ctlTabFirstLast(ACControlEnd, 0) = lvwPaymentGroups

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


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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


            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwPaymentGroups.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCaptionHeader1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwPaymentGroups.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCaptionHeader2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


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
    ' Name: GetExisting
    '
    ' Description:
    '
    ' History: 07/09/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetExisting) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetExisting(ByRef bAll As Boolean) As Integer
    '
    'Dim result As Integer = 0
    'Dim oListItem As ListViewItem
    'Dim i, i2 As Integer
    'Dim bFirst As Boolean
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Loop round the listview
    'i = 1
    'i2 = 0
    'bFirst = True
    '
    'Do 
    'If i > lvwPaymentGroups.Items.Count Then
    'Exit Do
    'End If
    '
    'oListItem = lvwPaymentGroups.Items.Item(i - 1)
    '
    'If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
    'Exit Do
    'Else
    'If Not bAll And (oListItem.Selected) Then
    'Ignore this one...
    'Else
    '                If bFirst Then
    '                    ReDim m_vExistingDates(i2)
    '                    bFirst = False
    '                Else
    '                    ReDim Preserve m_vExistingDates(i2)
    '                End If
    ''
    '                txtDate.Text = oListItem.Text
    '                m_vExistingDates(i2) = m_oFormFields.UnformatControl(ctlControl:=txtDate)
    '                i2 = i2 + 1
    'End If
    'End If
    'i += 1
    'Loop 
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExisting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExisting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    '
    ' Name: ValidateOK
    '
    ' Description:
    '
    ' History: 07/09/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateOK() As Integer

        Dim result As Integer = 0
        Dim bFirst As Boolean
        Dim i, iTopRow As Integer
        Dim vPaymentGroups As Object
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            i = 1
            bFirst = True

            vPaymentGroups = Nothing

            Do
                If i > lvwPaymentGroups.Items.Count Then
                    Exit Do
                End If

                oListItem = lvwPaymentGroups.Items.Item(i - 1)

                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim vPaymentGroups(1, i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vPaymentGroups(1, i - 1)
                    End If


                    vPaymentGroups(0, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 2).Text

                    vPaymentGroups(1, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 3).Text

                End If
                i += 1
            Loop
            If Information.IsArray(vPaymentGroups) Then

                For iThisRow As Integer = 0 To vPaymentGroups.GetUpperBound(1)

                    If iThisRow > 0 Then
                        iTopRow = iThisRow - 1
                        For iCheckRow As Integer = 0 To iTopRow




                            If vPaymentGroups(0, iCheckRow).Equals(vPaymentGroups(0, iThisRow)) And vPaymentGroups(1, iCheckRow).Equals(vPaymentGroups(1, iThisRow)) Then
                                MessageBox.Show("Payment Group cannot include the same Company twice, please remove the Duplicate", Application.ProductName)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        Next iCheckRow
                    End If

                Next iThisRow
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOK Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	' PRIVATE Methods (End)
	
	' PRIVATE Events (Begin)
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		
		Dim oListItem As ListViewItem
		
		Try 
			
			
			'Create address component if not already done so
			If m_oPaymentGroup Is Nothing Then
				
				' Get an instance of the contact interface object via
				' the public object manager.
				Dim temp_m_oPaymentGroup As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oPaymentGroup, sClassName:="iACTInsurerPaymentGroup.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
				m_oPaymentGroup = temp_m_oPaymentGroup
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Payment Group component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			
			m_lReturn = CType(m_oPaymentGroup.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'Set these up

			m_oPaymentGroup.GroupID = 0

			m_oPaymentGroup.CompanyID = 0


			m_oPaymentGroup.Business = m_oBusiness

			m_lReturn = m_oPaymentGroup.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, edit grid

			If m_oPaymentGroup.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			


			oListItem = lvwPaymentGroups.Items.Add(m_oPaymentGroup.GroupDesc, "")
			

			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oPaymentGroup.CompanyDesc

			ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oPaymentGroup.GroupID

			ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oPaymentGroup.CompanyID
			
			
			oListItem = Nothing
			
			lvwPaymentGroups.Focus()
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Add command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		
		Try 
			
			'Reset Interface
			cmdEdit.Enabled = False
			cmdDelete.Enabled = False
			
			lvwPaymentGroups.Items.RemoveAt(lvwPaymentGroups.FocusedItem.Index)
			
			lvwPaymentGroups.Focus()
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Delete command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
		Dim oListItem As ListViewItem
		
		Try 
			
			
			'Create address component if not already done so
			If m_oPaymentGroup Is Nothing Then
				
				' Get an instance of the contact interface object via
				' the public object manager.
				Dim temp_m_oPaymentGroup As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oPaymentGroup, sClassName:="iACTInsurerPaymentGroup.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
				m_oPaymentGroup = temp_m_oPaymentGroup
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Payment Group component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			
			m_lReturn = CType(m_oPaymentGroup.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			oListItem = lvwPaymentGroups.FocusedItem
			'    iSelected = oListItem.Tag
			'    m_oPaymentGroup.GroupID = m_vPaymentGroups(2, iSelected)
			'    m_oPaymentGroup.CompanyID = m_vPaymentGroups(3, iSelected)

			m_oPaymentGroup.GroupID = ListViewHelper.GetListViewSubItem(oListItem, 2).Text

			m_oPaymentGroup.CompanyID = ListViewHelper.GetListViewSubItem(oListItem, 3).Text


			m_oPaymentGroup.Business = m_oBusiness

			m_lReturn = m_oPaymentGroup.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, edit grid

			If m_oPaymentGroup.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			

			oListItem.Text = m_oPaymentGroup.GroupDesc
			

			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oPaymentGroup.CompanyDesc

			ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oPaymentGroup.GroupID

			ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oPaymentGroup.CompanyID
			oListItem = Nothing
			
			lvwPaymentGroups.Focus()
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (cmdNavigate_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub cmdNavigate_Click()
		'
		' Click event of the Navigate button.
		'
		'Try 
			'
			' Set the interface status.
			'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
			'
			' Process the next set of actions depending
			' upon the interface task etc.
			'm_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			'
			' Check the return value.
			'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				'Me.Hide()
			'End If
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		'
	'End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			m_lReturn = CType(ValidateOK(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				Exit Sub
			End If
			
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
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
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTInsurerPaymentGroups.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

				sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				

				sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
			End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New iACTInsurerPaymentGroups.General()
			
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

			m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				
				Exit Sub
			End If
			
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
				m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
				
				' Check the return value.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Do not procced with the interface termination.
                    eventArgs.Cancel = 1
					
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
			
			If Not (m_oPaymentGroup Is Nothing) Then
				' Terminate the IPTExtra object

		m_oPaymentGroup.Dispose()
				
				' Check for errors.
				 
				
				' Destroy the instance of the IPTExtra object
				' from memory.
				m_oPaymentGroup = Nothing
			End If
			
			' Terminate the business object

		m_oBusiness.Dispose()
			
			' Check for errors.
			 
			
			' Destroy the instance of the business object
			' from memory.
			m_oBusiness = Nothing
			
			
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
	
	
	Private Sub lvwPaymentGroups_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPaymentGroups.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		'Not if we're viewing, thank you very much
		If Task <> gPMConstants.PMEComponentAction.PMView Then
			If lvwPaymentGroups.GetItemAt(x, y) Is Nothing Then
				cmdDelete.Enabled = False
				cmdAdd.Enabled = True
				cmdEdit.Enabled = False
			Else
				cmdDelete.Enabled = True
				cmdAdd.Enabled = False
				cmdEdit.Enabled = True
			End If
		End If
		
	End Sub
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
		'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
		'Try 
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
	
	' PRIVATE Events (End)
End Class
