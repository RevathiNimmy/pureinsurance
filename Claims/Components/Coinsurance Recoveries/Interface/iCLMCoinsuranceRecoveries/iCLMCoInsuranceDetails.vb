Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer guide no. 129
Imports SharedFiles
Friend Partial Class frmDetails
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmDetails"
    
	Public iError As Integer
	Private sTitle As String = ""
	Private sMessage As String = ""
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Declaration for the member variables
	
	Private vShareArray As Object
	Private m_sPartyName As String = ""
	Private m_dShare As Double
	Private m_cShareValue As Decimal
	Private lst_item As ListViewItem
	
	
	Public Property PartyName() As String
		Get
			Return m_sPartyName
		End Get
		Set(ByVal Value As String)
			m_sPartyName = Value
		End Set
	End Property
	
	
	Public Property Share() As Double
		Get
			Return m_dShare
		End Get
		Set(ByVal Value As Double)
			m_dShare = Value
		End Set
	End Property
	
	
	Public Property ShareValue() As Decimal
		Get
			Return m_cShareValue
		End Get
		Set(ByVal Value As Decimal)
			m_cShareValue = Value
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		'frmInterface.lstView.ListItems(1).Selected = True
		

        Dim sTitle As String = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		

        Dim sMessage As String = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		
		Dim iMsgResult As DialogResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
		
		' Check message result.
		If iMsgResult = System.Windows.Forms.DialogResult.No Then
			' Set return to false, meaning
			' don't cancel.
			Exit Sub
		End If
		
		'    'AJM 27/07/2001 - check if there are any items before trying to set a value
		'    If frmInterface.lstView.ListItems.Count > 0 Then
		'        frmInterface.lstView.ListItems(1).Selected = True
		'    End If
		
		
		Me.Close()
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		Dim lBusinessDataID As Integer
		Dim lst_item As ListViewItem
        Dim ITShare As Double

		Try 
			Application.DoEvents()
			
			If txtBox(g_cISHARE_PERCENTAGE).Text = "0.00" Or txtBox(g_cISHARE_PERCENTAGE).Text.Trim() = "" Then
				iError = 0
			End If
			
			If iError = 0 Then
				iError = 1
				'       If m_lReturn <> PMTrue Then

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShareTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShare, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				iError = 0
				'        End If
				Exit Sub
			End If
			
			' Add the items to the lstView
			' Add the subitems of the LstView to get the Total
			' Share Percentage and the Share Value
			m_sPartyName = cmbBox.Text
			If m_sPartyName <> "" Then
				'        Select Case m_nFindClaimMode
				'        Case PMADD

				m_dShare = CDbl(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatLong, txtBox(g_cISHARE_PERCENTAGE).Text))
				m_cShareValue = CDec(txtBox(g_cICURRENT_SHARE_VALUE).Text)
				
				
				If cmbBox.Enabled Then
                    lst_item = m_ofrmInterface.lstView.Items.Add(m_sPartyName)
					
					ListViewHelper.GetListViewSubItem(lst_item, 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, m_dShare)
					ListViewHelper.GetListViewSubItem(lst_item, 2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cShareValue)
					lst_item.Tag = CStr(VB6.GetItemData(cmbBox, cmbBox.SelectedIndex))
					' leave the new column blank for new mode
					'lBusinessDataID = frmInterface.lstView.SelectedItem.Tag
					
					lBusinessDataID = VB6.GetItemData(cmbBox, cmbBox.SelectedIndex)
					' Inform the business object with a new data item.

					m_lReturn = g_oBusiness.EditAdd(lRow:=lBusinessDataID, vPartyName:=cmbBox.Text, vShare:=txtBox(g_cISHARE_PERCENTAGE).Text, vShareValue:=txtBox(g_cICURRENT_SHARE_VALUE).Text)
					
					'lst_item.Tag = lBusinessDataID
					
					ITShare = 0
                    For ivar1 As Integer = 1 To m_ofrmInterface.lstView.Items.Count
                        lst_item = m_ofrmInterface.lstView.Items.Item(ivar1 - 1)

                        ITShare = CDbl(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatLong, ListViewHelper.GetListViewSubItem(lst_item, 1).Text)) + ITShare
                    Next ivar1
                    m_ofrmInterface.txtTotalSharePercentage.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, ITShare)
                    m_ofrmInterface.txtTotalNewShareValue.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr((ITShare / 100) * CDec(m_ofrmInterface.txtTotalCurrentShareValue.Text)))
				Else
                    lst_item = m_ofrmInterface.lstView.Items.Item(m_ofrmInterface.lstView.FocusedItem.Index)
					ListViewHelper.GetListViewSubItem(lst_item, 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, txtBox(g_cISHARE_PERCENTAGE).Text)
					If ListViewHelper.GetListViewSubItem(lst_item, 3).Text <> "" Then
						ListViewHelper.GetListViewSubItem(lst_item, 3).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, txtBox(g_cINEW_SHARE_VALUE).Text)
					Else
						ListViewHelper.GetListViewSubItem(lst_item, 3).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, txtBox(g_cINEW_SHARE_VALUE).Text)
					End If
					
                    lst_item = m_ofrmInterface.lstView.Items.Item(m_ofrmInterface.lstView.FocusedItem.Index)
					If Convert.ToString(lst_item.Tag) <> "" Then

						lBusinessDataID = Convert.ToString(lst_item.Tag)
						
						' Inform the business object with a new data item.

						m_lReturn = g_oBusiness.EditUpdate(lRow:=lBusinessDataID, vPartyName:=cmbBox.Text, vShare:=txtBox(g_cISHARE_PERCENTAGE).Text, vShareValue:=txtBox(g_cINEW_SHARE_VALUE).Text)
						
					End If
					ITShare = 0
                    For ivar1 As Integer = 1 To m_ofrmInterface.lstView.Items.Count
                        lst_item = m_ofrmInterface.lstView.Items.Item(ivar1 - 1)

                        ITShare += CDbl(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatLong, ListViewHelper.GetListViewSubItem(lst_item, 1).Text))
                    Next ivar1
                    m_ofrmInterface.txtTotalSharePercentage.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, ITShare)
                    m_ofrmInterface.txtTotalNewShareValue.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr((ITShare / 100) * CDec(m_ofrmInterface.txtTotalCurrentShareValue.Text)))
				End If
			End If
			'frmInterface.lstView.ListItems(1).Selected = True
			Me.Close()
		
		Catch excep As System.Exception
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
		End Try
		
	End Sub
	
	
	' ***************************************************************** '
	' Form Name: frmDetails
	'
	' Name : Form_Load()
	'
	' Description: The Load event of the form
	'
	' Author: Ranjit
	
	' Date: 08 June 2000
	' ***************************************************************** '
	

	Private Sub frmDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

		Try 
			iError = 1
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
		
		Catch excep As System.Exception
			
			'   paste the log error message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Load Event", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
		End Try
	End Sub
	
	
	
	' ***************************************************************** '
	' Form Name: frmDetails
	'
	' Name : txtBox_LostFocus()
	'
	' Description: The Text Boxes Lost Focus event
	'
	' Author: Ranjit
	
	' Date: 08 June 2000
	' ***************************************************************** '
	
	Private Sub txtBox_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtBox_1.Leave, _txtBox_0.Leave, _txtBox_2.Leave
		Dim Index As Integer = Array.IndexOf(txtBox, eventSender)

		Try 
			
			Select Case Index
				Case g_cISHARE_PERCENTAGE

					m_lReturn = CType(Text_Validation(g_cISHARE_PERCENTAGE, CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatLong, txtBox(g_cISHARE_PERCENTAGE).Text))), gPMConstants.PMEReturnCode)
					'     If m_lReturn = PMTrue Then
					'        txtBox(g_cISHARE_PERCENTAGE).Text = FormatField(PMFormatPercent, txtBox(g_cISHARE_PERCENTAGE).Text)
					'     End If
				Case g_cICURRENT_SHARE_VALUE

					m_lReturn = CType(Text_Validation(g_cICURRENT_SHARE_VALUE, CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMConstants.PMEFormatStyle.PMFormatLong, txtBox(g_cICURRENT_SHARE_VALUE).Text))), gPMConstants.PMEReturnCode)
					'     If m_lReturn = PMTrue Then
					'        txtBox(g_cICURRENT_SHARE_VALUE).Text = FormatField(PMFormatCurrency, txtBox(g_cICURRENT_SHARE_VALUE).Text)
					'     End If
				Case g_cINEW_SHARE_VALUE

					m_lReturn = CType(Text_Validation(g_cINEW_SHARE_VALUE, CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMConstants.PMEFormatStyle.PMFormatLong, txtBox(g_cINEW_SHARE_VALUE).Text))), gPMConstants.PMEReturnCode)
					'     If m_lReturn = PMTrue Then
					'        txtBox(g_cINEW_SHARE_VALUE).Text = FormatField(PMFormatCurrency, txtBox(g_cINEW_SHARE_VALUE).Text)
					'     End If
			End Select
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iError = 0
			End If
		
		Catch excep As System.Exception
			
			
			' paste the log message code here
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Text Box Lost Focus Event", vApp:=ACApp, vClass:=ACClass, vMethod:="txtBox_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
		End Try
	End Sub
	
	' ***************************************************************** '
	' Form Name: frmDetails
	'
	' Name : Text_Validation()
	'
	' Description: The Text Validation event
	'
	' Author: Ranjit
	
	' Date: 08 June 2000
	' ***************************************************************** '
	
	Public Function Text_Validation(ByRef v_iIndex As Integer, ByRef v_sText As String) As Integer
		Dim result As Integer = 0
		Dim iShare As Double
		Dim cShareValue As Decimal
		Dim ivar1 As Integer
		
		Try 
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Select Case v_iIndex
				Case g_cISHARE_PERCENTAGE
					ivar1 = 0
					ivar1 = (v_sText.IndexOf("-"c) + 1)
					If v_sText <> "" Then
						If ivar1 = 0 Or v_sText = "0" Then
							iShare = CDbl(StringsHelper.Format(CDbl(v_sText), "###.###"))
                            cShareValue = (iShare / 100) * CDec(m_ofrmInterface.txtTotalCurrentShareValue.Text)
							If iShare > 100 Then
								result = gPMConstants.PMEReturnCode.PMFalse
								'Exit Function
							Else
								If cmbBox.Enabled Then
									txtBox(g_cICURRENT_SHARE_VALUE).Text = CStr(cShareValue)
								Else
									txtBox(g_cINEW_SHARE_VALUE).Text = CStr(cShareValue)
								End If
							End If
						Else
							result = gPMConstants.PMEReturnCode.PMFalse
							txtBox(g_cISHARE_PERCENTAGE).Text = ""
							'Exit Function
						End If
					Else
						result = gPMConstants.PMEReturnCode.PMFalse
					End If
				Case g_cICURRENT_SHARE_VALUE
					ivar1 = 0
					ivar1 = (v_sText.IndexOf("-"c) + 1)
					If v_sText <> "" Then
						If ivar1 = 0 Or v_sText = "0" Then
							'     elseif isNuv_sText
							
							cShareValue = CDec(v_sText)
                            iShare = CDbl(StringsHelper.Format((cShareValue / CDec(m_ofrmInterface.txtTotalCurrentShareValue.Text)) * 100, "###.###"))
							If iShare > 100 Then
								result = gPMConstants.PMEReturnCode.PMFalse
								'Exit Function
							End If
							txtBox(g_cISHARE_PERCENTAGE).Text = CStr(iShare)
							
						Else
							result = gPMConstants.PMEReturnCode.PMFalse
							txtBox(g_cICURRENT_SHARE_VALUE).Text = ""
							'Exit Function
						End If
					Else
						result = gPMConstants.PMEReturnCode.PMFalse
					End If
				Case g_cINEW_SHARE_VALUE
					ivar1 = 0
					ivar1 = (v_sText.IndexOf("-"c) + 1)
					If v_sText <> "" Then
						If ivar1 = 0 Or v_sText = "0" Then
							cShareValue = CDec(v_sText)
                            iShare = CDbl(StringsHelper.Format((cShareValue / CDec(m_ofrmInterface.txtTotalCurrentShareValue.Text)) * 100, "###.###"))
							If iShare > 100 Then
								result = gPMConstants.PMEReturnCode.PMFalse
								'Exit Function
							End If
							txtBox(g_cISHARE_PERCENTAGE).Text = CStr(iShare)
							
						Else
							result = gPMConstants.PMEReturnCode.PMFalse
							txtBox(g_cINEW_SHARE_VALUE).Text = ""
							'Exit Function
						End If
					Else
						result = gPMConstants.PMEReturnCode.PMFalse
					End If
			End Select
			If result = gPMConstants.PMEReturnCode.PMFalse Then
				iError = 0
				txtBox(g_cISHARE_PERCENTAGE).Text = ""
				If cmbBox.Enabled Then
					txtBox(g_cICURRENT_SHARE_VALUE).Text = ""
				Else
					txtBox(g_cINEW_SHARE_VALUE).Text = ""
				End If
			End If
			
			Return result
		
		Catch 
			
			
			' paste the log message code here
			
			txtBox(g_cISHARE_PERCENTAGE).Text = ""
			If cmbBox.Enabled Then
				txtBox(g_cICURRENT_SHARE_VALUE).Text = ""
			Else
				txtBox(g_cINEW_SHARE_VALUE).Text = ""
			End If
			Return gPMConstants.PMEReturnCode.PMFalse
		End Try
	End Function
	
	Public Function DisplayCaptions() As Integer
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			
			

            cmdOk.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            SSTab1.SelectedTab.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDetailstabCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lblBox(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lblBox(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSharePercentgae, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lblBox(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrentShareValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lblBox(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewShareValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function

    Private Sub frmDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        'developer guide no.293
        If e.Alt And e.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
    End Sub
End Class