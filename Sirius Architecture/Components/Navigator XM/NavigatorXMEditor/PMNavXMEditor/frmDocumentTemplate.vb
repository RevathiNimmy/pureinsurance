Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmDocumentTemplate
	Inherits System.Windows.Forms.Form
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_lDocumentTypeID As Integer
	Private m_lDocumentTemplateID As Integer
	
	Private m_sTempCode As String = ""
	Private m_sTypeCode As String = ""
	Private m_sTempDesc As String = ""
	
	Private m_vDocTypes( ,  ) As Object
	Private m_vDocTemplates( ,  ) As Object
	
	Private m_oBusiness As Object
	
	Private Const ACClass As String = "frmDocumentTemplate"
	
	' array constants
	Private Const DOC_TYPE_ID As Integer = 0
	Private Const DOC_TYPE_CODE As Integer = 1
	Private Const DOC_TYPE_DESC As Integer = 2
	
	Private Const DOC_TEMPLATE_ID As Integer = 0
	Private Const DOC_TEMPLATE_CODE As Integer = 1
	Private Const DOC_TEMPLATE_DESC As Integer = 2
	Private Const DOC_TEMPLATE_TYPE As Integer = 3
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public ReadOnly Property DocumentTemplateID() As Integer
		Get
			Return m_lDocumentTemplateID
		End Get
	End Property
	
	Public WriteOnly Property Business() As Object
		Set(ByVal Value As Object)
			m_oBusiness = Value
		End Set
	End Property
	
	Public ReadOnly Property TemplateCode() As String
		Get
			Return m_sTempCode
		End Get
	End Property
	
	Public ReadOnly Property TypeCode() As String
		Get
			Return m_sTypeCode
		End Get
	End Property
	
	Public ReadOnly Property TemplateDesc() As String
		Get
			Return m_sTempDesc
		End Get
	End Property
	
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Initialise the form
	'
	' ***************************************************************** '
	Public Function Initialise() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: Load (Standard Method)
	'
	' Description: Load the form details
	'
	' ***************************************************************** '
	Public Function Load_Renamed() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Gets the interface details to be displayed.
			m_lReturn = CType(GetInterfaceDetails(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Load failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ShowForm (Standard Method)
	'
	' Description: Show the form using the display state passed
	'
	' ***************************************************************** '
	Public Function ShowForm(ByRef lDisplayState As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Show the the form, allow user input etc.
			VB6.ShowForm(Me, lDisplayState)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowForm failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetDocumentTypes
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Private Function GetDocumentTypes() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Get the details from the business object.
			

            m_lReturn = m_oBusiness.GetDocumentTypes(m_vDocTypes)

            ' Check for other errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTypes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocumentTemplates
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetDocumentTemplates() As Integer

        Dim result As Integer = 0
        Dim lWidth As Integer
        Dim oItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Get the details from the business object.


            m_lReturn = m_oBusiness.GetDocumentTemplates(m_lDocumentTypeID, m_vDocTemplates)

            ' Check for other errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                ' Failed to get details.

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get document templates from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            lvwDocuments.View = View.Details

            lWidth = CInt((VB6.PixelsToTwipsX(lvwDocuments.Width) / 3) - 45)

            lvwDocuments.Columns.Clear()
            lvwDocuments.Columns.Add("Code", "Template Code", CInt(VB6.TwipsToPixelsX(lWidth)))
            lvwDocuments.Columns.Add("Type", "Type Code", CInt(VB6.TwipsToPixelsX(lWidth)))
            lvwDocuments.Columns.Add("Desc", "Template Description", CInt(VB6.TwipsToPixelsX(lWidth)))

            lvwDocuments.Items.Clear()


            If Not Information.IsArray(m_vDocTemplates) Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            For lLoop As Integer = m_vDocTemplates.GetLowerBound(1) To m_vDocTemplates.GetUpperBound(1)

                oItem = lvwDocuments.Items.Add("T" & CStr(m_vDocTemplates(DOC_TEMPLATE_ID, lLoop)), CStr(m_vDocTemplates(DOC_TEMPLATE_CODE, lLoop)), "")
                ListViewHelper.GetListViewSubItem(oItem, 1).Text = CStr(m_vDocTemplates(DOC_TEMPLATE_TYPE, lLoop))
                ListViewHelper.GetListViewSubItem(oItem, 2).Text = CStr(m_vDocTemplates(DOC_TEMPLATE_DESC, lLoop))

            Next


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTemplates failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	
	' ***************************************************************** '
	' Name: GetInterfaceDetails
	'
	' Description: get the roadmap info to display on listview
	'
	' History: RDC 28032003 created
	' ***************************************************************** '
	Private Function GetInterfaceDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Get the interface details from the
			' business object.
			m_lReturn = CType(GetDocumentTypes(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the details.
				Return result
			End If
			
			' Assign the details from the List data storage
			' to the interface.
			m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInterfaceDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DataToInterface
	'
	' Description: display the roadmap info on the listview
	'
	' History: RDC 28032003 created
	' ***************************************************************** '
	Private Function DataToInterface() As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			cboDocumentType.Items.Clear()
			cboDocumentType.Items.Add("(all document types)")
			VB6.SetItemData(cboDocumentType, 0, ID_NO_VALUE)
			
			For lLoop As Integer = m_vDocTypes.GetLowerBound(1) To m_vDocTypes.GetUpperBound(1)
				cboDocumentType.Items.Add(CStr(m_vDocTypes(DOC_TEMPLATE_DESC, lLoop)))
				VB6.SetItemData(cboDocumentType, cboDocumentType.Items.Count - 1, CInt(m_vDocTypes(DOC_TEMPLATE_ID, lLoop)))
			Next 
			
			cboDocumentType.SelectedIndex = 0
			m_lDocumentTypeID = ID_NO_VALUE
			m_lDocumentTemplateID = ID_NO_VALUE
			
			m_lReturn = CType(GetDocumentTemplates(), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	Private Sub cboDocumentType_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDocumentType.SelectionChangeCommitted
		
		m_lDocumentTypeID = VB6.GetItemData(cboDocumentType, cboDocumentType.SelectedIndex)
		
		m_lReturn = CType(GetDocumentTemplates(), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOk
		
		Me.Close()
		
	End Sub
	
	Private Sub frmDocumentTemplate_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		If m_lDocumentTemplateID = ID_NO_VALUE And m_lStatus = gPMConstants.PMEReturnCode.PMOk Then
			' nothing selected when Ok clicked
			Cancel = True
			Exit Sub
		End If
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private Sub lvwDocuments_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDocuments.DoubleClick
		
		If m_lDocumentTemplateID <> ID_NO_VALUE Then
			m_lStatus = gPMConstants.PMEReturnCode.PMOk
			Me.Close()
		End If
		
	End Sub
	
	Private Sub lvwDocuments_ItemClick(ByVal Item As ListViewItem)
		
		m_lDocumentTemplateID = CInt(Mid(Item.Name, 2))
		
		m_sTempCode = Item.Text.Trim()
		m_sTypeCode = ListViewHelper.GetListViewSubItem(Item, 1).Text.Trim()
		m_sTempDesc = ListViewHelper.GetListViewSubItem(Item, 2).Text.Trim()
		
	End Sub
End Class
