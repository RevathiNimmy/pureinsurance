Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmListType
    Inherits System.Windows.Forms.Form
    Private frmNewListType As frmNewListType
    Private FrmImport As FrmImport

	' ***************************************************************** '
	' Module Name: frmListType
	'
	' Date: 28/06/2002
	'
	' Description:  This will show and allow maintenance of list types
	'
	' Edit History:
	'   28/06/2002 SJP  - Tidied up after merge from Carole Nash
	' ***************************************************************** '
	
	
	' ***************************************************************** '
	'
	' Name: cmdAdd_Click()
	'
	' Description:  This will add a List Type when the Add button is pressed
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ***************************************************************** '
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		
		Try 
            'NIIT DONE 
            frmNewListType = New frmNewListType

			'show from for new item
			frmNewListType.ShowDialog()
			
			'update view
			PopList()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show newlisttype form", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	
	' ***************************************************************** '
	'
	' Name: cmdCancel_Click())
	'
	' Description:  This will unload the form
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ***************************************************************** '
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Try 
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: cmdDelete_Click()
	'
	' Description:  This will delete a list item
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ***************************************************************** '
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		
		Try 
			
			'   Check item is selected
			If lvwListTypes.FocusedItem Is Nothing Then Exit Sub
			
			'Check that List Item is in use

            If Not (m_oBusiness.ListInUse(Convert.ToString(lvwListTypes.FocusedItem.Tag))) Then

                'delete it

                m_lReturn = m_oBusiness.deletelisttype(Convert.ToString(lvwListTypes.FocusedItem.Tag))

                'update display
                PopList()

            Else
                '   Can't delete it.
                MessageBox.Show("List is in use and cannot be deleted without corrupting data", "List Management", MessageBoxButtons.OK)
                Exit Sub
            End If

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete item", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: cmdImport_Click()
    '
    ' Description:  This will import a list from a CSV file
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub cmdImport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdImport.Click

        Try
            'NIIT DONE
            FrmImport = New FrmImport
            FrmImport.ShowDialog()

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load import form", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdImport_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()
        Try

            'Calling function to show the form on taskbar
            iPMFunc.ShowFormInTaskBar_Attach()

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialize the Interface Object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: Form_Load()
    '
    ' Description:  This will perform events whilst loading form
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '

    Private Sub frmListType_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try


            'Removes the hook from the form once the form is created on screen.
            iPMFunc.ShowFormInTaskBar_Detach()
            'frmListType = New frmListType ' test

            'get list and display them
            PopList()

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Falied to load form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: PopList()
    '
    ' Description:  This will populate the list.
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub PopList()


        Dim ListItem As ListViewItem
        Dim vData(,) As Object

        Try

            '   This will get list types from business object.


            m_lReturn = m_oBusiness.GetListTypes(vData)

            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                MessageBox.Show("No list types were found", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception("1, " + +", Failed to get list of list types")
                Exit Sub
            End If

            'if we have a list then display it
            If Information.IsArray(vData) Then

                With lvwListTypes
                    'clear
                    .Items.Clear()

                    'ad stuff to list view

                    For i As Integer = 0 To vData.GetUpperBound(1)

                        ListItem = .Items.Add(CStr(vData(1, i)))

                        ListViewHelper.GetListViewSubItem(ListItem, 1).Text = CStr(vData(2, i))

                        'set list type id to the tag


                        ListItem.Tag = CStr(vData(0, i))
                    Next i

                End With

            Else
                lvwListTypes.Items.Clear()
            End If

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fill list", vApp:=ACApp, vClass:=ACClass, vMethod:="PopList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub _SSTab1_TabPage0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _SSTab1_TabPage0.Click

    End Sub
End Class
