Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 05/05/1999
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_sStepStatus As String = ""
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_lDataModelType As Integer
	Private m_lGISObjectId As Integer
	Private m_lGISDataModelId As Integer
	Private m_sGISDataModel As String = ""
	Private m_sObjectName As String = ""
	Private m_sTableName As String = ""
	Private m_lMaxInstances As Object
	Private m_lIsQuoteObject As Integer
	Private m_lParentObjectId As String = ""
	Private m_lParentObjectType As Integer
	Private m_SQLServerVersion As Integer
    Private m_lPolarisObjectId As Object
	Private m_lIsSelectableForScreen As Integer
	Private m_lObjectType As Integer 'this now holds the object type
	Private m_lEditFlags As Integer
	
	Private m_sParentCode As String = ""
	Private m_vAllowedParents( ,  ) As Object
	
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iGISObject.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	'Private m_oBusiness As bSIRIPTExtras.Business
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues As Object
	Private m_vLookupDetails As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
    ' developer guide no. 7
    Private Const vbFormCode As Integer = 0
	' PUBLIC Property Procedures (Begin)
	Public WriteOnly Property SQLServerVersion() As Integer
		Set(ByVal Value As Integer)
			m_SQLServerVersion = Value
		End Set
	End Property
	
	Public Property DataModelType() As Integer
		Get
			Return m_lDataModelType
		End Get
		Set(ByVal Value As Integer)
			m_lDataModelType = Value
		End Set
	End Property
	
	Public Property GISObjectId() As Integer
		Get
			Return m_lGISObjectId
		End Get
		Set(ByVal Value As Integer)
			m_lGISObjectId = Value
		End Set
	End Property
	
	Public Property GISDataModelId() As Integer
		Get
			Return m_lGISDataModelId
		End Get
		Set(ByVal Value As Integer)
			m_lGISDataModelId = Value
		End Set
	End Property
	
	Public Property GISDataModel() As String
		Get
			Return m_sGISDataModel
		End Get
		Set(ByVal Value As String)
			m_sGISDataModel = Value
		End Set
	End Property
	
	Public Property ObjectName() As String
		Get
			Return m_sObjectName
		End Get
		Set(ByVal Value As String)
			m_sObjectName = Value
		End Set
	End Property
	
	Public Property TableName() As String
		Get
			Return m_sTableName
		End Get
		Set(ByVal Value As String)
			m_sTableName = Value
		End Set
	End Property
	
	Public Property MaxInstances() As Object
		Get
			Return m_lMaxInstances
		End Get
		Set(ByVal Value As Object)


			m_lMaxInstances = Value
		End Set
	End Property
	
	Public Property IsQuoteObject() As Integer
		Get
			Return m_lIsQuoteObject
		End Get
		Set(ByVal Value As Integer)
			m_lIsQuoteObject = Value
		End Set
	End Property
	
	Public Property ParentObjectId() As String
		Get
			Return m_lParentObjectId
		End Get
		Set(ByVal Value As String)

			m_lParentObjectId = CStr(Value)
		End Set
	End Property
	
    Public Property PolarisObjectId() As Object
        Get
            Return m_lPolarisObjectId
        End Get
        Set(ByVal Value As Object)

            m_lPolarisObjectId = (Value)
        End Set
    End Property
	
	Public Property IsSelectableForScreen() As Integer
		Get
			Return m_lIsSelectableForScreen
		End Get
		Set(ByVal Value As Integer)

			m_lIsSelectableForScreen = CInt(Value)
		End Set
	End Property
	
	Public Property ObjectType() As Integer
		Get
			Return m_lObjectType
		End Get
		Set(ByVal Value As Integer)

			m_lObjectType = CInt(Value)
		End Set
	End Property
	
	Public Property EditFlags() As Integer
		Get
			Return m_lEditFlags
		End Get
		Set(ByVal Value As Integer)
			m_lEditFlags = Value
		End Set
	End Property
	
	Public Property ParentCode() As String
		Get
			Return m_sParentCode
		End Get
		Set(ByVal Value As String)
			m_sParentCode = Value
		End Set
	End Property
    ' developer guide no. 33
    Public WriteOnly Property AllowedParents() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vAllowedParents = Value
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
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	' PRIVATE Property Procedures (Begin)
	
    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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
	
	
	Public Property StepStatus() As String
		Get
			
			Return m_sStepStatus
			
		End Get
		Set(ByVal Value As String)
			
			m_sStepStatus = Value
			
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	' PRIVATE Property Procedures (End)
	
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
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign all of the controls to
			' PMFormControl
			'
			' Example:-
			'
			'        ' Pass control and required settings to FormControl
			'        m_lReturn = m_oFormFields.AddNewFormField( _
			''                       ctlControl:=<Control Name>, _
			''                       lFieldType:=<PM field type>, _
			''                       lFormat:=<PM format string>, _
			''                       lMandatory:=<PMMandatory or PMNonMandatory)
			'
			'        'Error checking
			'        If m_lReturn <> PMTrue Then
			'          SetFieldValidation = PMFalse
			'          Exit Function
			'        End If
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtObjectName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTableName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtMaxInstances, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'    m_lReturn& = m_oFormFields.AddNewFormField( _
			'ctlControl:=cboParent, _
			'lFieldType:=PMString, _
			'lFormat:=PMFormatString, _
			'lMandatory:=PMNonMandatory)
			
			'    If (m_lReturn& <> PMTrue) Then
			'        SetFieldValidation = PMFalse
			'        Exit Function
			'    End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPolarisObjectId, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' {* USER DEFINED CODE (End) *}
			
			
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
			
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			
			'    m_lReturn& = m_oBusiness.GetDetails(vExtras:=m_vExtras)
			
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors
			'    If (m_lReturn& <> PMTrue) Then
			'        ' Failed to get details.
			'        GetBusiness = PMFalse
			'
			'        ' Log Error.
			'        LogMessage _
			''            iType:=PMLogError, _
			''            sMsg:="Failed to get details from the business object", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="GetBusiness"
			'
			'        Exit Function
			'    End If
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
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
		Dim bClaimInModel, bPerilInModel, bTermsInModel, bParentIsBinder, bParentIsOutput As Boolean
		Dim lIndex As Integer
		
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
			
			' Assign the details to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign the all of the interface
			' details from the business object, using the FormatField
			' function for any type conversion.
			'
			' Example:-
			'
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_dtDDate)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtObjectName, vControlValue:=m_sObjectName)
			If m_sObjectName = m_sGISDataModel & "_Output" Then
				txtObjectName.Enabled = False
			End If
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTableName, vControlValue:=m_sTableName)
			

			If Convert.IsDBNull(m_lMaxInstances) Or IsNothing(m_lMaxInstances) Then
				txtMaxInstances.Text = ""
			Else
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtMaxInstances, vControlValue:=m_lMaxInstances)
			End If
			

			If Convert.IsDBNull(m_lPolarisObjectId) Or IsNothing(m_lPolarisObjectId) Then
				txtPolarisObjectId.Text = ""
			Else
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPolarisObjectId, vControlValue:=m_lPolarisObjectId)
			End If
			
			If m_lIsQuoteObject = gPMConstants.PMEReturnCode.PMTrue Then
				chkIsQuoteObject.CheckState = CheckState.Checked
			Else
				chkIsQuoteObject.CheckState = CheckState.Unchecked
			End If
			

			If Convert.IsDBNull(m_lIsSelectableForScreen) Or IsNothing(m_lIsSelectableForScreen) Then
				chkIsSelectableForScreen.CheckState = CheckState.Unchecked
			Else
				If m_lIsSelectableForScreen = gPMConstants.PMEReturnCode.PMFalse Then
					chkIsSelectableForScreen.CheckState = CheckState.Unchecked
				Else
					chkIsSelectableForScreen.CheckState = CheckState.Checked
				End If
			End If
			
			Dim lTmpParentID, lTmpParentType As Integer
			
			cboParent.Items.Clear()
			
			'  cboParent.AddItem "None"
			lIndex = 0
			m_lParentObjectType = -1 'Invalidate
			
			If Information.IsArray(m_vAllowedParents) Then
				'The first one in the array is the policy binder...
				For ltemp As Integer = m_vAllowedParents.GetLowerBound(1) To m_vAllowedParents.GetUpperBound(1)
					'        For ltemp = LBound(m_vAllowedParents, 2) + 1 To UBound(m_vAllowedParents, 2)
					'Can only add if this one's id is not -1
					If CDbl(m_vAllowedParents(ACOGISObjectId, ltemp)) <> -1 Then
						Dim cboParent_NewIndex As Integer = -1
						cboParent_NewIndex = cboParent.Items.Add(CStr(m_vAllowedParents(ACOTableName, ltemp)))
						
						lTmpParentID = CInt(m_vAllowedParents(ACOGISObjectId, ltemp))
						lTmpParentType = CInt(m_vAllowedParents(ACOObjectType, ltemp))
						VB6.SetItemData(cboParent, cboParent_NewIndex, lTmpParentID)
						
						'Are any of the following in the model
						If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
							'bAssociatedClientInModel = bAssociatedClientInModel Or (lTmpParentType = GISOTAssociatedClient)
							'bDisclosureInModel = bDisclosureInModel Or (lTmpParentType = GISOTDisclosure)
							bClaimInModel = bClaimInModel Or (lTmpParentType = GISDataModelType.GISOTClaim)
							bPerilInModel = bPerilInModel Or (lTmpParentType = GISDataModelType.GISOTPeril)
							' PW080503 - Terms needs to be a standard object
							' (ENDVR00000841)
							bTermsInModel = bTermsInModel Or (CStr(m_vAllowedParents(ACOObjectName, ltemp)) = "Terms")
						End If
						

						If Not (Convert.IsDBNull(m_lParentObjectId) Or IsNothing(m_lParentObjectId)) Then
							If lTmpParentID = StringsHelper.ToDoubleSafe(m_lParentObjectId) Then
								bParentIsBinder = m_vAllowedParents(ACOObjectName, ltemp).ToUpper().EndsWith("_BINDER")
								bParentIsOutput = m_vAllowedParents(ACOObjectName, ltemp).ToUpper().EndsWith("_OUTPUT")
								m_lParentObjectType = lTmpParentType
								lIndex = cboParent_NewIndex
							End If
						End If
					End If
				Next ltemp
			End If
			
			cboParent.SelectedIndex = lIndex
			
			lIndex = 0
			cboObjectType.Items.Clear()
            Dim cboObjectType_NewIndex As Integer = -1
			If DataModelType = GISDataModelType.GISDMTypeRisk Then
				
				If m_lParentObjectType = GISDataModelType.GISOTRisk Then
                    'Modified by Archana Tokas on 31/03/2010 11:55:10 declared out of if
                    'Dim cboObjectType_NewIndex As Integer = -1
					cboObjectType_NewIndex = cboObjectType.Items.Add("Risk")
					VB6.SetItemData(cboObjectType, cboObjectType_NewIndex, GISDataModelType.GISOTRisk)
					If m_lObjectType = GISDataModelType.GISOTRisk Then
						lIndex = cboObjectType_NewIndex
					End If
					
					If bParentIsBinder Then
						
						cboObjectType_NewIndex = cboObjectType.Items.Add("Non GIS Specials")
						VB6.SetItemData(cboObjectType, cboObjectType_NewIndex, GISDataModelType.GISOTNonGisSpecials)
						If m_lObjectType = GISDataModelType.GISOTNonGisSpecials Then
							lIndex = cboObjectType_NewIndex
						End If
					End If
					
					If bParentIsOutput And Not bTermsInModel Then
						' PW110303 - add new terms object type
						' PSCR22
						cboObjectType_NewIndex = cboObjectType.Items.Add("Terms")
						' PW080503 - Terms needs to be a standard object
						' (ENDVR00000841)
						VB6.SetItemData(cboObjectType, cboObjectType_NewIndex, GISDataModelType.GISOTRisk)
						If m_sObjectName = "Terms" Then
							lIndex = cboObjectType_NewIndex
						End If
					End If
					
				End If
				
			ElseIf DataModelType = GISDataModelType.GISDMTypeClaim And m_SQLServerVersion >= 8 Then 
				'RVH 14/04/2003 -   CQ Issue ENDVR00000423 - START : Add default type as "RISK".
				'                   Without an item in the combo, the user is not able to add
				'                   objects at the same level as the claim. As the user is prevented
				'                   from adding child objects below Claim and Claim_Peril, they need
				'                   an object that that CAN hang child objects off.
				
				' MEvans : 14-04-2003 : CQ 423
				' Should not allow any risk objects to be added under the
				' claims branch...
				If m_lParentObjectType = GISDataModelType.GISOTRisk Then
					cboObjectType_NewIndex = cboObjectType.Items.Add("Risk")
					VB6.SetItemData(cboObjectType, cboObjectType_NewIndex, GISDataModelType.GISOTRisk)
					If m_lObjectType = GISDataModelType.GISOTRisk Then
						lIndex = cboObjectType_NewIndex
					End If
				End If
				'RVH 14/04/2003 -   CQ Issue ENDVR00000423 - END
				
				If m_lParentObjectType = GISDataModelType.GISOTRisk And Not bClaimInModel And bParentIsBinder Then
					cboObjectType_NewIndex = cboObjectType.Items.Add("Claim")
					VB6.SetItemData(cboObjectType, cboObjectType_NewIndex, GISDataModelType.GISOTClaim)
					If m_lObjectType = GISDataModelType.GISOTClaim Then
						lIndex = cboObjectType_NewIndex
					End If
				End If
				
				If m_lParentObjectType = GISDataModelType.GISOTClaim And Not bPerilInModel Then
					cboObjectType_NewIndex = cboObjectType.Items.Add("Claim_Peril")
					VB6.SetItemData(cboObjectType, cboObjectType_NewIndex, GISDataModelType.GISOTPeril)
					If m_lObjectType = GISDataModelType.GISOTPeril Then
						lIndex = cboObjectType_NewIndex
					End If
				End If
				
				'22/04/2003 - PWC - (408) User Definable Fields
			ElseIf DataModelType = GISDataModelType.GISDMTypeParty Then 
				
				'AK 270603 - this datamodel type does not need a special object type,
				'            can use the default 'Risk' type
				cboObjectType_NewIndex = cboObjectType.Items.Add("Risk")
				VB6.SetItemData(cboObjectType, cboObjectType_NewIndex, GISDataModelType.GISOTRisk)
				If m_lObjectType = GISDataModelType.GISOTRisk Then
					lIndex = cboObjectType_NewIndex
				End If
				
				'        cboObjectType.AddItem "Party"
				'        cboObjectType.ItemData(cboObjectType.NewIndex) = GISOTParty
				'        'If (m_lObjectType = GISOTRisk) Then
				'            lIndex = cboObjectType.NewIndex
				'        'End If
			ElseIf DataModelType = GISDataModelType.GISDMTypeCase Then 
				
				'can use the default 'Case' type
				cboObjectType_NewIndex = cboObjectType.Items.Add("Case")
				VB6.SetItemData(cboObjectType, cboObjectType_NewIndex, GISDataModelType.GISOTCase)
				If m_lObjectType = GISDataModelType.GISOTCase Then
					lIndex = cboObjectType_NewIndex
				End If
			Else
				MessageBox.Show("Unknown Data Model type.", "GIS Object", MessageBoxButtons.OK, MessageBoxIcon.Error)
			End If
			
			If cboObjectType.Items.Count = 0 Then
				MessageBox.Show("Cannot add additional objects to: " & cboParent.Text, "GIS Object", MessageBoxButtons.OK, MessageBoxIcon.Error)
				result = gPMConstants.PMEReturnCode.PMFalse
			Else
				cboObjectType.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMAdd)
				cboObjectType.SelectedIndex = lIndex
				cboObjectType_Leave(cboObjectType, New EventArgs())
			End If
			
			If m_iTask = gPMConstants.PMEComponentAction.PMView Then
				m_lReturn = CType(DisableForm(lDisabled:=True), gPMConstants.PMEReturnCode)
			End If
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result

            ''Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
			
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
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	'
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			
			' Assign the details to the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
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
		Dim sTemp As String = ""

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign all of the details from the
			' interface to the data storage.
			'
			' Example:-
			'
			'    m_DName$ = trim$(txtName.Text)
			'    m_DDate = CDate(txtDescription.Text)
			'    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
			'    m_lReturn& = m_oFormFields.UnformatControl(txtName)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			

			m_sObjectName = CStr(m_oFormFields.UnformatControl(txtObjectName))

			m_sTableName = CStr(m_oFormFields.UnformatControl(txtTableName))


			m_lMaxInstances = m_oFormFields.UnformatControl(txtMaxInstances)
			

            m_lPolarisObjectId = (m_oFormFields.UnformatControl(txtPolarisObjectId))
			If m_lPolarisObjectId = 0 Then

				m_lPolarisObjectId = Nothing
			End If
			
			m_lIsQuoteObject = chkIsQuoteObject.CheckState
			m_lIsSelectableForScreen = chkIsSelectableForScreen.CheckState
			m_lObjectType = VB6.GetItemData(cboObjectType, cboObjectType.SelectedIndex)
			
			If cboParent.SelectedIndex < 1 Then
				m_lParentObjectId = CStr(m_vAllowedParents(ACOGISObjectId, 0))
				m_sParentCode = ""
			Else
				'Oops, now we're not showing the binder...
				'        m_lParentObjectId = m_vAllowedParents(0, cboParent.ListIndex - 1)
				m_lParentObjectId = CStr(m_vAllowedParents(0, cboParent.SelectedIndex))
				m_sParentCode = cboParent.Text
			End If
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values.
	'
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Center the interface.
			iPMFunc.CenterForm(Me)
			
			' Display all language specific captions.
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the status of the Navigate button.
			Select Case (m_lNavigate)
				Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
					cmdNavigate.Visible = True
					cmdNavigate.Enabled = True
					
				Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
					cmdNavigate.Visible = True
					cmdNavigate.Enabled = False
					
				Case Else
					cmdNavigate.Visible = False
			End Select
			
			m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			
			'    If (m_iTask = PMAdd) Then
			'        txtObjectName.Enabled = True
			'        txtTableName.Enabled = True
			'        cboParent.Enabled = True
			'    End If
			
			'    chkIsQuoteObject.Enabled = False
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			ReDim m_ctlTabFirstLast(1, 0)
			
			' Set the first and last data entry controls for
			' all of the tabs.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to set the first and last data entry
			' controls for all of the tabs.
			'
			' Example:-
			'
			'    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
			'    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			m_ctlTabFirstLast(ACControlStart, 0) = txtObjectName
			m_ctlTabFirstLast(ACControlEnd, 0) = cboObjectType
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
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
			

            ' developer guide no. 243 starts
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

			SSTabHelper.SetTabEnabled(tabMainTab, 0, True)
			SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to display all language specific
			' captions.
			' The GetResData function will allow you to do this.
			'
			' Example:-
			'
			'    lblDesc.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACDesc, _
			''        iDataType:=PMResString)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			

            lblObjectName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionObjectName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblTableName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionTableName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblMaxInstances.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionMaxInstances, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            chkIsQuoteObject.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionIsQuoteObject, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblParent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionParent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblPolarisObjectId.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionPolarisObject, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            chkIsSelectableForScreen.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionIsSelectableForScreen, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblObjectType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionObjectType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: ValidateForm
	'
	' Description:
	'
	' History: 07/09/1999 Tomo - Created.
	'
	' ***************************************************************** '
	Private Function ValidateForm() As Integer
		
		Dim result As Integer = 0
		Dim sTemp, sTemp2, sComp As String
		Dim vReservedKeywords As Object
		Dim PBSqlReservedKeywords As PBSqlReservedKeywords
		Dim sCode As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			PBSqlReservedKeywords = New PBSqlReservedKeywords()
			


			vReservedKeywords = PBSqlReservedKeywords.vSqlReservedKeywords
			
			sComp = PBSqlReservedKeywords.vInvalidCharacters
			
			sTemp = txtObjectName.Text.Trim().ToUpper()
			
			Dim dbNumericTemp As Double
			If Double.TryParse(sTemp.Substring(0, 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				MessageBox.Show("Cannot have numeric value as the first character of an object name", "GIS Object", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			For	Each sChar As Char In sComp
				If sTemp.IndexOf(sChar) + 1 Then
					MessageBox.Show("Cannot have '" & sChar & "' in the object name", "GIS Object", MessageBoxButtons.OK, MessageBoxIcon.Error)
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			Next sChar
			
			sTemp = txtTableName.Text.Trim().ToUpper()
			

			For ltemp As Integer = 0 To vReservedKeywords.GetUpperBound(0)

                If sTemp = CStr(vReservedKeywords(ltemp)) Then
                    MessageBox.Show("Cannot have '" & txtTableName.Text.Trim() & "' as the table name", "GIS Object", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
			Next 
			
			For	Each sChar As Char In sComp
				If sTemp.IndexOf(sChar) + 1 Then
					MessageBox.Show("Cannot have '" & sChar & "' in the table name", "GIS Object", MessageBoxButtons.OK, MessageBoxIcon.Error)
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			Next sChar
			
			'Need to check that the object and table names haven't already been used
			
			sTemp = txtTableName.Text.Trim().ToUpper()
			sTemp2 = txtObjectName.Text.Trim().ToUpper()
			
			If Information.IsArray(m_vAllowedParents) Then
				For ltemp As Integer = m_vAllowedParents.GetLowerBound(1) To m_vAllowedParents.GetUpperBound(1)
					
					If sTemp2 = CStr(m_vAllowedParents(ACOObjectName, ltemp)).ToUpper() Then
						If sTemp2 <> m_sObjectName.ToUpper() Then
							MessageBox.Show("This object name is already used", "GIS Object", MessageBoxButtons.OK, MessageBoxIcon.Error)
							Return gPMConstants.PMEReturnCode.PMFalse
						End If
					End If
					
					If sTemp = CStr(m_vAllowedParents(ACOTableName, ltemp)).ToUpper() Then
						If sTemp <> m_sTableName.ToUpper() Then
							MessageBox.Show("This table name is already used", "GIS Object", MessageBoxButtons.OK, MessageBoxIcon.Error)
							Return gPMConstants.PMEReturnCode.PMFalse
						End If
					End If
					
				Next ltemp
			End If
			
			PBSqlReservedKeywords = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cboObjectType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboObjectType.SelectedIndexChanged
		txtObjectName.Enabled = True
		
	End Sub
	
	Private Sub cboObjectType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboObjectType.Leave
		
		cmdOK.Enabled = True

		Dim sParent As String = ""
		
		Select Case VB6.GetItemData(cboObjectType, cboObjectType.SelectedIndex)
			Case GISDataModelType.GISOTRisk
				txtObjectName.Enabled = True
				' PW080503 - Terms needs to be a standard object (ENDVR00000841)
				If cboObjectType.Text = "Terms" Then
					txtObjectName.Text = "Terms"
					txtObjectName.Enabled = False
				End If
				
			Case GISDataModelType.GISOTClaim
				txtObjectName.Text = "Work_Claim"
				txtObjectName.Enabled = False
				
			Case GISDataModelType.GISOTPeril
				txtObjectName.Text = "Work_Claim_Peril"
				sParent = "_Work_Claim"
				txtTableName.Enabled = False
				
		End Select
		
		txtObjectName_Leave(txtObjectName, New EventArgs())
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
        ' Forms initialise event.
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
			'    m_lReturn& = g_oObjectManager.GetInstance( _
			'oObject:=m_oBusiness, _
			'sClassName:="bSIRIPTExtras.Business", _
			'vInstanceManager:=PMGetViaClientManager)
			
			' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        ' Failed to get an instance of the business object.
			'        m_lErrorNumber& = PMFalse
			'
			'        ' Display error stating the problem.
			'
			'        ' Get description from the resource file.
			'        sTitle$ = iPMFunc.GetResData( _
			''            iLangID:=g_iLanguageID%, _
			''            lID:=ACBusinessFailTitle, _
			''            iDataType:=PMResString)
			'
			'        sMessage$ = iPMFunc.GetResData( _
			''            iLangID:=g_iLanguageID%, _
			''            lID:=ACBusinessFail, _
			''            iDataType:=PMResString)
			'
			'        ' Display message.
			'        MsgBox sMessage$, vbCritical, sTitle$
			'
			'        Exit Sub
			'    End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New iGISObject.General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
			
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
        'Try

        '    ' Check if we have had an error so far.
        '    ' Possibly creating the business object.
        '    If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
        '        ' We have already encountered an error,
        '        ' so we MUST exit now.
        '        Exit Sub
        '    End If

        '    ' Set the mouse pointer to busy.
        '    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        '    ' Set the process modes for the busines object.
        '    '    m_lReturn& = m_oBusiness.SetProcessModes( _
        '    'vTask:=CVar(m_iTask%), _
        '    'vNavigate:=CVar(m_lNavigate&), _
        '    'vProcessMode:=CVar(m_lProcessMode&), _
        '    'vTransactionType:=CVar(m_sTransactionType$), _
        '    'vEffectiveDate:=CVar(m_dtEffectiveDate))

        '    ' Check for errors.
        '    '    If (m_lReturn& <> PMTrue) Then
        '    '        ' Failed to process the interface.
        '    '        m_lErrorNumber& = PMFalse
        '    '
        '    '        ' Log Error Message
        '    '        LogMessage _
        '    ''            iType:=PMLogOnError, _
        '    ''            sMsg:="Failed to set the process modes for the business object", _
        '    ''            vApp:=ACApp, _
        '    ''            vClass:=ACClass, _
        '    ''            vMethod:="Form_Load"
        '    '
        '    '        Exit Sub
        '    '    End If

        '    ' Set the business keys.
        '    ' {* USER DEFINED CODE (Begin) *}

        '    ' {* USER DEFINED CODE (End) *}

        '    ' Validate fields using Forms Control
        '    m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
        '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

        '        ' Set the mouse pointer to normal.
        '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        '        Exit Sub
        '    End If

        '    ' Set the interface default values.
        '    m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

        '    ' Check for errors.
        '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

        '        ' Set the mouse pointer to normal.
        '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        '        Exit Sub
        '    End If

        '    ' Gets the interface details to be displayed.
        '    m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

        '    ' Check for errors.
        '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '        ' Failed to get the interface details.
        '        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

        '        ' Set the mouse pointer to normal.
        '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        '        Exit Sub
        '    End If

        '    ' Set the mouse pointer to normal.
        '    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        'Catch excep As System.Exception




        '    ' Log Error.
        '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

        '    Exit Sub

        'End Try
		
		
    End Sub

    Public Function InterfaceLoad() As Integer
        Dim m_return As Integer = 1
        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                m_return = m_lErrorNumber
                Return m_return
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                m_return = m_lErrorNumber
                Return m_return
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                m_return = m_lErrorNumber
                Return m_return
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                m_return = m_lErrorNumber
                Return m_return
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return m_return
        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try
    End Function
	
	Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Forms query unload event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			'<PN:39897>

            ' Set the interface status.
            If m_lStatus = Nothing Then
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            End If
            '</PN:39897>
			
			' Check if the interface has been terminated by means
			' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    ' developer guide no. 7
                    eventArgs.Cancel = True
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

            '    ' Terminate the business object
            '    m_lReturn& = m_oBusiness.Terminate()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
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
            '
            '    ' Destroy the instance of the business object
            '    ' from memory.
            '    Set m_oBusiness = Nothing

            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            Erase m_ctlTabFirstLast

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

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
		Catch 
			
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
		'Try 
			'
			'    With tabMainTab
			'        ' Set the default button.
			'        If (.Tab < cmdNext.Count) Then
			'            cmdNext(.Tab).Default = True
			'        Else
			'            cmdOK.Default = True
			'        End If
			''
			'        ' Now I know this is crap, this goes against
			'        ' all my principles, but for some reason when
			'        ' using the mouse to select a tab the setfocus
			'        ' code below doesn't work. The cursor sticks,
			'        ' and you can't tab off. Therefore I've used
			'        ' this to get around the problem.
			'        DoEvents
			''
			'        ' Set focus to the first control on the tab.
			'        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
			'            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
			'        End If
			'    End With
		'
		'Catch 
			'
			'
			'
			'
			'
			'tabMainTabPreviousTab = tabMainTab.SelectedIndex
		'End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
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
			
			' validate data entered.
			m_lReturn = CType(ValidateForm(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
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
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
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
	
	Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
		
		' Click event of the Navigate button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMNavigate
			
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
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub txtMaxInstances_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMaxInstances.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMaxInstances)
	End Sub
	
	Private Sub txtMaxInstances_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtMaxInstances.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		'<Pankaj PN:39367> Max Instances should accept numeric values only
		If KeyAscii <> 8 Then
			If Not (KeyAscii > 47 And KeyAscii < 58) Then
				KeyAscii = 0
			End If
		End If
		'</Pankaj PN:39367>
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	Private Sub txtMaxInstances_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMaxInstances.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMaxInstances)
	End Sub
	
	Private Sub txtObjectName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtObjectName.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtObjectName)
		
	End Sub
	
	Private Sub txtObjectName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtObjectName.Leave
		Dim sCode As String = ""
		
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtObjectName)
		
		If txtObjectName.Text = "" Then
			txtTableName.Text = ""
		Else
			'MSB030902 - Commented out as the table name needs to be changed each time the user changes the object name
			'        If (txtTableName.Text = "") Then
			' Only change the table name when adding a new object, not when you are editing
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				
				If VB6.GetItemData(cboObjectType, cboObjectType.SelectedIndex) = GISDataModelType.GISOTClaim Or VB6.GetItemData(cboObjectType, cboObjectType.SelectedIndex) = GISDataModelType.GISOTPeril Then
					txtTableName.Text = GISDataModel & "_" & txtObjectName.Text.Substring(0).Replace("Work_Claim", "Claim")
				Else
					txtTableName.Text = GISDataModel & "_" & txtObjectName.Text
                End If

                Dim iCodeLen As Integer
                iCodeLen = System.Math.Min(Strings.Len(GISDataModel) + 1, Strings.Len(txtObjectName.Text.ToString()))

                'sCode = txtObjectName.Text.Substring(0, Strings.Len(GISDataModel) + 1)
                sCode = txtObjectName.Text.Substring(0, iCodeLen)
				If sCode.ToUpper() = GISDataModel.ToUpper() & "_" Then
					MessageBox.Show("Data model code not allowed as prefix of object name", "Object Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
					'Strip off the data model code
					txtObjectName.Text = Mid(txtObjectName.Text, Strings.Len(txtObjectName.Text) - (Strings.Len(txtObjectName.Text) - (Strings.Len(GISDataModel) + 1)) + 1, Strings.Len(txtObjectName.Text) - (Strings.Len(GISDataModel) + 1))
					txtTableName.Text = GISDataModel & "_" & txtObjectName.Text
				End If
			End If
			'        End If
		End If
		
	End Sub
	
	Private Sub txtPolarisObjectId_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolarisObjectId.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPolarisObjectId)
	End Sub
	
	Private Sub txtPolarisObjectId_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolarisObjectId.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPolarisObjectId)
	End Sub
	
	Private Sub txtTableName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTableName.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtTableName)
	End Sub
	
	Private Sub txtTableName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTableName.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTableName)
	End Sub
	
	' PRIVATE Events (End)
	Private Function DisableForm(ByRef lDisabled As Integer) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set all of the forms controls to the disable state.

			For	Each ctlFormControl As Control In ContainerHelper.Controls(Me)
				' Check the type of the control.
				If TypeOf ctlFormControl Is TextBox Then
					ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				ElseIf (TypeOf ctlFormControl Is ComboBox) Then 
					ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				ElseIf (TypeOf ctlFormControl Is CheckBox) Then 
					ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				ElseIf (TypeOf ctlFormControl Is RadioButton) Then 
					ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				ElseIf (TypeOf ctlFormControl Is RadioButton) Then 
					ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				End If
			Next ctlFormControl
			
			cmdOK.Visible = Not lDisabled
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Class
