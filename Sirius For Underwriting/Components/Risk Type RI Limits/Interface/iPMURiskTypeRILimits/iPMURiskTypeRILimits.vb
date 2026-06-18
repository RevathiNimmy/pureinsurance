Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 09/06/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '

    'Developer Guide No.7
    Public Const vbFormCode As Integer = 0
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

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lRiskTypeId As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_lItemsFound As Integer

    'Variables to store data taken from the List View
    Private m_iAction As gPMConstants.PMEComponentAction
    Private m_lRiskTypeRILimitsId As Integer
    Private m_lGISObjectId As Integer
    Private m_lGISPropertyId As Integer
    Private m_sGISObjectName As String = ""
    Private m_sGISPropertyName As String = ""
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURiskTypeRILimits.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRRiskTypeRILimits.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_bChanged As Boolean


    Private m_oRILimits As iPMURiskTypeRIValues.Interface_Renamed

    ' Control array to store the first and last
    ' text box controls for each tab.
    ' Stores the search data from the business object.
    Private m_vRiskTypeRILimits(,) As Object
    Private m_vAllowedProperties(,) As Object
    Private m_dtLimitEffectiveDate As Date
    Private m_dtLimitExpiryDate As Date
    Private m_sLimitDescription As String
    Private m_nRiskTypeLimitVersionId As Integer
    Private m_iMode As Short
    Private m_iItemStatus As Short
    'JMK 22/10/2001 display Insurer/Reinsurer
    Private m_sUnderwritingType As String = ""
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

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
    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property RiskTypeId() As Integer
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    Public WriteOnly Property Description() As String
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property

    Public Property LimitEffectiveDate() As Date
        Get
            Return m_dtLimitEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtLimitEffectiveDate = Value
        End Set
    End Property

    Public Property LimitExpiryDate() As Date
        Get
            Return m_dtLimitExpiryDate
        End Get
        Set(ByVal Value As Date)
            m_dtLimitExpiryDate = Value
        End Set
    End Property

    Public Property LimitDescription() As String
        Get
            Return m_sLimitDescription
        End Get
        Set(ByVal Value As String)
            m_sLimitDescription = Value
        End Set
    End Property

    Public Property RiskTypeLimitVersionId() As Integer
        Get
            Return m_nRiskTypeLimitVersionId
        End Get
        Set(ByVal Value As Integer)
            m_nRiskTypeLimitVersionId = Value
        End Set
    End Property
    Public Property Mode() As Short
        Get
            Return m_iMode
        End Get
        Set(ByVal Value As Short)
            m_iMode = Value
        End Set
    End Property
    Public Property ItemStatus() As Short
        Get
            Return m_iItemStatus
        End Get
        Set(ByVal Value As Short)
            m_iItemStatus = Value
        End Set
    End Property
    Public Property RiskTypeRILimits() As Object
        Get
            Return m_vRiskTypeRILimits
        End Get
        Set(ByVal Value As Object)
            m_vRiskTypeRILimits = Value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
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
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExpiryDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DataToInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DataToInterface() As Integer
    '
    'Dim result As Integer = 0
    'Dim oListItem As ListViewItem
    'Dim bMatch As Boolean
    'Dim cThisPerc As Decimal
    '
    'Const ACFindImage As String = "FindImage"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Update the interface details.
    '
    ' Clear the search details.
    'lvwRiskTypeRILimits.Items.Clear()
    '
    'm_lItemsFound = 0
    '
    'If Not Information.IsArray(m_vRiskTypeRILimits) Then
    'Return result
    'End If
    '
    ' Assign the details to the interface.
    '
    'For 'lRow As Integer = m_vRiskTypeRILimits.GetLowerBound(1) To m_vRiskTypeRILimits.GetUpperBound(1)
    'If CDbl(m_vRiskTypeRILimits(ACRGISPropertyId, lRow)) <> 0 Then
    ' {* USER DEFINED CODE (Begin) *}
    'm_lItemsFound += 1
    ' Assign the details to the first column.
    ' Column 1 treaty description

    'oListItem = lvwRiskTypeRILimits.Items.Add(CStr(m_vRiskTypeRILimits(ACRGISPropertyName, lRow)).Trim(), "")
    '
    ' Assign details to the other columns
    '
    'Column 2 method
    '
    '            oListItem.SubItems(1) = m_vRiskTypeRILimits(ACRMethod, lRow)
    '
    ' {* USER DEFINED CODE (End) *}
    '
    ' Set the tag property with the index of
    ' the search data storage.
    'oListItem.Tag = CStr(lRow)
    '
    ' Refresh the first X amount of rows, to
    ' allow the user to see the results instantly.
    'If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
    ' Select the first item.
    'lvwRiskTypeRILimits.Items.Item(0).Selected = True
    '
    ' Refresh the initial results.
    'lvwRiskTypeRILimits.Refresh()
    'End If
    'End If
    'Next lRow
    '
    ' Enable the interface now that the search
    ' has completed.
    'm_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)
    '
    ' Check for errors
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to get details.
    'result = gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'cmdDelete.Enabled = False
    '
    'cmdAdd.Enabled = Not (Task = gPMConstants.PMEComponentAction.PMView)
    '
    'cmdOK.Enabled = True
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result


    '
    'Return result
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ClearDetail
    '
    ' Description: Clear RI Limit Details
    '              storage.
    '
    ' ***************************************************************** '
    Private Function ClearDetail() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the Detail details.

            ' {* USER DEFINED CODE (Begin) *}
            m_lGISPropertyId = 0
            m_sGISObjectName = ""
            m_sGISPropertyName = ""
            cboGISObject.SelectedIndex = -1
            cboGISProperty.SelectedIndex = -1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataRefresh
    '
    ' Description: Populate Risk Type RI Limits - Refresh
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataRefresh() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem, lGISPropertyId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}


            Select Case m_iAction
                Case gPMConstants.PMEComponentAction.PMAdd
                    m_lGISPropertyId = VB6.GetItemData(cboGISProperty, cboGISProperty.SelectedIndex)

                    m_sGISObjectName = cboGISObject.Text
                    m_sGISPropertyName = cboGISProperty.Text

                    If Information.IsArray(m_vRiskTypeRILimits) Then
                        lSelectedItem = m_vRiskTypeRILimits.GetUpperBound(1) + 1
                        ReDim Preserve m_vRiskTypeRILimits(ACRGISPropertyName, lSelectedItem)
                    Else
                        lSelectedItem = 0
                        ReDim m_vRiskTypeRILimits(ACRGISPropertyName, lSelectedItem)
                    End If

                    lGISPropertyId = m_lGISPropertyId

                    '    Case PMEdit
                    '        lSelectedItem& = _
                    ''            lvwRiskTypeRILimits.ListItems(lvwRiskTypeRILimits.SelectedItem.Index).Tag
                    '
                    '        lGISPropertyId = m_lGISPropertyId

                Case gPMConstants.PMEComponentAction.PMDelete

                    lSelectedItem = Convert.ToString(lvwRiskTypeRILimits.Items.Item(lvwRiskTypeRILimits.FocusedItem.Index).Tag)

                    lGISPropertyId = 0

            End Select

            m_vRiskTypeRILimits(ACRRiskTypeId, lSelectedItem) = m_lRiskTypeId
            m_vRiskTypeRILimits(ACRRILimitId, lSelectedItem) = lSelectedItem
            m_vRiskTypeRILimits(ACRGISPropertyId, lSelectedItem) = lGISPropertyId
            m_vRiskTypeRILimits(ACRGISPropertyName, lSelectedItem) = m_sGISPropertyName
            m_vRiskTypeRILimits(ACRGISObjectName, lSelectedItem) = m_sGISObjectName

            m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh Refreshs from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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

            ' {* USER DEFINED CODE (Begin) *}

            'Tell it what we're getting

            m_oBusiness.RiskTypeId = m_lRiskTypeId

            m_oBusiness.RiskTypeRILimitVersionId = m_nRiskTypeLimitVersionId

            m_lReturn = m_oBusiness.GetRiskTypeRILimits(r_vRiskTypeRILimits:=m_vRiskTypeRILimits)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If


            m_lReturn = m_oBusiness.GetAllowedProperties(r_vAllowedProperties:=m_vAllowedProperties)

            m_bChanged = False

            ' {* USER DEFINED CODE (End) *}

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
        Dim oListItem As ListViewItem
        Dim lSaveObject As Integer
        Static bAlreadyRun As Boolean

        Const ACFindImage As String = "FindImage"

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
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            'Developer Guide No. 26
            lblpRiskType.Text = m_sDescription

            txtDescription.Text = m_sLimitDescription
            txtEffectiveDate.Text = m_dtLimitEffectiveDate.ToShortDateString
            txtExpiryDate.Text = m_dtLimitExpiryDate.ToShortDateString

            lvwRiskTypeRILimits.Items.Clear()

            If Information.IsArray(m_vRiskTypeRILimits) Then
                For lTemp As Integer = m_vRiskTypeRILimits.GetLowerBound(1) To m_vRiskTypeRILimits.GetUpperBound(1)
                    If CInt(m_vRiskTypeRILimits(ACRGISPropertyId, lTemp)) <> 0 Then

                        'Developer Guide No.49
                        oListItem = lvwRiskTypeRILimits.Items.Add(CStr(m_vRiskTypeRILimits(ACRGISObjectName, lTemp)), ACFindImage)

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRiskTypeRILimits(ACRGISPropertyName, lTemp))

                        oListItem.Tag = CStr(lTemp)
                    End If
                Next lTemp

            End If

            If bAlreadyRun Then
                Return result
            End If

            bAlreadyRun = True

            cboGISObject.Items.Clear()
            cboGISProperty.Items.Clear()

            If Information.IsArray(m_vAllowedProperties) Then
                lSaveObject = 0
                For lTemp As Integer = m_vAllowedProperties.GetLowerBound(1) To m_vAllowedProperties.GetUpperBound(1)
                    If CDbl(m_vAllowedProperties(ACPGISObjectId, lTemp)) <> lSaveObject Then
                        Dim cboGISObject_NewIndex As Integer = -1
                        cboGISObject_NewIndex = cboGISObject.Items.Add(CStr(m_vAllowedProperties(ACPGISObjectName, lTemp)))
                        VB6.SetItemData(cboGISObject, cboGISObject_NewIndex, CInt(m_vAllowedProperties(ACPGISObjectId, lTemp)))
                        lSaveObject = CInt(m_vAllowedProperties(ACPGISObjectId, lTemp))
                    End If
                Next lTemp
            End If

            ' {* USER DEFINED CODE (End) *}

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
        Dim sDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not m_bChanged Then
                Return result
            End If

            ' Update the business object.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBusiness.RiskTypeId = m_lRiskTypeId

            m_oBusiness.RiskTypeRILimitVersionId = m_nRiskTypeLimitVersionId

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            m_sScreenHierarchy = m_sScreenHierarchy & $"/Limit({m_sDescription.Trim()})"
            m_lReturn = m_oBusiness.Update(v_vRiskTypeRILimits:=m_vRiskTypeRILimits, sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
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

            ' {* USER DEFINED CODE (End) *}

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

        Try


            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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
            'Display the ListView Tab

            tabDetailTab.Visible = False
            tabDetailTab.Top = VB6.TwipsToPixelsY(120)
            tabMainTab.Top = VB6.TwipsToPixelsY(120)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwRiskTypeRILimits.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the search list.
            lvwRiskTypeRILimits.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(lvwRiskTypeRILimits.Width) / 2) - 360))
            lvwRiskTypeRILimits.Columns.Item(1).Width = CInt(lvwRiskTypeRILimits.Columns.Item(0).Width)

            cmdDelete.Enabled = False

            cmdAdd.Enabled = Not (Task = gPMConstants.PMEComponentAction.PMView)

            If Mode = gPMConstants.PMEComponentAction.PMEdit Then
                cmdLimits.Enabled = True
            ElseIf Mode = gPMConstants.PMEComponentAction.PMAdd Then
                cmdLimits.Enabled = False
            End If


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


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).


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

            ' {* USER DEFINED CODE (End) *}

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

            'JMK 22/10/2001 - display Insurer/Reinsurer


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


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabDetailTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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



            lblRiskType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCRiskType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRiskTypeRILimits.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHObject, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRiskTypeRILimits.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHProperty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblGISObject.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCObject, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblGISProperty.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCProperty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
    ' Name: CallLimits
    '
    ' Description:
    '
    ' History: 30/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CallLimits() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create address component if not already done so
            If m_oRILimits Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oRILimits As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oRILimits, sClassName:="iPMURiskTypeRIValues.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oRILimits = temp_m_oRILimits

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get RI Values component", vApp:=ACApp, vClass:=ACClass, vMethod:="CallLimits", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If


            m_oRILimits.RiskTypeId = m_lRiskTypeId

            m_oRILimits.Description = m_sDescription

            m_oRILimits.MaxLevel = lvwRiskTypeRILimits.Items.Count

            m_oRILimits.ThisLevel = 1

            m_oRILimits.RiskTypeLimitVersionId = m_nRiskTypeLimitVersionId

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            m_sScreenHierarchy = $"Risk Type({m_sDescription.Trim()})/Limit({m_sDescription.Trim()})"
            m_oRILimits.ScreenHierarchy = m_sScreenHierarchy
            m_oRILimits.UniqueId = m_sUniqueId
            m_lReturn = m_oRILimits.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            If m_oRILimits.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallLimits Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallLimits", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)


    Private isInitializingComponent As Boolean
    Private Sub cboGISObject_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGISObject.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        If cboGISObject.SelectedIndex = -1 Then
            cboGISProperty.Items.Clear()
            cmdDetailOK.Enabled = False
            Exit Sub
        End If

    End Sub

    Private Sub cboGISObject_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGISObject.SelectedIndexChanged


        cboGISProperty.Items.Clear()

        If Not Information.IsArray(m_vAllowedProperties) Then
            Exit Sub
        End If

        If cboGISObject.SelectedIndex = -1 Then
            Exit Sub
        End If

        Dim lGISObjectId As Integer = VB6.GetItemData(cboGISObject, cboGISObject.SelectedIndex)

        For lTemp As Integer = m_vAllowedProperties.GetLowerBound(1) To m_vAllowedProperties.GetUpperBound(1)
            If CDbl(m_vAllowedProperties(ACPGISObjectId, lTemp)) = lGISObjectId Then
                Dim cboGISProperty_NewIndex As Integer = -1
                cboGISProperty_NewIndex = cboGISProperty.Items.Add(CStr(m_vAllowedProperties(ACPGISPropertyName, lTemp)))
                VB6.SetItemData(cboGISProperty, cboGISProperty_NewIndex, CInt(m_vAllowedProperties(ACPGISPropertyId, lTemp)))
            End If
        Next lTemp

        cmdDetailOK.Enabled = False

    End Sub


    Private Sub cboGISProperty_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGISProperty.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        cmdDetailOK.Enabled = (cboGISProperty.SelectedIndex <> -1)

    End Sub

    Private Sub cboGISProperty_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGISProperty.SelectedIndexChanged

        cmdDetailOK.Enabled = (cboGISProperty.SelectedIndex <> -1)

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_iAction = gPMConstants.PMEComponentAction.PMAdd

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        tabMainTab.Visible = False
        tabDetailTab.Top = VB6.TwipsToPixelsY(120)
        tabDetailTab.Visible = True

        cboGISObject.Enabled = True
        cboGISProperty.Enabled = True

        cmdDetailOK.Enabled = False

        m_lReturn = CType(ClearDetail(), gPMConstants.PMEReturnCode)

        m_bChanged = True

        cmdLimits.Enabled = False

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click


        m_iAction = gPMConstants.PMEComponentAction.PMDelete


        Dim lSelectedItem As Integer = Convert.ToString(lvwRiskTypeRILimits.Items.Item(lvwRiskTypeRILimits.FocusedItem.Index).Tag)

        m_vRiskTypeRILimits(ACRGISPropertyId, lSelectedItem) = 0

        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True
        cmdDelete.Enabled = False

        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)

        m_bChanged = True

        cmdLimits.Enabled = False

    End Sub

    Private Sub cmdDetailCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailCancel.Click

        tabDetailTab.Visible = False
        tabMainTab.Visible = True
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            cmdOK.Enabled = True
        End If

    End Sub

    Private Sub cmdDetailOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailOK.Click

        tabDetailTab.Visible = False
        tabMainTab.Visible = True
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True

        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)

        m_bChanged = True

        cmdLimits.Enabled = False

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdLimits_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLimits.Click

        m_lReturn = CType(CallLimits(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRiskTypeRILimits.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMURiskTypeRILimits.General()

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

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.RiskTypeId = m_lRiskTypeId
            ' {* USER DEFINED CODE (End) *}

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
            If m_iMode = 1 Then
                m_lReturn = ClearDetails()

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
                    'Developer Guide No.7
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

            m_oBusiness.Dispose()


            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

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

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayStatusSearching) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub DisplayStatusSearching()
    '
    'Static sMessage As String = ""
    '
    'Try 
    '
    ' Get message text if not already present.
    'If sMessage = "" Then

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    'End If
    '
    ' Display the status message.
    '    stbStatus.SimpleText = " " & sMessage$
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayStatusFound) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub DisplayStatusFound()
    '
    'Static sMessage As String = ""
    'Dim lItemsFound As Integer
    '
    'Try 
    '
    '
    ' Get message text if not already present.
    'If sMessage = "" Then

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    '
    'End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabDetailTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabDetailTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabDetailTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetSelectedIndex(tabDetailTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetTabCount(tabDetailTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabDetailTab) < (SSTabHelper.GetTabCount(tabDetailTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetSelectedIndex(tabDetailTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim vbReply As DialogResult

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            If m_bChanged Then
                vbReply = MessageBox.Show("All existing limits will be removed" & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to proceed?", Me.Text, MessageBoxButtons.YesNo)

                If vbReply = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If

            End If
            m_sLimitDescription = txtDescription.Text
            m_dtLimitEffectiveDate = txtEffectiveDate.Text
            m_dtLimitExpiryDate = txtExpiryDate.Text

            If m_sLimitDescription = "" Then
                MsgBox("Please enter Limit Description", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Reinsurance Model")
                txtDescription.Focus()
                Exit Sub
            ElseIf txtEffectiveDate.Text = "" Then
                MsgBox("Please enter Effective Date.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Reinsurance Model")
                txtEffectiveDate.Focus()
                Exit Sub
            ElseIf txtExpiryDate.Text = "" Then
                MsgBox("Please enter Limit Expiry Date", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Reinsurance Model")
                txtExpiryDate.Focus()
                Exit Sub
            End If


            If txtExpiryDate.Text <> "" And txtEffectiveDate.Text <> "" Then
                If m_dtLimitEffectiveDate > m_dtLimitExpiryDate Then
                    MsgBox("Expiry Date should be greater than Effective Date.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Reinsurance Model")
                    txtExpiryDate.Focus()
                    Exit Sub
                End If
            End If

            If m_iMode = gPMConstants.PMEComponentAction.PMEdit And m_iItemStatus <> ACItemStatus_Added Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            End If
            
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

    'UPGRADE_NOTE: (7001) The following declaration (cmdNavigate_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNavigate_Click()
    '
    ' Click event of the Navigate button.
    '
    'Try 
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
    '
    ' Process the next set of actions depending
    ' upon the interface task etc.
    'm_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    'Me.Hide()
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    Private Sub lvwRiskTypeRILimits_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRiskTypeRILimits.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.40
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If Task <> gPMConstants.PMEComponentAction.PMView Then
            cmdDelete.Enabled = Not (lvwRiskTypeRILimits.GetItemAt(x, y) Is Nothing)
        End If

    End Sub
    Private Sub txtDescription_GotFocus()

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)

    End Sub

    Private Sub txtDescription_LostFocus()

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)

        m_sLimitDescription = ToSafeString(txtDescription.Text)

    End Sub

    Private Sub txtEffectiveDate_GotFocus()

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEffectiveDate)

    End Sub

    Private Sub txtEffectiveDate_LostFocus()

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEffectiveDate)
        m_dtLimitEffectiveDate = ToSafeDate(txtEffectiveDate.Text)
        txtExpiryDate.Text = DateAdd("d", -1, DateAdd("yyyy", 1, m_dtLimitEffectiveDate))
    End Sub

    Private Sub txtExpiryDate_GotFocus()

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtExpiryDate)

    End Sub

    Private Sub txtExpiryDate_LostFocus()

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtExpiryDate)
        m_dtLimitExpiryDate = ToSafeDate(txtExpiryDate.Text)

    End Sub

    Public Function ClearDetails() As Integer

        Dim result As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            pnlRiskType.Text = m_sDescription
            m_sLimitDescription = ""
            txtEffectiveDate.Text = Format(Date.Now)
            m_dtLimitEffectiveDate = (Date.Now)

            m_dtLimitExpiryDate = DateAdd("YYYY", 1, DateAdd("d", -1, Now))
            txtDescription.Text = m_sLimitDescription
            txtExpiryDate.Text = Format(m_dtLimitExpiryDate)


            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,sMsg:="Failed to update the Detail details from the search data storage",vApp:=ACApp,vClass:=ACClass,vMethod:="ClearDetails",vErrNo:=Err.Number,vErrDesc:=ex.Message, excep:=ex)
            Return result
        End Try
    End Function
    ' PRIVATE Events (End)
End Class
