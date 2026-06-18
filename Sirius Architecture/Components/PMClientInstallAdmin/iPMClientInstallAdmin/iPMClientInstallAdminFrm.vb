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
	' Date: 29/01/1999
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
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	'Private m_oBusiness As bPMProductClientInstall.FormAdmin

    'developer guide no. 69
    Dim fDetails As frmDetails
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_vListArray( ,  ) As Object
	
	Private m_iPMProductID As Integer
	Private m_sPMProductCode As String = ""
	Private m_sRequiredServerVersion As String = ""
	Private m_dtServerSoftwareDate As Date
	Private m_sLatestClientVersion As String = ""
	Private m_dtClientSoftwareDate As Date
	Private m_iIsMandatory As gPMConstants.PMEReturnCode
	Private m_iIsAutoInstallable As gPMConstants.PMEReturnCode
	Private m_sPath As String = ""
	Private m_sInstallProgram As String = ""
	Private m_sDescription As String = ""
	Private m_iRebootLevel As Integer
	
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
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	
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
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateList
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function PopulateList() As Integer
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
		
		Dim sProductCode, sProductCaption, sClientVersion, sDescription As String
		Dim iIsMandatory, iIsAutoInstallable As Integer
		Dim sServerVersion As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear the List View
			lvwClientInstalls.Items.Clear()
			
			' Get all of the Product Installs

			m_lReturn = m_oBusiness.GetAll(m_vListArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Client Installation Details from d/b.")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' If there aren't any records then exit
			If Not Information.IsArray(m_vListArray) Then
				cmdView.Enabled = False
				cmdDelete.Enabled = False
				Return result
			End If
			
			For lRow As Integer = m_vListArray.GetLowerBound(1) To m_vListArray.GetUpperBound(1)
				
				sProductCode = CStr(m_vListArray(ACArrayProductCodeCol, lRow))
				sProductCaption = CStr(m_vListArray(ACArrayProductCaptionCol, lRow))
				sClientVersion = CStr(m_vListArray(ACArrayClientVersionCol, lRow))
				sDescription = CStr(m_vListArray(ACArrayDescriptionCol, lRow))
				iIsMandatory = CInt(m_vListArray(ACArrayMandatoryCol, lRow))
				iIsAutoInstallable = CInt(m_vListArray(ACArrayAutoInstallableCol, lRow))
				sServerVersion = CStr(m_vListArray(ACArrayServerVersionCol, lRow))
				
				oListItem = lvwClientInstalls.Items.Add(sProductCode, sProductCaption, "")
				
				ListViewHelper.GetListViewSubItem(oListItem, ACListClientVersionCol).Text = sClientVersion.Trim()
				ListViewHelper.GetListViewSubItem(oListItem, ACListDescriptionCol).Text = sDescription.Trim()
				
				If iIsMandatory = gPMConstants.PMEReturnCode.PMTrue Then
					ListViewHelper.GetListViewSubItem(oListItem, ACListMandatoryCol).Text = "Mandatory"
				Else
					ListViewHelper.GetListViewSubItem(oListItem, ACListMandatoryCol).Text = "Optional"
				End If
				
				If iIsAutoInstallable = gPMConstants.PMEReturnCode.PMTrue Then
					ListViewHelper.GetListViewSubItem(oListItem, ACListAutoInstallableCol).Text = "Auto"
				Else
					ListViewHelper.GetListViewSubItem(oListItem, ACListAutoInstallableCol).Text = "Engineer Required"
				End If
				
				ListViewHelper.GetListViewSubItem(oListItem, ACListServerVersionCol).Text = sServerVersion.Trim()
				
				oListItem.Tag = CStr(m_vListArray(ACArrayProductIDCol, lRow))
				
			Next lRow
			
			cboPMLookup1.RefreshList()
			
			cmdView.Enabled = True
			cmdDelete.Enabled = True
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateListFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		
		Dim lReturn As gPMConstants.PMEReturnCode = CType(AddInstall(), gPMConstants.PMEReturnCode)
		
		lReturn = CType(PopulateList(), gPMConstants.PMEReturnCode)
		
	End Sub
	
	
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		
		

		Try 

			m_iPMProductID = Convert.ToString(lvwClientInstalls.FocusedItem.Tag)
			If m_iPMProductID < 1 Then
				Exit Sub
			End If
			
			Dim lReturn As gPMConstants.PMEReturnCode = DeleteInstall()
			
			lReturn = PopulateList()
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Sub
	
	Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click
		
		

		Try 

			m_iPMProductID = Convert.ToString(lvwClientInstalls.FocusedItem.Tag)
			If m_iPMProductID < 1 Then
				Exit Sub
			End If
			
			
			Dim lReturn As Integer = ViewInstall()
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Sub
	
	' PRIVATE Methods (End)
	
	Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			With uctPMResizer1
				.KeepRatio = False
				.NoResizeByDefault = True
				.FormMinHeight = 3000
				.FormMinWidth = 5000
			End With
			
			With uctPMResizer1
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdDelete", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdAdd", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdView", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdNavigate", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("tabMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("lvwClientInstalls", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("imgIcon", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
			End With
			
			Dim lReturn As gPMConstants.PMEReturnCode = PopulateList()
			
		End If
	End Sub
	
	' PRIVATE Events (Begin)
	
	Private Sub Form_Initialize_Renamed()
		

		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMProductClientInstall.FormAdmin", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				' Display message.
				MessageBox.Show("Failed to create business object.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Exit Sub
			End If
			
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
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
			
			' Set the interface default values.
			m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
			
			' Terminate the business object

            m_oBusiness.Dispose()
			m_oBusiness = Nothing
			
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
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Everything OK, so we can hide the interface.
			Me.Hide()
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
		
		' Click event of the Navigate button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMNavigate
			
			Me.Hide()
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	' PRIVATE Events (End)
	
	' ***************************************************************** '
	' Name: AddInstall
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function AddInstall(Optional ByVal v_sINIFile As String = "") As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sINIFile, sFieldInError, sErrorValue, sFoundVersion As String
		Dim iIsExisting As gPMConstants.PMEReturnCode
		Dim bUpdate As Boolean
		Dim sMsg As String = ""
		Dim iOK As DialogResult
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' If we have not been supplied an INI File
			If v_sINIFile = "" Then
				
				' Get the INI File
				lReturn = CType(GetINIFile(sINIFile), gPMConstants.PMEReturnCode)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse
					MessageBox.Show("Unable to Locate INI File!", Application.ProductName)
					Return result
				End If
				
			Else
				
				' Use the One Supplied
				sINIFile = v_sINIFile
				
			End If
			
			' If we still haven't got one then exit.
			If sINIFile = "" Then
				Return result
			End If
			
			' Extract Details from INI File
			lReturn = CType(GetDetailsFromINI(v_sINIFile:=sINIFile, r_sProductCode:=m_sPMProductCode, r_sRequiredServerVersion:=m_sRequiredServerVersion, r_dtServerSoftwareDate:=m_dtServerSoftwareDate, r_sLatestClientVersion:=m_sLatestClientVersion, r_dtClientSoftwareDate:=m_dtClientSoftwareDate, r_iIsMandatory:=m_iIsMandatory, r_iIsAutoInstallable:=m_iIsAutoInstallable, r_sPath:=m_sPath, r_sInstallProgram:=m_sInstallProgram, r_sDescription:=m_sDescription, r_iRebootLevel:=m_iRebootLevel, r_sFieldInError:=sFieldInError, r_sErrorValue:=sErrorValue), gPMConstants.PMEReturnCode)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				If sErrorValue.Trim() = "" Then
					MessageBox.Show(sFieldInError & " has not been supplied.", "Invalid INI File!")
				Else
					MessageBox.Show(sFieldInError & " supplied with Invalid Value : " & sErrorValue.Trim(), "Invalid INI File!")
				End If
				Return result
			End If
			
			' Reboot/Logoff ONLY Allowed for Sirius Architecture Installs.
			If m_iRebootLevel > 0 Then
				If m_sPMProductCode.Trim() <> gPMConstants.PMProductCode(gPMConstants.PMEProductFamily.pmePFSiriusArchitecture) Then
					MessageBox.Show("Reboot or Logoff ONLY allowed for Sirius Architecture" & Strings.Chr(13) & Strings.Chr(10) & "Reboot Level will be reset to 'No Reboot'", Application.ProductName)
					m_iRebootLevel = 0
				End If
			End If
			
			' Display the Install Details
			lReturn = CType(DisplayInstallDetails(), gPMConstants.PMEReturnCode)
			' Check Use Response
			Select Case lReturn
				' OK, Carry ON
				Case gPMConstants.PMEReturnCode.PMOK
					
					' Cancel
				Case gPMConstants.PMEReturnCode.PMCancel
					
					Return result
					
					' Error
				Case Else
					result = gPMConstants.PMEReturnCode.PMFalse
					MessageBox.Show("Unable to Display Install Details!", Application.ProductName)
					Return result
			End Select
			
			' Check to see if the Server Has the Correct
			' Version Installed

			lReturn = m_oBusiness.CheckServerVersion(v_sPMProductCode:=m_sPMProductCode, v_srequiredserverversion:=m_sRequiredServerVersion, r_sfoundversion:=sFoundVersion)
			' Error ?
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				sMsg = m_sPMProductCode & " Client : " & m_sLatestClientVersion & " requires Server Version : " & m_sRequiredServerVersion
				sMsg = sMsg & Strings.Chr(13) & Strings.Chr(10)
				If sFoundVersion.Trim() = "" Then
					sMsg = sMsg & "No Version Found!"
				Else
					sMsg = sMsg & "Server Version : " & sFoundVersion & " was found!"
				End If
				MessageBox.Show(sMsg, "Required Server Version Not Found")
				Return result
			End If
			
			' Do we already have an Install for this product

			lReturn = m_oBusiness.CheckForExisting(v_sPMProductCode:=m_sPMProductCode, r_ipmproductid:=m_iPMProductID, r_iisexistinginstall:=iIsExisting)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				MessageBox.Show("Unable to Check for Existing Install", Application.ProductName)
				Return result
			End If
			
			' Ask if they want to continue
			If iIsExisting = gPMConstants.PMEReturnCode.PMTrue Then
				iOK = MessageBox.Show("This will update an existing Client Install entry." & Strings.Chr(13) & Strings.Chr(10) & "Are you sure you want to do this?", "Update Existing Entry?", MessageBoxButtons.YesNo)
				If iOK = System.Windows.Forms.DialogResult.Yes Then
					bUpdate = True
				Else
					Return result
				End If
			Else
				bUpdate = False
			End If
			

			lReturn = m_oBusiness.DirectAddOrUpdate(v_bupdate:=bUpdate, v_iPMProductId:=m_iPMProductID, v_srequiredserverversion:=m_sRequiredServerVersion, v_dtServerSoftwareDate:=m_dtServerSoftwareDate, v_sLatestClientVersion:=m_sLatestClientVersion, v_dtClientSoftwareDate:=m_dtClientSoftwareDate, v_iislatestclientmandatory:=m_iIsMandatory, v_iisclientautoinstallable:=m_iIsAutoInstallable, v_sclientinstallpath:=m_sPath, v_sclientinstallprogram:=m_sInstallProgram, v_sclientinstalldescription:=m_sDescription, v_iclientrebootlevel:=m_iRebootLevel)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Error Adding/Updating Entry", Application.ProductName)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddInstallFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddInstall", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ViewInstall
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function ViewInstall() As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the Details

			lReturn = m_oBusiness.GetSingle(v_iPMProductId:=m_iPMProductID, r_sRequiredServerVersion:=m_sRequiredServerVersion, r_dtServerSoftwareDate:=m_dtServerSoftwareDate, r_sLatestClientVersion:=m_sLatestClientVersion, r_dtClientSoftwareDate:=m_dtClientSoftwareDate, r_iislatestclientmandatory:=m_iIsMandatory, r_iisclientautoinstallable:=m_iIsAutoInstallable, r_sclientinstallpath:=m_sPath, r_sclientinstallprogram:=m_sInstallProgram, r_sclientinstalldescription:=m_sDescription, r_iclientrebootlevel:=m_iRebootLevel)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				MessageBox.Show("Error Getting Entry Details from Database.", Application.ProductName)
				Return result
			End If
			
			m_sPMProductCode = cboPMLookup1.ItemCaption(m_iPMProductID)
			
			' Display the Install Details
			lReturn = CType(DisplayInstallDetails(True), gPMConstants.PMEReturnCode)
			If (lReturn <> gPMConstants.PMEReturnCode.PMOK) And (lReturn <> gPMConstants.PMEReturnCode.PMCancel) Then
				result = gPMConstants.PMEReturnCode.PMFalse
				MessageBox.Show("Unable to Display Install Details!", Application.ProductName)
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ViewInstallFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewInstall", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DeleteInstall
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function DeleteInstall() As Integer
		
		Dim result As Integer = 0
		Dim iOK As DialogResult
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_sPMProductCode = cboPMLookup1.ItemCaption(m_iPMProductID)
			
			' Ask if they want to continue
			iOK = MessageBox.Show("Are you sure you to delete the Entry for " & m_sPMProductCode & "?", "Delete Entry?", MessageBoxButtons.YesNo)
			If iOK = System.Windows.Forms.DialogResult.No Then
				Return result
			End If
			

			lReturn = m_oBusiness.DirectDelete(m_iPMProductID)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Unable to Delete the entry!", Application.ProductName)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteInstallFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteInstall", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetINIFile
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function GetINIFile(ByRef r_sINIFile As String) As Integer
		
		Dim result As Integer = 0
		Dim sTmp As String = ""
		Dim lReturn As Integer
		Dim sSystem As String = ""
		


		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Set directory
		sTmp = ""
		lReturn = gPMFunctions.GetSystemName(sSystem)
		CommonDialog1Open.InitialDirectory = "\\" & sSystem
		CommonDialog1Open.FileName = ""
		' Set filters

		CommonDialog1Open.Filter = "INI Files (*.ini)|*.ini|All Files (*.*)|*.*"

        'developer guide no
        'TODO: Replace the commented code below once a suitable replacement of cancelError property is found in VB 
        'CommonDialog1Open.CancelError = False

		' Specify default filter
		CommonDialog1Open.FilterIndex = 1
		' Display the Open dialog box
		CommonDialog1Open.ShowDialog()
		

		Try 
			' Display name of selected file
			sTmp = CommonDialog1Open.FileName


			
			If sTmp = "" Then
				Return result
			Else
				r_sINIFile = sTmp.Trim()
			End If
			
			Return result
			
Err_GetINIFile: 
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetINIFileFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetINIFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			
			Return result
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Function
	
	' ***************************************************************** '
	' Name: DisplayInstallDetails
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Public Function DisplayInstallDetails(Optional ByVal v_bViewMode As Boolean = False) As Integer
		
		Dim result As Integer = 0
        'Dim fDetails As frmDetails
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            fDetails = New frmDetails
			

            With fDetails
                .Text = m_sPMProductCode
                SSTabHelper.SetTabCaption(.tabMain, 0, m_sPMProductCode)

                'developer guide no.26
                .panServerVersion.Name = " " & m_sRequiredServerVersion.Trim()
                .lblServerVersionlabel.Text = " " & m_sRequiredServerVersion.Trim()

                'developer guide no.26
                .panServerDate.Name = " " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_dtServerSoftwareDate)
                .lblServerDatelabel.Text = " " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_dtServerSoftwareDate)

                'developer guide no.26
                .panClientVersion.Name = " " & m_sLatestClientVersion.Trim()
                .lblClientVersionLabel.Text = " " & m_sLatestClientVersion.Trim()

                'developer guide no.26
                .panClientDate.Name = " " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_dtClientSoftwareDate)
                .lblClientDatelabel.Text = " " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_dtClientSoftwareDate)
                If m_iIsAutoInstallable = gPMConstants.PMEReturnCode.PMTrue Then

                    'developer guide no.26
                    .panAutoInstallable.Name = " " & "Yes"
                    .lblAutoInstallablelabel.Text = " " & "Yes"
                Else

                    'developer guide no.26
                    .panAutoInstallable.Name = " " & "No"
                    .lblAutoInstallablelabel.Text = " " & "No"
                End If

                'developer guide no.26
                .panPath.Name = " " & m_sPath
                .lblpathlabel.Text = " " & m_sPath

                'developer guide no.26
                .panProgram.Name = " " & m_sInstallProgram
                .lblProgramlabel.Text = " " & m_sInstallProgram

                Select Case m_iRebootLevel
                    Case 0

                        'developer guide no.26
                        .panRebootLevel.Name = " " & "No Reboot"
                        .lblRebootLevellabel.Text = " " & "No Reboot"
                    Case 1

                        'developer guide no.26
                        .panRebootLevel.Name = " " & "Logoff Only"
                        .lblRebootLevellabel.Text = " " & "Logoff Only"
                    Case 2

                        'developer guide no.26
                        .panRebootLevel.Name = " " & "Full Reboot"
                        .lblRebootLevellabel.Text = " " & "Full Reboot"
                End Select

                If m_iIsMandatory = gPMConstants.PMEReturnCode.PMTrue Then

                    'developer guide no.26
                    .panMandatory.Name = " " & "Yes"
                    .lblMandatory.Text = " " & "Yes"
                Else

                    'developer guide no.26
                    .panMandatory.Name = " " & "No"
                    .lblMandatory.Text = " " & "Yes"
                End If

                'developer guide no.26
                .panDescription.Name = " " & m_sDescription
                .lbldescriptionlabel.Text = " " & m_sDescription
            End With
			
			' In View Mode the Cancel Button is disabled
			fDetails.DisableQuit = v_bViewMode
			
			' Show the Details
			fDetails.ShowDialog()
			
			' Return OK/Cancel
			result = fDetails.Status
			
			fDetails.Close()
			fDetails = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayInstallDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayInstallDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: AutoAdd
	'
	' Description: Add the Supplied Client Install
	'
	' ***************************************************************** '
	Public Function AutoAdd(ByVal v_sINIFile As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add the Install
			lReturn = CType(AddInstall(v_sINIFile), gPMConstants.PMEReturnCode)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoAddFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
