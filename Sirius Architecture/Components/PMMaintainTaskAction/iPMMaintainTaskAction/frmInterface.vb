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

	Private m_oBusiness As bPMMaintainTaskAction.Business
	Private m_lReturn As Integer
	Private m_bInterfaceError As Boolean
	'********************************
	
	Private m_lSelectedItem As Integer
	Private m_colMaintainItems As Collection
	Private m_frmDetails As frmDetails
	Private m_vDocumentTemplates As Object
	Private m_lMaintainItemIdCounter As Integer
	Private m_vCodes As Object
	Private m_vTaskOutcomes( ,  ) As Object
	Private m_vTaskActionTypeOutcomes( ,  ) As Object
	
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
			If g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMMaintainTaskAction.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
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

            m_lReturn = GetDocumentTemplates()
            m_lReturn = GetTaskOutcomes()
            m_lReturn = GetTaskActionTypeOutcomes()

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
            lvwTable.Columns.Insert(ACListViewColIndexTemplate - 1, ACListViewColKeyTemplate, GetResString(ACResDataInterfaceListViewHeaderTemplate), 94)
            lvwTable.Columns.Insert(ACListViewColIndexEffectiveDate - 1, ACListViewColKeyEffectiveDate, GetResString(ACResDataInterfaceListViewHeaderEffectiveDate), 94)

            ' Tag Column Headers to identify in column click so we can sort correctly
            lvwTable.Columns.Item(ACListViewColIndexCode - 1).Tag = ACListViewTagTypeString
            lvwTable.Columns.Item(ACListViewColIndexDescription - 1).Tag = ACListViewTagTypeString
            lvwTable.Columns.Item(ACListViewColIndexTemplate - 1).Tag = ACListViewTagTypeString
            lvwTable.Columns.Item(ACListViewColIndexEffectiveDate - 1).Tag = ACListViewTagTypeDate

            ' set column sizes
            lvwTable.Columns.Item(ACListViewColIndexCode - 1).Width = CInt(VB6.TwipsToPixelsX(1300))
            lvwTable.Columns.Item(ACListViewColIndexDescription - 1).Width = CInt(VB6.TwipsToPixelsX(1950))
            lvwTable.Columns.Item(ACListViewColIndexTemplate - 1).Width = CInt(VB6.TwipsToPixelsX(1300))
            lvwTable.Columns.Item(ACListViewColIndexEffectiveDate - 1).Width = CInt(VB6.TwipsToPixelsX(1050))

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

                    'm_lReturn = ListViewSortByDate(v_oListView:=r_oListView, v_iSourceColumn:=lSourceCol, v_iDirection:=lSort)
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


                        oMaintainData.Id = CInt(ConvertFromNullValues(vMaintainData(ACDataPosId, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.Code = CStr(vMaintainData(ACDataPosCode, lItem)).Trim()

                        oMaintainData.Description = CStr(vMaintainData(ACDataPosDescription, lItem)).Trim()

                        oMaintainData.IsDeleted = CBool(ConvertFromNullValues(vMaintainData(ACDataPosIsDeleted, lItem), gPMConstants.PMEDataType.PMBoolean))

                        oMaintainData.EffectiveDate = CDate(ConvertFromNullValues(vMaintainData(ACDataPosEffectiveDate, lItem), gPMConstants.PMEDataType.PMDate))

                        oMaintainData.DueDays = CInt(ConvertFromNullValues(vMaintainData(ACDataPosDueDays, lItem), gPMConstants.PMEDataType.PMLong))

                        oMaintainData.DocumentTemplateCode = CStr(vMaintainData(ACDataPosDocumentTemplateCode, lItem)).Trim()

                        oMaintainData.OutcomeNotEditable = CBool(ConvertFromNullValues(vMaintainData(ACDataPosOutcomeNotEditable, lItem), gPMConstants.PMEDataType.PMBoolean))

                        ' item is from database therefore it is live...
                        oMaintainData.ItemIsLive = True

                        ' copies the current functional values to the original properties
                        ' so comparisons can determine if any values have been updated
                        m_lReturn = oMaintainData.CopyToOriginalData()


                        ' Get any related PMWrk_Action_Type_Outcomes
                        m_lReturn = PopulateActionTypeOutcomes(r_oMaintainData:=oMaintainData)

                        ' add item to maintain data collection
                        m_colMaintainItems.Add(oMaintainData, CStr(oMaintainData.Id))

                        ' destroy object
                        oMaintainData = Nothing

                    Next lItem

                End If

                ' now we have populates the collection]
                ' get the new base id for all new items
                m_lReturn = SetGeneratedBaseId()


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

                    'TODO:to be handled at runtime
                    'lstItem.Icon = "find"

                    'lstItem.SmallIcon = "find"
                    lstItem.ImageKey = "find"
                    lstItem.Tag = CStr(oMaintainItem.Id)

                    ListViewHelper.GetListViewSubItem(lstItem, ACListViewSubItemIndexDescription).Text = oMaintainItem.Description
                    ListViewHelper.GetListViewSubItem(lstItem, ACListViewSubItemIndexTemplate).Text = oMaintainItem.DocumentTemplateCode
                    ListViewHelper.GetListViewSubItem(lstItem, ACListViewSubItemIndexEffectiveDate).Text = DateTimeHelper.ToString(oMaintainItem.EffectiveDate)

                    ' ghost any deleted items

                    'TODO:to be handled at runtime
                    'lstItem.Ghosted = oMaintainItem.IsDeleted

                    ' populates the code array

                    m_lReturn = PopulateArray(r_vArray:=m_vCodes, v_sItemValue:=oMaintainItem.Code)

                Next lItem

            End If

            ' reselect row


            'lvwTable.FocusedItem = lvwTable.FindItem(CStr(m_lSelectedItem), lvwTag)
            'TODO:to be checked at runtime
            lvwTable.FocusedItem = lvwTable.FindItemWithText(CStr(m_lSelectedItem))
            ' Show all updates to list view
            'm_lReturn = ListViewBatchEnd()
            m_lReturn = ListViewFunc.ListViewBatchEnd()

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

            Else
                ' only enabled when an item is selected
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
                cmdView.Enabled = False

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
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ActionDelete() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ActionDelete"

        Dim oMaintainItem As MaintainData

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' load the current selected item
            If LoadSelectedItem(r_oMaintainData:=oMaintainItem) = gPMConstants.PMEReturnCode.PMTrue Then

                ' reset the current items deleted status
                oMaintainItem.IsDeleted = Not oMaintainItem.IsDeleted

                ' redisplay list items
                PopulateListView()

                ' set new buttons state
                SetupButtons()

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


                    .DocumentTemplates = m_vDocumentTemplates


                    .Codes = m_vCodes
                    .TaskOutcomes = VB6.CopyArray(m_vTaskOutcomes)
                End With

                ' load

                'TODO:to be checked at runtime
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
    ' Name: GetDocumentTemplates
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-06-2003 : workflow
    ' ***************************************************************** '
    Private Function GetDocumentTemplates() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetDocumentTemplates"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_oBusiness.GetDocumentTemplates(r_vResults:=m_vDocumentTemplates) <> gPMConstants.PMEReturnCode.PMTrue Then

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
                        m_lMaintainItemIdCounter += 1

                        oMaintainItem.Id = m_lMaintainItemIdCounter

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


    ' ***************************************************************** '
    ' Name: SetGeneratedBaseId
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-06-2003 : 223
    ' ***************************************************************** '
    Private Function SetGeneratedBaseId() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SetGeneratedBaseId"

        Dim lHighestId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if generated id is set
            ' at a position higher than all the existing entries we will
            ' have a unique id. (at least within this process)

            For lItem As Integer = 1 To m_colMaintainItems.Count

                If m_colMaintainItems.Item(lItem).Id > lHighestId Then

                    lHighestId = m_colMaintainItems.Item(lItem).Id
                End If
            Next

            m_lMaintainItemIdCounter = lHighestId + 1

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

        Dim llbound, lUbound, lItem As Integer
        Dim bItemExists As Boolean
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' create new collection
            If Not Information.IsArray(r_vArray) Then
                ReDim r_vArray(0)
            End If

            ' get array boundaries
            llbound = r_vArray.GetLowerBound(0)
            lUbound = r_vArray.GetUpperBound(0)

            ' for each item in the array
            For lItem = llbound To lUbound

                ' check if we match the values from the array specified

                If CStr(r_vArray(lItem)).Trim() = v_sItemValue.Trim() Then
                    ' indicate item already exists
                    bItemExists = True

                    ' quit loop
                    lItem = lUbound
                End If

            Next lItem

            ' if the item doesnt already exist
            ' add it to the array
            If Not bItemExists Then
                lItem = lUbound + 1
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

        Dim nItems As Integer
        Dim oMaintainItem As MaintainData
        Dim bTransactionError, bTransactionOpen As Boolean
        Dim lMaintainItemId As Integer

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
                        If oMaintainItem.ItemIsLive Then

                            ' and something has changes
                            If oMaintainItem.ItemUpdated Then

                                ' update item changes to database

                                If m_oBusiness.UpdateTaskActionType(v_lId:=oMaintainItem.Id, v_sDescription:=oMaintainItem.Description, v_bIsDeleted:=oMaintainItem.IsDeleted, v_dtEffectiveDate:=oMaintainItem.EffectiveDate, v_lDueDays:=oMaintainItem.DueDays, v_sDocumentTemplateCode:=oMaintainItem.DocumentTemplateCode, v_bOutcomeNotEditable:=oMaintainItem.OutcomeNotEditable) <> gPMConstants.PMEReturnCode.PMTrue Then

                                    bTransactionError = True

                                Else

                                    ' copy all data back to original properties so
                                    ' we dont updates this item again if this is only
                                    ' the apply
                                    m_lReturn = oMaintainItem.CopyToOriginalData()

                                End If

                            End If

                        Else

                            ' add item to database

                            If m_oBusiness.AddTaskActionType(r_lId:=lMaintainItemId, v_sCode:=oMaintainItem.Code, v_sDescription:=oMaintainItem.Description, v_bIsDeleted:=oMaintainItem.IsDeleted, v_dtEffectiveDate:=oMaintainItem.EffectiveDate, v_lDueDays:=oMaintainItem.DueDays, v_sDocumentTemplateCode:=oMaintainItem.DocumentTemplateCode, v_bOutcomeNotEditable:=oMaintainItem.OutcomeNotEditable) <> gPMConstants.PMEReturnCode.PMTrue Then

                                bTransactionError = True

                            Else
                                oMaintainItem.Id = lMaintainItemId
                                ' reset live indicator in case this is only an apply
                                ' so we dont attempt to add this item again
                                oMaintainItem.ItemIsLive = True
                            End If

                        End If

                        ' check if the task outcomes have changed in this process
                        If oMaintainItem.OutcomesUpdated Then

                            ' delete all existing task action type outcomes
                            ' and replace with the new selection

                            If m_oBusiness.UpdateTaskActionTypeOutcomes(v_lTaskActionTypeId:=oMaintainItem.Id, v_sTaskActionTypeOutcomeIds:=oMaintainItem.TaskOutcomeIds) <> gPMConstants.PMEReturnCode.PMTrue Then

                                bTransactionError = True

                            Else
                                ' reset so we dont update again if this was only apply
                                oMaintainItem.OutcomesUpdated = False
                            End If

                        End If

                        If bTransactionError Then
                            Exit For
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
    ' Name: GetTaskActionTypeOutcomes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '

    Private Function GetTaskActionTypeOutcomes() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskActionTypeOutcomes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_oBusiness.GetTaskActionTypeOutcomes(r_vResults:=m_vTaskActionTypeOutcomes) <> gPMConstants.PMEReturnCode.PMTrue Then

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
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateActionTypeOutcomes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Private Function PopulateActionTypeOutcomes(ByRef r_oMaintainData As MaintainData) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "PopulateActionTypeOutcomes"

        Dim llbound, lUbound As Integer

        Dim lTaskOutcomeId, lTaskActionTypeId, lItemId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if there are any prmwrk_task_action_type_outcomes
            If Information.IsArray(m_vTaskActionTypeOutcomes) Then

                ' get the item id
                lItemId = r_oMaintainData.Id

                ' get the array boundaries
                llbound = m_vTaskActionTypeOutcomes.GetLowerBound(1)
                lUbound = m_vTaskActionTypeOutcomes.GetUpperBound(1)

                ' for each item in the array
                For lItem As Integer = llbound To lUbound

                    ' get item properties
                    lTaskActionTypeId = CInt(m_vTaskActionTypeOutcomes(0, lItem))
                    lTaskOutcomeId = CInt(m_vTaskActionTypeOutcomes(1, lItem))

                    ' if the maintain data item id matches the arrays item id
                    If lTaskActionTypeId = lItemId Then
                        ' add the relevant outcome to the item
                        m_lReturn = r_oMaintainData.AddOutcome(v_lTaskOutcomeId:=lTaskOutcomeId)
                    End If

                Next lItem

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