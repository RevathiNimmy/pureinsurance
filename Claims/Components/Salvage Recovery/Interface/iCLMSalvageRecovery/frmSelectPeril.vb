Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmSelectPeril
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmSelectPeril"
	
    Dim frmInterface As frmInterface
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	
	' Variables for Select Peril Claim
	Private m_lClaimID As Integer
	Private m_sClaimNumber As String = ""
	Private m_nFindClaimMode As Integer
	Private m_lPerilId As Integer
	Private m_sPerilType As String = ""
	Private m_sClientName As String = ""
	Private m_sPolicyNumber As String = ""
	
	'TN20010329 Start
	Private m_lInsuranceFileCnt As Integer
	Private m_lPerilTypeID As Integer
	'TN20010329 End
	
	'Constants for Column Headers
	Private Const Column1 As Integer = 1
	Private Const Column2 As Integer = 2
	
	'Constants for Defining Width of Columns in List View
	
	Private Const ColWidthPeril As Integer = 3000
	'Array to Store Peril Details
	Private m_vPerilDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	Public Property ClaimNumber() As String
		Get
			
			Return m_sClaimNumber
			
		End Get
		Set(ByVal Value As String)
			
			m_sClaimNumber = Value
			
		End Set
	End Property
	Public Property PolicyNumber() As String
		Get
			
			Return m_sPolicyNumber
			
		End Get
		Set(ByVal Value As String)
			
			m_sPolicyNumber = Value
			
		End Set
	End Property
	
	Public Property ClaimId() As Integer
		Get
			
			Return m_lClaimID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lClaimID = Value
			
		End Set
	End Property
	Public Property ClaimMode() As Integer
		Get
			
			Return m_nFindClaimMode
			
		End Get
		Set(ByVal Value As Integer)
			
			m_nFindClaimMode = Value
			
		End Set
	End Property
	Public Property PerilType() As String
		Get
			
			Return m_sPerilType
			
		End Get
		Set(ByVal Value As String)
			
			m_sPerilType = Value
			
		End Set
	End Property
	
	Public Property PerilID() As Integer
		Get
			
			Return m_lPerilId
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lPerilId = Value
			
		End Set
	End Property
	Public Property ClientName() As String
		Get
			
			Return m_sClientName
			
		End Get
		Set(ByVal Value As String)
			
			m_sClientName = Value
			
		End Set
	End Property
	
	'TN20010329 Start
	Public WriteOnly Property InsuranceFileCnt() As Integer
		Set(ByVal Value As Integer)
			m_lInsuranceFileCnt = Value
		End Set
	End Property
	'TN20010329 End
	
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
	
	Public Property Task() As Integer
		Get
			
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iTask = Value
			
		End Set
	End Property
	
	' ***************************************************************** '
	' Name: FormLoad
	'
	' Description: Loads all required details of the form
	'
	' Date:15/07/00
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click
		'*****Start of Code change Internal Bug id 8
		
		lvwSelectPeril_DoubleClick(lvwSelectPeril, New EventArgs())
		
		'*****end of Code change Internal Bug id  8
		
	End Sub
	

	Private Sub frmSelectPeril_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
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
			
			' Set the interface default values.
			m_lReturn = SetInterfaceDefaults()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			
			'Get Peril Details
			

			m_lReturn = g_oBusiness1.GetPerilDetails(m_vPerilDetails, ClaimId)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				' Failed to get details.
				Exit Sub
			End If
			
			m_lReturn = DataToInterface()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Exit Sub
			End If
			
			txtClaimNumber.Enabled = False
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: FormIntialise
	'
	' Description: Intialise all required details of the form
	'
	' Date:15/07/00
	'
	' Edit History:Pandu
	' ***************************************************************** '
	Private Sub Form_Initialize_Renamed()
		
		' Forms initialise event.
		
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
			
			' Center the interface.
			iPMFunc.CenterForm(Me)
			
			
			lvwSelectPeril.Columns.Insert(Column1 - 1, "", 94)
			lvwSelectPeril.Columns.Insert(Column2 - 1, "", 94)
			
			lvwSelectPeril.Columns.Item(Column1 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthPeril))
			lvwSelectPeril.Columns.Item(Column2 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthPeril))
			
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'RWH(09/04/2001)
			'Made full row select on list views
			m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSelectPeril.Handle.ToInt32(), v_vShowRowSelect:=True)
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Update the interface details with the
			' property members.
			m_lReturn = PropertiesToInterface()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			cmdOK.Enabled = False
			cmdSelect.Enabled = False
			' Set to the first tab.
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			txtClaimNumber.Enabled = False
			txtClaimNumber.BackColor = SystemColors.Control
			
			
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
	' Date :14/08/2000
	'
	' Edit History :Pandu
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			'JMK 10/04/2001 show we're in Salvage Recovery
			Dim sCaption As String = ""

            sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            sCaption = sCaption & " - "


            sCaption = sCaption & CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectPeril, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            Me.Text = sCaption
            '        Me.Caption = iPMFunc.GetResData( _
            ''            iLangID:=g_iLanguageID%, _
            ''            lID:=ACSelectPeril, _
            ''            iDataType:=PMResString)

            'JMK end
            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            'Addded Select Button Pandu 18-10-2000 Internal Bug 8

            cmdSelect.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitleGeneral, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            'Peril


            lvwSelectPeril.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPeril, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            'Description


            lvwSelectPeril.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



            lblClaimNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            txtClaimNumber.Text = m_sClaimNumber.Trim()


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    '
    '
    ' ***************************************************************** '

    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        'Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSelectPeril.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vPerilDetails) Then
                Return result
            End If

            ' Assign the details to the interface.
            For lRow As Integer = m_vPerilDetails.GetLowerBound(1) To m_vPerilDetails.GetUpperBound(1)


                ' Assign the details to the first column.
                ' Column 1 Claim Type

                oListItem = lvwSelectPeril.Items.Add(CStr(m_vPerilDetails(ACIPeril, lRow)).Trim(), "")

                ' Assign details to other the columns
                ' Column 2 Claim Ref
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vPerilDetails(ACIDescription, lRow)).Trim()


                ' Set the tag property with the index of
                ' the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwSelectPeril.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwSelectPeril.Refresh()
                End If
            Next lRow

            ' Select the first item.
            lvwSelectPeril.Items.Item(0).Selected = True

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Edit History :Pandu
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable
            cmdSelect.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:lvwSelectPeril_Click
    '
    ' Description:Fill the Claim Reference,Policy No.,Client Short Name
    '              in Text Box for the listitem clicked
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private Sub lvwSelectPeril_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSelectPeril.Click

        If lvwSelectPeril.Items.Count > 0 Then

            '        sindex = lvwSelectPeril.SelectedItem.Tag
            '
            '                txtClaimRef.Text = Trim(m_vSearchData(ACIClaimRef, sindex))
            '                txtPolicy.Text = Trim(m_vSearchData(ACIInsuranceRef, sindex))
            '
            '            If m_lSiriusUnderWritingBroking = ACUnderWriting Then
            '
            '                txtPolicyHolder.Text = Trim(m_vSearchData(ACIUPolicyHolder, sindex))
            '
            '            Else
            '
            '                txtPolicyHolder.Text = Trim(m_vSearchData(ACIBPolicyHolder, sindex))
            '
            '            End If
            '
            '
            '
            '        cmdOK.Default = True

        End If

    End Sub
    ' ***************************************************************** '
    ' Name: lvwSelectPeril_DblClick
    '
    ' Description:Move to the next form in the road map
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub lvwSelectPeril_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSelectPeril.DoubleClick

        ' Double click event for the search details.

        Try

            ' Check if there are any items available.
            If lvwSelectPeril.Items.Count = 0 Then
                Exit Sub
            End If

            ' Update the property member from the interface.
            m_lReturn = DataToProperties()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to update business.
                Exit Sub
            End If

            m_lReturn = ProcessInterface()


            ' Set the interface status.
            'm_lStatus& = PMOK

            ' Process the next set of actions.
            'm_lReturn& = ProcessCommand()

            ' Check the return value.
            'If (m_lReturn& = PMTrue) Then
            ' Everything OK, so we can hide the interface.
            'Me.Hide
            'End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSelectPeril_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try



    End Sub
    ' ***************************************************************** '
    ' Name:lvwSelectPeril_KeyDown
    '
    ' Description:Set Command Button Ok as Not Default on Pressing Enter Key
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub lvwSelectPeril_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSelectPeril.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If KeyCode <> 13 Then
            VB6.SetDefault(cmdOK, False)
        End If

    End Sub

    ' ***************************************************************** '
    ' Name:lvwSelectPeril_KeyPress
    '
    ' Description:Fill the Policy Number in Text Box when enter button is
    '               pressed when focus is  on list item
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private Sub lvwSelectPeril_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwSelectPeril.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        'Dim sindex As Integer
        '
        '    If (KeyAscii = 13) Then
        '        If (lvwSelectPeril.ListItems.Count > 0) Then
        '            sindex = lvwSelectPeril.SelectedItem.Tag
        '
        '            txtClaimRef.Text = Trim(m_vSearchData(ACIClaimRef, sindex))
        '            txtPolicy.Text = Trim(m_vSearchData(ACIInsuranceRef, sindex))
        '
        '            If m_lSiriusUnderWritingBroking = ACUnderWriting Then
        '
        '                txtPolicyHolder.Text = Trim(m_vSearchData(ACIUPolicyHolder, sindex))
        '
        '            Else
        '
        '                txtPolicyHolder.Text = Trim(m_vSearchData(ACIBPolicyHolder, sindex))
        '
        '            End If
        '
        '
        '            cmdOK.Default = True
        '        End If
        '    End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ' ***************************************************************** '
    ' Name: cmdOK_Click
    '
    ' Description:Set Properties of the form on clicking OK Button from the
    '               relevant list item under focus or clicked
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


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
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = ProcessCommand()

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
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if form has been cancelled, if so, prompt
            ' if you wish to lose details.
            If Status = gPMConstants.PMEReturnCode.PMCancel Then

                ' Get string messages


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Set return to false, meaning don't cancel.
                    result = gPMConstants.PMEReturnCode.PMFalse
                Else

                    m_lReturn = g_oBusiness1.TidyUpAfterCancel(v_lWorkClaimID:=m_lClaimID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to tidy up...
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to tidy up work data following user cancel", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                    End If
                End If

            Else

                ' Update the property member from the interface.
                m_lReturn = DataToProperties()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update business.
                    Return result
                End If

                m_lReturn = ProcessInterface()



            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
	'PUBLIC Methods (End)
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

			lSelectedItem = Convert.ToString(lvwSelectPeril.Items.Item(lvwSelectPeril.FocusedItem.Index).Tag)
			
			' Update the property members.
			
			
			
			m_lPerilId = CInt(CStr(m_vPerilDetails(ACIPerilId, lSelectedItem)).Trim())
			m_sPerilType = CStr(m_vPerilDetails(ACIPeril, lSelectedItem)).Trim()
			
			'TN20010330 Start
			m_lPerilTypeID = CInt(m_vPerilDetails(ACIPerilTypeId, lSelectedItem))
			'TN20010330 End
			
			Return result
		
		Catch excep As System.Exception
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: ProcessInterface (Standard Method)
	'
	' Description: Calls the appropriate methods to process the
	'              interface.
	'
	' Date :11/08/2000
	'
	' Edit History:Pandu
	' ***************************************************************** '
	Private Function ProcessInterface() As Integer
        Dim frmInterface As New frmInterface
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Load the interface into memory.
			m_lReturn = LoadInterface()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to load the interface.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Display the interface.
			m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to display the inteface.
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'RWH(25/06/01) Quick dirty fix to prevent salvage amounts being
			're-applied if form is cancelled.
			If ClaimMode = gPMConstants.PMEComponentAction.PMView Then
				Me.Hide()
				If frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
					m_lStatus = gPMConstants.PMEReturnCode.PMCancel
				End If
			End If
			
			' Destroy the interface from memory.
			m_lReturn = UnLoadInterface()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to unload the interface.
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'RWH(25/06/01)
			If ClaimMode = gPMConstants.PMEComponentAction.PMView Then
				Me.Close()
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: LoadInterface (Standard Method)
	'
	' Description: Loads the instance of the interface into memory and
	'              passes the parameters in.
	'
	' Date :11/08/2000
	'
	' Edit History:Pandu
	'***************************************************************** '
	Private Function LoadInterface() As Integer
        Dim frmInterface As frmInterface
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the parameters to the interface properties.
			With frmInterface
				.CallingAppName = m_sCallingAppName
				.Navigate = m_lNavigate
				.ProcessMode = m_lProcessMode
				.TransactionType = m_sTransactionType
				.EffectiveDate = m_dtEffectiveDate
				.Task = m_iTask
				
				.ClaimNumber = m_sClaimNumber
				.ClaimId = m_lClaimID
				.ClaimMode = m_nFindClaimMode
				.PerilID = m_lPerilId
				.PerilType = m_sPerilType
				
				'TN20010329 Start
				.InsuranceFileCnt = m_lInsuranceFileCnt
				.PerilTypeID = m_lPerilTypeID
				'TN20010329 End
			End With
			
			' Load the instance of the interface into memory.
			Dim tempLoadForm As frmInterface = frmInterface
			
			' Check if we have had an error so far.
			If frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
				' We have already encountered an error,
				' so we MUST return the error.
				result = frmInterface.ErrorNumber
				
			Else
				
				
				Select Case ClaimMode
					Case gPMConstants.PMEComponentAction.PMAdd
						'Set the caption according to the mode
						frmInterface.Text = "[" & m_sClientName & "] " & m_sClaimNumber
					Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView
						frmInterface.Text = "[" & m_sClientName & "] " & m_sClaimNumber
						
						
				End Select
				
				
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface into memory", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: UnLoadInterface (Standard Method)
	'
	' Description: Unloads the instance of the interface from memory.
	'
	' Date :11/08/2000
	'
	' Edit History :Pandu
	' ***************************************************************** '
	Private Function UnLoadInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the property members from the interface parameters.
			With frmInterface
				
				m_lStatus = .Status
				
				
				m_lClaimID = .ClaimId
				m_sClaimNumber = .ClaimNumber
				
				
			End With
			
			' Unload and destroy the instance of the interface
			' from memory.
			frmInterface.Close()
			frmInterface = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unload the interface from memory", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLoadInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ShowInterface (Standard Method)
	'
	' Description: Displays the instance of the interface using the
	'              display state.
	'
	' Date :11/08/2000
	'
	' Edit History :Pandu
	' ***************************************************************** '
	Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			
			' Display the interface.
			VB6.ShowForm(frmInterface, lDisplayState)
			
			'    If (lDisplayState& = vbModal) Then
			'        ' Check for any form errors.
			'        If (frmInterface.ErrorNumber <> 0) Then
			'            ShowInterface = frmInterface.ErrorNumber
			'        End If
			'    End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			'Form Width is 8325 Height is 5820
			
			If VB6.PixelsToTwipsX(Me.Width) < 9345 Then Me.Width = VB6.TwipsToPixelsX(8345)
			If VB6.PixelsToTwipsY(Me.Height) < 6645 Then Me.Height = VB6.TwipsToPixelsY(5645)
			
			
			'ImgImage.Left = Me.Width - 975
			
			'Changed Width tabmaintab Pandu 18-10-2000 Internal Bug 8
			tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(390)
			
			tabMainTab.Height = Me.Height - VB6.TwipsToPixelsY(1125)
			
			'Changed Width lvwselectperil Pandu 18-10-2000 Internal Bug 8
			lvwSelectPeril.Width = Me.Width - VB6.TwipsToPixelsX(1950)
			
			lvwSelectPeril.Height = Me.Height - VB6.TwipsToPixelsY(2325)
			
			'Changed Width cmdCancel Pandu 18-10-2000 Internal Bug 8
			cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(1365)
			
			cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(900)
			
			'Changed Width cmdOK Pandu 18-10-2000 Internal Bug 8
			cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(2625)
			
			cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(900)
			
			'Added Select Button
			cmdSelect.Left = Me.Width - VB6.TwipsToPixelsX(1605)
			cmdSelect.Top = lvwSelectPeril.Top + VB6.TwipsToPixelsY(290)
			
			Return result
		
		Catch 
			
			
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' ***************************************************************** '
	' Name:Form_Resize
	'
	' Description: Resize the the controls on form
	'
	' Date:11/07/00
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	Private isInitializingComponent As Boolean
	Private Sub frmSelectPeril_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
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
	'
	' Name: UnlockClaim
	'
	' Description:
	'
	' History: 17/09/2000 Tomo - Created.
	'
	' ***************************************************************** '
	Public Function UnlockClaim(ByVal v_lOriginalClaimID As Integer) As Integer
		Dim result As Integer = 0
		Dim oPMLock As bPMLock.User
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get bPMLock
			Dim temp_oPMLock As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			oPMLock = temp_oPMLock
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Return result
			End If
			
			'PSL 11/07/2003 don't unlock if it is in View mode
			'PSL 15/07/2003 NO we do want to unlock in view mode
			'    If m_iTask <> PMView And m_iTask <> PMDummyDelete Then
			

			m_lReturn = oPMLock.UnlockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=g_oObjectManager.UserID)
			
			' DD 26/7/2004 - PN13122
			' Only error if return = PMError. If return = PMFalse, it just means
			' the claim was not locked in the first place.
			'If (m_lReturn <> PMTrue) Then
			If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock claim", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return result
				
			End If
			'End If
			oPMLock = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: ZeroAndCloseClaim
	'
	' Description: zero all reserves/recoveries and close claim
	'
	' History: 29/08/2001 TN - Created.
	'
	' ***************************************************************** '

	'Private Function ZeroAndCloseClaim(ByVal v_lClaimID As Integer) As Integer
		'Dim result As Integer = 0
		'Dim bOpenClaim As Object
		'

		'Dim oObject As bOpenClaim.Business
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'Dim temp_oObject As Object
			'm_lReturn = g_oObjectManager.GetInstance(temp_oObject, "bOpenClaim.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			'oObject = temp_oObject
			'
			'
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'
				' Log Error Message
				'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bOpenClaim", vApp:=ACApp, vClass:=ACClass, vMethod:="ZeroAndCloseClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				'
				'
				'Return gPMConstants.PMEReturnCode.PMFalse
			'End If
			'

			'result = oObject.ZeroAndCloseClaim(v_lClaimID:=v_lClaimID)
			'

			'm_lReturn = oObject.Terminate()
			'
			'oObject = Nothing
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
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ZeroAndCloseClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ZeroAndCloseClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
End Class
