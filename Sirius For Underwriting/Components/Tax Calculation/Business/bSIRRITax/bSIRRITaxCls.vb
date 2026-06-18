Option Strict Off
Option Explicit On
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 03/01/2001
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a TaxBandRate.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 10/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Database Class (Private)
    'Developer Guide No. 20
    Public m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPMAuthorityLevel As Integer

    ' Primary Keys to work with
    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskCnt As Integer
    Private m_sApplyMTATaxRatesonRen As String = ""


    Public WriteOnly Property Task() As Integer
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property RiskCnt() As Integer
        Get
            Return m_lRiskCnt
        End Get
        Set(ByVal Value As Integer)
            m_lRiskCnt = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public Property ApplyMTATaxRatesonRen() As String
        Get
            Return m_sApplyMTATaxRatesonRen
        End Get
        Set(ByVal Value As String)
            m_sApplyMTATaxRatesonRen = Value
        End Set
    End Property

    ''' <summary>
    ''' Determines whether we should apply taxes
    ''' </summary>
    ''' <param name="v_lInsFileCnt"></param>
    ''' <param name="v_lRiskCnt"></param>
    ''' <param name="r_bApplyTaxes"></param>
    ''' <param name="r_bTaxesSwitchedOff"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ApplyTaxes(ByVal v_lInsFileCnt As Integer, ByVal v_lRiskCnt As Integer,
                               ByRef r_bApplyTaxes As Boolean, ByRef r_bTaxesSwitchedOff As Boolean) As Integer

        Dim nResult As Integer
        Dim sOptionValue As String = ""

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            m_lInsuranceFileCnt = v_lInsFileCnt
            ' Check system option to see whether taxes are to be applied at all across the system.
            m_lReturn = CType(GetSystemOption(ACApplyTaxesSystemOptionNumber, sOptionValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Does system option specify taxes to be suppressed
            If Val(sOptionValue) = 0 Then
                r_bApplyTaxes = False
                r_bTaxesSwitchedOff = True
                Return nResult
            Else
                r_bTaxesSwitchedOff = False
            End If

            ' If we are still going, check the product next.
            m_lReturn = CType(ApplyTaxesToProduct(v_lInsFileCnt, r_bApplyTaxes), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Not r_bApplyTaxes Then
                Return nResult
            End If

            ' If we are still going, finally check the risk if it is present.
            If v_lRiskCnt > 0 Then
                ' Check taxes should be applied to risk.
                m_lReturn = CType(ApplyTaxesToRisk(v_lRiskCnt, r_bApplyTaxes), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return nResult

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ApplyTaxes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyTaxes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ApplyTaxesToProduct
    ' ***************************************************************** '
    Private Function ApplyTaxesToProduct(ByRef lInsFileCnt As Integer, ByRef bApplyTaxes As Boolean) As Integer

        Dim result As Integer = 0
        Dim vApplyTax(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", lInsFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACTaxAppliedToProductSQL, sSQLName:=ACTaxAppliedToProductName, bStoredProcedure:=ACTaxAppliedToProductStored, vResultArray:=vApplyTax)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Inverse logic (is_suppress_tax)

        bApplyTaxes = Not (CDbl(vApplyTax(0, 0)) = 1)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ApplyTaxesToRisk
    ' ***************************************************************** '
    Private Function ApplyTaxesToRisk(ByRef lRiskCnt As Integer, ByRef bApplyTaxes As Boolean) As Integer

        Dim result As Integer = 0
        Dim vApplyTax(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACTaxAppliedToRiskSQL, sSQLName:=ACTaxAppliedToRiskName, bStoredProcedure:=ACTaxAppliedToRiskStored, vResultArray:=vApplyTax)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Inverse logic (is_suppress_tax)

        bApplyTaxes = Not (CDbl(vApplyTax(0, 0)) = 1)

        Return result

    End Function
    Public Function CalculateTax(ByVal vPremium As Decimal, ByVal vSumInsured As Decimal, ByVal vSumInsuredChange As Decimal, ByVal vRunningTotal As Decimal,
        ByVal vCalcBasis As Integer, ByVal vIsValue As Boolean, ByVal vPercentage As Double, ByVal vFixedRate As Decimal, ByVal vBasisValue As Decimal, ByVal vIsRounded As Boolean,
        ByVal vAllowTaxCredit As Boolean, ByRef rTaxValue As Decimal) As Integer

        Return CalculateTax(vPremium:=vPremium, vSumInsured:=vSumInsured, vSumInsuredChange:=vSumInsuredChange, vRunningTotal:=vRunningTotal, vCalcBasis:=vCalcBasis, vIsValue:=vIsValue, vPercentage:=vPercentage,
        vFixedRate:=vFixedRate, vBasisValue:=vBasisValue, vIsRounded:=vIsRounded, vAllowTaxCredit:=vAllowTaxCredit, rTaxValue:=rTaxValue, vCurrencyId:=0)

    End Function
    ''' <summary>
    ''' CalculateTax
    ''' </summary>
    ''' <param name="vPremium"></param>
    ''' <param name="vSumInsured"></param>
    ''' <param name="vSumInsuredChange"></param>
    ''' <param name="vRunningTotal"></param>
    ''' <param name="vCalcBasis"></param>
    ''' <param name="vIsValue"></param>
    ''' <param name="vPercentage"></param>
    ''' <param name="vFixedRate"></param>
    ''' <param name="vBasisValue"></param>
    ''' <param name="vIsRounded"></param>
    ''' <param name="vAllowTaxCredit"></param>
    ''' <param name="rTaxValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CalculateTax(ByVal vPremium As Decimal,
                                 ByVal vSumInsured As Decimal,
                                 ByVal vSumInsuredChange As Decimal,
                                 ByVal vRunningTotal As Decimal,
                                 ByVal vCalcBasis As Integer,
                                 ByVal vIsValue As Boolean,
                                 ByVal vPercentage As Double,
                                 ByVal vFixedRate As Decimal,
                                 ByVal vBasisValue As Decimal,
                                 ByVal vIsRounded As Boolean,
                                 ByVal vAllowTaxCredit As Boolean,
                                 ByRef rTaxValue As Decimal,
                                 ByVal vCurrencyId As Integer) As Integer




        Dim nResult As Integer = 0
        Const kMethodName As String = "CalculateTax"

        Try


            nResult = PMEReturnCode.PMTrue
            Dim nRoundToPlaces As Integer = 0
            Dim sEnableDecimalsSuppressionValue As String = "0"

            'Get The decimal suppression Setting from product option
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:="", v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableDecimalsSuppression, v_vBranch:=1, r_vUnderwriting:=sEnableDecimalsSuppressionValue)
            If sEnableDecimalsSuppressionValue = "" Then sEnableDecimalsSuppressionValue = "0"
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
                nResult = m_lReturn
            End If

            If Trim(sEnableDecimalsSuppressionValue) = "" OrElse sEnableDecimalsSuppressionValue = "0" Then
                If vCurrencyId > 0 Then
                    Dim oResultArray As Object(,) = Nothing

                    bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", vCurrencyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCurrencyDetailsSQL, sSQLName:=ACGetCurrencyDetailsName, bStoredProcedure:=ACGetCurrencyDetailsStored, vResultArray:=oResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "CalculateTax Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    If Informations.IsArray(oResultArray) Then
                        nRoundToPlaces = CInt(oResultArray(12, 0))
                    Else
                        nRoundToPlaces = 2
                    End If
                Else
                    nRoundToPlaces = 2
                End If
            End If


            ' Calculate current tax
            If vIsValue Then
                ' Calculate tax as a value
                Select Case True
                    ' Sum insured change (must have valid basis else flat rate)
                    Case vCalcBasis = ACCalcBasisSumInsuredChange And vBasisValue > 0
                        ' Rounding?
                        If sEnableDecimalsSuppressionValue = "1" Then
                            rTaxValue = gPMFunctions.ToSafeRound(vFixedRate * (vSumInsuredChange / vBasisValue), nRoundToPlaces, sEnableDecimalsSuppressionValue)
                        Else

                            If vIsRounded Then
                                rTaxValue = vFixedRate * gPMMaths.Ceiling(vSumInsuredChange / vBasisValue)
                            Else
                                rTaxValue = vFixedRate * (vSumInsuredChange / vBasisValue)
                            End If
                        End If


                        ' Sum insured (must have valid basis else flat rate)
                    Case vCalcBasis = ACCalcBasisSumInsured And vBasisValue > 0
                        ' Rounding?
                        If sEnableDecimalsSuppressionValue = "1" Then
                            rTaxValue = gPMFunctions.ToSafeRound(vFixedRate * (vSumInsured / vBasisValue), nRoundToPlaces, sEnableDecimalsSuppressionValue)
                        Else
                            If vIsRounded Then
                                rTaxValue = vFixedRate * gPMMaths.Ceiling(vSumInsured / vBasisValue)
                            Else
                                rTaxValue = vFixedRate * (vSumInsured / vBasisValue)
                            End If
                        End If


                        ' Flat rate
                    Case Else
                        rTaxValue = vFixedRate
                        If (m_sTransactionType = "MTCR" OrElse m_sTransactionType = "MTC") AndAlso vPremium = 0 Then
                            rTaxValue = 0
                        End If
                        ' if the premium is a negative amount then the
                        ' tax should also be a negative amount
                        If vPremium < 0 Then
                            If (rTaxValue > 0) Then
                                rTaxValue = -rTaxValue
                            Else
                                rTaxValue = rTaxValue
                            End If

                        End If


                End Select
            Else
                ' Calculate tax as a percentage
                Select Case vCalcBasis
                    ' Percentage of running total of premiums and prior taxes
                    Case ACCalcBasisRunningTotal
                        rTaxValue = vPercentage / 100 * vRunningTotal

                        ' Percentage of sum insured change
                    Case ACCalcBasisSumInsuredChange
                        rTaxValue = vPercentage / 100 * vSumInsuredChange

                        ' Percentage of sum insured
                    Case ACCalcBasisSumInsured
                        rTaxValue = vPercentage / 100 * vSumInsured

                        ' Percentage of premium
                    Case Else
                        rTaxValue = vPercentage / 100 * vPremium

                End Select
            End If
             'PM101299
            If sEnableDecimalsSuppressionValue = "0" Or sEnableDecimalsSuppressionValue = "" Then
                rTaxValue = Math.Round(rTaxValue, nRoundToPlaces, MidpointRounding.AwayFromZero)
            ElseIf sEnableDecimalsSuppressionValue = "1" Then
                rTaxValue = gPMFunctions.ToSafeRound(rTaxValue, nRoundToPlaces, sEnableDecimalsSuppressionValue)
            End If
            ' If tax is a credit, check this is allowed
            If (rTaxValue < 0) And Not vAllowTaxCredit Then
                rTaxValue = 0
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here


            ' This is for debugging only

        End Try
        Return nResult
    End Function

    Public Function CalculateTaxes(ByRef vTaxArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CalculateTaxes"
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Array processing
        Dim lLower, lUpper As Integer

        ' Tax groups
        Dim lCurrentGroupID, lCurrentSequence, lCurrentCOBID, lCurrentTaxBandId As Integer
        Dim cGroupTaxTotal, cSequenceTaxTotal As Decimal
        Dim lTaxCalculationCnt As Integer

        Dim cTaxValue As Decimal

        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            lCurrentTaxBandId = 0
            ' If we have no array we have nothing to do
            If Not Informations.IsArray(vTaxArray) Then
                Return result
            End If

            lLower = vTaxArray.GetLowerBound(1)
            lUpper = vTaxArray.GetUpperBound(1)
            For lCount As Integer = lLower To lUpper
                ' Check sequence

                If lCurrentSequence <> gPMFunctions.ToSafeDecimal(vTaxArray(ACRSequence, lCount)) Then

                    lCurrentSequence = gPMFunctions.ToSafeInteger(vTaxArray(ACRSequence, lCount))
                    cGroupTaxTotal += cSequenceTaxTotal
                    cSequenceTaxTotal = 0
                End If

                ' Check Group and COB

                If lCurrentGroupID <> gPMFunctions.ToSafeLong(CDbl(vTaxArray(ACRTaxGroupID, lCount))) OrElse vTaxArray(ACRClassOfBusinessID, lCount) = "" OrElse lCurrentCOBID <> gPMFunctions.ToSafeLong(CDbl(vTaxArray(ACRClassOfBusinessID, lCount))) Then

                    lCurrentGroupID = gPMFunctions.ToSafeLong(CDbl(vTaxArray(ACRTaxGroupID, lCount)))
                    If Not String.IsNullOrEmpty(vTaxArray(ACRClassOfBusinessID, lCount)) Then
                        lCurrentCOBID = gPMFunctions.ToSafeLong(CDbl(vTaxArray(ACRClassOfBusinessID, lCount)))
                    End If
                    cGroupTaxTotal = 0
                    cSequenceTaxTotal = 0
                End If

                ' Set running total

                If gPMFunctions.ToSafeDouble(vTaxArray(ACRPremium, lCount)) <> 0 AndAlso lCurrentTaxBandId <> gPMFunctions.ToSafeInteger(vTaxArray(ACRTaxBandId, lCount)) Then
                    vTaxArray(ACRRunningTotal, lCount) = gPMFunctions.ToSafeDouble(vTaxArray(ACRPremium, lCount)) + cGroupTaxTotal
                    lCurrentTaxBandId = gPMFunctions.ToSafeInteger(vTaxArray(ACRTaxBandId, lCount))
                Else
                    vTaxArray(ACRRunningTotal, lCount) = gPMFunctions.ToSafeDouble(vTaxArray(ACRPremium, lCount))
                End If

                ' Calculate current row


                'lReturn = CType(CalculateTax(gPMFunctions.ToSafeDecimal(vTaxArray(ACRPremium, lCount)), gPMFunctions.ToSafeDecimal(vTaxArray(ACRSumInsured, lCount)), gPMFunctions.ToSafeDecimal(vTaxArray(ACRSumInsuredChange, lCount)), gPMFunctions.ToSafeDecimal(vTaxArray(ACRRunningTotal, lCount)), gPMFunctions.ToSafeInteger(vTaxArray(ACRCalcBasis, lCount)), gPMFunctions.ToSafeBoolean(vTaxArray(ACRIsValue, lCount)), gPMFunctions.ToSafeDouble(vTaxArray(ACRTaxRate, lCount)), gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, vTaxArray(ACRTaxRate, lCount)), gPMFunctions.ToSafeDecimal(vTaxArray(ACRBasisValue, lCount)), gPMFunctions.ToSafeBoolean(vTaxArray(ACRIsSIRounded, lCount)), gPMFunctions.ToSafeBoolean(vTaxArray(ACRAllowTaxCredit, lCount)), cTaxValue), gPMConstants.PMEReturnCode)
                lReturn = CType(CalculateTax(gPMFunctions.ToSafeDecimal(vTaxArray(ACRPremium, lCount)), gPMFunctions.ToSafeDecimal(vTaxArray(ACRSumInsured, lCount)),
                                             gPMFunctions.ToSafeDecimal(vTaxArray(ACRSumInsuredChange, lCount)),
                                             gPMFunctions.ToSafeDecimal(vTaxArray(ACRRunningTotal, lCount)), gPMFunctions.ToSafeInteger(vTaxArray(ACRCalcBasis, lCount)),
                                             gPMFunctions.ToSafeBoolean(vTaxArray(ACRIsValue, lCount)), gPMFunctions.ToSafeDouble(vTaxArray(ACRTaxRate, lCount)),
                                             gPMFunctions.ToSafeDecimal(vTaxArray(ACRTaxRate, lCount)), gPMFunctions.ToSafeDouble(vTaxArray(ACRBasisValue, lCount)),
                                             gPMFunctions.ToSafeBoolean(vTaxArray(ACRIsSIRounded, lCount)), gPMFunctions.ToSafeBoolean(vTaxArray(ACRAllowTaxCredit, lCount)),
                                             cTaxValue, vTaxArray(ACRCurrencyID, lCount)), gPMConstants.PMEReturnCode)



                ' get tax calculation cnt

                lTaxCalculationCnt = CInt(vTaxArray(ACRPrimaryKeyTaxCnt, lCount))


                If m_iTask <> 0 Then
                    'update tax value back to the database
                    lReturn = CType(ApplyCalculatedTax(lTaxCalculationCnt, cTaxValue), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ApplyCalculationTax Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

                vTaxArray(ACRTaxValue, lCount) = cTaxValue
                ' Store tax value and increment running tax total
                cSequenceTaxTotal += cTaxValue

            Next lCount

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here
            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DeleteTaxes
    '
    ' Description: Deletes taxes for EITHER a risk or a policy
    ' ***************************************************************** '
    Public Function DeleteTaxes(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if we have a risk cnt delete the risk tax, otherwise delete the insurance file tax
            If v_lRiskCnt > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "InsuranceFileCnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                'Remove any taxes that may have been previously created
                'to prevent them from being posted.
                result = m_oDatabase.SQLAction(sSQL:=ACDelAllInsuranceFileTaxSQL, sSQLName:=ACDelAllRiskTaxName, bStoredProcedure:=ACDelAllRiskTaxStored)
            Else
                If v_lInsuranceFileCnt > 0 Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "RiskCnt", v_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                    'Remove any taxes that may have been previously created
                    'to prevent them from being posted.
                    result = m_oDatabase.SQLAction(sSQL:=ACDelAllRiskTaxSQL, sSQLName:=ACDelAllRiskTaxName, bStoredProcedure:=ACDelAllRiskTaxStored)
                End If
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteTaxes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTaxes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAllTaxes
    '
    ' Description: Deletes all taxes for a policy (both Risk and Policy)
    '              This is done when the tax functionality is switched off
    ' ***************************************************************** '
    Public Function DeleteAllTaxes(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if we have a risk cnt delete the risk tax, otherwise delete the insurance file tax
            If v_lInsuranceFileCnt > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "InsuranceFileCnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                'Remove any taxes that may have been previously created
                'to prevent them from being posted.
                result = m_oDatabase.SQLAction(sSQL:="spu_Policy_Tax_DelAll", sSQLName:="Delete All Taxes", bStoredProcedure:=True)
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAllTaxes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllTaxes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function
    ' developer guide no.101 
    Public Function GetTaxesTotalDetails(ByVal v_lInsuranceFileCnt As Object, ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0

        Const kMethodName As String = "GetTaxesTotalDetails"
        Try


            result = True


            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACTaxTaxTotalSQL, sSQLName:=ACTaxTaxTotalName, bStoredProcedure:=ACTaxTotalAtRiskLevelStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTaxesTotalDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try
    End Function

    ''' <summary>
    ''' Name: GetInsuranceFileTax
    ''' Gets policy level tax details
    ''' </summary>
    ''' <param name="r_vInsuranceFileTax"></param>
    ''' <param name="r_sDescription"></param>
    ''' <returns></returns>
    Public Function GetInsuranceFileTax(ByRef r_vInsuranceFileTax As Object,
                                         ByRef r_sDescription As String) As Integer
        Return GetInsuranceFileTax(r_vInsuranceFileTax:=r_vInsuranceFileTax,
                                r_sDescription:=r_sDescription,
                                iTask:=0,
                                v_sTransType:="")

    End Function

    Public Function GetInsuranceFileTax(ByRef r_vInsuranceFileTax As Object,
                                    ByRef r_sDescription As String,
                                    ByRef iTask As Integer) As Integer
        Return GetInsuranceFileTax(r_vInsuranceFileTax:=r_vInsuranceFileTax,
                                r_sDescription:=r_sDescription,
                                iTask:=iTask,
                                v_sTransType:="")

    End Function

    Public Function GetInsuranceFileTax(ByRef r_vInsuranceFileTax As Object,
                                    ByRef r_sDescription As String,
                                    ByRef v_sTransType As String) As Integer
        Return GetInsuranceFileTax(r_vInsuranceFileTax:=r_vInsuranceFileTax,
                                        r_sDescription:=r_sDescription,
                                        iTask:=0,
                                        v_sTransType:=v_sTransType)
    End Function

    Public Function GetInsuranceFileTax(ByRef r_vInsuranceFileTax As Object,
                                        ByRef r_sDescription As String,
                                        ByRef iTask As Integer,
                                        ByRef v_sTransType As String) As Integer


        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Const s_kMethodName As String = "GetInsuranceFileTax"

        Dim nReturn As gPMConstants.PMEReturnCode
        Dim vArray(,) As Object = Nothing

        Try

            If v_sTransType = "MTA" Then
                ' recalculate policy taxes
                nReturn = RecalculatePolicyTaxes(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                 v_lTask:=CLng(m_iTask),
                                                 v_sTransactionType:=v_sTransType,
                                                 r_vInsuranceFileTax:=r_vInsuranceFileTax)
            Else
                ' recalculate policy taxes

                nReturn = CType(RecalculatePolicyTaxes(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                       v_lTask:=m_iTask,
                                                       v_sTransactionType:=m_sTransactionType,
                                                       r_vInsuranceFileTax:=r_vInsuranceFileTax), gPMConstants.PMEReturnCode)
            End If
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(s_kMethodName, "RecalculatePolicyTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Return the insurance reference
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_Cnt", m_lInsuranceFileCnt,
                                             gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsuranceRefSQL,
                                              sSQLName:=ACGetInsuranceRefName,
                                              bStoredProcedure:=ACGetInsuranceRefStored,
                                              vResultArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(s_kMethodName, "GetInsuranceFileRef Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vArray) Then

                r_sDescription = CStr(vArray(0, 0))
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=s_kMethodName, r_lFunctionReturn:=nResult, excep:=excep)

            Return nResult

        End Try


    End Function




    ' ***************************************************************** '
    ' Name: GetRiskTax
    '
    ' Parameters: n/a
    '
    ' Description: Gets risk level tax details
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function GetRiskTax(ByRef r_vRiskTax As Object, ByRef r_sDescription As String) As Integer
        Return GetRiskTax(r_vRiskTax:=r_vRiskTax, r_sDescription:=r_sDescription, iTask:=0)
    End Function

    Public Function GetRiskTax(ByRef r_vRiskTax As Object, ByRef r_sDescription As String, ByRef iTask As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRiskTax"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' recalculate policy risk taxesz

            lReturn = CType(RecalculatePolicyRiskTaxes(v_lRiskCnt:=m_lRiskCnt, v_lTask:=m_iTask, v_sTransactionType:=m_sTransactionType, r_vRiskTax:=r_vRiskTax), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RecalculatePolicyRiskTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Return the description
            bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Cnt", m_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskDescriptionSQL, sSQLName:=ACGetRiskDescriptionName, bStoredProcedure:=ACGetRiskDescriptionStored, vResultArray:=vArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetRiskDescription Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vArray) Then

                r_sDescription = CStr(vArray(0, 0))
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetInsuranceFileTaxWithoutRecalculation
    '
    ' Parameters: n/a
    '
    ' Description: Gets policy level tax details
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function GetInsuranceFileTaxWithoutRecalculation(ByRef r_vInsuranceFileTax As Object, ByRef r_sDescription As String) As Integer
        Return GetInsuranceFileTaxWithoutRecalculation(r_vInsuranceFileTax:=r_vInsuranceFileTax, r_sDescription:=r_sDescription, iTask:=0)
    End Function

    Public Function GetInsuranceFileTaxWithoutRecalculation(ByRef r_vInsuranceFileTax As Object, ByRef r_sDescription As String, ByRef iTask As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInsuranceFileTaxWithoutRecalculation"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get policy taxes straight from db with no recalculation
            lReturn = CType(GetExistingInsuranceFileTax(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vPolicyTaxes:=r_vInsuranceFileTax), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetExistingInsuranceFileTax Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Return the insurance reference
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_Cnt", m_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsuranceRefSQL, sSQLName:=ACGetInsuranceRefName, bStoredProcedure:=ACGetInsuranceRefStored, vResultArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInsuranceFileRef Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vArray) Then

                r_sDescription = CStr(vArray(0, 0))
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetTaxNotAppliedToClient(ByVal lInsuranceFileCnt As Integer, ByRef r_cTaxNotAppliedToClient As Decimal) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetTaxNotAppliedToClient
        ' PURPOSE: Returns the amount of Tax which is not going to be applied to
        '          the client. This is needed for calculating the Financed Amount on
        '          Instalments.
        ' AUTHOR: Danny Davis
        ' DATE: 04 November 2005, 13:07:42
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            'developer guide no 85
            bPMAddParameter.AddParameterLite(m_oDatabase, "tax_amount", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency, False)

            m_lReturn = m_oDatabase.SQLAction("spu_SIR_Get_TaxNotAppliedToClient", "Get Tax Not Applied to Client", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", Error running spu_SIR_Get_TaxNotAppliedToClient")
            End If

            r_cTaxNotAppliedToClient = gPMFunctions.ToSafeCurrency(m_oDatabase.Parameters.Item("tax_amount").Value)


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaxNotAppliedToClient", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function


    '********************************************************************************
    ' Name: GetSystemOption
    '
    ' Description: Get system options via central call. Masks additional fields.
    '********************************************************************************
    Public Function GetSystemOption(ByVal v_iOptionNumber As Integer, ByRef r_sResult As String) As Integer

        Try

            ' Delegate to common call

            Return bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_iOptionNumber, r_sResult, m_iSourceID)

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this object.
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Check database

            m_lReturn = CType(gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set defaults
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this object.
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' *****************************************************************
    ' Name: UpdateInsuranceFileTax (Public)
    ' ***************************************************************** '
    Public Function UpdateInsuranceFileTax(ByVal v_vInsuranceFileTax(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Process all taxes
            For lRow As Integer = v_vInsuranceFileTax.GetLowerBound(1) To v_vInsuranceFileTax.GetUpperBound(1)
                ' Change: Always save the tax as we now calculate here

                ' Add parameters

                bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", v_vInsuranceFileTax(ACRParentCnt, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                bPMAddParameter.AddParameterLite(m_oDatabase, "tax_band_id", v_vInsuranceFileTax(ACRTaxBandId, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "premium", v_vInsuranceFileTax(ACRPremium, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                bPMAddParameter.AddParameterLite(m_oDatabase, "percentage", v_vInsuranceFileTax(ACRTaxRate, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                bPMAddParameter.AddParameterLite(m_oDatabase, "value", v_vInsuranceFileTax(ACRTaxValue, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                bPMAddParameter.AddParameterLite(m_oDatabase, "is_value", v_vInsuranceFileTax(ACRIsValue, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "is_manually_changed", v_vInsuranceFileTax(ACRIsManuallyChanged, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "basis_value", v_vInsuranceFileTax(ACRBasisValue, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                bPMAddParameter.AddParameterLite(m_oDatabase, "calc_basis", v_vInsuranceFileTax(ACRCalcBasis, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", v_vInsuranceFileTax(ACRSumInsured, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured_rounded", v_vInsuranceFileTax(ACRIsSIRounded, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", v_vInsuranceFileTax(ACRCurrencyID, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "allow_tax_credit", v_vInsuranceFileTax(ACRAllowTaxCredit, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "original_sum_insured", v_vInsuranceFileTax(ACROriginalSumInsured, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)


                Dim dbNumericTemp As Double
                If (v_vInsuranceFileTax(ACRCountryID, lRow) Is DBNull.Value) Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "country_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                Else
                    bPMAddParameter.AddParameterLite(m_oDatabase, "country_id", (If(Double.TryParse(CStr(v_vInsuranceFileTax(ACRCountryID, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp), v_vInsuranceFileTax(ACRCountryID, lRow), DBNull.Value)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If




                Dim dbNumericTemp2 As Double
                If v_vInsuranceFileTax(ACRStateID, lRow) Is DBNull.Value Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "state_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                Else
                    bPMAddParameter.AddParameterLite(m_oDatabase, "state_id", (If(Double.TryParse(CStr(v_vInsuranceFileTax(ACRStateID, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2), v_vInsuranceFileTax(ACRStateID, lRow), DBNull.Value)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If



                Dim dbNumericTemp3 As Double
                If v_vInsuranceFileTax(ACRClassOfBusinessID, lRow) Is DBNull.Value Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "class_of_business_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                Else
                    bPMAddParameter.AddParameterLite(m_oDatabase, "class_of_business_id", (If(Double.TryParse(CStr(v_vInsuranceFileTax(ACRClassOfBusinessID, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3), v_vInsuranceFileTax(ACRClassOfBusinessID, lRow), DBNull.Value)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If

                bPMAddParameter.AddParameterLite(m_oDatabase, "tax_group_id", v_vInsuranceFileTax(ACRTaxGroupID, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "sequence", v_vInsuranceFileTax(ACRSequence, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", v_vInsuranceFileTax(ACRIsDeleted, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "tax_calculation_cnt", v_vInsuranceFileTax(ACRPrimaryKeyTaxCnt, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "is_not_applied_to_client", v_vInsuranceFileTax(ACRIsNotApplied, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "include_tax_in_instalments", v_vInsuranceFileTax(ACRIncludeIns, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "spread_tax_across_instalments", v_vInsuranceFileTax(ACRSpread, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "apply_tax_by", v_vInsuranceFileTax(ACRApplyTaxBy, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) '(RC)


                ' Call update, this will automatically delete if necessary!
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateInsuranceFileTaxSQL, sSQLName:=ACUpdateInsuranceFileTaxName, bStoredProcedure:=ACUpdateInsuranceFileTaxStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = m_oDatabase.SQLRollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next lRow

            ' Commit the transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInsuranceFileTax  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileTax ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            result = gPMConstants.PMEReturnCode.PMError

            ' Rollback transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            Return result
        End Try
    End Function

    ' *****************************************************************
    ' Name: UpdateRiskTax (Public)
    ' ***************************************************************** '
    Public Function UpdateRiskTax(ByVal v_vRiskTax(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(v_vRiskTax) Then
                'Nothing to do
                Return result
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Process all taxes
            For lRow As Integer = v_vRiskTax.GetLowerBound(1) To v_vRiskTax.GetUpperBound(1)
                ' Change: Always save the tax as we now calculate here

                ' Add parameters

                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", v_vRiskTax(ACRParentCnt, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                bPMAddParameter.AddParameterLite(m_oDatabase, "tax_band_id", v_vRiskTax(ACRTaxBandId, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "premium", v_vRiskTax(ACRPremium, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                bPMAddParameter.AddParameterLite(m_oDatabase, "percentage", v_vRiskTax(ACRTaxRate, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                bPMAddParameter.AddParameterLite(m_oDatabase, "value", v_vRiskTax(ACRTaxValue, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                bPMAddParameter.AddParameterLite(m_oDatabase, "is_value", v_vRiskTax(ACRIsValue, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "is_manually_changed", v_vRiskTax(ACRIsManuallyChanged, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "basis_value", v_vRiskTax(ACRBasisValue, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                bPMAddParameter.AddParameterLite(m_oDatabase, "calc_basis", v_vRiskTax(ACRCalcBasis, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", v_vRiskTax(ACRSumInsured, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured_rounded", v_vRiskTax(ACRIsSIRounded, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", v_vRiskTax(ACRCurrencyID, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "allow_tax_credit", v_vRiskTax(ACRAllowTaxCredit, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "original_sum_insured", v_vRiskTax(ACROriginalSumInsured, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)


                Dim dbNumericTemp As Double
                If v_vRiskTax(ACRCountryID, lRow) Is DBNull.Value Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "country_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                Else
                    bPMAddParameter.AddParameterLite(m_oDatabase, "country_id", (If(Double.TryParse(CStr(v_vRiskTax(ACRCountryID, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp), v_vRiskTax(ACRCountryID, lRow), DBNull.Value)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If



                Dim dbNumericTemp2 As Double
                If v_vRiskTax(ACRStateID, lRow) Is DBNull.Value Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "state_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                Else
                    bPMAddParameter.AddParameterLite(m_oDatabase, "state_id", (If(Double.TryParse(CStr(v_vRiskTax(ACRStateID, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2), v_vRiskTax(ACRStateID, lRow), DBNull.Value)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If



                Dim dbNumericTemp3 As Double
                If v_vRiskTax(ACRClassOfBusinessID, lRow) Is DBNull.Value OrElse v_vRiskTax(ACRClassOfBusinessID, lRow) = "" Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "class_of_business_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                Else
                    bPMAddParameter.AddParameterLite(m_oDatabase, "class_of_business_id", (If(Double.TryParse(CStr(v_vRiskTax(ACRClassOfBusinessID, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3), v_vRiskTax(ACRClassOfBusinessID, lRow), DBNull.Value)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If


                bPMAddParameter.AddParameterLite(m_oDatabase, "tax_group_id", v_vRiskTax(ACRTaxGroupID, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "sequence", v_vRiskTax(ACRSequence, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", v_vRiskTax(ACRIsDeleted, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                bPMAddParameter.AddParameterLite(m_oDatabase, "tax_calculation_cnt", v_vRiskTax(ACRPrimaryKeyTaxCnt, lRow), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "apply_tax_by", gPMFunctions.ToSafeLong(v_vRiskTax(ACRApplyTaxBy, lRow)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) '(RC)

                ' Call update, this will automatically delete if necessary!
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskTaxSQL, sSQLName:=ACUpdateRiskTaxName, bStoredProcedure:=ACUpdateRiskTaxStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = m_oDatabase.SQLRollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next lRow

            ' Commit the transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskTax ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            result = gPMConstants.PMEReturnCode.PMError

            ' Rollback transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            Return result
        End Try
    End Function

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: ApplyCalculatedTax
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 16-05-2005 : AUS005
    ' ***************************************************************** '
    Private Function ApplyCalculatedTax(ByVal v_lTaxCalculationCnt As Integer, ByRef v_crTaxValue As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ApplyCalculatedTax"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters

        ' tax calculation
        bPMAddParameter.AddParameterLite(m_oDatabase, "tax_calculation_cnt", v_lTaxCalculationCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        ' tax value
        bPMAddParameter.AddParameterLite(m_oDatabase, "tax_value", v_crTaxValue, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMCurrency)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kApplyCalculatedTaxSQL, sSQLName:=kApplyCalculatedTaxName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kApplyCalculatedTaxSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        Else
            v_crTaxValue = gPMFunctions.ToSafeDouble(m_oDatabase.Parameters.Item("tax_value").Value, 0)
        End If

        Return result
    End Function

    ''' <summary>
    ''' Recalculate Policy taxes
    ''' </summary>
    ''' <param name="v_lRiskCnt"></param>
    ''' <param name="v_lTask"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function RecalculatePolicyRiskTaxes(ByVal v_lRiskCnt As Integer, ByVal v_lTask As Integer,
                                               ByVal v_sTransactionType As String) As Integer
        Return RecalculatePolicyRiskTaxes(v_lRiskCnt:=v_lRiskCnt, v_lTask:=v_lTask,
                                               v_sTransactionType:=v_sTransactionType,
                                               r_vRiskTax:=Nothing)
    End Function
    Public Function RecalculatePolicyRiskTaxes(ByVal v_lRiskCnt As Integer, ByVal v_lTask As Integer,
                                               ByVal v_sTransactionType As String,
                                               ByRef r_vRiskTax(,) As Object) As Integer

        Dim nResult As Integer
        Const kMethodName As String = "RecalculatePolicyRiskTaxes"
        Dim oTransType(,) As Object = Nothing
        Dim nReturn As Integer = 0

        Try

            'EM 20120819 ' Ensure that we have an insurance file if we do not have one we cannot continue as renewals allows for duplicate risks in tax_calculations
            If (m_lInsuranceFileCnt = 0) Then
                'RaiseError(kMethodName, "Missing Mandatory data - the insurance file must be set before calling this method",
                '           gPMConstants.PMELogLevel.PMLogError)
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If v_sTransactionType = "" OrElse v_sTransactionType = "NB" Then
                'Get TransactionType if it is blank
                bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Cnt", v_lRiskCnt,
                                                 gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMLong, True)
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransTypeByRiskKeySQL,
                                                  sSQLName:=ACGetTransTypeByRiskKeyxName,
                                                  bStoredProcedure:=ACGetTransTypeByRiskKeyStored,
                                                  vResultArray:=oTransType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SelectRiskTax Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If Informations.IsArray(oTransType) Then

                    v_sTransactionType = CStr(oTransType(0, 0))
                End If
            End If
            If ApplyMTATaxRatesonRen = "1" Then
                v_sTransactionType = "MTA"
            End If
            ' Get risk tax
            bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Cnt", v_lRiskCnt,
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMInteger, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Mode", v_lTask, gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMInteger)
            'developer guide no.179
            bPMAddParameter.AddParameterLite(m_oDatabase, "TransType", v_sTransactionType,
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", m_lInsuranceFileCnt,
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMInteger) ' EM Pass the insurance file cnt as well
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "RecalculateTaxes", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRiskTaxSQL, sSQLName:=ACSelectRiskTaxName,
                                              bStoredProcedure:=ACSelectRiskTaxStored, vResultArray:=r_vRiskTax)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SelectRiskTax Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                ' Calculate the tax
                m_lReturn = CType(CalculateTaxes(r_vRiskTax), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CalculateTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Not (r_vRiskTax) Is Nothing Then
                    m_lReturn = UpdateRiskTax(r_vRiskTax)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "CalculateTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: RecalculatePolicyTaxes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function RecalculatePolicyTaxes(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTask As Integer, ByVal v_sTransactionType As String) As Integer
        Return RecalculatePolicyTaxes(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lTask:=v_lTask, v_sTransactionType:=v_sTransactionType, r_vInsuranceFileTax:=Nothing)
    End Function

    Public Function RecalculatePolicyTaxes(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTask As Integer, ByVal v_sTransactionType As String, ByRef r_vInsuranceFileTax(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculatePolicyTaxes"

        Dim lReturn As gPMConstants.PMEReturnCode


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vTransType(,) As Object = Nothing
            If v_sTransactionType = "" Or v_sTransactionType = "NB" Then
                'Get TransactionType if it is blank
                bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransTypeByRiskKeySQL, sSQLName:=ACGetTransTypeByRiskKeyxName, bStoredProcedure:=ACGetTransTypeByRiskKeyStored, vResultArray:=vTransType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SelectRiskTax Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If Informations.IsArray(vTransType) Then

                    v_sTransactionType = CStr(vTransType(0, 0))
                End If
            End If

            If ApplyMTATaxRatesonRen = "1" Then
                v_sTransactionType = "MTA"
            End If
            ' Get the insurance file tax
            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Mode", v_lTask, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'developer guide no. 179
            bPMAddParameter.AddParameterLite(m_oDatabase, "TransType", v_sTransactionType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "RecalculateTaxes", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuranceFileTaxSQL, sSQLName:=ACSelectInsuranceFileTaxName, bStoredProcedure:=ACSelectInsuranceFileTaxStored, vResultArray:=r_vInsuranceFileTax)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInsuranceFileTax Entries", gPMConstants.PMELogLevel.PMLogError)
            End If
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                ' Calculate the tax
                lReturn = CType(CalculateTaxes(r_vInsuranceFileTax), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CalculateTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Not (r_vInsuranceFileTax) Is Nothing Then
                    m_lReturn = UpdateInsuranceFileTax(r_vInsuranceFileTax)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "UpdateInsuranceFileTax Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetExistingInsuranceFileTax
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 16-12-2005 : Policy / Discount
    ' ***************************************************************** '
    Private Function GetExistingInsuranceFileTax(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vPolicyTaxes(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetExistingInsuranceFileTax"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", m_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLSelect(sSQL:=kGetExistingInsuranceFileTaxSQL, sSQLName:=kGetExistingInsuranceFileTaxName, bStoredProcedure:=True, vResultArray:=r_vPolicyTaxes)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kGetExistingInsuranceFileTaxSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function



    ' ***************************************************************** '
    ' Name: RecalculateSingleRiskTax
    '
    ' Description:
    '
    ' History:
    '           Created : Rajesh Choudhary : 26-OCT-2006
    ' ***************************************************************** '
    Public Function RecalculateSingleRiskTax(ByVal v_lTaxCalculationCnt As Integer, ByVal v_lApplyTaxBy As Integer, ByVal v_sTransactionType As String) As Integer
        Return RecalculateSingleRiskTax(v_lTaxCalculationCnt:=v_lTaxCalculationCnt, v_lApplyTaxBy:=v_lApplyTaxBy, v_sTransactionType:=v_sTransactionType, r_vRiskTax:=Nothing)
    End Function

    Public Function RecalculateSingleRiskTax(ByVal v_lTaxCalculationCnt As Integer, ByVal v_lApplyTaxBy As Integer, ByVal v_sTransactionType As String, ByRef r_vRiskTax(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculateSingleRiskTax"

        Dim lReturn As Integer = 0

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get risk tax
            bPMAddParameter.AddParameterLite(m_oDatabase, "Tax_Calculation_cnt", v_lTaxCalculationCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Apply_Tax_By", v_lApplyTaxBy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'developer guide no. 179
            bPMAddParameter.AddParameterLite(m_oDatabase, "TransType", v_sTransactionType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRiskSingleTaxSQL, sSQLName:=ACSelectRiskSingleTaxName, bStoredProcedure:=True, vResultArray:=r_vRiskTax)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SelectRiskTax Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Calculate the tax
            m_lReturn = CType(CalculateTaxes(r_vRiskTax), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CalculateTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    '********************************************************************************
    ' Name: PreviewTax
    '
    ' Description: Gets Insurance Commission level tax details
    '********************************************************************************
    Public Function PreviewTax(ByVal v_lTaxGroupId As Long, ByVal v_iCurrencyId As Integer, ByVal v_cTaxableAmount As Decimal, ByVal v_dtEffectiveDate As Date, ByRef r_vTax(,) As Object) As Long

        Dim vTaxArray(,) As Object = Nothing
        Dim vTax As Object = Nothing
        Dim sSQL As String = ""
        Dim sUserId As String = ""
        Dim sTransType As String = ""
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLBeginTrans()

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return m_lReturn
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "tax_group_id", v_lTaxGroupId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_amount", v_cTaxableAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_currency_id", v_iCurrencyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "effective_date", v_dtEffectiveDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCalculateTaxPreviewSQL, sSQLName:=ACCalculateTaxPreviewName, bStoredProcedure:=ACCalculateTaxPreviewStored, vResultArray:=vTaxArray)


            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return m_lReturn
            End If


            If Informations.IsArray(vTaxArray) = True Then

                ' Calculate the tax
                m_lReturn = CalculateTaxes(vTaxArray)

                'Calculate
                'Total from the output list

                sTransType = "TEMP" & CStr(m_iUserID)
                Call AddParameterLite(m_oDatabase, "trans_type", sTransType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                sSQL = "SELECT  MAX(TT.code),Sum (TC.value) FROM Tax_Calculation TC," &
                "Tax_Band TB,Tax_Type TT Where TC.Tax_Band_ID = TB.Tax_Band_id AND TB.Tax_Type_id = TT.Tax_Type_id " &
                "AND TC.TransType = {trans_type} GROUP BY  TC.tax_band_id"



                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CalculateTaxPreview", bStoredProcedure:=False, vResultArray:=vTax)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Return m_lReturn
                Else
                    r_vTax = vTax
                End If
            End If

            m_lReturn = m_oDatabase.SQLRollbackTrans()
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            End If


        Catch ex As Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PreviewTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PreviewTax", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' Copies the taxes between two insurance files and risks
    ''' </summary>
    ''' <param name="v_lSourceRiskCnt"></param>
    ''' <param name="v_lSourceInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyRiskTax(ByVal v_lSourceRiskCnt As Long, ByVal v_lSourceInsuranceFileCnt As Long) As Long
        Const kMethodName As String = "CopyRiskTax"

        Dim nReturn As Integer = 0

        Try
            CopyRiskTax = gPMConstants.PMEReturnCode.PMTrue

            AddParameterLite(m_oDatabase, "risk_cnt", m_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput,
                             gPMConstants.PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "insurance_file_cnt", m_lInsuranceFileCnt,
                             gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "source_risk_cnt", v_lSourceRiskCnt,
                             gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "source_insurance_file_cnt", v_lSourceInsuranceFileCnt,
                             gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            nReturn = m_oDatabase.SQLAction(
                sSQL:=kCopyRiskTaxSQL,
                sSQLName:=kCopyRiskTaxName,
                bStoredProcedure:=kCopyRiskTaxStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "CopyRiskTax Failed", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskTax", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function
    ' *****************************************************************
    ' Name: UpdateRiskInTaxCalculation (Public)
    ' ***************************************************************** '
    Public Function UpdateRiskInTaxCalculation(ByVal oldRiskCnt As Integer, ByVal newRiskCnt As Integer, ByVal insuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            bPMAddParameter.AddParameterLite(m_oDatabase, "oldRisk_cnt", oldRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "newRisk_cnt", newRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", insuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' Call update, this will automatically delete if necessary!
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskInTaxCalculationSQL, sSQLName:=ACUpdateRiskInTaxCalculationSQL, bStoredProcedure:=ACUpdateRiskTaxStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return m_lReturn

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskInTaxCalculation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskInTaxCalculation ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            result = gPMConstants.PMEReturnCode.PMError
            Return result
        End Try
    End Function
End Class

