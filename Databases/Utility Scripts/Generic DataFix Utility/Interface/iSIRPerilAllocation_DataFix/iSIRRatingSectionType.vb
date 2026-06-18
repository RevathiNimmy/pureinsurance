Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmDetail
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmDetail
    '
    ' Date: 16th August 2000
    '
    ' Description: Detail from used for data entry.
    '
    ' Edit History:
    ' 19/10/2005 RKS Premium Override
    ' ***************************************************************** '
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
    'Declare variables to hold the property values
    Private m_lRatingSectionTypeId As Integer
    Private m_lEarningPatternID As Integer
    Private m_lRateTypeId As Integer
    Private m_lMode As gPMConstants.PMEComponentAction
    'Private m_oLookup As Object
    Private m_dOldSumInsured As Double

    Private m_dProRataRate As Double
    Private m_iProRata As Integer

    Private m_sTransactionType As String = ""

    Private m_cOriginalAnnualPremium As Decimal

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails As Object
    Private m_vRatetype As Object

    Private m_bSysChange As Boolean

    Private m_lReturn As Integer

    Private m_lStatus As gPMConstants.PMEReturnCode

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    Private m_oCurrency As Object
    Private m_oCurrencyConvert As Object

    Private m_iDefinedCurrencyID As Integer
    Private m_sDefinedCurrencyCode As String = ""
    Private m_iTransactionCurrencyID As Integer
    Private m_sTransactionCurrencyCode As String = ""

    Private m_lCompanyID As Integer
    Private m_lInsuranceFileCnt As Integer

    Private m_dCurrencyBaseXrate As Double

    Private m_iIsAmended As Integer
    Private m_cCalculatedPremium As Decimal
    Private m_sOverrideReason As String = ""
    Private m_bAutoCalculated As Boolean
    Private m_obSirPerialAllocation As Object 'QBENZ022
    Private m_lRiskId As Integer
    Private m_lOriginalRiskcnt As Integer

    Private m_bUserAllowRatingSectionAddDelete As Boolean
    Private m_bUserAllowRatingSectionEdit As Boolean
    Private m_bAllowRatingSectionAdd As Boolean
    Private m_bAllowRatingSectionEdit As Boolean
    Private m_bAllowRatingSectionDelete As Boolean
    Private m_bAllowEditRatingSectionRateType As Boolean
    Private m_bAllowEditRatingSectionRate As Boolean
    Private m_bAllowEditRatingSectionSumInsured As Boolean
    Private m_bAllowEditRatingSectionThisPremium As Boolean
    'Developer Guide No. 50
    Dim objInterface As frmInterface
    'PN 37503 (RC)
    Public m_bFormLoaded As Boolean

    Public WriteOnly Property OverrideReason() As String
        Set(ByVal Value As String)
            m_sOverrideReason = Value
        End Set
    End Property


    Public WriteOnly Property IsAmended() As Integer
        Set(ByVal Value As Integer)
            m_iIsAmended = Value
        End Set
    End Property

    Public WriteOnly Property CalculatedPremium() As Decimal
        Set(ByVal Value As Decimal)
            m_cCalculatedPremium = Value
        End Set
    End Property
    ' QBENZ022
    Public WriteOnly Property AutoCalculated() As Boolean
        Set(ByVal Value As Boolean)
            m_bAutoCalculated = Value
        End Set
    End Property
    Public WriteOnly Property RiskId() As Integer
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

    Public WriteOnly Property BusinessObject() As Object
        Set(ByVal Value As Object)
            m_obSirPerialAllocation = Value
        End Set
    End Property
    Public WriteOnly Property OriginalRiskCnt() As Integer
        Set(ByVal Value As Integer)
            If Value <= 0 Then
                m_lOriginalRiskcnt = 0
            Else
                m_lOriginalRiskcnt = Value
            End If
        End Set
    End Property

    Public WriteOnly Property DefinedCurrencyID() As Integer
        Set(ByVal Value As Integer)
            Dim cPremium, cBaseAmount As Decimal

            m_iDefinedCurrencyID = Value

            'Set the defined currency premiums
            If m_iDefinedCurrencyID <> 0 And m_iDefinedCurrencyID <> m_iTransactionCurrencyID Then

                'Get the rate from the policy, if there is one.

                m_lReturn = m_oCurrencyConvert.GetInsuranceFileInformation(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_dCurrencyBaseXrate:=m_dCurrencyBaseXrate)


                cPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtPremium))

                'Convert to base from transaction currency.

                m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=m_iTransactionCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cPremium, vConversionRate:=m_dCurrencyBaseXrate)

                'Convert from base to defined currency

                m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iDefinedCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cPremium)

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPremiumDefCurr, vControlValue:=cPremium)



                cPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtThisPremium))

                'Convert to base from transaction currency.

                m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=m_iTransactionCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cPremium, vConversionRate:=m_dCurrencyBaseXrate)

                'Convert from base to defined currency

                m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iDefinedCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cPremium)

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisPremiumDefCurr, vControlValue:=cPremium)
            End If

        End Set
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property CompanyID() As Integer
        Set(ByVal Value As Integer)
            m_lCompanyID = Value
        End Set
    End Property

    Public WriteOnly Property TransactionCurrencyID() As Integer
        Set(ByVal Value As Integer)

            m_iTransactionCurrencyID = Value

            'Get currency code of the currency we are doing a transaction for.

            m_lReturn = m_oCurrency.GetISOCodeFromCurrencyID(v_iCurrencyID:=m_iTransactionCurrencyID, r_sISOCode:=m_sTransactionCurrencyCode)
            m_sTransactionCurrencyCode = m_sTransactionCurrencyCode.Trim()

            'Add transaction currency to end of sum insured caption

            Dim sCaption As String = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSumInsuredLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblSumInsured.Text = sCaption & " (" & m_sTransactionCurrencyCode & ")"

            'Add transaction currency to end of caption
            lblTransactionCurrency.Text = "Transaction Currency (" & m_sTransactionCurrencyCode & ")"

        End Set
    End Property

    Public Property RateTypeId() As Integer
        Get
            Return m_lRateTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lRateTypeId = Value
        End Set
    End Property

    Public Property RatingSectionTypeId() As Integer
        Get
            Return m_lRatingSectionTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lRatingSectionTypeId = Value

            'm_lReturn = ResetRatingSection()

        End Set
    End Property


    Public Property EarningPatternID() As Integer
        Get
            Return m_lEarningPatternID
        End Get
        Set(ByVal Value As Integer)
            m_lEarningPatternID = Value
        End Set
    End Property




    Public Property ProRata() As Integer
        Get
            Return m_iProRata
        End Get
        Set(ByVal Value As Integer)
            m_iProRata = Value
        End Set
    End Property


    Public Property ProRataRate() As Double
        Get
            Return m_dProRataRate
        End Get
        Set(ByVal Value As Double)
            m_dProRataRate = Value
        End Set
    End Property


    Public Property OriginalAnnualPremium() As Decimal
        Get
            Return m_cOriginalAnnualPremium
        End Get
        Set(ByVal Value As Decimal)
            m_cOriginalAnnualPremium = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property


    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property


    Public WriteOnly Property Mode() As Integer
        Set(ByVal Value As Integer)


            m_lMode = Value

            m_bSysChange = True

            m_lReturn = PopulateRatingSectionType() 'QBENZ022
            m_lReturn = ResetRatingSection()

            If Value = gPMConstants.PMEComponentAction.PMAdd Then

                'Thinh Nguyen 17/04/2002 (start) - only select first item if we have data
                'cboRatingSectionType.ListIndex = 0
                ' PW120902 - if adding a new one, we want it to default the other
                ' fields so turn off the system change flag
                m_bSysChange = False
                If cboRatingSectionType.Items.Count > 0 Then
                    cboRatingSectionType.SelectedIndex = 0
                End If
                'Thinh Nguyen 17/04/2002 (end) - only select first item if we have data

            Else
                'Set the Ratingsectiontype combo
                For nCount As Integer = 0 To cboRatingSectionType.Items.Count - 1
                    If VB6.GetItemData(cboRatingSectionType, nCount) = m_lRatingSectionTypeId Then
                        cboRatingSectionType.SelectedIndex = nCount
                    End If
                Next

                'Set the Ratetype combo
                For nCount As Integer = 0 To cboRateType.ListCount - 1
                    If cboRateType.ItemData(nCount) = m_lRateTypeId Then
                        cboRateType.ListIndex = nCount
                    End If
                Next

            End If

            If m_lMode = gPMConstants.PMEComponentAction.PMEdit Then cboRatingSectionType.Enabled = False

            If m_lMode = gPMConstants.PMEComponentAction.PMEdit Then
                txtThisPremium.Enabled = m_bAllowEditRatingSectionThisPremium
                txtThisPremiumDefCurr.Enabled = m_bAllowEditRatingSectionThisPremium

                cboRateType.Enabled = m_bAllowEditRatingSectionRateType
                txtRate.Enabled = m_bAllowEditRatingSectionRate
                txtRate2.Enabled = m_bAllowEditRatingSectionRate
                txtRate3.Enabled = m_bAllowEditRatingSectionRate
                txtSumInsured.Enabled = m_bAllowEditRatingSectionSumInsured
                cboEarningPattern.ItemId = m_lEarningPatternID
                cboEarningPattern.Enabled = Not (cboEarningPattern.ItemCode = "FULLY")


            End If

            m_bSysChange = False

        End Set
    End Property


    Public Property AllowEditRatingSectionRateType() As Boolean
        Get
            Return m_bAllowEditRatingSectionRateType
        End Get
        Set(ByVal Value As Boolean)
            m_bAllowEditRatingSectionRateType = Value
        End Set
    End Property


    Public Property AllowEditRatingSectionRate() As Boolean
        Get
            Return m_bAllowEditRatingSectionRate
        End Get
        Set(ByVal Value As Boolean)
            m_bAllowEditRatingSectionRate = Value
        End Set
    End Property


    Public Property AllowEditRatingSectionSumInsured() As Boolean
        Get
            Return m_bAllowEditRatingSectionSumInsured
        End Get
        Set(ByVal Value As Boolean)
            m_bAllowEditRatingSectionSumInsured = Value
        End Set
    End Property


    Public Property AllowEditRatingSectionThisPremium() As Boolean
        Get
            Return m_bAllowEditRatingSectionThisPremium
        End Get
        Set(ByVal Value As Boolean)
            m_bAllowEditRatingSectionThisPremium = Value
        End Set
    End Property
    ' QBENZ022
    Private Function PopulateRatingSectionType() As Integer

        Dim result As Integer = 0
        Try
            Dim r_vResults(,) As Object
            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_obSirPerialAllocation.GetRatingSectionType_ForRiskType(lMode:=m_lMode, lRatingSectionTypeId:=m_lRatingSectionTypeId, lOriginalRiskcnt:=m_lOriginalRiskcnt, lRiskCnt:=m_lRiskId, r_vResults:=r_vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Execute PopulateRatingSectionType", vApp:=ACApp, vClass:="", vMethod:="PopulateRatingSectionType", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If Information.IsArray(r_vResults) Then

                For ivar As Integer = 0 To r_vResults.GetUpperBound(1)
                    Dim cboRatingSectionType_NewIndex As Integer = -1

                    cboRatingSectionType_NewIndex = cboRatingSectionType.Items.Add(CStr(r_vResults(1, ivar)))

                    VB6.SetItemData(cboRatingSectionType, cboRatingSectionType_NewIndex, CInt(r_vResults(0, ivar)))
                Next
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Execute PopulateRatingSectionType", vApp:=ACApp, vClass:="", vMethod:="PopulateRatingSectionType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CalculatePremiumAndDisplay() As Integer

        Dim result As Integer = 0
        Dim cRate As Decimal
        Dim lRateTypeId As Integer
        Dim sRateType As String = ""
        Dim cSumInsured As Decimal
        Dim sTemp As String = ""
        Dim dTemp As Double
        Dim cPremium, cThisPremium, cBaseAmount As Decimal




        'Set the Return value
        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the values
        If txtRate.Visible Then
            sTemp = txtRate.Text
        ElseIf txtRate2.Visible Then
            sTemp = txtRate2.Text
        ElseIf txtRate3.Visible Then
            sTemp = txtRate3.Text
        End If

        If sTemp <> "" Then
            If sTemp.EndsWith("%") Then
                sTemp = sTemp.Substring(0, sTemp.Length - 1)
            End If
        End If

        Dim dbNumericTemp As Double
        If Not Double.TryParse(sTemp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            cRate = 0
        Else


            cRate = CDec(sTemp)
            If Information.Err().Number <> 0 Then
                lblOverflow.Text = "Overflow"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        Try

            cSumInsured = Conversion.Val(CStr(m_oFormFields.UnformatControl(ctlControl:=txtSumInsured)))

            If Information.Err().Number <> 0 Then
                lblOverflow.Text = "Overflow"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lblOverflow.Text = ""




            If cboRateType.ItemCaption <> "" Then
                lRateTypeId = cboRateType.ItemData(cboRateType.ListIndex)
            Else
                lRateTypeId = 0
            End If

            sRateType = GetRateType(lRateTypeId)


            Select Case sRateType.Trim().ToUpper()
                Case "V" ' Value

                    cPremium = cRate

                Case "C" ' Percentage

                    dTemp = (cRate * cSumInsured) / 100


                    cPremium = dTemp

                    If Information.Err().Number = 0 Then
                        lblOverflow.Text = ""
                    Else
                        cPremium = 0
                        lblOverflow.Text = "Overflow"
                    End If

                Case "P" ' Per 1000

                    dTemp = (cRate * cSumInsured) / 1000


                    cPremium = dTemp

                    If Information.Err().Number = 0 Then
                        lblOverflow.Text = ""
                    Else
                        cPremium = 0
                        lblOverflow.Text = "Overflow"
                    End If

                Case "M"

                    dTemp = (cRate * cSumInsured) / 1000000


                    cPremium = dTemp

                    If Information.Err().Number = 0 Then
                        lblOverflow.Text = ""
                    Else
                        cPremium = 0
                        lblOverflow.Text = "Overflow"
                    End If


                Case "Q" 'Quantity

                    dTemp = cRate * cSumInsured


                    cPremium = dTemp

                    If Information.Err().Number = 0 Then
                        lblOverflow.Text = ""
                    Else
                        cPremium = 0
                        lblOverflow.Text = "Overflow"
                    End If

                Case "T"
                    '*** To be implemented later

                Case "O"

                    cPremium = 0

                Case Else

                    cPremium = 0

            End Select




            dTemp = (cPremium - (m_iProRata * m_cOriginalAnnualPremium)) * m_dProRataRate


            cThisPremium = dTemp

            If Information.Err().Number = 0 Then
                lblOverflow.Text = ""
            Else
                cThisPremium = 0
                lblOverflow.Text = "Overflow"
            End If

            'Assign the premium to the interface
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPremium, vControlValue:=cPremium)

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisPremium, vControlValue:=cThisPremium)

            If (sRateType.Trim().ToUpper() = "V" Or sRateType.Trim().ToUpper() = "Q") And m_iDefinedCurrencyID <> 0 And m_iDefinedCurrencyID <> m_iTransactionCurrencyID Then

                'Convert to base from transaction currency.

                m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=m_iTransactionCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cPremium, vConversionRate:=m_dCurrencyBaseXrate)

                'Convert from base to defined currency

                m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iDefinedCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cPremium)

                'Convert to base from transaction currency.

                m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=m_iTransactionCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cThisPremium, vConversionRate:=m_dCurrencyBaseXrate)

                'Convert from base to defined currency

                m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iDefinedCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cThisPremium)

                'Assign the defined currency premium to the interface
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPremiumDefCurr, vControlValue:=cPremium)

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisPremiumDefCurr, vControlValue:=cThisPremium)

            End If

            Return result

Err_CalculatePremiumAndDisplay:
            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Calculate Premium", vApp:=ACApp, vClass:="", vMethod:="CalculatePremiumAndDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result



            Return result

        Catch exc As System.Exception
        End Try
    End Function

    Private Function ResetRatingSection() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For lTemp As Integer = 0 To cboRatingSectionType.Items.Count - 1
                If VB6.GetItemData(cboRatingSectionType, lTemp) = m_lRatingSectionTypeId Then
                    cboRatingSectionType.SelectedIndex = lTemp

                    Exit For
                End If
            Next lTemp
            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Calculate Premium", vApp:=ACApp, vClass:="", vMethod:="ResetRatingSection", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDetailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                    "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:="", vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRatingSectiontype.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRatingSectionTypeLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEarningPattern.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEarningPatternLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRatetype.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRateTypeLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRateLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSumInsured.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSumInsuredLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblSumInsured.Text = lblSumInsured.Text & " (" & m_sTransactionCurrencyCode & ")"

            lblPremium.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPremiumLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblThisPremium.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACThisPremiumLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:="", vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetRateType(ByVal v_lRateTypeId As Integer) As String

        Dim result As String = String.Empty

        'Set the rate Type as undefined
        result = "U"

        Return cboRateType.ItemCode.Trim()

        '    If IsArray(m_vRatetype) Then
        '
        '        For nCount = LBound(m_vRatetype, 2) To UBound(m_vRatetype, 2)
        '            If (m_vRatetype(0, nCount) = v_lRateTypeId) Then
        '                GetRateType = m_vRatetype(1, nCount)
        '                Exit For
        '            End If
        '        Next
        '
        '    End If

    End Function

    Private Sub cboCountry_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCountry.Click
        cboState.WhereClause = "country_id = " & cboCountry.ItemId
        cboState.RefreshList()
    End Sub

    '**********************************************************************
    ' PW030902 - Move the code that chooses which rate text box to show to the
    '            cboRateType_Click as this needs to be done every time the
    '            Rate Type is changed, not just when the Rating Section
    '            Type is changed.
    '**********************************************************************
    Private Sub cboRateType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRateType.Click

        Dim lRateTypeId As Integer
        Dim sRateType As String = ""
        Dim cRate As Decimal
        Dim iPos As Integer

        If Not m_bSysChange Then

            lRateTypeId = cboRateType.ItemData(cboRateType.ListIndex)
            sRateType = GetRateType(lRateTypeId)

            ' PW030902 - store the current rate from the text box which is
            ' currently in use
            If txtRate.Visible Then
                iPos = (txtRate.Text.IndexOf("%"c) + 1)
                If iPos = 0 Then
                    cRate = Conversion.Val(txtRate.Text.Trim.Replace(",", ""))
                Else
                    cRate = Conversion.Val(txtRate.Text.Substring(0, iPos - 1))
                End If
            ElseIf txtRate2.Visible Then
                iPos = (txtRate2.Text.IndexOf("%"c) + 1)
                If iPos = 0 Then
                    cRate = Conversion.Val(txtRate2.Text.Trim.Replace(",", ""))
                Else
                    cRate = Conversion.Val(txtRate2.Text.Substring(0, iPos - 1))
                End If
            ElseIf txtRate3.Visible Then
                iPos = (txtRate3.Text.IndexOf("%"c) + 1)
                If iPos = 0 Then
                    cRate = Conversion.Val(txtRate3.Text.Trim.Replace(",", ""))
                Else
                    cRate = Conversion.Val(txtRate3.Text.Substring(0, iPos - 1))
                End If
            End If

            ' PW030902 - Update each of the 3 rate text boxes with the current
            ' rate - these must be kept in line so that when the user switches
            ' between Rate Types, the values don't change            
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRate2, vControlValue:=cRate)
            txtRate.Text = StringsHelper.Format(cRate, "##0.000000").Trim() & "%"
            txtRate3.Text = StringsHelper.Format(cRate, "##0.0000").Trim()

            ' PW030903 - Show the correctly formatted Rate text box
            ' depending on the Rating type
            txtRate.Visible = False
            txtRate2.Visible = False
            txtRate3.Visible = False
            Select Case sRateType
                Case "C", "T", "%"
                    txtRate.Visible = True
                    txtRate2.Visible = False
                    txtRate3.Visible = False
                Case "P", "M"
                    txtRate2.Visible = True
                    txtRate.Visible = False
                    txtRate3.Visible = False
                Case Else
                    txtRate3.Visible = True
                    txtRate.Visible = False
                    txtRate2.Visible = False
            End Select

            ' Show currency convert fields?
            m_lReturn = ShowExtraFields()

            'Calculate the premium
            If m_bFormLoaded Then
                m_lReturn = CalculatePremiumAndDisplay()
            End If
        End If

    End Sub

    '**********************************************************************
    ' PW030902 - Move the code that chooses which rate text box to show to the
    '            cboRateType_Click as this needs to be done every time the
    '            Rate Type is changed, not just when the Rating Section
    '            Type is changed.
    '**********************************************************************
    Private Sub cboRatingSectionType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRatingSectionType.SelectedIndexChanged

        Dim lRatingSectionTypeId, lRateType As Integer
        Dim cRate As Decimal
        Dim cBaseAmount As Decimal

        Dim bFound As Boolean = False

        If Not m_bSysChange Then

            If cboRatingSectionType.SelectedIndex >= 0 Then

                'Rating Section type is changed, Get the default rate type and rate and display
                lRatingSectionTypeId = VB6.GetItemData(cboRatingSectionType, cboRatingSectionType.SelectedIndex)

                For icount As Integer = g_vRatingSectionType.GetLowerBound(1) To g_vRatingSectionType.GetUpperBound(1)

                    If CDbl(g_vRatingSectionType(0, icount)) = lRatingSectionTypeId Then


                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(g_vRatingSectionType(3, icount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                            lRateType = CInt(g_vRatingSectionType(3, icount))
                        Else
                            lRateType = 0
                        End If


                        Dim dbNumericTemp2 As Double
                        If Double.TryParse(CStr(g_vRatingSectionType(4, icount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                            cRate = CDec(g_vRatingSectionType(4, icount))
                        Else
                            cRate = 0
                        End If

                        'Get defined currency for "Value" rate types.

                        Dim dbNumericTemp3 As Double
                        If Double.TryParse(CStr(g_vRatingSectionType(5, icount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then

                            m_iDefinedCurrencyID = CInt(g_vRatingSectionType(5, icount))
                        Else
                            m_iDefinedCurrencyID = m_iTransactionCurrencyID
                        End If

                        ' Found a record set, default country and state

                        cboCountry.ItemId = gPMFunctions.ToSafeLong(CStr(g_vRatingSectionType(6, icount)))

                        cboState.ItemId = gPMFunctions.ToSafeLong(CStr(g_vRatingSectionType(7, icount)))


                        Dim dbNumericTemp4 As Double
                        If Double.TryParse(CStr(g_vRatingSectionType(8, icount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then

                            cboEarningPattern.ItemId = gPMFunctions.ToSafeLong(CStr(g_vRatingSectionType(8, icount)))

                            ' it should be disableled when Immediately Earned
                            cboEarningPattern.Enabled = Not (cboEarningPattern.ItemCode = "FULLY")

                        End If

                        bFound = True
                        Exit For
                    End If
                Next

                If bFound Then

                    'Set the Rate type combo
                    For icount As Integer = 0 To cboRateType.ListCount - 1
                        If cboRateType.ItemData(icount) = lRateType Then
                            cboRateType.ListIndex = icount
                            Exit For
                        End If
                    Next

                    'If rate type is "Value" or "Quantity x Rate" and currency is different from transaction
                    'then see show amount in both currencies.
                    If (lRateType = 2 Or lRateType = 3) And m_iDefinedCurrencyID <> m_iTransactionCurrencyID Then

                        'Get currency code of the currency we defined this value as.

                        m_lReturn = m_oCurrency.GetISOCodeFromCurrencyID(v_iCurrencyID:=m_iDefinedCurrencyID, r_sISOCode:=m_sDefinedCurrencyCode)
                        m_sDefinedCurrencyCode = m_sDefinedCurrencyCode.Trim()

                        'Add defined currency to end of caption
                        lblDefinedCurrency.Text = "Defined Currency (" & m_sDefinedCurrencyCode & ")"

                        'Get the rate from the policy, if there is one.

                        m_lReturn = m_oCurrencyConvert.GetInsuranceFileInformation(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_dCurrencyBaseXrate:=m_dCurrencyBaseXrate)

                        'Convert to base from defined currency.

                        m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=m_iDefinedCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cRate)

                        'Convert from base to transaction currency

                        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iTransactionCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cRate, vConversionDate:=Nothing, vConversionRate:=m_dCurrencyBaseXrate)
                    End If

                    ' Show currency convert fields?
                    m_lReturn = ShowExtraFields()

                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRate2, vControlValue:=cRate)
                    txtRate.Text = StringsHelper.Format(cRate, "##0.0000").Trim() & "%"
                    txtRate3.Text = StringsHelper.Format(cRate, "##0.0000").Trim()

                End If
            End If
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
        'Unload Me
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 20
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim nItem As ListViewItem

        'Developer Guide No. 50
        objInterface = DirectCast(Me.Owner, frmInterface)
        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        'Thinh Nguyen 17/04/2002 (start) - validate form
        If m_lMode = gPMConstants.PMEComponentAction.PMAdd Or m_lMode = gPMConstants.PMEComponentAction.PMEdit Then
            If ValidateOK() <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Invalid data please re-enter", "PerilAllocation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        End If
        'Thinh Nguyen 17/04/2002 (end) - validate form

        m_lReturn = CheckPremiumOverride()


        Select Case m_lMode
            Case gPMConstants.PMEComponentAction.PMAdd

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'add a new item in the listview
                    'Developer Guide No. 50

                    nItem = objInterface.lvwRatingSection.Items.Add(VB6.GetItemString(cboRatingSectionType, cboRatingSectionType.SelectedIndex))

                    'nItem.SubItems(ACRateTypeCol) = cboRateType1.List(cboRateType.ListIndex)
                    ListViewHelper.GetListViewSubItem(nItem, ACRateTypeCol).Text = cboRateType.ItemCaption
                    If txtRate.Visible Then
                        ListViewHelper.GetListViewSubItem(nItem, ACRateCol).Text = txtRate.Text
                    ElseIf txtRate2.Visible Then
                        ListViewHelper.GetListViewSubItem(nItem, ACRateCol).Text = txtRate2.Text
                    ElseIf txtRate3.Visible Then
                        ListViewHelper.GetListViewSubItem(nItem, ACRateCol).Text = txtRate3.Text
                    End If
                    ListViewHelper.GetListViewSubItem(nItem, ACSumInsuredCol).Text = txtSumInsured.Text
                    ListViewHelper.GetListViewSubItem(nItem, ACPremiumCol).Text = txtPremium.Text
                    ListViewHelper.GetListViewSubItem(nItem, ACThisPremiumCol).Text = CStr(gPMFunctions.ToSafeCurrency(txtThisPremium.Text, 0))
                    ListViewHelper.GetListViewSubItem(nItem, ACRatingSectionTypeIdCol).Text = CStr(VB6.GetItemData(cboRatingSectionType, cboRatingSectionType.SelectedIndex))

                    ListViewHelper.GetListViewSubItem(nItem, ACRateTypeIdCol).Text = cboRateType.ItemData(cboRateType.ListIndex)
                    ListViewHelper.GetListViewSubItem(nItem, ACDefinedCurrencyID).Text = CStr(m_iDefinedCurrencyID)

                    ListViewHelper.GetListViewSubItem(nItem, ACCountryCol).Text = IIf(cboCountry.ItemId = 0, "", cboCountry.ItemCaption)
                    ListViewHelper.GetListViewSubItem(nItem, ACCountryIDCol).Text = CStr(cboCountry.ItemId)
                    ListViewHelper.GetListViewSubItem(nItem, ACStateCol).Text = IIf(cboState.ItemId = 0, "", cboState.ItemCaption)
                    ListViewHelper.GetListViewSubItem(nItem, ACStateIDCol).Text = CStr(cboState.ItemId)

                    ListViewHelper.GetListViewSubItem(nItem, ACIsAmendedCol).Text = CStr(0)
                    ListViewHelper.GetListViewSubItem(nItem, ACCalculatedPremiumCol).Text = txtThisPremium.Text
                    ListViewHelper.GetListViewSubItem(nItem, ACOverrideReasonCol).Text = ""

                    ListViewHelper.GetListViewSubItem(nItem, ACEarningPatternCol).Text = IIf(cboEarningPattern.ItemId = 0, "", cboEarningPattern.ItemCaption)
                    ListViewHelper.GetListViewSubItem(nItem, ACEarningPatternIDCol).Text = CStr(cboEarningPattern.ItemId)

                Else
                    Exit Sub
                End If

            Case gPMConstants.PMEComponentAction.PMEdit

                'Update the selected item
                'Developer Guide No. 50

                nItem = objInterface.lvwRatingSection.FocusedItem
                nItem.Text = VB6.GetItemString(cboRatingSectionType, cboRatingSectionType.SelectedIndex)

                ListViewHelper.GetListViewSubItem(nItem, ACRateTypeCol).Text = cboRateType.ItemCaption

                If txtRate.Visible Then
                    ListViewHelper.GetListViewSubItem(nItem, ACRateCol).Text = txtRate.Text
                ElseIf txtRate2.Visible Then
                    ListViewHelper.GetListViewSubItem(nItem, ACRateCol).Text = txtRate2.Text
                ElseIf txtRate3.Visible Then
                    ListViewHelper.GetListViewSubItem(nItem, ACRateCol).Text = txtRate3.Text
                End If
                ListViewHelper.GetListViewSubItem(nItem, ACSumInsuredCol).Text = txtSumInsured.Text
                ListViewHelper.GetListViewSubItem(nItem, ACPremiumCol).Text = txtPremium.Text
                ListViewHelper.GetListViewSubItem(nItem, ACThisPremiumCol).Text = CStr(txtThisPremium.Text)
                ListViewHelper.GetListViewSubItem(nItem, ACRatingSectionTypeIdCol).Text = CStr(VB6.GetItemData(cboRatingSectionType, cboRatingSectionType.SelectedIndex))

                ListViewHelper.GetListViewSubItem(nItem, ACRateTypeIdCol).Text = cboRateType.ItemData(cboRateType.ListIndex)
                ListViewHelper.GetListViewSubItem(nItem, ACDefinedCurrencyID).Text = CStr(m_iDefinedCurrencyID)
                ListViewHelper.GetListViewSubItem(nItem, ACCountryCol).Text = IIf(cboCountry.ItemId = 0, "", cboCountry.ItemCaption)
                ListViewHelper.GetListViewSubItem(nItem, ACCountryIDCol).Text = CStr(cboCountry.ItemId)
                ListViewHelper.GetListViewSubItem(nItem, ACStateCol).Text = IIf(cboState.ItemId = 0, "", cboState.ItemCaption)
                ListViewHelper.GetListViewSubItem(nItem, ACStateIDCol).Text = CStr(cboState.ItemId)

                ListViewHelper.GetListViewSubItem(nItem, ACIsAmendedCol).Text = CStr(m_iIsAmended)
                'nItem.SubItems(ACCalculatedPremiumCol) = txtThisPremium.Text
                ListViewHelper.GetListViewSubItem(nItem, ACOverrideReasonCol).Text = txtOverrideReason.Text

                ListViewHelper.GetListViewSubItem(nItem, ACEarningPatternCol).Text = IIf(cboEarningPattern.ItemId = 0, "", cboEarningPattern.ItemCaption)
                ListViewHelper.GetListViewSubItem(nItem, ACEarningPatternIDCol).Text = CStr(cboEarningPattern.ItemId)

            Case Else ' Nothing to do for other modes

        End Select

        Me.Hide()
        '    Unload Me

    End Sub

    'PN 37503 (RC)
    Private Sub frmDetail_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            If m_bFormLoaded = False Then
                'cboRateType_Click(eventSender, eventArgs)
                m_bFormLoaded = True
            End If
        End If
    End Sub

    Private Sub Form_Initialize_Renamed()
        'Get an instance of the lookup object
        ''    m_lReturn& = g_oObjectManager.GetInstance( _
        ''''        oObject:=m_oLookup, _
        ''''        sClassName:="bPMLookup.Business", _
        ''''        vInstanceManager:=PMGetViaClientManager)

        'Set the product familiy of lookup
        ''''m_oLookup.PMLookupProductFamily = pmePFSiriusSolutions

        Dim temp_m_oCurrencyConvert As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oCurrencyConvert = temp_m_oCurrencyConvert

        Dim temp_m_oCurrency As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrency, "bACTCurrency.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oCurrency = temp_m_oCurrency

        m_oFormFields = New iPMFormControl.FormFields()

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSumInsured, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRate2, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPremium, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtThisPremium, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPremiumDefCurr, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtThisPremiumDefCurr, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        'Developer Guide No. 220
        Me.cboRateType.FirstItem = ""
        Me.cboEarningPattern.FirstItem = ""
        Me.cboState.FirstItem = "(none)"
        Me.cboCountry.FirstItem = "(none)"


    End Sub

    Public Sub frmLoad()
        txtRate2.Left = txtRate.Left
        txtRate2.Top = txtRate.Top

        txtRate3.Left = txtRate.Left
        txtRate3.Top = txtRate.Top

        m_oFormFields = New iPMFormControl.FormFields()

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSumInsured, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRate2, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPremium, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtThisPremium, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPremiumDefCurr, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtThisPremiumDefCurr, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        ReDim m_vLookupValues(3, 2)

        m_vLookupValues(0, 0) = "Rating_Section_Type"
        m_vLookupValues(0, 1) = "Earning Pattern"
        m_vLookupValues(0, 2) = "Rate_Type"


        'm_lReturn = GetLookupValues()


        'Fill the lookup values
        'm_lReturn = GetLookupDetails("Rating_Section_Type", cboRatingSectionType) 'Deepak Remove this
        'm_lReturn = GetLookupDetails("Policy_Section_Type", cboPolicySectionType1)
        'm_lReturn = GetLookupDetails("Rate_Type", cboRateType1, m_vRatetype)


        If m_lMode = gPMConstants.PMEComponentAction.PMEdit Then
            txtThisPremium.Enabled = m_bAllowEditRatingSectionThisPremium
            txtThisPremiumDefCurr.Enabled = m_bAllowEditRatingSectionThisPremium

            cboRateType.Enabled = m_bAllowEditRatingSectionRateType
            txtRate.Enabled = m_bAllowEditRatingSectionRate
            txtRate2.Enabled = m_bAllowEditRatingSectionRate
            txtRate3.Enabled = m_bAllowEditRatingSectionRate
            txtSumInsured.Enabled = m_bAllowEditRatingSectionSumInsured
        End If
        'Display the captions
        m_lReturn = DisplayCaptions()

        ' Show currency convert fields?
        m_lReturn = ShowExtraFields()

        cboRateType_Click(cboRateType, Nothing)
    End Sub


    Private Sub frmDetail_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)


        If UnloadMode <> vbFormCode Then
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Else
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

        End If

        m_oFormFields.Dispose()
        m_oFormFields = Nothing

        eventArgs.Cancel = Cancel <> 0
    End Sub


    Private Sub Form_Terminate_Renamed()
        If Not (m_oCurrencyConvert Is Nothing) Then

            m_oCurrencyConvert.Dispose()
            m_oCurrencyConvert = Nothing
        End If
        If Not (m_oCurrency Is Nothing) Then

            m_oCurrency.Dispose()
            m_oCurrency = Nothing
        End If
    End Sub

    Private Sub txtOverrideReason_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOverrideReason.Leave
        m_sOverrideReason = txtOverrideReason.Text
    End Sub

    Private Sub txtPremium_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPremium.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPremium)

    End Sub

    Private Sub txtPremium_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPremium.Leave


        Dim cThisPremium, cBaseAmount As Decimal

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPremium)


        Dim dTemp As Double = CDbl(m_oFormFields.UnformatControl(ctlControl:=txtPremium))


        Try

            Dim cTemp As Decimal = dTemp

            If Information.Err().Number <> 0 Then
                lblOverflow.Text = "Overflow"
                txtPremium.Focus()
                Exit Sub
            End If

            lblOverflow.Text = ""

            If Not m_bSysChange Then

                'Update defined currency box.
                If txtPremiumDefCurr.Visible Then
                    'Convert to base from defined currency.

                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=m_iTransactionCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cTemp, vConversionRate:=m_dCurrencyBaseXrate)

                    'Convert from base to transaction currency

                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iDefinedCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cTemp)

                    'Assign the defined currency premium to the interface
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPremiumDefCurr, vControlValue:=cTemp)

                End If


                If Conversion.Val(CStr(m_oFormFields.UnformatControl(ctlControl:=txtRate2))) <> 0 Then
                    Exit Sub
                End If


                cThisPremium = Conversion.Val(CStr(m_oFormFields.UnformatControl(ctlControl:=txtPremium))) * m_dProRataRate


                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisPremium, vControlValue:=cThisPremium)

                'Update defined currency box.
                If txtPremiumDefCurr.Visible Then
                    'Convert to base from defined currency.

                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=m_iTransactionCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cThisPremium, vConversionRate:=m_dCurrencyBaseXrate)

                    'Convert from base to transaction currency

                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iDefinedCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cThisPremium)

                    'Assign the defined currency premium to the interface
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisPremiumDefCurr, vControlValue:=cThisPremium)

                End If
            End If

        Catch exc As System.Exception
        End Try

    End Sub

    Private Sub txtPremiumDefCurr_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPremiumDefCurr.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPremiumDefCurr)

    End Sub

    Private Sub txtPremiumDefCurr_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPremiumDefCurr.Leave


        Dim cThisPremium, cBaseAmount As Decimal

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPremiumDefCurr)

        Dim dTemp As Double = CDbl(m_oFormFields.UnformatControl(ctlControl:=txtPremiumDefCurr))


        Try

            Dim cTemp As Decimal = dTemp

            If Information.Err().Number <> 0 Then
                lblOverflow.Text = "Overflow"
                txtPremium.Focus()
                Exit Sub
            End If

            lblOverflow.Text = ""

            If Not m_bSysChange Then

                'Update defined currency box.
                If txtPremiumDefCurr.Visible Then
                    'Convert to base from defined currency.

                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=m_iDefinedCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cTemp)

                    'Convert from base to transaction currency

                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iTransactionCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cTemp, vConversionDate:=Nothing, vConversionRate:=m_dCurrencyBaseXrate)

                    'Assign the defined currency premium to the interface
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPremium, vControlValue:=cTemp)

                End If


                If Conversion.Val(CStr(m_oFormFields.UnformatControl(ctlControl:=txtRate2))) <> 0 Then
                    Exit Sub
                End If


                cThisPremium = Conversion.Val(CStr(m_oFormFields.UnformatControl(ctlControl:=txtPremiumDefCurr))) * m_dProRataRate


                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisPremiumDefCurr, vControlValue:=cThisPremium)

                'Update defined currency box.
                If txtPremiumDefCurr.Visible Then
                    'Convert to base from defined currency.

                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=m_iDefinedCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cThisPremium)

                    'Convert from base to transaction currency

                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iTransactionCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cThisPremium, vConversionDate:=Nothing, vConversionRate:=m_dCurrencyBaseXrate)

                    'Assign the defined currency premium to the interface
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisPremium, vControlValue:=cThisPremium)

                End If
            End If

        Catch exc As System.Exception
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtRate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        If Not m_bSysChange AndAlso m_bFormLoaded Then

            'Calculate the premium
            m_lReturn = CalculatePremiumAndDisplay()

        End If

    End Sub

    Private Sub txtRate_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtRate.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim sRate As String = ""
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If

        If Not ((KeyAscii >= Strings.Asc(CStr(0)(0)) And KeyAscii <= Strings.Asc(CStr(9)(0))) Or KeyAscii = 46 Or KeyAscii = 45) Then
            KeyAscii = 0
        End If
        If txtRate.Text.IndexOf("."c) >= 0 And KeyAscii = 46 Then KeyAscii = 0
        If txtRate.Text.IndexOf("-"c) >= 0 And KeyAscii = 45 Then KeyAscii = 0


        If txtRate.Text.IndexOf("%"c) >= 0 Then
            sRate = txtRate.Text.Substring(0, Strings.Len(txtRate.Text) - 1)
        Else
            sRate = txtRate.Text
        End If
        If sRate.Length > 8 AndAlso IsNumeric(sRate) Then
            If gPMFunctions.ToSafeCurrency(CDbl(sRate) * 1000000, 0) > 0 Then
            Else
                KeyAscii = 0
            End If
        Else
            sRate = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtRate_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtRate.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim sRate As String = ""
        If txtRate.Text.IndexOf("%"c) >= 0 Then
            sRate = txtRate.Text.Substring(0, Strings.Len(txtRate.Text) - 1)
        Else
            sRate = txtRate.Text
        End If

        If gPMFunctions.ToSafeCurrency(sRate) = 0 Then
            txtRate.Text = "0.00"
        End If
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub txtRate2_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate2.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        If Not m_bSysChange AndAlso m_bFormLoaded Then

            'Calculate the premium
            m_lReturn = CalculatePremiumAndDisplay()

        End If

    End Sub


    Private Sub txtRate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate.Enter

        txtRate.SelectionStart = 0
        txtRate.SelectionLength = Strings.Len(txtRate.Text)

        '    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRate)

    End Sub

    Private Sub txtRate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate.Leave

        '    m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRate)
        Dim number As Decimal
        If txtRate.Text <> "" Then
            If txtRate.Text.EndsWith("%") Then
                txtRate.Text = txtRate.Text.Substring(0, Strings.Len(txtRate.Text) - 1)
            End If
        End If
        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(txtRate.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            txtRate.Text = ""
        ElseIf (txtRate.Text > "") Then
            number = CStr(Conversion.Val(txtRate.Text))
            txtRate.Text = number.ToString()
            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtRate.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                txtRate.Text = ""
            Else
                txtRate.Text = StringsHelper.Format(txtRate.Text, "##0.000000").Trim() & "%"
            End If
        ElseIf txtRate.Text = "0.000000%" Then
            txtRate.Text = ""
        End If

    End Sub

    Private Sub txtRate2_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate2.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRate2)
    End Sub

    Private Sub txtRate2_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtRate2.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim sRate As String = ""
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        If Not ((KeyAscii >= Strings.Asc(CStr(0)(0)) And KeyAscii <= Strings.Asc(CStr(9)(0))) Or KeyAscii = 46 Or KeyAscii = 45) Then
            KeyAscii = 0
        End If
        If txtRate.Text.IndexOf("."c) >= 0 And KeyAscii = 46 Then KeyAscii = 0
        If txtRate.Text.IndexOf("-"c) >= 0 And KeyAscii = 45 Then KeyAscii = 0
        If txtRate2.Text.IndexOf("%"c) >= 0 Then
            sRate = txtRate2.Text.Substring(0, Strings.Len(txtRate2.Text) - 1)
        Else
            sRate = txtRate2.Text
        End If
        If sRate.Length > 8 Then
            If gPMFunctions.ToSafeCurrency(CDbl(sRate) * 1000000) > 0 Then
            Else
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtRate2_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate2.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRate2)


        Dim cRate As Decimal = CDec(m_oFormFields.UnformatControl(ctlControl:=txtRate2))

        If cRate = 0 Then
            txtPremium.Enabled = True
            txtPremiumDefCurr.Enabled = True
        Else
            txtPremium.Enabled = False
            txtPremiumDefCurr.Enabled = False

            'Calculate the premium
            m_lReturn = CalculatePremiumAndDisplay()

        End If

    End Sub

    Private Sub txtRate2_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtRate2.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        If gPMFunctions.ToSafeCurrency(txtRate2.Text) = 0 Then
            txtRate2.Text = "0.00"
        End If
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub txtRate3_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate3.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        Dim sItemCaption As String = gPMFunctions.ToSafeString(cboRateType.ItemCaption)
        If sItemCaption.ToUpper() <> "NO RATING" Then
            If Not m_bSysChange AndAlso m_bFormLoaded Then
                'Calculate the premium
                m_lReturn = CalculatePremiumAndDisplay()
            End If
        End If
    End Sub

    Private Sub txtRate3_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate3.Enter

        txtRate3.SelectionStart = 0
        txtRate3.SelectionLength = Strings.Len(txtRate3.Text)

    End Sub

    Private Sub txtRate3_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtRate3.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim sRate As String = ""
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        If txtRate3.Text.IndexOf("%"c) >= 0 Then
            sRate = txtRate3.Text.Substring(0, Strings.Len(txtRate3.Text) - 1)
        Else
            sRate = txtRate3.Text
        End If
        If Not ((KeyAscii >= Strings.Asc(CStr(0)(0)) And KeyAscii <= Strings.Asc(CStr(9)(0))) Or KeyAscii = 46 Or KeyAscii = 45) Then
            KeyAscii = 0
        End If
        If txtRate3.Text.IndexOf("."c) >= 0 And KeyAscii = 46 Then KeyAscii = 0
        If txtRate3.Text.IndexOf("-"c) >= 0 And KeyAscii = 45 Then KeyAscii = 0

        If sRate.Length > 8 Then
            If gPMFunctions.ToSafeCurrency(CDbl(sRate) * 1000000) > 0 Then
            Else
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtRate3_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate3.Leave

        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(txtRate3.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            txtRate3.Text = ""
        ElseIf (txtRate3.Text > "") Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtRate3.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                txtRate3.Text = ""
            Else
                txtRate3.Text = StringsHelper.Format(txtRate3.Text, "##0.0000").Trim()
            End If
        ElseIf txtRate3.Text = "0.0000" Then
            txtRate3.Text = ""
        End If

    End Sub

    Private Sub txtRate3_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtRate3.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        If gPMFunctions.ToSafeCurrency(txtRate3.Text) = 0 Then
            txtRate3.Text = "0.00"
        End If
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub txtSumInsured_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSumInsured.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'PN: 62349
        'Change by: Nitesh Dwivedi
        'Purpose: Written If condition To Avoid Re-Calculation if there is no change in amount
        If StringsHelper.Format(m_dOldSumInsured, "00.00") <> StringsHelper.Format(txtSumInsured.Text, "00.00") Then
            If Not m_bSysChange AndAlso m_bFormLoaded Then
                'Calculate the premium
                m_lReturn = CalculatePremiumAndDisplay()
            End If
        End If
    End Sub

    Private Sub txtSumInsured_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSumInsured.Enter
        'PN: 62349
        'Change by: Krishna Nand
        'Purpose: to remove the comma from Sum Insured
        m_dOldSumInsured = gPMFunctions.ToSafeDouble(txtSumInsured.Text.Replace(",", ""))
        txtSumInsured.Text = txtSumInsured.Text.Replace(",", "")

        'End of Change by Krishna Nand on 20-01-2010

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSumInsured)

    End Sub

    Private Sub txtSumInsured_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtSumInsured.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        If Not ((KeyAscii >= Strings.Asc(CStr(0)(0)) And KeyAscii <= Strings.Asc(CStr(9)(0))) Or KeyAscii = 46 Or KeyAscii = 45) Then
            KeyAscii = 0
        End If
        If txtSumInsured.Text.IndexOf("."c) >= 0 And KeyAscii = 46 Then KeyAscii = 0
        If txtSumInsured.Text.IndexOf("-"c) >= 0 And KeyAscii = 45 Then KeyAscii = 0
        If Strings.Len(txtSumInsured.Text) > 8 Then
            If gPMFunctions.ToSafeCurrency(CDbl(txtSumInsured.Text) * 1000000) > 0 Then
            Else
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtSumInsured_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSumInsured.Leave


        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSumInsured)


        Dim dTemp As Double = CDbl(m_oFormFields.UnformatControl(ctlControl:=txtSumInsured))


        Try

            Dim cTemp As Decimal = dTemp

            If Information.Err().Number <> 0 Then
                lblOverflow.Text = "Overflow"
                txtSumInsured.Focus()
                Exit Sub
            End If

            lblOverflow.Text = ""

        Catch exc As System.Exception
        End Try
    End Sub

    Private Sub txtSumInsured_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtSumInsured.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        If gPMFunctions.ToSafeCurrency(txtSumInsured.Text) = 0 Then
            txtSumInsured.Text = "0.00"
        End If
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub txtThisPremium_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisPremium.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_lReturn = CheckPremiumOverride()
    End Sub

    Private Sub txtThisPremium_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisPremium.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtThisPremium)

    End Sub

    Private Sub txtThisPremium_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisPremium.Leave

        Dim cBaseAmount As Decimal

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtThisPremium)


        Dim dTemp As Double = CDbl(m_oFormFields.UnformatControl(ctlControl:=txtThisPremium))


        Try

            Dim cTemp As Decimal = dTemp

            If Information.Err().Number <> 0 Then
                lblOverflow.Text = "Overflow"
                txtThisPremium.Focus()
                Exit Sub
            End If

            lblOverflow.Text = ""

            If Not m_bSysChange Then
                'Update defined currency box.
                If txtPremiumDefCurr.Visible Then
                    'Convert to base from defined currency.

                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=m_iTransactionCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cTemp, vConversionRate:=m_dCurrencyBaseXrate)

                    'Convert from base to transaction currency

                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iDefinedCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cTemp)

                    'Assign the defined currency premium to the interface
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisPremiumDefCurr, vControlValue:=cTemp)

                End If
            End If

        Catch exc As System.Exception
        End Try
    End Sub

    Private Sub txtThisPremiumDefCurr_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisPremiumDefCurr.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtThisPremiumDefCurr)

    End Sub

    Private Sub txtThisPremiumDefCurr_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisPremiumDefCurr.Leave

        Dim cBaseAmount As Decimal

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtThisPremiumDefCurr)


        Dim dTemp As Double = CDbl(m_oFormFields.UnformatControl(ctlControl:=txtThisPremiumDefCurr))


        Try

            Dim cTemp As Decimal = dTemp

            If Information.Err().Number <> 0 Then
                lblOverflow.Text = "Overflow"
                txtThisPremiumDefCurr.Focus()
                Exit Sub
            End If

            lblOverflow.Text = ""

            If Not m_bSysChange Then
                'Update defined currency box.
                If txtPremiumDefCurr.Visible Then
                    'Convert to base from defined currency.

                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=m_iDefinedCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cTemp)

                    'Convert from base to transaction currency

                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iTransactionCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cTemp, vConversionDate:=Nothing, vConversionRate:=m_dCurrencyBaseXrate)

                    'Assign the defined currency premium to the interface
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisPremium, vControlValue:=cTemp)

                End If
            End If

        Catch exc As System.Exception
        End Try
    End Sub


    ' ***************************************************************** '
    '
    ' Name: ValidateOK
    '
    ' Description: return PMTrue if data required for this form is valid
    '
    ' History: 17/04/2002 Thinh Nguyen - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateOK() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If cboRatingSectionType.Text = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW170502 - The validation of Policy Section type is not required as this is not used
            '            (as advised by John White and Sarah Johnstone) - I have only commented this
            '            out because it seems to be a matter for contention. Thinh was advised to
            '            put this validation in because it is required.


            If cboRateType.ItemCaption = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOK Failed", vApp:=ACApp, vClass:="frmDetail", vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ShowExtraFields() As Integer

        Dim result As Integer = 0
        Dim lRateTypeId As Integer
        Dim sCaption As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        If Strings.Len(cboRateType.ItemCaption) Then
            lRateTypeId = cboRateType.ItemData(cboRateType.ListIndex)
        Else
            lRateTypeId = 0
        End If

        Dim sRateType As String = GetRateType(lRateTypeId)

        Dim bShowExtraFields As Boolean = (sRateType = "V" Or sRateType = "Q") And m_iDefinedCurrencyID <> 0 And m_iDefinedCurrencyID <> m_iTransactionCurrencyID

        If bShowExtraFields Then
            'Add transaction currency to end of rate caption

            sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRateLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblRate.Text = sCaption & " (" & m_sTransactionCurrencyCode & ")"

            'Show the extra fields
            lblDefinedCurrency.Visible = True
            lblTransactionCurrency.Visible = True
            txtPremiumDefCurr.Visible = True
            txtThisPremiumDefCurr.Visible = True

            txtPremium.Left = lblTransactionCurrency.Left
            txtThisPremium.Left = lblTransactionCurrency.Left
        Else
            'Remove transaction currency at end of rate caption

            sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRateLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblRate.Text = sCaption

            'Hide the extra fields
            lblDefinedCurrency.Visible = False
            lblTransactionCurrency.Visible = False
            txtPremiumDefCurr.Visible = False
            txtThisPremiumDefCurr.Visible = False

            txtPremium.Left = lblDefinedCurrency.Left
            txtThisPremium.Left = lblDefinedCurrency.Left
        End If

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckPremiumOverride
    '
    ' Description:
    '
    ' History: 24/10/2005 RKS - Created.
    '
    ' ***************************************************************** '
    Public Function CheckPremiumOverride() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'if in edit mode (compare this_premium with calculated_premium)

            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtThisPremium.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Or Not m_bFormLoaded Then 'PN 37503 (RC)
                Return result
            End If

            If m_lMode = gPMConstants.PMEComponentAction.PMEdit Then
                If Math.Round(m_cCalculatedPremium, 2) <> Math.Round(gPMFunctions.ToSafeCurrency(txtThisPremium.Text), 2) Then
                    'this premium is overridden
                    m_iIsAmended = 1
                    txtOverrideReason.Enabled = True
                    txtOverrideReason.Text = m_sOverrideReason
                    'Developer Guide No. 168
                    _StatusBar1_Panel1.Text = "WARNING: Premium has been amended"
                Else
                    m_iIsAmended = 0

                    txtOverrideReason.Enabled = False
                    txtOverrideReason.Text = ""
                    'Developer Guide No. 168
                    _StatusBar1_Panel1.Text = ""
                End If
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPremiumOverride Failed", vApp:=ACApp, vClass:=Me, vMethod:="CheckPremiumOverride", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub frmDetail_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
    End Sub
End Class
