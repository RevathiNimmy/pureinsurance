Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide no.129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: {TodaysDate}
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
    'Developer Guide no. 50
    Dim frmDetails As frmDetails
    'Developer Guide no. 7
    Private Const vbFormCode As Integer = 0
	' variable to collect the list item of the list view
	Private lst_item As ListViewItem
	
	'Public g_oBusiness As Object
	
	'The Collections row ID
	Private lBusinessDataID As Integer
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As gPMConstants.PMEReturnCode
	'**********************************Uncomment for Integration
	Private m_iTask As gPMConstants.PMEComponentAction
	'************************************************************
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	'Private m_lClaimID As Long
	Private m_lPartyID As Integer
	Private m_sPartyName As String = ""
	Private m_dShare As Double
	Private m_cShareValue As Decimal
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iCLMCoinsuranceRecoveries.General
	
	' Declare an instance of the Business object.
	'Private m_oBusiness As Object
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' Stores the details from the business object.
	
	Public Property ClaimID() As Integer
		Get
			Return m_lClaimID
		End Get
		Set(ByVal Value As Integer)
			'm_lClaimID = lClaimID
		End Set
	End Property
	
	
	Public Property PartyID() As Integer
		Get
			Return m_lPartyID
		End Get
		Set(ByVal Value As Integer)
			m_lPartyID = Value
		End Set
	End Property
	
	
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
	
	Public Property TransactionType() As String
		Get
			
			Return m_sTransactionType
			
		End Get
		Set(ByVal Value As String)
			
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	
	' PUBLIC Methods (Begin)
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	' ***************************************************************** '
	Public Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Try 
			' No data entry fields in this form
			' Pass control and required settings to FormControl
			'    m_lReturn = m_oFormFields.AddNewFormField( _
			''                   ctlControl:=<Control Name>, _
			''                   lFieldType:=<PM field type>, _
			''                   lFormat:=<PM format string>, _
			''                   lMandatory:=<PMMandatory or PMNonMandatory)
			
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the details from the business object.
			

			m_lReturn = g_oBusiness.GetDetails( , m_lClaimID, m_lInsuranceFileCnt)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
				
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: BusinessToInterface
	'
	' Description: Updates all interface details from the business
	'              object.
	'
	' ***************************************************************** '
	Public Function BusinessToInterface() As Integer
		Dim result As Integer = 0
		Dim sngwidth As Integer
        Dim r_vPartyID As Object
		Dim r_vPartyName As String = ""
        Dim r_vShare, r_vShareValue As Object
		Dim ivar As Integer

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' Assign the details from the business object
			' to the data storage.
			m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			lstView.Columns.Clear()
			lstView.LabelEdit = False
			' Calculate the width of a ColumnHeader object.
			sngwidth = VB6.PixelsToTwipsX(lstView.Width) / 4
			
			' insert the column headers
			lstView.Columns.Insert(0, "Coinsurer Name", CInt(VB6.TwipsToPixelsX(sngwidth)))
			lstView.Columns.Insert(1, "Share %", CInt(VB6.TwipsToPixelsX(sngwidth - 750)))
			lstView.Columns.Insert(2, "Current Share Value", CInt(VB6.TwipsToPixelsX(sngwidth)))
			lstView.Columns.Insert(3, "New Share Value", CInt(VB6.TwipsToPixelsX(sngwidth)))
			
			'insert the records in the List View
			lstView.Items.Clear()
			'RWH(05/03/2001)
			g_bRetainedRecordAlreadyIncluded = False

			g_oBusiness.ClaimID = m_lClaimID

			m_lReturn = g_oBusiness.Getnext(vPartyID:=r_vPartyID, vPartyName:=r_vPartyName, vShare:=r_vShare, vShareValue:=r_vShareValue)
			
			'    'RWH(15/08/01) If this is a payment we need to show payment figures NOT reserve.
			'    If (m_iTask = PMView) Then
			'        m_lReturn = g_oBusiness.GetCurrentPayment(r_cPayment:=cCurrentPayment)
			'        If (m_lReturn <> PMTrue) Then
			'            BusinessToInterface = PMFalse
			'            Exit Function
			'        End If
			'
			'    End If
			
			ivar = 1
			Do While m_lReturn <> gPMConstants.PMEReturnCode.PMEOF
				If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					lst_item = lstView.Items.Add(r_vPartyName)
					
					'RWH(05/03/2001) If we are adding a retained record added in New Business
					'then flag it.
					If r_vPartyName.Trim().ToUpper() = "RETAINED" Then
						g_bRetainedRecordAlreadyIncluded = True
					End If
                    lst_item.SubItems.Add(1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, r_vShare)
					
					'RWH(03/02/2001) Use txtTotalCurrentShareValue as the other doesn't exist until
					'we save here.
					'            'RWH(15/08/01) If this is a payment we need to show payment figures NOT reserve.
					'            If (m_iTask = PMView) Then
					'                lst_item.SubItems(2) = cCurrentPayment / 100 * r_vShare
					'            Else

                    lst_item.SubItems.Add(2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, Conversion.Val(txtTotalCurrentShareValue.Text) * (CDbl(r_vShare) / 100))
					'            lst_item.SubItems(2) = FormatField(PMFormatCurrency, r_vShareValue)
					'            End If

					lst_item.Tag = CStr(CInt(r_vPartyID))
					

					m_lReturn = g_oBusiness.Getnext(vPartyID:=r_vPartyID, vPartyName:=r_vPartyName, vShare:=r_vShare, vShareValue:=r_vShareValue)
					'lst_item.Tag = ivar
					'ivar = ivar + 1
				Else
					Exit Do
				End If
			Loop 
			
			'AJM 25/09/2001 - right align numeric figures
			lstView.Columns.Item(1).TextAlign = HorizontalAlignment.Right
			lstView.Columns.Item(2).TextAlign = HorizontalAlignment.Right
			lstView.Columns.Item(3).TextAlign = HorizontalAlignment.Right
			
			'    If m_iTask = PMAdd Then
			'        r_vResultArray = Null
			'        m_lReturn = g_oBusiness.GetTreatment_Values(r_vResultArray)
			'
			'        If m_lReturn <> PMTrue Then
			'            BusinessToInterface = PMFalse
			'            Exit Function
			'        End If
			'
			'        cmbBox.Clear
			'        If IsArray(r_vResultArray) Then
			'            For ivar = 0 To UBound(r_vResultArray, 2)
			'                cmbBox.AddItem r_vResultArray(0, ivar)
			'            Next ivar
			'                cmbBox.ListIndex = 0
			'        End If
			'     Else
			'        r_vResultArray = Null
			'        m_lReturn = g_oBusiness.GetTreatmentValue(m_lClaimID, r_vResultArray)
			'        If m_lReturn <> PMTrue Then
			'           BusinessToInterface = PMFalse
			'           Exit Function
			'        End If
			'        cmbBox.Clear
			'        If IsArray(r_vResultArray) Then
			'            cmbBox.AddItem r_vResultArray(0, 0)
			'        End If
			'        cmbBox.Enabled = False
			'     End If
			
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: InterfaceToBusiness
	'
	' Description: Updates all business members from the interface
	'              details.
	'
	' ***************************************************************** '
	Public Function InterfaceToBusiness() As Integer
		
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the business object.
			
			' Assign the details from the interface to the data storage.
			m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Set the business data ID to one because we are only
			' dealing with one record item only.
			
			' Check the task.
			'    Select Case (m_iTask)
			'        Case PMAdd
			'            ' Inform the business object with a new data item.
			'            m_lReturn& = g_oBusiness.EditAdd(lRow:=lBusinessDataID&, _
			''                                            vPartyName:=PartyName, vShare:=Share, _
			''                                            vShareValue:=ShareValue)
			'
			'        Case PMEdit
			'            ' Inform the business object with an updated data item.
			'
			'            m_lReturn& = g_oBusiness.EditUpdate(lRow:=lBusinessDataID&, _
			''                                            vPartyName:=PartyName, vShare:=Share, _
			''                                            vShareValue:=ShareValue)
			'    End Select
			'
			'    ' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        InterfaceToBusiness = PMFalse
			'
			'        ' Log Error.
			'        LogMessage _
			''            iType:=PMLogError, _
			''            sMsg:="Failed to assign the interface details to business object", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="InterfaceToBusiness"
			'    End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	'***************************************************************** '
	' Name: DisplayLookupDetails
	'
	' Description: Displays all of the lookup details using the lookup
	'              values/details.
	'
	' ***************************************************************** '
	Public Function DisplayLookupDetails() As Integer
		Dim result As Integer = 0
		Dim r_vresultArray( ,  ) As Object
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the lookup values.
			If Task <> gPMConstants.PMEComponentAction.PMView Then
				m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				m_lReturn = CType(GetLookupDetails(sLookupTable:="CoInsurance_Treatment", ctlLookup:=cmbBox), gPMConstants.PMEReturnCode)
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			If cmbBox.Items.Count > 0 And m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				cmbBox.SelectedIndex = 0
			Else

				r_vresultArray = Nothing

				m_lReturn = g_oBusiness.GetTreatmentValue(m_lClaimID, r_vresultArray)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Throw New Exception()
					Return result
				End If
				cmbBox.Items.Clear()
				If Information.IsArray(r_vresultArray) Then
					cmbBox.Items.Add(CStr(r_vresultArray(0, 0)))
					cmbBox.SelectedIndex = 0
				End If
				cmbBox.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	'
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: InterfaceToData
	'
	' Description: Updates the data storage from the interface details.
	'
	' ***************************************************************** '
	Private Function InterfaceToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the data storage.
			lst_item = lstView.Items.Item(lBusinessDataID - 1)
			If Convert.ToString(lst_item.Tag) <> "" Then

				PartyID = Convert.ToString(lst_item.Tag)
				PartyName = lst_item.Text

				Share = CDbl(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatLong, ListViewHelper.GetListViewSubItem(lst_item, 1).Text))
				If ListViewHelper.GetListViewSubItem(lst_item, 3).Text = "" Then
					ShareValue = CDec(ListViewHelper.GetListViewSubItem(lst_item, 2).Text)
				Else
					ShareValue = CDec(ListViewHelper.GetListViewSubItem(lst_item, 3).Text)
				End If
			Else
				PartyID = 0
			End If
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	''' <summary>
    ''' Sets all of the interface default values.
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
	Private Function SetInterfaceDefaults() As Integer
	    Dim nResult As Integer = 0
	    Dim aoResultArray(,) As Object

	    Try

            nResult = PMEReturnCode.PMTrue

	        ' Center the interface.
	        iPMFunc.CenterForm(Me)

	        ' Display all language specific captions.
            nResult = CType(DisplayCaptions(), PMEReturnCode)

	        ' Check for errors.
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If


            nResult = CType(SetFirstLastControls(), PMEReturnCode)

	        ' Check for errors.
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If m_iTask = PMEComponentAction.PMView Then
                txtClaimNumber.Enabled = False
                txtTotalCurrentShareValue.Enabled = False
                txtTotalNewShareValue.Enabled = False
                txtTotalSharePercentage.Enabled = False
                cmbBox.Enabled = False
            End If

	        ' Set any other default values to the interface.
	        ' add the values to the Total Share Percentage and Add the Values in the Share Values


	        aoResultArray = Nothing

            nResult = g_oBusiness.GetMainShare(m_lClaimID, aoResultArray)

	        ' Check for errors.
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
	        If Information.IsArray(aoResultArray) Then

	            If CStr(aoResultArray(0, 0)) <> "" Then

	                txtTotalCurrentShareValue.Text = CStr(aoResultArray(0, 0))

	                txtTotalNewShareValue.Text = CStr(aoResultArray(0, 0))
	            Else
                    txtTotalCurrentShareValue.Text = gPMFunctions.FormatField(PMEFormatStyle.PMFormatCurrency,
                                                                              "0")
                    txtTotalNewShareValue.Text = gPMFunctions.FormatField(PMEFormatStyle.PMFormatCurrency, "0")
	            End If
	        Else
                txtTotalCurrentShareValue.Text = gPMFunctions.FormatField(PMEFormatStyle.PMFormatCurrency, "0")
                txtTotalNewShareValue.Text = gPMFunctions.FormatField(PMEFormatStyle.PMFormatCurrency, "0")
	        End If
            txtTotalSharePercentage.Text = gPMFunctions.FormatField(PMEFormatStyle.PMFormatPercent, "100")

	        If m_sClaimNumber = "" Then


	            aoResultArray = Nothing

	            g_oBusiness.ClaimID = m_lClaimID

                nResult = g_oBusiness.GetClaimNumber(aoResultArray)
	            If Information.IsArray(aoResultArray) Then

	                m_sClaimNumber = CStr(aoResultArray(0, 0))
	                txtClaimNumber.Text = m_sClaimNumber
	            End If
	        Else
	            txtClaimNumber.Text = m_sClaimNumber
	        End If

            If Task = PMEComponentAction.PMAdd Then
                ' do nothing
            ElseIf Task = PMEComponentAction.PMEdit Then
                cmdButton(g_cIADD).Enabled = False
            Else
                cmdButton(g_cIADD).Enabled = False
                cmdButton(g_cIEDIT).Enabled = False
                cmdButton(g_cIDELETE).Enabled = False
            End If
            Return nResult

	    Catch excep As System.Exception

            nResult = PMEReturnCode.PMError

	        ' Log Error.
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                               sMsg:="Failed to set the interface defaults",
                               vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults",
                               vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            Return nResult

	    End Try
	End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            'ReDim m_ctlTabFirstLast(1,0 )

            ' Set the first and last data entry controls for
            ' all of the tabs.


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If



            cmdButton(g_cIOK).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdButton(g_cICANCEL).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdButton(g_cIHELP).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            tabMainTab.SelectedTab.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblClaimNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCoinsuranceTreatment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCoinsuranceTreatment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTotalShare.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTotalShare, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTotalNewShareValue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTotalNewShareValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTotalCurrentShareValue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTotalCurrentShareValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdButton(g_cIADD).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionAdd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdButton(g_cIEDIT).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionEdit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdButton(g_cIDELETE).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionDelete, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case Else 'PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                    '        Case PMView
                    ' Get lookup values for viewing only.
                    '            m_lReturn& = g_oBusiness.GetLookupValues( _
                    ''                iLookupType:=PMLookupSingle, _
                    ''                vTableArray:=m_vLookupValues, _
                    ''                iLanguageID:=g_iLanguageID%, _
                    ''                vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'Developer Guide no. 153
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.

                'Developer Guide no. 153
                Dim NewIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(Trim(m_vLookupDetails(ACDetailDesc, lCntr)), m_vLookupDetails(ACDetailKey, lCntr)))
                'SP150998 - compare long value not string
                ' Check if this is the selected index.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        'Developer Guide no. 153
                        ctlLookup.SelectedIndex = NewIndex
                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

                'Developer Guide no. 153
                ctlLookup.SelectedIndex = -1
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdButton_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdButton_2.Click, _cmdButton_1.Click, _cmdButton_0.Click, _cmdButton_5.Click, _cmdButton_4.Click, _cmdButton_3.Click
        Dim Index As Integer = Array.IndexOf(cmdButton, eventSender)
        Dim dShare As Double
        Dim sMessage, sTitle As String
        Dim vArray(,) As Object
        Dim bflag As Boolean

        Try

            Select Case Index
                Case g_cIOK
                    cmdOk_Click()
                Case g_cICANCEL
                    cmdCancel_Click()
                Case g_cIHELP
                    ' do nothing
                Case g_cIADD
                    Dim tempLoadForm As frmDetails = frmDetails
                    'developer guide no.50
                    frmDetails = New frmDetails
                    frmDetails.txtBox(g_cINEW_SHARE_VALUE).Visible = False
                    frmDetails.lblBox(g_cINEW_SHARE_VALUE).Visible = False


                    vArray = Nothing


                    m_lReturn = g_oBusiness.GetParty(m_lClaimID, vArray)

                    frmDetails.cmbBox.Items.Clear()

                    If Information.IsArray(vArray) Then


                        For ivar1 As Integer = 0 To vArray.GetUpperBound(1)
                            bflag = False
                            For ivar2 As Integer = 1 To lstView.Items.Count
                                ' check whether the name exists in the list view
                                lst_item = lstView.Items.Item(ivar2 - 1)

                                If lst_item.Text = CStr(vArray(1, ivar1)) Then
                                    bflag = True
                                End If
                            Next ivar2
                            If Not bflag Then
                                Dim cmbBox_NewIndex As Integer = -1

                                cmbBox_NewIndex = frmDetails.cmbBox.Items.Add(CStr(vArray(1, ivar1)))

                                VB6.SetItemData(frmDetails.cmbBox, cmbBox_NewIndex, CInt(vArray(0, ivar1)))
                            End If
                        Next ivar1

                        If frmDetails.cmbBox.Items.Count = 1 Then
                            frmDetails.cmbBox.SelectedIndex = 0
                        ElseIf frmDetails.cmbBox.Items.Count > 1 Then
                            frmDetails.cmbBox.SelectedIndex = 1
                        End If
                    End If

                    If frmDetails.cmbBox.Items.Count = 0 Then

                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACADDTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAdd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        ' Display message.
                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If

                    frmDetails.ShowDialog()
                Case g_cIEDIT
                    Dim tempLoadForm2 As frmDetails = frmDetails
                    'developer guide no.50
                    frmDetails = New frmDetails
                    frmDetails.cmbBox.Items.Clear()
                    frmDetails.cmbBox.Items.Add(lstView.Items.Item(lstView.FocusedItem.Index).Text)
                    frmDetails.cmbBox.SelectedIndex = 0
                    frmDetails.cmbBox.Enabled = False

                    frmDetails.txtBox(g_cINEW_SHARE_VALUE).Visible = True
                    frmDetails.lblBox(g_cINEW_SHARE_VALUE).Visible = True

                    lst_item = lstView.Items.Item(lstView.FocusedItem.Index)


                    frmDetails.txtBox(g_cICURRENT_SHARE_VALUE).Text = CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMConstants.PMEFormatStyle.PMFormatLong, ListViewHelper.GetListViewSubItem(lst_item, 2).Text))
                    If ListViewHelper.GetListViewSubItem(lst_item, 3).Text = "" Then


                        frmDetails.txtBox(g_cINEW_SHARE_VALUE).Text = CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMConstants.PMEFormatStyle.PMFormatLong, ListViewHelper.GetListViewSubItem(lst_item, 2).Text))
                    Else


                        frmDetails.txtBox(g_cINEW_SHARE_VALUE).Text = CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMConstants.PMEFormatStyle.PMFormatLong, ListViewHelper.GetListViewSubItem(lst_item, 3).Text))
                    End If


                    frmDetails.txtBox(g_cISHARE_PERCENTAGE).Text = CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatLong, ListViewHelper.GetListViewSubItem(lst_item, 1).Text))
                    frmDetails.txtBox(g_cICURRENT_SHARE_VALUE).Enabled = False
                    frmDetails.ShowDialog()
                Case g_cIDELETE
                    lst_item = lstView.Items.Item(lstView.FocusedItem.Index)
                    If lst_item.Text = "Retained" Then
                        ' Failed to get an instance of the business object.
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                        ' Display error stating the problem.

                        ' Get description from the resource file.

                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelete, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        ' Display message.
                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Exit Sub
                    Else
                        '            m_lReturn = g_oBusiness.editdelete(lstView.SelectedItem.Index)
                        '            If m_lReturn <> PMTrue Then
                        '                m_lErrorNumber = PMFalse
                        '                Exit Sub
                        '            End If
                        lst_item = lstView.Items.Item(lstView.FocusedItem.Index)

                        dShare = CDbl(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatLong, ListViewHelper.GetListViewSubItem(lst_item, 1).Text))

                        txtTotalSharePercentage.Text = CStr(CDbl(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatLong, txtTotalSharePercentage.Text)) - dShare)
                        txtTotalNewShareValue.Text = CStr(CDec(txtTotalNewShareValue.Text) - (dShare / 100) * CDec(txtTotalCurrentShareValue.Text))

                        m_lReturn = g_oBusiness.editdelete(Convert.ToString(lstView.FocusedItem.Tag))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                            Exit Sub
                        End If
                        lstView.Items.RemoveAt(lstView.FocusedItem.Index)
                    End If
            End Select

        Catch excep As System.Exception


            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMCoinsuranceRecoveries.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Select the first otem in the List View
            'NIIT- As no items in the list at time of initialization
            'lstView.Items.Item(0).Selected = True

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

            ' Set the process modes for the busines object.
            '    m_lReturn& = g_oBusiness.SetProcessModes( _
            ''        vTask:=CVar(m_iTask%), _
            ''        vNavigate:=CVar(m_lNavigate&), _
            ''        vProcessMode:=CVar(m_lProcessMode&), _
            ''        vTransactionType:=CVar(m_sTransactionType$), _
            ''        vEffectiveDate:=CVar(m_dtEffectiveDate))
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> pmtrue) Then
            '        ' Failed to process the interface.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error Message
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to set the process modes for the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_Load"
            '
            '        Exit Sub
            '    End If

            ' Set the business keys.

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub

            End If

            ' Set the mouse pointer to normal.

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'developer guide no.303
            'm_lReturn = CType(SetExtraListViewProperties(lstView.Handle.ToInt32(), 1), gPMConstants.PMEReturnCode)
            lstView.FullRowSelect = True

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

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub

                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object
            '    m_lReturn& = g_oBusiness.Terminate()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> pmtrue) Then
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to terminate the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_QueryUnload"
            '    End If

            ' Destroy the instance of the business object
            ' from memory.
            'Set g_oBusiness = Nothing

            ' Terminate the form control object.
		m_oFormFields.Dispose()

            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

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

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With

            'developer guide no.293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub cmdOk_Click()
        Dim sTitle, sMessage As String
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            If CDbl(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatLong, txtTotalSharePercentage.Text)) > 100 Or CDbl(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatLong, txtTotalSharePercentage.Text)) < 100 Then
                ' raise error saying that Total Share % can't be greater than 100

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShareTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShareTotal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Update the CoInsurance treatment values
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                If cmbBox.SelectedIndex <> -1 Then

                    m_lReturn = g_oBusiness.UpdateTreatment(m_lClaimID, VB6.GetItemData(cmbBox, cmbBox.SelectedIndex))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                End If
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            lBusinessDataID = 0
            ' For ivar = 1 To lstView.ListItems.Count
            lBusinessDataID += 1
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If
            ' Next ivar

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click()

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

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


            m_lReturn = oPMLock.UnLockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=g_oObjectManager.UserID)

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

            oPMLock = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
