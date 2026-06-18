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

	Private m_oBusiness As bPMMaintainTask.Business
	Private m_lReturn As Integer
	Private m_bInterfaceError As Boolean
	'********************************
	
	Private m_lSelectedItem As Integer
	Private m_colMaintainItems As Collection
	Private m_frmDetails As frmDetails
	Private m_vTaskOutcomes As Object
	
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
	
	Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
		m_lReturn = ActionApply()
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lReturn = ActionCancel()
	End Sub
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		m_lReturn = ActionEdit()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lReturn = ActionOK()
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
			If g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMMaintainTask.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
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
			
			m_lReturn = GetTaskOutcomes()
			
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

			sReturn = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=v_lItemId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			
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
			
			cmdEdit.Text = GetResString(ACResDataInterfaceButtonEdit)
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
			lvwTable.Columns.Insert(ACListViewColIndexCompletionTask - 1, ACListViewColKeyCompletionTask, GetResString(ACResDataInterfaceListViewHeaderCompletionOutcome), 94)
			lvwTable.Columns.Insert(ACListViewColIndexIncompleteTask - 1, ACListViewColKeyIncompleteTask, GetResString(ACResDataInterfaceListViewHeaderIncompleteOutcome), 94)
			
			' Tag Column Headers to identify in column click so we can sort correctly
			lvwTable.Columns.Item(ACListViewColIndexCode - 1).Tag = ACListViewTagTypeString
			lvwTable.Columns.Item(ACListViewColIndexDescription - 1).Tag = ACListViewTagTypeString
			lvwTable.Columns.Item(ACListViewColIndexCompletionTask - 1).Tag = ACListViewTagTypeString
			lvwTable.Columns.Item(ACListViewColIndexIncompleteTask - 1).Tag = ACListViewTagTypeString
			
			' set column sizes
			lvwTable.Columns.Item(ACListViewColIndexCode - 1).Width = CInt(VB6.TwipsToPixelsX(1300))
			lvwTable.Columns.Item(ACListViewColIndexDescription - 1).Width = CInt(VB6.TwipsToPixelsX(2700))
			lvwTable.Columns.Item(ACListViewColIndexCompletionTask - 1).Width = CInt(VB6.TwipsToPixelsX(1700))
			lvwTable.Columns.Item(ACListViewColIndexIncompleteTask - 1).Width = CInt(VB6.TwipsToPixelsX(1700))
			
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
    Private Function SortListView(ByRef r_oListView As ListView, ByVal ColumnHeader As ColumnHeader) As Integer

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

                ' determine new sort order
                If ListViewHelper.GetSortOrderProperty(r_oListView) = SortOrder.Ascending Then
                    lSort = SortOrder.Descending
                Else
                    lSort = SortOrder.Ascending
                End If

                ' determine the column header to sort by
                lSourceCol = ColumnHeader.Index + 1 - 1

                ' If its the created date, then sort by that
                If sColHeaderTag = ACListViewTagTypeDate Then

                    '	m_lReturn = ListViewSortByDate(v_oListView:=r_oListView, v_iSourceColumn:=lSourceCol, v_iDirection:=lSort)
                    m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=r_oListView, v_iSourceColumn:=lSourceCol, v_iDirection:=lSort)

                ElseIf sColHeaderTag = ACListViewTagTypeNumber Then

                    'm_lReturn = ListViewSortByValue(v_oListView:=r_oListView, v_iSourceColumn:=lSourceCol, v_iDirection:=lSort)
                    m_lReturn = ListViewFunc.ListViewSortByValue(v_oListView:=r_oListView, v_iSourceColumn:=lSourceCol, v_iDirection:=lSort)
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
    ' Name: GetData
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
        Const sFunctionName As String = "GetData"

        Dim llbound, lUbound As Integer
        Dim vMaintainData As Object
        Dim oMaintainData As MaintainData

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' set up the collection
            m_colMaintainItems = New Collection()

            ' get data to maintain

            If m_oBusiness.GetMaintainData(r_vResults:=vMaintainData) = gPMConstants.PMEReturnCode.PMTrue Then

                ' if there is any existing data
                If Information.IsArray(vMaintainData) Then

                    ' get array boundaries

                    llbound = vMaintainData.GetLowerBound(1)

                    lUbound = vMaintainData.GetUpperBound(1)

                    ' for each item in the array
                    For lItem As Integer = llbound To lUbound

                        ' create new instance of maintain data
                        oMaintainData = New MaintainData()

                        ' populate maintain data object


                        oMaintainData.Id = ConvertFromNullValues(vMaintainData(ACMaintainDataPosId, lItem), gPMConstants.PMEDataType.PMLong)

                        oMaintainData.Code = CStr(vMaintainData(ACMaintainDataPosCode, lItem)).Trim()

                        oMaintainData.Description = CStr(vMaintainData(ACMaintainDataPosDescription, lItem)).Trim()

                        oMaintainData.CompletionTaskId = ConvertFromNullValues(vMaintainData(ACMaintainDataPosCompletionTaskId, lItem), gPMConstants.PMEDataType.PMLong)

                        oMaintainData.IncompleteTaskId = ConvertFromNullValues(vMaintainData(ACMaintainDataPosIncompleteTaskId, lItem), gPMConstants.PMEDataType.PMLong)

                        oMaintainData.ActionRequired = ConvertFromNullValues(vMaintainData(ACMaintainDataPosActionRequired, lItem), gPMConstants.PMEDataType.PMBoolean)

                        oMaintainData.CompletionTaskCode = CStr(vMaintainData(ACMaintainDataPosCompletionCode, lItem)).Trim()

                        oMaintainData.CompletionTaskDescription = CStr(vMaintainData(ACMaintainDataPosCompletionDescription, lItem)).Trim()

                        oMaintainData.IncompleteCode = CStr(vMaintainData(ACMaintainDataPosIncompleteCode, lItem)).Trim()

                        oMaintainData.IncompleteDescription = CStr(vMaintainData(ACMaintainDataPosIncompleteDescription, lItem)).Trim()

                        '***********
                        ' MEvans : 20-10-2003 : Continuation Tasks

                        oMaintainData.AutoUpdateBatch = ConvertFromNullValues(vMaintainData(ACMaintainDataPosAutoUpdateBatch, lItem), gPMConstants.PMEDataType.PMBoolean)
                        '***********

                        ' copies the current functional values to the original properties
                        ' so comparisons can determine if any values have been updated
                        m_lReturn = oMaintainData.CopyToOriginalData()

                        ' add item to maintain data collection
                        m_colMaintainItems.Add(oMaintainData, CStr(oMaintainData.Id))

                        ' destroy object
                        oMaintainData = Nothing

                    Next lItem

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
    ' Name: ConvertFromNullValues
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-06-2003 : 223
    ' ***************************************************************** '
    Private Function ConvertFromNullValues(ByRef r_vValue As gPMConstants.PMEReturnCode, ByVal v_iDataType As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const sFunctionName As String = "ConvertFromNullValues"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            Select Case v_iDataType
                Case gPMConstants.PMEDataType.PMLong, gPMConstants.PMEDataType.PMCurrency, gPMConstants.PMEDataType.PMBoolean, gPMConstants.PMEDataType.PMDate

                    ' If r_vValue Is DBNull.Value Or r_vValue = "" Then
                    If r_vValue = "" Then
                        Return gPMConstants.PMEReturnCode.PMFalse
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
        gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
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
            'm_lReturn = ListViewBatchStart(lvwList:=lvwTable)
            m_lReturn = ListViewFunc.ListViewBatchStart(lvwList:=lvwTable)

            ' if we have a valid collection
            If Not (m_colMaintainItems Is Nothing) Then

                ' remove all existing list items
                lvwTable.Items.Clear()

                ' for each item in the collection
                For lItem As Integer = 1 To m_colMaintainItems.Count

                    ' get the next item
                    oMaintainItem = m_colMaintainItems.Item(lItem)

                    ' create a new list view item

                    'lstItem = lvwTable.Items.Add()
                    lstItem = lvwTable.Items.Add("")

                    ' populate item details
                    lstItem.Text = oMaintainItem.Code

                    ListViewHelper.GetListViewSubItem(lstItem, ACListViewSubItemIndexDescription).Text = oMaintainItem.Description
                    ListViewHelper.GetListViewSubItem(lstItem, ACListViewSubItemIndexCompletionTask).Text = oMaintainItem.CompletionTaskCode
                    ListViewHelper.GetListViewSubItem(lstItem, ACListViewSubItemIndexIncompleteTask).Text = oMaintainItem.IncompleteCode

                    lstItem.Tag = CStr(oMaintainItem.Id)

                Next lItem

            End If

            ' reselect row


            'lvwTable.FocusedItem = lvwTable.FindItem(CStr(m_SelectedItem), lvwTag)
            lvwTable.FindItemWithText(CStr(m_lSelectedItem))

            ' Show all updates to list view
            'm_lReturn = ListViewBatchEnd()
            m_lReturn = ListViewFunc.ListViewBatchEnd()
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdEdit.Enabled = m_lSelectedItem <> 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

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
    ' Name: ActionEdit
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 27-06-2003 : workflow
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
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
    Private Function ActionCancel() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ActionCancel"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Me.Close()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

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
                ' create new instance of maintain data object
                oMaintainData = New MaintainData()
            ElseIf v_lActionType = ACActionEdit Or v_lActionType = ACActionView Then
                ' load selected item
                If LoadSelectedItem(r_oMaintainData:=oMaintainData) <> gPMConstants.PMEReturnCode.PMTrue Then
                    bFailedToLoadSelectedItem = True
                End If
            End If

            ' if we have successfully retrieved selected item
            If Not bFailedToLoadSelectedItem Then

                ' create new instance of frm details screen
                m_frmDetails = New frmDetails()

                ' set properties
                With m_frmDetails
                    .MaintainData = oMaintainData
                    .ActionType = v_lActionType


                    .TaskOutcomes = m_vTaskOutcomes
                End With

                ' load

                'Load(m_frmDetails)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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

                    If m_frmDetails.ActionType = ACActionEdit Then

                        ' Get existing item
                        oExistingItem = m_colMaintainItems.Item(CStr(oMaintainItem.Id))

                        ' copy back data from details
                        If oExistingItem.Copy(v_oMaintainData:=oMaintainItem) <> gPMConstants.PMEReturnCode.PMTrue Then

                            ' Log Error
                            result = gPMConstants.PMEReturnCode.PMFalse

                            LogMsg(v_sMsg:="Failed to copy data back from form details", v_sMethod:=sFunctionName)

                        End If

                        m_lSelectedItem = oMaintainItem.Id

                    End If

                    ' if the apply button is disabled we want to re-enable it
                    ' if a change has just been made.
                    If Not cmdApply.Enabled Then
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
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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

        Dim nItems As Integer
        Dim oMaintainItem As MaintainData
        Dim bTransactionError, bTransactionOpen As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if we have any items
            If Not (m_colMaintainItems Is Nothing) Then


                If m_oBusiness.BeginTrans = gPMConstants.PMEReturnCode.PMTrue Then

                    bTransactionOpen = True

                    ' get the item count
                    nItems = m_colMaintainItems.Count

                    ' for each item
                    For lItem As Integer = 1 To nItems

                        ' get maintain data item from collection
                        oMaintainItem = m_colMaintainItems.Item(lItem)

                        ' if already in the database
                        If oMaintainItem.ItemUpdated Then

                            '***********
                            ' MEvans : 20-10-2003 : Continuation Tasks
                            ' added autoupdatebatch to method

                            ' update item changes to database

                            If m_oBusiness.UpdateTask(v_lTaskId:=oMaintainItem.Id, v_vCompletionOutcome:=ConvertToNullValues(r_vValue:=oMaintainItem.CompletionTaskId, v_iDataType:=gPMConstants.PMEDataType.PMLong), v_vIncompleteOutcome:=ConvertToNullValues(r_vValue:=oMaintainItem.IncompleteTaskId, v_iDataType:=gPMConstants.PMEDataType.PMLong), v_bActionRequired:=oMaintainItem.ActionRequired, v_bAutoUpdateBatch:=oMaintainItem.AutoUpdateBatch) <> gPMConstants.PMEReturnCode.PMTrue Then

                                bTransactionError = True

                            Else

                                ' copy all data back to original properties so
                                ' we dont updates this item again if this is only
                                ' the apply
                                m_lReturn = oMaintainItem.CopyToOriginalData()

                            End If

                        End If

                    Next lItem

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
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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

                LogMsg(v_sMsg:="Failed to update items to database", v_sMethod:=sFunctionName)

            Else
                cmdApply.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskOutcomes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Private Function GetTaskOutcomes() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskOutcomes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_oBusiness.GetTaskOutcomes(r_vResults:=m_vTaskOutcomes) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ConvertToNullValues
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-06-2003 : 223
    ' ***************************************************************** '
    Private Function ConvertToNullValues(ByRef r_vValue As gPMConstants.PMEReturnCode, ByVal v_iDataType As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const sFunctionName As String = "ConvertToNullValues"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case v_iDataType
                Case gPMConstants.PMEDataType.PMLong, gPMConstants.PMEDataType.PMCurrency
                    If r_vValue = gPMConstants.PMEReturnCode.PMFalse Then

                        r_vValue = Nothing
                    End If

                Case gPMConstants.PMEDataType.PMString
                    If r_vValue = "" Then

                        r_vValue = Nothing
                    End If

            End Select



            Return r_vValue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error. 
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function
End Class