Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	Private m_lStatus As Integer
	Private m_sCallingAppName As String = ""
	Private m_lErrorNumber As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
    Private m_Interface As Interface_Renamed
	
	Private Const ACClass As String = "frmInterface"
	
	
    Public Property InterfaceObj() As Interface_Renamed
		Get
			Return m_Interface
		End Get
        Set(ByVal Value As Interface_Renamed)
			m_Interface = Value
		End Set
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
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
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Private Sub cmbListType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbListType.SelectedIndexChanged
		
		'Load the scheme for the item type just selected
		Dim r As Integer = GetSchemes(VB6.GetItemData(cmbListType, cmbListType.SelectedIndex))
		
	End Sub
	
	Private Sub cmbScheme_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbScheme.SelectedIndexChanged
		
		'Load the versions for the scheme just selected
		Dim r As Integer = GetVersions(VB6.GetItemData(cmbScheme, cmbScheme.SelectedIndex))
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Dim r As Integer
		
		'Validate the various fields first
		If cmbListType.SelectedIndex < 0 Then
			MessageBox.Show("You must select a List Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		Else
			If cmbScheme.SelectedIndex < 0 Then
				MessageBox.Show("You must select a Scheme", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Else
				If cmbVersion.SelectedIndex < 0 Then
					MessageBox.Show("You must select a version", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Else
					If txtFileName.Text = "" Then
						MessageBox.Show("You must select a file to import", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					Else
						'All fields OK so do the import if user confirms
						If MessageBox.Show("Are you sure you want to import this list grouping and overwrite existing grouping for " & cmbScheme.Text & ", Version " & cmbVersion.Text, "Confirm Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
							r = m_Interface.DoImport()
							If r Then
								MessageBox.Show("Import Complete!", "Import Grouping List", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
							Else
								MessageBox.Show("Import Failed!", "Import Grouping List", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
							End If
						End If
					End If
				End If
			End If
		End If
	End Sub
	
	Private Sub cmdOpen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOpen.Click
		
		'Use Common Dialogue (OPen FIle) to get location of import file
		

        CommonDialog1Open.Filter = "All Files (*.*)|*.*|Text Files" & _
                                   "(*.txt)|*.txt"
        ' Specify default filter
        CommonDialog1Open.FilterIndex = 1
        ' Display the Open dialog box
        CommonDialog1Open.ShowDialog()
        ' Display name of selected file
        txtFileName.Text = CommonDialog1Open.FileName

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        GetListTypes()

    End Sub

    Private Function GetListTypes() As Integer

        'Load available ListTypes for the combobox
        Dim result As Integer = 0
        Try

            Dim lRecordsFound As gPMConstants.PMEReturnCode
            Dim vTotalArray(,) As Object
            Dim lReturn As gPMConstants.PMEReturnCode


            'Get ListTypes from DB using business object

            lReturn = g_oBusiness.GetListTypes(r_vResultArray:=vTotalArray, r_lRecordsFound:=lRecordsFound)

            ' Check for errors
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return lReturn
            End If

            If lRecordsFound = gPMConstants.PMEReturnCode.PMNotFound Then
                ' Failed to get details.
                Return result
            End If

            'Loop through array returned from business object, adding items to the combo

            For l As Integer = 0 To vTotalArray.GetUpperBound(1)

                cmbListType.Items.Add(CStr(vTotalArray(1, l)))

                VB6.SetItemData(cmbListType, cmbListType.Items.Count - 1, CInt(vTotalArray(0, l)))
            Next

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the list types from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function GetSchemes(ByRef lListTypeID As Integer) As Integer

        'Load available schemes for the selected List Type into combobox

        Dim result As Integer = 0
        Try

            Dim lRecordsFound As gPMConstants.PMEReturnCode
            Dim vTotalArray(,) As Object
            Dim lReturn As gPMConstants.PMEReturnCode

            'Get schemes from DB using business object

            lReturn = g_oBusiness.GetSchemes(lListTypeID, r_vResultArray:=vTotalArray, r_lRecordsFound:=lRecordsFound)

            ' Check for errors
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return lReturn
            End If

            'Clear combos (cmbVersion will now have different items in it
            cmbScheme.Items.Clear()
            cmbVersion.Items.Clear()

            If lRecordsFound = gPMConstants.PMEReturnCode.PMNotFound Then
                ' Failed to get details.
                Return result
            End If


            Application.DoEvents()

            'Loop through array returned from business object, adding items to the combo

            For l As Integer = 0 To vTotalArray.GetUpperBound(1)

                cmbScheme.Items.Add(CStr(vTotalArray(1, l)))

                VB6.SetItemData(cmbScheme, cmbScheme.Items.Count - 1, CInt(vTotalArray(0, l)))
            Next

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the list types from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function GetVersions(ByRef lSchemeID As Integer) As Integer

        'On Error GoTo Err_GetSchemes

        Dim result As Integer = 0
        Dim lRecordsFound As gPMConstants.PMEReturnCode
        Dim vTotalArray(,) As Object

        'Get versions from DB using business object

        Dim lReturn As gPMConstants.PMEReturnCode = g_oBusiness.GetVersions(lSchemeID, r_vResultArray:=vTotalArray, r_lRecordsFound:=lRecordsFound)

        ' Check for errors
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get details.
            Return lReturn
        End If

        If lRecordsFound = gPMConstants.PMEReturnCode.PMNotFound Then
            ' Failed to get details.
            Return result
        End If

        cmbVersion.Items.Clear()

        'Loop through array returned from business object, adding items to the combo

        For l As Integer = 0 To vTotalArray.GetUpperBound(1)

            cmbVersion.Items.Add(CStr(vTotalArray(1, l)))

            VB6.SetItemData(cmbVersion, cmbVersion.Items.Count - 1, CInt(vTotalArray(0, l)))
        Next

        Return result




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the list types from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function
End Class
