Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 08/07/2002
    '
    ' Description: Main interface. Based on iPMBPartyAG.
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'developer guide no. 7
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

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lAddressCount As Integer
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPartyAGG.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    'EK 10/10/99 Add access to Commission Rates
    ' Declare an instance of the Agent Rates interface.
    Private m_oRates As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Stores the return value for reference id
    Private m_iRefID As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}

    'Addresses and Contacts
    Private m_iLine As Integer
    Private m_lAddressCnt As Integer
    Private m_lBranchID As Integer
    Private m_lAddressUsageTypeID As Integer

    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sMainPostCode As String = ""
    Private m_sName As String = ""
    'Private m_iPartyAgentOriginID As Integer
    Private m_iIsBranch As Integer
    Private m_iIsHeadOffice As Integer
    Private m_dtAgencyAgreement As Date
    'Private m_dtAgencyNextReview As Date
    Private m_lHeadOfficeCnt As Object
    Private m_sHeadOfficeRef As String = ""
    Private m_vAddresses(,) As Object
    Private m_vAddressTypes(,) As Object
    Private m_vContacts(,) As Object
    Private m_sAddressLine1 As String = ""
    Private m_iActive As Integer
    Private m_sFileCode As String = ""
    Private m_lCurrencyID As Integer
    Private m_sTermsOfPayment As String = ""
    Private m_lPartyCategoryID As Integer

    'RWH(24/07/2000) RSAIB Process 004.
    Private m_iDefaultCountryID As Integer
    Private m_sDefaultCountryCode As String = ""

    ' CF 280699
    Private m_vDefaultCommissionPercent As Object
    'Private m_sAgencyAccountNumber As String
    'Private m_sTradingName As String
    'Private m_lBinderIndicator As Long
    'Private m_lReportIndicator As Long
    'Private m_vWitholdingTax As Variant

    'TN20001117
    Private m_sAgencyOrUnderwriting As String = ""

    'Flag to indicate whether we need to check the headoffice id matches
    'the headoffice ref as user may change the reference directly
    Private m_bVerifyHeadOfficeCnt As Boolean

    'Note the index in the lookup array of the main address
    Private m_iMainAddressIndex As Integer

    ' Declare an instance of the address interface.

    Private m_oAddress As iPMBAddress.Interface_Renamed

    ' Declare an instance of the contact interface.

    Private m_oContact As iPMBContact.Interface_Renamed

    ' Gemini List Manager
    Private m_oListManager As iGEMListManager.Interface_Renamed

    'Maintain Party Code
    Private m_bIsSetMaskingCode As Boolean
    Private m_bIsReadOnly As Boolean
    Private m_sMaskCode As String = ""
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}
            'SP090998
            '    m_lReturn& = GetLookupDetails( _
            'sLookupTable:=SIRLookupPartyAgentOrigin, _
            'ctlLookup:=cboSource)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' ************************************************************
            ' Enter your code here to retrieve all of the lookup
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

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
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



            ' Error Section.

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
    ' Error Section.
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


    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    Public Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the interface exit status.
            m_lStatus = Value

        End Set
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
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
    ' {* USER DEFINED CODE (Begin) *}

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public Property ShortName() As String
        Get

            Return m_sShortName

        End Get
        Set(ByVal Value As String)

            m_sShortName = Value

        End Set
    End Property
    Public Property LongName() As String
        Get

            Return m_sName

        End Get
        Set(ByVal Value As String)

            'For some reason this wont compile if use 'name' as the property
            'name.
            m_sName = Value

        End Set
    End Property
    Public Property MainPostCode() As String
        Get

            Return m_sMainPostCode

        End Get
        Set(ByVal Value As String)

            m_sMainPostCode = Value

        End Set
    End Property
    Public Property AddressLine1() As String
        Get

            Return m_sAddressLine1

        End Get
        Set(ByVal Value As String)

            m_sAddressLine1 = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)

    Private ReadOnly Property AgentGroupName() As String
        Get
            Return gPMFunctions.ToSafeString(txtName.Text).Replace(" ", "").ToUpper()
        End Get
    End Property

    Public ReadOnly Property BranchId() As Integer
        Get
            Dim result As Integer = 0
            If uctBranch.ListIndex > -1 Then
                result = uctBranch.ItemData(uctBranch.ListIndex)
            End If
            Return result
        End Get
    End Property


    Public Property DefaultCountryID() As Integer
        Get
            Return m_iDefaultCountryID
        End Get
        Set(ByVal Value As Integer)
            m_iDefaultCountryID = Value
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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Reference (Agent Group Code) must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIDReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Name must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Branch must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=uctBranch, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

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
            'SP090998
            'm_lPartyCnt& = 25

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



            ' Error Section.

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
            m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtIDReference, vControlValue:=m_sShortName)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            uctBranch.ItemId = m_lBranchID

            If m_iActive = 0 Then
                chkActive.CheckState = CheckState.Unchecked
            Else
                chkActive.CheckState = CheckState.Checked
            End If

            'Fill the contact grid
            PopulateContacts()

            'Fill the address grid
            PopulateAddresses()

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    txtDesc.Text = FormatField( _
            ''        iFormatType:=PMFormatString, _
            ''        vFieldValue:=m_sDDesc$)
            '
            '    optChoice.Value = CBool(FormatField( _
            ''        iFormatType:=PMFormatBoolean, _
            ''        vFieldValue:=m_iDChoice%))
            '
            '    txtDate.Text = FormatField( _
            ''        iFormatType:=PMFormatDateLong, _
            ''        vFieldValue:=m_dtDDate)
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName,
            '                                               vControlValue:=m_sName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

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

            ' Set the mouse pointer to an hour glass
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1
            m_sUniqueId = GetUniqueID()
            m_sScreenHierarchy = $"Agent Group({txtIDReference.Text.Trim()})"
            ' Check the task.
            Select Case (m_iTask)
                'SP090989
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    'EK 210199 Bug 253 added resolved name

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vPartyAgentBranch:=m_lBranchID, vShortName:=m_sShortName, vName:=m_sName, vActive:=m_iActive, vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)

                    ' {* USER DEFINED CODE (End) *}
                    'EK 210199 Bug 253 added resolved name

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vPartyAgentBranch:=m_lBranchID, vShortName:=m_sShortName, vName:=m_sName, vActive:=m_iActive, vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)

                    ' {* USER DEFINED CODE (End) *}

            End Select

            ' Reset the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception



            ' Reset the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowRates
    '
    ' Description: Entry point to show rates from SIRToolbarFunc
    '
    ' MSS260701 - Created
    ' ***************************************************************** '
    Public Function ShowRates() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show rates", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateAddresses
    '
    ' Description: Fills the grid control with address details
    '
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

            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwList:=lvwAddress, bSizeHeaders:=True), gPMConstants.PMEReturnCode)

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

        'Const ContactImage As String = "ContactImage"   '' Unused Local Variable
        Dim oListItem As ListViewItem


        Try

            If Not Information.IsArray(m_vContacts) Then
                Exit Sub
            End If

            lvwContact.Items.Clear()

            ' Assign the details to the interface.
            For i As Integer = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                oListItem = lvwContact.Items.Add(CStr(m_vContacts(1, i)).Trim())

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

            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwList:=lvwContact, bSizeHeaders:=True), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: ValidateOK
    '
    ' Description: This validates mandatory address types and duplicate
    ' addresses
    '
    ' ***************************************************************** '
    Private Function ValidateOK() As Integer

        Dim result As Integer = 0
        Dim iMainAddresses As Integer
        Dim bDuplicate As Boolean
        Dim oListItem As ListViewItem
        Dim sAddressUsage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check Branch
            If uctBranch.ListIndex = 0 Then
                MessageBox.Show("Please enter a valid Branch", Application.ProductName)
                SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                uctBranch.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Validate Addresses
            iMainAddresses = 0

            'Count how many addresses are main address
            If lvwAddress.Items.Count > 0 Then
                For i As Integer = 1 To lvwAddress.Items.Count
                    oListItem = lvwAddress.Items.Item(i - 1)

                    'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
                    Select Case (m_sDefaultCountryCode.Trim())
                        Case "GBR"
                            sAddressUsage = ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(i - 1), 1).Text.Trim()
                        Case Else
                            sAddressUsage = lvwAddress.Items.Item(i - 1).Text.Trim()
                    End Select

                    For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                        'RWH(24/07/2000)
                        If (sAddressUsage = CStr(m_vAddressTypes(1, j))) And (CDbl(m_vAddressTypes(0, j)) = m_iMainAddressIndex) Then
                            iMainAddresses += 1
                        End If
                    Next j
                Next i
            End If

            Select Case iMainAddresses
                Case 0
                    'No
                    MessageBox.Show("You must have an address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                    cmdAddAd.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse

                Case 1
                    'Yes

                Case Else
                    'No.
                    MessageBox.Show("You can have only one address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                    cmdAddAd.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse

            End Select

            bDuplicate = False

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateAddressPostCodeProperties
    '
    ' Description: This checks for the main address and gets the
    ' post code and address line 1 for it via the address business
    '
    ' ***************************************************************** '
    Private Sub UpdateAddressPostCodeProperties()
        'RWH(21/07/2000) Altered to move PostCode from far left to far
        'right of ListView.

        Dim lAddressCnt As Integer

        Dim oAddressBusiness As bSIRAddress.Business
        Dim sAddressUsage As String = ""


        Try

            'Find the main address
            For i As Integer = 1 To lvwAddress.Items.Count

                'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        sAddressUsage = ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(i - 1), 1).Text.Trim()
                    Case Else
                        sAddressUsage = lvwAddress.Items.Item(i - 1).Text.Trim()
                End Select

                For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                    'RWH(24/07/2000)
                    If (sAddressUsage = CStr(m_vAddressTypes(1, j))) And (CDbl(m_vAddressTypes(0, j)) = m_iMainAddressIndex) Then
                        lAddressCnt = Convert.ToString(lvwAddress.Items.Item(i - 1).Tag)
                        Exit For
                    End If
                Next j
            Next i

            'Get address business to retrieve
            Dim temp_oAddressBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAddressBusiness, "bSIRAddress.Business", vInstanceManager:="ClientManager")
            oAddressBusiness = temp_oAddressBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            oAddressBusiness.AddressCnt = lAddressCnt


            m_lReturn = oAddressBusiness.GetDetails(vAddressCnt:=lAddressCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oAddressBusiness.Dispose()
                oAddressBusiness = Nothing
                Exit Sub
            End If


            m_lReturn = oAddressBusiness.GetNext(vPostalCode:=m_sMainPostCode, vAddress1:=m_sAddressLine1)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oAddressBusiness.Dispose()
                oAddressBusiness = Nothing
                Exit Sub
            End If


            oAddressBusiness.Dispose()
            oAddressBusiness = Nothing

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateAddressPostCodeProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddressPostCodeProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.


            ' {* USER DEFINED CODE (Begin) *}
            'SP090998

            m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vPartyAgentBranch:=m_lBranchID, vShortName:=m_sShortName, vName:=m_sName, vActive:=m_iActive)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get additional details required for display that not stored on this
            'record


            m_lReturn = m_oBusiness.GetOtherDetails(vAgentCnt:=m_lHeadOfficeCnt, vAgentref:=m_sHeadOfficeRef)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get addresses for the party

            m_lReturn = m_oBusiness.GetAddressDetails(vAddresses:=m_vAddresses)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get addresse type lookups for the party

            m_lReturn = m_oBusiness.GetAddresstypelookups(vaddresstypes:=m_vAddressTypes)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get contacts for the party

            m_lReturn = m_oBusiness.GetContactDetails(vContacts:=m_vContacts)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the contact details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

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
        Dim sMsg As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}


            m_sShortName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtIDReference))

            ' If this is an add then check for duplicate references

            If Task = gPMConstants.PMEComponentAction.PMAdd Then
                If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                    m_lReturn = m_oBusiness.CheckReference(m_sShortName)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'If the returned reference is an empty string, then the reference exists
                    If m_sShortName = "" Then


                        'Developer Guide no: 243
                        sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRefExists, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        MessageBox.Show(sMsg, "Agent Group", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                End If
            End If

            m_lBranchID = uctBranch.ItemId


            m_sName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtName))
            m_iActive = chkActive.CheckState

            'Validation of following not required if we are cancelling out

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

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

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

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            'Get Country Code for Postcode checking.(Process 004)

            m_lReturn = m_oBusiness.GetDefaultCountryCode(v_iCountryID:=m_iDefaultCountryID, r_sCountryCode:=m_sDefaultCountryCode)

            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get default country code", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")

                Return result
            End If

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    '            cmdNavigate.Visible = True
                    '            cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    '            cmdNavigate.Visible = True
                    '            cmdNavigate.Enabled = False

                Case Else
                    '            cmdNavigate.Visible = False
            End Select

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    m_lReturn& = m_oListManager.PopulateListControl( _
            'v_sPropertyID:=cboTermsOfPayment.Tag, _
            'r_oControl:=cboTermsOfPayment)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        SetInterfaceDefaults = PMFalse
                '        Exit Function
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' TF291298 - Disable menu options if New Client
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                mnuFinancial.Enabled = False
                mnuCommission.Enabled = False
                mnuNotes.Enabled = False
                mnuLetter.Enabled = False
                Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonFinancial - 1).Enabled = False
                Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonCommission - 1).Enabled = False
                Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonNotes - 1).Enabled = False
                Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonLetter - 1).Enabled = False
            End If

            'Default to 'Is Branch = true' if new agent
            '    If (m_iTask% = PMAdd) Then
            '        chkIsBranch.Value = vbChecked
            '    End If

            'TN20001711

            mnuCommission.Available = False
            Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonCommission - 1).Visible = False

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
            ReDim m_ctlTabFirstLast(1, 2)

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
            'SP090998
            m_ctlTabFirstLast(ACControlStart, 0) = txtIDReference
            m_ctlTabFirstLast(ACControlEnd, 0) = chkActive

            m_ctlTabFirstLast(ACControlStart, 1) = lvwAddress
            m_ctlTabFirstLast(ACControlEnd, 1) = cmdEditAd

            m_ctlTabFirstLast(ACControlStart, 2) = lvwContact
            m_ctlTabFirstLast(ACControlEnd, 2) = cmdEditCon

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

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


            'Developer Guide no: 243
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


            'Developer Guide no: 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            cmdAddAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            cmdEditAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            cmdDeleteAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            cmdAddCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            cmdEditCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            cmdDeleteCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide no: 243
            Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonFinancial - 1).ToolTipText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFinancial, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' TF031298



            'Developer Guide no: 243
            Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonNotes - 1).ToolTipText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNotes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide no: 243
            Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonLetter - 1).ToolTipText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLetter, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            lblIDReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReference, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            lblName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            sAddressUsage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListUsage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            sAddressLine1 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            sAddressLine2 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            sAddressLine3 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            sAddressLine4 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            sPostCode = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListPostCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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

            lvwContact.Columns.Item(0).Text = "Area Code"
            lvwContact.Columns.Item(1).Text = "Number"
            lvwContact.Columns.Item(2).Text = "Extension"
            lvwContact.Columns.Item(3).Text = "Type"
            lvwContact.Columns.Item(4).Text = "Description"

            ' Set full row select on the address control
            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwAddress.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            ' Set full row select on the contact control
            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwContact.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

            m_oAddress.Reference = txtIDReference.Text

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
                m_sScreenHierarchy = $"Agent Group({txtIDReference.Text.Trim()})"
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

            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwList:=lvwAddress, bSizeHeaders:=True), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAddCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddCon.Click

        Dim oListItem As ListViewItem

        Try

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Create icontact if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Reset the moust pointer
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contacts", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'set the main postcode and reference
            m_lReturn = CType(m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Reset the moust pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If


            m_oContact.Reference = txtIDReference.Text

            m_oContact.PostCode = m_sMainPostCode


            m_lReturn = m_oContact.ContactCnt

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_sScreenHierarchy = "" Then
                m_sScreenHierarchy = $"Agent Group({txtIDReference.Text.Trim()})"
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

            Me.Refresh()


            oListItem = lvwContact.Items.Add(m_oContact.AreaCode)

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

            ' Store the Contact tag

            oListItem.Tag = m_oContact.ContactCnt

            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwList:=lvwContact, bSizeHeaders:=True), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'Private Sub cmdAgentLookUp_Click()
    '
    'Dim vCnt As Variant
    'Dim vShortName As Variant
    'Dim vName As Variant
    '
    '
    '    On Error GoTo Err_cmdAgentLookUp_Click
    '
    '    m_lReturn& = SelectParty(vPartyCnt:=vCnt, _
    ''                            vShortName:=vShortName, _
    ''                            vName:=vName, _
    ''                            vAgentOnly:=1)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Exit Sub
    '    End If
    '
    '    'save the count in the tag and update controls
    ''    txtHeadOffice.Tag = CStr(vCnt)
    '
    '    m_sHeadOfficeRef$ = CStr(vShortName)
    ''    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtHeadOffice, _
    '''                                            vControlValue:=m_sHeadOfficeRef$)
    '
    '    'because we know Agent cnt matches the Agent ref, can bypass
    '    'the validation at the end
    '    m_bVerifyHeadOfficeCnt = False
    '
    '    Exit Sub
    '
    'Err_cmdAgentLookUp_Click:
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:=PMErrorText, _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="cmdAgentLookUp_Click", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Sub
    '
    'End Sub

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
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process method GeneratePartyCode", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub
                End If
            End If

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            'Maintain Party Code
            If m_bIsReadOnly Then
                lblIDReference.Enabled = False
                txtIDReference.Enabled = False
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

            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

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
    Private Sub cmdDeleteCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteCon.Click

        Try

            'Set row to be deleted - if a valid one selected
            If lvwContact.Items.Count < 1 Then
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

            'Use pmview instead
            'm_lReturn& = m_oContact.SetProcessModes(vTask:=PMDelete)
            m_lReturn = CType(m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the contact id


            m_oContact.ContactCnt = Convert.ToString(lvwContact.FocusedItem.Tag)
            'm_oContact.ContactCnt = m_lContactCnt&


            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            lvwContact.Items.RemoveAt(lvwContact.FocusedItem.Index)

            lvwContact.Focus()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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

            m_oAddress.Reference = txtIDReference.Text
            '    m_oAddress.PostCode = pnlIDPostCode

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
                m_sScreenHierarchy = $"Agent Group({txtIDReference.Text.Trim()})"
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

            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwList:=lvwAddress, bSizeHeaders:=True), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditCon.Click

        Dim oListItem As ListViewItem

        Try

            'Set row to be deleted - if a valid one selected
            If lvwContact.Items.Count < 1 Then
                Exit Sub
            End If

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Create address component if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Reset the moust pointer
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contact component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = CType(m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Reset the moust pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If


            m_oContact.Reference = txtIDReference.Text

            m_oContact.PostCode = m_sMainPostCode

            'set the contact id
            oListItem = lvwContact.FocusedItem

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_sScreenHierarchy = "" Then
                m_sScreenHierarchy = $"Agent Group({txtIDReference.Text.Trim()})"
            End If

            m_oContact.ContactCnt = Convert.ToString(oListItem.Tag)


            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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

            'Set oListItem = lvwContacts.ListItems.Item(m_iLine)


            oListItem.Text = m_oContact.AreaCode
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description

            ' Store the Contact tag

            oListItem.Tag = m_oContact.ContactCnt

            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwList:=lvwContact, bSizeHeaders:=True), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditCon_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (SelectParty) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SelectParty(ByRef vPartyCnt As Object, ByRef vShortName As Object, Optional ByRef vName As Object = Nothing, Optional ByRef vAgentOnly As String = "") As Integer
    'Dim result As Integer = 0
    'Dim iSIRFindParty As Object
    '
    'Dim oFindParty, vKeyArray As Object
    '
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'oFindParty = New iSIRFindParty.Interface()
    '
    'Set appropriate key if agent only


    'If (Not Information.IsNothing(vAgentOnly)) And (Not String.IsNullOrEmpty(vAgentOnly)) Then
    '
    ''ReDim vKeyArray(1, 0)

    'vKeyArray(0, 0) = gSIRLibrary.SIRNavKeyAgentOnly

    'vKeyArray(1, 0) = vAgentOnly
    '

    'm_lErrorNumber = oFindParty.SetKeys(vKeyArray)
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    '

    'm_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lErrorNumber = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'oFindParty.CallingAppName = "iPMBPartyAGG.Interface"
    '

    'm_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lErrorNumber = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lErrorNumber = oFindParty.Start()
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lErrorNumber = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
    '


    'vPartyCnt = oFindParty.PartyCnt


    'vShortName = oFindParty.ShortName

    'If Not Information.IsNothing(vName) Then


    'vName = oFindParty.LongName
    'End If
    'Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lErrorNumber = oFindParty.Terminate()
    '
    'oFindParty = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectPartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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

                'update the database
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
                If i > lvwContact.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwContact.Items.Item(i - 1)
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

            'Add new addresses in database
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




    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'developer guide no. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_1.Click, _cmdPrevious_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                'The previous button has index 0 on tab 1, etc
                'tabMainTab.Tab = Index - 1
                SSTabHelper.SetSelectedIndex(tabMainTab, Index)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                'm_ctlTabFirstLast(ACControlStart, Index + 1).SetFocus
                m_ctlTabFirstLast(ACControlStart, Index).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub
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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyAGG.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guide no: 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide no: 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBPartyAGG.General()

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

            ' List manager
            m_oListManager = New iGEMListManager.Interface_Renamed()

            ' Initialise it
            m_lReturn = CType(m_oListManager, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Check for latest version
            m_lReturn = m_oListManager.CheckListVersions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


            m_sAgencyOrUnderwriting = m_oBusiness.UnderwritingOrAgency

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load



        ' Forms load event.

        Try
            'developer guide no. 220
            Me.uctBranch.FirstItem = "(none)"

            '*ZAZ* Remove buttons menus - waiting for conformation of
            ' which (if any) need to go in
            mnuFind.Available = False
            mnuRelatedDocuments.Available = False
            'developer guide no. (Resize the interface if Mainmenu is not visible)
            'start
            MainMenu1.Visible = False
            If Not MainMenu1.Visible Then
                Toolbar1.Top = Toolbar1.Top - 22
                tabMainTab.Top = tabMainTab.Top - 22
                cmdApply.Top = cmdApply.Top - 22
                cmdOK.Top = cmdOK.Top - 22
                cmdCancel.Top = cmdCancel.Top - 22
                cmdHelp.Top = cmdHelp.Top - 22
            End If
            'end
            For i As Integer = 1 To Toolbar1.Items.Count
                Toolbar1.Items.Item(i - 1).Visible = False
            Next

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

            m_oBusiness.PartyCnt = m_lPartyCnt
            ' {* USER DEFINED CODE (End) *}


            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)


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

            'If adding, still need to get address types for populating
            'the combo box cells in the grid control
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                'Get addresse type lookups for the party

                m_lReturn = m_oBusiness.GetAddresstypelookups(vaddresstypes:=m_vAddressTypes)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                End If

                'Set the index of the main address
                For i As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)

                    'See if this is the main address
                    If CStr(m_vAddressTypes(2, i)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                        m_iMainAddressIndex = CInt(m_vAddressTypes(0, i))
                        Exit For
                    End If

                Next i

            End If

            'Maintain Party Code
            If Task = gPMConstants.PMEComponentAction.PMAdd Or Task = gPMConstants.PMEComponentAction.PMEdit Then
                m_lReturn = CType(SetClientCodeCntl(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtreference from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            Else
                cmdApply.Visible = False
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

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

            'developer guide no. 7
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'developer guide no. 7
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

            ' Terminate the address object (if used)
            If Not (m_oAddress Is Nothing) Then


                m_oAddress.Dispose()

                ' Destroy the instance of the Address object
                ' from memory.
                m_oAddress = Nothing

            End If

            ' Terminate the contact object (if used)
            If Not (m_oContact Is Nothing) Then


                m_oContact.Dispose()

                ' Destroy the instance of the contact object
                ' from memory.
                m_oContact = Nothing

            End If
            'EK 10/10/99 Access to Commission Rates
            ' Terminate the commission rates object (if used)
            If Not (m_oRates Is Nothing) Then


                m_oRates.Dispose()
                ' Destroy the instance of the policy shares object
                ' from memory.
                m_oRates = Nothing

            End If
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

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
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub


    Private Sub lvwAddress_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddress.Click

        If Not (lvwAddress.FocusedItem Is Nothing) Then

            m_lAddressCnt = Convert.ToString(lvwAddress.FocusedItem.Tag)
            m_iLine = lvwAddress.FocusedItem.Index + 1
        End If

    End Sub

    Private Sub lvwAddress_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddress.DoubleClick

        ' Active the edit button
        cmdEditAd_Click(cmdEditAd, New EventArgs())

    End Sub

    Private Sub lvwAddress_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAddress.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If Not (lvwAddress.GetItemAt(x, y) Is Nothing) Then
            cmdDeleteAd.Enabled = True
            cmdEditAd.Enabled = True
        Else
            cmdDeleteAd.Enabled = False
            cmdEditAd.Enabled = False
        End If

    End Sub

    Private Sub lvwContact_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContact.DoubleClick

        ' Activate the edit button
        cmdEditCon_Click(cmdEditCon, New EventArgs())

    End Sub

    Private Sub lvwContact_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContact.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If Not (lvwContact.GetItemAt(x, y) Is Nothing) Then
            cmdDeleteCon.Enabled = True
            cmdEditCon.Enabled = True
        Else
            cmdDeleteCon.Enabled = False
            cmdEditCon.Enabled = False
        End If

    End Sub

    Public Sub mnuCommission_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuCommission.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(SIRToolbarFunc.ProcessToolbar(v_iButton:=ACIButtonCommission), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch


            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuFinancial_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFinancial.Click

        ' TF291298

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(SIRToolbarFunc.ProcessToolbar(v_iButton:=SIRToolbarFunc.ACIButtonFinancial), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuLetter_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuLetter.Click

        ' TF291298

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(SIRToolbarFunc.ProcessToolbar(v_iButton:=SIRToolbarFunc.ACIButtonLetter), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuNotes.Click

        ' TF291298

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(SIRToolbarFunc.ProcessToolbar(v_iButton:=SIRToolbarFunc.ACIButtonNotes), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


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
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If

            End With

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Dim nReturn As Integer
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Maintain Party Code
            If m_bIsSetMaskingCode And m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = ValidateNumberingScheme()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                m_lReturn = GeneratePartyCode()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process method GeneratePartyCode", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If
            End If

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            'Maintain Party Code
            If m_bIsReadOnly Then
                lblIDReference.Enabled = False
                txtIDReference.Enabled = False
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Validate some address stuff
            m_lReturn = CType(ValidateOK(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the mouse pointer to the hourglass
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Set the mouse back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'update the party cnt property

                m_lPartyCnt = m_oBusiness.PartyCnt

                'Update party addresses
                m_lReturn = CType(UpdateAddresses(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Address Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                    Exit Sub
                End If

                ' CTAF 021000 - Update Orion
                m_lReturn = CType(UpdateOrion(vPartyCnt:=m_lPartyCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Orion. PartyCnt = " & m_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Exit Sub
                End If

                'Update party contacts
                m_lReturn = CType(UpdateContacts(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Contact Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                    Exit Sub
                End If

                'Set the main address post code and address line 1 into the properties
                '(as this may have changed in the address component)
                UpdateAddressPostCodeProperties()

                If PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt) = gPMConstants.PMEReturnCode.PMTrue Then
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
            End If

        Catch excep As System.Exception



            ' Error Section.

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



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_3.Click, _cmdNext_0.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _Toolbar1_Button1.Click, _Toolbar1_Button2.Click, _Toolbar1_Button3.Click, _Toolbar1_Button4.Click, _Toolbar1_Button5.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        ' TF291298

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(SIRToolbarFunc.ProcessToolbar(v_iButton:=Button.Owner.Items.IndexOf(Button)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub


    'Private Sub txtAgencyNextReview_GotFocus()
    '
    '    m_lReturn& = m_oFormFields.GotFocus(ctlControl:=txtAgencyNextReview)
    '
    'End Sub

    'Private Sub txtAgencyNextReview_LostFocus()
    'EK BUG158 07/01/99 This validation is now done in Validate OK
    '    If IsDate(txtAgencyNextReview) Then
    '
    '        If Year(txtAgencyNextReview) < 1900 Then
    '            txtAgencyNextReview = ""
    '        End If
    '
    '    End If

    '    m_lReturn& = m_oFormFields.LostFocus(ctlControl:=txtAgencyNextReview)

    'End Sub

    'Private Sub txtheadoffice_Change()
    '
    '    'Agent ref may no longer match the party_cnt in the tag, so need to
    '    'verify this when validating
    '    m_bVerifyHeadOfficeCnt = True
    '
    'End Sub
    '
    'Private Sub txtheadoffice_GotFocus()
    '
    '    m_lReturn& = m_oFormFields.GotFocus(ctlControl:=txtHeadOffice)
    '
    'End Sub
    '
    '
    'Private Sub txtheadoffice_LostFocus()
    '
    '    m_lReturn& = m_oFormFields.LostFocus(ctlControl:=txtHeadOffice)
    '
    'End Sub


    'Private Sub txtAgencyAgreement_GotFocus()
    '
    '    m_lReturn& = m_oFormFields.GotFocus(ctlControl:=txtAgencyAgreement)
    '
    'End Sub
    '
    'Private Sub txtAgencyAgreement_LostFocus()
    '
    '    m_lReturn& = m_oFormFields.LostFocus(ctlControl:=txtAgencyAgreement)
    '
    'End Sub

    'Private Sub chkisbranch_GotFocus()
    '
    '    m_lReturn& = m_oFormFields.GotFocus(ctlControl:=chkIsBranch)
    '
    'End Sub
    '
    '
    'Private Sub chkisbranch_LostFocus()
    '
    '    m_lReturn& = m_oFormFields.LostFocus(ctlControl:=chkIsBranch)
    '
    'End Sub


    ' PRIVATE Events (End)


    ' PRIVATE Events (End)
    Private isInitializingComponent As Boolean

    Private Sub txtIDReference_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtIDReference.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If gPMFunctions.ToSafeInteger(Strings.Len(txtIDReference.Text)) >= 20 And (KeyAscii <> 8) Then
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
    Private Sub txtIDReference_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'Set the reference on the second and third tab
        'pnlAdReference.Caption = txtIDReference.Text
        'pnlConReference.Caption = txtIDReference.Text


    End Sub

    Private Sub txtIDReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtIDReference)

    End Sub

    Private Sub txtIDReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtIDReference)

    End Sub

    Private Sub txtName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtName)

    End Sub

    Private Sub txtName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtName)

    End Sub

    ' ***************************************************************** '
    ' Name: UpdateOrion
    '
    ' Description: Update the party_address usage table with old
    ' and new addresses for the party.
    '
    ' CTAF 021000 - Taken from uctPartyPCControl (by ECK)
    '
    ' ***************************************************************** '
    Private Function UpdateOrion(ByRef vPartyCnt As Object) As Integer
        Dim result As Integer = 0
        Dim oSIROrionUpdate As bSIROrionUpdate.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oSIROrionUpdate As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oSIROrionUpdate, "bSIROrionUpdate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSIROrionUpdate = temp_oSIROrionUpdate

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bSIROrionUpdate.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' CTAF 021000 - Commented out the source code for now until it's
            '               decided what to do about insurers and changing sources

            ' Get Orion Account IDs
            ' eck010900
            '    Select Case Task
            '        Case PMAdd

            m_lReturn = oSIROrionUpdate.SiriusToOrion(v_lPartyCnt:=vPartyCnt)
            '        Case PMEdit
            '            m_lReturn& = oSIROrionUpdate.SiriusToOrion( _
            ''                            v_lPartyCnt:=vPartyCnt, _
            ''                            v_iOldSourceId:=m_iPartySourceId, _
            ''                            v_iOldPartyId:=m_iPartyId)
            '    End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oSIROrionUpdate.Dispose()

            oSIROrionUpdate = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOrion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetClientCodeCntl
    '
    ' Description: Enable/Disable Client code and set global variables
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
                    ' Raise Error
                    gPMFunctions.RaiseError("SetClientCodeCntl", "Can not create object of bSIRPolicyNumMaint.Business")
                    Return result
                End If
            End If


            m_lReturn = oClientNumber.SendClientReadOnlyDetails(v_sPartyType:=gSIRLibrary.SIRPartyTypeAgentGroup, r_bIsReadOnly:=r_bIsReadOnly, r_bIsNumberingSchemeExists:=r_bIsNumberingSchemeExists, r_sMaskCode:=r_sMaskCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Raise Error.
                gPMFunctions.RaiseError("SetClientCodeCntl", "SendClientReadOnlyDetails Failed")
                Return result
            End If

            m_bIsSetMaskingCode = r_bIsNumberingSchemeExists
            m_bIsReadOnly = r_bIsReadOnly
            m_sMaskCode = r_sMaskCode

            If r_bIsNumberingSchemeExists And r_bIsReadOnly Then
                lblIDReference.Enabled = False
                txtIDReference.Enabled = False
                cmdApply.Visible = True
            ElseIf r_bIsNumberingSchemeExists And Not r_bIsReadOnly Then
                lblIDReference.Enabled = True
                txtIDReference.Enabled = True
                cmdApply.Enabled = True
            Else
                lblIDReference.Enabled = True
                txtIDReference.Enabled = True
                cmdApply.Visible = False
            End If



        Catch ex As Exception

            ' Log Error
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function


    Private Function GeneratePartyCode() As Integer
        Dim result As Integer = 0
        Dim bSIRPolicyNumMaint As Object

        Const kMethodName As String = "GeneratePartyCode"
        Dim sFailureReason, sGeneratedClientCode, sAgentGroupName As String

        Dim oClientNumber As bSIRPolicyNumMaint.Business
        Dim iBranchId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bIsSetMaskingCode And txtIDReference.Text = "" Then
                Dim temp_oClientNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oClientNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oClientNumber = temp_oClientNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Raise Error.
                    gPMFunctions.RaiseError("GeneratePartyCode", "Unable to get instance of  bSIRPolicyNumMaint.Business")
                    Return result
                End If

                sAgentGroupName = AgentGroupName
                sGeneratedClientCode = AgentGroupName
                iBranchId = BranchId
ReCreateCode:

                m_lReturn = oClientNumber.GenerateClientCode(v_sPartyType:=gSIRLibrary.SIRPartyTypeAgentGroup, v_iSourceID:=iBranchId, r_sGeneratedClientCode:=sGeneratedClientCode, r_sFailureReason:=sFailureReason, v_sType:=sAgentGroupName, v_sValue:=sAgentGroupName, v_sTradeName:=sAgentGroupName)
                'Added by Nitesh for PN-72074---START (25-05-2010)
                m_sShortName = sGeneratedClientCode

                m_lReturn = m_oBusiness.CheckReference(m_sShortName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Raise Error
                    gPMFunctions.RaiseError("GeneratePartyCode", "GenerateClientCode Failed ")
                    Return result
                End If
                'If the returned reference is an empty string, then the reference exists
                If m_sShortName = "" Then
                    GoTo ReCreateCode
                End If
                'Added by Nitesh for PN-72074---END

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                    ' Raise Error
                    gPMFunctions.RaiseError("GeneratePartyCode", "GenerateClientCode Failed ")
                    Return result
                ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then  'Numbering Scheme not set
                    MessageBox.Show("Numbering scheme for Agent Group is not set.", "Agent Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                ElseIf sFailureReason <> "" Then
                    MessageBox.Show(sFailureReason, "Agent Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                txtIDReference.Text = sGeneratedClientCode
                lblIDReference.Enabled = False
                txtIDReference.Enabled = False
            End If



        Catch ex As Exception
            ' Log Error Message
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
                ' Branch
                If m_sMaskCode.IndexOf("B"c) >= 0 Then
                    If uctBranch.ListIndex < 0 Or BranchId < 1 Then
                        MessageBox.Show("Please select some Branch", "field - Branch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If

                ' Last Name, First Name
                If (m_sMaskCode.IndexOf("L"c) >= 0) Or (m_sMaskCode.IndexOf("F"c) >= 0) Or (m_sMaskCode.IndexOf("N"c) >= 0) Or (m_sMaskCode.IndexOf("I"c) >= 0) Or (m_sMaskCode.IndexOf("T"c) >= 0) Then
                    If txtName.Text = "" Then
                        MessageBox.Show("Please Enter Name", "field - Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
