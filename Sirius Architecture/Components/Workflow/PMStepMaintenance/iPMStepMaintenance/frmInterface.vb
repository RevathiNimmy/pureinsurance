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
	' Date:
	'
	' Description: Main interface form.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "frmInterface"
	
	'********************************
	' General Property variables
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lPMAuthorityLevel As Integer
	Private m_bError As Integer

	Private m_oBusiness As bPMStepMaintenance.Business

	Private m_oEventTaskBusiness As bPMEventTask.Business
	Private m_lReturn As Integer
	Private m_bInterfaceError As Boolean
	'********************************
	
	Private m_lSelectedItem As Integer
	Private m_colMaintainItems As Collection
	Private m_frmDetails As frmDetails
	Private m_vDocumentTemplates As Object
	Private m_lMaintainItemIdCounter As Integer
	Private m_vCodes As Object
	Private m_vTaskOutcomes As Object
	Private m_vTaskActionTypeOutcomes As Object
	
	Private m_vLookupDetails( ,  ) As Object
	Private m_vLookupTables( ,  ) As Object
	Private m_vPMUsers As Object
	Private m_vTaskGroupTask( ,  ) As Object
	Private m_vTaskGroupTaskAction( ,  ) As Object
	Private m_vTaskActionDueDays As Object
	Private m_vPMUserGroupUsers( ,  ) As Object
	Private m_vTaskGroupUserGroups( ,  ) As Object
	Private m_vTaskEvent As Object
	Private m_vWorkflowSteps( ,  ) As Object
	Private m_vValidUserBranches( ,  ) As Object
	
	Private m_lWorkflowId As Integer
	Private m_lStepOrderCounter As Integer
	
	Public WriteOnly Property WorkflowId() As Integer
		Set(ByVal Value As Integer)
			m_lWorkflowId = Value
		End Set
	End Property
	
	'********************************
	' General Interface Properties
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			m_sCallingAppName = Value
		End Set
	End Property
	Public WriteOnly Property PMAuthoritylevel() As Integer
		Set(ByVal Value As Integer)
			m_lPMAuthorityLevel = Value
		End Set
	End Property
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	Public ReadOnly Property Error_Renamed() As Integer
		Get
			Return m_bError
		End Get
	End Property
	'********************************
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		If ActionAdd() = gPMConstants.PMEReturnCode.PMTrue Then
			
		End If
	End Sub
	
	Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
		m_lReturn = ActionApply()
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Me.Close()
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		m_lReturn = ActionDelete()
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		m_lReturn = ActionEdit()
	End Sub
	
	Private Sub cmdMoveDown_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMoveDown.Click
		ActionMoveStepDown()
	End Sub
	
	Private Sub cmdMoveUp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMoveUp.Click
		ActionMoveStepUp()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lReturn = ActionOK()
	End Sub
	
	Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click
		m_lReturn = ActionView()
	End Sub
	
	' ***************************************************************** '
	' Name: Form_Initialize
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process Id
	' ***************************************************************** '
	Private Sub Form_Initialize_Renamed()
		
		Const sFunction As String = "Form_Initialize"
		
		Try 
			
			' initialise form error indicator
			m_bError = False
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			If g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMStepMaintenance.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
				m_oBusiness = temp_m_oBusiness
				
				' interface error shut down
				m_bError = False
				
				' Failed to get an instance of the business object.
                gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & "Failed to create business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:=New Exception(Information.Err().Description))
				
				' reset mouse pointer
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
			Else
				m_oBusiness = temp_m_oBusiness
			End If
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oEventTaskBusiness As Object
			If g_oObjectManager.GetInstance(temp_m_oEventTaskBusiness, "bPMEventTask.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
				m_oEventTaskBusiness = temp_m_oEventTaskBusiness
				
				' interface error shut down
				m_bError = False
				
				' Failed to get an instance of the business object.
                gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & "Failed to create business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:=New Exception(Information.Err().Description))
				
				' reset mouse pointer
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
			Else
				m_oEventTaskBusiness = temp_m_oEventTaskBusiness
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:=excep)
			
			' reset mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Exit Sub
			
		End Try
	End Sub
	
	' ***************************************************************** '
	' Name: Form_QueryUnload
	'
	' Description: Determines whether any actions need to take place
	'               before unload.
	' History:
	'           Created : MEvans : Date : Process Id
	' ***************************************************************** '
	Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	' ***************************************************************** '
	' Name: Form_Unload
	'
	' Description: Destroys all object references
	'
	' History:
	'           Created : MEvans : Date : Process Id
	' ***************************************************************** '
	Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		Const sFunctionName As String = "Form_Unload"
		
		Try 
			
			' destroy collection
			m_colMaintainItems = Nothing
			
			' Terminate the business object

            m_oBusiness.Dispose()
            ' destroy object reference
			m_oBusiness = Nothing
		
		Catch excep As System.Exception
			
			
			
			m_bInterfaceError = True
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: Form_Load
	'
	' Parameters: N/A
	'
	' Description: Sets up the form, populates controls, etc
	'
	' History:
	'           Created : MEvans : Date : Process Id
	' ***************************************************************** '

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Const sFunctionName As String = "Form_Load"
		
		Try 
			
			' set up interface
			m_lReturn = SetUpForm()
			
			
			
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			m_bInterfaceError = True
			
			'******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SetUpForm
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-06-2003 : WorkFlow
    ' ***************************************************************** '
    Private Function SetUpForm() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SetUpForm"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetResourceData()

            m_lReturn = GetData()

            m_lReturn = SetupListView()

            m_lReturn = GetMaintainData()

            m_lReturn = PopulateListView()

            m_lReturn = SetupButtons()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetResString
    '
    ' Parameters: n/a
    '
    ' Description: Returns string item from resource file
    '
    ' History:
    '           Created : MEvans : Date : Process Id
    ' ***************************************************************** '
    Private Function GetResString(ByVal v_lItemId As Integer) As String

        Dim result As String = String.Empty
        Const sFunctionName As String = "GetResString"

        Dim sReturn As String = ""

        Try

            ' always want to return a string

            sReturn = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=v_lItemId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            Return sReturn

        Catch excep As System.Exception



            result = "Error"

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: LogMsg
    '
    ' Parameters: n/a
    '
    ' Description: Wrapper for default LogMessageToFile function
    '
    ' History:
    '           Created : MEvans : 28-05-2003 : 223
    ' ***************************************************************** '
    Private Sub LogMsg(ByVal v_sMsg As String, ByVal v_sMethod As String)

        Const sFunctionName As String = "LogMsg"

        Try

            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=v_sMethod & ":" & v_sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=v_sMethod)

        Catch excep As System.Exception



            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: GetResourceData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-06-2003 : WorkFlow
    ' ***************************************************************** '
    Private Function GetResourceData() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetResourceData"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Me.Text = GetResString(ACResDataInterfaceTitle)
            tabMainTab.SelectedTab.Text = GetResString(ACResDataInterfaceTabActionTypes)

            cmdAdd.Text = GetResString(ACResDataInterfaceButtonAdd)
            cmdEdit.Text = GetResString(ACResDataInterfaceButtonEdit)
            cmdView.Text = GetResString(ACResDataInterfaceButtonView)
            cmdOK.Text = GetResString(ACResDataInterfaceButtonOK)
            cmdCancel.Text = GetResString(ACResDataInterfaceButtonCancel)
            cmdApply.Text = GetResString(ACResDataInterfaceButtonApply)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetupListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-06-2003 : Workflow
    ' ***************************************************************** '
    Private Function SetupListView() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SetupListView"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' sets up additional listview options such as full row select
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwTable.Handle.ToInt32(), v_vShowGridLines:=True, v_vShowRowSelect:=True)


            ' set the list view headers
            m_lReturn = SetColumnsHeaders()


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:SetColumnsHeaders
    '
    ' Parameters:n/a
    '
    ' Description: Retrieves the column header description from the
    '                   resource file and adds them into the columns
    ' History:
    '           Created : MEvans : 23-06-2003 : WorkFlow
    ' ***************************************************************** '
    Private Function SetColumnsHeaders() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SetColumnsHeaders"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the list view column headers
            lvwTable.Columns.Clear()

            ' Grid Column Header Captions
            lvwTable.Columns.Insert(ACListViewColIndexCode - 1, ACListViewColKeyCode, GetResString(ACResDataInterfaceListViewHeaderCode), 94)
            lvwTable.Columns.Insert(ACListViewColIndexDescription - 1, ACListViewColKeyDescription, GetResString(ACResDataInterfaceListViewHeaderDescription), 94)
            lvwTable.Columns.Insert(ACListViewColIndexEffectiveDate - 1, ACListViewColKeyEffectiveDate, GetResString(ACResDataInterfaceListViewHeaderEffectiveDate), 94)
            lvwTable.Columns.Insert(ACListViewColIndexStepOrder - 1, ACListViewColKeyStepOrder, GetResString(ACResDataInterfaceListViewHeaderStepOrder), 94)

            ' Tag Column Headers to identify in column click so we can sort correctly
            lvwTable.Columns.Item(ACListViewColIndexCode - 1).Tag = ACListViewTagTypeString
            lvwTable.Columns.Item(ACListViewColIndexDescription - 1).Tag = ACListViewTagTypeString
            lvwTable.Columns.Item(ACListViewColIndexEffectiveDate - 1).Tag = ACListViewTagTypeDate
            lvwTable.Columns.Item(ACListViewColIndexStepOrder - 1).Tag = ACListViewTagTypeNumber

            ' set column sizes
            lvwTable.Columns.Item(ACListViewColIndexCode - 1).Width = CInt(VB6.TwipsToPixelsX(1300))
            lvwTable.Columns.Item(ACListViewColIndexDescription - 1).Width = CInt(VB6.TwipsToPixelsX(3550))
            lvwTable.Columns.Item(ACListViewColIndexEffectiveDate - 1).Width = CInt(VB6.TwipsToPixelsX(1050))
            lvwTable.Columns.Item(ACListViewColIndexStepOrder - 1).Width = CInt(0)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SortListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-06-2003 : 223
    ' ***************************************************************** '
    Private Function SortListView(ByRef r_oListView As ListView, ByVal ColumnHeader As ColumnHeader, Optional ByVal v_lSortOrder As Integer = -1) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SortListView"

        Dim sColHeaderTag As String = ""
        Dim lSort As SortOrder
        Dim lSourceCol As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get column header tag so we can determine column header

            sColHeaderTag = Convert.ToString(ColumnHeader.Tag)

            ' we dont want to resort on image header
            If sColHeaderTag <> ACListViewTagTypeImage Then

                If v_lSortOrder = -1 Then
                    ' determine new sort order
                    If ListViewHelper.GetSortOrderProperty(r_oListView) = SortOrder.Ascending Then
                        lSort = SortOrder.Descending
                    Else
                        lSort = SortOrder.Ascending
                    End If
                Else
                    lSort = v_lSortOrder
                End If

                ' determine the column header to sort by
                lSourceCol = ColumnHeader.Index + 1 - 1

                ' If its the created date, then sort by that
                If sColHeaderTag = ACListViewTagTypeDate Then
                    'Developer Guide No.178
                    ' m_lReturn = ListViewSortByDate(v_oListView:=r_oListView, v_iSourceColumn:=lSourceCol, v_iDirection:=lSort)
                    'start
                    m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=r_oListView, v_iSourceColumn:=lSourceCol, v_iDirection:=lSort)

                ElseIf sColHeaderTag = ACListViewTagTypeNumber Then

                    m_lReturn = ListViewFunc.ListViewSortByValue(v_oListView:=r_oListView, v_iSourceColumn:=lSourceCol, v_iDirection:=lSort)
                    'end
                Else
                    ' ACListViewTagTypeString

                    ' else sort by normal field
                    ListViewHelper.SetSortedProperty(r_oListView, False)
                    ListViewHelper.SetSortOrderProperty(r_oListView, lSort)
                    ListViewHelper.SetSortKeyProperty(r_oListView, lSourceCol)
                    ListViewHelper.SetSortedProperty(r_oListView, True)

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function


    Private Sub lvwTable_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTable.Click

        If Not (lvwTable.FocusedItem Is Nothing) Then


            m_lSelectedItem = Convert.ToString(lvwTable.FocusedItem.Tag)
            lvwTable.FocusedItem.Selected = True

            ' setup buttons
            m_lReturn = SetupButtons()

        Else

            ' remove the selection
            m_lSelectedItem = 0

            ' setup buttons
            m_lReturn = SetupButtons()

        End If
    End Sub

    Private Sub lvwTable_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwTable.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwTable.Columns(eventArgs.Column)

        ' reset the porder item selected to be the first in the list
        m_lReturn = SortListView(lvwTable, ColumnHeader)

        If lvwTable.Items.Count > 0 Then
            ' display the first item in the visible list
            lvwTable.FocusedItem = lvwTable.TopItem

            m_lSelectedItem = Convert.ToString(lvwTable.FocusedItem.Tag)
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: GetMaintainData
    '
    ' Parameters: n/a
    '
    ' Description: Retrieves the data for the form
    '
    ' History:
    '           Created : MEvans : 23-06-2003 : Workflow
    ' ***************************************************************** '
    Private Function GetMaintainData() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetMaintainData"

        Dim llBound, lUBound As Integer
        Dim vMaintainData As Object
        Dim oMaintainData As MaintainData

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' set up the collection
            m_colMaintainItems = New Collection()

            ' get data to maintain

            If m_oBusiness.GetMaintainData(v_lWorkflowId:=m_lWorkflowId, r_vResults:=vMaintainData) = gPMConstants.PMEReturnCode.PMTrue Then

                ' if there is any existing data
                If Information.IsArray(vMaintainData) Then

                    ' get array boundaries

                    llBound = vMaintainData.GetLowerBound(1)

                    lUBound = vMaintainData.GetUpperBound(1)

                    ' for each item in the array
                    For lItem As Integer = llBound To lUBound

                        ' create new instance of maintain data
                        oMaintainData = New MaintainData()

                        ' populate maintain data object


                        oMaintainData.Id = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataWorkflowStepId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.Code = CStr(vMaintainData(ACMaintainDataStepCode, lItem)).Trim()

                        oMaintainData.description = CStr(vMaintainData(ACMaintainDataStepDescription, lItem)).Trim()

                        oMaintainData.IsDeleted = CBool(ConvertFromNullValues(vMaintainData(ACMaintainDataIsDeleted, lItem), gPMConstants.PMEDataType.PMBoolean))

                        oMaintainData.EffectiveDate = CDate(ConvertFromNullValues(vMaintainData(ACMaintainDataEffectiveDate, lItem), gPMConstants.PMEDataType.PMDate))


                        oMaintainData.WorkflowId = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataWorkflowId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.StepOrder = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataStepOrder, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.TaskGroupId = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataTaskGroupId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.TaskId = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataTaskId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.PMUserGroupId = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataPMUserGroupId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.UserId = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataUserId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.StepDaysDuration = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataStepDayDuration, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.CompleteNextWorkflowStepId = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataCompleteNextWorkflowStepId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.OverdueNextWorkflowStepId = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataOverdueNextWorkflowStepId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.ExecutableTask = CBool(ConvertFromNullValues(vMaintainData(ACMaintainDataExecutableTask, lItem), gPMConstants.PMEDataType.PMBoolean))

                        oMaintainData.TaskActionTypeid = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataTaskActionTypeId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.EventTypeId = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataEventTypeId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.EventDescription = CStr(ConvertFromNullValues(vMaintainData(ACMaintainDataEventDescription, lItem), gPMConstants.PMEDataType.PMString))

                        oMaintainData.TaskDescription = CStr(ConvertFromNullValues(vMaintainData(ACMaintainDataTaskDescription, lItem), gPMConstants.PMEDataType.PMString))

                        oMaintainData.EventLogSubjectId = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataEventLogSubjectId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.IsUrgent = CBool(ConvertFromNullValues(vMaintainData(ACMaintainDataIsUrgent, lItem), gPMConstants.PMEDataType.PMBoolean))

                        oMaintainData.Customer = CStr(vMaintainData(ACMaintainDataCustomer, lItem)).Trim()

                        oMaintainData.Workflow = CStr(vMaintainData(ACMaintainDataWorkflow, lItem)).Trim()

                        oMaintainData.BranchId = CInt(ConvertFromNullValues(vMaintainData(ACMaintainDataBranchId, lItem), gPMConstants.PMEDataType.PMLong))

                        ' item is from database therefore it is live...
                        oMaintainData.ItemIsLive = True

                        ' copies the current functional values to the original properties
                        ' so comparisons can determine if any values have been updated
                        m_lReturn = oMaintainData.CopyToOriginalData()

                        ' add item to maintain data collection
                        m_colMaintainItems.Add(oMaintainData, CStr(oMaintainData.Id))

                        ' get the highest step order of the existing items
                        If oMaintainData.StepOrder > m_lStepOrderCounter Then
                            m_lStepOrderCounter = oMaintainData.StepOrder
                        End If

                        ' destroy object
                        oMaintainData = Nothing

                    Next lItem

                End If

                ' now we have populates the collection]
                ' get the new base id for all new items

                'm_lReturn = SetGeneratedBaseId
                m_lMaintainItemIdCounter = -100


            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function



    ' ***************************************************************** '
    ' Name: ConvertFromNullValues
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-06-2003 : 223
    ' ***************************************************************** '
    Private Function ConvertFromNullValues(ByRef r_vValue As Object, ByVal v_iDataType As Integer) As Object

        Dim result As Object = Nothing
        Const sFunctionName As String = "ConvertFromNullValues"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            Select Case v_iDataType
                Case gPMConstants.PMEDataType.PMLong, gPMConstants.PMEDataType.PMCurrency, gPMConstants.PMEDataType.PMBoolean, gPMConstants.PMEDataType.PMDate



                    If r_vValue Is DBNull.Value Or CStr(r_vValue) = "" Then
                        Return 0
                    Else
                        Return r_vValue
                    End If

                Case Else
                    Return r_vValue

            End Select

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        '******************************
        ' Log Error.
        gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
        '*******************************

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: PopulateListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-06-2003 : Workflow
    ' ***************************************************************** '
    Private Function PopulateListView() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "PopulateListView"

        Dim oMaintainItem As MaintainData
        Dim lstItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Lock the listview so no updates are shown
            'TODO Check at runtime
            'm_lReturn = ListViewBatchStart(lvwList:=lvwTable)


            ' resize the columns to only display right hand side scroll bar
            If m_colMaintainItems.Count > 14 Then
                lvwTable.Columns.Item(ACListViewColIndexDescription - 1).Width = CInt(VB6.TwipsToPixelsX(3340))
            Else
                lvwTable.Columns.Item(ACListViewColIndexDescription - 1).Width = CInt(VB6.TwipsToPixelsX(3550))
            End If

            ' if we have a valid collection
            If Not (m_colMaintainItems Is Nothing) Then

                ' remove all existing list items
                lvwTable.Items.Clear()

                ' for each item in the collection
                For lItem As Integer = 1 To m_colMaintainItems.Count

                    ' get the next item
                    oMaintainItem = m_colMaintainItems.Item(lItem)

                    ' create a new list view item

                    'TODO Check at runtime
                    lstItem = lvwTable.Items.Add("")

                    ' populate item details
                    lstItem.Text = oMaintainItem.Code

                    'TODO Check at runtime
                    'lstItem.Icon = "find"

                    'TODO Check at runtime
                    'lstItem.SmallIcon = "find"
                    lstItem.Tag = CStr(oMaintainItem.Id)

                    ListViewHelper.GetListViewSubItem(lstItem, ACListViewSubItemIndexDescription).Text = oMaintainItem.description
                    ListViewHelper.GetListViewSubItem(lstItem, ACListViewSubItemIndexEffectiveDate).Text = DateTimeHelper.ToString(oMaintainItem.EffectiveDate)
                    ListViewHelper.GetListViewSubItem(lstItem, ACListViewSubItemIndexStepOrder).Text = CStr(oMaintainItem.StepOrder)

                    ' ghost any deleted items

                    'TODO Check at runtime
                    'lstItem.Ghosted = oMaintainItem.IsDeleted

                    ' populates the code array

                    m_lReturn = PopulateArray(r_vArray:=m_vCodes, v_sItemValue:=oMaintainItem.Code)

                Next lItem

            End If

            ' reselect row


            'TODO Check at runtime
            'lvwTable.FocusedItem = lvwTable.FindItem(CStr(m_lSelectedItem), lvwTag)

            ' sort the list view on the step order
            m_lReturn = SortListView(lvwTable, lvwTable.Columns.Item(ACListViewColIndexStepOrder - 1), SortOrder.Ascending)

            ' Show all updates to list view
            'TODO Check at runtime
            'm_lReturn = ListViewBatchEnd()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function



    ' ***************************************************************** '
    ' Name: SetupButtons
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-06-2003 : Workflow
    ' ***************************************************************** '
    Private Function SetupButtons() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SetupButtons"

        Dim oMaintainData As MaintainData

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lSelectedItem <> 0 Then

                If LoadSelectedItem(r_oMaintainData:=oMaintainData) = gPMConstants.PMEReturnCode.PMTrue Then

                    ' button caption changes dependant on item isdeleted status
                    If oMaintainData.IsDeleted Then
                        cmdDelete.Text = GetResString(ACResDataInterfaceButtonUndelete)
                    Else
                        cmdDelete.Text = GetResString(ACResDataInterfaceButtonDelete)
                    End If

                End If

                ' only enabled when an item is selected
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
                cmdView.Enabled = True

                ' you can only move when there is somewhere to move to
                cmdMoveUp.Enabled = Not (oMaintainData.StepOrder = 1)

                ' you can only move when there is somewhere to move to
                cmdMoveDown.Enabled = Not (oMaintainData.StepOrder = m_lStepOrderCounter)

                lblMove.Enabled = cmdMoveDown.Enabled Or cmdMoveUp.Enabled

            Else
                ' only enabled when an item is selected
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
                cmdView.Enabled = False
                cmdMoveDown.Enabled = False
                cmdMoveUp.Enabled = False
            End If

            ' always enabled
            cmdAdd.Enabled = True
            cmdOK.Enabled = True
            cmdCancel.Enabled = True

            ' TODO
            'cmdApply.Enabled = False

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: LoadSelectedItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-06-2003 : WorkFlow.
    ' ***************************************************************** '
    Private Function LoadSelectedItem(ByRef r_oMaintainData As MaintainData) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "LoadSelectedItem"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lSelectedItem <> 0 Then
                ' get selected item
                r_oMaintainData = m_colMaintainItems.Item(CStr(m_lSelectedItem))
            Else
                result = gPMConstants.PMEReturnCode.PMFalse

                ' log error
                LogMsg(v_sMsg:="Failed to load selected item " & m_lSelectedItem, v_sMethod:=sFunctionName)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    Private Sub lvwTable_ItemClick(ByVal Item As ListViewItem)

        If m_lSelectedItem <> Convert.ToString(lvwTable.FocusedItem.Tag) Then

            ' set the selected debt item to the one just selected

            m_lSelectedItem = Convert.ToString(lvwTable.FocusedItem.Tag)

        End If
    End Sub


    ' ***************************************************************** '
    ' Name: ActionAdd
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-06-2003 : workflow
    ' ***************************************************************** '
    Private Function ActionAdd() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ActionAdd"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' create instance of details form and load it
            If LoadDetailsForm(v_lActionType:=ACActionAdd) = gPMConstants.PMEReturnCode.PMTrue Then

                ' display the details form
                If ShowDetailsForm() = gPMConstants.PMEReturnCode.PMTrue Then

                    ' save any data additions / changes to the database
                    m_lReturn = SaveDetailsData()

                End If

            End If

            ' destroy instance of details form
            m_lReturn = UnloadDetailsForm()





            ' destroy form instance
            m_frmDetails = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ActionEdit
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ActionEdit() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ActionEdit"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' create instance of details form and load it
            If LoadDetailsForm(v_lActionType:=ACActionEdit) = gPMConstants.PMEReturnCode.PMTrue Then

                ' display the details form
                If ShowDetailsForm() = gPMConstants.PMEReturnCode.PMTrue Then

                    ' save any data additions / changes to the database
                    m_lReturn = SaveDetailsData()

                End If

            End If

            ' destroy instance of details form
            m_lReturn = UnloadDetailsForm()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ActionDelete
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function ActionDelete() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ActionDelete"

        Dim oMaintainItem As MaintainData
        Dim sMessage, sSteps As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' load the current selected item
            If LoadSelectedItem(r_oMaintainData:=oMaintainItem) = gPMConstants.PMEReturnCode.PMTrue Then

                If Not IsStepUsed(oMaintainItem.Id, sSteps) Then

                    ' reset the current items deleted status
                    oMaintainItem.IsDeleted = Not oMaintainItem.IsDeleted

                    ' redisplay list items
                    PopulateListView()

                    ' set new buttons state
                    SetupButtons()

                Else
                    sMessage = GetResString(ACMessageDeleteStepNotAllowed)
                    MessageBox.Show(sMessage & " " & sSteps, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If



            ' destroy object reference
            oMaintainItem = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: ActionOK
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Private Function ActionOK() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ActionOK"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If ProcessUpdates() <> gPMConstants.PMEReturnCode.PMTrue Then

                LogMsg(v_sMsg:="Failed to update / add item to database", v_sMethod:=sFunctionName)

            Else

                Me.Close()

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: ActionCancel
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ActionCancel) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ActionCancel() As Integer
    '
    'Dim result As Integer = 0
    'Const sFunctionName As String = "ActionCancel"
    '
    'Try 
    '
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    '******************************
    ' Log Error.
    'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
    '*******************************
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: LoadDetailsForm
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-06-2003 : workflow
    ' ***************************************************************** '
    Private Function LoadDetailsForm(ByVal v_lActionType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "LoadDetailsForm"

        Dim oMaintainData As MaintainData
        Dim bFailedToLoadSelectedItem As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bFailedToLoadSelectedItem = False

            ' set up maintain data object
            If v_lActionType = ACActionAdd Then
                m_lReturn = BuildWorkflowStepArray(0)
                ' create new instance of maintain data object
                oMaintainData = New MaintainData()

            ElseIf v_lActionType = ACActionEdit Or v_lActionType = ACActionView Then

                ' load selected item
                If LoadSelectedItem(r_oMaintainData:=oMaintainData) <> gPMConstants.PMEReturnCode.PMTrue Then
                    bFailedToLoadSelectedItem = True
                End If
                m_lReturn = BuildWorkflowStepArray(m_lSelectedItem)
            End If

            ' if we have successfully retrieved selected item
            If Not bFailedToLoadSelectedItem Then

                ' create new instance of frm details screen
                m_frmDetails = New frmDetails()

                ' set properties
                With m_frmDetails
                    ' selected maintain data item
                    .MaintainData = oMaintainData

                    .ActionType = v_lActionType


                    .Codes = m_vCodes

                    ' data arrays
                    .LookupDetails = VB6.CopyArray(m_vLookupDetails)
                    .LookupTables = VB6.CopyArray(m_vLookupTables)


                    .PMUsers = m_vPMUsers
                    .TaskGroupTask = VB6.CopyArray(m_vTaskGroupTask)
                    .TaskGroupTaskAction = VB6.CopyArray(m_vTaskGroupTaskAction)
                    .PMUserGroupUsers = VB6.CopyArray(m_vPMUserGroupUsers)
                    .TaskGroupUserGroups = VB6.CopyArray(m_vTaskGroupUserGroups)


                    .TaskEvent = m_vTaskEvent
                    .WorkflowSteps = VB6.CopyArray(m_vWorkflowSteps)
                    .ValidUserBranches = VB6.CopyArray(m_vValidUserBranches)

                End With

                ' load

                'Developer Guide no.
                'Load(m_frmDetails)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    ' Name: ShowDetailsForm
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-06-2003 : 223
    ' ***************************************************************** '
    Private Function ShowDetailsForm() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ShowDetailsForm"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display the interface.
            m_frmDetails.ShowDialog()

            If m_frmDetails.Error_Renamed Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UnloadDetailsForm
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-06-2003 : 223
    ' ***************************************************************** '
    Private Function UnloadDetailsForm() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UnloadDetailsForm"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Unload and destroy the instance of the form
            m_frmDetails.Close()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************


        Finally
            ' destroy form reference
            m_frmDetails = Nothing
        End Try



        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SaveEditData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-06-2003 : 223
    ' ***************************************************************** '
    Private Function SaveDetailsData() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SaveDetailsData"

        Dim oMaintainItem, oExistingItem As MaintainData

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case m_frmDetails.ReturnType
                Case ACReturnOk

                    ' get item back from form details
                    oMaintainItem = m_frmDetails.MaintainData

                    If m_frmDetails.ActionType = ACActionAdd Then

                        ' get temporary item id
                        ' increment counter so we get a new id
                        m_lMaintainItemIdCounter -= 1
                        m_lStepOrderCounter += 1


                        oMaintainItem.Id = m_lMaintainItemIdCounter
                        oMaintainItem.StepOrder = m_lStepOrderCounter
                        oMaintainItem.WorkflowId = m_lWorkflowId

                        ' it is a new item so add it to the collection
                        m_colMaintainItems.Add(oMaintainItem, CStr(oMaintainItem.Id))

                        ' select the item we have just added
                        m_lSelectedItem = oMaintainItem.Id



                    ElseIf m_frmDetails.ActionType = ACActionEdit Then

                        ' Get existing item
                        oExistingItem = m_colMaintainItems.Item(CStr(oMaintainItem.Id))

                        ' copy back data from details
                        If oExistingItem.Copy(v_oMaintainData:=oMaintainItem) <> gPMConstants.PMEReturnCode.PMTrue Then

                            ' Log Error
                            result = gPMConstants.PMEReturnCode.PMFalse

                            LogMsg(v_sMsg:="Failed to copy data back from form details", v_sMethod:=sFunctionName)

                        End If

                        m_lSelectedItem = oMaintainItem.Id

                    ElseIf m_frmDetails.ActionType = ACActionView Then
                        m_lSelectedItem = oMaintainItem.Id

                    End If

                    If Not cmdApply.Enabled Then
                        ' enable the apply button if we have been to the details screen
                        cmdApply.Enabled = oMaintainItem.ItemUpdated
                    End If

                Case ACReturnCancel
                    ' do nothing

            End Select

            ' update list view display
            m_lReturn = SetupButtons()
            m_lReturn = PopulateListView()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    '' ***************************************************************** '
    '' Name: SetGeneratedBaseId
    ''
    '' Parameters: n/a
    ''
    '' Description:
    ''
    '' History:
    ''           Created : MEvans : 11-06-2003 : 223
    '' ***************************************************************** '
    'Private Function SetGeneratedBaseId() As Long
    '
    '    Const sFunctionName = "SetGeneratedBaseId"
    '
    '    Dim lItem As Long
    '    Dim lHighestId As Long
    '
    '    On Error GoTo Err_SetGeneratedBaseId
    '
    '    SetGeneratedBaseId = PMTrue
    '
    '    ' if generated id is set
    '    ' at a position higher than all the existing entries we will
    '    ' have a unique id. (at least within this process)
    '
    '    For lItem = 1 To m_colMaintainItems.Count
    '        If m_colMaintainItems(lItem).Id > lHighestId Then
    '            lHighestId = m_colMaintainItems(lItem).Id
    '        End If
    '    Next
    '
    '    m_lMaintainItemIdCounter = lHighestId + 1
    '
    '    Exit Function
    '
    'Err_SetGeneratedBaseId:
    '
    '    SetGeneratedBaseId = PMError
    '
    '    '******************************
    '    ' Log Error.
    '    LogMessageToFile _
    ''        sUsername:=g_oObjectManager.UserName, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:=sFunctionName & " Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:=sFunctionName, _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.description
    '    '*******************************
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    ' Name: ActionView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-06-2003 : Workflow
    ' ***************************************************************** '
    Private Function ActionView() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ActionView"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' create instance of details form and load it
            If LoadDetailsForm(v_lActionType:=ACActionView) = gPMConstants.PMEReturnCode.PMTrue Then

                ' display the details form
                If ShowDetailsForm() = gPMConstants.PMEReturnCode.PMTrue Then

                    ' save any data additions / changes to the database
                    m_lReturn = SaveDetailsData()

                End If

            End If

            ' destroy instance of details form
            m_lReturn = UnloadDetailsForm()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: PopulateArray
    '
    ' Parameters: n/a
    '
    ' Description: Populates the array if the specified item doesnt
    '                 already exist
    ' History:
    '           Created : MEvans : 03-06-2003 : 223
    ' ***************************************************************** '
    Private Function PopulateArray(ByRef r_vArray() As Object, ByVal v_sItemValue As String) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "PopulateArray"

        Dim llBound, lUBound, lItem As Integer
        Dim bItemExists As Boolean
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' create new collection
            If Not Information.IsArray(r_vArray) Then
                ReDim r_vArray(0)
            End If

            ' get array boundaries
            llBound = r_vArray.GetLowerBound(0)
            lUBound = r_vArray.GetUpperBound(0)

            ' for each item in the array
            For lItem = llBound To lUBound

                ' check if we match the values from the array specified

                If CStr(r_vArray(lItem)).Trim() = v_sItemValue.Trim() Then
                    ' indicate item already exists
                    bItemExists = True

                    ' quit loop
                    lItem = lUBound
                End If

            Next lItem

            ' if the item doesnt already exist
            ' add it to the array
            If Not bItemExists Then
                lItem = lUBound + 1
                ReDim Preserve r_vArray(lItem)

                r_vArray(lItem) = v_sItemValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessUpdates
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Private Function ProcessUpdates() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ProcessUpdates"

        Dim bTransactionError, bTransactionOpen As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if we have any items
            If Not (m_colMaintainItems Is Nothing) Then


                If m_oBusiness.BeginTrans = gPMConstants.PMEReturnCode.PMTrue Then

                    bTransactionOpen = True

                    If PeformAdds(r_bTransactionError:=bTransactionError) <> gPMConstants.PMEReturnCode.PMTrue Then
                        bTransactionError = True
                    End If

                    ' do this one more time to pick
                    ' up any records who now need to be updated
                    ' because of the new keys that have been added for new items
                    If Not bTransactionError Then

                        If PeformUpdates(r_bTransactionError:=bTransactionError) <> gPMConstants.PMEReturnCode.PMTrue Then
                            bTransactionError = True
                        End If

                    End If

                    If bTransactionError Then

                        If m_oBusiness.RollbackTrans <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' log error
                            result = gPMConstants.PMEReturnCode.PMFalse

                            LogMsg(v_sMsg:="Failed to rollback transaction", v_sMethod:=sFunctionName)

                        Else
                            bTransactionOpen = False
                        End If
                    Else

                        If m_oBusiness.CommitTrans <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' log error
                            result = gPMConstants.PMEReturnCode.PMFalse

                            LogMsg(v_sMsg:="Failed to commit transaction", v_sMethod:=sFunctionName)
                        Else
                            bTransactionOpen = False
                        End If
                    End If
                Else
                    ' log error
                    result = gPMConstants.PMEReturnCode.PMFalse

                    LogMsg(v_sMsg:="Failed to begin transaction", v_sMethod:=sFunctionName)

                End If
            End If



            If bTransactionOpen Then

                If m_oBusiness.RollbackTrans <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' log error
                    result = gPMConstants.PMEReturnCode.PMFalse

                    LogMsg(v_sMsg:="Failed to rollback transaction", v_sMethod:=sFunctionName)

                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name:ActionApply
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Private Function ActionApply() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ActionApply"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If ProcessUpdates() <> gPMConstants.PMEReturnCode.PMTrue Then

                LogMsg(v_sMsg:="Failed to update / add item to database", v_sMethod:=sFunctionName)

            Else
                GetMaintainData()
                PopulateListView()
                SetupButtons()

                cmdApply.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookups
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-06-2003 : 223
    ' ***************************************************************** '
    Private Function GetLookups() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetLookups"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim m_vLookupTables(3, 6)

            m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = ACLookupTablePMWrkTaskGroup
            m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = ACLookupTablePMWrkTask
            m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = ACLookupTablePMUserGroup
            m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 3) = ACLookupTableEventType
            m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 4) = ACLookupTableEventLogSubject
            m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 5) = ACLookupTablePMWrkTaskActionType
            m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 6) = ACLookupTableTaskOutcome


            If m_oEventTaskBusiness.GetLookupValues(r_vTableArray:=m_vLookupTables, r_vResultArray:=m_vLookupDetails) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPMUserGroupUsers
    '
    ' Parameters: n/a
    '
    ' Description: Returns all entries from PMUser_Group_User
    '
    ' History:
    '           Created : MEvans : 01-07-2003: workflow
    ' ***************************************************************** '
    Private Function GetPMUserGroupUsers() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetPMUserGroupUsers"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_oEventTaskBusiness.GetPMUserGroupUsers(r_vResults:=m_vPMUserGroupUsers) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetTaskActionTypeOutcomes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Private Function GetTaskActionTypeOutcomes() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskActionTypeOutcomes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_oEventTaskBusiness.GetTaskActionTypeOutcomes(r_vResults:=m_vTaskActionTypeOutcomes) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskGroupTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Private Function GetTaskGroupTask() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskGroupTask"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If m_oEventTaskBusiness.GetTaskGroupTask(r_vResults:=m_vTaskGroupTask) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskGroupTaskAction
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Private Function GetTaskGroupTaskAction() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskGroupTaskAction"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_oEventTaskBusiness.GetTaskGroupTaskAction(r_vResults:=m_vTaskGroupTaskAction) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskGroupUserGroups
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Private Function GetTaskGroupUserGroups() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskGroupUserGroups"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_oEventTaskBusiness.GetTaskGroupUserGroups(r_vResults:=m_vTaskGroupUserGroups) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPMUsers
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Private Function GetPMUsers() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetPMUsers"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_oEventTaskBusiness.GetPMUser(r_vResults:=m_vPMUsers) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Interface
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Private Function GetData() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetData"

        Dim bError As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bError = False

            ' Lookups
            If Not bError Then
                If GetLookups() <> gPMConstants.PMEReturnCode.PMTrue Then
                    bError = True
                End If
            End If

            ' PMUsers
            If Not bError Then
                If GetPMUsers() <> gPMConstants.PMEReturnCode.PMTrue Then
                    bError = True
                End If
            End If

            ' Task Group Tasks
            If Not bError Then
                If GetTaskGroupTask() <> gPMConstants.PMEReturnCode.PMTrue Then
                    bError = True
                End If
            End If

            ' Task Group Task Actions
            If Not bError Then
                If GetTaskGroupTaskAction() <> gPMConstants.PMEReturnCode.PMTrue Then
                    bError = True
                End If
            End If

            ' PMUser Group Users
            If Not bError Then
                If GetPMUserGroupUsers() <> gPMConstants.PMEReturnCode.PMTrue Then
                    bError = True
                End If
            End If

            ' Task Group User Groups
            If Not bError Then
                If GetTaskGroupUserGroups() <> gPMConstants.PMEReturnCode.PMTrue Then
                    bError = True
                End If
            End If

            ' Task Action Type Outcomes
            If Not bError Then
                If GetTaskActionTypeOutcomes() <> gPMConstants.PMEReturnCode.PMTrue Then
                    bError = True
                End If
            End If

            If Not bError Then
                If GetValidUserBranches() <> gPMConstants.PMEReturnCode.PMTrue Then
                    bError = True
                End If
            End If

            If bError Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BuildWorkflowStepArray
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function BuildWorkflowStepArray(ByVal v_lSelectedItem As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "BuildWorkflowStepArray"

        Dim lArrayPos As Integer
        Dim oMaintainDataItem As MaintainData

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' resize the array
            ReDim m_vWorkflowSteps(2, m_colMaintainItems.Count)

            ' add an initial item as initially there arent any
            m_vWorkflowSteps(ACDetailKey, 0) = 0
            m_vWorkflowSteps(ACDetailDesc, 0) = ACWorkflowStepSelectNone
            m_vWorkflowSteps(ACDetailCode, 0) = "NONE"

            lArrayPos = 1

            ' for each step in the collection
            For lItem As Integer = 1 To m_colMaintainItems.Count

                ' get the step
                oMaintainDataItem = m_colMaintainItems(lItem)

                ' dont want the user selecting deleted items
                ' or if there editing the currently selected item
                If Not oMaintainDataItem.IsDeleted And oMaintainDataItem.Id <> v_lSelectedItem Then

                    ' add the steps details to the array
                    m_vWorkflowSteps(ACDetailKey, lArrayPos) = oMaintainDataItem.Id
                    m_vWorkflowSteps(ACDetailDesc, lArrayPos) = oMaintainDataItem.description
                    m_vWorkflowSteps(ACDetailCode, lArrayPos) = oMaintainDataItem.Code

                    lArrayPos += 1

                End If

            Next lItem

            ' resize the array
            ReDim Preserve m_vWorkflowSteps(2, lArrayPos - 1)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: IsStepUsed
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function IsStepUsed(ByVal v_lStepId As Integer, ByRef sSteps As String) As Boolean

        Dim result As Boolean = False
        Const sFunctionName As String = "IsStepUsed"

        Dim bReturn As Boolean
        Dim oMaintainDataItem As MaintainData

        Try

            bReturn = False

            ' for each step item
            For lItem As Integer = 1 To m_colMaintainItems.Count

                ' get item
                oMaintainDataItem = m_colMaintainItems.Item(lItem)

                ' check if the step is being used
                If oMaintainDataItem.CompleteNextWorkflowStepId = v_lStepId Or oMaintainDataItem.OverdueNextWorkflowStepId = v_lStepId Then

                    ' build message to indicate to the user which steps
                    ' are using the selected step
                    sSteps = sSteps & oMaintainDataItem.description & ","

                    bReturn = True

                End If

            Next lItem

            ' remove trailing comma
            If sSteps <> "" Then
                sSteps = Mid(sSteps, 1, sSteps.Length - 1)
            End If


            Return bReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ActionMoveStepUp
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function ActionMoveStepUp() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ActionMoveStepUp"

        Dim oMaintainItem, oSelectedMaintainItem As MaintainData
        Dim lStepOrder As Integer
        Dim bFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If LoadSelectedItem(r_oMaintainData:=oSelectedMaintainItem) = gPMConstants.PMEReturnCode.PMTrue Then

                lStepOrder = oSelectedMaintainItem.StepOrder

                For lItem As Integer = 1 To m_colMaintainItems.Count

                    oMaintainItem = m_colMaintainItems.Item(lItem)

                    If oMaintainItem.StepOrder = lStepOrder - 1 Then
                        oMaintainItem.StepOrder = lStepOrder
                        bFound = True
                        Exit For
                    End If

                Next lItem

                If bFound Then

                    oSelectedMaintainItem.StepOrder = lStepOrder - 1
                    cmdApply.Enabled = True
                End If



            End If

            PopulateListView()

            SetupButtons()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ActionMoveStepDown
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function ActionMoveStepDown() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ActionMoveStepDown"

        Dim oMaintainItem, oSelectedMaintainItem As MaintainData
        Dim lStepOrder As Integer
        Dim bFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If LoadSelectedItem(r_oMaintainData:=oSelectedMaintainItem) = gPMConstants.PMEReturnCode.PMTrue Then

                lStepOrder = oSelectedMaintainItem.StepOrder

                For lItem As Integer = 1 To m_colMaintainItems.Count

                    oMaintainItem = m_colMaintainItems.Item(lItem)

                    If oMaintainItem.StepOrder = lStepOrder + 1 Then
                        oMaintainItem.StepOrder = lStepOrder
                        bFound = True
                        Exit For
                    End If

                Next lItem

                If bFound Then

                    oSelectedMaintainItem.StepOrder = lStepOrder + 1
                    cmdApply.Enabled = True
                End If

            End If

            PopulateListView()

            SetupButtons()


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateAssociatedSteps
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function UpdateAssociatedSteps(ByVal v_lCurrentId As Integer, ByVal v_lNextId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UpdateAssociatedSteps"

        Dim oMaintainItem As MaintainData

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For lItem As Integer = 1 To m_colMaintainItems.Count

                oMaintainItem = m_colMaintainItems.Item(lItem)

                If oMaintainItem.CompleteNextWorkflowStepId = v_lCurrentId Then
                    oMaintainItem.CompleteNextWorkflowStepId = v_lNextId
                End If

                If oMaintainItem.OverdueNextWorkflowStepId = v_lCurrentId Then
                    oMaintainItem.OverdueNextWorkflowStepId = v_lNextId
                End If

            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PeformUpdates
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function PeformUpdates(ByRef r_bTransactionError As Boolean) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "PeformUpdates"

        Dim nItems As Integer
        Dim oMaintainItem As MaintainData

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the item count
            nItems = m_colMaintainItems.Count

            ' for each item
            For lItem As Integer = 1 To nItems

                ' get maintain data item from collection
                oMaintainItem = m_colMaintainItems.Item(lItem)

                ' if already in the database
                If oMaintainItem.ItemIsLive Then

                    ' and something has changes
                    If oMaintainItem.ItemUpdated Then

                        ' update item changes to database

                        If m_oBusiness.UpdatePackageStep(v_lWorkflowStepId:=oMaintainItem.Id, v_lWorkflowId:=oMaintainItem.WorkflowId, v_lStepOrder:=oMaintainItem.StepOrder, v_sStepCode:=oMaintainItem.Code, v_sStepDescription:=oMaintainItem.description, v_dtEffectiveDate:=oMaintainItem.EffectiveDate, v_bIsDeleted:=oMaintainItem.IsDeleted, v_lTaskGroupId:=oMaintainItem.TaskGroupId, v_lTaskId:=oMaintainItem.TaskId, v_lTaskACtionTypeId:=oMaintainItem.TaskActionTypeid, v_lPMUserGroupId:=oMaintainItem.PMUserGroupId, v_lUserId:=oMaintainItem.UserId, v_lStepDaysDuration:=oMaintainItem.StepDaysDuration, v_lCompleteNextWorkflowStepId:=oMaintainItem.CompleteNextWorkflowStepId, v_lOverduenextWorkflowStepId:=oMaintainItem.OverdueNextWorkflowStepId, v_bExecuatableTask:=oMaintainItem.ExecutableTask, v_lEventTypeId:=oMaintainItem.EventTypeId, v_lEventLogSubjectId:=oMaintainItem.EventLogSubjectId, v_sEventDescription:=oMaintainItem.EventDescription, v_sTaskDescription:=oMaintainItem.TaskDescription, v_bIsUrgent:=oMaintainItem.IsUrgent, v_sCustomer:=oMaintainItem.Customer, v_sWorkflow:=oMaintainItem.Workflow, v_lBranchId:=oMaintainItem.BranchId) <> gPMConstants.PMEReturnCode.PMTrue Then

                            r_bTransactionError = True

                        Else

                            ' reset the update indicator for a newly
                            ' added item
                            oMaintainItem.UpdateThisItem = False

                            ' copy all data back to original properties so
                            ' we dont updates this item again if this is only
                            ' the apply
                            m_lReturn = oMaintainItem.CopyToOriginalData()

                        End If

                    End If

                Else
                    '
                    '            ' add item to database
                    '            If m_oBusiness.AddPackageStep( _
                    ''                                    r_lWorkflowStepId:=oMaintainItem.Id, _
                    ''                                    v_lWorkflowId:=oMaintainItem.WorkflowId, _
                    ''                                    v_lStepOrder:=oMaintainItem.StepOrder, _
                    ''                                    v_sStepCode:=oMaintainItem.Code, _
                    ''                                    v_sStepDescription:=oMaintainItem.description, _
                    ''                                    v_dtEffectiveDate:=oMaintainItem.EffectiveDate, _
                    ''                                    v_bIsDeleted:=oMaintainItem.IsDeleted, _
                    ''                                    v_lTaskGroupId:=oMaintainItem.TaskGroupId, _
                    ''                                    v_lTaskId:=oMaintainItem.TaskId, _
                    ''                                    v_lTaskACtionTypeId:=oMaintainItem.TaskActionTypeid, _
                    ''                                    v_lPMUserGroupId:=oMaintainItem.PMUserGroupId, _
                    ''                                    v_lUserId:=oMaintainItem.UserId, _
                    ''                                    v_lStepDaysDuration:=oMaintainItem.StepDaysDuration, _
                    ''                                    v_lCompleteNextWorkflowStepId:=oMaintainItem.CompleteNextWorkflowStepId, _
                    ''                                    v_lOverduenextWorkflowStepId:=oMaintainItem.OverdueNextWorkflowStepId, _
                    ''                                    v_bExecuatableTask:=oMaintainItem.ExecutableTask, _
                    ''                                    v_lEventTypeId:=oMaintainItem.EventTypeId, _
                    ''                                    v_lEventLogSubjectId:=oMaintainItem.EventLogSubjectId, _
                    ''                                    v_sEventDescription:=oMaintainItem.EventDescription _
                    ''                                ) <> PMTrue Then
                    '
                    '                r_bTransactionError = True
                    '
                    '            Else
                    '                lTempId = oMaintainItem.Id
                    '                oMaintainItem.Id = lMaintainItemId
                    '
                    '                ' update any existing dependant items
                    '                If AdditionalUpdateRequired(lTempId, lMaintainItemId) Then
                    '                    r_bProcessUpdatesAgain = True
                    '                End If
                    '
                    '                ' reset live indicator in case this is only an apply
                    '                ' so we dont attempt to add this item again
                    '                oMaintainItem.ItemIsLive = True
                    '            End If

                End If

                If r_bTransactionError Then
                    Exit For
                End If

            Next lItem

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PeformAdds
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function PeformAdds(ByRef r_bTransactionError As Boolean) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "PeformAdds"

        Dim nItems As Integer
        Dim oMaintainItem As MaintainData
        Dim lTempId, lMaintainItemId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the item count
            nItems = m_colMaintainItems.Count

            ' for each item
            For lItem As Integer = 1 To nItems

                ' get maintain data item from collection
                oMaintainItem = m_colMaintainItems.Item(lItem)

                ' if already in the database
                If oMaintainItem.ItemIsLive Then

                    ' and something has changes
                    '            If oMaintainItem.ItemUpdated Then
                    '
                    '                ' update item changes to database
                    '                If m_oBusiness.UpdatePackageStep( _
                    ''                                    v_lWorkflowStepId:=oMaintainItem.Id, _
                    ''                                    v_lWorkflowId:=oMaintainItem.WorkflowId, _
                    ''                                    v_lStepOrder:=oMaintainItem.StepOrder, _
                    ''                                    v_sStepCode:=oMaintainItem.Code, _
                    ''                                    v_sStepDescription:=oMaintainItem.description, _
                    ''                                    v_dtEffectiveDate:=oMaintainItem.EffectiveDate, _
                    ''                                    v_bIsDeleted:=oMaintainItem.IsDeleted, _
                    ''                                    v_lTaskGroupId:=oMaintainItem.TaskGroupId, _
                    ''                                    v_lTaskId:=oMaintainItem.TaskId, _
                    ''                                    v_lTaskACtionTypeId:=oMaintainItem.TaskActionTypeid, _
                    ''                                    v_lPMUserGroupId:=oMaintainItem.PMUserGroupId, _
                    ''                                    v_lUserId:=oMaintainItem.UserId, _
                    ''                                    v_lStepDaysDuration:=oMaintainItem.StepDaysDuration, _
                    ''                                    v_lCompleteNextWorkflowStepId:=oMaintainItem.CompleteNextWorkflowStepId, _
                    ''                                    v_lOverduenextWorkflowStepId:=oMaintainItem.OverdueNextWorkflowStepId, _
                    ''                                    v_bExecuatableTask:=oMaintainItem.ExecutableTask, _
                    ''                                    v_lEventTypeId:=oMaintainItem.EventTypeId, _
                    ''                                    v_lEventLogSubjectId:=oMaintainItem.EventLogSubjectId, _
                    ''                                    v_sEventDescription:=oMaintainItem.EventDescription _
                    ''                                        ) <> PMTrue Then
                    '
                    '                    r_bTransactionError = True
                    '
                    '                Else
                    '
                    '                    ' copy all data back to original properties so
                    '                    ' we dont updates this item again if this is only
                    '                    ' the apply
                    '                    m_lReturn = oMaintainItem.CopyToOriginalData
                    '
                    '                End If
                    '
                    '            End If

                Else

                    ' add item to database

                    If m_oBusiness.AddPackageStep(r_lWorkflowStepId:=lMaintainItemId, v_lWorkflowId:=oMaintainItem.WorkflowId, v_lStepOrder:=oMaintainItem.StepOrder, v_sStepCode:=oMaintainItem.Code, v_sStepDescription:=oMaintainItem.description, v_dtEffectiveDate:=oMaintainItem.EffectiveDate, v_bIsDeleted:=oMaintainItem.IsDeleted, v_lTaskGroupId:=oMaintainItem.TaskGroupId, v_lTaskId:=oMaintainItem.TaskId, v_lTaskACtionTypeId:=oMaintainItem.TaskActionTypeid, v_lPMUserGroupId:=oMaintainItem.PMUserGroupId, v_lUserId:=oMaintainItem.UserId, v_lStepDaysDuration:=oMaintainItem.StepDaysDuration, v_lCompleteNextWorkflowStepId:=0, v_lOverduenextWorkflowStepId:=0, v_bExecuatableTask:=oMaintainItem.ExecutableTask, v_lEventTypeId:=oMaintainItem.EventTypeId, v_lEventLogSubjectId:=oMaintainItem.EventLogSubjectId, v_sEventDescription:=oMaintainItem.EventDescription, v_sTaskDescription:=oMaintainItem.TaskDescription, v_bIsUrgent:=oMaintainItem.IsUrgent, v_sCustomer:=oMaintainItem.Customer, v_sWorkflow:=oMaintainItem.Workflow, v_lBranchId:=oMaintainItem.BranchId) <> gPMConstants.PMEReturnCode.PMTrue Then

                        r_bTransactionError = True

                    Else

                        lTempId = oMaintainItem.Id
                        'oMaintainItem.ReplacedItemId = oMaintainItem.Id
                        oMaintainItem.Id = lMaintainItemId

                        ' update the selected item with the new key
                        If m_lSelectedItem = lTempId Then
                            m_lSelectedItem = lMaintainItemId
                        End If

                        ' update any existing dependant items
                        If UpdateAssociatedSteps(lTempId, lMaintainItemId) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_bTransactionError = True
                        End If

                        ' ensure that if this item references any other
                        ' we update it after all the items have been added
                        ' so we apply the correct keys for newly added steps.
                        If oMaintainItem.CompleteNextWorkflowStepId <> 0 Or oMaintainItem.OverdueNextWorkflowStepId <> 0 Then

                            oMaintainItem.UpdateThisItem = True
                        End If

                        ' reset live indicator in case this is only an apply
                        ' so we dont attempt to add this item again
                        oMaintainItem.ItemIsLive = True


                    End If

                End If

                If r_bTransactionError Then
                    Exit For
                End If

            Next lItem

            '    ' rebuild collection with correct key'd items
            '    For lItem = 1 To nItems
            '
            '        ' get maintain data item from collection
            '        Set oMaintainItem = m_colMaintainItems(lItem)
            '
            '        ' if this is a new item then
            '        If oMaintainItem.ReplacedItemId <> 0 Then
            '
            '            ' create a new item
            '            Set oNewMaintainItem = New MaintainData
            '            m_lReturn = oNewMaintainItem.Copy(oMaintainItem)
            '
            '            ' add it to the collection
            '            m_colMaintainItems.Add oNewMaintainItem, CStr(oNewMaintainItem.Id)
            '
            '            ' remove the old item from the collection
            '            m_colMaintainItems.Remove CStr(oMaintainItem.ReplacedItemId)
            '        End If
            '
            '    Next lItem


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetValidUserBranches
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 20-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    Private Function GetValidUserBranches() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetValidUserBranches"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_oBusiness.GetValidUserBranches(r_vResults:=m_vValidUserBranches) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve the valid user branches", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function
End Class
