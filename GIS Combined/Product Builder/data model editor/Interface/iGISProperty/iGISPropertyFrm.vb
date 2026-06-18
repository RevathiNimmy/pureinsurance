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
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 05/05/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' RKS 26/04/2005 : Added Document Filter field for 354-Standard Wording
    '                  Control Enchancements
    ' RKS 31/05/2005 : Added a blank item in cboDocument_Filter
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    ' developer guide no. 7
    Private Const vbFormCode As Integer = 0
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
    Private m_lGISPropertyId As Integer
    Private m_lGISObjectId As Integer
    Private m_sPropertyName As String = ""
    Private m_sColumnName As String = ""
    Private m_lDataType As Integer
    Private m_lIsInputProperty As CheckState
    Private m_lIsIdentifyingProperty As CheckState
    Private m_lIsPrimaryKey As Integer
    ' developer guide no. 101
    Private m_lPolarisProperty As Object
    Private m_lIsDeleted As CheckState
    Private m_lIsSearchProperty As CheckState
    Private m_lIsChaseCycleProperty As CheckState
    Private m_lIndexLinkingId As Integer
    Private m_lEditFlags As Integer
    ' developer guide no. 17
    Private m_vSpecialsType As Object
    Private m_vSpecialsTypeReference As Object
    Private m_vIsChaseCycleProperty As Object
    Private m_lIsInMISExport As CheckState
    Private m_lIsFormattedText As CheckState

    Private m_vPartyTypeArray(,) As Object
    Private m_vSumInsuredTypeArray(,) As Object
    Private m_vDocumentFilterArray(,) As Object
    Private m_vPMLookupList(,) As Object
    Private m_vGISUserDefHeaderArray(,) As Object
    Private m_vProductArray(,) As Object
    Private m_vIndexLinkingArray(,) As Object
    Private m_lISClaim360Display As CheckState
    Private m_bDisableClaim360Display As Boolean



    Private m_vGISProperty(,) As Object

    Private m_lIsNonGIS As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iGISProperty.General

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

    Private m_sGisDataModel As String = ""
    Private m_lGISDataModelTypeID As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    Private m_lSwiftIntegration As Integer

    Private Const listIndexString As Integer = 0
    Private Const listIndexComment As Integer = 1
    Private Const listIndexInteger As Integer = 2
    Private Const listIndexDate As Integer = 3
    Private Const listIndexBoolean As Integer = 4
    Private Const listIndexCurrency As Integer = 5
    Private Const listIndexPercentage As Integer = 6
    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)
    ' GisDataModel

    Public WriteOnly Property GISDataModelTypeID() As Integer
        Set(ByVal Value As Integer)
            m_lGISDataModelTypeID = Value
        End Set
    End Property

    Public WriteOnly Property GisDataModel() As String
        Set(ByVal Value As String)
            m_sGisDataModel = Value
        End Set
    End Property

    ' PUBLIC Property Procedures (Begin)
    Public Property EditFlags() As Integer
        Get
            Return m_lEditFlags
        End Get
        Set(ByVal Value As Integer)
            m_lEditFlags = Value
        End Set
    End Property

    ' developer guide no. 71
    Public Property SpecialsType() As Integer
        Get
            Return m_vSpecialsType
        End Get
        Set(ByVal Value As Integer)

            m_vSpecialsType = (Value)
        End Set
    End Property
    '' developer guide no. 71
    Public Property SpecialsTypeReference() As Object
        Get
            Return m_vSpecialsTypeReference
        End Get
        Set(ByVal Value As Object)

            m_vSpecialsTypeReference = Value
        End Set
    End Property
    Public Property IsChaseCycleProperty() As CheckState
        Get
            Return m_lIsChaseCycleProperty
        End Get
        Set(ByVal Value As CheckState)
            m_lIsChaseCycleProperty = CInt(Value)
        End Set
    End Property


    Public Property GISPropertyId() As Integer
        Get
            Return m_lGISPropertyId
        End Get
        Set(ByVal Value As Integer)
            m_lGISPropertyId = Value
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

    Public Property PropertyName() As String
        Get
            Return m_sPropertyName
        End Get
        Set(ByVal Value As String)
            m_sPropertyName = Value
        End Set
    End Property

    Public Property ColumnName() As String
        Get
            Return m_sColumnName
        End Get
        Set(ByVal Value As String)
            m_sColumnName = Value
        End Set
    End Property

    Public Property DataType() As Integer
        Get
            Return m_lDataType
        End Get
        Set(ByVal Value As Integer)
            m_lDataType = Value
        End Set
    End Property

    Public Property IsInputProperty() As Integer
        Get
            Return m_lIsInputProperty
        End Get
        Set(ByVal Value As Integer)
            m_lIsInputProperty = Value
        End Set
    End Property

    Public Property IsIdentifyingProperty() As Integer
        Get
            Return m_lIsIdentifyingProperty
        End Get
        Set(ByVal Value As Integer)
            m_lIsIdentifyingProperty = Value
        End Set
    End Property

    Public Property IsPrimaryKey() As Integer
        Get
            Return m_lIsPrimaryKey
        End Get
        Set(ByVal Value As Integer)
            m_lIsPrimaryKey = Value
        End Set
    End Property

    ' developer guide no. 101
    Public Property PolarisPropertyId() As Object
        Get
            Return m_lPolarisProperty
        End Get
        Set(ByVal Value As Object)

            m_lPolarisProperty = (Value)
        End Set
    End Property

    Public Property IsDeleted() As CheckState
        Get
            Return m_lIsDeleted
        End Get
        Set(ByVal Value As CheckState)


            m_lIsDeleted = CInt(Value)
        End Set
    End Property

    Public Property IsSearchProperty() As CheckState
        Get
            Return m_lIsSearchProperty
        End Get
        Set(ByVal Value As CheckState)


            m_lIsSearchProperty = CInt(Value)
        End Set
    End Property

    Public Property IndexLinkingId() As Integer
        Get
            Return m_lIndexLinkingId
        End Get
        Set(ByVal Value As Integer)

            m_lIndexLinkingId = CInt(Value)
        End Set
    End Property
    Public Property PartyTypeArray() As Object
        Get
            Return VB6.CopyArray(m_vPartyTypeArray)
        End Get
        Set(ByVal Value As Object)
            m_vPartyTypeArray = Value
        End Set
    End Property


    Public Property SumInsuredTypeArray() As Object
        Get
            Return VB6.CopyArray(m_vSumInsuredTypeArray)
        End Get
        Set(ByVal Value As Object)
            m_vSumInsuredTypeArray = Value
        End Set
    End Property


    Public Property DocumentFilterArray() As Object
        Get
            Return VB6.CopyArray(m_vDocumentFilterArray)
        End Get
        Set(ByVal Value As Object)
            m_vDocumentFilterArray = Value
        End Set
    End Property
    Public Property PMLookupList() As Object
        Get
            Return VB6.CopyArray(m_vPMLookupList)
        End Get
        Set(ByVal Value As Object)
            m_vPMLookupList = Value
        End Set
    End Property

    Public Property GISUserDefHeaderArray() As Object
        Get
            Return VB6.CopyArray(m_vGISUserDefHeaderArray)
        End Get
        Set(ByVal Value As Object)
            m_vGISUserDefHeaderArray = Value
        End Set
    End Property

    Public Property ProductArray() As Object
        Get
            Return VB6.CopyArray(m_vProductArray)
        End Get
        Set(ByVal Value As Object)
            m_vProductArray = Value
        End Set
    End Property

    Public Property IndexLinkingArray() As Object
        Get
            Return VB6.CopyArray(m_vIndexLinkingArray)
        End Get
        Set(ByVal Value As Object)
            m_vIndexLinkingArray = Value
        End Set
    End Property

    Public Property IsNonGIS() As Integer
        Get
            Return m_lIsNonGIS
        End Get
        Set(ByVal Value As Integer)
            m_lIsNonGIS = Value
        End Set
    End Property

    Public Property GISProperty() As Object
        Get
            Return VB6.CopyArray(m_vGISProperty)
        End Get
        Set(ByVal Value As Object)
            m_vGISProperty = Value
        End Set
    End Property

    Public Property IsInMISExport() As Integer
        Get
            Return m_lIsInMISExport
        End Get
        Set(ByVal Value As Integer)
            m_lIsInMISExport = Value
        End Set
    End Property
    'PLICO 51
    Public Property IsFormattedText() As Integer
        Get
            Return m_lIsFormattedText
        End Get
        Set(ByVal Value As Integer)
            m_lIsFormattedText = Value
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
    Public Property IsClaim360Display() As CheckState
        Get
            Return m_lISClaim360Display
        End Get
        Set(ByVal Value As CheckState)


            m_lISClaim360Display = CInt(Value)
        End Set
    End Property
    Public Property DisableClaim360Display() As Boolean

        Get
            Return m_bDisableClaim360Display
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableClaim360Display = Value
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

    Public WriteOnly Property SwiftIntegration() As Integer
        Set(ByVal Value As Integer)
            m_lSwiftIntegration = Value
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

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPropertyName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtColumnName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboDataType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPolarisPropertyId, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The mandatory nature of these depends on the option chosen.
            'We'll handle it programmatically
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboGISListId, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPMLookupList, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtComboLookupTableName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPartyTypeId, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPMUSumInsuredType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboDocumentFilter, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboGISUserDefHeaderId, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

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
        Dim cCommonCode As Object

        Dim lTemp, lIndex As Integer

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


            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPropertyName, vControlValue:=m_sPropertyName)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtColumnName, vControlValue:=m_sColumnName)


            'if any of these entries are changed you must change the listIndexXXX consts
            cboDataType.Items.Clear()
            Dim cboDataType_NewIndex As Integer = -1
            cboDataType_NewIndex = cboDataType.Items.Add("String")
            VB6.SetItemData(cboDataType, cboDataType_NewIndex, iGISSharedConstants.GISDataTypeText)
            If m_lDataType = iGISSharedConstants.GISDataTypeText Then
                lIndex = cboDataType_NewIndex
            End If

            cboDataType_NewIndex = cboDataType.Items.Add("Comment")
            VB6.SetItemData(cboDataType, cboDataType_NewIndex, iGISSharedConstants.GISDataTypeComment)
            If m_lDataType = iGISSharedConstants.GISDataTypeComment Then
                lIndex = cboDataType_NewIndex
            End If
            'PN15782 describe as Numeric rather than Integer
            cboDataType_NewIndex = cboDataType.Items.Add("Integer")
            '    cboDataType.ItemData(cboDataType.NewIndex) = PMInteger
            VB6.SetItemData(cboDataType, cboDataType_NewIndex, iGISSharedConstants.GISDataTypeNumeric)
            '    If (m_lDataType = PMInteger) Then
            If m_lDataType = iGISSharedConstants.GISDataTypeNumeric Then
                lIndex = cboDataType_NewIndex
            End If

            cboDataType_NewIndex = cboDataType.Items.Add("Date")
            '    cboDataType.ItemData(cboDataType.NewIndex) = PMDate
            VB6.SetItemData(cboDataType, cboDataType_NewIndex, iGISSharedConstants.GISDataTypeDate)
            '    If (m_lDataType = PMDate) Then
            If m_lDataType = iGISSharedConstants.GISDataTypeDate Then
                lIndex = cboDataType_NewIndex
            End If

            cboDataType_NewIndex = cboDataType.Items.Add("Boolean")
            '    cboDataType.ItemData(cboDataType.NewIndex) = PMBoolean
            VB6.SetItemData(cboDataType, cboDataType_NewIndex, iGISSharedConstants.GISDataTypeOption)
            '    If (m_lDataType = PMBoolean) Then
            If m_lDataType = iGISSharedConstants.GISDataTypeOption Then
                lIndex = cboDataType_NewIndex
            End If

            cboDataType_NewIndex = cboDataType.Items.Add("Currency")
            '    cboDataType.ItemData(cboDataType.NewIndex) = PMCurrency
            VB6.SetItemData(cboDataType, cboDataType_NewIndex, iGISSharedConstants.GISDataTypeCurrency)
            '    If (m_lDataType = PMCurrency) Then
            If m_lDataType = iGISSharedConstants.GISDataTypeCurrency Then
                lIndex = cboDataType_NewIndex
            End If

            '    cboDataType.AddItem "Decimal"
            cboDataType_NewIndex = cboDataType.Items.Add("Percentage")
            '    cboDataType.ItemData(cboDataType.NewIndex) = PMDecimal
            VB6.SetItemData(cboDataType, cboDataType_NewIndex, iGISSharedConstants.GISDataTypePercentage)
            '    If (m_lDataType = PMDecimal) Then
            If m_lDataType = iGISSharedConstants.GISDataTypePercentage Then
                lIndex = cboDataType_NewIndex
            End If


            'SJ 15/07/2004 - start
            'Add new data type of integer
            cboDataType_NewIndex = cboDataType.Items.Add("G2Integer")
            VB6.SetItemData(cboDataType, cboDataType_NewIndex, iGISSharedConstants.GISDataTypeInteger)
            If m_lDataType = iGISSharedConstants.GISDataTypeInteger Then
                lIndex = cboDataType_NewIndex
            End If
            'SJ 15/07/2004 - end


            cboDataType.SelectedIndex = lIndex


            If Convert.IsDBNull(m_lPolarisProperty) Or IsNothing(m_lPolarisProperty) Then
                txtPolarisPropertyId.Text = ""
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPolarisPropertyId, vControlValue:=m_lPolarisProperty)
            End If


            If Not (Convert.IsDBNull(m_lIsInputProperty) Or IsNothing(m_lIsInputProperty)) Then
                chkIsInputProperty.CheckState = m_lIsInputProperty
            End If


            If Not (Convert.IsDBNull(m_lIsIdentifyingProperty) Or IsNothing(m_lIsIdentifyingProperty)) Then
                chkIsIdentifyingProperty.CheckState = m_lIsIdentifyingProperty
            End If


            If Not (Convert.IsDBNull(m_lIsPrimaryKey) Or IsNothing(m_lIsPrimaryKey)) Then
                chkIsPrimaryKey.CheckState = m_lIsPrimaryKey
            End If


            If Not (Convert.IsDBNull(m_lIsDeleted) Or IsNothing(m_lIsDeleted)) Then
                chkIsDeleted.CheckState = m_lIsDeleted
            End If

            If Not (Convert.IsDBNull(m_lISClaim360Display) Or IsNothing(m_lISClaim360Display)) Then
                chkIsClaim360Display.CheckState = m_lISClaim360Display
            End If

            If Not (Convert.IsDBNull(m_lIsSearchProperty) Or IsNothing(m_lIsSearchProperty)) Then
                chkIsSearchProperty.CheckState = m_lIsSearchProperty
            End If

            If Not (Convert.IsDBNull(m_lIsChaseCycleProperty) Or IsNothing(m_lIsChaseCycleProperty)) Then
                chkIsChaseCycleProperty.CheckState = m_lIsChaseCycleProperty
            End If

            chkIsMandatory.CheckState = IIf(m_lEditFlags And GISSharedPropertyConstants.GISDSEditMandatory, 1, 0)

            chkIsInMISExport.CheckState = m_lIsInMISExport

            'Once checked this option is disabled
            chkIsInMISExport.Enabled = (m_lIsInMISExport = CheckState.Unchecked)
            chkIsClaim360Display.Enabled = Not DisableClaim360Display
            If m_lGISDataModelTypeID = 2 Then
                chkIsClaim360Display.Visible = True
            Else
                chkIsClaim360Display.Visible = False
            End If

            'PLICO 51
            chkIsformattedText.Enabled = (m_lIsFormattedText = CheckState.Checked)
            chkIsFormattedText.CheckState = m_lIsFormattedText


            cboGISListId.Items.Clear()
            lIndex = -1

            'SJ 21/01/2004 - start
            m_lReturn = CType(LoadGisLists(cboGISListId, m_sGisDataModel), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadGisLists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface")
                Return result
            End If

            'populate party type drop list
            cboPartyTypeId.Items.Clear()
            lIndex = -1
            If Information.IsArray(m_vPartyTypeArray) Then
                For lTemp = m_vPartyTypeArray.GetLowerBound(1) To m_vPartyTypeArray.GetUpperBound(1)
                    Dim cboPartyTypeId_NewIndex As Integer = -1
                    cboPartyTypeId_NewIndex = cboPartyTypeId.Items.Add(CStr(m_vPartyTypeArray(1, lTemp)))
                    VB6.SetItemData(cboPartyTypeId, cboPartyTypeId_NewIndex, CInt(m_vPartyTypeArray(0, lTemp)))

                    If Not (Convert.IsDBNull(m_vSpecialsTypeReference) Or IsNothing(m_vSpecialsTypeReference)) Then
                        'If CInt(m_vPartyTypeArray(0, lTemp)) = m_vSpecialsTypeReference Then
                        If (m_vPartyTypeArray(0, lTemp)) = m_vSpecialsTypeReference Then
                            lIndex = cboPartyTypeId_NewIndex
                        End If
                    End If
                Next lTemp
            End If
            If m_vSpecialsType = GISSharedPropertyConstants.ACOPartyTypeID Then cboPartyTypeId.SelectedIndex = lIndex

            cboPMUSumInsuredType.Items.Clear()
            lIndex = -1
            If Information.IsArray(m_vSumInsuredTypeArray) Then
                For lTemp = m_vSumInsuredTypeArray.GetLowerBound(1) To m_vSumInsuredTypeArray.GetUpperBound(1)
                    Dim cboPMUSumInsuredType_NewIndex As Integer = -1
                    cboPMUSumInsuredType_NewIndex = cboPMUSumInsuredType.Items.Add(CStr(m_vSumInsuredTypeArray(1, lTemp)))
                    VB6.SetItemData(cboPMUSumInsuredType, cboPMUSumInsuredType_NewIndex, CInt(m_vSumInsuredTypeArray(0, lTemp)))

                    If Not (Convert.IsDBNull(m_vSpecialsTypeReference) Or IsNothing(m_vSpecialsTypeReference)) Then
                        'If CInt(m_vSumInsuredTypeArray(0, lTemp)) = m_vSpecialsTypeReference Then
                        If (m_vSumInsuredTypeArray(0, lTemp)) = m_vSpecialsTypeReference Then
                            lIndex = cboPMUSumInsuredType_NewIndex
                        End If
                    End If
                Next lTemp
            End If
            If m_vSpecialsType = GISSharedPropertyConstants.ACOSumInsuredTypeID Then cboPMUSumInsuredType.SelectedIndex = lIndex


            cboDocumentFilter.Items.Clear()
            lIndex = -1

            'Add a blank Item first
            Dim cboDocumentFilter_NewIndex As Integer = -1
            cboDocumentFilter_NewIndex = cboDocumentFilter.Items.Add("")
            VB6.SetItemData(cboDocumentFilter, cboDocumentFilter_NewIndex, -1)

            If Information.IsArray(m_vDocumentFilterArray) Then
                For lTemp = m_vDocumentFilterArray.GetLowerBound(1) To m_vDocumentFilterArray.GetUpperBound(1)
                    cboDocumentFilter_NewIndex = cboDocumentFilter.Items.Add(CStr(m_vDocumentFilterArray(0, lTemp)))
                    VB6.SetItemData(cboDocumentFilter, cboDocumentFilter_NewIndex, lTemp)

                    If Not (Convert.IsDBNull(m_vSpecialsTypeReference) Or IsNothing(m_vSpecialsTypeReference)) Then
                        'If CDbl(m_vDocumentFilterArray(0, lTemp)) = m_vSpecialsTypeReference Then
                        If Convert.ToString(m_vDocumentFilterArray(0, lTemp)) = Convert.ToString(m_vSpecialsTypeReference) Then
                            lIndex = cboDocumentFilter_NewIndex
                        End If
                    End If
                Next lTemp
            End If
            If m_vSpecialsType = GISSharedPropertyConstants.ACOStdWordingType Then cboDocumentFilter.SelectedIndex = lIndex

            cboPMLookupList.Items.Clear()
            lIndex = -1
            If Information.IsArray(m_vPMLookupList) Then
                For lTemp = m_vPMLookupList.GetLowerBound(1) To m_vPMLookupList.GetUpperBound(1)
                    Dim cboPMLookupList_NewIndex As Integer = -1
                    cboPMLookupList_NewIndex = cboPMLookupList.Items.Add(CStr(m_vPMLookupList(0, lTemp)))
                    VB6.SetItemData(cboPMLookupList, cboPMLookupList_NewIndex, lTemp)

                    If Not (Convert.IsDBNull(m_vSpecialsTypeReference) Or IsNothing(m_vSpecialsTypeReference)) Then
                        'If CStr(m_vPMLookupList(0, lTemp)).ToUpper() = CStr(m_vSpecialsTypeReference).ToUpper() Then
                        If Convert.ToString(m_vPMLookupList(0, lTemp)).ToUpper() = CStr(m_vSpecialsTypeReference).ToUpper() Then
                            lIndex = cboPMLookupList_NewIndex
                        End If
                    End If
                Next
            End If
            If m_vSpecialsType = GISSharedPropertyConstants.ACOPMLookupTableName Then cboPMLookupList.SelectedIndex = lIndex

            cboGISUserDefHeaderId.Items.Clear()
            lIndex = -1
            If Information.IsArray(m_vGISUserDefHeaderArray) Then
                For lTemp = m_vGISUserDefHeaderArray.GetLowerBound(1) To m_vGISUserDefHeaderArray.GetUpperBound(1)
                    Dim cboGISUserDefHeaderId_NewIndex As Integer = -1
                    cboGISUserDefHeaderId_NewIndex = cboGISUserDefHeaderId.Items.Add(CStr(m_vGISUserDefHeaderArray(1, lTemp)))
                    VB6.SetItemData(cboGISUserDefHeaderId, cboGISUserDefHeaderId_NewIndex, CInt(m_vGISUserDefHeaderArray(0, lTemp)))

                    If Not (Convert.IsDBNull(m_vSpecialsTypeReference) Or IsNothing(m_vSpecialsTypeReference)) Then
                        'If CInt(m_vGISUserDefHeaderArray(0, lTemp)) = m_vSpecialsTypeReference Then
                        If Convert.ToString(m_vGISUserDefHeaderArray(0, lTemp)) = Convert.ToString(m_vSpecialsTypeReference) Then
                            lIndex = cboGISUserDefHeaderId_NewIndex
                        End If
                    End If
                Next lTemp
            End If
            If m_vSpecialsType = GISSharedPropertyConstants.ACOGISUserDefHeaderID Then cboGISUserDefHeaderId.SelectedIndex = lIndex

            cboProductId.Items.Clear()
            lIndex = -1
            If Information.IsArray(m_vProductArray) Then
                For lTemp = m_vProductArray.GetLowerBound(1) To m_vProductArray.GetUpperBound(1)
                    Dim cboProductId_NewIndex As Integer = -1
                    cboProductId_NewIndex = cboProductId.Items.Add(CStr(m_vProductArray(1, lTemp)))
                    VB6.SetItemData(cboProductId, cboProductId_NewIndex, CInt(m_vProductArray(0, lTemp)))

                    If Not (Convert.IsDBNull(m_vSpecialsTypeReference) Or IsNothing(m_vSpecialsTypeReference)) Then
                        'If CInt(m_vProductArray(0, lTemp)) = m_vSpecialsTypeReference Then
                        If Convert.ToString(m_vProductArray(0, lTemp)) = Convert.ToString(m_vSpecialsTypeReference) Then
                            lIndex = cboProductId_NewIndex
                        End If
                    End If
                Next lTemp
            End If
            If m_vSpecialsType = GISSharedPropertyConstants.ACOProductID Then cboProductId.SelectedIndex = lIndex

            Dim sCodeDescription As String = ""
            Dim aCommonCodes(2, 100) As Object
            Dim iArrayPos As Integer
            Dim aSwiftSpecialTypes As Object
            Dim cboIndexLinking_NewIndex As Integer = -1
            If m_lSwiftIntegration <> 0 Then

                ' this has been called from swift


                'populate the specials type if required
                If (m_lSwiftIntegration And GISSharedPropertyConstants.SwiftMode_DisplaySpecialsList) = GISSharedPropertyConstants.SwiftMode_DisplaySpecialsList Then

                    GetSwiftSpecialListTypeNames(aSwiftSpecialTypes)
                End If

                'enable swift special field
                optFieldType(GISSharedPropertyConstants.ACOSwiftSpecialType).Visible = True

                ' hide controls not applicable to swift
                optFieldType(GISSharedPropertyConstants.ACOGISListID).Visible = False
                lblGISListId.Visible = False
                cboGISListId.Visible = False
                optFieldType(GISSharedPropertyConstants.ACOPartyTypeID).Visible = False
                lblPartyTypeId.Visible = False
                cboPartyTypeId.Visible = False

                optFieldType(GISSharedPropertyConstants.ACOSumInsuredTypeID).Visible = False
                lblPMUSumInsuredType.Visible = False
                cboPMUSumInsuredType.Visible = False

                optFieldType(GISSharedPropertyConstants.ACOStdWordingType).Visible = False
                lblDocumentFilter.Visible = False
                cboDocumentFilter.Visible = False

                optFieldType(GISSharedPropertyConstants.ACOGISUserDefHeaderID).Visible = False
                lblGISUserDefHeaderId.Visible = False
                cboGISUserDefHeaderId.Visible = False
                optFieldType(GISSharedPropertyConstants.ACOProductID).Visible = False
                lblProductId.Visible = False
                cboProductId.Visible = False
                optFieldType(GISSharedPropertyConstants.ACOReserveID).Visible = False
                optFieldType(GISSharedPropertyConstants.ACOPaymentID).Visible = False
                optFieldType(GISSharedPropertyConstants.ACOCaseHeader).Visible = False
                optFieldType(GISSharedPropertyConstants.ACOCaseClaimList).Visible = False

                ' Now for the differences between swift modes
                If (m_lSwiftIntegration And GISSharedPropertyConstants.SwiftMode_NotRenderedByPB) = GISSharedPropertyConstants.SwiftMode_NotRenderedByPB Then
                    optFieldType(GISSharedPropertyConstants.ACOSwiftClientSelector).Visible = False
                    optFieldType(GISSharedPropertyConstants.ACOSwiftAddressSelector).Visible = False
                Else
                    optFieldType(GISSharedPropertyConstants.ACOSwiftAddress).Visible = False
                    optFieldType(GISSharedPropertyConstants.ACOSwiftListView).Visible = False
                    optFieldType(GISSharedPropertyConstants.ACOSwiftNotes).Visible = False
                    optFieldType(GISSharedPropertyConstants.ACOComboLookup).Visible = False
                End If

                'swift common code
                'Modified by Archana Tokas on 31/03/2010 02:35:20 commented as iSWcommonCode.CCommonCode not found in compelete list todolist 
                'cCommonCode = New iSWcommonCode.CCommonCode()

                cboCommonCode.Items.Clear()
                lIndex = 0
                'Dim cboIndexLinking_NewIndex As Integer = -1
                cboIndexLinking_NewIndex = cboIndexLinking.Items.Add("(Null)")

                lTemp = 1
                iArrayPos = 0
                For lTemp = 1 To 100

                    sCodeDescription = cCommonCode.CodeTypeSingular(lTemp)
                    If sCodeDescription <> "" Then

                        aCommonCodes(0, iArrayPos) = lTemp

                        aCommonCodes(1, iArrayPos) = sCodeDescription
                        iArrayPos += 1
                    End If
                Next

                'sort list headers into alphabetical order
                QuicksortArrays(aCommonCodes, 0, iArrayPos - 1, 1)

                For lTemp = 1 To iArrayPos
                    Dim cboCommonCode_NewIndex As Integer = -1

                    cboCommonCode_NewIndex = cboCommonCode.Items.Add(CStr(aCommonCodes(1, lTemp - 1)))

                    VB6.SetItemData(cboCommonCode, cboCommonCode_NewIndex, CInt(aCommonCodes(0, lTemp - 1)))
                Next

                'add the generic specials selector if required
                If (m_lSwiftIntegration And GISSharedPropertyConstants.SwiftMode_DisplaySpecialsList) = GISSharedPropertyConstants.SwiftMode_DisplaySpecialsList Then
                    cboSpecialsSelector.Items.Clear()

                    For lTemp = 0 To aSwiftSpecialTypes(0).GetUpperBound(0)
                        Dim cboSpecialsSelector_NewIndex As Integer = -1

                        cboSpecialsSelector_NewIndex = cboSpecialsSelector.Items.Add(CStr(aSwiftSpecialTypes(0)(lTemp)))

                        VB6.SetItemData(cboSpecialsSelector, cboSpecialsSelector_NewIndex, CInt(aSwiftSpecialTypes(1)(lTemp)))
                    Next
                End If
            End If


            If Not optFieldType(m_vSpecialsType).Visible Then
                ' error ?????
            End If


            Select Case m_vSpecialsType
                Case GISSharedPropertyConstants.ACOSwiftCommonCode, GISSharedPropertyConstants.ACOSwiftClientSelector, GISSharedPropertyConstants.ACOSwiftAddressSelector, GISSharedPropertyConstants.ACOSwiftListView, GISSharedPropertyConstants.ACOSwiftNotes, GISSharedPropertyConstants.ACOSwiftAddress, GISSharedPropertyConstants.ACOComboLookup
                    ' set the other swift special option button on the main tab
                    ' this must be done before formatting this option so that the click event for ACOSwiftSpecialType is fired first
                    optFieldType(GISSharedPropertyConstants.ACOSwiftSpecialType).Checked = True
            End Select


            ' now set the relevant option button
            Select Case m_vSpecialsType
                Case GISSharedPropertyConstants.ACOGISListID
                    ' remember this will fire a click event
                    optFieldType(m_vSpecialsType).Checked = True

                    ' now format the controls dependent upon this option
                    For lTemp = 0 To cboGISListId.Items.Count - 1
                        'If m_vSpecialsTypeReference = CType(cboGISListId.Items(lTemp), VB6.ListBoxItem).ItemString Then
                        If m_vSpecialsTypeReference = CType(cboGISListId.Items(lTemp), VB6.ListBoxItem).ItemData Then
                            cboGISListId.SelectedIndex = lTemp
                            Exit For
                        End If
                    Next lTemp

                Case GISSharedPropertyConstants.ACOPMLookupTableName
                    ' remember this will fire a click event
                    optFieldType(m_vSpecialsType).Checked = True

                    ' now format the controls dependent upon this option
                    'm_lReturn& = m_oFormFields.FormatControl(ctlControl:=cboPMLookupList, vControlValue:=m_vSpecialsTypeReference)

                Case GISSharedPropertyConstants.ACOComboLookup
                    ' remember this will fire a click event
                    optFieldType(m_vSpecialsType).Checked = True

                    ' now format the controls dependent upon this option
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtComboLookupTableName, vControlValue:=m_vSpecialsTypeReference)

                Case GISSharedPropertyConstants.ACOSwiftCommonCode
                    ' remember this will fire a click event
                    optFieldType(m_vSpecialsType).Checked = True

                    ' now format the controls dependent upon this option
                    'set the drop list to the value
                    For lTemp = 0 To cboCommonCode.Items.Count - 1
                        If m_vSpecialsTypeReference = VB6.GetItemData(cboCommonCode, lTemp) Then
                            cboCommonCode.SelectedIndex = lTemp
                            Exit For
                        End If
                    Next lTemp

                Case GISSharedPropertyConstants.ACOSwiftListView
                    If (m_lSwiftIntegration And GISSharedPropertyConstants.SwiftMode_DisplaySpecialsList) = GISSharedPropertyConstants.SwiftMode_DisplaySpecialsList Then

                        ' remember this will fire a click event
                        optFieldType(m_vSpecialsType).Checked = True

                        ' now format the controls dependent upon this option
                        'set the drop list to the value
                        For lTemp = 0 To cboSpecialsSelector.Items.Count - 1
                            If m_vSpecialsTypeReference = VB6.GetItemData(cboSpecialsSelector, lTemp) Then
                                cboSpecialsSelector.SelectedIndex = lTemp
                                Exit For
                            End If
                        Next lTemp
                    End If

                Case Else

                    ' remember this will fire a click event
                    optFieldType(m_vSpecialsType).Checked = True

            End Select


            cboIndexLinking.Items.Clear()
            lIndex = 0

            cboIndexLinking_NewIndex = cboIndexLinking.Items.Add("(Null)")

            If Information.IsArray(m_vIndexLinkingArray) Then
                For lTemp = m_vIndexLinkingArray.GetLowerBound(1) To m_vIndexLinkingArray.GetUpperBound(1)
                    cboIndexLinking_NewIndex = cboIndexLinking.Items.Add(CStr(m_vIndexLinkingArray(1, lTemp)))
                    VB6.SetItemData(cboIndexLinking, cboIndexLinking_NewIndex, CInt(m_vIndexLinkingArray(0, lTemp)))

                    If Not (Convert.IsDBNull(m_lIndexLinkingId) Or IsNothing(m_lIndexLinkingId)) Then
                        If CInt(m_vIndexLinkingArray(0, lTemp)) = m_lIndexLinkingId Then
                            lIndex = cboIndexLinking_NewIndex
                        End If
                    End If
                Next lTemp
            End If

            cboIndexLinking.SelectedIndex = lIndex

            If (m_lIsPrimaryKey = gPMConstants.PMEReturnCode.PMTrue) Or (m_lEditFlags And GISSharedPropertyConstants.GISDSEditReadOnly) Or (m_iTask = gPMConstants.PMEComponentAction.PMView) Then
                m_lReturn = CType(DisableForm(lDisabled:=True), gPMConstants.PMEReturnCode)
            End If



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


            m_sPropertyName = CStr(m_oFormFields.UnformatControl(txtPropertyName))

            m_sColumnName = CStr(m_oFormFields.UnformatControl(txtColumnName))
            m_lDataType = VB6.GetItemData(cboDataType, cboDataType.SelectedIndex)

            m_lPolarisProperty = (m_oFormFields.UnformatControl(txtPolarisPropertyId))
            If m_lPolarisProperty = 0 Then

                m_lPolarisProperty = Nothing
            End If

            m_lIsInputProperty = chkIsInputProperty.CheckState
            m_lISClaim360Display = chkIsClaim360Display.CheckState
            m_lIsIdentifyingProperty = chkIsIdentifyingProperty.CheckState
            m_lIsPrimaryKey = chkIsPrimaryKey.CheckState
            m_lIsDeleted = chkIsDeleted.CheckState
            m_lIsSearchProperty = chkIsSearchProperty.CheckState
            m_lIsChaseCycleProperty = chkIsChaseCycleProperty.CheckState
            m_lIsInMISExport = chkIsInMISExport.CheckState
            'PLICO 51
            m_lIsFormattedText = chkIsformattedText.CheckState

            m_lEditFlags = IIf(chkIsMandatory.CheckState = CheckState.Checked, m_lEditFlags Or GISSharedPropertyConstants.GISDSEditMandatory, m_lEditFlags And Not GISSharedPropertyConstants.GISDSEditMandatory)

            'store the data type
            For m_vSpecialsType = 0 To GISSharedPropertyConstants.ACOSpecialLastIndex
                If m_vSpecialsType <> GISSharedPropertyConstants.ACOSwiftSpecialType Then
                    If optFieldType(m_vSpecialsType).Checked Then
                        Exit For
                    End If
                End If
            Next

            If m_vSpecialsType = GISSharedPropertyConstants.ACOSwiftListView And m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                ' this type of property should not exist as a column in the database
                If (m_lEditFlags And GISSharedPropertyConstants.GISDSEditNoDBColumn) <> GISSharedPropertyConstants.GISDSEditNoDBColumn Then
                    m_lEditFlags += GISSharedPropertyConstants.GISDSEditNoDBColumn
                End If
            End If
            ' developer guide no. 17
            m_vSpecialsTypeReference = Nothing

            Select Case m_vSpecialsType
                Case GISSharedPropertyConstants.ACOSpecialNone
                Case GISSharedPropertyConstants.ACOGISListID
                    If cboGISListId.SelectedIndex = -1 Then

                        m_vSpecialsTypeReference = Nothing
                    Else
                        ' m_vSpecialsTypeReference = VB6.GetItemData(cboGISListId, cboGISListId.SelectedIndex)
                        m_vSpecialsTypeReference = CType(cboGISListId.Items(cboGISListId.SelectedIndex), VB6.ListBoxItem).ItemData
                        'm_vSpecialsTypeReference = cboGISListId.Text
                    End If

                Case GISSharedPropertyConstants.ACOPMLookupTableName
                    If cboPMLookupList.SelectedIndex = -1 Then

                        m_vSpecialsTypeReference = Nothing
                    Else
                        ' developer guide no. 17
                        m_vSpecialsTypeReference = (cboPMLookupList.Text)
                    End If

                Case GISSharedPropertyConstants.ACOComboLookup

                    m_vSpecialsTypeReference = CInt(m_oFormFields.UnformatControl(txtComboLookupTableName))
                    If m_vSpecialsTypeReference = StringsHelper.ToDoubleSafe("") Then

                        m_vSpecialsTypeReference = Nothing
                    End If

                Case GISSharedPropertyConstants.ACOPartyTypeID
                    If cboPartyTypeId.SelectedIndex = -1 Then

                        m_vSpecialsTypeReference = Nothing
                    Else
                        m_vSpecialsTypeReference = VB6.GetItemData(cboPartyTypeId, cboPartyTypeId.SelectedIndex)
                    End If

                Case GISSharedPropertyConstants.ACOSumInsuredTypeID
                    If cboPMUSumInsuredType.SelectedIndex = -1 Then

                        m_vSpecialsTypeReference = Nothing
                    Else
                        m_vSpecialsTypeReference = VB6.GetItemData(cboPMUSumInsuredType, cboPMUSumInsuredType.SelectedIndex)
                    End If

                Case GISSharedPropertyConstants.ACOStdWordingType
                    If cboDocumentFilter.SelectedIndex = -1 Then

                        m_vSpecialsTypeReference = Nothing
                    Else
                        m_vSpecialsTypeReference = (VB6.GetItemString(cboDocumentFilter, cboDocumentFilter.SelectedIndex))
                    End If

                Case GISSharedPropertyConstants.ACOGISUserDefHeaderID
                    If cboGISUserDefHeaderId.SelectedIndex = -1 Then

                        m_vSpecialsTypeReference = Nothing
                    Else
                        m_vSpecialsTypeReference = VB6.GetItemData(cboGISUserDefHeaderId, cboGISUserDefHeaderId.SelectedIndex)
                    End If

                Case GISSharedPropertyConstants.ACOProductID
                    If cboProductId.SelectedIndex = -1 Then

                        m_vSpecialsTypeReference = Nothing
                    Else
                        m_vSpecialsTypeReference = VB6.GetItemData(cboProductId, cboProductId.SelectedIndex)
                    End If

                Case GISSharedPropertyConstants.ACOReserveID, GISSharedPropertyConstants.ACOPaymentID, GISSharedPropertyConstants.ACOSwiftClientSelector, GISSharedPropertyConstants.ACOSwiftAddressSelector, GISSharedPropertyConstants.ACOSwiftNotes, GISSharedPropertyConstants.ACOSwiftAddress

                Case GISSharedPropertyConstants.ACOSwiftCommonCode
                    m_vSpecialsTypeReference = VB6.GetItemData(cboCommonCode, cboCommonCode.SelectedIndex)
                Case GISSharedPropertyConstants.ACOSwiftListView
                    m_vSpecialsTypeReference = VB6.GetItemData(cboSpecialsSelector, cboSpecialsSelector.SelectedIndex)

            End Select




            ' {* USER DEFINED CODE (End) *}


            If cboIndexLinking.SelectedIndex < 1 Then

                m_lIndexLinkingId = Nothing
            Else
                m_lIndexLinkingId = VB6.GetItemData(cboIndexLinking, cboIndexLinking.SelectedIndex)
            End If



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
            chkIsChaseCycleProperty.Enabled = False

            chkIsChaseCycleProperty.CheckState = False


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

            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = -1) Then
                txtColumnName.Enabled = True
                cboDataType.Enabled = True
                txtPolarisPropertyId.Enabled = True
                chkIsInputProperty.Enabled = True
                chkIsSearchProperty.Enabled = True
                chkIsMandatory.Enabled = True
                For lCount As Integer = 0 To GISSharedPropertyConstants.ACOSpecialLastIndex
                    optFieldType(lCount).Enabled = True
                    optFieldType(lCount).Visible = True
                    ' ================================================================
                    ' This is temporary code that will be removed once the DocuMaster option has been fully implemented
                    If lCount = GISSharedPropertyConstants.ACODocuMaster Then
                        optFieldType(lCount).Enabled = False
                        optFieldType(lCount).Visible = False
                    End If
                    ' ================================================================
                Next

                'Standard wording and sum insured are only available when non-GIS
                If IsNonGIS = 1 Then
                    optFieldType(GISSharedPropertyConstants.ACOStdWordingType).Enabled = True
                    optFieldType(GISSharedPropertyConstants.ACOSumInsuredTypeID).Enabled = True
                End If

            End If

            'swift mode checks
            If m_lSwiftIntegration = 0 Then optFieldType(GISSharedPropertyConstants.ACOSwiftSpecialType).Visible = False

            'hide the generic specials selector if not required
            If (m_lSwiftIntegration And GISSharedPropertyConstants.SwiftMode_DisplaySpecialsList) = 0 Then
                optFieldType(GISSharedPropertyConstants.ACOSwiftListView).Visible = False
                cboSpecialsSelector.Visible = False

                optFieldType(GISSharedPropertyConstants.ACOCaseHeader).Top = optFieldType(GISSharedPropertyConstants.ACOSwiftSpecialType).Top
                optFieldType(GISSharedPropertyConstants.ACOCaseClaimList).Top = optFieldType(GISSharedPropertyConstants.ACOCaseHeader).Top + VB6.TwipsToPixelsY(345)

            End If

            'Party Builder integration - Party Builder data models
            'have a limited set of field types.
            If m_lGISDataModelTypeID = 4 Or m_lGISDataModelTypeID = 5 Then
                optFieldType(GISSharedPropertyConstants.ACOPartyTypeID).Visible = False
                lblPartyTypeId.Visible = False
                cboPartyTypeId.Visible = False

                optFieldType(GISSharedPropertyConstants.ACOSumInsuredTypeID).Visible = False
                lblPMUSumInsuredType.Visible = False
                cboPMUSumInsuredType.Visible = False

                optFieldType(GISSharedPropertyConstants.ACOStdWordingType).Visible = False
                lblDocumentFilter.Visible = False
                cboDocumentFilter.Visible = False

                optFieldType(GISSharedPropertyConstants.ACOProductID).Visible = False
                lblProductId.Visible = False
                cboProductId.Visible = False

                optFieldType(GISSharedPropertyConstants.ACOReserveID).Visible = False

                optFieldType(GISSharedPropertyConstants.ACOPaymentID).Visible = False

                optFieldType(GISSharedPropertyConstants.ACOCaseHeader).Visible = False
                optFieldType(GISSharedPropertyConstants.ACOCaseClaimList).Visible = False

                lblIndexLinking.Visible = False
                cboIndexLinking.Visible = False

                optFieldType(GISSharedPropertyConstants.ACOGISUserDefHeaderID).Top = optFieldType(GISSharedPropertyConstants.ACOPartyTypeID).Top
                lblGISUserDefHeaderId.Top = lblPartyTypeId.Top
                cboGISUserDefHeaderId.Top = cboPartyTypeId.Top
                fraGeneral.Height = VB6.TwipsToPixelsY(4500)
                tabMainTab.Height = fraGeneral.Height + VB6.TwipsToPixelsY(500)
                cmdOK.Top = tabMainTab.Top + tabMainTab.Height + VB6.TwipsToPixelsY(100)
                cmdCancel.Top = cmdOK.Top
                cmdHelp.Top = cmdOK.Top
                Me.Height = cmdHelp.Top + cmdHelp.Height + VB6.TwipsToPixelsY(600)

                optFieldType(GISSharedPropertyConstants.ACODocuMaster).Visible = False

            End If

            ' {* USER DEFINED CODE (End) *}
            iPMFunc.CenterForm(Me)

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

            m_ctlTabFirstLast(ACControlStart, 0) = txtPropertyName
            m_ctlTabFirstLast(ACControlEnd, 0) = cboGISUserDefHeaderId

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


            ' developer guide no. 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            ' developer guide no. 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabEnabled(tabMainTab, 0, True)
            ' developer guide no. 243
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


            ' developer guide no. 243
            lblPropertyName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionPropertyName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblColumnName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionColumnName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblDataType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionDataType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblPolarisPropertyId.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionPolarisProperty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            chkIsInputProperty.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionIsInputProperty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            chkIsIdentifyingProperty.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionIsIdentifier, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            chkIsPrimaryKey.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionIsPrimaryKey, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            chkIsDeleted.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionIsDeleted, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            chkIsSearchProperty.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionIsSearchProperty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOSpecialNone).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionNormal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOGISListID).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionGISList, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOPMLookupTableName).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionPMLookup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOPartyTypeID).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOSumInsuredTypeID).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSumInsured, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOGISUserDefHeaderID).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionUserDefined, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOStdWordingType).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionStandardWording, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            lblGISListId.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionGISListId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblPMLookupTableName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionLookupTableName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblPartyTypeId.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionPartyType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblPMUSumInsuredType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSumInsuredType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblDocumentFilter.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionDocumentFilter, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblGISUserDefHeaderId.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionUserDefinedTable, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblIndexLinking.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionIndexLinkingId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'swift field support


            optFieldType(GISSharedPropertyConstants.ACOSwiftListView).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSwiftSpecails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblSwiftListViewType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSwiftListViewType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOSwiftCommonCode).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSwiftCC, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblCommonCodeType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionCommonCodeType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOSwiftClientSelector).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSwiftClientSelector, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOSwiftAddressSelector).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSwiftAddressSelector, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOSwiftSpecialType).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSwiftSpecial, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOSwiftNotes).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSwiftNotes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOSwiftAddress).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSwiftAddress, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOComboLookup).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionComboLookup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            lblComboLookupTableName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionComboLookupTableName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' developer guide no. 243
            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOCaseHeader).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionCaseHeader, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            ' developer guide no. 243
            optFieldType(GISSharedPropertyConstants.ACOCaseClaimList).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionCaseClaimLinks, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set all of the forms controls to the disable state.

            For Each ctlFormControl As Control In ContainerHelper.Controls(Me)
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

    ' ***************************************************************** '
    '
    ' Name: ValidateForm
    '
    ' Description:
    '
    ' History: 07/09/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateForm(ByRef r_iTab As Integer) As Integer

        Dim result As Integer = 0
        Dim lIndex As Integer
        Dim sTemp, sTemp2, sComp As String
        Dim vReservedKeywords As Object
        Dim PBSqlReservedKeywords As PBSqlReservedKeywords
        Dim bSwiftSpecialType As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            PBSqlReservedKeywords = New PBSqlReservedKeywords()


            vReservedKeywords = PBSqlReservedKeywords.vSqlReservedKeywords
            sComp = PBSqlReservedKeywords.vInvalidCharacters

            ' RDC 28012004 moved from after the following 'If' block
            sTemp = txtPropertyName.Text.Trim().ToUpper()


            r_iTab = 0

            Dim dbNumericTemp As Double
            If Double.TryParse(sTemp.Substring(0, 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("Cannot have numeric value as the first character of a property name", "GIS Object", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            For Each sChar As Char In sComp
                If sTemp.IndexOf(sChar) + 1 Then
                    MessageBox.Show("Cannot have '" & sChar & "' in the property name", "GIS Property", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next sChar

            sTemp = txtColumnName.Text.Trim().ToUpper()


            For lTemp As Integer = 0 To vReservedKeywords.GetUpperBound(0)

                If sTemp = CStr(vReservedKeywords(lTemp)) Then
                    MessageBox.Show("Cannot have '" & txtColumnName.Text.Trim() & "' as the column name", "GIS Property", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next

            For Each sChar As Char In sComp
                If sTemp.IndexOf(sChar) + 1 Then
                    MessageBox.Show("Cannot have '" & sChar & "' in the column name", "GIS Property", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next sChar


            'Seems the easiest way
            For lTemp As Integer = GISSharedPropertyConstants.ACOSpecialNone To GISSharedPropertyConstants.ACOSpecialLastIndex
                If optFieldType(lTemp).Checked Then
                    If lTemp <> GISSharedPropertyConstants.ACOSwiftSpecialType Then
                        lIndex = lTemp
                        Exit For
                    End If
                End If
            Next lTemp

            ' has the swift special option been set
            For lTemp As Integer = GISSharedPropertyConstants.ACOSpecialNone To GISSharedPropertyConstants.ACOSpecialLastIndex
                If optFieldType(lTemp).Checked Then
                    If lTemp = GISSharedPropertyConstants.ACOSwiftSpecialType Then
                        bSwiftSpecialType = True
                        Exit For
                    End If
                End If
            Next lTemp



            Select Case lIndex
                Case GISSharedPropertyConstants.ACOGISListID
                    If cboGISListId.SelectedIndex = -1 Then
                        MessageBox.Show("You must select a list id", "Data Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case GISSharedPropertyConstants.ACOPMLookupTableName
                    If cboPMLookupList.SelectedIndex = -1 Then
                        MessageBox.Show("You must select a lookup table name", "Data Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedPropertyConstants.ACOComboLookup
                    If txtComboLookupTableName.Text = "" Then
                        MessageBox.Show("You must enter a lookup table name", "Data Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case GISSharedPropertyConstants.ACOPartyTypeID
                    If cboPartyTypeId.SelectedIndex = -1 Then
                        MessageBox.Show("You must select a party type", "Data Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case GISSharedPropertyConstants.ACOSumInsuredTypeID
                    If cboPMUSumInsuredType.SelectedIndex = -1 Then
                        MessageBox.Show("You must select a sum insured type", "Data Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case GISSharedPropertyConstants.ACOGISUserDefHeaderID
                    If cboGISUserDefHeaderId.SelectedIndex = -1 Then
                        MessageBox.Show("You must select a user-defined table", "Data Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedPropertyConstants.ACOProductID
                    If cboProductId.SelectedIndex = -1 Then
                        MessageBox.Show("You must select a product", "Data Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedPropertyConstants.ACOSwiftCommonCode
                    If cboCommonCode.SelectedIndex = -1 Then
                        SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                        MessageBox.Show("You must select a common code", "Data Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedPropertyConstants.ACOSpecialNone
                    ' an option has not been selected
                    If bSwiftSpecialType Then
                        SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                        MessageBox.Show("You must select one of the Swift Special types", "Data Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
            End Select

            'Need to check that the object and table names haven't already been used

            sTemp = txtColumnName.Text.Trim().ToUpper()
            sTemp2 = txtPropertyName.Text.Trim().ToUpper()

            If Information.IsArray(m_vGISProperty) Then
                For iTemp As Integer = m_vGISProperty.GetLowerBound(1) To m_vGISProperty.GetUpperBound(1)

                    If sTemp2 = CStr(m_vGISProperty(ACPPropertyName, iTemp)).ToUpper() Then
                        If sTemp2 <> m_sPropertyName.ToUpper() Then
                            MessageBox.Show("This property name is already used", "GIS Property", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    If sTemp = CStr(m_vGISProperty(ACPColumnName, iTemp)).ToUpper() And CStr(m_vGISProperty(ACPPropertyName, iTemp)) <> "dElEtEd" Then
                        If sTemp <> m_sColumnName.ToUpper() Then
                            MessageBox.Show("This column name is already used", "GIS Property", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                Next iTemp
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ChangeOption
    '
    ' Description:
    '
    ' History: 15/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ChangeOption(ByVal v_iIndex As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' first of all, because the swift special options are on a different tab then they
            ' will not be turned off automatically if the swift special option is unselected
            ' on the main tab so we must do it manually now
            If Not optFieldType(GISSharedPropertyConstants.ACOSwiftSpecialType).Checked Then
                optFieldType(GISSharedPropertyConstants.ACOSwiftCommonCode).Checked = False
                optFieldType(GISSharedPropertyConstants.ACOSwiftClientSelector).Checked = False
                optFieldType(GISSharedPropertyConstants.ACOSwiftAddressSelector).Checked = False
                optFieldType(GISSharedPropertyConstants.ACOSwiftListView).Checked = False
                optFieldType(GISSharedPropertyConstants.ACOSwiftNotes).Checked = False
                optFieldType(GISSharedPropertyConstants.ACOSwiftAddress).Checked = False
                optFieldType(GISSharedPropertyConstants.ACOComboLookup).Checked = False
            End If


            'A case could not have coped with the resetting of text when the control is disabled
            If v_iIndex = GISSharedPropertyConstants.ACOSpecialNone Then
                If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = -1) Then
                    chkIsInputProperty.Enabled = True
                    cboDataType.Enabled = True
                    'Only if it's a string
                    If cboDataType.SelectedIndex = listIndexString Then
                        chkIsSearchProperty.Enabled = True
                    End If
                End If
            End If

            If v_iIndex = GISSharedPropertyConstants.ACOGISListID Then
                lblGISListId.Font = VB6.FontChangeBold(lblGISListId.Font, True)
                If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = -1) Then
                    cboGISListId.Enabled = True
                End If
                'Reset the data type to string and disable it
                cboDataType.SelectedIndex = listIndexString
                cboDataType.Enabled = False

                'cboDataType.Sorted = True
            Else
                lblGISListId.Font = VB6.FontChangeBold(lblGISListId.Font, False)
                cboGISListId.Enabled = False
                cboGISListId.SelectedIndex = -1


            End If

            If v_iIndex = GISSharedPropertyConstants.ACOPMLookupTableName Then
                lblPMLookupTableName.Font = VB6.FontChangeBold(lblPMLookupTableName.Font, True)
                If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = -1) Then
                    cboPMLookupList.Enabled = True
                End If
                'Reset the data type and disable it
                cboDataType.SelectedIndex = listIndexInteger
                cboDataType.Enabled = False
            Else
                lblPMLookupTableName.Font = VB6.FontChangeBold(lblPMLookupTableName.Font, False)
                cboPMLookupList.Enabled = False
                cboPMLookupList.SelectedIndex = -1
            End If

            If v_iIndex = GISSharedPropertyConstants.ACOPartyTypeID Then
                lblPartyTypeId.Font = VB6.FontChangeBold(lblPartyTypeId.Font, True)
                If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = -1) Then
                    cboPartyTypeId.Enabled = True
                End If
                'Reset the data type to integer and disable it
                cboDataType.SelectedIndex = listIndexInteger
                cboDataType.Enabled = False
            Else
                lblPartyTypeId.Font = VB6.FontChangeBold(lblPartyTypeId.Font, False)
                cboPartyTypeId.Enabled = False
                cboPartyTypeId.SelectedIndex = -1
            End If

            If v_iIndex = GISSharedPropertyConstants.ACOSumInsuredTypeID Then
                lblPMUSumInsuredType.Font = VB6.FontChangeBold(lblPMUSumInsuredType.Font, True)
                If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = -1) Then
                    cboPMUSumInsuredType.Enabled = True
                End If
                'Reset the data type to integer and disable it
                cboDataType.SelectedIndex = listIndexInteger
                cboDataType.Enabled = False
            Else
                lblPMUSumInsuredType.Font = VB6.FontChangeBold(lblPMUSumInsuredType.Font, False)
                cboPMUSumInsuredType.Enabled = False
                cboPMUSumInsuredType.SelectedIndex = -1
            End If

            If v_iIndex = GISSharedPropertyConstants.ACOStdWordingType Then
                lblDocumentFilter.Font = VB6.FontChangeBold(lblDocumentFilter.Font, True)
                cboDocumentFilter.Enabled = True
                'Reset the data type to integer and disable it
                cboDataType.SelectedIndex = listIndexInteger
                cboDataType.Enabled = False
            Else
                lblDocumentFilter.Font = VB6.FontChangeBold(lblDocumentFilter.Font, False)
                cboDocumentFilter.Enabled = False
                cboDocumentFilter.SelectedIndex = -1
            End If


            If v_iIndex = GISSharedPropertyConstants.ACOGISUserDefHeaderID Then
                lblGISUserDefHeaderId.Font = VB6.FontChangeBold(lblGISUserDefHeaderId.Font, True)
                If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = -1) Then
                    cboGISUserDefHeaderId.Enabled = True
                End If
                'Reset the data type to integer and disable it
                cboDataType.SelectedIndex = listIndexInteger
                cboDataType.Enabled = False
                chkIsChaseCycleProperty.Enabled = True

            Else
                lblGISUserDefHeaderId.Font = VB6.FontChangeBold(lblGISUserDefHeaderId.Font, False)
                cboGISUserDefHeaderId.Enabled = False
                cboGISUserDefHeaderId.SelectedIndex = -1
                chkIsChaseCycleProperty.Enabled = False
                chkIsChaseCycleProperty.CheckState = False

            End If


            If v_iIndex = GISSharedPropertyConstants.ACOProductID Then
                lblProductId.Font = VB6.FontChangeBold(lblProductId.Font, True)
                If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = -1) Then
                    cboProductId.Enabled = True
                End If
                'Reset the data type to integer and disable it
                cboDataType.SelectedIndex = listIndexInteger
                cboDataType.Enabled = False
            Else
                lblProductId.Font = VB6.FontChangeBold(lblProductId.Font, False)
                cboProductId.Enabled = False
                cboProductId.SelectedIndex = -1
            End If

            If v_iIndex = GISSharedPropertyConstants.ACOSwiftCommonCode Then
                lblCommonCodeType.Font = VB6.FontChangeBold(lblCommonCodeType.Font, True)
                If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = -1) Then
                    cboCommonCode.Enabled = True
                End If
                'Reset the data type to string and disable it
                cboDataType.SelectedIndex = listIndexString
                cboDataType.Enabled = False
            Else
                lblCommonCodeType.Font = VB6.FontChangeBold(lblCommonCodeType.Font, False)
                cboCommonCode.Enabled = False
                cboCommonCode.SelectedIndex = -1
            End If

            If v_iIndex = GISSharedPropertyConstants.ACOSwiftListView Then
                lblSwiftListViewType.Font = VB6.FontChangeBold(lblSwiftListViewType.Font, True)
                If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = -1) Then
                    cboSpecialsSelector.Enabled = True
                End If
                'Reset the data type to text and disable it
                cboDataType.SelectedIndex = listIndexString
                cboDataType.Enabled = False
            Else
                lblSwiftListViewType.Font = VB6.FontChangeBold(lblSwiftListViewType.Font, False)
                cboSpecialsSelector.Enabled = False
                cboSpecialsSelector.SelectedIndex = -1
            End If

            If v_iIndex = GISSharedPropertyConstants.ACOComboLookup Then
                lblComboLookupTableName.Font = VB6.FontChangeBold(lblComboLookupTableName.Font, True)
                If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = -1) Then
                    txtComboLookupTableName.Enabled = True
                End If
                'Reset the data type and disable it
                cboDataType.SelectedIndex = listIndexString
                cboDataType.Enabled = False
            Else
                lblComboLookupTableName.Font = VB6.FontChangeBold(lblComboLookupTableName.Font, False)
                txtComboLookupTableName.Enabled = False
                txtComboLookupTableName.Text = ""
            End If


            'a few of these can be handled more efficiently with a case statement
            Select Case v_iIndex
                Case GISSharedPropertyConstants.ACOReserveID, GISSharedPropertyConstants.ACOPaymentID, GISSharedPropertyConstants.ACOCaseHeader, GISSharedPropertyConstants.ACOCaseClaimList
                    'Reset the data type to integer and disable it
                    cboDataType.SelectedIndex = listIndexInteger
                    cboDataType.Enabled = False

                Case GISSharedPropertyConstants.ACOSwiftSpecialType
                    SSTabHelper.SetTabVisible(tabMainTab, 1, True)
                    If optFieldType(GISSharedPropertyConstants.ACOSwiftNotes).Checked Then
                        'Reset the data type to text and disable it
                        cboDataType.SelectedIndex = listIndexComment
                    ElseIf (optFieldType(GISSharedPropertyConstants.ACOSwiftCommonCode).Checked Or optFieldType(GISSharedPropertyConstants.ACOSwiftListView).Checked Or optFieldType(GISSharedPropertyConstants.ACOComboLookup).Checked) Then
                        'Reset the data type to string and disable it
                        cboDataType.SelectedIndex = listIndexString
                    ElseIf optFieldType(GISSharedPropertyConstants.ACOPMLookupTableName).Checked Then
                        'Reset the data type and disable it
                        If (m_lSwiftIntegration And GISSharedPropertyConstants.SwiftMode_NotRenderedByPB) = GISSharedPropertyConstants.SwiftMode_NotRenderedByPB Then
                            cboDataType.SelectedIndex = listIndexString
                        Else
                            cboDataType.SelectedIndex = listIndexInteger
                        End If
                    Else
                        'Reset the data type to integer and disable it
                        cboDataType.SelectedIndex = listIndexInteger
                    End If
                    cboDataType.Enabled = False

                Case GISSharedPropertyConstants.ACOSwiftClientSelector, GISSharedPropertyConstants.ACOSwiftAddressSelector, GISSharedPropertyConstants.ACOSwiftAddress
                    SSTabHelper.SetTabVisible(tabMainTab, 1, True)
                    'Reset the data type to integer and disable it
                    cboDataType.SelectedIndex = listIndexInteger
                    cboDataType.Enabled = False

                Case GISSharedPropertyConstants.ACOSwiftNotes
                    SSTabHelper.SetTabVisible(tabMainTab, 1, True)
                    'Reset the data type to comment and disable it
                    cboDataType.SelectedIndex = listIndexComment
                    cboDataType.Enabled = False

                Case GISSharedPropertyConstants.ACOSwiftCommonCode, GISSharedPropertyConstants.ACOSwiftListView
                    SSTabHelper.SetTabVisible(tabMainTab, 1, True)
                    'Reset the data type to string and disable it
                    cboDataType.SelectedIndex = listIndexString
                    cboDataType.Enabled = False

                Case GISSharedPropertyConstants.ACOComboLookup
                    SSTabHelper.SetTabVisible(tabMainTab, 1, True)
                    ' note - the enabling of the control itself was handled earlier

                Case Else
                    SSTabHelper.SetTabVisible(tabMainTab, 1, False)

            End Select


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangeOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub cboDataType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDataType.SelectedIndexChanged

        If cboDataType.Text = "String" Then
            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_lGISPropertyId = 0) Then
                chkIsSearchProperty.Enabled = True
            End If
        Else
            chkIsSearchProperty.CheckState = CheckState.Unchecked
            chkIsSearchProperty.Enabled = False

        End If

        If cboDataType.Text = "Currency" OrElse cboDataType.Text = "Integer"  Then
            cboIndexLinking.Enabled = True
        Else
            If cboIndexLinking.Items.Count > 0 Then
                cboIndexLinking.SelectedIndex = 0
            Else
                cboIndexLinking.SelectedIndex = -1
            End If
            cboIndexLinking.Enabled = False

        End If
        If cboDataType.Text = "Comment" Then
            chkIsFormattedText.Enabled = True
        Else
            chkIsFormattedText.Enabled = False
        End If

    End Sub

    Private Sub Form_Initialize_Renamed()


        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iGISProperty.General()

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
                    ' developer guide no. 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
                'Modified by Archana Tokas on 31/03/2010 02:25:20 commented with if refer no solution 19
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
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private isInitializingComponent As Boolean
    Private Sub optFieldType_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optFieldType_18.CheckedChanged, _optFieldType_17.CheckedChanged, _optFieldType_16.CheckedChanged, _optFieldType_13.CheckedChanged, _optFieldType_10.CheckedChanged, _optFieldType_12.CheckedChanged, _optFieldType_11.CheckedChanged, _optFieldType_20.CheckedChanged, _optFieldType_19.CheckedChanged, _optFieldType_15.CheckedChanged, _optFieldType_14.CheckedChanged, _optFieldType_9.CheckedChanged, _optFieldType_8.CheckedChanged, _optFieldType_1.CheckedChanged, _optFieldType_2.CheckedChanged, _optFieldType_3.CheckedChanged, _optFieldType_4.CheckedChanged, _optFieldType_6.CheckedChanged, _optFieldType_5.CheckedChanged, _optFieldType_0.CheckedChanged, _optFieldType_7.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            Dim Index As Integer = Array.IndexOf(optFieldType, eventSender)

            m_lReturn = CType(ChangeOption(v_iIndex:=Index), gPMConstants.PMEReturnCode)

        End If
        Dim Index1 As Integer = Array.IndexOf(optFieldType, eventSender)
        If m_lGISDataModelTypeID = 1 And cboDataType.Text = "Integer" And Index1 = GISSharedPropertyConstants.ACOGISUserDefHeaderID Then
            chkIsChaseCycleProperty.Enabled = True
        Else
            chkIsChaseCycleProperty.Enabled = False
        End If

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

        Dim iTab As Integer

        Try




            'now we have 2 tabs ensure we are the first incase the mandatory field check fails
            SSTabHelper.SetSelectedIndex(tabMainTab, 0) : Application.DoEvents()

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' validate data entered.
            m_lReturn = CType(ValidateForm(iTab), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

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

    Private Sub txtColumnName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtColumnName.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtColumnName)
    End Sub

    Private Sub txtColumnName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtColumnName.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtColumnName)
    End Sub

    Private Sub cboPMLookupList_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMLookupList.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboPMLookupList)
    End Sub

    Private Sub cboPMLookupList_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMLookupList.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=cboPMLookupList)
    End Sub

    Private Sub txtComboLookupTableName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComboLookupTableName.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtComboLookupTableName)
    End Sub

    Private Sub txtComboLookupTableName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComboLookupTableName.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtComboLookupTableName)
    End Sub

    Private Sub txtPolarisPropertyId_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolarisPropertyId.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPolarisPropertyId)
    End Sub

    Private Sub txtPolarisPropertyId_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolarisPropertyId.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPolarisPropertyId)
    End Sub

    Private Sub txtPropertyName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPropertyName.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPropertyName)
    End Sub

    Private Sub txtPropertyName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPropertyName.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPropertyName)

        If txtColumnName.Text = "" Then
            txtColumnName.Text = txtPropertyName.Text
        End If
    End Sub


    ' PRIVATE Events (End)


End Class
