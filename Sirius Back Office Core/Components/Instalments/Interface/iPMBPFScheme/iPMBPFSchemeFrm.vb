Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Forms
'refer developer guide no. 129
Imports SharedFiles
Imports System.Text
Imports Microsoft.VisualBasic.ApplicationServices
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23/10/2000
    '
    ' Description: Main interface.
    '
    ' Edit History: TF231000 - Created
    ' RAW 06/02/2003 : ISS2053 : Handle error correctly when DirectAdd fails
    '                            Changed Format of SchemeNo and SchemeVersion from string to Integer
    ' RAW 09/04/2003 : IAG ENDVR619 : tidied up handling of uctPickList
    ' MKR 08/02/2005 : PN 18586 : Handled error correctly in txtEndDate_LostFocus
    ' VB  14/02/2005 : PN 18426 : Added a procedure that delete the record that was automatically
    '                             saved without clicking OKButton. And added some codes for EditButton and DeletedButton that Enble/Disable.
    ' CJB 30/06/2005 : PN2 0043 : In Form_Load for Broking position to Head Office - branch 1 when in add mode.
    ' CJB 28/07/2005 : PN 22704 : In BusinessToInterface wrapped smoe vars with NullToString() to prevent errors
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    'Developer Guide No 7
    Private Const vbFormCode As Integer = 0

    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    'VB 14/02/2005 PN18426
    Private m_iTaskMode As Integer
    Private m_lButtonStatus As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_bChanged As Boolean

    ' {* USER DEFINED CODE (Begin) *}
    ' DataBase Attributes
    Private m_lCompanyNo As Integer
    Private m_lSchemeNo As Integer
    Private m_lSchemeVersion As Integer
    Private m_vSchemeName As String = ""
    Private m_vSchemeArray(,) As Object
    Private m_lPartyCnt As Integer
    Private m_vPartyCode As String = ""
    Private m_vPartyName As String = ""

    Private m_lPFEDIDefinitionID As Integer

    'DD 28/04/2004 - Added for Multi-Currency
    Private m_vReceiptDifference As Object
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPFScheme.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    Private m_oParty As Object
    Private m_oMediaTypeValidation As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    'PF Client Fee
    Private m_vAllowClientFees As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_bCtlDown As Boolean
    Private m_sKeyed As String = ""
    Private Const AC_KEYCONSTANT As String = "PFXML"

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control
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

    ' PWF 03/12/2002 END

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Set the interface exit status.
            m_lStatus = Value

        End Set
    End Property

    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property
    ' VB  14/02/2005 : PN 18426
    Public Property TaskMode() As Integer
        Get

            Return m_iTaskMode

        End Get
        Set(ByVal Value As Integer)

            m_iTaskMode = Value

        End Set
    End Property
    ' VB  14/02/2005 : PN 18426
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
    ' PWF 03/12/2002 Properties required by the interface class
    ' ISS1074
    Public Property CompanyNo() As Integer
        Get
            Dim result As Integer = 0
            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(m_lCompanyNo), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                result = m_lCompanyNo
            End If
            Return result
        End Get
        Set(ByVal Value As Integer)
            m_lCompanyNo = Value
        End Set
    End Property


    Public Property SchemeNo() As Integer
        Get
            Dim result As Integer = 0
            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(m_lSchemeNo), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                result = m_lSchemeNo
            End If
            Return result
        End Get
        Set(ByVal Value As Integer)
            m_lSchemeNo = Value
        End Set
    End Property


    Public Property SchemeVersion() As Integer
        Get
            Dim result As Integer = 0
            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(m_lSchemeVersion), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                result = m_lSchemeVersion
            End If
            Return result
        End Get
        Set(ByVal Value As Integer)
            m_lSchemeVersion = Value
        End Set
    End Property

    Public ReadOnly Property SchemeName() As String
        Get
            Return m_vSchemeName
        End Get
    End Property

    Public ReadOnly Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
    End Property

    Public ReadOnly Property PartyCode() As String
        Get
            Return m_vPartyCode
        End Get
    End Property

    Public ReadOnly Property PartyName() As String
        Get
            Return m_vPartyName
        End Get
    End Property



    Public Property Changed() As Boolean
        Get

            Return m_bChanged

        End Get
        Set(ByVal Value As Boolean)

            m_bChanged = Value

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            With m_oFormFields

                m_lReturn = .AddNewFormField(ctlControl:=cboBranch, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=cboSchemeType, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=cboMediaType, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=cboExportMethod, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=cboCurrency, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' RAW 06/02/2003 : ISS2053 : changed Type and Format from String
                m_lReturn = .AddNewFormField(ctlControl:=txtSchemeNo, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' RAW 06/02/2003 : ISS2053 : changed Type and Format from String
                m_lReturn = .AddNewFormField(ctlControl:=txtSchemeVersion, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtStartDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtEndDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtSchemeName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtSchemeDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Thinh Nguyen 04/12/2001 (start)
                m_lReturn = .AddNewFormField(ctlControl:=txtQuote, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtBank, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtcredit, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Alix - 08/04/2003
                m_lReturn = .AddNewFormField(ctlControl:=txtConfirmationDoc, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' /Alix

                'DD 17/11/2003
                m_lReturn = .AddNewFormField(ctlControl:=cboPrintType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Thinh Nguyen 04/12/2001 (end)

                m_lReturn = .AddNewFormField(ctlControl:=txtInsrMailboxNo, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtEDIMessageCount, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtPartyCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=actInterest, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'TR - PN5188 Made Suspense Acc mandatory
                m_lReturn = .AddNewFormField(ctlControl:=actSuspense, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=actAdmin, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=actCommissionSuspense, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=actTaxSuspense, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=cboBankAccount, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' START CHANGES - Changed By: AAB  - Changed On: 15-Oct-2003 11:36
                ' Added to support Re-Insurance Dripping
                m_lReturn = .AddNewFormField(ctlControl:=actReInsuranceSuspense, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' END CHANGES - Changed By: AAB  - Changed On: 15-Oct-2003 11:36

                'DD 17/11/2003
                m_lReturn = .AddNewFormField(ctlControl:=txtPFMessage, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'DD 28/04/2004
                m_lReturn = .AddNewFormField(ctlControl:=cboDifference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '***************************************
                'PN28314
                '***************************************
                m_lReturn = .AddNewFormField(ctlControl:=txtProcessingDays, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(
ctlControl:=actSubAgentCommissionSuspense,
lFieldType:=gPMConstants.PMEDataType.PMString,
lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                lMandatory:=PMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    SetFieldValidation = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If


            End With

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim bFound As Boolean
        Dim sDocDescription As String = "" 'MKW130204 PN10449

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            m_lPartyCnt = gPMFunctions.ToSafeLong(m_vSchemeArray(bSIRPremFinConst.k_PFSchemePartyCnt, 0))
            m_lCompanyNo = gPMFunctions.ToSafeLong(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCompanyNo, 0))
            m_vPartyCode = gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemePartyCode, 0)).Trim()

            With m_oFormFields
                If Not (Convert.IsDBNull(m_lCompanyNo) Or IsNothing(m_lCompanyNo)) Then cboBranch.ItemId = m_lCompanyNo

                m_lReturn = .FormatControl(ctlControl:=txtSchemeNo, vControlValue:=m_lSchemeNo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtSchemeVersion, vControlValue:=m_lSchemeVersion)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtStartDate, vControlValue:=m_vSchemeArray(bSIRPremFinConst.k_PFSchemeStartDate, 0))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtEndDate, vControlValue:=m_vSchemeArray(bSIRPremFinConst.k_PFSchemeEndDate, 0))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtSchemeName, vControlValue:=m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSchemeName, 0))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtSchemeDescription, vControlValue:=m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSchemeDescription, 0))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtPartyCode, vControlValue:=m_vSchemeArray(bSIRPremFinConst.k_PFSchemePartyName, 0))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                SetCombo(cboExportMethod, gPMFunctions.ToSafeLong(m_vSchemeArray(bSIRPremFinConst.k_PFSchemePaymentMethod, 0)))


                If Not (Convert.IsDBNull(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeInsrMailboxNo, 0)) Or IsNothing(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeInsrMailboxNo, 0))) Then
                    m_lReturn = .FormatControl(ctlControl:=txtInsrMailboxNo, vControlValue:=m_vSchemeArray(bSIRPremFinConst.k_PFSchemeInsrMailboxNo, 0))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                m_lReturn = .FormatControl(ctlControl:=txtEDIMessageCount, vControlValue:=m_vSchemeArray(bSIRPremFinConst.k_PFSchemeEdiMessageCount, 0))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                bFound = False
                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankAccountID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    For n As Integer = 0 To cboBankAccount.ListCount - 1
                        cboBankAccount.ListIndex = n
                        'PSL 17/02/2003 This should be bank ID not accountID
                        If cboBankAccount.Id = CDbl(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankAccountID, 0)) Then
                            bFound = True
                            Exit For
                        End If
                    Next
                End If
                Dim dbNumericTemp2 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeInterestAccountID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    actInterest.AccountId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeInterestAccountID, 0))
                End If
                Dim dbNumericTemp3 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeAdminAccountID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    actAdmin.AccountId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeAdminAccountID, 0))
                End If
                Dim dbNumericTemp4 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProtectionAccountID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    actProtection.AccountId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProtectionAccountID, 0))
                End If
                Dim dbNumericTemp5 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCommissionSuspenseAccountID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                    actCommissionSuspense.AccountId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCommissionSuspenseAccountID, 0))
                End If
                Dim dbNumericTemp6 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeTaxSuspenseAccountID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                    actTaxSuspense.AccountId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeTaxSuspenseAccountID, 0))
                End If
                Dim dbNumericTemp7 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeTaxGroupID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                    cboTaxGroupID.ItemId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeTaxGroupID, 0))
                End If
                chkSpread.CheckState = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSpreadCommission, 0))
                chkSpreadRI.CheckState = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSpreadReInsurance, 0))
                chkSpreadTaxes.CheckState = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSpreadTaxes, 0))


                If IsNumeric(m_vSchemeArray(k_PFSchemeSubAgentCommissionSuspenseAccountID, 0)) Then
                    actSubAgentCommissionSuspense.AccountId = m_vSchemeArray(k_PFSchemeSubAgentCommissionSuspenseAccountID, 0)
                End If


                chkSubAgentSpread.CheckState = m_vSchemeArray(k_PFSchemeSubAgentSpreadCommission, 0)

                chkDepositAsInstalment.CheckState = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDepositAsInstalment, 0))
                Dim dbNumericTemp8 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeReInsuranceSuspenseAccount, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
                    actReInsuranceSuspense.AccountId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeReInsuranceSuspenseAccount, 0))
                End If
                Dim dbNumericTemp9 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSuspenseAccountID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp9) Then
                    actSuspense.AccountId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSuspenseAccountID, 0))
                End If

                'Details tab
                Dim dbNumericTemp10 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteDocID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp10) Then
                    txtQuote.Tag = CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteDocID, 0))
                    m_lReturn = GetDocumentDescription(CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteDocID, 0)), sDocDescription)
                    txtQuote.Text = sDocDescription
                Else
                    txtQuote.Tag = CStr(0)
                    txtQuote.Text = ""
                End If

                Dim dbNumericTemp11 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankDocID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp11) Then
                    txtBank.Tag = CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankDocID, 0))
                    m_lReturn = GetDocumentDescription(CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankDocID, 0)), sDocDescription)
                    txtBank.Text = sDocDescription
                Else
                    txtBank.Tag = CStr(0)
                    txtBank.Text = ""
                End If
                ''Start(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(5.7.2)
                Dim dbNumericTemp12 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDocID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp12) Then
                    txtCollectionNotification.Tag = CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDocID, 0))
                    m_lReturn = GetDocumentDescription(CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDocID, 0)), sDocDescription)
                    txtCollectionNotification.Text = sDocDescription
                Else
                    txtCollectionNotification.Tag = CStr(0)
                    txtCollectionNotification.Text = ""
                End If
                ''End(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(5.7.2)

                Dim dbNumericTemp13 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCreditDocID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp13) Then
                    txtcredit.Tag = CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCreditDocID, 0))
                    m_lReturn = GetDocumentDescription(CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCreditDocID, 0)), sDocDescription)
                    txtcredit.Text = sDocDescription
                Else
                    txtcredit.Tag = CStr(0)
                    txtcredit.Text = ""
                End If

                Dim dbNumericTemp14 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDocConfirmationID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp14) Then
                    txtConfirmationDoc.Tag = CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDocConfirmationID, 0))
                    m_lReturn = GetDocumentDescription(CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDocConfirmationID, 0)), sDocDescription)
                    txtConfirmationDoc.Text = sDocDescription
                Else
                    txtConfirmationDoc.Tag = CStr(0)
                    txtConfirmationDoc.Text = ""
                End If

                Dim dbNumericTemp15 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemePFSchemeTypeID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp15) Then
                    cboSchemeType.ItemId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemePFSchemeTypeID, 0))
                End If

                Dim dbNumericTemp16 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeMediaTypeID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp16) Then
                    cboMediaType.ItemId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeMediaTypeID, 0))
                End If

                Dim dbNumericTemp17 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCurrencyID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp17) Then
                    cboCurrency.ItemId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCurrencyID, 0))
                End If

                Dim dbNumericTemp18 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemePrintTypeID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp18) Then
                    cboPrintType.ItemId = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemePrintTypeID, 0))
                End If

                chkBankName.CheckState = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankNameMandatory, 0))
                chkBankAddress.CheckState = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankAddressMandatory, 0))
                chkBranchName.CheckState = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBranchNameMandatory, 0))
                chkBranchCode.CheckState = CInt(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBranchCodeMandatory, 0))

                If CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteableInd, 0)) = "Y" Then
                    chkEnabled.CheckState = CheckState.Checked
                Else
                    chkEnabled.CheckState = CheckState.Unchecked
                End If

                chkDepositOnOtherMediaType.CheckState = gPMFunctions.ToSafeInteger(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDepositOnOtherMediaType, 0))
                chkAgentRefMandatory.CheckState = gPMFunctions.ToSafeInteger(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeAgentRefMandatory, 0))
                chkBusinessCodeMandatory.CheckState = gPMFunctions.ToSafeInteger(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBusinessCodeMandatory, 0))
                ''Start(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(5.7.2)
                Dim dbNumericTemp19 As Double
                If Double.TryParse(CStr(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDays, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp19) Then
                    txtNotificationDays.Text = gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDays, 0), "")
                Else
                    txtNotificationDays.Text = ""
                End If
                ''End(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(5.7.2)

                'PF Client Fee
                chkAllowClientFees.CheckState = IIf(gPMFunctions.ToSafeBoolean(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeAllowClientFees, 0)), 1, 0)

                '(RC) PLICO 9-10
                chkRatesForInformationOnly.CheckState = IIf(gPMFunctions.ToSafeBoolean(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeRatesForInformationOnly, 0)), 1, 0)

                m_lReturn = .FormatControl(ctlControl:=txtPFMessage, vControlValue:=gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemePFMessage, 0)))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=cboDifference, vControlValue:=gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeReceiptDifference, 0)))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                txtProviderWebsite.Text = gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderWebsite, 0))
                txtProviderUsername.Text = gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderUsername, 0))
                txtProviderBrokerID.Text = gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderBrokerID, 0))
                txtProviderTimeout.Text = gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderTimeout, 0))
                txtProviderPassword.Text = gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderPassword, 0))

                txtFinancialInstitutionCode.Text = gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeFinancialInstitutionCode, 0))
                txtDirectDebitSupplierName.Text = gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDirectDebitSupplierName, 0))
                txtDirectDebitSupplierID.Text = gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDirectDebitSupplierID, 0))
                txtRemitter.Text = gPMFunctions.ToSafeString(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeRemitter, 0))
                txtProcessingDays.Text = CStr(gPMFunctions.ToSafeLong(m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProcessingDays, 0)))
                ' PN4526
                chkPlanRefEditable.CheckState = CStr(gPMFunctions.ToSafeInteger(m_vSchemeArray(bSIRPremFinConst.k_PFSchemePlanRefEditable, 0)))


            End With

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
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1
            If m_sUniqueId = "" Then
                m_sUniqueId = gPMFunctions.GetUniqueID()
            End If
            m_sScreenHierarchy = GetScreenHierarchy()
            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    'PN12594 Pass Business Code Manadatory

                    m_lReturn = m_oBusiness.DirectAdd(lCompanyNo:=m_lCompanyNo, lSchemeNo:=m_lSchemeNo, lSchemeVersion:=m_lSchemeVersion, vSchemeArray:=m_vSchemeArray, sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy)

                    ' RAW 06/02/2003 : ISS2053 : Added
                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' Alix Bergeret - 21/02/2003 - Issue 2399
                        ' DirectAdd might fail occasionnaly if user
                        ' tries to enter twice the same values (it
                        ' breaks the primary key contraint)
                        ' When it happens, we don't want any errors to be
                        ' logged, we just warn the user.

                        result = 12345

                        ' Log Error.
                        'LogMessage _
                        'iType:=PMLogError, _
                        'sMsg:="Failed to add the interface details to business object", _
                        'vApp:=ACApp, _
                        'vClass:=ACClass, _
                        'vMethod:="InterfaceToBusiness"


                        'Developer Guide No 243
                        MessageBox.Show(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=304, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), "Finance Scheme", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    End If
                    ' RAW 06/02/2003 : ISS2053 : End
                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    'PN12594 Pass business code mandatory

                    m_lReturn = m_oBusiness.DirectEdit(lCompanyNo:=m_lCompanyNo, lSchemeNo:=m_lSchemeNo, lSchemeVersion:=m_lSchemeVersion, vSchemeArray:=m_vSchemeArray, sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    End If
            End Select

            ' {* USER DEFINED CODE (End) *}
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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            With m_oBusiness


                m_lReturn = .GetDetails(lCompanyNo:=m_lCompanyNo, lSchemeNo:=m_lSchemeNo, lSchemeVersion:=m_lSchemeVersion, r_vSchemeArray:=m_vSchemeArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
                End If


            End With

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

            'Prepare the array
            ReDim m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCountOfFields, 0)

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemePartyCnt, 0) = m_lPartyCnt

            m_lCompanyNo = cboBranch.ItemId
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCompanyNo, 0) = m_lCompanyNo

            m_lSchemeNo = CInt(txtSchemeNo.Text)
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSchemeNo, 0) = m_lSchemeNo

            m_lSchemeVersion = CInt(txtSchemeVersion.Text)
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSchemeVersion, 0) = m_lSchemeNo

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSchemeDescription, 0) = txtSchemeDescription.Text
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeStartDate, 0) = CDate(txtStartDate.Text)
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeEndDate, 0) = CDate(txtEndDate.Text)
            m_vSchemeName = txtSchemeName.Text.Trim()
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSchemeName, 0) = txtSchemeName.Text

            If cboExportMethod.SelectedIndex <> -1 Then
                m_vSchemeArray(bSIRPremFinConst.k_PFSchemePaymentMethod, 0) = VB6.GetItemData(cboExportMethod, cboExportMethod.SelectedIndex)
            Else

                m_vSchemeArray(bSIRPremFinConst.k_PFSchemePaymentMethod, 0) = DBNull.Value
            End If

            Dim dbNumericTemp As Double
            If Double.TryParse(Convert.ToString(txtQuote.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                If Conversion.Val(Convert.ToString(txtQuote.Tag)) > 0 Then
                    m_vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteDocID, 0) = Convert.ToString(txtQuote.Tag)
                Else

                    m_vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteDocID, 0) = DBNull.Value
                End If
            Else

                m_vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteDocID, 0) = DBNull.Value
            End If

            Dim dbNumericTemp2 As Double
            If Double.TryParse(Convert.ToString(txtBank.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                If Conversion.Val(Convert.ToString(txtBank.Tag)) > 0 Then
                    m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankDocID, 0) = Convert.ToString(txtBank.Tag)
                Else

                    m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankDocID, 0) = DBNull.Value
                End If
            Else

                m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankDocID, 0) = DBNull.Value
            End If

            ''Start(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(5.7.2)
            Dim dbNumericTemp3 As Double
            If Double.TryParse(Convert.ToString(txtCollectionNotification.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                If Conversion.Val(Convert.ToString(txtCollectionNotification.Tag)) > 0 Then
                    m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDocID, 0) = Convert.ToString(txtCollectionNotification.Tag)
                Else

                    m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDocID, 0) = DBNull.Value
                End If
            Else

                m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDocID, 0) = DBNull.Value
            End If
            ''End(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(5.7.2)

            Dim dbNumericTemp4 As Double
            If Double.TryParse(Convert.ToString(txtcredit.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                If Conversion.Val(Convert.ToString(txtcredit.Tag)) > 0 Then
                    m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCreditDocID, 0) = Convert.ToString(txtcredit.Tag)
                Else

                    m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCreditDocID, 0) = DBNull.Value
                End If
            Else

                m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCreditDocID, 0) = DBNull.Value
            End If

            Dim dbNumericTemp5 As Double
            If Double.TryParse(Convert.ToString(txtConfirmationDoc.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                If Conversion.Val(Convert.ToString(txtConfirmationDoc.Tag)) > 0 Then
                    m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDocConfirmationID, 0) = Convert.ToString(txtConfirmationDoc.Tag)
                Else

                    m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDocConfirmationID, 0) = DBNull.Value
                End If
            Else

                m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDocConfirmationID, 0) = DBNull.Value
            End If

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeInsrMailboxNo, 0) = txtInsrMailboxNo.Text.Trim()
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeEdiMessageCount, 0) = CInt(Conversion.Val(txtEDIMessageCount.Text))

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeMediaTypeID, 0) = cboMediaType.ItemId
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCurrencyID, 0) = cboCurrency.ItemId

            If cboPrintType.ItemId < 1 Then

                m_vSchemeArray(bSIRPremFinConst.k_PFSchemePrintTypeID, 0) = DBNull.Value
            Else
                m_vSchemeArray(bSIRPremFinConst.k_PFSchemePrintTypeID, 0) = cboPrintType.ItemId
            End If

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSpreadCommission, 0) = chkSpread.CheckState
            m_vSchemeArray(k_PFSchemeSubAgentSpreadCommission, 0) = chkSubAgentSpread.CheckState
            m_vSchemeArray(k_PFSchemeSubAgentCommissionSuspenseAccountID, 0) = gPMFunctions.ZeroToNull(actSubAgentCommissionSuspense.AccountId)


            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeInterestAccountID, 0) = gPMFunctions.ZeroToNull(actInterest.AccountId)

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeAdminAccountID, 0) = gPMFunctions.ZeroToNull(actAdmin.AccountId)

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProtectionAccountID, 0) = gPMFunctions.ZeroToNull(actProtection.AccountId)

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeTaxGroupID, 0) = gPMFunctions.ZeroToNull(cboTaxGroupID.ItemId)


            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankAccountID, 0) = gPMFunctions.ZeroToNull(cboBankAccount.Id)

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeTaxSuspenseAccountID, 0) = gPMFunctions.ZeroToNull(actTaxSuspense.AccountId)

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCommissionSuspenseAccountID, 0) = gPMFunctions.ZeroToNull(actCommissionSuspense.AccountId)

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSuspenseAccountID, 0) = gPMFunctions.ZeroToNull(actSuspense.AccountId)

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankNameMandatory, 0) = chkBankName.CheckState
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBankAddressMandatory, 0) = chkBankAddress.CheckState
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBranchNameMandatory, 0) = chkBranchName.CheckState
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBranchCodeMandatory, 0) = chkBranchCode.CheckState
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemePFSchemeTypeID, 0) = cboSchemeType.ItemId

            If chkEnabled.CheckState = CheckState.Checked Then
                m_vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteableInd, 0) = "Y"
            Else
                m_vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteableInd, 0) = "N"
            End If


            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeReInsuranceSuspenseAccount, 0) = gPMFunctions.ZeroToNull(actReInsuranceSuspense.AccountId)
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSpreadReInsurance, 0) = chkSpreadRI.CheckState
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDepositAsInstalment, 0) = chkDepositAsInstalment.CheckState
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeSpreadTaxes, 0) = chkSpreadTaxes.CheckState

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDepositOnOtherMediaType, 0) = chkDepositOnOtherMediaType.CheckState
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeAgentRefMandatory, 0) = chkAgentRefMandatory.CheckState
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemePFMessage, 0) = txtPFMessage.Text
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeBusinessCodeMandatory, 0) = chkBusinessCodeMandatory.CheckState

            ''Start(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(5.7.2)
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDays, 0) = gPMFunctions.ToSafeInteger(txtNotificationDays.Text, 0)
            ''End(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(5.7.2)

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeReceiptDifference, 0) = VB6.GetItemData(cboDifference, cboDifference.SelectedIndex)
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderWebsite, 0) = txtProviderWebsite.Text
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderUsername, 0) = txtProviderUsername.Text
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderPassword, 0) = txtProviderPassword.Text
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderBrokerID, 0) = txtProviderBrokerID.Text
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderTimeout, 0) = txtProviderTimeout.Text

            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeFinancialInstitutionCode, 0) = txtFinancialInstitutionCode.Text
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDirectDebitSupplierName, 0) = txtDirectDebitSupplierName.Text
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeDirectDebitSupplierID, 0) = txtDirectDebitSupplierID.Text
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeRemitter, 0) = txtRemitter.Text
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeProcessingDays, 0) = txtProcessingDays.Text

            '(RC) PLICO 9-10
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemeRatesForInformationOnly, 0) = chkRatesForInformationOnly.CheckState

            If chkAllowClientFees.CheckState = CheckState.Checked Then
                m_vSchemeArray(bSIRPremFinConst.k_PFSchemeAllowClientFees, 0) = 1
            Else
                m_vSchemeArray(bSIRPremFinConst.k_PFSchemeAllowClientFees, 0) = 0
            End If
            ' PN74526
            m_vSchemeArray(bSIRPremFinConst.k_PFSchemePlanRefEditable, 0) = chkPlanRefEditable.CheckState

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function FormLogic() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: FormLogic
        ' PURPOSE: Handles the field display logic depending upon scheme type
        ' AUTHOR: Danny Davis
        ' DATE: 05/06/2003, 16:57
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim sUnderwriting, sCode As String
        Dim sTypeCode As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            chkSpreadRI.Visible = True
            chkSpreadTaxes.Visible = True
            actReInsuranceSuspense.Visible = True
            chkDepositAsInstalment.Visible = True

            'DD 05/06/2003: Spreading commission is not applicable for TP
            chkSpread.Enabled = (cboSchemeType.ItemCode <> "TP" And cboSchemeType.ItemCode <> "TPR")
            chkSubAgentSpread.Visible = (cboSchemeType.ItemCode <> "TP" AndAlso cboSchemeType.ItemCode <> "TPR" AndAlso cboSchemeType.ItemCode <> "")
            chkSubAgentSpread.Enabled = (cboSchemeType.ItemCode <> "TP" And cboSchemeType.ItemCode <> "TPR")


            sCode = m_oMediaTypeValidation.GetValidationCode(cboMediaType.ItemId)
            If (sCode = ACMediaTypeBank Or sCode = ACMediaTypeCreditCard) And cboSchemeType.ItemCode <> "TPSG" Then
                m_oFormFields.Item("cboExportMethod-0").IsMandatory = True
                cboExportMethod.Visible = True
                lblExportMethod.Visible = True
                If sCode = ACMediaTypeBank Then
                    chkBankAddress.Enabled = True
                    chkBankName.Enabled = True
                    chkBranchCode.Enabled = True
                    chkBranchName.Enabled = True
                Else
                    chkBankAddress.Enabled = False
                    chkBankName.Enabled = False
                    chkBranchCode.Enabled = False
                    chkBranchName.Enabled = False
                    chkBankAddress.CheckState = CheckState.Unchecked
                    chkBankName.CheckState = CheckState.Unchecked
                    chkBranchCode.CheckState = CheckState.Unchecked
                    chkBranchName.CheckState = CheckState.Unchecked
                End If
            Else
                m_oFormFields.Item("cboExportMethod-0").IsMandatory = False
                cboExportMethod.Visible = False
                lblExportMethod.Visible = False
                chkBankAddress.Enabled = False
                chkBankName.Enabled = False
                chkBranchCode.Enabled = False
                chkBranchName.Enabled = False
                chkBankAddress.CheckState = CheckState.Unchecked
                chkBankName.CheckState = CheckState.Unchecked
                chkBranchCode.CheckState = CheckState.Unchecked
                chkBranchName.CheckState = CheckState.Unchecked
            End If

            'DD 05/06/2003: Third Party has a Finance Provider and EDI
            'Developer Guide No 292
            If cboSchemeType.ItemCode.Length >= 2 Then
                sTypeCode = cboSchemeType.ItemCode.Substring(0, 2)
            End If
            framFP.Enabled = (sTypeCode = "TP")
            lblPartyCode.Enabled = (sTypeCode = "TP")
            cmdRelatedPartyFind.Enabled = (sTypeCode = "TP")
            txtPartyCode.Enabled = (sTypeCode = "TP")
            framEDI.Enabled = (cboSchemeType.ItemCode = "TP")
            lblInsrMailboxNo.Enabled = (cboSchemeType.ItemCode = "TP")
            txtInsrMailboxNo.Enabled = (cboSchemeType.ItemCode = "TP")
            lblEDIMessageCount.Enabled = (cboSchemeType.ItemCode = "TP")
            txtEDIMessageCount.Enabled = (cboSchemeType.ItemCode = "TP")
            If sTypeCode <> "TP" Then
                txtPartyCode.Text = ""
                m_lPartyCnt = 0
                m_vPartyCode = ""
                m_vPartyName = ""
            End If

            If cboSchemeType.ItemCode = "IH" Or cboSchemeType.ItemCode = "EIH" Then
                SSTabHelper.SetTabVisible(tabMainTab, 3, True)
                SSTabHelper.SetTabVisible(tabMainTab, 4, True)
                SSTabHelper.SetTabVisible(tabMainTab, 5, True)
                cmdNext(5).Visible = True
                SSTabHelper.SetTabVisible(tabMainTab, 6, True)
                framEDI.Enabled = False
                fraXMLExport.Enabled = True

                m_oFormFields.Item("actSuspense-0").IsMandatory = True
                m_oFormFields.Item("actInterest-0").IsMandatory = True
                m_oFormFields.Item("actCommissionSuspense-0").IsMandatory = chkSpread.Enabled
                m_oFormFields.Item("actSubAgentCommissionSuspense-0").IsMandatory = chkSubAgentSpread.Enabled

                m_oFormFields.Item("cboBankAccount-0").IsMandatory = True
                chkDepositOnOtherMediaType.CheckState = CheckState.Unchecked
                chkDepositOnOtherMediaType.Visible = False
                txtPFMessage.Text = ""
                txtPFMessage.Visible = False
                lblPFMessage.Visible = False
                chkAgentRefMandatory.CheckState = CheckState.Unchecked
                chkAgentRefMandatory.Visible = False
                chkBusinessCodeMandatory.CheckState = CheckState.Unchecked
                chkBusinessCodeMandatory.Visible = False
            ElseIf sTypeCode = "TP" Then
                'DD 05/06/2003: There are no accounts required for TP and TPR
                SSTabHelper.SetTabVisible(tabMainTab, 3, True) ' PN:71547 Parellel of PN:67389 ' The Value has been "False To True" as per Steev Ross Suggestion under PN:67389
                SSTabHelper.SetTabVisible(tabMainTab, 4, True)
                SSTabHelper.SetTabVisible(tabMainTab, 5, True)
                cmdNext(5).Visible = False
                SSTabHelper.SetTabVisible(tabMainTab, 6, True)
                m_oFormFields.Item("actSuspense-0").IsMandatory = False
                m_oFormFields.Item("actInterest-0").IsMandatory = False
                m_oFormFields.Item("actCommissionSuspense-0").IsMandatory = False
                m_oFormFields.Item("actSubAgentCommissionSuspense-0").IsMandatory = False
                m_oFormFields.Item("cboBankAccount-0").IsMandatory = False
                chkSpread.CheckState = CheckState.Unchecked
                chkDepositOnOtherMediaType.Visible = True
                txtPFMessage.Visible = True
                lblPFMessage.Visible = True
                chkAgentRefMandatory.Visible = True
                chkBusinessCodeMandatory.Visible = True
                framEDI.Enabled = True
                fraXMLExport.Enabled = False
            End If

            If sTypeCode = "TP" Then
                SSTabHelper.SetTabCaption(tabMainTab, 5, "5 - Branches")
                SSTabHelper.SetTabCaption(tabMainTab, 6, "6 - Products")
            Else
                SSTabHelper.SetTabCaption(tabMainTab, 5, "6 - Branches")
                SSTabHelper.SetTabCaption(tabMainTab, 6, "7 - Products")
            End If


            framXML.Enabled = (sTypeCode = "TPSG")

            lblCommissionSuspense.Enabled = (chkSpread.CheckState = CheckState.Checked)
            actCommissionSuspense.Enabled = (chkSpread.CheckState = CheckState.Checked)
            m_oFormFields.Item("actCommissionSuspense-0").IsMandatory = (chkSpread.CheckState = CheckState.Checked)

            lblSubAgentCommissionSuspense.Enabled = (chkSubAgentSpread.CheckState = CheckState.Checked)
            actSubAgentCommissionSuspense.Enabled = (chkSubAgentSpread.CheckState = CheckState.Checked)
            m_oFormFields.Item("actSubAgentCommissionSuspense-0").IsMandatory = (chkSubAgentSpread.CheckState = CheckState.Checked)

            ' To support Reinsurance Dripping
            lblReInsuranceSuspense.Enabled = (chkSpreadRI.CheckState = CheckState.Checked)
            actReInsuranceSuspense.Enabled = (chkSpreadRI.CheckState = CheckState.Checked)
            m_oFormFields.Item("actReInsuranceSuspense-0").IsMandatory = (chkSpreadRI.CheckState = CheckState.Checked)

            m_oFormFields.Item("txtPartyCode-0").IsMandatory = (cboSchemeType.ItemCode = "TP")

            If cboSchemeType.ItemId = 4 Then
                cboPrintType.Visible = False
                lblPrintType.Visible = False
            Else
                cboPrintType.Visible = True
                lblPrintType.Visible = True
            End If

            m_lReturn = EnableDisableControls(v_iTab:=SSTabHelper.GetSelectedIndex(tabMainTab))

            If cboSchemeType.ItemCode = "TP" Then
                lblPartyCode.Font = VB6.FontChangeBold(lblPartyCode.Font, True)
                chkDepositAsInstalment.CheckState = CheckState.Unchecked
                chkDepositAsInstalment.Visible = False
                chkDepositOnOtherMediaType.CheckState = CheckState.Unchecked
                chkDepositOnOtherMediaType.Visible = False
                chkRatesForInformationOnly.Visible = True
                SSTabHelper.SetTabVisible(tabMainTab, 4, False)
            Else
                lblPartyCode.Font = VB6.FontChangeBold(lblPartyCode.Font, False)
                chkDepositAsInstalment.Visible = True
                chkDepositOnOtherMediaType.Visible = True
                chkRatesForInformationOnly.CheckState = CheckState.Unchecked
                chkRatesForInformationOnly.Visible = False
                SSTabHelper.SetTabVisible(tabMainTab, 4, True)
            End If

            'Allow Client Fee must always be hidden for all PFScheme_Type's
            chkAllowClientFees.Visible = False



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="FormLogic", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
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
            m_lReturn = DisplayCaptions()

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

            ' Set control order for tabs
            m_lReturn = SetFirstLastControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Force first tab to display
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            ' Set any other default values to the interface.

            SSTabHelper.SetTabVisible(tabMainTab, 5, True)

            frameTax.Visible = True


            If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                cboSchemeType.Enabled = False
                cboBranch.Enabled = False
                txtSchemeNo.Enabled = False
                txtSchemeVersion.Enabled = False
            Else
                chkEnabled.CheckState = CheckState.Checked
                ' PN74526
                chkPlanRefEditable.CheckState = CheckState.Checked
            End If


            FormLogic()
            UpdateTransactionTypeVisibility()

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
            ReDim m_ctlTabFirstLast(1, 6)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            m_ctlTabFirstLast(ACControlStart, 0) = cboSchemeType
            m_ctlTabFirstLast(ACControlEnd, 0) = chkEnabled
            m_ctlTabFirstLast(ACControlStart, 1) = cboMediaType
            m_ctlTabFirstLast(ACControlEnd, 1) = chkBusinessCodeMandatory
            m_ctlTabFirstLast(ACControlStart, 2) = lvRates
            m_ctlTabFirstLast(ACControlEnd, 2) = cmdDelete
            m_ctlTabFirstLast(ACControlStart, 3) = cboBankAccount
            m_ctlTabFirstLast(ACControlEnd, 3) = actTaxSuspense
            m_ctlTabFirstLast(ACControlStart, 4) = txtInsrMailboxNo
            m_ctlTabFirstLast(ACControlEnd, 4) = txtProviderTimeout
            m_ctlTabFirstLast(ACControlStart, 5) = uctPickListBranches
            m_ctlTabFirstLast(ACControlEnd, 5) = uctPickListBranches
            m_ctlTabFirstLast(ACControlStart, 6) = uctPickListProducts
            m_ctlTabFirstLast(ACControlEnd, 6) = uctPickListProducts


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


            'Developer guide No 243
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


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}


            lblSchemeNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSchemeNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSchemeVersion.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSchemeVersion, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSchemeName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSchemeName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lblStartDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStartDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEndDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEndDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblInsrMailboxNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInsrMailboxNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEDIMessageCount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEdiMessageCount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkAllowClientFees.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllowClientFees, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            '    '(RC) PLICO 9-10
            '    chkRatesForInformationOnly.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACRatesForInformationOnly, _
            ''        iDataType:=PMResString)


            '    lblQuote.Caption = GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lId:=ACDocQuote, _
            'iDataType:=PMResString)

            '    lblCredit.Caption = GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lId:=ACDocCreditDetail, _
            'iDataType:=PMResString)

            ' Alix Bergeret - 08/04/2003
            '    lblConfirmationDoc.Caption = GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lId:=ACDocConfirmation, _
            'iDataType:=PMResString)

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
    ' Name: ProcessFindParty
    '
    ' Description: Process the Party lookup component.
    '
    ' ***************************************************************** '

    Private Function ProcessFindParty() As Integer
        Dim result As Integer = 0
        Dim sCompanyNo As String = ""


        Dim oFindParty As iPMBFindParty.Interface_Renamed

        Dim oBusinessPartyFP As Object
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Create Find Party object
            Dim temp_oFindParty As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                result = gPMConstants.PMEReturnCode.PMFalse
                oFindParty = Nothing
                Return result
            End If

            With oFindParty
                ' Set the process modes.

                m_lReturn = .SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' carry on - let FindParty use defaults
                End If

                ' Set the properties.


                .CallingAppName = m_sCallingAppName

                .ShortName = m_vPartyCode

                .SpecialParty = gSIRLibrary.SIRPartyTypeFinanceProvider


                m_lReturn = .Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Retrieve Party properties

                If .Status = gPMConstants.PMEReturnCode.PMOK Then

                    m_lPartyCnt = .PartyCnt


                    txtPartyCode.Text = .LongName.Trim()

                    m_vPartyCode = .ShortName.Trim()

                    Dim temp_oBusinessPartyFP As Object = Nothing
                    m_lReturn = g_oObjectManager.GetInstance(temp_oBusinessPartyFP, "bSIRPartyFP.Business", vInstanceManager:="ClientManager")
                    oBusinessPartyFP = temp_oBusinessPartyFP

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Failed to get an instance of the business object.
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                        ' Display error stating the problem.

                        ' Get description from the resource file.

                        'Developer Guide No 243
                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        'Developer Guide No 243
                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        ' Display message.
                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Return result
                    End If


                    m_lReturn = oBusinessPartyFP.GetNext(vPartyCnt:=m_lPartyCnt, vPFEDIDefinitionID:=m_lPFEDIDefinitionID)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
                    End If
                    If m_lPFEDIDefinitionID = 1 Then
                        chkAllowClientFees.CheckState = CheckState.Checked
                    Else
                        chkAllowClientFees.CheckState = CheckState.Unchecked
                    End If


                    oBusinessPartyFP.Dispose()
                    oBusinessPartyFP = Nothing


                    'Retrieve the Company No

                    m_oBusiness.GetCompanyNo(m_lPartyCnt, sCompanyNo)
                    m_lCompanyNo = CInt(sCompanyNo)
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' Destroy Find Party object

            oFindParty.Dispose()
            oFindParty = Nothing

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Find Party.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFindParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPaymentMethods
    '
    ' Description:  Populates the combo box with the available
    '               payment methods.
    '
    ' History: 11/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function GetPaymentMethods() As Integer
        Dim result As Integer = 0
        Dim oExport As bSIRPFExport.Business
        Dim vArray(,) As Object = Nothing

        Try

            ' Create Find Party object
            Dim temp_oExport As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oExport, "bSIRPFExport.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oExport = temp_oExport

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                result = gPMConstants.PMEReturnCode.PMFalse
                oExport = Nothing
                Return result
            End If


            oExport.GetPaymentMethods(vArray)

            With cboExportMethod
                .Items.Clear()

                For nRow As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    'Developer Guide No 153
                    Dim listIndex As Integer = .Items.Add(New VB6.ListBoxItem(CStr(vArray(1, nRow)), CInt(vArray(0, nRow))))

                Next nRow
            End With


            oExport.Dispose()
            oExport = Nothing

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentMethods Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentMethods", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function RefreshGrid() As Integer
        Dim result As Integer = 0
        Dim nEnd, nStart As Integer
        Dim lvItem As ListViewItem

        Dim vResultArray(,) As Object = Nothing

        'Get the data

        m_lReturn = m_oBusiness.GetRatesList(m_lCompanyNo, m_lSchemeNo, m_lSchemeVersion, vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot retrieve rates list.", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshGrid", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            result = m_lReturn
        End If

        'Clear the grid and rebuild
        lvRates.Items.Clear()
        If Information.IsArray(vResultArray) Then

            nStart = vResultArray.GetLowerBound(1)

            nEnd = vResultArray.GetUpperBound(1)

            For nRow As Integer = nStart To nEnd

                lvItem = lvRates.Items.Add(CDate(vResultArray(1, nRow)))

                ListViewHelper.GetListViewSubItem(lvItem, 1).Text = CDate(vResultArray(2, nRow))

                ListViewHelper.GetListViewSubItem(lvItem, 2).Text = CStr(vResultArray(3, nRow)).Trim()

                ListViewHelper.GetListViewSubItem(lvItem, 3).Text = CStr(vResultArray(4, nRow))

                ListViewHelper.GetListViewSubItem(lvItem, 4).Text = CStr(vResultArray(5, nRow))

                ListViewHelper.GetListViewSubItem(lvItem, 5).Text = CStr(vResultArray(0, nRow))
                lvItem = Nothing
            Next nRow
        End If

        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    '*************************************************************************************
    ' Name        : DeleteTempData
    ' Date        : 14/02/2005
    ' Description : Added a procedure that delete the record that is automatically
    '               saved without clicking OKButton
    ' History     :
    '*************************************************************************************

    Private Sub DeleteTempData()

        Dim oRates As bSIRPFRF.Business
        Dim oBusiness As bSIRPFScheme.Business

        Try

            Dim lRow As Integer
            Dim sProductFamily As String = ""
            If Me.TaskMode = gPMConstants.PMEComponentAction.PMAdd And m_lButtonStatus <> gPMConstants.PMEReturnCode.PMOK And Me.Task = gPMConstants.PMEComponentAction.PMEdit Then


                'Pass the Product Family properly (previously only passing 1 char)
                lRow = 1

                'Load Rates Form
                Dim temp_oRates As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oRates, "bSIRPFRF.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oRates = temp_oRates

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Rates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_click", vErrNo:=Information.Err().Number, vErrDesc:="Failed to get Business Object.")

                Else

                    Do While lvRates.Items.Count >= lRow

                        'At the moment the longest Prod Family code is 3 chars ("MTA" & "REN")
                        sProductFamily = ListViewHelper.GetListViewSubItem(lvRates.Items.Item(lRow - 1), 3).Text.Substring(0, 3).Trim()

                        m_lReturn = oRates.DirectDelete(CInt(ListViewHelper.GetListViewSubItem(lvRates.Items.Item(lRow - 1), 5).Text), CDate(lvRates.Items.Item(lRow - 1).Text), sProductFamily)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Rates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_click", vErrNo:=Information.Err().Number, vErrDesc:="Failed to delete Temporary data the Rates File.")
                            Exit Do
                        End If
                        lRow += 1
                    Loop
                End If

                oRates = Nothing


                Dim temp_oBusiness As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRPFScheme.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oBusiness = temp_oBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If Not (oBusiness Is Nothing) Then

                        oBusiness.Dispose()
                        oBusiness = Nothing
                    End If

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the PFScheme Business Object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Else
                    If m_sUniqueId = "" Then
                        m_sUniqueId = gPMFunctions.GetUniqueID()
                    End If
                    m_lReturn = oBusiness.DirectDelete(cboBranch.ItemId, txtSchemeNo.Text, txtSchemeVersion.Text, m_sUniqueId)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then Throw New Exception()

                    oBusiness = Nothing
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                End If
            End If

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete Temporary data", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTempData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub actAdmin_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles actAdmin.Change
        m_bChanged = True
    End Sub
    Private Sub actCommissionSuspense_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles actCommissionSuspense.Change
        m_bChanged = True
    End Sub
    Private Sub actInterest_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles actInterest.Change
        m_bChanged = True
    End Sub
    Private Sub actProtection_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles actProtection.Change
        m_bChanged = True
    End Sub
    Private Sub actTaxSuspense_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles actTaxSuspense.Change
        m_bChanged = True
    End Sub
    'UPGRADE_NOTE: (7001) The following declaration (cbobank_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cbobank_Click()
    'm_bChanged = True
    'End Sub
    Private Sub cboBranch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranch.Click
        Dim iCurrencyID As Integer

        m_bChanged = True

        Dim lId As Integer = cboBranch.ItemId

        m_lReturn = m_oParty.GetBaseCurrencyID(lId, iCurrencyID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot retrieve Base Currency for Branch.", vApp:=ACApp, vClass:=ACClass, vMethod:="cboBranch_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Exit Sub
        End If
        'PM030108-Start
        cboBankAccount.CompanyId = lId
        cboBankAccount.RefreshList()
        'PM030108-End
        If iCurrencyID <> 0 Then
            cboCurrency.ItemId = iCurrencyID
        End If
    End Sub
    'UPGRADE_NOTE: (7001) The following declaration (cboCredit_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cboCredit_Click()
    'm_bChanged = True
    'End Sub
    Private Sub cboCurrency_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.Click
        m_bChanged = True
    End Sub
    Private Sub cboExportMethod_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboExportMethod.SelectionChangeCommitted
        m_bChanged = True
    End Sub
    Private Sub cboMediaType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMediaType.Click
        m_bChanged = True
        FormLogic()
    End Sub
    Private Sub cboPrintType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPrintType.Click
        If cboPrintType.ListIndex > 0 Then
            m_oFormFields.Item("txtCredit-0").IsMandatory = gPMConstants.PMEReturnCode.PMTrue 'MKW130204 PN10449
            cmdCredit.Font = VB6.FontChangeBold(cmdCredit.Font, True) 'MKW130204 PN10449
        Else
            m_oFormFields.Item("txtCredit-0").IsMandatory = gPMConstants.PMEReturnCode.PMFalse 'MKW130204 PN10449
            cmdCredit.Font = VB6.FontChangeBold(cmdCredit.Font, False) 'MKW130204 PN10449
        End If
        m_bChanged = True
    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cboQuote_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cboQuote_Click()
    'm_bChanged = True
    'End Sub
    Private Sub cboSchemeType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSchemeType.Click
        Try
            FormLogic()
            UpdateTransactionTypeVisibility()
            m_bChanged = True

        Catch
        End Try

    End Sub

    ' ADO #39458 T8: Show/hide Transaction Type dropdown based on scheme type
    Private Sub UpdateTransactionTypeVisibility()
        Dim bIsClaimRecovery As Boolean = (cboSchemeType.ItemCode = "CR")
        lblTransactionType.Visible = bIsClaimRecovery
        cboTransactionType.Visible = bIsClaimRecovery
        If bIsClaimRecovery AndAlso cboTransactionType.SelectedIndex = -1 Then
            cboTransactionType.SelectedIndex = 0
        End If
    End Sub

    Private Sub cboTransactionType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTransactionType.SelectedIndexChanged
        If isInitializingComponent Then Exit Sub
        m_bChanged = True
    End Sub
    'UPGRADE_NOTE: (7001) The following declaration (cboTaxType_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cboTaxType_Click()
    'If cboPrintType.ListIndex > 0 Then
    'm_oFormFields.Item("actTaxSuspense-0").IsMandatory = gPMConstants.PMEReturnCode.PMTrue
    'lblTaxSuspense.Font = VB6.FontChangeBold(lblTaxSuspense.Font, True)
    'Else
    'm_oFormFields.Item("actTaxSuspense-0").IsMandatory = gPMConstants.PMEReturnCode.PMFalse
    'lblTaxSuspense.Font = VB6.FontChangeBold(lblTaxSuspense.Font, False)
    'End If
    'm_bChanged = True
    'End Sub

    Private Sub chkBankAddress_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkBankAddress.CheckStateChanged
        m_bChanged = True
    End Sub
    Private Sub chkBankName_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkBankName.CheckStateChanged
        m_bChanged = True
    End Sub
    Private Sub chkBranchCode_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkBranchCode.CheckStateChanged
        m_bChanged = True
    End Sub
    Private Sub chkBranchName_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkBranchName.CheckStateChanged
        m_bChanged = True
    End Sub
    Private Sub chkEnabled_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkEnabled.CheckStateChanged
        m_bChanged = True
    End Sub
    Private Sub chkSpread_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSpread.CheckStateChanged
        FormLogic()
        m_bChanged = True
    End Sub

    Private Sub chkSpreadRI_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSpreadRI.CheckStateChanged
        FormLogic()
        m_bChanged = True
    End Sub


    Private Sub chkSpreadTaxes_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSpreadTaxes.CheckStateChanged
        FormLogic()
        m_bChanged = True
    End Sub


    Private Sub cmbBank_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbBank.Click
        m_lReturn = GetDocument(txtDocument:=txtBank)
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        Dim bIsSchemeRangeValid As Boolean

        'Save the current record, if adding in add mode
        If Task = gPMConstants.PMEComponentAction.PMAdd Then


            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Check if scheme no and version are in valid range
            bIsSchemeRangeValid = CheckSchemeValuesRange()

            ' If either scheme no or scheme version is not
            ' valid then do not proceed further
            If Not bIsSchemeRangeValid Then
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'VB 14/02/2005 PN-18426 Its only save PickList datas when OkButton Click
                'Save the pick lists
                'SetPickListPKs
                'uctPickListBranches.Save
                'uctPickListProducts.Save

                ' Everything OK, so we can change the status
                Task = gPMConstants.PMEComponentAction.PMEdit
            End If

            ' RAW 06/02/2003 : ISS2053 : added
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            ' RAW 06/02/2003 : ISS2053 : end
        End If

        OpenRates(gPMConstants.PMEComponentAction.PMAdd)

        m_bChanged = True

        'VB  14/02/2005 : PN 18426 For Enbling/Disabling EditButton and DeleteButton
        If lvRates.Items.Count > 0 Then
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
        Else
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
        End If

    End Sub

    Private Sub cmdBankClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBankClear.Click
        txtBank.Text = ""
        txtBank.Tag = CStr(0)
    End Sub
    ''Start(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(5.7.2)
    Private Sub cmdCollectionNotification_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCollectionNotification.Click
        m_lReturn = GetDocument(txtDocument:=txtCollectionNotification)
    End Sub

    Private Sub cmdCollectionNotificationClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCollectionNotificationClear.Click
        txtCollectionNotification.Text = ""
        txtCollectionNotification.Tag = CStr(0)
    End Sub
    ''End(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(5.7.2)
    Private Sub cmdConfirmationDoc_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdConfirmationDoc.Click
        m_lReturn = GetDocument(txtDocument:=txtConfirmationDoc)
    End Sub

    Private Sub cmdConfirmationDocClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdConfirmationDocClear.Click
        txtConfirmationDoc.Text = ""
        txtConfirmationDoc.Tag = CStr(0)
    End Sub

    Private Sub cmdCredit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCredit.Click
        m_lReturn = GetDocument(txtDocument:=txtcredit)
    End Sub

    Private Sub cmdCreditClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCreditClear.Click
        txtcredit.Text = ""
        txtcredit.Tag = CStr(0)
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        Dim oRates As bSIRPFRF.Business
        'RKS 09/11/2004 PN16268
        If lvRates.FocusedItem Is Nothing Then Exit Sub

        Dim sProductFamily As String = ""

        If MessageBox.Show("Are you sure you want to delete this Rates File?", "Delete Rates File", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then

            'Pass the Product Family properly (previously only passing 1 char)

            'Load Rates Form
            Dim temp_oRates As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oRates, "bSIRPFRF.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oRates = temp_oRates

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Rates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_click", vErrNo:=Information.Err().Number, vErrDesc:="Failed to get Business Object.")
                Exit Sub
            Else
                'TR - At the moment the longest Prod Family code is 3 chars ("MTA" & "REN")
                'refer developer guide no. 52
                'Developer Guide No 292
                If lvRates.FocusedItem.SubItems(3).Text.Length >= 3 Then
                    sProductFamily = lvRates.FocusedItem.SubItems(3).Text.Substring(0, 3).Trim()
                Else
                    sProductFamily = lvRates.FocusedItem.SubItems(3).Text.Trim()
                End If
                If m_sUniqueId = "" Then
                    m_sUniqueId = gPMFunctions.GetUniqueID()
                End If
                If m_sScreenHierarchy = "" Then
                    m_sScreenHierarchy = GetScreenHierarchy()
                End If
                'refer developer guide no. 52
                m_lReturn = oRates.DirectDelete(CInt(lvRates.FocusedItem.SubItems(5).Text), CDate(lvRates.FocusedItem.Text), sProductFamily, m_sUniqueId, m_sScreenHierarchy)

                If m_lReturn = gPMConstants.PMEReturnCode.PMFail Then
                    MessageBox.Show("Failed to delete the Rates File. There is probably a Plan already using it.", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Rates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_click", vErrNo:=Information.Err().Number, vErrDesc:="Failed to delete the Rates File. There is probably a Plan already using it.")
                    Exit Sub
                Else
                    RefreshGrid()
                End If
            End If
        End If

        m_bChanged = True

        If lvRates.Items.Count > 0 Then
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
        Else
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
        End If
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        If Not (lvRates.FocusedItem Is Nothing) Then
            OpenRates(gPMConstants.PMEComponentAction.PMEdit)
        End If

        m_bChanged = True

    End Sub

    ' ***************************************************************** '
    '
    ' Name: OpenRates
    '
    ' Description:  Opens the rates from for adding/editing
    '
    ' History: 12/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Sub OpenRates(ByRef nTask As Integer)

        Try


            Dim oRates As iPMBPFRF.Interface_Renamed

            'Load Rates Form
            Dim temp_oRates As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oRates, sClassName:="iPMBPFRF.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oRates = temp_oRates

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            If m_sUniqueId = "" Then
                m_sUniqueId = gPMFunctions.GetUniqueID()
            End If
            With oRates

                m_lReturn = .SetProcessModes(vTask:=nTask)

                .CompanyNo = m_lCompanyNo

                .SchemeNumber = m_lSchemeNo

                .SchemeVersion = m_lSchemeVersion

                .SchemeType = cboSchemeType.ItemCode

                .UniqueId = m_sUniqueId

                .ParentScreenHierarchy = GetScreenHierarchy()

                If lvRates.Items.Count > 0 Then

                    'refer developer guide no. 52
                    'Developer guide No 234
                    If Not Information.IsNothing(lvRates.FocusedItem) Then
                        .PFRFID = CInt(lvRates.FocusedItem.SubItems(5).Text)
                    End If
                Else

                    .PFRFID = 0
                End If
                If nTask = gPMConstants.PMEComponentAction.PMEdit Then

                    .StartDate = CDate(lvRates.FocusedItem.Text)

                    'refer developer guide no. 52
                    'Developer Guide No 292
                    If lvRates.FocusedItem.SubItems(3).Text.Length >= 3 Then
                        .Productfamily = lvRates.FocusedItem.SubItems(3).Text.Substring(0, 3).Trim()
                    End If
                End If

                .SpreadCommission = chkSpread.Checked
                m_lReturn = .Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    .Dispose()
                    Exit Sub
                End If


                .Dispose()
                RefreshGrid()
            End With

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenRates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenRates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        'refer developer guide no. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(objCnt:=cmdHelp, lContextID:=ScreenHelpID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        End If


    End Sub

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_5.Click, _cmdNext_4.Click, _cmdNext_3.Click, _cmdNext_1.Click, _cmdNext_0.Click, _cmdNext_2.Click

        Dim lTab As Integer

        Try

            'Find next visible tab
            lTab = SSTabHelper.GetSelectedIndex(tabMainTab)
            Do
                lTab += 1
            Loop While lTab < SSTabHelper.GetTabCount(tabMainTab) And Not SSTabHelper.GetTabVisible(tabMainTab, lTab)

            'Change to the next visible tab.
            SSTabHelper.SetSelectedIndex(tabMainTab, lTab)

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, lTab).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try

    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_5.Click, _cmdPrevious_4.Click, _cmdPrevious_3.Click, _cmdPrevious_2.Click, _cmdPrevious_1.Click, _cmdPrevious_0.Click

        Dim lTab As Integer

        Try

            'Find next visible tab
            lTab = SSTabHelper.GetSelectedIndex(tabMainTab)
            Do
                lTab -= 1
            Loop While lTab > 0 And Not SSTabHelper.GetTabVisible(tabMainTab, lTab)

            'Change to the next visible tab.
            SSTabHelper.SetSelectedIndex(tabMainTab, lTab)

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, lTab).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try

    End Sub


    Private Sub SetPickListPKs()

        uctPickListBranches.ForeignKeys.Item("CompanyNo").Value = m_lCompanyNo

        uctPickListBranches.ForeignKeys.Item("SchemeNo").Value = m_lSchemeNo

        uctPickListBranches.ForeignKeys.Item("SchemeVersion").Value = m_lSchemeVersion

        uctPickListBranches.ForeignKeys.Item("UserId").Value = g_oObjectManager.UserID

        uctPickListBranches.ForeignKeys.Item("UniqueId").Value = m_sUniqueId

        uctPickListBranches.ForeignKeys.Item("ScreenHierarchy").Value = m_sScreenHierarchy

        uctPickListProducts.ForeignKeys.Item("CompanyNo").Value = m_lCompanyNo

        uctPickListProducts.ForeignKeys.Item("SchemeNo").Value = m_lSchemeNo

        uctPickListProducts.ForeignKeys.Item("SchemeVersion").Value = m_lSchemeVersion

        uctPickListProducts.ForeignKeys.Item("UserId").Value = g_oObjectManager.UserID

        uctPickListProducts.ForeignKeys.Item("UniqueId").Value = m_sUniqueId

        uctPickListProducts.ForeignKeys.Item("ScreenHierarchy").Value = m_sScreenHierarchy
    End Sub

    Private Sub cmdQuote_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdQuote.Click
        m_lReturn = GetDocument(txtDocument:=txtQuote)
    End Sub

    Private Sub cmdQuoteClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdQuoteClear.Click
        txtQuote.Text = ""
        txtQuote.Tag = CStr(0)
    End Sub

    ' PRIVATE Methods (End)
    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPFScheme.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of bSIRPFScheme.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialize")

                Exit Sub
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oParty As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oParty, "bSIRParty.Business", vInstanceManager:="ClientManager")
            m_oParty = temp_m_oParty

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialize")

                Exit Sub
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oMediaTypeValidation As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oMediaTypeValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:="ClientManager")
            m_oMediaTypeValidation = temp_m_oMediaTypeValidation

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of bSIRMediaTypeValidation.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialize")

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBPFScheme.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

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


        Dim Key As uctPickList.PickListKey ' RAW 09/04/2003 : IAG ENDVR619 : changed to use class from uctPicklist

        Dim vValue As String = ""

        ' Forms load event.

        Try

            iPMFunc.ShowFormInTaskBar_Detach()

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

            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, g_iSourceID, vValue)

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            GetPaymentMethods()

            'Setup the picklists
            ' RAW 09/04/2003 : IAG ENDVR619 : changed to use class from uctPicklist
            Key = New uctPickList.PickListKey()
            Key.KeyName = "CompanyNo"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(Key, Key:="CompanyNo")

            ' RAW 09/04/2003 : IAG ENDVR619 : changed to use class from uctPicklist
            Key = New uctPickList.PickListKey()
            Key.KeyName = "CompanyNo"
            uctPickListProducts.ForeignKeys.Add(Key, Key:="CompanyNo")

            ' RAW 09/04/2003 : IAG ENDVR619 : changed to use class from uctPicklist
            Key = New uctPickList.PickListKey()
            Key.KeyName = "SchemeNo"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(Key, Key:="SchemeNo")

            ' RAW 09/04/2003 : IAG ENDVR619 : changed to use class from uctPicklist
            Key = New uctPickList.PickListKey()
            Key.KeyName = "SchemeNo"
            uctPickListProducts.ForeignKeys.Add(Key, Key:="SchemeNo")

            ' RAW 09/04/2003 : IAG ENDVR619 : changed to use class from uctPicklist
            Key = New uctPickList.PickListKey()
            Key.KeyName = "SchemeVersion"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(Key, Key:="SchemeVersion")

            ' RAW 09/04/2003 : IAG ENDVR619 : changed to use class from uctPicklist
            Key = New uctPickList.PickListKey()
            Key.KeyName = "SchemeVersion"
            uctPickListProducts.ForeignKeys.Add(Key, Key:="SchemeVersion")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "UserId"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(Key, Key:="UserId")

            ' RAW 09/04/2003 : IAG ENDVR619 : changed to use class from uctPicklist
            Key = New uctPickList.PickListKey()
            Key.KeyName = "UserId"
            uctPickListProducts.ForeignKeys.Add(Key, Key:="UserId")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "UniqueId"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListBranches.ForeignKeys.Add(Key, Key:="UniqueId")

            ' RAW 09/04/2003 : IAG ENDVR619 : changed to use class from uctPicklist
            Key = New uctPickList.PickListKey()
            Key.KeyName = "UniqueId"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListProducts.ForeignKeys.Add(Key, Key:="UniqueId")


            Key = New uctPickList.PickListKey()
            Key.KeyName = "ScreenHierarchy"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListBranches.ForeignKeys.Add(Key, Key:="ScreenHierarchy")

            ' RAW 09/04/2003 : IAG ENDVR619 : changed to use class from uctPicklist
            Key = New uctPickList.PickListKey()
            Key.KeyName = "ScreenHierarchy"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListProducts.ForeignKeys.Add(Key, Key:="ScreenHierarchy")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "ExtraSQL"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListBranches.ForeignKeys.Add(Key, Key:="ExtraSQL")

            uctPickListBranches.ForeignKeys.Item("ExtraSQL").Value = "AND B.source_id NOT IN (SELECT source_id FROM PMUser_Source WHERE user_id = " & g_oObjectManager.UserID & ")"

            'TODO
            cboBranch.WhereClause = "source_id NOT IN (SELECT source_id FROM PMUser_Source WHERE user_id = " & g_oObjectManager.UserID & ")"
            cboBranch.RefreshList()
            If Task = gPMConstants.PMEComponentAction.PMAdd Then

                txtEDIMessageCount.Text = CStr(0)
                cmdRelatedPartyFind.Visible = True

                cboBranch_Click(cboBranch, Nothing)



            Else
                RefreshGrid()
            End If

            'Put in the picklist PK values
            SetPickListPKs()

            ' RAW 09/04/2003 : IAG ENDVR619 : added error handling
            m_lReturn = uctPickListBranches.Load_Renamed()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load list of Branches", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' RAW 09/04/2003 : IAG ENDVR619 : added error handling
            m_lReturn = uctPickListProducts.Load_Renamed()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load list of Products", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            SetExtraListViewProperties(lvRates.Handle.ToInt32(), 1)

            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'Developer Guide No 220
            cboSchemeType.FirstItem = ""
            cboMediaType.FirstItem = ""
            cboCurrency.FirstItem = ""
            cboPrintType.FirstItem = ""
            cboTaxGroupID.FirstItem = "(None)"
            cboBranch.FirstItem = ""
            cboBankAccount.CompanyId = m_lCompanyNo
            cboBankAccount.FirstItem = ""

            cboSchemeType.ListIndex = -1
            cboMediaType.ListIndex = -1
            cboPrintType.ListIndex = 0
            cboTaxGroupID.ListIndex = -1
            cboBankAccount.ListIndex = -1
            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_lReturn = EnableDisableControls(v_iTab:=0)

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

            ' Set the interface status.
            'Changes as per VB code

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1

                    'Developer Guide No 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If
            DeleteTempData() ' VB 14/02/2005 : PN18426

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
        'If cboSchemeType.ItemCode.Length >= 2 Then
        '    sTypeCode = cboSchemeType.ItemCode.Substring(0, 2)
        'End If
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

            If cboSchemeType.ItemCode <> "TP" Then

                If eventArgs.Alt And eventArgs.KeyCode = Keys.D5 Then
                    tabMainTab.SelectedIndex = 4
                End If
                If eventArgs.Alt And eventArgs.KeyCode = Keys.D6 Then
                    tabMainTab.SelectedIndex = 5
                End If
                If eventArgs.Alt And eventArgs.KeyCode = Keys.D7 Then
                    tabMainTab.SelectedIndex = 6
                End If
            Else
                If eventArgs.Alt And eventArgs.KeyCode = Keys.D5 Then
                    tabMainTab.SelectedIndex = 4
                End If
                If eventArgs.Alt And eventArgs.KeyCode = Keys.D6 Then
                    tabMainTab.SelectedIndex = 5
                End If
            End If
        Catch




            Exit Sub
        End Try


    End Sub
    Private Sub lvRates_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvRates.DoubleClick
        cmdEdit_Click(cmdEdit, New EventArgs())
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try
            m_lReturn = EnableDisableControls(v_iTab:=SSTabHelper.GetSelectedIndex(tabMainTab))

        Catch
            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Function CheckSchemeValuesRange() As Boolean

        ' Check for max value permitted by Long data type
        If Conversion.Val(txtSchemeNo.Text) > 2147483468 Then
            MessageBox.Show("The scheme number entered exceeds the maximum value permitted for this field." &
                            " Please re-enter.", "Instalment Scheme", MessageBoxButtons.OK, MessageBoxIcon.Error)

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            txtSchemeNo.Focus()

            Return False

            ' Do not allow negative values
        ElseIf Conversion.Val(txtSchemeNo.Text) < 0 Then

            MessageBox.Show("The scheme number entered is less than the value permitted for this field." &
                            " Please re-enter.", "Instalment Scheme", MessageBoxButtons.OK, MessageBoxIcon.Error)

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            txtSchemeNo.Focus()

            Return False

        End If

        ' Check for max value permitted by Long data type
        If Conversion.Val(txtSchemeVersion.Text) > 2147483468 Then
            MessageBox.Show("The scheme version number entered exceeds the maximum value permitted for this field." &
                            " Please re-enter.", "Instalment Scheme", MessageBoxButtons.OK, MessageBoxIcon.Error)

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            txtSchemeVersion.Focus()

            Return False

            ' Do not allow negative values
        ElseIf Conversion.Val(txtSchemeVersion.Text) < 0 Then

            MessageBox.Show("The scheme version number entered is less than the value permitted for this field." &
                            " Please re-enter.", "Instalment Scheme", MessageBoxButtons.OK, MessageBoxIcon.Error)

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            txtSchemeVersion.Focus()

            Return False

        End If

        ' If control has reached here then it must be valid
        Return True

    End Function

    Private Function GetScreenHierarchy() As String
        m_sScreenHierarchy = "Instalment Scheme(" + Trim(txtSchemeName.Text) + ")"
        Return m_sScreenHierarchy

    End Function

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' PW040303
        ' ISS2446
        Dim bHasValidation As Boolean
        Dim sInvalidBranches As String = String.Empty
        Dim sPlural As String = String.Empty
        Dim blSchemeExists, bIsSchemeRangeValid As Boolean
        Dim vResultArray(,) As Object = Nothing
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            m_lButtonStatus = gPMConstants.PMEReturnCode.PMOK ' VB 14/02/2005 : PN18426

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            If (cboSchemeType.ItemCode <> "TP" And cboSchemeType.ItemCode <> "TPR") Then
                m_lReturn = m_oBusiness.ChkFinanceNetComm(lCompanyNo:=m_lCompanyNo, lSchemeNo:=m_lSchemeNo, lSchemeVersion:=m_lSchemeVersion, r_vSchemeArray:=vResultArray)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If IsArray(vResultArray) And chkSpread.Checked Then
                        MsgBox("Cannot Have Both Spread Commission and Finance Net Commission Enabled.", vbCritical, "Instalment Scheme")
                        Exit Sub
                    End If
                End If
            End If
            'DD 10/07/2003: Corrected checking of accounts
            'TR - PN5188 02/07/03 - Now check if the Account controls have been
            'populated (Account Lookups not covered by formfields object)
            If m_oFormFields.Item("actInterest-0").IsMandatory And Strings.Len(actInterest.Text) = 0 Then
                MessageBox.Show("Please select an Interest Account.", "Instalment Scheme", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If m_oFormFields.Item("actSuspense-0").IsMandatory And Strings.Len(actSuspense.Text) = 0 Then
                MessageBox.Show("Please select a Suspense Revenue Account.", "Instalment " &
                                "Scheme", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            'TR - Suspense Commission Acc only mandatory if chkSpread is checked
            If m_oFormFields.Item("actCommissionSuspense-0").IsMandatory Then
                If Strings.Len(actCommissionSuspense.Text) = 0 Then
                    MessageBox.Show("Please select a Suspense Commission Account.", "Instalment " &
                                    "Scheme", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            ' START CHANGES - Changed By: AAB  - Changed On: 15-Oct-2003 13:20
            ' To support Re-Insurance Dripping
            If m_oFormFields.Item("actReInsuranceSuspense-0").IsMandatory Then
                If Strings.Len(actReInsuranceSuspense.Text) = 0 Then
                    MessageBox.Show("Please select a Suspense ReInsurance Account.", "Instalment " &
                                    "Scheme", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
            ' END CHANGES - Changed By: AAB  - Changed On: 15-Oct-2003 13:20

            ' Check if scheme no and version are in valid range
            bIsSchemeRangeValid = CheckSchemeValuesRange()

            ' If either scheme no or scheme version is not
            ' valid then do not proceed further
            If Not bIsSchemeRangeValid Then
                Exit Sub
            End If

            ' PW040303 - check if the media type has associated validation: start
            ' ISS2446
            ' Call the business object method

            m_lReturn = m_oBusiness.CheckMediaType(vMediaTypeID:=cboMediaType.ItemId, r_bHasValidation:=bHasValidation)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            ' Inform the user
            If Not bHasValidation Then
                If MessageBox.Show("The Media Type has no associated Media Type Validation. " & "This Scheme will therefore not appear in the list of " & "available Plans when Instalments are quoted at time " & "of New Business and Renewals. Do you wish to save anyway?", "Scheme Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If
            ' PW040303 - check if the media type has associated validation: end
            ' ISS2446

            ' RDT - 05/12/2004 - PN 17309 - Check that the scheme number and version combitaion is unique when adding a new scheme
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                ' Call the business object method

                m_lReturn = m_oBusiness.ValidateSchemeNumber(v_lSchemeNo:=CInt(txtSchemeNo.Text), v_lSchemeVersion:=CInt(txtSchemeVersion.Text), r_blSchemeExists:=blSchemeExists)
                ' Check for errors
                If blSchemeExists Then
                    MessageBox.Show("A scheme with scheme number " & CInt(txtSchemeNo.Text) & " and scheme version " & CStr(CInt(txtSchemeVersion.Text)) & " already exists." & Strings.Chr(13) & Strings.Chr(10) &
                                    "The scheme number and scheme version must be unique." &
                                    " Please use a " & Strings.Chr(13) & Strings.Chr(10) & "different scheme number or version.", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    Exit Sub
                End If
            End If
            ' PW040303 - check if the media type has associated validation: end
            ' ISS2446

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'Save the pick lists
                SetPickListPKs()
                uctPickListBranches.Save()
                uctPickListProducts.Save()

                'Check the Branches chosen to ensure their base currency matches the currency
                'of the Scheme

                m_lReturn = m_oBusiness.ValidateBranches(lCompanyNo:=m_lCompanyNo, lSchemeNo:=m_lSchemeNo, lSchemeVersion:=m_lSchemeVersion, r_sInvalidBranches:=sInvalidBranches)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to run ValidateBranches", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                ElseIf sInvalidBranches <> "" Then
                    If sInvalidBranches.IndexOf(","c) >= 0 Then
                        sPlural = "es are"
                    Else
                        sPlural = " is"
                    End If

                    MessageBox.Show("The following branch" & sPlural &
                                    " invalid because the base currency is different from the scheme currency:" & Strings.Chr(13) & Strings.Chr(10) &
                                    sInvalidBranches & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                    "The invalid entries have been removed from the chosen list.", "Invalid Branches chosen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    m_lReturn = uctPickListBranches.Load_Renamed()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Load the lists", vApp:=ACApp, vClass:=ACClass, vMethod:="Load_Renamed", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If
                Else
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If
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
            m_lButtonStatus = gPMConstants.PMEReturnCode.PMCancel ' VB 14/02/2005 : PN18426
            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

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
            m_lReturn = m_oGeneral.ProcessCommand()

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

    Private Sub cmdRelatedPartyFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRelatedPartyFind.Click

        Dim sName As String
        Dim lReturn As Integer

        Try

            sName = txtPartyCode.Text.Trim()

            ' Process the find party.
            lReturn = ProcessFindParty()

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Find Party.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRelatedPartyLookup_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub tabMainTab_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles tabMainTab.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If eventArgs.KeyCode = Keys.ControlKey Then m_bCtlDown = True
    End Sub

    Private Sub tabMainTab_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles tabMainTab.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        'ACR 04-06-05 "unlock" trick for editing the provider url
        'hold control with focus on the tab and type "pfxml"

        If framXML.Enabled Then
            If eventArgs.KeyCode = Keys.ControlKey Then m_bCtlDown = False

            If m_bCtlDown And Not (eventArgs.KeyCode = Keys.ControlKey) Then
                m_sKeyed = m_sKeyed & Strings.Chr(KeyCode).ToString().ToUpper()
            Else
                m_sKeyed = ""
            End If

            If m_sKeyed = AC_KEYCONSTANT Then
                txtProviderWebsite.Enabled = True
                lblProviderWebsite.Enabled = True
            End If
        End If
    End Sub



    Private isInitializingComponent As Boolean
    Private Sub txtEDIMessageCount_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEDIMessageCount.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub
    Private Sub txtEndDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEndDate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub
    Private Sub txtInsrMailboxNo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsrMailboxNo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub
    Private Sub txtPartyCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPartyCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtProcessingDays_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtProcessingDays.Enter
        m_lReturn = m_oFormFields.GotFocus(txtProcessingDays)
    End Sub

    Private Sub txtProcessingDays_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtProcessingDays.Leave
        m_lReturn = m_oFormFields.LostFocus(txtProcessingDays)
    End Sub

    Private Sub txtSchemeDescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeDescription.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub
    Private Sub txtSchemeName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub
    Private Sub txtSchemeNo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeNo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub
    Private Sub txtSchemeNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeNo.Enter
        m_lReturn = m_oFormFields.GotFocus(txtSchemeNo)

        txtSchemeNo.MaxLength = 10
    End Sub
    Private Sub txtSchemeNo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeNo.Leave
        m_lReturn = m_oFormFields.LostFocus(txtSchemeNo)
    End Sub
    Private Sub txtEndDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEndDate.Enter
        m_lReturn = m_oFormFields.GotFocus(txtEndDate)
    End Sub
    Private Sub txtEndDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEndDate.Leave

        Try

            m_lReturn = m_oFormFields.LostFocus(txtEndDate)

            If Information.IsDate(Me.txtStartDate.Text) And Information.IsDate(Me.txtEndDate.Text) Then
                If CDate(Me.txtStartDate.Text) >= CDate(Me.txtEndDate.Text) Then
                    MessageBox.Show("The Start Date cannot be on or after the End Date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.txtEndDate.Text = ""
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    Me.txtEndDate.Focus()
                    Exit Sub
                End If
            End If

        Catch


            Exit Sub
        End Try


    End Sub
    Private Sub txtSchemeVersion_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeVersion.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub
    ' RAW 06/02/2003 : ISS2053 : added
    Private Sub txtSchemeVersion_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeVersion.Enter
        m_lReturn = m_oFormFields.GotFocus(txtSchemeVersion)

        txtSchemeVersion.MaxLength = 10
    End Sub
    Private Sub txtSchemeVersion_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeVersion.Leave
        m_lReturn = m_oFormFields.LostFocus(txtSchemeVersion)
    End Sub
    ' RAW 06/02/2003 : ISS2053 : end
    Private Sub txtStartDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStartDate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub
    Private Sub txtStartDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStartDate.Enter
        m_lReturn = m_oFormFields.GotFocus(txtStartDate)
    End Sub
    Private Sub txtStartDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStartDate.Leave

        If Information.IsDate(Me.txtStartDate.Text) And Information.IsDate(Me.txtEndDate.Text) Then
            If CDate(Me.txtStartDate.Text) >= CDate(Me.txtEndDate.Text) Then
                MessageBox.Show("The Start Date cannot be on or after the End Date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.txtStartDate.Text = ""
                Me.txtStartDate.Focus()
                Exit Sub
            End If
        End If

        m_lReturn = m_oFormFields.LostFocus(txtStartDate)

    End Sub

    ' PRIVATE Events (End)
    '***************************************************************************
    ' Name : GetDocTemplate
    '
    ' Desc : get document template details and add them to combo box
    '
    ' Hist : Thinh Nguyen 04/12/2001 Created
    '***************************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (GetDocTemplate) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetDocTemplate(ByRef r_txtTextbox As TextBox) As Integer
    '
    'Dim result As Integer = 0
    'Dim vResultArray As Object
    'Dim lCount As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oBusiness.GetDocTemplate(r_vResultArray:=vResultArray)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    '
    'If Not Information.IsArray(vResultArray) Then
    'Return gPMConstants.PMEReturnCode.PMNotFound
    'End If
    '
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Sub uctPickListBranches_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles uctPickListBranches.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        m_bChanged = True
        eventArgs.Cancel = Cancel
    End Sub
    Private Sub uctPickListProducts_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctPickListProducts.GotFocus
        m_bChanged = True
    End Sub

    Sub SetCombo(ByRef Cbo As ComboBox, ByVal lValue As Integer)


        For n As Integer = 0 To Cbo.Items.Count - 1
            If VB6.GetItemData(Cbo, n) = lValue Then
                Cbo.SelectedIndex = n
                Exit For
            End If
        Next

    End Sub

    Private Function GetDocument(ByRef txtDocument As TextBox) As Integer

        Dim result As Integer = 0
        Dim lDocumentTemplateId As Integer
        Dim sDocumentCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the document template
            m_lReturn = GetDocumentTemplate(r_lDocumentTemplateID:=lDocumentTemplateId, r_sDocumentCode:=sDocumentCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMCancel) Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If

            txtDocument.Text = sDocumentCode

            txtDocument.Tag = CStr(lDocumentTemplateId)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetDocumentTemplate
    '
    ' Description: Creates an instance of find document template and gets the properties
    '
    ' History: 01/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetDocumentTemplate(ByRef r_lDocumentTemplateID As Integer, ByRef r_sDocumentCode As String) As Integer
        Dim result As Integer = 0
        Dim oObject As iPMBFindDocTemplate.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of iPMBFindDocTemplate.Interface
            Dim temp_oObject As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Start it up

            m_lReturn = oObject.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Exit out if it was cancelled

            If oObject.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            ' Get it's properties
            With oObject

                r_lDocumentTemplateID = .DocumentTemplateId
                r_sDocumentCode = .DocumentCode
            End With

            ' Terminate it

            oObject.Dispose()

            ' Clear up
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentTemplate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function GetDocumentDescription(ByVal r_lDocumentTemplateID As Integer, ByRef r_sDocumentCode As String) As Integer
        Dim result As Integer = 0
        Dim oObject As bSIRDocTemplate.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of iPMBFindDocTemplate.Interface
            Dim temp_oObject As Object = Nothing
            'Developer Guide No 218
            'm_lReturn = g_oObjectManager.GetInstance(temp_oObject, "bSirDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, "bSIRDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oObject = temp_oObject
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oObject.GetDescription(r_lDocumentTemplateID, r_sDocumentCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear up
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentDescription Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentDescription", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: EnableDisableControls
    '
    ' Description:
    '
    ' History: 20/08/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function EnableDisableControls(ByVal v_iTab As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Frame4.Enabled = False
            framFP.Enabled = False
            fraScheme.Enabled = False
            SSFrame4.Enabled = False
            SSFrame1.Enabled = False
            Frame1.Enabled = False
            lvRates.Enabled = False
            cmdAdd.Enabled = False
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            Frame9.Enabled = False
            Frame6.Enabled = False
            fmeSuspense.Enabled = False
            frameTax.Enabled = False
            uctPickListBranches.Visible = False
            uctPickListProducts.Visible = False

            v_iTab += 1

            Select Case v_iTab
                Case 1
                    Frame4.Enabled = True
                    'Developer Guide No 292
                    If cboSchemeType.ItemCode.Length >= 2 Then
                        framFP.Enabled = (cboSchemeType.ItemCode.Substring(0, 2) = "TP")
                    End If

                    fraScheme.Enabled = True
                Case 2
                    SSFrame4.Enabled = True
                    SSFrame1.Enabled = True
                    Frame1.Enabled = True
                Case 3
                    lvRates.Enabled = True
                    cmdAdd.Enabled = True
                    If lvRates.Items.Count > 0 Then
                        cmdEdit.Enabled = True
                        cmdDelete.Enabled = True
                    Else
                        cmdEdit.Enabled = False
                        cmdDelete.Enabled = False
                    End If
                Case 4
                    Frame9.Enabled = True
                    Frame6.Enabled = True
                    fmeSuspense.Enabled = True
                    frameTax.Enabled = True
                Case 6
                    uctPickListBranches.Visible = True
                Case 7
                    uctPickListProducts.Visible = True
            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableDisableControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableDisableControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub chkSubAgentSpread_CheckStateChanged(sender As Object, e As EventArgs) Handles chkSubAgentSpread.CheckStateChanged
        FormLogic()
        m_bChanged = True
    End Sub
End Class
