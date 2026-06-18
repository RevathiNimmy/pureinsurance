Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Creatable Form class which contains all the methods, business rules
    ' required for processing Reinsurance.
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 19/12/2003
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
    Private Const ACClass As String = "Form"

    ' Database Class (Private)
    'Developer Guide No. 20
    Public m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New StringsHelper.FixedLengthString(2)
    Private m_sMapStatus As New StringsHelper.FixedLengthString(2)
    Private m_sStepStatus As New StringsHelper.FixedLengthString(2)

    ' Primary Keys to work with
    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskCnt As Integer

    ' Collection of reinsurance
    Private m_oReinsurances As bSIRReinsurance.Reinsurances

    Private m_lTMPRisk_cnt_under_renewal As Integer

    Private m_bApplyReinsurance As Boolean

    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get
            ' Return Number in Collection
            Return m_oReinsurances.Count()
        End Get
    End Property

    Public Property RiskID() As Integer
        Get
            Return m_lRiskCnt
        End Get
        Set(ByVal Value As Integer)
            m_lRiskCnt = Value
        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get
            ' Return the Steps Status
            Return m_sStepStatus.Value
        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get
            Return m_iTask
        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
    End Property
    Public Property TMPRiskCntUnderRenewal() As Integer
        Get
            Return m_lTMPRisk_cnt_under_renewal
        End Get
        Set(ByVal Value As Integer)
            m_lTMPRisk_cnt_under_renewal = Value
        End Set
    End Property

    Public Property ApplyReIns() As Boolean
        Get
            Return m_bApplyReinsurance
        End Get
        Set(ByVal Value As Boolean)
            m_bApplyReinsurance = Value
        End Set
    End Property


    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '

    Public Function ApplyReinsurance(ByRef r_bApplyReinsurance As Boolean) As Integer
        ' Retained to preserve class interface
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        r_bApplyReinsurance = True
        Return result
    End Function


    Public Function AutoReinsure(ByRef r_bAutoReinsure As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "AutoReinsure"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(m_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACAutoReinsureRiskSQL, sSQLName:=ACAutoReinsureRiskName, bStoredProcedure:=ACAutoReinsureRiskStored, vResultArray:=vArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Failed to check auto reinsure flag")
            End If

            ' Get result

            r_bAutoReinsure = (CDbl(vArray(0, 0)) = 1)

            ApplyReIns = r_bAutoReinsure

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Calculate facultative taxes
    ' ***************************************************************** '
    Public Function CalculateFacTax(ByVal v_lArrangementLineID As Integer, ByVal v_lPartyCnt As Integer, ByVal v_cPremium As Decimal, ByVal v_cCommission As Decimal, ByRef r_cPremiumTax As Decimal, ByRef r_cCommissionTax As Decimal) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "CalculateFacTax"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Delete existing taxes for this fac reinsurer
            lReturn = CType(DeleteTaxCalculationEntries(FACPREMIUMTAXTYPE, FACCOMMISSIONTAXTYPE, v_lArrangementLineID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DeleteTaxCalculationEntries", "Unable to delete tax entries for facultative reinsurance")
            End If

            ' Add Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CStr(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(m_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", CStr(v_lArrangementLineID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", CStr(v_lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium", CStr(v_cPremium), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission", CStr(v_cCommission), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_transtype", FACPREMIUMTAXTYPE, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_transtype", FACCOMMISSIONTAXTYPE, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_tax", CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_tax", CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLAction(sSQL:=ACCalculateFaculativeTaxSQL, sSQLName:=ACCalculateFaculativeTaxName, bStoredProcedure:=ACCalculateFaculativeTaxStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to calculate facultative taxes")
            End If

            ' Get tax amounts
            r_cPremiumTax = m_oDatabase.Parameters.Item("premium_tax").Value
            r_cCommissionTax = m_oDatabase.Parameters.Item("commission_tax").Value

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        End Try
        Return result
    End Function

    ''' <summary>
    ''' This will create or refresh an entire reinsurance arrangement for
    ''' the given risk, including the original reinsurance
    ''' </summary>
    ''' <param name="bIsPT"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CalculateRI(Optional ByVal bIsPT As Boolean = False) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "CalculateRI"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Don't calculate if this is view mode
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                ' Add parameters
                bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CStr(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(m_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "transtype", m_sTransactionType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                If bIsPT Then
                    nResult = m_oDatabase.SQLAction(sSQL:=kRefreshRISQL, sSQLName:=kRefreshRIName, bStoredProcedure:=kRefreshRIStored)
                Else
                    If m_lTMPRisk_cnt_under_renewal > 0 Then
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TMPRisk_cnt_under_renewal", m_lTMPRisk_cnt_under_renewal, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    End If
                    nResult = m_oDatabase.SQLAction(sSQL:=ACRefreshRISQL, sSQLName:=ACRefreshRIName, bStoredProcedure:=ACRefreshRIStored)
                End If

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACRefreshRISQL)
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Calculate taxes for a given treaty
    ' ***************************************************************** '
    Public Function CalculateTreatyTax(ByVal v_lArrangementLineID As Integer, ByVal v_lTreatyID As Integer, ByVal v_cPremium As Decimal, ByVal v_cCommission As Decimal, ByRef r_cPremiumTax As Decimal, ByRef r_cCommissionTax As Decimal) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "CalculateTreatyTax"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Delete existing taxes for this fac reinsurer
            lReturn = CType(DeleteTaxCalculationEntries(TREATYPREMIUMTAXTYPE, TREATYCOMMISSIONTAXTYPE, v_lArrangementLineID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DeleteTaxCalculationEntries", "Unable to delete tax entries for treaty reinsurance")
            End If

            ' Add Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CStr(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(m_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", CStr(v_lArrangementLineID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", CStr(v_lTreatyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium", CStr(v_cPremium), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission", CStr(v_cCommission), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_transtype", TREATYPREMIUMTAXTYPE, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_transtype", TREATYCOMMISSIONTAXTYPE, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_tax", CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_tax", CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACCalculateTreatyTaxSQL, sSQLName:=ACCalculateTreatyTaxName, bStoredProcedure:=ACCalculateTreatyTaxStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to calculate treaty taxes")
            End If

            ' Get tax amounts
            r_cPremiumTax = m_oDatabase.Parameters.Item("premium_tax").Value
            r_cCommissionTax = m_oDatabase.Parameters.Item("commission_tax").Value

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Cancel"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For Each oReinsurance As Reinsurance In m_oReinsurances
                Select Case oReinsurance.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next oReinsurance

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '

    ' Update risk status
    ' ***************************************************************** '
    Public Function ChangeRiskStatus() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "RiskCnt", CStr(m_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskStatusSQL, sSQLName:=ACUpdateRiskStatusName, bStoredProcedure:=ACUpdateRiskStatusStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangeRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeRiskStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Delete all tax calculations
    ' ***************************************************************** '
    Public Function DeleteTaxCalculationEntries(ByVal v_sTransTypePremium As String, ByVal v_sTransTypeCommission As String) As Integer
        Return DeleteTaxCalculationEntries(v_sTransTypePremium:=v_sTransTypePremium, v_sTransTypeCommission:=v_sTransTypeCommission, v_lArrangementLineID:=0)
    End Function

    Public Function DeleteTaxCalculationEntries(ByVal v_sTransTypePremium As String, ByVal v_sTransTypeCommission As String, ByVal v_lArrangementLineID As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "DeleteTaxCalculationEntries"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CStr(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(m_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "transtype_premium", v_sTransTypePremium, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "transtype_commission", v_sTransTypeCommission, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            If v_lArrangementLineID > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", CStr(v_lArrangementLineID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            End If

            ' Execute selection Query
            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteTaxCalculationEntriesSQL, sSQLName:=ACDeleteTaxCalculationEntriesName, bStoredProcedure:=ACDeleteTaxCalculationEntriesStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to delete reinsurance tax entries")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Update ri band details
    ' ***************************************************************** '
    Public Function EditUpdate(ByVal lRIBandID As Integer, ByRef vRILines(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vOldRI As Object

        Const kMethodName As String = "EditUpdate"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get appropriate band
        Dim oReinsurance As bSIRReinsurance.Reinsurance = Nothing
        Try
            oReinsurance = m_oReinsurances.Item("RI" & lRIBandID)

        Catch
        End Try


        If oReinsurance Is Nothing Then
            gPMFunctions.RaiseError("Set oReinsurance = m_oReinsurances.Item", "Unable to locate reinsurance for band " & lRIBandID)
        End If

        ' Check if the values have changed
        If oReinsurance.DatabaseStatus <> gPMConstants.PMEComponentAction.PMEdit Then
            ' Check we have an array
            If Informations.IsArray(vRILines) And Informations.IsArray(oReinsurance.RILines) Then
                ' If dimensions have changed then we have added fac so set edited
                If vRILines.GetUpperBound(1) <> oReinsurance.RILines.GetUpperBound(1) Then
                    oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
                Else
                    ' Cache array

                    vOldRI = oReinsurance.RILines.Clone()

                    ' We need to walk the 2 arrays to check
                    For lCount As Integer = vRILines.GetLowerBound(1) To vRILines.GetUpperBound(1)
                        ' Check for changes to the editable fields only!!


                        If Not vRILines(MainModule.RIArrangementLineEnum.DBRILSumInsured, lCount).Equals(vOldRI(MainModule.RIArrangementLineEnum.DBRILSumInsured, lCount)) Or Not vRILines(MainModule.RIArrangementLineEnum.DBRILPremium, lCount).Equals(vOldRI(MainModule.RIArrangementLineEnum.DBRILPremium, lCount)) Or Not vRILines(MainModule.RIArrangementLineEnum.DBRILCommission, lCount).Equals(vOldRI(MainModule.RIArrangementLineEnum.DBRILCommission, lCount)) Or Not vRILines(MainModule.RIArrangementLineEnum.DBRILAgreementCode, lCount).Equals(vOldRI(MainModule.RIArrangementLineEnum.DBRILAgreementCode, lCount)) Then

                            oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
                            Exit For
                        End If
                    Next
                End If
            Else
                oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
            End If
        End If

        ' Store array
        oReinsurance.RILines = vRILines

        GoTo Finally_Renamed
Catch_Renamed:
        ' DO Not Call any functions before here or the error will be lost
        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:
        ' Release reference
        oReinsurance = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    ' Gets the reinsurance details for the supplied band
    ' ***************************************************************** '
    Public Function GetBandValues(ByVal lRIBandID As Integer, ByRef cSumInsured As Decimal, ByRef cPremium As Decimal, ByRef vRILines(,) As Object, ByRef lRIModelID As Integer, ByRef iFacPremiumMethod As Integer, ByRef cOriginalSumInsured As Decimal, ByRef cOriginalPremium As Decimal, ByRef vOriginalRILines(,) As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "GetBandValues"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get appropriate band
        Dim oReinsurance As bSIRReinsurance.Reinsurance = Nothing
        Try
            oReinsurance = m_oReinsurances.Item("RI" & lRIBandID)

        Catch
        End Try

        If oReinsurance Is Nothing Then
            gPMFunctions.RaiseError("Set oReinsurance = m_oReinsurances.Item", "Unable to locate reinsurance for band " & lRIBandID)
        End If

        ' Set return values
        cSumInsured = oReinsurance.SumInsured
        cPremium = oReinsurance.Premium
        If oReinsurance.RILines IsNot Nothing Then
            vRILines = oReinsurance.RILines.Clone()
        End If
        lRIModelID = oReinsurance.RIModelID
        iFacPremiumMethod = oReinsurance.FacPremiumMethod

        cOriginalSumInsured = oReinsurance.OriginalSumInsured
        cOriginalPremium = oReinsurance.OriginalPremium
        If oReinsurance.OriginalRILines IsNot Nothing Then
            vOriginalRILines = oReinsurance.OriginalRILines.Clone
        End If
        oReinsurance = Nothing

        GoTo Finally_Renamed
Catch_Renamed:
        ' DO Not Call any functions before here or the error will be lost
        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:
        Return result

    End Function

    ' ***************************************************************** '
    ' Gets reinsurance arrangement details
    ' ***************************************************************** '
    Public Function GetDetails() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetDetails"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oReinsurances.Clear()

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(m_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Get RI Arrangement details
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRIArrangementSQL, sSQLName:=ACSelectRIArrangementName, bStoredProcedure:=ACSelectRIArrangementStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACSelectRIArrangementSQL)
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(vResultArray) Then
                ' No Records, return PMFalse
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else
                ' Yes, load them into the collection

                lReturn = CType(StoreRIArrangement(vResultArray), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("StoreRIArrangement", "Unable to store reinsurance arrangement details")
                End If
            End If

            Return result
        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(m_sUsername, ACClass, kMethodName, result, excep:=excep)
            Return result
        Finally
            'Return result
            'Resume
            'Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Gets reinsurance arrangement details
    ' ***************************************************************** '
    Public Function GetRIBands(ByRef vBands(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetRIBands"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(m_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Get RI Arrangement details
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRIArrangementBandsSQL, sSQLName:=ACSelectRIArrangementBandsName, bStoredProcedure:=ACSelectRIArrangementBandsStored, vResultArray:=vBands)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACSelectRIArrangementBandsSQL)
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(vBands) Then
                ' No Records, return PMFalse
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Get sum insured totals for the whole risk
    ' ***************************************************************** '
    Public Function GetRiskTotals(ByRef r_cRiskRetainedSI As Decimal, ByRef r_cRiskTreatySI As Decimal, ByRef r_cRiskFacSI As Decimal) As Integer

        Dim result As Integer = 0
        Dim vRILines(,) As Object

        Const kMethodName As String = "GetRiskTotals"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise
            r_cRiskRetainedSI = 0
            r_cRiskTreatySI = 0
            r_cRiskFacSI = 0

            ' Loop round all bands
            For Each oReinsurance As bSIRReinsurance.Reinsurance In m_oReinsurances
                ' Cache array and walk lines
                If Informations.IsArray(oReinsurance.RILines) Then
                    vRILines = oReinsurance.RILines.Clone()
                Else
                    ' If RILines is not an array, just continue to the next oReinsurance in the loop
                    Continue For
                End If
                If Informations.IsArray(vRILines) Then

                    For lCount As Integer = vRILines.GetLowerBound(1) To vRILines.GetUpperBound(1)
                        ' Increment appropriate counter
                        Select Case vRILines(MainModule.RIArrangementLineEnum.DBRILType, lCount)
                            Case "R"

                                r_cRiskRetainedSI += gPMFunctions.ToSafeCurrency(CStr(vRILines(MainModule.RIArrangementLineEnum.DBRILSumInsured, lCount)))
                            Case "T"

                                r_cRiskTreatySI += gPMFunctions.ToSafeCurrency(CStr(vRILines(MainModule.RIArrangementLineEnum.DBRILSumInsured, lCount)))
                            Case "F"

                                r_cRiskFacSI += gPMFunctions.ToSafeCurrency(CStr(vRILines(MainModule.RIArrangementLineEnum.DBRILSumInsured, lCount)))
                        End Select
                    Next
                End If
            Next oReinsurance

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Retrieve details from treaty
    ' ***************************************************************** '
    Public Function GetTreatyInfo(ByVal lTreatyId As Integer, ByRef sCode As String, ByRef sAgreementCode As String, ByRef dCommissionPercent As Double, ByRef bIsRetained As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetTreatyInfo"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", CStr(lTreatyId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute SQL Statement
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectTreatySQL, sSQLName:=ACSelectTreatyName, bStoredProcedure:=ACSelectTreatyStored, vResultArray:=vArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to select treaty details")
            End If

            If Informations.IsArray(vArray) Then

                sCode = CStr(vArray(2, 0))

                sAgreementCode = CStr(vArray(6, 0))

                dCommissionPercent = gPMFunctions.ToSafeDouble(CStr(vArray(11, 0)))

                bIsRetained = gPMFunctions.ToSafeBoolean(CStr(vArray(12, 0)))
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Entry point for any initialisation code for this object.
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

            ' Initialisation Code.

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lCurrentRecord = 0
            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTypeOfBusinessNB

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Create Reinsurances Collection
            m_oReinsurances = New bSIRReinsurance.Reinsurances()

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Set the optional process modes.
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Informations.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
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

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Set the Process, Map and Step status.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Entry point for any termination code for this object.
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_oReinsurances = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    ' Loops round the collection, doing any required Adds, Deletes or Updates.
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim bInTransaction As Boolean
        Dim vRILines(,) As Object

        Dim cPremiumTax, cCommissionTax As Decimal

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Update"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we haven't already started a transaction start one.
            If Not bInTransaction Then
                lReturn = m_oDatabase.SQLBeginTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Unable to begin transaction")
                End If
                bInTransaction = True
            End If

            ' First thing to do is remove any partial tax calculations
            lReturn = CType(DeleteTaxCalculationEntries(TREATYPREMIUMTAXTYPE, TREATYCOMMISSIONTAXTYPE), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DeleteTaxCalculationEntries", "Unable to delete temporary treaty taxes")
            End If

            lReturn = CType(DeleteTaxCalculationEntries(FACPREMIUMTAXTYPE, FACCOMMISSIONTAXTYPE), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DeleteTaxCalculationEntries", "Unable to delete temporary facultative taxes")
            End If

            ' Loop round Collection
            For Each oReinsurance As bSIRReinsurance.Reinsurance In m_oReinsurances
                ' If this line is marked as updated, update it
                If oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit Then
                    ' Write modified flag to arrangement
                    bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", CStr(oReinsurance.ArrangementID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "is_modified", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateRIArrangementSQL, sSQLName:=ACUpdateRIArrangementName, bStoredProcedure:=ACUpdateRIArrangementStored)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to update ri arrangement")
                    End If

                    ' Cache array and walk lines

                    vRILines = oReinsurance.RILines.Clone()

                    For lCount As Integer = vRILines.GetLowerBound(1) To vRILines.GetUpperBound(1)
                        ' Check line status

                        If gPMFunctions.ToSafeLong(CStr(vRILines(MainModule.RIArrangementLineEnum.DBRILLineID, lCount))) > 0 Then
                            ' Add parameters

                            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", (vRILines(MainModule.RIArrangementLineEnum.DBRILLineID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", (vRILines(MainModule.RIArrangementLineEnum.DBRILThisShare, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_percent", (vRILines(MainModule.RIArrangementLineEnum.DBRILPremiumPercent, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_percent", (vRILines(MainModule.RIArrangementLineEnum.DBRILCommPercent, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", (vRILines(MainModule.RIArrangementLineEnum.DBRILAgreementCode, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", (vRILines(MainModule.RIArrangementLineEnum.DBRILSumInsured, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_value", (vRILines(MainModule.RIArrangementLineEnum.DBRILPremium, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_value", (vRILines(MainModule.RIArrangementLineEnum.DBRILCommission, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_tax", (vRILines(MainModule.RIArrangementLineEnum.DBRILPremiumTax, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_tax", (vRILines(MainModule.RIArrangementLineEnum.DBRILCommissionTax, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "is_commission_modified", (vRILines(MainModule.RIArrangementLineEnum.DBRILIsCommissionModified, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                            ' Update this line
                            lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateRIArrangementLineSQL, sSQLName:=ACUpdateRIArrangementLineName, bStoredProcedure:=ACUpdateRIArrangementLineStored)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to update ri arrangement lines")
                            End If
                        Else
                            ' Add parameters

                            'developer guide no 85. 
                            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
                            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", CStr(oReinsurance.ArrangementID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "type", (vRILines(MainModule.RIArrangementLineEnum.DBRILType, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", (vRILines(MainModule.RIArrangementLineEnum.DBRILTreatyID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", (vRILines(MainModule.RIArrangementLineEnum.DBRILPartyCnt, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "default_share_percent", (vRILines(MainModule.RIArrangementLineEnum.DBRILDefaultShare, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", (vRILines(MainModule.RIArrangementLineEnum.DBRILThisShare, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_percent", (vRILines(MainModule.RIArrangementLineEnum.DBRILPremiumPercent, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_percent", (vRILines(MainModule.RIArrangementLineEnum.DBRILCommPercent, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", (vRILines(MainModule.RIArrangementLineEnum.DBRILAgreementCode, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "priority", (vRILines(MainModule.RIArrangementLineEnum.DBRILPriority, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "number_of_lines", (vRILines(MainModule.RIArrangementLineEnum.DBRILLines, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "line_limit", (vRILines(MainModule.RIArrangementLineEnum.DBRILLineLimit, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", (vRILines(MainModule.RIArrangementLineEnum.DBRILSumInsured, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_value", (vRILines(MainModule.RIArrangementLineEnum.DBRILPremium, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_value", (vRILines(MainModule.RIArrangementLineEnum.DBRILCommission, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_tax", (vRILines(MainModule.RIArrangementLineEnum.DBRILPremiumTax, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_tax", (vRILines(MainModule.RIArrangementLineEnum.DBRILCommissionTax, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                            bPMAddParameter.AddParameterLite(m_oDatabase, "is_commission_modified", (vRILines(MainModule.RIArrangementLineEnum.DBRILIsCommissionModified, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                            ' Insert this line
                            lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsertRIArrangementLineSQL, sSQLName:=ACInsertRIArrangementLineName, bStoredProcedure:=ACInsertRIArrangementLineStored)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to insert ri arrangement lines")
                            End If

                            ' Store new ID

                            vRILines(MainModule.RIArrangementLineEnum.DBRILLineID, lCount) = m_oDatabase.Parameters.Item("ri_arrangement_line_id").Value

                            ' Recalculate taxes now this is a proper line (we have already stored the values :-)

                            If CStr(vRILines(MainModule.RIArrangementLineEnum.DBRILType, lCount)) = "F" Then




                                lReturn = CType(CalculateFacTax(CInt(vRILines(MainModule.RIArrangementLineEnum.DBRILLineID, lCount)), CInt(vRILines(MainModule.RIArrangementLineEnum.DBRILPartyCnt, lCount)), CDec(vRILines(MainModule.RIArrangementLineEnum.DBRILPremium, lCount)), CDec(vRILines(MainModule.RIArrangementLineEnum.DBRILCommission, lCount)), cPremiumTax, cCommissionTax), gPMConstants.PMEReturnCode)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError("CalculateFacultativeTax", "Unable to calculate facultative tax")
                                End If
                            Else




                                lReturn = CType(CalculateTreatyTax(CInt(vRILines(MainModule.RIArrangementLineEnum.DBRILLineID, lCount)), CInt(vRILines(MainModule.RIArrangementLineEnum.DBRILTreatyID, lCount)), CDec(vRILines(MainModule.RIArrangementLineEnum.DBRILPremium, lCount)), CDec(vRILines(MainModule.RIArrangementLineEnum.DBRILCommission, lCount)), cPremiumTax, cCommissionTax), gPMConstants.PMEReturnCode)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError("CalculateTreatyTax", "Unable to calculate treaty tax")
                                End If
                            End If

                            ' Quick check that taxes match
                            If (cPremiumTax <> vRILines(MainModule.RIArrangementLineEnum.DBRILPremiumTax, lCount)) Or (cCommissionTax <> vRILines(MainModule.RIArrangementLineEnum.DBRILCommissionTax, lCount)) Then
                                ' Decrement the count so this row get it's totals updated next time through
                                lCount -= 1
                            End If
                        End If
                    Next

                    ' Store array back (we may have new id's) and set db status

                    oReinsurance.RILines = vRILines
                    oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                End If
            Next oReinsurance

            ' Commit transaction
            If bInTransaction Then
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Unable to commit transaction")
                End If
                bInTransaction = False
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' Rollback any transactions
            If bInTransaction Then
                lReturn = m_oDatabase.SQLRollbackTrans()
                bInTransaction = False
            End If

        Finally
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ValidateBands
    '
    ' Description: Check bands for 100% allocation
    '
    ' History:
    '   23/06/2003 Peter Finney - Created
    '
    ' Notes:
    '   r_lValid returns band status
    '       0 - Bands all okay
    '       1 - Sum Insured not 100% allocated
    '       2 - Premium not 100% allocated
    '   r_lBand returns number of invalid band if found
    ' ***************************************************************** '
    Public Function ValidateBands(ByRef r_lValid As Integer, ByRef r_lBand As Integer) As Integer

        Dim result As Integer = 0
        Try


            ' Default to true
            result = gPMConstants.PMEReturnCode.PMTrue
            r_lValid = 0

            ' Process each band
            For Each oReinsurance As bSIRReinsurance.Reinsurance In m_oReinsurances
                If oReinsurance IsNot Nothing Then
                    ' Check for minor rounding
                    oReinsurance.Round()

                    ' Check band SI against allocated SI (no rounding issues)
                    If oReinsurance.SumInsured <> oReinsurance.TotalSumInsured Then
                        r_lValid = 1
                        r_lBand = oReinsurance.RIBand
                        Exit For
                    End If

                    ' Check band premium against allocated SI (no rounding issues)
                    If oReinsurance.Premium <> oReinsurance.TotalPremium Then
                        r_lValid = 2
                        r_lBand = oReinsurance.RIBand
                        Exit For
                    End If
                End If
            Next oReinsurance

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateBands Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateBands", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '                        PRIVATE METHODS
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Groups reinsurance lines by RI Band and adds them to the collection.
    ' ***************************************************************** '
    Private Function StoreRIArrangement(ByVal vArrangements(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vRILines(,) As Object = Nothing
        Dim oReinsurance As bSIRReinsurance.Reinsurance

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "StoreRIArrangement"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' For each record
            For lCount As Integer = vArrangements.GetLowerBound(1) To vArrangements.GetUpperBound(1)
                ' Check if we have an object for this band already

                oReinsurance = Nothing
                Try
                    oReinsurance = m_oReinsurances.Item("RI" & CStr(vArrangements(MainModule.RIArrangementEnum.DBRIBandID, lCount)))
                Catch ex As Exception

                End Try

                ' If we don't have one, create one
                If oReinsurance Is Nothing Then
                    ' Create, set ri band and add to collection
                    oReinsurance = New Reinsurance()

                    oReinsurance.RIBand = CInt(vArrangements(MainModule.RIArrangementEnum.DBRIBandID, lCount))
                    m_oReinsurances.Add(oReinsurance)
                End If

                ' Get RI Lines

                bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", CStr(vArrangements(MainModule.RIArrangementEnum.DBRIArrangementID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRIArrangementLineSQL, sSQLName:=ACSelectRIArrangementLineName, bStoredProcedure:=ACSelectRIArrangementLineStored, vResultArray:=vRILines)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get ri arrangement lines")
                End If

                ' Check what type this is

                If gPMFunctions.ToSafeBoolean(CStr(vArrangements(MainModule.RIArrangementEnum.DBRIOriginalFlag, lCount))) Then

                    oReinsurance.OriginalSumInsured = CDec(vArrangements(MainModule.RIArrangementEnum.DBRISumInsured, lCount))

                    oReinsurance.OriginalPremium = CDec(vArrangements(MainModule.RIArrangementEnum.DBRIPremium, lCount))

                    oReinsurance.OriginalRILines = vRILines
                Else

                    oReinsurance.ArrangementID = CInt(vArrangements(MainModule.RIArrangementEnum.DBRIArrangementID, lCount))

                    oReinsurance.SumInsured = CDec(vArrangements(MainModule.RIArrangementEnum.DBRISumInsured, lCount))

                    oReinsurance.Premium = CDec(vArrangements(MainModule.RIArrangementEnum.DBRIPremium, lCount))

                    oReinsurance.RIModelID = gPMFunctions.ToSafeLong(CStr(vArrangements(MainModule.RIArrangementEnum.DBRIModelID, lCount)), 0)

                    oReinsurance.FacPremiumMethod = CInt(vArrangements(MainModule.RIArrangementEnum.DBRIFacPremiumType, lCount))

                    oReinsurance.RILines = vRILines
                End If
            Next lCount

            ' We're done, perform a first pass for minor rounding issues
            m_oReinsurances.Round()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            result = PMEReturnCode.PMError
        End Try

        Return result
    End Function


    ' ***************************************************************** '
    '                           CLASS EVENTS
    ' ***************************************************************** '
    Public Sub New()
        MyBase.New()
        Exit Sub
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function CheckIfDisplayedRI(ByVal IRiskType As Integer, ByRef r_blsDisplayed As Boolean) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                lReturn = .Parameters.Add(sName:="IRiskType", vValue:=CStr(IRiskType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                lReturn = .SQLSelect(sSQL:=ACAutoDisReinsureScrSQL, sSQLName:=ACAutoDisReinsureScrName, bStoredProcedure:=ACAutoDisReinsureScrStored)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If we have a record then the Product is installed
                r_blsDisplayed = Not (.Records.Fields(0) < 1)


            End With
            'Start(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.3.1)
            If r_blsDisplayed Then
                With m_oDatabase

                    'Clear the parameters
                    .Parameters.Clear()
                    bPMAddParameter.AddParameterLite(m_oDatabase, "user_id", CStr(m_iUserID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Authority", "display_reinsurance", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                    lReturn = .SQLSelect(sSQL:=ACSelectUserAuthority, sSQLName:=ACSelectUserAuthorityScrName, bStoredProcedure:=ACSelectUserAuthorityStored)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("CheckIfRIDisplayed", "Call to select user authority failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    r_blsDisplayed = Not (.Records.Fields(0) = 0)

                End With
            End If

            'End(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.3.1)

            '    lReturn = m_oDatabase.CloseDatabase
            '
            '    Set m_oDatabase = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Check if Display RI", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfDisplayedRI", excep:=excep)

            Return result

        End Try
    End Function

    'Start-(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)
    'Arul-PN 70641-Two new parameters(r_crRISumInsured  r_crRIPremium) have been added
    Public Function ChecktheExistenceofRIArrangement(ByVal v_lRiskCnt As Integer, ByRef r_bIsRiskRIArrangementExist As Boolean) As Integer
        Return ChecktheExistenceofRIArrangement(v_lRiskCnt:=v_lRiskCnt, r_bIsRiskRIArrangementExist:=r_bIsRiskRIArrangementExist, r_crRISumInsured:=0, r_crRIPremium:=0)
    End Function

    Public Function ChecktheExistenceofRIArrangement(ByVal v_lRiskCnt As Integer, ByRef r_bIsRiskRIArrangementExist As Boolean, ByRef r_crRISumInsured As Decimal, ByRef r_crRIPremium As Decimal) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Const kMethodName As String = "ChecktheExistenceofRIArrangement"
        'Start Arul-PN 70641
        Const kRISumInsured As Integer = 0
        Const kRIPremium As Integer = 1
        'End Arul-PN 70641


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(m_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Get RI Arrangement details
            ' PN71191
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRIArrangementGetSumInsuredAndPremiumSQL, sSQLName:=ACSelectRIArrangementGetSumInsuredAndPremiumName, bStoredProcedure:=ACSelectRIArrangementGetSumInsuredAndPremiumStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACSelectRIArrangementSQL, gPMConstants.PMELogLevel.PMLogError)
            End If


            If NOT Informations.IsArray(vResultArray) Then
                r_bIsRiskRIArrangementExist = False
                'Start-Arul-PN 70641
                r_crRISumInsured = 0
                r_crRIPremium = 0
                'End-Arul-PN 70641
            Else
                'Start-Arul-PN 70641
                r_bIsRiskRIArrangementExist = True

                r_crRISumInsured = CDec(vResultArray(kRISumInsured, 0))

                r_crRIPremium = CDec(vResultArray(kRIPremium, 0))
                'End-Arul-PN 70641
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    'End-(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)
End Class
