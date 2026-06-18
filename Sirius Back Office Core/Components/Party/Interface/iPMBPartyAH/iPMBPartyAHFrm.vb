Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
'public class is required
Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 11/08/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '
    'developer guide no. 69
    Public m_ofrmTransfer As frmTransfer


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_sStepStatus As String = ""
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_sMainPostCode As String = ""
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPartyAH.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    'CMG/PB 15/7/2002 Add address tab
    Private m_vAddresses(,) As Object
    Private m_vAddressTypes(,) As Object
    Private m_lAddressCount As Integer
    Private m_iMainAddressIndex As Integer
    Private m_vCommissionAccounts As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Instance of gemini list manager
    Private m_oGEMListManager As iGEMListManager.Interface_Renamed

    ' Instance of Contact

    Private m_oContact As iPMBContact.Interface_Renamed

    ' Contact details
    Private m_vContacts(,) As Object

    ' CMG/PB 15/7/2002 Declare an instance of the address interface.

    Private m_oAddress As iPMBAddress.Interface_Renamed

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    'Check NRMA for NOT POstcode
    Private m_bIsNRMA As Boolean
    Private m_bLinkToCommission As Boolean

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}

    Private m_lPartyCnt As Integer
    'CMG/PB 15/07/2002 Address Tab
    Private m_lAddressCnt As Integer
    Private m_sClientCode As String = ""
    Private m_sLastName As String = ""
    Private m_sForeName As String = ""
    Public m_lCurrencyID As Integer
    Private m_lDepartmentID As Integer
    Private m_sTitle As String = ""
    Private m_sInitials As String = ""
    'EK 22/10/99 Add resolved Name
    Private m_sResolvedname As String = ""
    ' SJP(CMG) 010402004 PS235
    Private m_sCommissionCnt As String = ""

    'CMG/PB 15/7/2002
    Private m_lAddressUsageTypeID As Integer

    'RWH(24/07/2000) RSAIB Process 004.
    Private m_iDefaultCountryID As Integer
    Private m_sDefaultCountryCode As String = ""

    Private m_iLine As Integer
    Private m_lContactCnt As Integer
    'EK 12/10/99
    Private m_sHandlerType As String = ""

    'Maintain Party Code
    Private m_bIsSetMaskingCode As Boolean
    Private m_bIsReadOnly As Boolean
    Private m_sMaskCode As String = ""
    Private m_bIsViewOnlyAHMaintenance As Boolean
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

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

    Public WriteOnly Property IsNRMA() As Boolean
        Set(ByVal Value As Boolean)

            m_bIsNRMA = Value

        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get
            Return m_sStepStatus
        End Get
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
    'EK 12/10/99
    Public Property HandlerType() As String
        Get
            Return m_sHandlerType
        End Get
        Set(ByVal Value As String)
            m_sHandlerType = Value
        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Public Property LastName() As String
        Get
            Return m_sLastName
        End Get
        Set(ByVal Value As String)
            m_sLastName = Value
        End Set
    End Property
    'DC260903 -PS256 fsa compliance
    Public Property ResolvedName() As String
        Get
            Return m_sResolvedname
        End Get
        Set(ByVal Value As String)
            m_sResolvedname = Value
        End Set
    End Property

    Public Property ForeName() As String
        Get
            Return m_sForeName
        End Get
        Set(ByVal Value As String)
            m_sForeName = Value
        End Set
    End Property
    Public Property CurrencyID() As Integer
        Get
            Return m_lCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_lCurrencyID = Value
        End Set
    End Property
    Public Property DepartmentID() As Integer
        Get
            Return m_lDepartmentID
        End Get
        Set(ByVal Value As Integer)
            m_lDepartmentID = Value
        End Set
    End Property
    Public Property Title() As String
        Get
            Return m_sTitle
        End Get
        Set(ByVal Value As String)
            m_sTitle = Value
        End Set
    End Property
    Public Property Initials() As String
        Get
            Return m_sInitials
        End Get
        Set(ByVal Value As String)
            m_sInitials = Value
        End Set
    End Property
    Public Property ClientCode() As String
        Get
            Return m_sClientCode
        End Get
        Set(ByVal Value As String)
            m_sClientCode = Value
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

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClientCode, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLastname, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringCase, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'EK 19/10/99 Set as Mandatory
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtForename, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringCase, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboDepartment, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTitle, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringCase, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInitials, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringUpper, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetDetails(vPartyCnt:=m_lPartyCnt)

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            If Task = gPMConstants.PMEComponentAction.PMEdit Or Task = gPMConstants.PMEComponentAction.PMView Then
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

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtClientCode, vControlValue:=m_sClientCode)
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtLastname, vControlValue:=m_sLastName)
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtForename, vControlValue:=m_sForeName)
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInitials, vControlValue:=m_sInitials)

                ' Set the currency
                cboCurrency.ItemId = m_lCurrencyID

                ' Department
                cboDepartment.ItemId = m_lDepartmentID

                ' Title
                cboTitle.Text = m_sTitle

                ' Resize the list view columns
                'developer guide no. 178
                m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwContacts, True, True), gPMConstants.PMEReturnCode)

                'Fill the contact grid
                PopulateContacts()

                'CMG/PB 16072002 Fill the address grid
                PopulateAddresses()

                ' {* USER DEFINED CODE (End) *}
            End If

            ''Start(Saurabh Agrawal) Tech Spec LOA008 AccountHandlers(5.2.2.1)
            Dim Key As uctPickList.PickListKey
            Key = New uctPickList.PickListKey()
            Key.KeyName = "PartyCnt"
            Key.ValueType = gPMConstants.PMEDataType.PMLong

            uctPickListBranches.ForeignKeys.Add(Key, Key:="PartyCnt")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "user_id"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(Key, Key:="user_id")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "unique_id"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListBranches.ForeignKeys.Add(Key, Key:="unique_id")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "screen_hierarchy"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListBranches.ForeignKeys.Add(Key, Key:="screen_hierarchy")

            uctPickListBranches.ForeignKeys.Item("PartyCnt").Value = m_lPartyCnt
            'developer guide no. 68
            m_lReturn = uctPickListBranches.Load_Renamed

            ''End(Saurabh Agrawal) Tech Spec LOA008 AccountHandlers(5.2.2.1)
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
        Dim lBusinessDataID As Integer

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
            lBusinessDataID = 1
            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If
            If m_sHandlerType = "AH" Then
                m_sScreenHierarchy = $"Account Handler({m_sClientCode.Trim()})"
            Else
                m_sScreenHierarchy = $"Account Executive({m_sClientCode.Trim()})"
            End If
            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vShortName:=m_sClientCode, vName:=m_sLastName, vForename:=m_sForeName, vCurrencyID:=m_lCurrencyID, vDepartmentID:=m_lDepartmentID, vPartyTitleCode:=m_sTitle, vInitials:=m_sInitials, vResolvedName:=m_sResolvedname, vCommissionCnt:=m_sCommissionCnt, sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy)

                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vShortName:=m_sClientCode, vName:=m_sLastName, vForename:=m_sForeName, vCurrencyID:=m_lCurrencyID, vDepartmentID:=m_lDepartmentID, vPartyTitleCode:=m_sTitle, vInitials:=m_sInitials, vResolvedName:=m_sResolvedname, vCommissionCnt:=m_sCommissionCnt, sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy)

                    ' {* USER DEFINED CODE (End) *}

            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If
            ''Start(Saurabh Agrawal) Tech Spec LOA008 Account Handlers(5.2.2.2)
            If m_sHandlerType = "AH" Then
                If Task = gPMConstants.PMEComponentAction.PMEdit Then
                    m_lReturn = CType(SaveBranchPickListData(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("InterfaceToBusiness", "SaveBranchPickListData", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If
            ''End(Saurabh Agrawal) Tech Spec LOA008 Account Handlers(5.2.2.2)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try


            ' Get the lookup values.

            ' m_lReturn& = GetLookupValues()
            '
            ' ' Check for errors.
            ' If (m_lReturn& <> PMTrue) Then
            '     DisplayLookupDetails = PMFalse
            '     Exit Function
            ' End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: PopulateAddresses
    '
    ' Description: Fills the grid control with address details
    '
    ' CMG/PB 16072002 Add address tab to Account Executive
    ' ***************************************************************** '
    Private Sub PopulateAddresses()
        'RWH(21/07/2000) Altered to move PostCode from far left to far
        'right of ListView.

        Dim k As Integer
        Dim oListItem As ListViewItem
        Dim sAddressUsage As String = ""

        Try

            'Just go if no addresses
            If Not Information.IsArray(m_vAddresses) Then
                Exit Sub
            End If
            lvwAddress.Items.Clear()

            m_lAddressCount = 0

            ' Assign the details to the interface.
            For i As Integer = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                'First column.
                For k = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                    If m_vAddresses(1, i).Equals(m_vAddressTypes(0, k)) Then
                        'RWH(24/07/2000)
                        sAddressUsage = CStr(m_vAddressTypes(1, k)).Trim()
                        Exit For
                    End If
                Next k
                'See if this is the main address
                If CStr(m_vAddressTypes(2, k)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                    m_sMainPostCode = CStr(m_vAddresses(0, i))
                    m_iMainAddressIndex = CInt(m_vAddressTypes(0, k))
                End If

                'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        ' Assign the details to the first column.
                        ' Postcode

                        oListItem = lvwAddress.Items.Add(CStr(m_vAddresses(0, i)).Trim(), ACIADDRESS)

                        ' Assign details to other the columns
                        ' Address Usage
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = sAddressUsage
                        ' Address Line 1
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vAddresses(2, i)).Trim()
                        ' Address Line 2
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vAddresses(3, i)).Trim()
                        ' Address Line 3
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vAddresses(4, i)).Trim()
                        ' Address Line 4
                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vAddresses(5, i)).Trim()

                    Case Else
                        ' Assign the details to the first column.
                        ' Address Usage

                        oListItem = lvwAddress.Items.Add(sAddressUsage, ACIADDRESS)

                        ' Assign details to other the columns
                        ' Address Line 1
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAddresses(2, i)).Trim()
                        ' Address Line 2
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vAddresses(3, i)).Trim()
                        ' Address Line 3
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vAddresses(4, i)).Trim()
                        ' Address Line 4
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vAddresses(5, i)).Trim()
                        ' Postcode
                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vAddresses(0, i)).Trim()

                End Select

                ' Store the Address_cnt
                oListItem.Tag = CStr(m_vAddresses(6, i)).Trim()
                ' {* USER DEFINED CODE (End) *}
                m_lAddressCount += 1
                ' Set the tag property with the index of
                ' the search data storage.

            Next i
            'developer guide no. 178
            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwAddress, True), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: PopulateContacts
    '
    ' Description: Fills the grid control with contact details
    '
    ' ***************************************************************** '
    Private Sub PopulateContacts()

        Dim oListItem As ListViewItem

        Try

            If Not Information.IsArray(m_vContacts) Then
                Exit Sub
            End If

            lvwContacts.Items.Clear()

            ' Assign the details to the interface.
            For i As Integer = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                oListItem = lvwContacts.Items.Add(CStr(m_vContacts(1, i)).Trim())

                ' Assign details to other the columns
                ' Column 2
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vContacts(2, i)).Trim()

                ' Column 3
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vContacts(3, i)).Trim()

                ' Column 4
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vContacts(4, i)).Trim()

                ' Column 5
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vContacts(5, i)).Trim()


                ' Store the Contact_cnt
                oListItem.Tag = CStr(m_vContacts(0, i)).Trim()
                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.

            Next i
            '    'Populate the cells

            ' Size the list view
            'developer guide no. 178
            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwContacts), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vForename:=m_sForeName, vInitials:=m_sInitials, vDepartmentID:=m_lDepartmentID, vPartyTitleCode:=m_sTitle, vShortName:=m_sClientCode, vName:=m_sLastName, vCurrencyID:=m_lCurrencyID, vResolvedName:=m_sResolvedname, vCommissionCnt:=m_sCommissionCnt)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If


            m_lReturn = m_oBusiness.GetContactDetails(vPartyCnt:=m_lPartyCnt, vContacts:=m_vContacts)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the contact details.", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'CMG/PB 16072002 Add Address tab to Account Executive
            'Get addresses for the party
            ''Saurabh
            If m_sHandlerType <> "AH" Then

                m_lReturn = m_oBusiness.GetAddressDetails(vPartyCnt:=m_lPartyCnt, vAddresses:=m_vAddresses)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
                End If
            End If

            ' {* USER DEFINED CODE (End) *}

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

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            m_sClientCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtClientCode))

            m_sLastName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtLastname))

            m_sForeName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtForename))

            m_sInitials = CStr(m_oFormFields.UnformatControl(ctlControl:=txtInitials))

            m_lCurrencyID = cboCurrency.ItemId
            m_lDepartmentID = cboDepartment.ItemId

            ' Get the text manually, because Form Controls will just return the
            ' list index for combo's otherwise
            m_sTitle = cboTitle.Text
            'EK 22/10/99 resolved Name
            m_sResolvedname = m_sTitle.Trim() & " " & m_sInitials.Trim() & " " & m_sLastName.Trim()

            If cboCommissionAccount.SelectedIndex > 0 Then
                m_sCommissionCnt = CStr(VB6.GetItemData(cboCommissionAccount, cboCommissionAccount.SelectedIndex))
            Else
                m_sCommissionCnt = CStr(0)
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
            Select Case m_sHandlerType
                Case ACHandler
                    pnlType.Text = "Account Handler"
                    Me.Text = "Account Handler"
                    'CMG/PB Hide the Address Tab in Account Handler mode
                    SSTabHelper.SetTabVisible(tabMainTab, 2, gPMConstants.PMEReturnCode.PMFalse)

                    'Hide the Next button in Account Handler because Address tab is
                    'Account Executive only
                    ''Saurabh Changed PMFalse to PMTrue
                    cmdNext(1).Visible = gPMConstants.PMEReturnCode.PMTrue
                    SSTabHelper.SetTabVisible(tabMainTab, 3, gPMConstants.PMEReturnCode.PMTrue)

                Case ACExecutive
                    pnlType.Text = "Account Executive"
                    Me.Text = "Account Executive"

                    SSTabHelper.SetTabVisible(tabMainTab, 3, gPMConstants.PMEReturnCode.PMFalse)
                    'tabMainTab.SelectedIndex = 2
                    'DC260903 -PS256 fsa compliance
                Case ACExecHandler
                    pnlType.Text = "Executive Handler"
                    Me.Text = "Executive Handler"
                    SSTabHelper.SetTabVisible(tabMainTab, 3, gPMConstants.PMEReturnCode.PMFalse)

            End Select

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

            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwContacts.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If
            lvwContacts.FullRowSelect = True
            lvwAddress.FullRowSelect = True


            ' Disable edit and delete
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False

            lblCommissionAccount.Visible = False
            cboCommissionAccount.Visible = False


            '2005 Roadmap
            'Hide the transfer button when creating new accounts
            If m_lPartyCnt = 0 Then
                cmdTransfer.Enabled = False
                cmdTransfer.Visible = False
            End If
            If Task = gPMConstants.PMEComponentAction.PMView Then
                cmdTransfer.Enabled = False
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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            'CMG PB 16072002 Only Executive has address tab
            Select Case HandlerType
                Case ACHandler, ACExecHandler
                    ReDim m_ctlTabFirstLast(1, 1)
                Case ACExecutive
                    ReDim m_ctlTabFirstLast(1, 2)
                    'DC260903 -PS256 fsa compliance
            End Select

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

            m_ctlTabFirstLast(ACControlStart, 0) = txtClientCode
            m_ctlTabFirstLast(ACControlEnd, 0) = txtInitials

            m_ctlTabFirstLast(ACControlStart, 1) = lvwContacts
            m_ctlTabFirstLast(ACControlEnd, 1) = cmdDelete

            'CMG/PB 16072002 Address tab only visible in Account Executive
            If HandlerType = ACExecutive Then
                m_ctlTabFirstLast(ACControlStart, 2) = lvwAddress
                m_ctlTabFirstLast(ACControlEnd, 2) = cmdEditAd
            End If
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
        Dim sPostCode, sAddressUsage, sAddressLine1, sAddressLine2, sAddressLine3, sAddressLine4 As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            'developer guide no. 243
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


            'developer guide no. 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'CMG/PB 16072002 Added listview in Address Tab (Account Exec only)

            sAddressUsage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListUsage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine1 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine2 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine3 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine4 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sPostCode = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListPostCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'CMG/PB 15/7/2002
            'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
            With lvwAddress
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        .Columns.Item(0).Text = sPostCode
                        .Columns.Item(1).Text = sAddressUsage
                        .Columns.Item(2).Text = sAddressLine1
                        .Columns.Item(3).Text = sAddressLine2
                        .Columns.Item(4).Text = sAddressLine3
                        .Columns.Item(5).Text = sAddressLine4

                    Case Else
                        .Columns.Item(0).Text = sAddressUsage
                        .Columns.Item(1).Text = sAddressLine1
                        .Columns.Item(2).Text = sAddressLine2
                        .Columns.Item(3).Text = sAddressLine3
                        .Columns.Item(4).Text = sAddressLine4
                        .Columns.Item(5).Text = sPostCode

                End Select
            End With

            'EK 15/11/99
            Select Case HandlerType
                Case ACHandler

                    'developer guide no. 243
                    SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    ''Start(Saurabh Agrawal)Tech spec LOA008 Account Handlers

                    SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    ''End(Saurabh Agrawal)Tech spec LOA008 Account Handlers
                Case ACExecutive

                    SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    'DC260903 -PS256 fsa compliance
                Case ACExecHandler

                    SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            End Select


            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


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
            'EK 15/11/99
            Select Case HandlerType
                Case "AH"

                    lblClientCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Case "CO"

                    lblClientCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExecCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    'DC260903 -PS256 fsa compliance
                Case "HC"

                    lblClientCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExecCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            End Select

            lblLastname.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLastname, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblForename.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACForename, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDepartment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDepartment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTitle.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblInitials.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInitials, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCommissionAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCommissionAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAdd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEdit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelete, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupValues() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Gets all of the lookup values.
    '
    ' Check the task.
    'Select Case (m_iTask)
    'Case gPMConstants.PMEComponentAction.PMAdd
    ' Get all of the lookup values.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMEdit
    ' Get all of the lookup values with the correct
    ' effective date.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMView
    ' Get lookup values for viewing only.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    'End Select
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
    '
    'Return result
    'End If
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    '
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    '
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    'SP150998 - compare long value not string
    ' Check if this is the selected index.
    'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
    'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'End If
    '
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

    'ctlLookup.ListIndex = 0
    'End If
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ************************************************************************** '
    '
    ' Fills combo box with polaris list via ComponentManager
    '
    ' History: original by SJ
    '          modified heavily for S4B by CF - 7/5/99
    '
    ' ************************************************************************** '
    Public Function FillCombo(ByRef cboControl As ComboBox, ByRef sPropertyID As String) As Integer

        Dim result As Integer = 0
        Dim vListArray() As Object
        Dim lNumItems As Integer
        Dim sText As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With cboControl

                ' Save text
                sText = .Text

                ' Mouse pointer to busy while it re-loads the list
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                m_lReturn = m_oGEMListManager.PopulateListControl(v_sPropertyId:=sPropertyID, r_oControl:=cboControl)

                ' Reset the mouse pointer back to normal
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = CType(m_oGEMListManager.GetList(v_sPropertyId:=sPropertyID, r_vListData:=vListArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get list from List Manager", vApp:=ACApp, vClass:=ACClass, vMethod:="FillCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If

                ' Put the list into the Array

                lNumItems = vListArray.GetUpperBound(0)
                If Information.IsArray(vListArray) Then
                    .Items.Clear()
                    .Items.Add(" ")

                    For lItem As Integer = 0 To lNumItems

                        .Items.Add(CStr(vListArray(lItem)).Trim())

                    Next
                End If

                'sj 15/02/99 - end

                ' Restore text
                If .DropDownStyle = ComboBoxStyle.DropDown Then
                    .Text = sText
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub cboCommissionAccount_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCommissionAccount.SelectedIndexChanged
        If cboCommissionAccount.SelectedIndex = 0 Then
            cboCommissionAccount.SelectedIndex = -1
        End If
    End Sub

    Private Sub cboCurrency_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.GotFocus
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboCurrency)
    End Sub

    Private Sub cboCurrency_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.LostFocus
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=cboCurrency)
    End Sub

    Private Sub cboCommissionAccount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCommissionAccount.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboCommissionAccount)
    End Sub

    Private Sub cboCommissionAccount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCommissionAccount.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=cboCommissionAccount)
    End Sub

    Private Sub cboTitle_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTitle.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboTitle)
    End Sub

    Private Sub cboTitle_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTitle.Leave


        Dim bFound As Boolean = False
        Dim sText As String = cboTitle.Text.ToUpper()

        For lCount As Integer = 0 To cboTitle.Items.Count - 1

            ' See if the value matches the list item
            If VB6.GetItemString(cboTitle, lCount).ToUpper() = sText Then
                ' If so, set the value to the list item
                cboTitle.Text = VB6.GetItemString(cboTitle, lCount)
                bFound = True
                Exit For
            End If

        Next

        If Not bFound Then
            cboTitle.Text = ""
        End If

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=cboTitle)
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        Dim oListItem As ListViewItem

        Try

            'Create icontact if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contacts", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'set the main postcode and reference

            m_lReturn = CType(m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_lReturn = m_oContact.ContactCnt


            m_oContact.Reference = txtClientCode.Text

            m_oContact.PostCode = m_sMainPostCode

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_sHandlerType = "AH" Then
                m_sScreenHierarchy = $"Account Handler({m_sClientCode.Trim()})"
            Else
                m_sScreenHierarchy = $"Account Executive({m_sClientCode.Trim()})"
            End If

            m_oContact.UniqueId = m_sUniqueId
            m_oContact.ScreenHeirarchy = m_sScreenHierarchy

            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Me.Refresh


            oListItem = lvwContacts.Items.Add(m_oContact.AreaCode)

            ' Assign details to other the columns
            ' Column 2
            'Temporary thing

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description

            ' Store the Address_cnt

            oListItem.Tag = m_oContact.ContactCnt

            ' Size the list view
            'developer guide no. 178
            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwContacts), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: cmdAddAd_Click
    '
    ' Description: Add Address
    '
    'CMG/PB 16072002
    ' ***************************************************************** '
    Private Sub cmdAddAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAd.Click
        'RWH(21/07/2000) Altered to move PostCode from far left to far
        'right of ListView.

        Dim sTmp As String = ""


        Dim oListItem As ListViewItem

        Try

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Create icontact if not already done so
            If m_oAddress Is Nothing Then

                ' Get an instance of the address interface object via
                ' the public object manager.
                Dim temp_m_oAddress As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAddress, sClassName:="iPMBAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAddress = temp_m_oAddress

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Reset the moust pointer
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'set the main postcode and reference

            m_oAddress.Reference = txtClientCode.Text
            'PSL 20/02/2003  NRMA don't have postcodes

            m_oAddress.IsNRMA = m_bIsNRMA


            m_oAddress.PostCode = m_sMainPostCode

            m_lReturn = CType(m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_sScreenHierarchy = "" Then
                m_sScreenHierarchy = $"Account Executive({txtClientCode.Text.Trim()})"
            End If

            m_oAddress.UniqueId = m_sUniqueId
            m_oAddress.ScreenHeirarchy = m_sScreenHierarchy

            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to list

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
            Select Case (m_sDefaultCountryCode.Trim())
                Case "GBR"
                    ' Add the data to the list view


                    oListItem = lvwAddress.Items.Add(m_oAddress.PostalCode, ACIADDRESS)
                    ' Address Usage

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.AddressUsageType
                    ' Address line 1

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address1
                    ' Address line 2

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address2
                    ' Address line 3

                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address3
                    ' Address line 4

                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.Address4
                Case Else
                    ' Add the data to the list view


                    oListItem = lvwAddress.Items.Add(m_oAddress.AddressUsageType, ACIADDRESS)
                    ' Address line 1

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.Address1
                    ' Address line 2

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address2
                    ' Address line 3

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address3
                    ' Address line 4

                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address4
                    ' Postcode

                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.PostalCode
            End Select

            ' Store the Address_cnt

            oListItem.Tag = m_oAddress.AddressCnt


            If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

                m_sMainPostCode = m_oAddress.PostalCode
            End If
            'developer guide no. 178
            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwAddress, True), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        Try


            'Maintain Party Code
            If m_bIsSetMaskingCode And m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = ValidateNumberingScheme()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                m_lReturn = GeneratePartyCode()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            'Maintain Party Code
            If m_bIsReadOnly Then
                lblClientCode.Enabled = False
                txtClientCode.Enabled = False
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Validate some address stuff
            m_lReturn = CType(ValidateOK(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            Else
                cmdApply.Visible = False
            End If




        Catch ex As Exception
            'Error Log

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Exit Sub

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Try

            'Set row to be deleted - if a valid one selected
            If lvwContacts.Items.Count < 1 Then
                Exit Sub
            End If

            'Create contact component if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contact component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = CType(m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the contact id



            m_oContact.ContactCnt = Convert.ToString(lvwContacts.FocusedItem.Tag)


            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False

            lvwContacts.Items.RemoveAt(m_iLine - 1)

            ' Size the list view
            'developer guide no. 178
            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwContacts), gPMConstants.PMEReturnCode)
            If (pnlType.Text.ToLower = "account executive") Then
                tabMainTab.SelectedIndex = 2
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: cmdDeleteAd_Click
    '
    ' Description: Delete Address
    '
    'CMG/PB 16072002
    ' ***************************************************************** '
    Private Sub cmdDeleteAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAd.Click

        Try

            'Set row to be deleted - if a valid one selected
            If lvwAddress.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oAddress Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oAddress As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAddress, sClassName:="iPMBAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAddress = temp_m_oAddress

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = CType(m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the address id


            m_oAddress.AddressCnt = m_lAddressCnt

            m_oAddress.AddressUsageTypeID = m_lAddressUsageTypeID

            ''PN_70707 Start
            '    m_lReturn& = m_oAddress.Start()
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Exit Sub
            '    End If
            ''PN_70707 End

            'If not cancelled, edit grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Update the address details
            ' & postcode

            lvwAddress.Items.RemoveAt(m_iLine - 1)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim oListItem As ListViewItem

        Try

            'Set row to be deleted - if a valid one selected
            If lvwContacts.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contact component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = CType(m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the contact id

            m_oContact.ContactCnt = m_lContactCnt

            m_oContact.Reference = txtClientCode.Text


            m_oContact.PostCode = m_sMainPostCode

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_sHandlerType = "AH" Then
                m_sScreenHierarchy = $"Account Handler({m_sClientCode.Trim()})"
            Else
                m_sScreenHierarchy = $"Account Executive({m_sClientCode.Trim()})"
            End If

            m_oContact.UniqueId = m_sUniqueId
            m_oContact.ScreenHeirarchy = m_sScreenHierarchy

            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False

            oListItem = lvwContacts.Items.Item(m_iLine - 1)


            oListItem.Text = m_oContact.AreaCode
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description

            ' Size the list view
            'developer guide no. 178
            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwContacts), gPMConstants.PMEReturnCode)
            If (pnlType.Text.ToLower = "account executive") Then
                tabMainTab.SelectedIndex = 2
            End If
        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: cmdEditAd_Click
    '
    ' Description: Edit Address
    '
    'CMG/PB 16072002
    ' ***************************************************************** '
    Private Sub cmdEditAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditAd.Click
        'RWH(21/07/2000) Altered to move PostCode from far left to far
        'right of ListView.

        Dim lAddressCnt As Integer
        Dim sTmp As String = ""
        Dim oListItem As ListViewItem
        Dim sAddressUsage As String = ""

        Try

            'Set the address count being edited - if a valid one selected
            If lvwAddress.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oAddress Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                'RWH(24/07/2000) Changed from 'iSIRAddress.Interface'
                Dim temp_m_oAddress As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAddress, sClassName:="iPMBAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAddress = temp_m_oAddress

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            '    'set the main postcode and reference

            m_oAddress.Reference = txtClientCode.Text
            '    m_oAddress.PostCode = pnlIDPostCode
            'PSL 20/02/2003  NRMA don't have postcodes

            m_oAddress.IsNRMA = m_bIsNRMA

            m_lReturn = CType(m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            oListItem = lvwAddress.FocusedItem

            'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
            Select Case (m_sDefaultCountryCode.Trim())
                Case "GBR"

                    m_oAddress.PostCode = oListItem.Text
                    sAddressUsage = ListViewHelper.GetListViewSubItem(oListItem, 1).Text
                Case Else

                    m_oAddress.PostCode = ListViewHelper.GetListViewSubItem(oListItem, 5).Text
                    sAddressUsage = oListItem.Text
            End Select

            For k As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                If sAddressUsage = CStr(m_vAddressTypes(1, k)) Then

                    m_oAddress.AddressUsageTypeID = m_vAddressTypes(0, k)
                    Exit For
                End If
            Next k

            ' Get the address count

            lAddressCnt = Convert.ToString(lvwAddress.FocusedItem.Tag)

            'set the address id

            m_oAddress.AddressCnt = lAddressCnt

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_sScreenHierarchy = "" Then
                m_sScreenHierarchy = $"Account Executive({txtClientCode.Text.Trim()})"
            End If

            m_oAddress.UniqueId = m_sUniqueId
            m_oAddress.ScreenHeirarchy = m_sScreenHierarchy

            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            With lvwAddress.Items.Item(m_iLine - 1)
                'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        ' Postcode

                        .Text = m_oAddress.PostalCode
                        ' Address usage type

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 1).Text = m_oAddress.AddressUsageType
                        ' Address lines 1-4

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 2).Text = m_oAddress.Address1

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 3).Text = m_oAddress.Address2

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 4).Text = m_oAddress.Address3

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 5).Text = m_oAddress.Address4
                    Case Else
                        ' Address usage type

                        .Text = m_oAddress.AddressUsageType
                        ' Address lines 1-4

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 1).Text = m_oAddress.Address1

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 2).Text = m_oAddress.Address2

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 3).Text = m_oAddress.Address3

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 4).Text = m_oAddress.Address4
                        'Postcode

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 5).Text = m_oAddress.PostalCode
                End Select
                ' Store the AddressCnt in the tag


                .Tag = m_oAddress.AddressCnt
            End With
            'developer guide no. 178
            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwAddress, True), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdPrev_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrev_2.Click, _cmdPrev_1.Click, _cmdPrev_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPrev, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                ''Saurabh
                If Index = 2 Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, Index - 1)
                Else
                    SSTabHelper.SetSelectedIndex(tabMainTab, Index)
                End If
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    Private Sub cmdTransfer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTransfer.Click
        'developer guide no. 69
        If IsNothing(m_ofrmTransfer) Then
            m_ofrmTransfer = New iPMBPartyAH.frmTransfer
        End If
        m_lReturn = m_ofrmTransfer.Initialise()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'developer guide no. 69
            m_ofrmTransfer = Nothing
            Exit Sub
        End If

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        If m_sHandlerType = "AH" Then
            m_sScreenHierarchy = $"Account Handler({m_sClientCode.Trim()})"
        Else
            m_sScreenHierarchy = $"Account Executive({m_sClientCode.Trim()})"
        End If
        ' Pass standard details to form properties
        'developer guide no. 69
        With m_ofrmTransfer
            .OldHandlerCnt = m_lPartyCnt
            .OldHandlerType = m_sHandlerType
            .OldHandlerRef = m_sClientCode
            .UniqueId = m_sUniqueId
            .ScreenHierarchy = m_sScreenHierarchy
        End With
        'developer guide no. 69
        m_ofrmTransfer.ShowForm(FormShowConstants.Modal)
        ' developer guide no. 69
        m_ofrmTransfer.Close()
        'developer guide no. 69
        m_ofrmTransfer = Nothing

    End Sub

    Private Function GetUserAuthorities() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim oUserAuthorities As bACTUserAuthorities.Business
            Dim temp_oUserAuthorities As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oUserAuthorities = temp_oUserAuthorities

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", excep:=New Exception(Information.Err().Description))
                Return result
            End If


            m_lReturn = oUserAuthorities.GetValueFromTable(v_sTableName:="User_Authorities", v_vReturnColumn:="is_view_only_account_handler_maintenance", v_sKeyColumn:="user_id", v_sKeyValue:=g_oObjectManager.UserID, v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=m_bIsViewOnlyAHMaintenance)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Execute GetPartyViewOptions", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", excep:=New Exception(Information.Err().Description))
                Return result
            Else
                'Start - Prakash Varghese - PN 61117
                'Modified the condition ToSafeBoolean(m_bIsViewOnlyAHMaintenance) = True
                'to ToSafeBoolean(m_bIsViewOnlyAHMaintenance) since it is giving problems in runtime
                If gPMFunctions.ToSafeBoolean(m_bIsViewOnlyAHMaintenance) Then
                    m_iTask = gPMConstants.PMEComponentAction.PMView
                End If
                'End - Prakash Varghese - PN 61117
            End If

            'Terminate the object

            oUserAuthorities.Dispose()
            oUserAuthorities = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthoritiesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyAH.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            'EK 12/10/99
            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBPartyAH.General()

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

            ' Initialise Gemini List Manager
            m_oGEMListManager = New iGEMListManager.Interface_Renamed()

            'developer guide no. 9
            m_lReturn = m_oGEMListManager.Initialise()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            m_lReturn = m_oGEMListManager.CheckListVersions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' PWF 12/08/2002 - Moved from BusinessToData as it is also required for Add!
            ' Get address type lookups for the party

            m_lReturn = m_oBusiness.GetAddresstypelookups(vaddresstypes:=m_vAddressTypes)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            m_ofrmTransfer = New frmTransfer
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

            ' Get User Authorities for Setting up the mode
            m_lReturn = GetUserAuthorities()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If
            'developer guide no. 38
            Me.cboDepartment.FirstItem = "(Not Known)"
            Me.cboCurrency.FirstItem = ""

            Me.cboCurrency.ItemId = m_oBusiness.m_iCurrencyID
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

            'SJP (CMG) 01/04/2003 - PS235
            m_lReturn = CType(GetHiddenOptions(v_lSourceId:=g_iSourceID, r_vLinkToCommission:=m_bLinkToCommission), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            ' end - PS235

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.HandlerType = m_sHandlerType
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

            m_lReturn = CType(FillCombo(cboControl:=cboTitle, sPropertyID:="524300"), gPMConstants.PMEReturnCode)

            'Maintain Party Code
            If Task = gPMConstants.PMEComponentAction.PMAdd Or Task = gPMConstants.PMEComponentAction.PMEdit Then
                m_lReturn = CType(SetClientCodeCntl(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtclientcode from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            Else
                cmdApply.Visible = False
            End If

            m_oFormFields.GotFocus(ctlControl:=cboCurrency)
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Const vbFormCode As Integer = 0
    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface status.
            'Changes done as per VB code
            'm_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                'If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                'EK 160903 PN6792 Check that we are not creating a duplicate code
                If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then
                    m_lReturn = CType(ValidateOK(), gPMConstants.PMEReturnCode)
                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        eventArgs.Cancel = True
                        Cancel = 1
                        ' Set the mouse pointer to normal.
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                End If
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                'Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
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
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                tabMainTab.SelectedIndex = 2
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
                tabMainTab.SelectedIndex = 3
            End If
        Catch



            Exit Sub
        End Try


    End Sub


    Private Sub lvwAddress_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddress.Click

        If Not (lvwAddress.FocusedItem Is Nothing) Then
            cmdDeleteAd.Enabled = True
            cmdEditAd.Enabled = True

            m_lAddressCnt = Convert.ToString(lvwAddress.FocusedItem.Tag)
            m_iLine = lvwAddress.FocusedItem.Index + 1
        End If

    End Sub

    Private Sub lvwAddress_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddress.DoubleClick

        ' Active the edit button
        cmdEditAd_Click(cmdEditAd, New EventArgs())

    End Sub

    Private Sub lvwAddress_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddress.Enter

        If SSTabHelper.GetTabVisible(tabMainTab, 2) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 2)
        End If
        lvwAddress_Click(lvwAddress, New EventArgs())

    End Sub

    Private Sub lvwAddress_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAddress.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        If Not (lvwAddress.GetItemAt(x, y) Is Nothing) Then
            cmdDeleteAd.Enabled = True
            cmdEditAd.Enabled = True
        Else
            cmdDeleteAd.Enabled = False
            cmdEditAd.Enabled = False
        End If

    End Sub

    Private Sub lvwContacts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.Click

        If lvwContacts.Items.Count > 0 Then

            If Not (lvwContacts.FocusedItem Is Nothing) Then
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True

                m_lContactCnt = Convert.ToString(lvwContacts.FocusedItem.Tag)
                m_iLine = lvwContacts.FocusedItem.Index + 1
            End If

        End If

    End Sub

    Private Sub lvwContacts_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.DoubleClick

        lvwContacts_Click(lvwContacts, New EventArgs())

    End Sub

    Private Sub lvwContacts_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.Enter

        If SSTabHelper.GetTabVisible(tabMainTab, 1) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 1)
        End If
        lvwContacts_Click(lvwContacts, New EventArgs())

    End Sub

    Private Sub lvwContacts_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContacts.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        If Not (lvwContacts.GetItemAt(x, y) Is Nothing) Then
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
        Else
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
        End If

    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                If SSTabHelper.GetSelectedIndex(tabMainTab) < cmdNext.Length Then
                    VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)), True)
                Else
                    VB6.SetDefault(cmdOK, True)
                End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab))
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch




            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim nReturn As Integer
        ' Click event of the OK button.
        Try

            'Maintain Party Code
            If m_bIsSetMaskingCode And m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = ValidateNumberingScheme()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                m_lReturn = GeneratePartyCode()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Maintain Party Code
            If m_bIsReadOnly Then
                lblClientCode.Enabled = False
                txtClientCode.Enabled = False
            End If

            'EK 12/11/99 Check that we are not creating a duplicate code
            m_lReturn = CType(ValidateOK(), gPMConstants.PMEReturnCode)


            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)


            m_lPartyCnt = m_oBusiness.PartyCnt

            'Update party addresses
            m_lReturn = CType(UpdateAddresses(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Address Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                Exit Sub
            End If


            'Update party contacts
            m_lReturn = CType(UpdateContacts(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Contact Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                Exit Sub
            End If

            ' Check the return value.
            'DC260106 PN27040 if cancel out of custom data screen will also close the form
            m_lReturn = CType(PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Or m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

            Dim oPartyBusiness As bSIRParty.Business = Nothing
            nReturn = g_oObjectManager.GetInstance(oPartyBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
            End If

            oPartyBusiness.AddPartyHistory(m_lPartyCnt, String.Empty)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party History Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
            End If
        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: UpdateAddresses
    '
    ' Description: This goes thru all addresses in the the grid control
    ' and the original address array and sees what the differences
    ' are. It then adds new addresses or deletes existing ones according
    ' to what user has done.
    '
    ' ***************************************************************** '
    Private Function UpdateAddresses() As Integer
        'RWH(21/07/2000) Altered to move PostCode from far left to far
        'right of ListView.

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim vNewAddresses, vOldAddresses(,) As Object
        Dim bFirst As Boolean
        Dim i As Integer
        Dim sAddressUsage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Go thru original address array to get list of old addresses
            If Information.IsArray(m_vAddresses) Then
                ReDim vOldAddresses(1, m_vAddresses.GetUpperBound(1))
                For i = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                    vOldAddresses(0, i) = CInt(m_vAddresses(6, i))

                    vOldAddresses(1, i) = CInt(m_vAddresses(1, i))
                Next i
            End If

            'Go thru addresses grid to get list of new addresses
            i = 1
            bFirst = True
            Do
                If i > lvwAddress.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwAddress.Items.Item(i - 1)

                'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        sAddressUsage = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
                    Case Else
                        sAddressUsage = oListItem.Text.Trim()
                End Select

                If sAddressUsage = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim vNewAddresses(1, i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vNewAddresses(1, i - 1)
                    End If



                    vNewAddresses(0, i - 1) = Convert.ToString(oListItem.Tag)

                    For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                        'RWH(24/07/2000)
                        If sAddressUsage = CStr(m_vAddressTypes(1, j)) Then

                            vNewAddresses(1, i - 1) = m_vAddressTypes(0, j)
                            Exit For
                        End If
                    Next j

                End If
                i += 1
            Loop

            'Delete old address usages in database
            If (Information.IsArray(vOldAddresses)) And (Not Information.IsArray(vNewAddresses)) Then

                For i = 0 To vOldAddresses.GetUpperBound(1)
                    m_lReturn = m_oBusiness.DeleteAddress(m_lPartyCnt, vOldAddresses(0, i))
                Next

                m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vDeleteAddresses:=vOldAddresses)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Add new addresses in database
            If (Not Information.IsArray(vOldAddresses)) And (Information.IsArray(vNewAddresses)) Then

                m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vAddAddresses:=vNewAddresses)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            'If we have old and new addresses, delete common ones
            If (Information.IsArray(vOldAddresses)) And (Information.IsArray(vNewAddresses)) Then

                'Delete unchanged addresses (ie set them to 0)

                For i = vOldAddresses.GetLowerBound(1) To vOldAddresses.GetUpperBound(1)

                    For j As Integer = vNewAddresses.GetLowerBound(1) To vNewAddresses.GetUpperBound(1)




                        If (vNewAddresses(0, j).Equals(vOldAddresses(0, i))) And (vNewAddresses(1, j).Equals(vOldAddresses(1, i))) Then

                            vNewAddresses(0, j) = 0

                            vOldAddresses(0, i) = 0
                        End If
                    Next j
                Next i

                For m As Integer = 0 To vOldAddresses.GetUpperBound(1)
                    Dim nUnmatchRecord As Integer = 0
                    nUnmatchRecord = DeleteAddress(CInt(vOldAddresses(0, m)), vNewAddresses)
                    If nUnmatchRecord = 1 Then
                        m_lReturn = m_oBusiness.DeleteAddress(m_lPartyCnt, vOldAddresses(0, m))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Next

                'update the database

                m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vDeleteAddresses:=vOldAddresses, vAddAddresses:=vNewAddresses)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddressesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateContacts
    '
    ' Description: This goes thru all contacts in the the grid control
    ' and the original contact array and sees what the differences
    ' are. It then adds new contacts or deletes existing ones according
    ' to what user has done.
    '
    ' ***************************************************************** '
    Private Function UpdateContacts() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim vNewContacts, vOldContacts As Object
        Dim bFirst As Boolean
        Dim i As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the party count off the business
            If m_lPartyCnt = 0 Then

                m_lPartyCnt = m_oBusiness.PartyCnt
            End If

            'Go thru original contact array to get list of old contacts
            If Information.IsArray(m_vContacts) Then
                ReDim vOldContacts(m_vContacts.GetUpperBound(1))
                For i = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                    vOldContacts(i) = CInt(m_vContacts(0, i))
                Next i
            End If

            'Go thru contacts grid to get list of new contacts
            'SP171298
            i = 1
            bFirst = True

            Do
                If i > lvwContacts.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwContacts.Items.Item(i - 1)
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim vNewContacts(i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vNewContacts(i - 1)
                    End If



                    vNewContacts(i - 1) = Convert.ToString(oListItem.Tag)

                End If
                i += 1
            Loop

            'Delete old contact usages in database
            If (Information.IsArray(vOldContacts)) And (Not Information.IsArray(vNewContacts)) Then

                m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vDeleteContacts:=vOldContacts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Add new contacts in database
            If (Not Information.IsArray(vOldContacts)) And (Information.IsArray(vNewContacts)) Then

                m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vAddContacts:=vNewContacts)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'If we have old and new Contacts, delete common ones
            If (Information.IsArray(vOldContacts)) And (Information.IsArray(vNewContacts)) Then

                'Delete unchanged Contacts (ie set them to 0)

                For i = vOldContacts.GetLowerBound(0) To vOldContacts.GetUpperBound(0)

                    For j As Integer = vNewContacts.GetLowerBound(0) To vNewContacts.GetUpperBound(0)


                        If vNewContacts(j).Equals(vOldContacts(i)) Then

                            vNewContacts(j) = 0

                            vOldContacts(i) = 0
                        End If
                    Next j
                Next i

                'update the database

                m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vDeleteContacts:=vOldContacts, vAddContacts:=vNewContacts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContactsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'EK 12/11/99
    ' ***************************************************************** '
    ' Name: ValidateOK
    '
    ' Description: This validates mandatory data
    '
    ' ***************************************************************** '
    Private Function ValidateOK() As Integer


        Dim result As Integer = 0
        Dim lPartyCnt As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'SJ 08/06/2004 - start
            'Issue 12401
            '    If m_iTask = PMAdd Then
            '        m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=Trim$(txtClientCode.Text), vPartyCnt:=lPartyCnt)
            '
            '        If m_lReturn <> PMTrue Then
            '            MsgBox ("Unable to access bSIRParty")
            '            ValidateOK = PMFalse
            '        End If
            '
            '        If lPartyCnt <> 0 Then
            '            MsgBox ("Party Code already exists")
            '            ValidateOK = PMFalse
            '        End If
            '
            '    End If
            ''68673 Start
            If uctPickListBranches.SelectedItems = 0 And m_sHandlerType = ACHandler Then
                MessageBox.Show("You must attach at least one Branch to the Account Handler", "Account Handler Branch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMFalse
                SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                uctPickListBranches.Focus()
                Return result
            End If
            ''68673 End

            m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=txtClientCode.Text.Trim(), vPartyCnt:=lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetPartyCnt Failed for " & txtClientCode.Text, vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK")
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                If lPartyCnt <> 0 Then
                    MessageBox.Show("Party Code already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtClientCode.Focus()
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                If lPartyCnt <> 0 And lPartyCnt <> m_lPartyCnt Then
                    MessageBox.Show("Party Code already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtClientCode.Focus()
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'SJ 08/06/2004 - end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (GetCommissionAccounts) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetCommissionAccounts() As Integer
    '
    'Dim result As Integer = 0
    'Dim oRiskGroup As Object
    'Dim vSources As Object
    'Dim lLoop As Integer
    'Dim sText As String = ""
    'Dim lAccountId, lIndex As Integer
    '
    'Try 
    '
    '
    'AR20041201 - PN17207 Populate combo if ExecHandler
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCommissionAccounts failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionAccounts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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
            Else
                Me.BringToFront()
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

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_1.Click, _cmdNext_0.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
                ''Saurabh
                If m_sHandlerType = "AH" And Index = 1 Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, Index + 2)
                Else
                    SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
                End If
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            Exit Sub
        End Try


    End Sub

    ' PRIVATE Events (End)
    Private Sub txtClientCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClientCode.Enter
        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtClientCode)
    End Sub

    Private Sub txtClientCode_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtClientCode.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If Conversion.Val(CStr(Strings.Len(txtClientCode.Text))) >= 20 And (KeyAscii <> 8) Then
            KeyAscii = 0
        End If
        If (KeyAscii <> 8) And ((KeyAscii < 65 Or KeyAscii > 90) And (KeyAscii < 97 Or KeyAscii > 122) And (KeyAscii < 48 Or KeyAscii > 57)) Then
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtClientCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClientCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtClientCode)
    End Sub

    Private Sub cboDepartment_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDepartment.GotFocus
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboDepartment)
    End Sub

    Private Sub cboDepartment_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDepartment.LostFocus
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=cboDepartment)
    End Sub

    Private Sub txtForename_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtForename.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtForename)
    End Sub

    Private Sub txtForename_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtForename.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtForename)
    End Sub

    Private Sub txtInitials_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInitials.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtInitials)
    End Sub

    Private Sub txtInitials_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInitials.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInitials)
    End Sub

    Private Sub txtLastname_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLastname.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtLastname)
    End Sub

    Private Sub txtLastname_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLastname.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtLastname)
    End Sub

    ' ***************************************************************** '
    '
    ' Name: SetClientCodeCntl
    '
    ' Description:
    '
    ' History: VB
    '
    ' ***************************************************************** '
    Private Function SetClientCodeCntl() As Integer
        Dim result As Integer = 0
        Dim bSIRPolicyNumMaint As Object
        Const kMethodName As String = "SetClientCodeCntl"

        Dim r_bIsReadOnly, r_bIsNumberingSchemeExists As Boolean

        Dim oClientNumber As bSIRPolicyNumMaint.Business
        Dim r_sMaskCode As String = ""
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If oClientNumber Is Nothing Then
                Dim temp_oClientNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oClientNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oClientNumber = temp_oClientNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    gPMFunctions.RaiseError("SetClientCodeCntl", "bSIRPolicyNumMaint.Business instance not created")
                    Return result
                End If
            End If

            If m_sHandlerType = ACHandler Then 'Account Handler

                m_lReturn = oClientNumber.SendClientReadOnlyDetails(v_sPartyType:=gSIRLibrary.SIRPartyTypeAccountHandler, r_bIsReadOnly:=r_bIsReadOnly, r_bIsNumberingSchemeExists:=r_bIsNumberingSchemeExists, r_sMaskCode:=r_sMaskCode)

            ElseIf m_sHandlerType = ACExecutive Then  'ACExecutive -Executive

                m_lReturn = oClientNumber.SendClientReadOnlyDetails(v_sPartyType:=gSIRLibrary.SIRPartyTypeConsultant, r_bIsReadOnly:=r_bIsReadOnly, r_bIsNumberingSchemeExists:=r_bIsNumberingSchemeExists, r_sMaskCode:=r_sMaskCode)

            ElseIf m_sHandlerType = ACExecHandler Then  'Executive Handler

                m_lReturn = oClientNumber.SendClientReadOnlyDetails(v_sPartyType:=gSIRLibrary.SIRPartyTypeExecutiveHandler, r_bIsReadOnly:=r_bIsReadOnly, r_bIsNumberingSchemeExists:=r_bIsNumberingSchemeExists, r_sMaskCode:=r_sMaskCode)
            Else
                r_bIsNumberingSchemeExists = False
                r_bIsReadOnly = False
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                gPMFunctions.RaiseError("SetClientCodeCntl", "SendClientReadOnlyDetails failed")
                Return result
            End If

            m_bIsSetMaskingCode = r_bIsNumberingSchemeExists
            m_bIsReadOnly = r_bIsReadOnly
            m_sMaskCode = r_sMaskCode

            If r_bIsNumberingSchemeExists And r_bIsReadOnly Then
                lblClientCode.Enabled = False
                txtClientCode.Enabled = False
                cmdApply.Visible = True
            ElseIf r_bIsNumberingSchemeExists And Not r_bIsReadOnly Then
                lblClientCode.Enabled = True
                txtClientCode.Enabled = True
                cmdApply.Enabled = True
            Else
                lblClientCode.Enabled = True
                txtClientCode.Enabled = True
                cmdApply.Visible = False
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function


    Private Function GeneratePartyCode() As Integer
        Dim result As Integer = 0
        Dim bSIRPolicyNumMaint As Object

        Const kMethodName As String = "GeneratePartyCode"

        Dim sFailureReason, sGeneratedClientCode, sInitial, sTitle, sLastName, sPartyType, sFirstName As String

        Dim oClientNumber As bSIRPolicyNumMaint.Business

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bIsSetMaskingCode And txtClientCode.Text = "" Then
                Dim temp_oClientNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oClientNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oClientNumber = temp_oClientNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    gPMFunctions.RaiseError("GeneratePartyCode", "bSIRPolicyNumMaint.Business instance not Created")
                    Return result
                End If
                'End If

                sGeneratedClientCode = ForeName
                sInitial = txtInitials.Text.Trim().ToUpper()
                sTitle = cboTitle.Text.Trim().ToUpper()
                sLastName = txtLastname.Text.Trim().ToUpper()
                sFirstName = txtForename.Text.Trim().ToUpper()

                If m_sHandlerType = ACHandler Then 'Account Handler

                    m_lReturn = oClientNumber.GenerateClientCode(v_sPartyType:=gSIRLibrary.SIRPartyTypeAccountHandler, r_sGeneratedClientCode:=sGeneratedClientCode, v_iSourceID:=g_iSourceID, r_sFailureReason:=sFailureReason, v_sType:=sFirstName, v_sInitial:=sInitial, v_sTitle:=sTitle, v_sValue:=sLastName)
                    sPartyType = "Accounts Handler"

                ElseIf m_sHandlerType = ACExecutive Then  'ACExecutive -Executive

                    m_lReturn = oClientNumber.GenerateClientCode(v_sPartyType:=gSIRLibrary.SIRPartyTypeConsultant, r_sGeneratedClientCode:=sGeneratedClientCode, v_iSourceID:=g_iSourceID, r_sFailureReason:=sFailureReason, v_sType:=sFirstName, v_sInitial:=sInitial, v_sTitle:=sTitle, v_sValue:=sLastName)
                    sPartyType = "Accounts Executive"

                ElseIf m_sHandlerType = ACExecHandler Then  'Executive Handler

                    m_lReturn = oClientNumber.GenerateClientCode(v_sPartyType:=gSIRLibrary.SIRPartyTypeExecutiveHandler, r_sGeneratedClientCode:=sGeneratedClientCode, v_iSourceID:=g_iSourceID, r_sFailureReason:=sFailureReason, v_sType:=sFirstName, v_sInitial:=sInitial, v_sTitle:=sTitle, v_sValue:=sLastName)
                    sPartyType = "Executive Handler"

                End If


                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                    ' Log Error.
                    gPMFunctions.RaiseError("GeneratePartyCode", "GenerateClientCode Failed ")
                    Return result
                ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then  'Numbering Scheme not set
                    MessageBox.Show("Numbering scheme for " & sPartyType & " is not set.", sPartyType, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                ElseIf sFailureReason <> "" Then
                    MessageBox.Show(sFailureReason, sPartyType, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                txtClientCode.Text = sGeneratedClientCode
                lblClientCode.Enabled = False
                txtClientCode.Enabled = False
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Function ValidateNumberingScheme() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateNumberingScheme"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sMaskCode <> "" Then

                ' Last Name
                If m_sMaskCode.IndexOf("L"c) >= 0 Then
                    If txtLastname.Text = "" Then
                        MessageBox.Show("Please Enter Last Name", "field - Last Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If

                ' First Name
                If m_sMaskCode.IndexOf("F"c) >= 0 Then
                    If txtForename.Text = "" Then
                        MessageBox.Show("Please Enter Fore Name", "field - Fore Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If

                ' Initials
                If m_sMaskCode.IndexOf("I"c) >= 0 Then
                    If txtInitials.Text = "" Then
                        MessageBox.Show("Please Enter Initials", "field - Initials", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If

            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ''Start(Saurabh Agrawal) Tech Spec LOA008 Account Handler(5.2.2.2)
    Public Function SaveBranchPickListData() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveBranchPickListData"
        result = gPMConstants.PMEReturnCode.PMTrue


        uctPickListBranches.ForeignKeys.Item("PartyCnt").Value = m_lPartyCnt
        uctPickListBranches.ForeignKeys.Item("user_id").Value = g_oObjectManager.UserID

        uctPickListBranches.ForeignKeys.Item("unique_id").Value = m_sUniqueId

        uctPickListBranches.ForeignKeys.Item("screen_hierarchy").Value = m_sScreenHierarchy

        m_lReturn = uctPickListBranches.Save()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("SaveBranchPickListData", "uctBranchesPickList Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        Return result


        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ValidateNumberingScheme())




        Return result
    End Function
    ''End(Saurabh Agrawal) Tech Spec LOA008 Account Handler(5.2.2.2)


    Private Sub cboCurrency_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCurrency.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboCurrency)
    End Sub

    Private Function DeleteAddress(ByVal Value As Integer, ByVal vAddress As Object(,)) As Integer
        Dim nResult As Integer = 1
        Const kMethodName As String = "DeleteAddress"

        Try
            For i As Integer = 0 To vAddress.GetUpperBound(1)
                If Value = CInt(vAddress(0, i)) Then
                    nResult = 0
                    Exit For
                End If
            Next

            Return nResult
        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            Return nResult
        End Try
    End Function
End Class
