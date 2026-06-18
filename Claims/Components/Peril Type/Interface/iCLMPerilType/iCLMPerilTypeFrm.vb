Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name     : frmInterface
	' Description   : Main interface.
	' Date          : 30/08/2000
	' Author        : Pandu
	' Edit History  :
	' ***************************************************************** '

	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
	Private Const ACColumn1 As Integer = 1
	Private Const ACColumn2 As Integer = 2
	
	'CMG/PB 07112002 Loss Schedule
	'Rule Types
	Private Const ACRuleTypeInitialisation As String = "init"
	Private Const ACRuleTypeValidate As String = "validate"
	Private Const ACRuleTypeFieldLevel As String = "field"
	Private Const ACRuleTypeRowLevel As String = "row"
	Private Const ACRuleTypePayment As String = "payment"

	'DC150302
	Private m_sUnderwritingOrAgency As String = ""
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	'CMG/PB 07112002
	Private m_bLossSchedule As Boolean
	'End CMG
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iCLMPerilType.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	'Stores the data from the GetPerilsForReserve method of the business object.
	Public m_vTotalArray( ,  ) As Object
	
	'GSD Claims Builder constant
    Private m_bClaimsBuilder As Boolean
    Public Const ScreenHelpID As Integer = 30

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
	
	'DC150302
	Public WriteOnly Property UnderwritingOrAgency() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the process mode.
			m_sUnderwritingOrAgency = Value
			
		End Set
	End Property
	
	
	' ***************************************************************** '
	' Name:         GetBusiness
	'
	' Description:  Retrieves the details from the business object.
	'
	' Date:         11/07/00
	'
	' Edit History: SK
	
	'
	' ***************************************************************** '
	
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 		
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Disable parts of the interface while
			' a search is in progress.
			m_lReturn = DisableInterface(bDisable:=True)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = g_oBusiness.GetPerilTypes(r_vResultArray:=m_vTotalArray)
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			'populate the list view for each tab
			'Assign Values to Interface
			m_lReturn = DataToInterfaceSumm()
			Me.Refresh()
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
            End If
			
			'After loading & populating all the listviews
			'set the default to the 1st tab
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            ' Check the return values.
			
			Select Case (m_lReturn)
				Case gPMConstants.PMEReturnCode.PMTrue
					' Found search details.
				Case gPMConstants.PMEReturnCode.PMNotFound
					' No search details found.
				Case Else
					' Failed to get details.
					result = gPMConstants.PMEReturnCode.PMFalse
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
					
					Return result
			End Select
            Return result
		
		Catch excep As System.Exception

			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: DataToInterfaceSumm
	'
	' Description: Updates all interface details from the search data.
	'              storage.
	' Date:30/08/2000
	'
	' Edit History:Pandu
	'
	'
	' ***************************************************************** '
	Public Function DataToInterfaceSumm() As Integer
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
        'Const ACFindImage As String = "FindImage"
		Const ACColPerilTypeID As Integer = 0
		Const ACColPerilType As Integer = 1
		Const ACColDesc As Integer = 2

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			'make the tab on which the listview is to be put as the current tab
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)

			' Clear the search details.
			lvwPerilType.Items.Clear()
			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(m_vTotalArray) Then
				Return result
			End If
			
			' Assign the details to the interface.
			For lRow As Integer = m_vTotalArray.GetLowerBound(1) To m_vTotalArray.GetUpperBound(1)
				
				
				' Assign the details to the first column.
				' Column 1 Reserve Type
				oListItem = lvwPerilType.Items.Add(CStr(m_vTotalArray(ACColPerilType, lRow)).Trim())
				
				' Assign details to other the columns
				' Column 2 Description
                oListItem.SubItems.Add(1).Text = CStr(m_vTotalArray(ACColDesc, lRow)).Trim()

				' Set the tag property with Reserve Type ID
				oListItem.Tag = CStr(m_vTotalArray(ACColPerilTypeID, lRow)).Trim()

				' Refresh the first X amount of rows, to
				' allow the user to see the results instantly.
				If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
					' Select the first item.
					lvwPerilType.Items.Item(0).Selected = True
					
					' Refresh the initial results.
					lvwPerilType.Refresh()
				End If
				'    DoEvents
			Next lRow
			
			'    DoEvents
			
			
			' Select the first item.
			lvwPerilType.Items.Item(0).Selected = True
			
			' Enable the interface now that the search has completed.
			m_lReturn = DisableInterface(bDisable:=False)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception

			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterfaceSumm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DataToProperties
	'
	' Description: Updates the property member from the search data
	'              storage.
	' Date:15/07/00
	'
	' Edit History:Pandu
	'
	' ***************************************************************** '
	Public Function DataToProperties() As Integer
		
		Dim result As Integer = 0
		Dim lSelectedItem As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Store the selected item's tag, so we can use this
			' as the index to the search data storage details.

			lSelectedItem = Convert.ToString(lvwPerilType.Items.Item(lvwPerilType.FocusedItem.Index).Tag)

			Return result
		
		Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values.
	'
	' Date :15/07/2000
	'
	' Edit History : Pandu
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            Const lColumnWidth As Integer = 2850
			
			cmdOK.Enabled = False
			cmdDefinefields.Enabled = False
			CmdShowScreen.Enabled = False
			CmdReserves.Enabled = False
			
			If m_bLossSchedule Then
				cmdInitialisation.Visible = True
				cmdValidate.Visible = True
				cmdFieldLevel.Visible = True
				cmdRowLevel.Visible = True
				cmdPayment.Visible = True
			End If
			
			' Center the interface.
			iPMFunc.CenterForm(Me)
			
			
			'Add the columns to the listview
			lvwPerilType.Columns.Insert(ACColumn1 - 1, "", 94)
			lvwPerilType.Columns.Insert(ACColumn2 - 1, "", 94)
			
			''Set the column widths
            lvwPerilType.Columns.Item(ACColumn1 - 1).Width = CInt(VB6.TwipsToPixelsX(lColumnWidth))
            lvwPerilType.Columns.Item(ACColumn2 - 1).Width = CInt(VB6.TwipsToPixelsX(lColumnWidth))
			
			VB6.SetDefault(cmdOK, True)

			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			' Set to the first tab.
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            lvwPerilType.FullRowSelect = True
            Return result
		
		Catch excep As System.Exception
			
            result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DisplayCaptions
	'
	' Description: Display all language specific captions.
	'
	' Date :15/07/2000
	'
	' Edit History :Pandu
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
            ' Display all language specific captions.

            'Developer Guide No: 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
                Return result
			End If

            'Developer Guide No: 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            cmdDefinefields.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDefineFields, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            CmdShowScreen.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShowScreen, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
            'CMG/PB 07112002 Loss Schedule
            'Developer Guide No: 243
            cmdInitialisation.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInitialisationButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            cmdValidate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACValidateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            cmdFieldLevel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFieldLevelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            cmdRowLevel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRowLevelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            cmdPayment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			'End CMG

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            lvwPerilType.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            lvwPerilType.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

			Return result
		
        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: DisableInterface
	'
	' Description: Disables parts of the interface while a search is
	'              in progress.
	'
	' Date :15/07/2000
	'
	' Edit History :SK
	'
	' ***************************************************************** '
	Private Function DisableInterface(ByRef bDisable As Boolean) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
            cmdOK.Enabled = Not bDisable
			cmdDefinefields.Enabled = Not bDisable
			CmdShowScreen.Enabled = Not bDisable
			CmdReserves.Enabled = Not bDisable
			Return result
		
		Catch excep As System.Exception

			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ResizeInterface
	'
	' Description: Resizes the interface controls.
	'
	' Date :15/07/2000
	'
	' Edit History:Pandu
	' ***************************************************************** '
	Private Function ResizeInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Form Width-7950 Height-4785
			
			If VB6.PixelsToTwipsX(Me.Width) < 7950 Then Me.Width = VB6.TwipsToPixelsX(7950)
			If VB6.PixelsToTwipsY(Me.Height) < 4785 Then Me.Height = VB6.TwipsToPixelsY(4785)
			
            tabMainTab.Width = Width - VB6.TwipsToPixelsX(375)

            tabMainTab.Height = Height - VB6.TwipsToPixelsY(1000)
			
			
            cmdHelp.Left = Width - VB6.TwipsToPixelsX(1320)
            cmdHelp.Top = Height - VB6.TwipsToPixelsY(900)
			
            cmdCancel.Left = Width - VB6.TwipsToPixelsX(2520)
            cmdCancel.Top = Height - VB6.TwipsToPixelsY(900)
			
            cmdOK.Left = Width - VB6.TwipsToPixelsX(3720)
            cmdOK.Top = Height - VB6.TwipsToPixelsY(900)
			
			CmdShowScreen.Left = VB6.TwipsToPixelsX(120)
            CmdShowScreen.Top = Height - VB6.TwipsToPixelsY(900)
			
            lvwPerilType.Width = Width - VB6.TwipsToPixelsX(2055)
            lvwPerilType.Height = Height - VB6.TwipsToPixelsY(1770)
			
			cmdDefinefields.Left = tabMainTab.Width - VB6.TwipsToPixelsX(1335)
            cmdDefinefields.Top = tabMainTab.Height - VB6.TwipsToPixelsY(450) - 20
			
			CmdReserves.Left = tabMainTab.Width - VB6.TwipsToPixelsX(1335)
			'GSD 160702
			If m_bClaimsBuilder Then
                CmdReserves.Top = tabMainTab.Height - VB6.TwipsToPixelsY(450) - 20
			Else
                CmdReserves.Top = tabMainTab.Height - VB6.TwipsToPixelsY(975) - 20

			End If
			
			Return result
		
        Catch
            Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	
	Private Sub cmdDefinefields_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDefinefields.Click
        'developer guide no. 

        Dim oDefineFields As Object
		
		Const PerilMode As Integer = 1
		
		' Create Find Party object
		Dim temp_oDefineFields As Object
		m_lReturn = g_oObjectManager.GetInstance(temp_oDefineFields, sClassName:="iCLMDefnFlds.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
		oDefineFields = temp_oDefineFields
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iPMBFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			Exit Sub
		End If
		
		' Set component properties and start interface

        oDefineFields.CallingAppName = ACApp
        oDefineFields.Mode = gPMConstants.PMEComponentAction.PMAdd
        oDefineFields.DataMode = PerilMode
        oDefineFields.Typeid = Convert.ToString(lvwPerilType.FocusedItem.Tag)
        oDefineFields.TypeName = lvwPerilType.FocusedItem.Text

		m_lReturn = oDefineFields.Start()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			Exit Sub
		End If
        ' Destroy Find Party object
        oDefineFields.Dispose()
        oDefineFields = Nothing
	End Sub
	
	Private Sub cmdFieldLevel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFieldLevel.Click
		m_lReturn = GenerateRule(ACRuleTypeFieldLevel)
	End Sub
	
	Private Sub cmdInitialisation_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInitialisation.Click
		m_lReturn = GenerateRule(ACRuleTypeInitialisation)
	End Sub

	Private Function GenerateRule(ByRef v_sRuleType As String) As Integer
		Dim result As Integer = 0
		Dim oRuleEditor As ipmuruleeditor.Interface_Renamed
        Dim sRuleFileName, sTitle, sMessage As String

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the general interface object object via
			' the public object manager.
			Dim temp_oRuleEditor As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oRuleEditor, sClassName:="ipmuruleeditor.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oRuleEditor = temp_oRuleEditor

			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the interface object.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

                'Developer Guide No: 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRuleEditor, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'Developer Guide No: 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRuleEditorFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Return result
			End If
			
			sRuleFileName = "LossSchedule\" &  _
			                "PerilType" &  _
			                Convert.ToString(lvwPerilType.FocusedItem.Tag) &  _
			                "_" & v_sRuleType &  _
			                ".rul"

            CType(oRuleEditor, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            oRuleEditor.RuleFileName = sRuleFileName
            oRuleEditor.Start()
            oRuleEditor.Dispose()
            oRuleEditor = Nothing
			
			Return result
		
        Catch excep As System.Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load rule editor object", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateRule", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
		End Try
	End Function
	
	
	Private Sub cmdPayment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPayment.Click
		m_lReturn = GenerateRule(ACRuleTypePayment)
	End Sub
	
	Private Sub CmdReserves_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdReserves.Click
        Dim oReserve As iCLMPerilReserveType.Interface_Renamed
        ' Create Find Party object
        Dim temp_oReserve As Object

		m_lReturn = g_oObjectManager.GetInstance(temp_oReserve, sClassName:="iCLMPerilReserveType.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
		oReserve = temp_oReserve
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iCLMPerilReserveType.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			Exit Sub
		End If
		
        ' Set component properties and start interface
        oReserve.CallingAppName = ACApp
        oReserve.PerilTypeId = Convert.ToString(lvwPerilType.FocusedItem.Tag)
        oReserve.PerilTypeDescription = lvwPerilType.FocusedItem.Text
		m_lReturn = oReserve.Start()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			Exit Sub
        End If
		' Destroy Find Party object

        oReserve.Dispose()
        oReserve = Nothing
		
	End Sub
	
	Private Sub cmdRowLevel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRowLevel.Click
		m_lReturn = GenerateRule(ACRuleTypeRowLevel)
	End Sub
	
	Private Sub CmdShowScreen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdShowScreen.Click
		'GSD 170702
		If m_bClaimsBuilder Then
			ClaimsBuilderInterface()
		Else
			PerilInterface()
		End If
		
	End Sub
	
	Private Sub ClaimsBuilderInterface()
		Dim oPeril As iPMURisk.Interface_Renamed
        Dim vKeyArray(,) As Object
		
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		'GSD 150702
		
		ReDim vKeyArray(1, 0)

		vKeyArray(0, 0) = "GIS_Screen_id"


		vKeyArray(1, 0) = Convert.ToString(lvwPerilType.FocusedItem.Tag)
		
		Dim temp_oPeril As Object
		m_lReturn = g_oObjectManager.GetInstance(temp_oPeril, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
		oPeril = temp_oPeril
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object '.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGenericRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			Exit Sub
		End If

		m_lReturn = oPeril.Initialise
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			Exit Sub
        End If
        m_sTransactionType = "C_CR"
		m_lReturn = oPeril.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vTransactionType:=m_sTransactionType)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

		m_lReturn = oPeril.SetKeys(vKeyArray)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			Exit Sub
		End If

		m_lReturn = oPeril.Start()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			Exit Sub
		End If

		If oPeril.Status <> gPMConstants.PMEReturnCode.PMOK Then

            oPeril.Dispose()
            oPeril = Nothing
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			Exit Sub
		End If

		m_lReturn = oPeril.GetKeys(vKeyArray)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			Exit Sub
        End If

        oPeril.Dispose()
        oPeril = Nothing
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
	End Sub
	
	Private Sub PerilInterface()
		
        Const PMKeyPerilTypeID As String = "peril_type_id"
		Dim Var(1, 2) As Object
		Dim vKeyArray(1, 0) As Object

		Dim ACPeril As gPMConstants.PMEComponentAction = gPMConstants.PMEComponentAction.PMView
		
		'        ' Create Find Party object
		'        m_lReturn& = g_oObjectManager.GetInstance( _
		''            oObject:=oGenericPeril, _
		''            sClassName:="iCLMPeril.Interface", _
		''            vInstanceManager:=PMGetLocalInterface)
		'        Set oGenericPeril = CreateObject("iCLMPeril.Interface")
		'
		'
		'    If (m_lReturn& <> PMTrue) Then
		'
		'        LogMessage _
		''            iType:=PMLogOnError, _
		''            sMsg:="Failed to create object 'iCLMPeril.Interface'.", _
		''            vApp:=ACApp, _
		''            vClass:=ACClass, _
		''            vMethod:="GetGenericPeril", _
		''            vErrNo:=Err.Number, _
		''            vErrDesc:=Err.Description
		'        Exit Sub
		'    End If
		
        Dim oGenericPeril As New iCLMPeril.Interface_Renamed

        'Developer Guide No.9
        'CType(oGenericPeril, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        oGenericPeril.Initialise()
		'TN20010620 start
		oGenericPeril.DisableScreen = gPMConstants.PMEReturnCode.PMTrue
        'TN20010620 end	
        vKeyArray(0, 0) = PMKeyPerilTypeID
        vKeyArray(1, 0) = Convert.ToString(lvwPerilType.FocusedItem.Tag)
        oGenericPeril.SetKeys(vKeyArray)
        m_lReturn = oGenericPeril.Start()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iCLMPeril.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGenericRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Exit Sub
        End If
		
		' Destroy Find Party object
		oGenericPeril.Dispose()
		
	End Sub
	
	Private Sub cmdValidate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdValidate.Click
		m_lReturn = GenerateRule(ACRuleTypeValidate)
	End Sub
	
	' ***************************************************************** '
	' Name: FormIntialise
	'
	' Description: Intialise all required details of the form
	'
	' Date:15/07/00
	'
	' Edit History:Pandu
	' DGRIFF : Modified : 13-08-2003 : 1704 : Remove Show Screen button
	'                                         if Claims builder enabled
	' ***************************************************************** '
	Private Sub Form_Initialize_Renamed()
		
		Dim vreturn As String = ""
		Dim vValue As String = ""
		
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Create an instance of the general interface object.
			m_oGeneral = New iCLMPerilType.General()
			
			'CMG/PB See if LossSchedule is enabled and set a boolean
			iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTLossSchedule, g_iSourceID, vValue)
			m_bLossSchedule = (gPMFunctions.NullToString(vValue) = "1")
			'End CMG
			
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			'GSD 150702
			m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTClaimsBuilder, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vreturn)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue failed for Claims Builder", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
			End If
			
			If vreturn = "1" Then
				'DGRIFF : 13-08-2003: 1704
				CmdShowScreen.Visible = False
				m_bClaimsBuilder = True
			End If
			
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

    ' ***************************************************************** '
    ' Name:         FormLoad
    ' Description:  Loads all required details of the form
    ' Date:         15/07/00
    ' Edit History: SK
    ' ***************************************************************** '

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
			
			' Validate fields using Forms Control
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If

			' Set the interface default values.
			m_lReturn = SetInterfaceDefaults()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If

			' Gets the interface details to be displayed.
			m_lReturn = m_oGeneral.GetInterfaceDetails()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the interface details.
				'm_lErrorNumber& = PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			'GSD 150702
			If m_bClaimsBuilder Then
				cmdDefinefields.Visible = False
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
            ' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
    End Sub
	' ***************************************************************** '
	' Name: Form_Query Unload
	'
	' Description: Store all Property Details before unloading form
	'
	' Date:30/08/2000
	'
	' Edit History:Pandu
	' ***************************************************************** '
	Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Forms query unload event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Check if the interface has been terminated by means
			' other than pressing the command buttons.

            'developer guide no. 19 (no solution)
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
		m_oGeneral.Dispose()

            

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

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
	
	' ***************************************************************** '
	' Name:Form_Resize
	'
	' Description: Resize the the controls on form
	'
	' Date:30/08/2000
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	Private isInitializingComponent As Boolean
	Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Try 
			
			m_lReturn = ResizeInterface()
		
		Catch 
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	' ***************************************************************** '
	' Name: lvwPerilType_ColumnClick
	'
	' Description:Sort the Details of List View as per the column clicked
	'
	' Date:30/08/2000
	'
	' Edit History:Pandu
	' ***************************************************************** '
	Private Sub lvwPerilType_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwPerilType.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwPerilType.Columns(eventArgs.Column)
        Try
            'Commented the below code and handled sorting through below function
            ListViewFunc.SortListView(lvwPerilType, eventArgs)

            'With lvwPerilType

            '	' If date column clicked, then sort by date sort column
            '	If ColumnHeader.Index + 1 - 1 = 1 Then
            '		ListViewHelper.SetSortedProperty(lvwPerilType, False)
            '		If ListViewHelper.GetSortKeyProperty(lvwPerilType) <> 1 Then
            '			ListViewHelper.SetSortKeyProperty(lvwPerilType, 1)
            '			ListViewHelper.SetSortOrderProperty(lvwPerilType, SortOrder.Ascending)
            '		Else
            '			ListViewHelper.SetSortOrderProperty(lvwPerilType, (ListViewHelper.GetSortOrderProperty(lvwPerilType) + 1) Mod 2)
            '		End If
            '		ListViewHelper.SetSortedProperty(lvwPerilType, True)

            '		' If current sort column header is
            '		' pressed.
            '	ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwPerilType)) Then 
            '		' Set sort order opposite of
            '		' current direction.
            '		ListViewHelper.SetSortOrderProperty(lvwPerilType, (ListViewHelper.GetSortOrderProperty(lvwPerilType) + 1) Mod 2)
            '		ListViewHelper.SetSortedProperty(lvwPerilType, True)

            '	Else
            '		' Sort by this column (ascending).
            '		ListViewHelper.SetSortedProperty(lvwPerilType, False)

            '		' Turn off sorting so that the list
            '		' is not sorted twice
            '		ListViewHelper.SetSortOrderProperty(lvwPerilType, SortOrder.Ascending)
            '		ListViewHelper.SetSortKeyProperty(lvwPerilType, ColumnHeader.Index + 1 - 1)
            '		ListViewHelper.SetSortedProperty(lvwPerilType, True)
            '	End If
            'End With

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPerilType_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: lvwPerilType_GotFocus
	'
	' Description:Set Ok Button a default
	'
	' Date:30/08/2000
	'
	' Edit History:Pandu
	' ***************************************************************** '
	Private Sub lvwPerilType_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwPerilType.Enter
		' GotFocus Event for the search details
		

		'Try 
			'
			' Unset any default buttons so can select with Enter key.
			'    cmdFindNow.Default = False
			'cmdOK.Default = False
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPerilType_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
			'
		'End Try
		
		
	End Sub
	' ***************************************************************** '
	' Name: lvwPerilType_LostFocus
	'
	' Description:Set find now as default
	'
	' Date:30/08/2000
	'
	' Edit History:Pandu
	' ***************************************************************** '
	Private Sub lvwPerilType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwPerilType.Leave
		' LostFocus Event for the search details
		
		Try 
			
			' Set the default button.
			VB6.SetDefault(cmdOK, True)
		
		Catch excep As System.Exception
			
            ' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPerilType_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
	End Sub
	
	' ***************************************************************** '
	' Name: cmdOK_Click
	'
	' Description:Set Properties of the form on clicking OK Button from the
	'               relevant list item under focus or clicked
	'
	' Date:30/08/2000
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Process the next set of actions.
			m_lReturn = m_oGeneral.ProcessCommand()
			
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
	' ***************************************************************** '
	' Name: cmdCancel_Click
	'
	' Description:Unload the Form
	'
	' Date:30/08/2000
	'
	' Edit History:Pandu
	' ***************************************************************** '
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Process the next set of actions.
			m_lReturn = m_oGeneral.ProcessCommand()
			
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
	
	' ***************************************************************** '
	' Name: DisplayMessage
	'
	' Description: Display the Suitable Message
	'
	' Date:30/08/2000
	'
	' Edit History:Pandu
	
	' ***************************************************************** '

	'Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)
		'
		'Static sMessage As String = ""
		'
		'Try 
			'
			'

			'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			'
			'
			' Display the status message.
			'
			'MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error Section.
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		'
	'End Sub
	
	
	
	
	
	
	
	
	

	'Private Sub lvwRiskType_BeforeLabelEdit(ByRef Cancel As Integer)
		'
	'End Sub
    'For the working of Alt1
    Private Sub tabMainTab_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMainTab.KeyDown
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
            tabMainTab.Focus()
        End If
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(objCnt:=cmdHelp, lContextID:=ScreenHelpID)
    End Sub
End Class
