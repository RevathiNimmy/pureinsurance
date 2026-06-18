Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'refer Developer Guide No. 129
Imports SharedFiles
Imports System.ComponentModel
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    '' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 24/10/2000
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
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
    Private m_iProducts(2) As String

    ' {* USER DEFINED CODE (Begin) *}
    ' DataBase Attributes
    Private m_lRateCount As Integer
    Private m_lRecordNo As Integer
    Private m_lCompanyNo As Integer
    Private m_lSchemeNo As Integer
    Private m_lSchemeVersion As Integer
    Private m_dtStartDate As Date
    Private m_sProductFamily As String = ""
    Private m_vArrangementFee As Object
    Private m_vMnemonic As Object
    Private m_vEndDate As Object
    Private m_vDaysDelay As Object
    Private m_vDepositReq As Object
    Private m_vDepositPC As Object
    Private m_vProtectRate As Object
    Private m_vMinInterest As Object
    Private m_vMin1 As Object
    Private m_vMax1 As Object
    Private m_vRate1 As Object
    Private m_vR1Com As Object
    Private m_vMin2 As Object
    Private m_vMax2 As Object
    Private m_vRate2 As Object
    Private m_vR2Com As Object
    Private m_vMin3 As Object
    Private m_vMax3 As Object
    Private m_vRate3 As Object
    Private m_vR3Com As Object
    Private m_vMin4 As Object
    Private m_vMax4 As Object
    Private m_vRate4 As Object
    Private m_vR4Com As Object
    Private m_vMin5 As Object
    Private m_vMax5 As Object
    Private m_vRate5 As Object
    Private m_vR5Com As Object
    Private m_vAllowOveride As Object
    Private m_vMinMTA As Object
    Private m_vMinMTAInstalments As Object
    Private m_vpffrequencyID As Object
    Private m_vTaxChargedTo As Object
    Private m_vFeeType As Object
    Private m_vFeeChargedTo As Object
    Private m_vProtectionType As Object
    Private m_vProtectionChargedTo As Object
    Private m_vDepositType As Object
    Private m_vDepositChargedTo As Object
    Private m_vBackdatedRollupTo As Object
    Private m_vAlignTo As Object
    Private m_vStartLimit As Object
    Private m_vRecollectOnNext As Object
    Private m_vRecollectDays As Object
    Private m_vRetryLimit As Object
    Private m_vMTAOnNextInstalment As Object
    Private m_sSchemeType As String = ""
    Private m_lPFRF_ID As Integer
    Private m_vSchemeName As Object
    Private m_vSchemeDescription As Object
    Private m_lPFRFID As Integer
    Private m_bChanged As Boolean
    Private m_bSuppressDecimalValues As Boolean

    Private m_vFinanceNetCommission As Object
    Private m_iFirstInstalmentAlignWithDayInMonth As Integer
    Private m_iSingleInstalmentPerMonth As Integer


    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPFRF.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030401 : Added the following Variable. Ref. Issue 2915
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private m_vExistingDaysDelay As Object
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030401 : END
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    ' Alix
    Private m_vStatementFrequencyID As Object
    Private m_vStatementDocID As Object
    Private m_vAdvanceInstalments As Object
    Private m_vReviewPMUserGroup As Object
    Private m_vRemainderThreshhold As Object
    Private m_vRemainderAtEnd As Object
    Private m_vMaxInstalments As Object
    Private m_bSpreadCommission As Boolean
    Private m_sUniqueId As String
    Private m_sParentScreenHierarchy As String
    'Developer Guide No 7
    Private Const vbFormCode As Integer = 0
    Private m_bDepositOverrideAllowed As Boolean
    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Private m_bApplyFeePercentagesToPolicyRisk As Boolean = True
    Private m_bApplyFeePercentagesToTaxes As Boolean = True
    Private m_vTransactionType As Object

    Public Property Changed() As Boolean
        Get

            Return m_bChanged

        End Get
        Set(ByVal Value As Boolean)

            m_bChanged = Value

        End Set
    End Property


    Public Property PFRFID() As Integer
        Get

            Return m_lPFRFID

        End Get
        Set(ByVal Value As Integer)

            m_lPFRFID = Value

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
    Public WriteOnly Property CompanyNo() As Integer
        Set(ByVal Value As Integer)

            m_lCompanyNo = Value

        End Set
    End Property
    Public WriteOnly Property SchemeNumber() As Integer
        Set(ByVal Value As Integer)

            m_lSchemeNo = Value

        End Set
    End Property

    Public WriteOnly Property SchemeVersion() As Integer
        Set(ByVal Value As Integer)

            m_lSchemeVersion = Value

        End Set
    End Property

    Public WriteOnly Property StartDate() As Date
        Set(ByVal Value As Date)

            m_dtStartDate = Value

        End Set
    End Property

    Public Property Productfamily() As String
        Get

            Return m_sProductFamily

        End Get
        Set(ByVal Value As String)

            m_sProductFamily = Value

        End Set
    End Property
    Public Property SpreadCommission() As Boolean

        Get
            Return m_bSpreadCommission
        End Get
        Set(ByVal Value As Boolean)
            m_bSpreadCommission = Value
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

    Public Property ParentScreenHierarchy() As String

        Get
            Return m_sParentScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sParentScreenHierarchy = Value
        End Set

    End Property

    Public Property SchemeType() As String
        Get
            Return m_sSchemeType
        End Get
        Set(ByVal Value As String)
            m_sSchemeType = Value
        End Set
    End Property
    ''' <summary>
    ''' Hold the decimal rounding flag.
    ''' </summary>
    ''' <returns></returns>
    Public Property IsSuppressDecimalValues() As Boolean
        Get
            Return m_bSuppressDecimalValues
        End Get
        Set(ByVal Value As Boolean)
            m_bSuppressDecimalValues = Value
        End Set
    End Property

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' Edit History  :
    ' RAM20030401   : Added a new field. Existing Delay Days - Issue 2915
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            Dim lPMMandatory As gPMConstants.PMEMandatoryStatus

            If m_sSchemeType = "TPSG" Then
                lPMMandatory = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            Else
                lPMMandatory = gPMConstants.PMEMandatoryStatus.PMMandatory
            End If

            With m_oFormFields

                m_lReturn = .AddNewFormField(ctlControl:=txtStartDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=lPMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtEndDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=lPMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=cboProductFamily, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lPMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtArrangementFee, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .AddNewFormField(ctlControl:=txtMnemonic, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtDepositPC, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=4)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .AddNewFormField(ctlControl:=txtProtectRate, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .AddNewFormField(ctlControl:=txtMinInterest, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lMandatory:=lPMMandatory, lDecimalPlaces:=6)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtMinMTA, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=lPMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtMinMTAInstalments, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=lPMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtMin1, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtMin2, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtMin3, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtMin4, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtMin5, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtMax1, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtMax2, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtMax3, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtMax4, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtMax5, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtRate1, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=6)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtRate2, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=6)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtRate3, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=6)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtRate4, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=6)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtRate5, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=6)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .AddNewFormField(ctlControl:=txtR1Com, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtR2Com, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtR3Com, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtR4Com, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddNewFormField(ctlControl:=txtR5Com, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtDaysLater, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtRetryLimit, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .AddNewFormField(ctlControl:=txtFirstInstalmentFrom, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=lPMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtFirstInstalmentTo, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=lPMMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=cboFrequency, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lPMMandatory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20030401   : Added a new field. Existing Delay Days
                '                 Ref. Issue 2915  - START
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                m_lReturn = .AddNewFormField(ctlControl:=txtExistingDaysDelay, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20030401   : Ref. Issue 2915  - END
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                ' Alix Bergeret - 07/04/2003
                m_lReturn = .AddNewFormField(ctlControl:=cboStatementFrequ, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=cboStatementDoc, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtAdvanceInst, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=cboReviewUserGrp, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtRemainderThreshhold, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddNewFormField(ctlControl:=txtMaxInstalments, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=lPMMandatory) 'PN12130 Make mandatory
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' /Alix

            End With

            ' {* USER DEFINED CODE (End) *}


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    '
    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    ' Edit History  :
    ' RAM20030402   : Assign m_vExistingDaysDelay to associated textbox
    '                 Issue 2915 Changes.
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

            With m_oFormFields

                m_lReturn = .FormatControl(ctlControl:=txtStartDate, vControlValue:=m_dtStartDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'PSL 18/02/2003 The Data in DB has a space SO Trim
                Select Case m_sProductFamily.Trim()
                    Case "NB", "SG"
                        cboProductFamily.SelectedIndex = 0
                    Case "MTA"
                        cboProductFamily.SelectedIndex = 1
                    Case "REN"
                        cboProductFamily.SelectedIndex = 2
                    Case "TPR"
                        If m_sSchemeType = "CR" Then
                            cboProductFamily.SelectedIndex = 1  ' Third-Party Recovery
                        ElseIf cboProductFamily.Items.Count > 3 Then
                            cboProductFamily.SelectedIndex = 3
                            cboProductFamily.Enabled = False
                        Else
                            cboProductFamily.Enabled = True
                        End If
                    Case "SR"
                        cboProductFamily.SelectedIndex = 0  ' Salvage Recovery
                End Select

                m_lReturn = .FormatControl(ctlControl:=txtEndDate, vControlValue:=m_vEndDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtMnemonic, vControlValue:=m_vMnemonic)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtMinInterest, vControlValue:=m_vMinInterest)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtMin1, vControlValue:=m_vMin1)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtMin2, vControlValue:=m_vMin2)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtMin3, vControlValue:=m_vMin3)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtMin4, vControlValue:=m_vMin4)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtMin5, vControlValue:=m_vMin5)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtMax1, vControlValue:=m_vMax1)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtMax2, vControlValue:=m_vMax2)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtMax3, vControlValue:=m_vMax3)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtMax4, vControlValue:=m_vMax4)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtMax5, vControlValue:=m_vMax5)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtRate1, vControlValue:=m_vRate1)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtRate2, vControlValue:=m_vRate2)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtRate3, vControlValue:=m_vRate3)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtRate4, vControlValue:=m_vRate4)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtRate5, vControlValue:=m_vRate5)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtR1Com, vControlValue:=m_vR1Com)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtR2Com, vControlValue:=m_vR2Com)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtR3Com, vControlValue:=m_vR3Com)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtR4Com, vControlValue:=m_vR4Com)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .FormatControl(ctlControl:=txtR5Com, vControlValue:=m_vR5Com)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Fees
                m_lReturn = .FormatControl(ctlControl:=txtArrangementFee, vControlValue:=m_vArrangementFee)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If CDbl(m_vFeeType) = 1 Then
                    optFeeType(0).Checked = True
                Else
                    optFeeType(1).Checked = True
                End If

                If CDbl(m_vFeeChargedTo) = 1 Then
                    optFeeChargedTo(0).Checked = True
                Else
                    optFeeChargedTo(1).Checked = True
                End If

                'Protection


                If CDbl(m_vProtectionType) = 1 Then
                    optProtectionType(0).Checked = True
                Else
                    optProtectionType(1).Checked = True
                End If

                If CDbl(m_vProtectionChargedTo) = 1 Then
                    optProtectionChargedTo(0).Checked = True
                Else
                    optProtectionChargedTo(1).Checked = True
                End If
                m_lReturn = .FormatControl(ctlControl:=txtProtectRate, vControlValue:=m_vProtectRate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Deposit

                If CDbl(m_vDepositType) = 1 Then
                    optDepositType(0).Checked = True
                Else
                    optDepositType(1).Checked = True
                End If

                If CDbl(m_vDepositChargedTo) = 1 Then
                    optIncluded(0).Checked = True
                Else
                    optIncluded(1).Checked = True
                End If
                m_lReturn = .FormatControl(ctlControl:=txtDepositPC, vControlValue:=m_vDepositPC)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'If CDbl(m_vTaxChargedTo) = 1 Then
                '    optTaxChargedTo(0).Checked = True
                'Else
                '    optTaxChargedTo(1).Checked = True
                'End If

                'Rules

                If CDbl(m_vBackdatedRollupTo) = 1 Then
                    optBackDated(0).Checked = True
                Else
                    optBackDated(1).Checked = True
                End If

                If CDbl(m_vAlignTo) = 1 Then
                    optAlign(0).Checked = True
                    chkSingleInstalmentPerMonth.Enabled = True
                Else
                    optAlign(1).Checked = True
                    chkSingleInstalmentPerMonth.Enabled = False
                End If


                If Not (Convert.IsDBNull(m_vRecollectOnNext) Or IsNothing(m_vRecollectOnNext)) Then
                    chkOnNextInstalmentDate.CheckState = CInt(m_vRecollectOnNext)
                End If




                m_lReturn = .FormatControl(ctlControl:=txtFirstInstalmentTo, vControlValue:=m_vStartLimit)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtFirstInstalmentFrom, vControlValue:=m_vDaysDelay)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .FormatControl(ctlControl:=txtDaysLater, vControlValue:=m_vRecollectDays)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtRetryLimit, vControlValue:=m_vRetryLimit)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtMinMTA, vControlValue:=m_vMinMTA)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .FormatControl(ctlControl:=txtMinMTAInstalments, vControlValue:=m_vMinMTAInstalments)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If Not (Convert.IsDBNull(m_vMTAOnNextInstalment) Or IsNothing(m_vMTAOnNextInstalment)) Then


                    chkAllowMTA.CheckState = CInt(m_vMTAOnNextInstalment)
                End If


                If Not (Convert.IsDBNull(m_vFinanceNetCommission) Or IsNothing(m_vFinanceNetCommission)) Then
                    ChkFinanceNetCommission.CheckState = CInt(m_vFinanceNetCommission)
                End If

                chkFirstInstalmentAlignWithDayInMonth.CheckState = ToSafeInteger(m_iFirstInstalmentAlignWithDayInMonth)
                chkSingleInstalmentPerMonth.CheckState = ToSafeInteger(m_iSingleInstalmentPerMonth)

                For nIndex As Integer = 0 To cboFrequency.Items.Count - 1

                    If VB6.GetItemData(cboFrequency, nIndex) = CDbl(m_vpffrequencyID) Then
                        cboFrequency.SelectedIndex = nIndex
                    End If
                Next nIndex

                '''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20030402 - Issue 2915 Changes - START
                '''''''''''''''''''''''''''''''''''''''''''''''''''''

                If Not (Convert.IsDBNull(m_vExistingDaysDelay) Or IsNothing(m_vExistingDaysDelay)) Then
                    m_lReturn = .FormatControl(ctlControl:=txtExistingDaysDelay, vControlValue:=m_vExistingDaysDelay)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                '''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20030402 - Issue 2915 Changes - END
                '''''''''''''''''''''''''''''''''''''''''''''''''''''

                ' Alix Bergeret - 07/04/2003
                ' 6 news fields for "Recovery by instalments"


                If Not (Convert.IsDBNull(m_vAdvanceInstalments) Or IsNothing(m_vAdvanceInstalments)) Then
                    m_lReturn = .FormatControl(ctlControl:=txtAdvanceInst, vControlValue:=m_vAdvanceInstalments)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If


                If Not (Convert.IsDBNull(m_vRemainderThreshhold) Or IsNothing(m_vRemainderThreshhold)) Then
                    m_lReturn = .FormatControl(ctlControl:=txtRemainderThreshhold, vControlValue:=m_vRemainderThreshhold)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If


                If Not (Convert.IsDBNull(m_vMaxInstalments) Or IsNothing(m_vMaxInstalments)) Then
                    m_lReturn = .FormatControl(ctlControl:=txtMaxInstalments, vControlValue:=m_vMaxInstalments)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If


                If Not (Convert.IsDBNull(m_vRemainderAtEnd) Or IsNothing(m_vRemainderAtEnd)) Then


                    chkRemainderAtEnd.CheckState = CInt(m_vRemainderAtEnd)
                End If

                For nIndex As Integer = 0 To cboStatementFrequ.Items.Count - 1

                    If VB6.GetItemData(cboStatementFrequ, nIndex) = CDbl(m_vStatementFrequencyID) Then
                        cboStatementFrequ.SelectedIndex = nIndex
                    End If
                Next nIndex

                For nIndex As Integer = 0 To cboStatementDoc.Items.Count - 1

                    If VB6.GetItemData(cboStatementDoc, nIndex) = CDbl(m_vStatementDocID) Then
                        cboStatementDoc.SelectedIndex = nIndex
                    End If
                Next nIndex

                For nIndex As Integer = 0 To cboReviewUserGrp.Items.Count - 1

                    If VB6.GetItemData(cboReviewUserGrp, nIndex) = CDbl(m_vReviewPMUserGroup) Then
                        cboReviewUserGrp.SelectedIndex = nIndex
                    End If
                Next nIndex

                ' /Alix
                If m_bDepositOverrideAllowed Then
                    chkDepositOverrideAllowed.CheckState = CheckState.Checked
                Else
                    chkDepositOverrideAllowed.CheckState = CheckState.Unchecked
                End If
                chkApplyFeePercentagesToFees.Checked = m_bApplyFeePercentagesToPolicyRisk
                chkApplyFeePercentagesToTaxes.Checked = m_bApplyFeePercentagesToTaxes

                ' ADO #39993: Set Product Family combo from transaction_type for Claim Recovery
                If m_sSchemeType = "CR" AndAlso Not (Convert.IsDBNull(m_vTransactionType) OrElse IsNothing(m_vTransactionType)) Then
                    Dim iTransType As Integer = CInt(m_vTransactionType)
                    Select Case iTransType
                        Case 1 : cboProductFamily.SelectedIndex = 0  ' Salvage Recovery
                        Case 2 : cboProductFamily.SelectedIndex = 1  ' Third-Party Recovery
                    End Select
                End If
            End With

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
    ' Edit History  :
    ' RAM20030401   : Issue 2915 Changes
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim vParams As Object

        ReDim vParams(ACFirstInstalmentAlignWithDayInMonth)

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Update the business object.

        ' Assign the details from the interface to the data storage.
        m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

        vParams(ACFinanceNetCommission) = m_vFinanceNetCommission
        vParams(ACSingleInstalmentPerMonth) = m_iSingleInstalmentPerMonth
        vParams(ACFirstInstalmentAlignWithDayInMonth) = m_iFirstInstalmentAlignWithDayInMonth

        vParams(ACMaxInstalments) = m_vMaxInstalments

        ' Check for errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to assign the data.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Dim sScreenHierarchy As String = m_sParentScreenHierarchy + " / " + m_sProductFamily + "(" + cboFrequency.Text + ")(" + m_dtStartDate.Date.ToString("dd-MM-yyyy") + "-" + CType(m_vEndDate, Date).ToString("dd-MM-yyyy") + ")"
        ' Check the task.
        Select Case (m_iTask)
            Case gPMConstants.PMEComponentAction.PMAdd

                m_lReturn = m_oBusiness.DirectAdd(m_lPFRFID, v_lCompanyNo:=m_lCompanyNo, v_lSchemeNo:=m_lSchemeNo, v_lSchemeVersion:=m_lSchemeVersion, v_vStartDate:=m_dtStartDate,
                                                  v_vEndDate:=m_vEndDate, v_vProductFamily:=m_sProductFamily, v_vArrangementFee:=m_vArrangementFee, v_vMnemonic:=m_vMnemonic,
                                                  v_vProtectRate:=m_vProtectRate, v_vDaysDelay:=m_vDaysDelay, v_vDepositPC:=m_vDepositPC, v_vMinInterest:=m_vMinInterest,
                                                  v_vMinMTA:=m_vMinMTA, v_vMinMTAInstalments:=m_vMinMTAInstalments, v_vMin1:=m_vMin1, v_vMax1:=m_vMax1, v_vRate1:=m_vRate1,
                                                  v_vR1Com:=m_vR1Com, v_vMin2:=m_vMin2, v_vMax2:=m_vMax2, v_vRate2:=m_vRate2, v_vR2Com:=m_vR2Com, v_vMin3:=m_vMin3, v_vMax3:=m_vMax3,
                                                  v_vRate3:=m_vRate3, v_vR3Com:=m_vR3Com, v_vMin4:=m_vMin4, v_vMax4:=m_vMax4, v_vRate4:=m_vRate4, v_vR4Com:=m_vR4Com, v_vMin5:=m_vMin5, v_vMax5:=m_vMax5,
                                                  v_vRate5:=m_vRate5, v_vR5Com:=m_vR5Com, v_vpffrequencyID:=m_vpffrequencyID, v_vTaxChargedTo:=m_vTaxChargedTo, v_vFeeType:=m_vFeeType, v_vFeeChargedTo:=m_vFeeChargedTo,
                                                  v_vProtectionType:=m_vProtectionType, v_vProtectionChargedTo:=m_vProtectionChargedTo, v_vDepositType:=m_vDepositType, v_vDepositChargedTo:=m_vDepositChargedTo, v_vBackdatedRollupTo:=m_vBackdatedRollupTo,
                                                  v_vAlignTo:=m_vAlignTo, v_vStartLimit:=m_vStartLimit, v_vRecollectOnNext:=m_vRecollectOnNext, v_vRecollectDays:=m_vRecollectDays, v_vRetryLimit:=m_vRetryLimit, v_vMTAOnNextInstalment:=m_vMTAOnNextInstalment,
                                                  v_vExistingDaysDelay:=m_vExistingDaysDelay, v_vStatementFrequID:=m_vStatementFrequencyID, v_vStatementReportID:=m_vStatementDocID, v_vAdvanceInstalments:=m_vAdvanceInstalments, v_vUserGroup:=m_vReviewPMUserGroup,
                                                  v_vRemainderThreshhold:=m_vRemainderThreshhold, v_vRemainderAtEnd:=m_vRemainderAtEnd, vParams:=vParams, bDepositOverrideAllowed:=m_bDepositOverrideAllowed, bApplyFeePercentagesToPolicyRisk:=chkApplyFeePercentagesToFees.Checked,
                                                  bApplyFeePercentagesToTaxes:=chkApplyFeePercentagesToTaxes.Checked, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=sScreenHierarchy, v_vTransactionType:=GetTransactionTypeValue())

            Case gPMConstants.PMEComponentAction.PMEdit
                ' Inform the business object with an updated data item.

                m_lReturn = m_oBusiness.DirectEdit(m_lPFRFID, v_lCompanyNo:=m_lCompanyNo, v_lSchemeNo:=m_lSchemeNo, v_lSchemeVersion:=m_lSchemeVersion, v_vStartDate:=m_dtStartDate, v_vEndDate:=m_vEndDate, v_vProductFamily:=m_sProductFamily, v_vArrangementFee:=m_vArrangementFee, v_vMnemonic:=m_vMnemonic, v_vProtectRate:=m_vProtectRate, v_vDaysDelay:=m_vDaysDelay, v_vDepositPC:=m_vDepositPC, v_vMinInterest:=m_vMinInterest, v_vMinMTA:=m_vMinMTA, v_vMinMTAInstalments:=m_vMinMTAInstalments, v_vMin1:=m_vMin1, v_vMax1:=m_vMax1, v_vRate1:=m_vRate1, v_vR1Com:=m_vR1Com, v_vMin2:=m_vMin2, v_vMax2:=m_vMax2, v_vRate2:=m_vRate2, v_vR2Com:=m_vR2Com, v_vMin3:=m_vMin3, v_vMax3:=m_vMax3, v_vRate3:=m_vRate3, v_vR3Com:=m_vR3Com, v_vMin4:=m_vMin4, v_vMax4:=m_vMax4, v_vRate4:=m_vRate4, v_vR4Com:=m_vR4Com, v_vMin5:=m_vMin5, v_vMax5:=m_vMax5, v_vRate5:=m_vRate5, v_vR5Com:=m_vR5Com, v_vpffrequencyID:=m_vpffrequencyID, v_vTaxChargedTo:=m_vTaxChargedTo, v_vFeeType:=m_vFeeType, v_vFeeChargedTo:=m_vFeeChargedTo, v_vProtectionType:=m_vProtectionType, v_vProtectionChargedTo:=m_vProtectionChargedTo, v_vDepositType:=m_vDepositType, v_vDepositChargedTo:=m_vDepositChargedTo, v_vBackdatedRollupTo:=m_vBackdatedRollupTo, v_vAlignTo:=m_vAlignTo, v_vStartLimit:=m_vStartLimit, v_vRecollectOnNext:=m_vRecollectOnNext, v_vRecollectDays:=m_vRecollectDays, v_vRetryLimit:=m_vRetryLimit, v_vMTAOnNextInstalment:=m_vMTAOnNextInstalment, v_vExistingDaysDelay:=m_vExistingDaysDelay, v_vStatementFrequID:=m_vStatementFrequencyID, v_vStatementReportID:=m_vStatementDocID, v_vAdvanceInstalments:=m_vAdvanceInstalments, v_vUserGroup:=m_vReviewPMUserGroup, v_vRemainderThreshhold:=m_vRemainderThreshhold, v_vRemainderAtEnd:=m_vRemainderAtEnd, vParams:=vParams, bDepositOverrideAllowed:=m_bDepositOverrideAllowed, bApplyFeePercentagesToPolicyRisk:=chkApplyFeePercentagesToFees.Checked, bApplyFeePercentagesToTaxes:=chkApplyFeePercentagesToTaxes.Checked, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=sScreenHierarchy, v_vTransactionType:=GetTransactionTypeValue())

        End Select

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
        End If

        Return result


        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function


    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    Private Sub FormLogic()

        If cboProductFamily.SelectedIndex >= 0 Then
            fraMTA.Visible = (VB6.GetItemString(cboProductFamily, cboProductFamily.SelectedIndex) = "MTA") And (m_sSchemeType = "IH")
        End If
        If m_bSpreadCommission = True Then
            ChkFinanceNetCommission.Checked = False
            ChkFinanceNetCommission.Enabled = False
        Else
            ChkFinanceNetCommission.Enabled = True
        End If
        'DD 10/07/2003: Third Party Recovery
        If m_sSchemeType = "TPSG" Then


            For i As Integer = 0 To ContainerHelper.Controls(Me).Count - 1

                If Convert.ToString(ContainerHelper.Controls(Me)(i).Tag) = "HIDESG;" Then

                    ContainerHelper.Controls(Me)(i).Visible = False
                End If
            Next i

            lblEndDate.Enabled = False
            txtEndDate.Enabled = False
            lblMnemonic.Enabled = False
            txtMnemonic.Enabled = False

            cmdNext(0).Visible = False
            SSTabHelper.SetTabVisible(tabMainTab, 1, False)
            SSTabHelper.SetTabVisible(tabMainTab, 2, False)
            SSTabHelper.SetTabVisible(tabMainTab, 3, False)

        ElseIf m_sSchemeType = "TPR" Then
            optBackDated(0).Checked = True
            optBackDated(0).Enabled = False
            optBackDated(1).Enabled = False
            fraBackDated.Enabled = False

            optAlign(0).Checked = True
            optAlign(0).Enabled = False
            optAlign(1).Enabled = False
            fraAlign.Enabled = False

            framRecollection.Enabled = False
            chkOnNextInstalmentDate.Enabled = False
            txtDaysLater.Enabled = False
            txtRetryLimit.Enabled = False
        End If

        m_lReturn = CType(EnableDisableControls(SSTabHelper.GetSelectedIndex(tabMainTab)), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub BuildProductFamilyCombo()
        Select Case m_sSchemeType
            Case "TP", "IH"
                'Third Party and In-House
                Dim cboProductFamily_NewIndex As Integer = -1
                'Developer Guide No 153
                cboProductFamily_NewIndex = cboProductFamily.Items.Add(New VB6.ListBoxItem("New Business", 0))
                m_iProducts(0) = "NB"
                'Developer Guide No 153
                cboProductFamily_NewIndex = cboProductFamily.Items.Add(New VB6.ListBoxItem("MTA", 1))
                m_iProducts(1) = "MTA"

                'Developer Guide No 153
                cboProductFamily_NewIndex = cboProductFamily.Items.Add(New VB6.ListBoxItem("Renewal", 2))
                m_iProducts(2) = "REN"
            Case "TPSG"
                'Stargate. No rates applicable
                'Developer Guide No 153
                Dim cboProductFamily_NewIndex As Integer = -1
                cboProductFamily_NewIndex = cboProductFamily.Items.Add(New VB6.ListBoxItem("Stargate", 0))

                m_iProducts(0) = "SG"
            Case "TPR"
                'For future expansion
                'Developer Guide No 153
                Dim cboProductFamily_NewIndex As Integer = -1
                cboProductFamily_NewIndex = cboProductFamily.Items.Add(New VB6.ListBoxItem("TPR", 0))
                m_iProducts(0) = "TPR"

            Case "CR"
                'Claim Recovery - Salvage and Third-Party transaction types
                Dim cboProductFamily_NewIndex As Integer = -1
                cboProductFamily_NewIndex = cboProductFamily.Items.Add(New VB6.ListBoxItem("Salvage Recovery", 0))
                m_iProducts(0) = "SR"
                cboProductFamily_NewIndex = cboProductFamily.Items.Add(New VB6.ListBoxItem("Third-Party Recovery", 1))
                m_iProducts(1) = "TPR"
        End Select
    End Sub

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' Edit History  :
    ' RAM20030402   : Added r_vExistingDaysDelay Optional Parameter
    '                   Issue 2915 Changes
    ' Alix Bergeret - 07/04/2003
    ' Retreive data in an array rather than loads of parameters
    ' Also retreiving 6 more values for "Recovery by instalments"
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            If m_lRecordNo = 0 Then
                m_lRecordNo = 1
            End If

            ' RAM20030402 : Added r_vExistingDaysDelay Parameter

            m_lReturn = m_oBusiness.GetDetails(v_lPFRF_ID:=m_lPFRFID, r_vResultArray:=vResultArray)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            ' Assign values to local variables

            m_lCompanyNo = CInt(vResultArray(0, 0))

            m_lSchemeNo = CInt(vResultArray(1, 0))

            m_lSchemeVersion = CInt(vResultArray(2, 0))

            m_dtStartDate = CDate(vResultArray(3, 0))

            m_sProductFamily = CStr(vResultArray(4, 0))


            m_vArrangementFee = vResultArray(5, 0)


            m_vMnemonic = vResultArray(6, 0)


            m_vEndDate = vResultArray(7, 0)


            m_vDaysDelay = vResultArray(9, 0)


            m_vDepositReq = vResultArray(10, 0)


            m_vDepositPC = vResultArray(11, 0)


            m_vProtectRate = vResultArray(13, 0)


            m_vMinInterest = vResultArray(14, 0)


            m_vMin1 = vResultArray(15, 0)


            m_vMax1 = vResultArray(16, 0)


            m_vRate1 = vResultArray(17, 0)


            m_vR1Com = vResultArray(18, 0)


            m_vMin2 = vResultArray(19, 0)


            m_vMax2 = vResultArray(20, 0)


            m_vRate2 = vResultArray(21, 0)


            m_vR2Com = vResultArray(22, 0)


            m_vMin3 = vResultArray(23, 0)


            m_vMax3 = vResultArray(24, 0)


            m_vRate3 = vResultArray(25, 0)


            m_vR3Com = vResultArray(26, 0)


            m_vMin4 = vResultArray(27, 0)


            m_vMax4 = vResultArray(28, 0)


            m_vRate4 = vResultArray(29, 0)


            m_vR4Com = vResultArray(30, 0)


            m_vMin5 = vResultArray(31, 0)


            m_vMax5 = vResultArray(32, 0)


            m_vRate5 = vResultArray(33, 0)


            m_vR5Com = vResultArray(34, 0)


            m_vAllowOveride = vResultArray(35, 0)


            m_vMinMTA = vResultArray(36, 0)


            m_vMinMTAInstalments = vResultArray(37, 0)


            m_vpffrequencyID = vResultArray(39, 0)


            m_vTaxChargedTo = vResultArray(40, 0)


            m_vFeeType = vResultArray(41, 0)


            m_vFeeChargedTo = vResultArray(42, 0)


            m_vProtectionType = vResultArray(43, 0)


            m_vProtectionChargedTo = vResultArray(44, 0)


            m_vDepositType = vResultArray(45, 0)


            m_vDepositChargedTo = vResultArray(46, 0)


            m_vBackdatedRollupTo = vResultArray(47, 0)


            m_vAlignTo = vResultArray(48, 0)


            m_vStartLimit = vResultArray(49, 0)


            m_vRecollectOnNext = vResultArray(50, 0)


            m_vRecollectDays = vResultArray(51, 0)


            m_vRetryLimit = vResultArray(52, 0)


            m_vMTAOnNextInstalment = vResultArray(53, 0)


            m_vExistingDaysDelay = vResultArray(54, 0)

            ' Added for recovery by instalments


            m_vStatementFrequencyID = vResultArray(55, 0)


            m_vStatementDocID = vResultArray(56, 0)


            m_vAdvanceInstalments = vResultArray(57, 0)


            m_vReviewPMUserGroup = vResultArray(58, 0)


            m_vRemainderThreshhold = vResultArray(59, 0)


            m_vRemainderAtEnd = vResultArray(60, 0)


            m_vMaxInstalments = vResultArray(61, 0)

            m_bDepositOverrideAllowed = vResultArray(65, 0)

            m_vFinanceNetCommission = vResultArray(ACFinanceNetCommission, 0)
            m_iSingleInstalmentPerMonth = vResultArray(ACSingleInstalmentPerMonth, 0)
            m_iFirstInstalmentAlignWithDayInMonth = vResultArray(ACFirstInstalmentAlignWithDayInMonth, 0)

            m_bApplyFeePercentagesToPolicyRisk = gPMFunctions.ToSafeBoolean(vResultArray(ACApplyFeePercentagesToPolicyRisk, 0))
            m_bApplyFeePercentagesToTaxes = gPMFunctions.ToSafeBoolean(vResultArray(ACApplyFeePercentagesToTaxes, 0))

            ' ADO #39993: Read transaction_type for Claim Recovery
            If vResultArray.GetUpperBound(0) >= ACTransactionType Then
                m_vTransactionType = vResultArray(ACTransactionType, 0)
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
    ' Edit History  :
    ' RAM20030401   : Issue 2915 Changes
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            m_dtStartDate = CDate(txtStartDate.Text)

            m_vEndDate = CDate(txtEndDate.Text)
            m_sProductFamily = m_iProducts(cboProductFamily.SelectedIndex)



            m_vArrangementFee = m_oFormFields.UnformatControl(txtArrangementFee)

            m_vMnemonic = txtMnemonic.Text.Trim()
            If cboFrequency.SelectedIndex = -1 Then

                m_vpffrequencyID = 0
            Else

                m_vpffrequencyID = VB6.GetItemData(cboFrequency, cboFrequency.SelectedIndex)
            End If

            If CDbl(m_vpffrequencyID) = 0 Then


                m_vpffrequencyID = DBNull.Value
            End If


            m_vMinInterest = m_oFormFields.UnformatControl(txtMinInterest)


            m_vMin1 = m_oFormFields.UnformatControl(txtMin1)


            m_vMin2 = m_oFormFields.UnformatControl(txtMin2)


            m_vMin3 = m_oFormFields.UnformatControl(txtMin3)


            m_vMin4 = m_oFormFields.UnformatControl(txtMin4)


            m_vMin5 = m_oFormFields.UnformatControl(txtMin5)


            m_vMax1 = m_oFormFields.UnformatControl(txtMax1)


            m_vMax2 = m_oFormFields.UnformatControl(txtMax2)


            m_vMax3 = m_oFormFields.UnformatControl(txtMax3)


            m_vMax4 = m_oFormFields.UnformatControl(txtMax4)


            m_vMax5 = m_oFormFields.UnformatControl(txtMax5)


            m_vRate1 = m_oFormFields.UnformatControl(txtRate1)


            m_vRate2 = m_oFormFields.UnformatControl(txtRate2)


            m_vRate3 = m_oFormFields.UnformatControl(txtRate3)


            m_vRate4 = m_oFormFields.UnformatControl(txtRate4)


            m_vRate5 = m_oFormFields.UnformatControl(txtRate5)


            m_vR1Com = m_oFormFields.UnformatControl(txtR1Com)


            m_vR2Com = m_oFormFields.UnformatControl(txtR2Com)


            m_vR3Com = m_oFormFields.UnformatControl(txtR3Com)


            m_vR4Com = m_oFormFields.UnformatControl(txtR4Com)


            m_vR5Com = m_oFormFields.UnformatControl(txtR5Com)


            'Protection


            m_vProtectionType = IIf(optProtectionType(0).Checked, 1, 0)

            m_vProtectionChargedTo = IIf(optProtectionChargedTo(0).Checked, 1, 0)


            m_vProtectRate = m_oFormFields.UnformatControl(txtProtectRate)

            'Deposit

            m_vDepositType = IIf(optDepositType(0).Checked, 1, 0)

            m_vDepositChargedTo = IIf(optIncluded(0).Checked, 1, 0)


            m_vDepositPC = m_oFormFields.UnformatControl(txtDepositPC)

            'Tax

            'm_vTaxChargedTo = IIf(optTaxChargedTo(0).Checked, 1, 0)

            'Arrangment fee

            m_vFeeType = IIf(optFeeType(0).Checked, 1, 0)

            m_vFeeChargedTo = IIf(optFeeChargedTo(0).Checked, 1, 0)


            'Rules

            m_vBackdatedRollupTo = IIf(optBackDated(0).Checked, 1, 0)

            m_vAlignTo = IIf(optAlign(0).Checked, 1, 0)

            m_vRecollectOnNext = chkOnNextInstalmentDate.CheckState


            m_vDaysDelay = m_oFormFields.UnformatControl(txtFirstInstalmentFrom)


            m_vStartLimit = m_oFormFields.UnformatControl(txtFirstInstalmentTo)


            m_vRecollectDays = m_oFormFields.UnformatControl(txtDaysLater)


            m_vRetryLimit = m_oFormFields.UnformatControl(txtRetryLimit)


            m_vMinMTA = m_oFormFields.UnformatControl(txtMinMTA)


            m_vMinMTAInstalments = m_oFormFields.UnformatControl(txtMinMTAInstalments)

            m_vMTAOnNextInstalment = chkAllowMTA.CheckState

            m_vFinanceNetCommission = ChkFinanceNetCommission.CheckState

            m_iSingleInstalmentPerMonth = chkSingleInstalmentPerMonth.CheckState
            m_iFirstInstalmentAlignWithDayInMonth = chkFirstInstalmentAlignWithDayInMonth.CheckState

            m_vExistingDaysDelay = m_oFormFields.UnformatControl(txtExistingDaysDelay)

            m_vAdvanceInstalments = m_oFormFields.UnformatControl(txtAdvanceInst)


            m_vRemainderThreshhold = m_oFormFields.UnformatControl(txtRemainderThreshhold)

            m_vMaxInstalments = m_oFormFields.UnformatControl(txtMaxInstalments)


            'PN12130 - revised DD 01/06/2004 (zero is valid)

            If CDbl(m_vMaxInstalments) < 0 Then

                m_vMaxInstalments = 0
            End If

            If cboStatementFrequ.SelectedIndex = -1 Then

                m_vStatementFrequencyID = 0
            Else

                m_vStatementFrequencyID = VB6.GetItemData(cboStatementFrequ, cboStatementFrequ.SelectedIndex)
            End If
            If cboStatementDoc.SelectedIndex = -1 Then

                m_vStatementDocID = 0
            Else

                m_vStatementDocID = VB6.GetItemData(cboStatementDoc, cboStatementDoc.SelectedIndex)
            End If
            If cboReviewUserGrp.SelectedIndex = -1 Then

                m_vReviewPMUserGroup = 0
            Else

                m_vReviewPMUserGroup = VB6.GetItemData(cboReviewUserGrp, cboReviewUserGrp.SelectedIndex)
            End If

            m_vRemainderAtEnd = chkRemainderAtEnd.CheckState

            If CDbl(m_vStatementFrequencyID) = 0 Then


                m_vStatementFrequencyID = DBNull.Value
            End If

            If CDbl(m_vStatementDocID) = 0 Then


                m_vStatementDocID = DBNull.Value
            End If

            If CDbl(m_vReviewPMUserGroup) = 0 Then


                m_vReviewPMUserGroup = DBNull.Value
            End If

            m_bDepositOverrideAllowed = chkDepositOverrideAllowed.CheckState
            ' /Alix

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
        Dim vUnderwriting As String = ""
        Dim bIsUnderwriting As Boolean

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
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = CType(FormatBlankFields(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Format blank fields", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                'Setting the default values... PN 17445
                optAlign(0).Checked = True
                optBackDated(1).Checked = True
                optFeeType(1).Checked = True
                optProtectionType(1).Checked = True
                optDepositType(1).Checked = True
                'optTaxChargedTo(1).Checked = True
                optFeeChargedTo(1).Checked = True
                optProtectionChargedTo(1).Checked = True
                optIncluded(1).Checked = True

            End If

            ' Alix Bergeret - 08/04/2003
            ' Only display "third party recovery" tab is scheme type is "TPR"
            If m_sSchemeType <> "TPR" Then
                cmdNext(2).Visible = False
                SSTabHelper.SetTabVisible(tabMainTab, 3, False)
            End If

            'DD 05/06/2003: Recollection is not applicable for Third Party (PF) scheme
            'PN18343
            framRecollection.Visible = m_sSchemeType = "IH"

            'Tax is visible for Underwriting
            iPMFunc.getUnderwritingOrAgency(vUnderwriting)
            bIsUnderwriting = (gPMFunctions.ToSafeString(vUnderwriting) = "U")

            'lblTax.Visible = bIsUnderwriting
            'optTaxChargedTo(0).Visible = bIsUnderwriting
            'optTaxChargedTo(1).Visible = bIsUnderwriting

            'Force first tab to display
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            'Get the Decimal Suppression flag
            Dim sTempOptionValue As String = ""
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableDecimalsSuppression, v_vBranch:=1, r_vUnderwriting:=sTempOptionValue)

            If Trim(sTempOptionValue) = "1" Then
                IsSuppressDecimalValues = True
            End If
            chkApplyFeePercentagesToFees.Checked = True
            chkApplyFeePercentagesToTaxes.Checked = True

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
            ReDim m_ctlTabFirstLast(1, 3)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            m_ctlTabFirstLast(ACControlStart, 0) = txtStartDate
            m_ctlTabFirstLast(ACControlEnd, 0) = optIncluded(1)
            m_ctlTabFirstLast(ACControlStart, 1) = cboFrequency
            m_ctlTabFirstLast(ACControlEnd, 1) = txtMinInterest
            m_ctlTabFirstLast(ACControlStart, 2) = optBackDated(0)
            m_ctlTabFirstLast(ACControlEnd, 2) = txtMinMTAInstalments
            m_ctlTabFirstLast(ACControlStart, 3) = cboStatementFrequ
            m_ctlTabFirstLast(ACControlEnd, 3) = chkRemainderAtEnd


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


            'Developer Guide No 243
            'Starts
            '         Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

            '' Check for an error.
            'If Me.Text = "" Then
            '	' Failed to get data from the resource file.
            '	result = gPMConstants.PMEReturnCode.PMFalse

            '	' Log Error.
            '	iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
            '	                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

            '	Return result
            'End If


            'cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

            ''lblProductFamily.Caption = GetResData( _
            ''iLangID:=g_iLanguageID%, _
            ''lID:=ACProductFamily, _
            ''iDataType:=PMResString)

            '' {* USER DEFINED CODE (Begin) *}

            '' ************************************************************
            '' Enter your code here to display all language specific
            '' captions.
            '' The GetResData function will allow you to do this.
            ''
            '' Example:-
            ''
            ''    lblDesc.Caption = GetResData( _
            ' ''        iLangID:=g_iLanguageID%, _
            ' ''        lID:=ACDesc, _
            ' ''        iDataType:=PMResString)
            ''
            '' NOTE: Replace this section with your new code.
            '' ************************************************************

            '' {* USER DEFINED CODE (End) *}

            '' Alix Bergeret - 07/04/2003

            'SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'lblStatementFrequ.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblStatFrequ, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'lblStatementDoc.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblStatDoc, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'lblAdvanceInst.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblAdvance, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'lblReviewUserGrp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblUserGroup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'lblRemainderThreshhold.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblThreshhold, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'chkRemainderAtEnd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblAtEnd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'lblMaxInstalments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblMaxInst, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
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


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'lblProductFamily.Caption = GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACProductFamily, _
            'iDataType:=PMResString)

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            ' Alix Bergeret - 07/04/2003

            SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblStatementFrequ.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblStatFrequ, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblStatementDoc.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblStatDoc, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAdvanceInst.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblAdvance, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblReviewUserGrp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblUserGroup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRemainderThreshhold.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblThreshhold, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkRemainderAtEnd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblAtEnd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblMaxInstalments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblMaxInst, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ' /Alix
            'Ends
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)


    Private isInitializingComponent As Boolean
    Private Sub cboProductFamily_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProductFamily.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub


    Private Sub cboReviewUserGrp_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles cboReviewUserGrp.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        KeyAscii = 0
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub cboStatementDoc_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles cboStatementDoc.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        KeyAscii = 0
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub cboStatementFrequ_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles cboStatementFrequ.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        KeyAscii = 0
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        'Developer Guide No. 184(latest guide)
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(objCnt:=cmdHelp, lContextID:=ScreenHelpID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        End If

    End Sub

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_2.Click, _cmdNext_1.Click, _cmdNext_0.Click

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

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_2.Click, _cmdPrevious_1.Click, _cmdPrevious_0.Click

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

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPFRF.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBPFRF.General()

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

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            BuildProductFamilyCombo()
            GetFrequencies()
            ' Alix Bergeret - 07/04/2003
            GetReports()
            GetUserGroups()
            ' /Alix

            If Task = gPMConstants.PMEComponentAction.PMAdd Then
                txtFirstInstalmentFrom.Text = CStr(bSIRPremFinConst.PFDirectDebitDelay)
                cboProductFamily.SelectedIndex = 0
                txtMinMTA.Text = CStr(0)
                txtMinMTAInstalments.Text = CStr(0)
                ' Alix - if thirs party recovery then force rate type to TPR
                If m_sSchemeType = "TPR" Then
                    cboProductFamily.Enabled = False
                    cboProductFamily.SelectedIndex = 3
                End If
            Else
                txtStartDate.Enabled = False
                cboProductFamily.Enabled = False
            End If

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
                MessageBox.Show("Failed to get data from the business object", Application.ProductName)
                ' Failed to get the interface details.


                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'eck310102
            'CHECK_PSL
            '    If m_vSchemeType > 1 Then '(In House)
            '        txtStartDate.Enabled = False
            '        txtEndDate.Enabled = False
            '        txtMnemonic.Enabled = False
            '        cboProductFamily.Enabled = False
            '        txtFirstInstalmentFrom.Enabled = False
            '    End If
            'eck310102

            FormLogic()

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

                '	' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    '		' Do not procced with the interface termination.
                    Cancel = 1
                    'Developer Guide No 7
                    eventArgs.Cancel = True
                    '		' Set the mouse pointer to normal.
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

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Put check for max value permitted by Integer data type
            If Conversion.Val(txtMaxInstalments.Text) > 32767 Then
                MessageBox.Show("The maximum instalments entered exceeds the maximum value permitted for this field." & _
                                " Please re-enter.", "Maximum Instalments", MessageBoxButtons.OK, MessageBoxIcon.Error)

                SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                txtMaxInstalments.Focus()
                Exit Sub
            End If

            ' Put check for max value permitted by int data type
            If Conversion.Val(txtFirstInstalmentFrom.Text) > 32767 Then
                MessageBox.Show("The First Installment From value entered exceeds the maximum value permitted for this field." & _
                                " Please re-enter.", "First Instalment Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error)
                SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                txtFirstInstalmentFrom.Focus()
                Exit Sub
            End If

            ' Put check for max value permitted by int data type
            If Conversion.Val(txtFirstInstalmentTo.Text) > 32767 Then
                MessageBox.Show("The First Installment To value entered exceeds the maximum value permitted for this field." & _
                                " Please re-enter.", "First Instalment Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error)

                SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                txtFirstInstalmentTo.Focus()
                Exit Sub
            End If

            ' Put check for max value permitted by int data type
            If Conversion.Val(txtExistingDaysDelay.Text) > 32767 Then
                MessageBox.Show("The Days Delay value entered exceeds the maximum value permitted for this field." & _
                                " Please re-enter.", "First Instalment Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error)

                SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                txtExistingDaysDelay.Focus()
                Exit Sub
            End If

            ' Put check for max value permitted by Integer data type
            If Conversion.Val(txtDaysLater.Text) > 32767 Then
                MessageBox.Show("The Days Later value entered exceeds the maximum value permitted for this field." & _
                                " Please re-enter.", "Recollection of Failed Instalment", MessageBoxButtons.OK, MessageBoxIcon.Error)

                SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                txtDaysLater.Focus()
                Exit Sub
            End If

            ' Put check for max value permitted by Integer data type
            If Conversion.Val(txtRetryLimit.Text) > 32767 Then
                MessageBox.Show("The Retry Limit value entered exceeds the maximum value permitted for this field." & _
                                " Please re-enter.", "Recollection of First Instalment", MessageBoxButtons.OK, MessageBoxIcon.Error)

                SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                txtRetryLimit.Focus()
                Exit Sub
            End If

            If txtDepositPC.Text.Trim = "" Then
                txtDepositPC.Text = "0.0"
            End If
            Dim dDepositPC As Double = m_oFormFields.UnformatControl(txtDepositPC)
            If (_optDepositType_0.Checked AndAlso (dDepositPC < 0 Or dDepositPC > 100)) Or (_optDepositType_1.Checked AndAlso dDepositPC < 0) Then
                MessageBox.Show("Please enter a valid value for deposit override.", "Deposit Override Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtDepositPC.Focus()
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
    ' PRIVATE Events (End)





    Private Sub optDepositType_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optDepositType_0.CheckedChanged, _optDepositType_1.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            Dim Index As Integer = Array.IndexOf(optDepositType, eventSender)

            Dim vFieldValue As Object
            If m_bSuppressDecimalValues AndAlso _optDepositType_1.Checked Then txtDepositPC.Text = 0
            Try

                With m_oFormFields
                    ' Look for the right field
                    For iIndex As Integer = 1 To .Count()
                        ' We found it
                        'Developer Guide No 98
                        If .Item(iIndex).Caption = "Deposit" Then
                            ' Save its value as the format change seems to lose it


                            'Developer Guide No 98
                            vFieldValue = .Item(iIndex).FieldValue
                            If Index = 0 Then
                                ' Change format to %age
                                ' Developer Guide No. 24
                                'Developer Guide No 98
                                .Item(iIndex).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
                            Else
                                ' Change format to Currency
                                'Developer Guide No. 24
                                'Developer Guide No 98
                                .Item(iIndex).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
                            End If
                            ' Refresh control
                            .FormatControl(txtDepositPC, vFieldValue)
                            Exit For
                        End If
                    Next iIndex
                End With

                Exit Sub

            Catch
            End Try



        End If
    End Sub

    Private Sub optFeeType_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optFeeType_1.CheckedChanged, _optFeeType_0.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            Dim Index As Integer = Array.IndexOf(optFeeType, eventSender)

            Dim vFieldValue As Object

            ' Remove decimal part from the value if Enable Suppress Decimal hidden option is ON as well as Amount option is checked
            If m_bSuppressDecimalValues AndAlso _optFeeType_1.Checked Then
                txtArrangementFee.Text = txtArrangementFee.Text.Replace("%", "")
                txtArrangementFee.Text = gPMFunctions.ToSafeRound(txtArrangementFee.Text, 0, True)
            End If


            Try

                With m_oFormFields
                    ' Look for the right field
                    For iIndex As Integer = 1 To .Count()
                        ' We found it
                        'Developer Guide No 98
                        If .Item(iIndex).Caption = "Arrangement Fee" Then
                            ' Save its value as the format change seems to lose it


                            'Developer Guide No 98
                            vFieldValue = .Item(iIndex).FieldValue
                            If Index = 0 Then
                                ' Change format to %age
                                'Developer Guide No. 24
                                'Developer Guide No 98
                                .Item(iIndex).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
                            Else
                                ' Change format to Currency
                                ' Developer Guide No. 24
                                'Developer Guide No 98
                                .Item(iIndex).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
                            End If
                            ' Refresh control
                            .FormatControl(txtArrangementFee, vFieldValue)
                            Exit For
                        End If
                    Next iIndex
                End With

                Exit Sub

            Catch
            End Try



        End If
    End Sub

    Private Sub optProtectionType_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optProtectionType_1.CheckedChanged, _optProtectionType_0.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            Dim Index As Integer = Array.IndexOf(optProtectionType, eventSender)
            If m_bSuppressDecimalValues AndAlso _optProtectionType_1.Checked Then txtProtectRate.Text = 0
            Dim vFieldValue As Object

            Try

                With m_oFormFields
                    ' Look for the right field
                    For iIndex As Integer = 1 To .Count()
                        ' We found it
                        'Developer Guide No 98
                        If .Item(iIndex).Caption = "Protection" Then
                            ' Save its value as the format change seems to lose it


                            'Developer Guide No 98
                            vFieldValue = .Item(iIndex).FieldValue
                            If Index = 0 Then
                                ' Change format to %age
                                ' Developer Guide No. 24
                                'Developer Guide No 98
                                .Item(iIndex).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
                            Else
                                ' Change format to Currency
                                ' Developer Guide No. 24
                                'Developer Guide No 98
                                .Item(iIndex).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
                            End If
                            ' Refresh control
                            .FormatControl(txtProtectRate, vFieldValue)
                            Exit For
                        End If
                    Next iIndex
                End With

                Exit Sub

            Catch
            End Try



        End If
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        m_lReturn = CType(EnableDisableControls(SSTabHelper.GetSelectedIndex(tabMainTab)), gPMConstants.PMEReturnCode)

        tabMainTabPreviousTab = tabMainTab.SelectedIndex
    End Sub

    Private Sub txtAdvanceInst_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAdvanceInst.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtAdvanceInst_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAdvanceInst.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAdvanceInst)
    End Sub

    Private Sub txtAdvanceInst_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAdvanceInst.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAdvanceInst)
    End Sub

    Private Sub txtArrangementFee_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtArrangementFee.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtArrangementFee_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtArrangementFee.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtArrangementFee)
        txtArrangementFee.MaxLength = 15
    End Sub

    Private Sub txtArrangementFee_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtArrangementFee.Leave
        If txtArrangementFee.Text.Trim() = "" Then
            txtArrangementFee.MaxLength = 15
        Else
            txtArrangementFee.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtArrangementFee)
            txtArrangementFee.MaxLength = txtArrangementFee.Text.Trim().Length
        End If

    End Sub



    Private Sub txtDaysLater_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDaysLater.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDaysLater)

        txtDaysLater.MaxLength = 5

    End Sub

    Private Sub txtFirstInstalmentFrom_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFirstInstalmentFrom.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtFirstInstalmentFrom_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFirstInstalmentFrom.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFirstInstalmentFrom)

        ' Set the max length to 5 as this will hold int
        txtFirstInstalmentFrom.MaxLength = 5

    End Sub

    Private Sub txtFirstInstalmentTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFirstInstalmentTo.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFirstInstalmentTo)

        txtFirstInstalmentTo.MaxLength = 5
    End Sub

    Private Sub txtFirstInstalmentTo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFirstInstalmentTo.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFirstInstalmentTo)

        If Strings.Len(txtFirstInstalmentTo.Text) > 0 Then
            Dim dbNumericTemp As Double
            If Double.TryParse(txtFirstInstalmentTo.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                If Conversion.Val(txtFirstInstalmentTo.Text) >= 0 Then

                    ' Valid entry check that is not less than instalment from
                    If Conversion.Val(txtFirstInstalmentTo.Text.Trim()) < Conversion.Val(txtFirstInstalmentFrom.Text.Trim()) Then
                        MessageBox.Show("Days delay cannot be less than " & (txtFirstInstalmentFrom.Text) & " working days.", "Days Delay ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) 'PN 62189
                        SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                        txtFirstInstalmentTo.Focus()
                    End If
                End If
            Else
                MessageBox.Show("Days delay must be numeric.", "Days Delay is NOT numeric", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                txtFirstInstalmentTo.Focus()
            End If
        End If
    End Sub

    Private Sub txtMaxInstalments_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMaxInstalments.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMaxInstalments_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMaxInstalments.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMaxInstalments)

        ' Set the max length to 5 as this will hold an int
        txtMaxInstalments.MaxLength = 5
    End Sub
    Private Sub txtMaxInstalments_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMaxInstalments.Leave
        'PN12130 - revised by DD 01/06/2004
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMaxInstalments)
        Dim dbNumericTemp As Double
        If Double.TryParse(txtMaxInstalments.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            If gPMFunctions.ToSafeLong(txtMaxInstalments) < 0 Then
                MessageBox.Show("Please enter the maximum instalments." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                "A value of zero means that the number is dynamically calculated.", "Maximum Instalments", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtMaxInstalments.Focus()
            End If
        End If
    End Sub

    Private Sub txtR1Com_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR1Com.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtR1Com_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR1Com.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtR1Com)
        txtR1Com.MaxLength = 15
    End Sub

    Private Sub txtR1Com_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR1Com.Leave

        If txtR1Com.Text.Trim() = "" Then
            txtR1Com.MaxLength = 15
        Else
            txtR1Com.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtR1Com)
            txtR1Com.MaxLength = txtR1Com.Text.Trim().Length
        End If

    End Sub

    Private Sub txtR2Com_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR2Com.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtR2Com_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR2Com.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtR2Com)
        txtR2Com.MaxLength = 15
    End Sub

    Private Sub txtR2Com_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR2Com.Leave

        If txtR2Com.Text.Trim() = "" Then
            txtR2Com.MaxLength = 15
        Else
            txtR2Com.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtR2Com)
            txtR2Com.MaxLength = txtR2Com.Text.Trim().Length
        End If

    End Sub

    Private Sub txtR3Com_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR3Com.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtR3Com_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR3Com.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtR3Com)
        txtR3Com.MaxLength = 15
    End Sub

    Private Sub txtR3Com_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR3Com.Leave

        If txtR3Com.Text.Trim() = "" Then
            txtR3Com.MaxLength = 15
        Else
            txtR3Com.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtR3Com)
            txtR3Com.MaxLength = txtR3Com.Text.Trim().Length
        End If

    End Sub

    Private Sub txtR4Com_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR4Com.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtR4Com_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR4Com.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtR4Com)
        txtR4Com.MaxLength = 15
    End Sub

    Private Sub txtR4Com_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR4Com.Leave

        If txtR4Com.Text.Trim() = "" Then
            txtR4Com.MaxLength = 15
        Else
            txtR4Com.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtR4Com)
            txtR4Com.MaxLength = txtR4Com.Text.Trim().Length
        End If

    End Sub

    Private Sub txtR5Com_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR5Com.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtR5Com_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR5Com.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtR5Com)
        txtR5Com.MaxLength = 15
    End Sub

    Private Sub txtR5Com_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtR5Com.Leave

        If txtR5Com.Text.Trim() = "" Then
            txtR5Com.MaxLength = 15
        Else
            txtR5Com.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtR5Com)
            txtR5Com.MaxLength = txtR5Com.Text.Trim().Length
        End If

    End Sub

    Private Sub txtFirstInstalmentFrom_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFirstInstalmentFrom.Leave
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFirstInstalmentFrom)

        txtFirstInstalmentFrom.Text = txtFirstInstalmentFrom.Text.Trim()

        Dim dbNumericTemp As Double
        If Strings.Len(txtFirstInstalmentFrom.Text) > 0 And Double.TryParse(txtFirstInstalmentFrom.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And Conversion.Val(txtFirstInstalmentFrom.Text) >= 0 Then
            ' Valid entry so do nothing
        Else
            MessageBox.Show("You must enter a numeric value. ", "Days Delay is NOT numeric", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            SSTabHelper.SetSelectedIndex(tabMainTab, 2)
            txtFirstInstalmentFrom.Focus()
        End If
    End Sub

    Private Sub txtDepositPC_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDepositPC.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtDepositPC_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDepositPC.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDepositPC)
        txtDepositPC.MaxLength = 15
    End Sub

    Private Sub txtDepositPC_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDepositPC.Leave
        If txtDepositPC.Text.Trim() = "" Then
            txtDepositPC.MaxLength = 15
        Else
            txtDepositPC.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDepositPC)
            txtDepositPC.MaxLength = txtDepositPC.Text.Trim().Length
        End If
    End Sub

    Private Sub txtEndDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEndDate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtEndDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEndDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEndDate)

    End Sub

    Private Sub txtEndDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEndDate.Leave

        If Information.IsDate(Me.txtStartDate.Text) And Information.IsDate(Me.txtEndDate.Text) Then
            If CDate(Me.txtStartDate.Text) >= CDate(Me.txtEndDate.Text) Then
                MessageBox.Show("The Start Date cannot be on or after the End Date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.txtEndDate.Text = ""
                Me.txtEndDate.Focus()
                Exit Sub
            End If
        End If
        'PN11944 Only format if something has been entered - spaces returns 1899 date
        If txtEndDate.Text <> "" Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEndDate)
        End If
    End Sub

    Private Sub txtMax1_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax1.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMax1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax1.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMax1)
        txtMax1.MaxLength = 15
    End Sub

    Private Sub txtMax1_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax1.Leave

        If txtMax1.Text.Trim() = "" Then
            txtMax1.MaxLength = 15
        Else
            txtMax1.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMax1)
            txtMax1.MaxLength = txtMax1.Text.Trim().Length
        End If

    End Sub

    Private Sub txtMax2_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax2.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMax2_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax2.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMax2)
        txtMax2.MaxLength = 15
    End Sub

    Private Sub txtMax2_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax2.Leave

        If txtMax2.Text.Trim() = "" Then
            txtMax2.MaxLength = 15
        Else
            txtMax2.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMax2)
            txtMax2.MaxLength = txtMax2.Text.Trim().Length
        End If

    End Sub

    Private Sub txtMax3_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax3.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMax3_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax3.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMax3)
        txtMax3.MaxLength = 15
    End Sub

    Private Sub txtMax3_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax3.Leave

        If txtMax3.Text.Trim() = "" Then
            txtMax3.MaxLength = 15
        Else
            txtMax3.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMax3)
            txtMax3.MaxLength = txtMax3.Text.Trim().Length
        End If

    End Sub

    Private Sub txtMax4_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax4.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMax4_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax4.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMax4)
        txtMax4.MaxLength = 15
    End Sub

    Private Sub txtMax4_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax4.Leave

        If txtMax4.Text.Trim() = "" Then
            txtMax4.MaxLength = 15
        Else
            txtMax4.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMax4)
            txtMax4.MaxLength = txtMax4.Text.Trim().Length
        End If

    End Sub

    Private Sub txtMax5_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax5.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMax5_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax5.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMax5)
        txtMax5.MaxLength = 15
    End Sub

    Private Sub txtMax5_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMax5.Leave

        If txtMax5.Text.Trim() = "" Then
            txtMax5.MaxLength = 15
        Else
            txtMax5.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMax5)
            txtMax5.MaxLength = txtMax5.Text.Trim().Length
        End If

    End Sub

    Private Sub txtMin1_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin1.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMin1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin1.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMin1)
        txtMin1.MaxLength = 15
    End Sub

    Private Sub txtMin1_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin1.Leave

        If txtMin1.Text.Trim() = "" Then
            txtMin1.MaxLength = 15
        Else
            txtMin1.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMin1)
            txtMin1.MaxLength = txtMin1.Text.Trim().Length
        End If

    End Sub

    Private Sub txtMin2_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin2.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMin2_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin2.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMin2)
        txtMin2.MaxLength = 15
    End Sub

    Private Sub txtMin2_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin2.Leave

        If txtMin2.Text.Trim() = "" Then
            txtMin2.MaxLength = 15
        Else
            txtMin2.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMin2)
            txtMin2.MaxLength = txtMin2.Text.Trim().Length
        End If

    End Sub

    Private Sub txtMin3_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin3.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMin3_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin3.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMin3)
        txtMin3.MaxLength = 15
    End Sub

    Private Sub txtMin3_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin3.Leave

        If txtMin3.Text.Trim() = "" Then
            txtMin3.MaxLength = 15
        Else
            txtMin3.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMin3)
            txtMin3.MaxLength = txtMin3.Text.Trim().Length
        End If

    End Sub

    Private Sub txtMin4_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin4.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMin4_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin4.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMin4)
        txtMin4.MaxLength = 15
    End Sub

    Private Sub txtMin4_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin4.Leave

        If txtMin4.Text.Trim() = "" Then
            txtMin4.MaxLength = 15
        Else
            txtMin4.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMin4)
            txtMin4.MaxLength = txtMin4.Text.Trim().Length
        End If

    End Sub

    Private Sub txtMin5_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin5.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMin5_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin5.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMin5)
        txtMin5.MaxLength = 15
    End Sub

    Private Sub txtMin5_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMin5.Leave

        If txtMin5.Text.Trim() = "" Then
            txtMin5.MaxLength = 15
        Else
            txtMin5.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMin5)
            txtMin5.MaxLength = txtMin5.Text.Trim().Length
        End If

    End Sub

    Private Sub txtMinInterest_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMinInterest.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMinInterest_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMinInterest.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMinInterest)
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtMinInterest, vControlValue:=txtMinInterest.Text)

        txtMinInterest.MaxLength = 3
    End Sub

    Private Sub txtMinInterest_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMinInterest.Leave
        If txtMinInterest.Text.Trim() = "" Then
            txtMinInterest.MaxLength = 3
        Else
            txtMinInterest.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMinInterest)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtMinInterest, vControlValue:=txtMinInterest.Text)
            txtMinInterest.MaxLength = txtMinInterest.Text.Trim().Length
        End If
    End Sub

    Private Sub txtMinMTA_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMinMTA.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMinMTA_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMinMTA.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMinMTA)

    End Sub

    Private Sub txtMinMTA_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMinMTA.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMinMTA)
    End Sub

    Private Sub txtMinMTAInstalments_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMinMTAInstalments.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtMinMTAInstalments_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMinMTAInstalments.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMinMTAInstalments)
    End Sub

    Private Sub txtMinMTAInstalments_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMinMTAInstalments.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMinMTAInstalments)
    End Sub

    Private Sub txtMnemonic_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMnemonic.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtProtectRate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtProtectRate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtProtectRate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtProtectRate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtProtectRate)
        txtProtectRate.MaxLength = 3
    End Sub

    Private Sub txtProtectRate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtProtectRate.Leave
        If txtProtectRate.Text.Trim() = "" Then
            txtProtectRate.MaxLength = 3
        Else
            txtProtectRate.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtProtectRate)
            txtProtectRate.MaxLength = txtProtectRate.Text.Trim().Length
        End If

    End Sub

    Private Sub txtRate1_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate1.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtRate1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate1.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRate1)
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRate1, vControlValue:=txtRate1.Text)
        txtRate1.MaxLength = 15
    End Sub

    Private Sub txtRate1_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate1.Leave

        If txtRate1.Text.Trim() = "" Then
            txtRate1.MaxLength = 15
        Else
            txtRate1.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRate1)
            txtRate1.MaxLength = txtRate1.Text.Trim().Length
        End If

    End Sub

    Private Sub txtRate2_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate2.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtRate2_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate2.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRate2)
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRate2, vControlValue:=txtRate2.Text)
        txtRate2.MaxLength = 15
    End Sub

    Private Sub txtRate2_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate2.Leave

        If txtRate2.Text.Trim() = "" Then
            txtRate2.MaxLength = 15
        Else
            txtRate2.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRate2)
            txtRate2.MaxLength = txtRate2.Text.Trim().Length
        End If

    End Sub

    Private Sub txtRate3_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate3.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtRate3_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate3.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRate3)
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRate3, vControlValue:=txtRate3.Text)
        txtRate3.MaxLength = 15
    End Sub

    Private Sub txtRate3_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate3.Leave

        If txtRate3.Text.Trim() = "" Then
            txtRate3.MaxLength = 15
        Else
            txtRate3.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRate3)
            txtRate3.MaxLength = txtRate3.Text.Trim().Length
        End If

    End Sub

    Private Sub txtRate4_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate4.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtRate4_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate4.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRate4)
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRate4, vControlValue:=txtRate4.Text)
        txtRate4.MaxLength = 15
    End Sub

    Private Sub txtRate4_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate4.Leave

        If txtRate4.Text.Trim() = "" Then
            txtRate4.MaxLength = 15
        Else
            txtRate4.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRate4)
            txtRate4.MaxLength = txtRate4.Text.Trim().Length
        End If

    End Sub

    Private Sub txtRate5_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate5.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtRate5_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate5.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRate5)
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRate5, vControlValue:=txtRate5.Text)
        txtRate5.MaxLength = 15
    End Sub

    Private Sub txtRate5_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate5.Leave

        If txtRate5.Text.Trim() = "" Then
            txtRate5.MaxLength = 15
        Else
            txtRate5.MaxLength = 100
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRate5)
            txtRate5.MaxLength = txtRate5.Text.Trim().Length
        End If

    End Sub

    Private Sub txtRemainderThreshhold_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRemainderThreshhold.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtRemainderThreshhold_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRemainderThreshhold.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRemainderThreshhold)
    End Sub

    Private Sub txtRemainderThreshhold_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRemainderThreshhold.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRemainderThreshhold)
    End Sub



    Private Sub txtRetryLimit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRetryLimit.Enter
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRetryLimit)

        txtRetryLimit.MaxLength = 5

    End Sub

    Private Sub txtStartDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStartDate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChanged = True
    End Sub

    Private Sub txtStartDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStartDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtStartDate)
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
        'PN11944 Only format if something has been entered - spaces returns 1899 date
        If txtStartDate.Text <> "" Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtStartDate)
        End If

    End Sub

    Private Sub cboProductFamily_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProductFamily.Enter
        m_lReturn = m_oFormFields.GotFocus(cboProductFamily)
    End Sub

    Private Sub cboProductFamily_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProductFamily.Leave
        m_lReturn = m_oFormFields.LostFocus(cboProductFamily)
        FormLogic()
    End Sub
    Public Function GetFrequencies() As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            m_oBusiness.GetFrequencies(vArray)

            If Not IsNothing(vArray) Then
                ' Alix - 07/04/2003
                cboStatementFrequ.Items.Clear()

                With cboFrequency
                    .Items.Clear()
                    Dim cboStatementFrequ_NewIndex As Integer = -1

                    cboStatementFrequ_NewIndex = cboStatementFrequ.Items.Add(New VB6.ListBoxItem("", 0))

                    For nRow As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                        Dim cboFrequency_NewIndex As Integer = -1
                        cboFrequency_NewIndex = .Items.Add(New VB6.ListBoxItem(CStr(vArray(1, nRow)), CInt(vArray(0, nRow))))
                        ' Alix - 07/04/2003
                        cboStatementFrequ_NewIndex = cboStatementFrequ.Items.Add(New VB6.ListBoxItem(CStr(vArray(1, nRow)), CInt(vArray(0, nRow))))
                        ' /Alix
                    Next nRow
                End With
            Else
                MessageBox.Show("No Frequencies available for Instalment scheme. Please select frequencies for Instalment from LookUp Maintenance.", "Get Frequency", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFrequencies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFrequencies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Alix Bergeret - 07/04/2003
    Public Function GetReports() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try


            m_oBusiness.GetReports(vArray)

            With cboStatementDoc
                .Items.Clear()
                'Developer Guide No 153

                Dim cboStatementDoc_NewIndex As Integer = .Items.Add(New VB6.ListBoxItem("", 0))

                For nRow As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    'Developer Guide No 153


                    cboStatementDoc_NewIndex = .Items.Add(New VB6.ListBoxItem(CStr(vArray(1, nRow)), CInt(vArray(0, nRow))))
                Next nRow
            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReports Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReports", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Alix Bergeret - 07/04/2003
    Public Function GetUserGroups() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try


            m_oBusiness.GetUserGroups(vArray)

            With cboReviewUserGrp
                .Items.Clear()
                'Developer Guide No 153

                Dim cboReviewUserGrp_NewIndex As Integer = .Items.Add(New VB6.ListBoxItem("", 0))

                For nRow As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    'Developer gudie No 153


                    cboReviewUserGrp_NewIndex = .Items.Add(New VB6.ListBoxItem(CStr(vArray(1, nRow)), CInt(vArray(0, nRow))))
                Next nRow
            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function FormatBlankFields() As Integer

        With m_oFormFields
            m_lReturn = .FormatControl(ctlControl:=txtMin1, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtMin2, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtMin3, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtMin4, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtMin5, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtMax1, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtMax2, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtMax3, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtMax4, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtMax5, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtRate1, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtRate2, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtRate3, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtRate4, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtRate5, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtR1Com, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtR2Com, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtR3Com, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtR4Com, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .FormatControl(ctlControl:=txtR5Com, vControlValue:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030401 - Added the following Code to process Existing Days Delay Field
    '               Ref : Issue 2915
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtExistingDaysDelay_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExistingDaysDelay.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtExistingDaysDelay)

        'AAB - 05-22-2003 - Based on Danny Davis advise, we are only checking that the value
        '                   is numeric for now.
        txtExistingDaysDelay.Text = txtExistingDaysDelay.Text.Trim()
        Dim dbNumericTemp As Double
        If Strings.Len(txtExistingDaysDelay.Text) > 0 And Double.TryParse(txtExistingDaysDelay.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then 'And |'       Val(txtExistingDaysDelay.Text) > 0 Then
            '         ' Valid entry so do nothing
            '    Else
            '        If Val(Trim(txtExistingDaysDelay.Text)) > Val(Trim(txtFirstInstalmentFrom.Text)) Then
            '            MsgBox "Invalid no of days delay." & vbCrLf & "So, defaulting the First Intalment From Days", vbExclamation, "Days Delay Invalid"
            '            ' Default the First Instalment From Date
            '            txtExistingDaysDelay.Text = txtFirstInstalmentFrom.Text
            '            txtExistingDaysDelay.SetFocus
            '        End If
        End If

    End Sub

    Private Sub txtExistingDaysDelay_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExistingDaysDelay.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtExistingDaysDelay)

        txtExistingDaysDelay.MaxLength = 5

    End Sub

    Private Function EnableDisableControls(ByVal v_iTab As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Tab 0
            Frame1.Enabled = False
            fraFees.Enabled = False
            fraFeesType.Enabled = False
            fraFeesChargedTo.Enabled = False
            fraDeposit.Enabled = False
            fraDepositType.Enabled = False
            fraDepositChargedTo.Enabled = False

            'Tab 1
            fraCharges.Enabled = False

            'Tab 2
            fraBackDated.Enabled = False
            fraAlign.Enabled = False
            Frame3.Enabled = False
            framRecollection.Enabled = False
            fraMTA.Enabled = False

            'Tab 3


            Select Case v_iTab
                Case 0
                    Frame1.Enabled = True
                    fraFees.Enabled = True
                    fraFeesType.Enabled = True
                    fraFeesChargedTo.Enabled = True
                    fraDeposit.Enabled = True
                    fraDepositType.Enabled = True
                    fraDepositChargedTo.Enabled = True
                Case 1
                    fraCharges.Enabled = True
                Case 2

                    If m_sSchemeType <> "TPR" Then
                        fraBackDated.Enabled = True
                        fraAlign.Enabled = True
                        framRecollection.Enabled = True
                    End If

                    Frame3.Enabled = True
                    fraMTA.Enabled = True
                Case 3

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableDisableControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableDisableControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub _optAlign_1_CheckedChanged(sender As Object, e As EventArgs) Handles _optAlign_1.CheckedChanged
        If _optAlign_1.Checked Then
            chkSingleInstalmentPerMonth.Checked = False
            chkSingleInstalmentPerMonth.Enabled = False
        Else
            chkSingleInstalmentPerMonth.Enabled = True
        End If
    End Sub

    Private Sub chkDepositOverrideAllowed_CheckedChanged(sender As Object, e As EventArgs)
        If chkDepositOverrideAllowed.CheckState = CheckState.Unchecked Then
            txtDepositPC.Text = "0.00"
            _optDepositType_1.Enabled = False
            _optDepositType_0.Enabled = False
            txtDepositPC.Enabled = False
        Else
            _optDepositType_1.Enabled = True
            _optDepositType_0.Enabled = True
            txtDepositPC.Enabled = True
        End If
    End Sub

#Region "Decimal Suppression Methods"
    Private Sub txtArrangementFee_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtArrangementFee.KeyPress, txtDepositPC.KeyPress, _
        txtProtectRate.KeyPress, txtMinInterest.KeyPress

        'Write logic to prevent decimal if value option has selected.
        If IsSuppressDecimalValues Then
            Select Case sender.name
                Case txtArrangementFee.Name, txtDepositPC.Name, txtProtectRate.Name, txtMinInterest.Name
                    If _optFeeType_1.Checked Then
                        gPMFunctions.NumPress(sender, e)
                    End If

                    If _optDepositType_1.Checked Then
                        gPMFunctions.NumPress(sender, e)
                    End If

                    If _optProtectionType_1.Checked Then
                        gPMFunctions.NumPress(sender, e)
                    End If
            End Select
        End If

    End Sub

#End Region


    ' ADO #39993: Returns the transaction_type value for Claim Recovery rates
    Private Function GetTransactionTypeValue() As Object
        If m_sSchemeType <> "CR" Then Return Nothing
        If cboProductFamily.SelectedIndex < 0 Then Return Nothing
        Select Case m_iProducts(cboProductFamily.SelectedIndex)
            Case "SR" : Return 1
            Case "TPR" : Return 2
            Case Else : Return Nothing
        End Select
    End Function
End Class
