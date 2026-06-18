Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Friend Partial Class frmRIPortfolioTransfer
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmRIPortfolioTransfer
	'
	' Date: 06/07/04
	'
	' Description: Interface for RI portfolio transfer.
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "frmRIPortfolioTransfer"
	
	' Object parameter members.
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lErrorNumber As Integer
	
	Private Enum DeferredRIField
		edInsuranceFileCnt = 0
		edInsuranceRef = 1
		edClientCode = 2
		edClientName = 3
		edTransferDate = 4
	End Enum
	
	Private m_vPolicies( ,  ) As Object
	
	' Stores the return value for the a function call.
	Private m_lReturn As Integer
	Private m_lItemsFound As Integer
	
	Private m_lProgressValue As Integer
	Private m_sStatusBarText As String = ""
	

	Private m_oBusiness As bSIRRIPortfolioTransfer.Business
	
	
	Public Property Status() As Integer
		Get
			' Return the interface exit status.
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			' set the interface exit status.
			m_lStatus = Value
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdTransfer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTransfer.Click
		
		Dim sMessage As String = ""
		
		Try 
			
			' Get policies to be processed
			m_lReturn = GetBusiness()
			
			' Display message box according to number of policies returned
			If m_lItemsFound < 1 Then
				' No policy matches criteria, warn user and do nothing

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConfirm1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				MessageBox.Show(sMessage, "RI Portfolio Transfer", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Else
				' Policies found, ask user confirmation before processing

				sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConfirm2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				If MessageBox.Show(CStr(m_lItemsFound) & " " & sMessage, "RI Portfolio Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
					' Process those policies
					m_lReturn = TransferPolicies()
				End If
			End If
			
			' If an error's occurred, it should have been handled already
			
			' Set up the interface again
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			cmdCancel.Text = "&Close"
			cmdCancel.Enabled = True
		
		Catch excep As System.Exception
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error occurred whilst processing the policy.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdTransfer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		Dim sMessage, sTitle As String
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Create business  object
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRIPortfolioTransfer.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oBusiness = temp_m_oBusiness
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	

	Private Sub frmRIPortfolioTransfer_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Try 
			
			m_lReturn = SetInterfaceDefaults()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occurred setting interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="form_load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			End If
		
		Catch excep As System.Exception
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name:     TransferPolicies
	' Desc:     Transfer all selected policies to new RI model
	' Author:   Alix Bergeret - 08/07/2004
	' ***************************************************************** '
	Public Function TransferPolicies() As Integer
		
		Dim result As Integer = 0
		Dim lCurrInsFileCnt As Integer
		Dim sMessage As String = ""
        Dim iCount As Long ' E007
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Disable the buttons
			cmdCancel.Enabled = False
			
			For lLoopy As Integer = 0 To (m_lItemsFound - 1)
				
				' Update the interface
                txtPolicyNumber.Text = gPMFunctions.NullToString(m_vPolicies(DeferredRIField.edInsuranceRef, lLoopy)).Trim()
                txtPolicyNumber.Refresh()
                txtClientCode.Text = gPMFunctions.NullToString(m_vPolicies(DeferredRIField.edClientCode, lLoopy)).Trim()
                txtClientCode.Refresh()
                txtClientName.Text = gPMFunctions.NullToString(m_vPolicies(DeferredRIField.edClientName, lLoopy)).Trim()
                txtClientName.Refresh()
				lCurrInsFileCnt = gPMFunctions.NullToLong(m_vPolicies(DeferredRIField.edInsuranceFileCnt, lLoopy))
                _sbrStatus_Panel1.Text = "Processing Policy..."
                sbrStatus.Refresh()

				
				' Process policy

                'm_lReturn = m_oBusiness.ProcessSinglePolicy(v_lInsuranceFileCnt:=lCurrInsFileCnt, v_dtTransferDate:=m_vPolicies(DeferredRIField.edTransferDate, lLoopy), r_sMessage:=sMessage)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' MessageBox.Show(sMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' E007
                    m_lReturn = m_oBusiness.InsertInsFilePTRIUsage(v_lInsFileCnt:=lCurrInsFileCnt, _
                                                                    v_dtTransferDate:=m_vPolicies(DeferredRIField.edTransferDate, lLoopy))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage( _
                                iType:=PMConst.PMLogOnError, _
                                sMsg:="An error has occurred whilst setting the policy for manual processing.", _
                                vApp:=ACApp, _
                                vClass:=ACClass, _
                                vMethod:="TransferPolicies", _
                                vErrNo:=Err.Number, _
                                vErrDesc:=Err.Description)

                        iPMFunc.SetMousePointer(PMConst.PMMouseNormal)
                        'InsertInsFilePTRIUsage = PMFalse
                        Exit Function
                    End If
                    iCount = iCount + 1

                End If

            Next lLoopy
			
			' Finished!
            _sbrStatus_Panel1.Text = "Processing complete."
            sbrStatus.Refresh()
            ' E007
            If iCount = 0 Then
                MessageBox.Show("Processing of policies is complete.", "Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                sMessage = "Processing of policies is complete." + CStr(iCount) + " Policies needed to be manually processed."
                MsgBox(sMessage, MessageBoxButtons.OK, "Processing Complete")
            End If
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransferPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransferPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
	End Function
	
	' *********************************************************************** '
	' Name:     GetBusiness
	' Desc:     Get all policies from DB which are using a replaced RI model
	' Author:   Alix Bergeret
	' *********************************************************************** '
	Private Function GetBusiness() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim lProductID As Integer
		Dim dtTransferDate As Date
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display a searching message
			DisplayStatusSearching()
			
			' Get selection criteria from interface
			lProductID = cboProducts.ItemId

			dtTransferDate = txtDate.Value
			
			' Get matching policies

            'm_lReturn = m_oBusiness.GetPoliciesPortfolioTransfer(v_lProductID:=lProductID, v_dtTransferDate:=dtTransferDate, r_vPolicyArray:=m_vPolicies)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Update the module level variable that holds the number of policies we're dealing with
			If Information.IsArray(m_vPolicies) Then
				m_lItemsFound = m_vPolicies.GetUpperBound(1) + 1
			Else
				m_lItemsFound = 0
			End If
			
			' Display a searching message
			DisplayStatusFound()
			
			' Set the mouse pointer to normal
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name:     SetInterfaceDefaults
	' Desc:     Sets all of the interface default values.
	' Author:   Alix Bergeret - 08/07/2004
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Default some fields
			txtPolicyNumber.Text = ""
			txtClientCode.Text = ""
			txtClientName.Text = ""
			cboProducts.FirstItem = "All"
			txtDate.Value = DateTime.Today
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name:     DisplayStatusSearching
	' Desc:     Display the status searching message.
	' Author:   Alix Bergeret - 08/07/2004
	' ***************************************************************** '
	Private Sub DisplayStatusSearching()
		
		Static sMessage As String = ""
		
		Try 
			
			' Get message text if not already present.
			If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			End If
			
			' Display the status message.
            _sbrStatus_Panel1.Text = sMessage
            sbrStatus.Refresh()
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name:     DisplayStatusFound
	' Desc:     Display the status found message.
	' Author:   Alix Bergeret - 08/07/2004
	' ***************************************************************** '
	Private Sub DisplayStatusFound()
		
		Static sMessage As String = ""
		
		Try 
			
			' Get message text if not already present.
			If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			End If
			
			' Display the status message.
            _sbrStatus_Panel1.Text = CStr(m_lItemsFound) & " " & sMessage
            sbrStatus.Refresh()
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
	End Sub
	
	Private Sub frmRIPortfolioTransfer_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		If Not (m_oBusiness Is Nothing) Then
			
			' Terminate the business object

		m_oBusiness.Dispose()
			

			' Destroy the instance of the business object from memory.
			m_oBusiness = Nothing
			
		End If
		
	End Sub
	
	' ***************************************************************** '
	' Name:     DisplayMessage
	' Desc:     Displays message to report failure of child process.
	' Author:   Alix Bergeret - 08/07/2004
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (DisplayMessage) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function DisplayMessage(ByVal v_sComponentName As String) As Integer
		'
		'Dim result As Integer = 0
		'Dim sTitle, sMessage As String
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Get description from the resource file.

			'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			'

			'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendProcessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			'
			' Display message.
			'MessageBox.Show(sMessage & v_sComponentName, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayMessage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMessage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	' ***************************************************************** '
	' Name:     DisplayCaptions
	' Desc:     Display all language specific captions.
	' Author:   Alix Bergeret - 08/07/2004
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFormCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCmdCancel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdTransfer.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCmdTransfer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblProduct.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblTransferDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblTransferDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblPolicyNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblPolicyNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblClientCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblClientCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblClientName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblClientName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            fmeSelectPolicy.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFrame1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            fmeCurrentPolicy.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFrame2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			Return result
		
		Catch excep As System.Exception
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return gPMConstants.PMEReturnCode.PMError
			
		End Try
	End Function

    
End Class
