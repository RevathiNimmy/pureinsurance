Option Strict Off
Option Explicit On
Imports System.Data
Imports SSP.Shared
'developer guide no. 129
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
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
    Private m_oDatabase As dPMDAO.Database

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
    Private m_oReinsurances As bSIRReinsuranceRI2007.Reinsurances


    Private m_lRIArrangement_id As Integer

    'Gaurav
    Private m_vDeletedRIArrangementIds() As Object
    Private m_vParticipantsArray As Object

    Private m_lTMPRisk_cnt_under_renewal As Integer
    Private m_lCopyFACRiskCnt As Long
    Private m_nRIVersionId As Integer

    Private m_bApplyReinsurance As Boolean



    Public Property DeletedRIArrangementIds() As Object
        Get
            Return m_vDeletedRIArrangementIds.Clone
        End Get
        Set(ByVal Value As Object)
            m_vDeletedRIArrangementIds = Value
        End Set
    End Property


    Public Property RIArrangementId() As Integer
        Get
            Return m_lRIArrangement_id
        End Get
        Set(ByVal Value As Integer)
            m_lRIArrangement_id = Value
        End Set
    End Property

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
    'developer guide no. 101
    Public WriteOnly Property BrokerParticipantArray() As Object
        Set(ByVal Value As Object)
            m_vParticipantsArray = Value
        End Set
    End Property

    Public Property TMPRiskCntUnderRenewal() As Integer
        Get
            Return m_lTMPRisk_cnt_under_renewal
        End Get
        Set(ByVal Value As Integer)
            m_lTMPRisk_cnt_under_renewal = Value
        End Set
    End Property
    Public Property CopyFACRiskCnt() As Integer
        Get
            Return m_lCopyFACRiskCnt
        End Get
        Set(ByVal Value As Integer)
            m_lCopyFACRiskCnt = Value
        End Set
    End Property


    Public Property RIVersionId() As Integer
        Get
            Return m_nRIVersionId
        End Get
        Set(value As Integer)
            m_nRIVersionId = value
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

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", m_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACAutoReinsureRiskSQL, sSQLName:=ACAutoReinsureRiskName, bStoredProcedure:=ACAutoReinsureRiskStored, vResultArray:=vArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Failed to check auto reinsure flag")
            End If

            ' Get result

            r_bAutoReinsure = (gPMFunctions.ToSafeDouble(vArray(0, 0)) = 1)

            ApplyReIns = r_bAutoReinsure

        Catch ex As Exception

            'Debugger.Break()
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
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", m_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", m_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", v_lArrangementLineID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", v_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium", v_cPremium, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission", v_cCommission, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            'developer guide no. 98
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_transtype", FACPREMIUMTAXTYPE, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_transtype", FACCOMMISSIONTAXTYPE, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_tax", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_tax", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLAction(sSQL:=ACCalculateFaculativeTaxSQL, sSQLName:=ACCalculateFaculativeTaxName, bStoredProcedure:=ACCalculateFaculativeTaxStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to calculate facultative taxes")
            End If

            ' Get tax amounts
            r_cPremiumTax = m_oDatabase.Parameters.Item("premium_tax").Value
            r_cCommissionTax = m_oDatabase.Parameters.Item("commission_tax").Value

        Catch ex As Exception
            'Debugger.Break()
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
    ' This will create or refresh an entire reinsurance arrangement for
    ' the given risk, including the original reinsurance
    ' ***************************************************************** '
    Public Function CalculateRI() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "CalculateRI"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Don't calculate if this is view mode
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                ' Add parameters
                bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", m_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", m_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "Trans_type", m_sTransactionType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                If m_lTMPRisk_cnt_under_renewal > 0 Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "TMPRisk_cnt_under_renewal", m_lTMPRisk_cnt_under_renewal, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If
                If m_lCopyFACRiskCnt > 0 Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "copy_fac_risk_cnt", m_lCopyFACRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If
                'If RI2007 Product option is ON Call ACRefreshRI2007SQL

                lReturn = m_oDatabase.SQLAction(sSQL:=ACRefreshRI2007SQL, sSQLName:=ACRefreshRI2007Name, bStoredProcedure:=ACRefreshRI2007Stored)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACRefreshRI2007SQL)
                End If
            End If

        Catch ex As Exception
            'Debugger.Break()
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
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", m_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", m_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", v_lArrangementLineID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium", v_cPremium, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission", v_cCommission, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_transtype", TREATYPREMIUMTAXTYPE, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_transtype", TREATYCOMMISSIONTAXTYPE, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_tax", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_tax", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACCalculateTreatyTaxSQL, sSQLName:=ACCalculateTreatyTaxName, bStoredProcedure:=ACCalculateTreatyTaxStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to calculate treaty taxes")
            End If

            ' Get tax amounts
            r_cPremiumTax = m_oDatabase.Parameters.Item("premium_tax").Value
            r_cCommissionTax = m_oDatabase.Parameters.Item("commission_tax").Value

        Catch ex As Exception
            'Debugger.Break()
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
            'Debugger.Break()
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

            bPMAddParameter.AddParameterLite(m_oDatabase, "RiskCnt", m_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskStatusSQL, sSQLName:=ACUpdateRiskStatusName, bStoredProcedure:=ACUpdateRiskStatusStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            'Debugger.Break()



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
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", m_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", m_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "transtype_premium", v_sTransTypePremium, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "transtype_commission", v_sTransTypeCommission, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            If v_lArrangementLineID > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", v_lArrangementLineID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            ' Execute selection Query
            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteTaxCalculationEntriesSQL, sSQLName:=ACDeleteTaxCalculationEntriesName, bStoredProcedure:=ACDeleteTaxCalculationEntriesStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to delete reinsurance tax entries")
            End If

        Catch ex As Exception
            'Debugger.Break()
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
        Dim oReinsurance As bSIRReinsuranceRI2007.Reinsurance
        Dim vOldRI As Object

        Const kMethodName As String = "EditUpdate"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get appropriate band
        Try

            oReinsurance = m_oReinsurances.Item("RI" & lRIBandID)

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








                            If Not vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lCount).Equals(vOldRI(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lCount)) Or Not vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Premium, lCount).Equals(vOldRI(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Premium, lCount)) Or Not vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Commission, lCount).Equals(vOldRI(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Commission, lCount)) Or Not vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007AgreementCode, lCount).Equals(vOldRI(MainModule.RIArrangementLineEnumRI2007.DBRIL2007AgreementCode, lCount)) Then

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

        Catch ex As Exception
            'Debugger.Break()
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' Release reference
            oReinsurance = Nothing
        End Try

        Return result
    End Function


    ' ***************************************************************** '
    ' Gets the reinsurance details for the supplied band
    ' ***************************************************************** '
    Public Function GetBandValues(ByVal lRIBandID As Integer, ByRef cSumInsured As Decimal, ByRef cPremium As Decimal, ByRef vRILines(,) As Object, ByRef lRIModelID As Integer, ByRef iFacPremiumMethod As Integer, ByRef cOriginalSumInsured As Decimal, ByRef cOriginalPremium As Decimal, ByRef vOriginalRILines(,) As Object, ByRef bIsextendedlimitApplied As Boolean, ByRef cExtendedLimitAmount As Decimal) As Integer
        Return GetBandValues(lRIBandID:=lRIBandID, cSumInsured:=cSumInsured, cPremium:=cPremium, vRILines:=vRILines, lRIModelID:=lRIModelID, iFacPremiumMethod:=iFacPremiumMethod, cOriginalSumInsured:=cOriginalSumInsured, cOriginalPremium:=cOriginalPremium, vOriginalRILines:=vOriginalRILines, bIsextendedlimitApplied:=bIsextendedlimitApplied, cExtendedLimitAmount:=cExtendedLimitAmount, lXOLRIModelId:=0)
    End Function

    Public Function GetBandValues(ByVal lRIBandID As Integer, ByRef cSumInsured As Decimal, ByRef cPremium As Decimal, ByRef vRILines(,) As Object, ByRef lRIModelID As Integer, ByRef iFacPremiumMethod As Integer, ByRef cOriginalSumInsured As Decimal, ByRef cOriginalPremium As Decimal, ByRef vOriginalRILines(,) As Object, ByRef bIsextendedlimitApplied As Boolean, ByRef cExtendedLimitAmount As Decimal, ByRef lXOLRIModelId As Long) As Integer

        Dim result As Integer = 0
        Dim oReinsurance As bSIRReinsuranceRI2007.Reinsurance

        Const kMethodName As String = "GetBandValues"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get appropriate band
        Try


            oReinsurance = m_oReinsurances.Item("RI" & lRIBandID)
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
            m_lRIArrangement_id = oReinsurance.ArrangementID
            bIsextendedlimitApplied = oReinsurance.IsExtendedLimitApplied
            cExtendedLimitAmount = oReinsurance.ExtendedLimitAmount
            ' E007 Changes
            lXOLRIModelId = oReinsurance.XOLRIModelId
            oReinsurance = Nothing

        Catch ex As Exception
            'Debugger.Break()
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
        End Try
        Return result
    End Function

    ''' <summary>
    '''  Gets reinsurance arrangement details
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDetails() As Integer

        Dim oResultArray(,) As Object = Nothing
        Dim nReturn As Integer

        Const kMethodName As String = "GetDetails"

        Try

            nReturn = PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oReinsurances.Clear()

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", m_lRiskCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
            If m_nRIVersionId > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "version_id", m_nRIVersionId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)
            End If
            ' Get RI Arrangement details
            nReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRIArrangementSQL, sSQLName:=ACSelectRIArrangementName, bStoredProcedure:=ACSelectRIArrangementStored, vResultArray:=oResultArray, bKeepNulls:=True)

            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACSelectRIArrangementSQL)
            End If

            ' Do we have any records ?
            If Not IsArray(oResultArray) Then
                ' No Records, return PMFalse
                nReturn = PMEReturnCode.PMNotFound
            Else

                nReturn = CType(StoreRIArrangement(oResultArray), PMEReturnCode)
                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError("StoreRIArrangement", "Unable to store reinsurance arrangement details")
                End If
            End If
            Return nReturn
        Catch excep As Exception
            nReturn = PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn, excep:=excep)
            Return nReturn
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
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", m_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

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
            'Debugger.Break()
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Get sum insured totals for the whole risk
    ' ***************************************************************** '
    Public Function GetRiskTotals(ByRef r_cRiskRetainedSI As Decimal, ByRef r_cRiskTreatySI As Decimal, ByRef r_cRiskFacSI As Decimal) As Integer
        Return GetRiskTotals(r_cRiskRetainedSI:=r_cRiskRetainedSI, r_cRiskTreatySI:=r_cRiskTreatySI, r_cRiskFacSI:=r_cRiskFacSI, r_cRiskXOLTreatySI:=0, r_cRiskXOLFacSI:=0)
    End Function

    Public Function GetRiskTotals(ByRef r_cRiskRetainedSI As Decimal, ByRef r_cRiskTreatySI As Decimal, ByRef r_cRiskFacSI As Decimal, ByRef r_cRiskXOLTreatySI As Decimal, ByRef r_cRiskXOLFacSI As Decimal) As Integer

        Dim result As Integer = 0
        Dim vRILines(,) As Object

        Const kMethodName As String = "GetRiskTotals"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise
            r_cRiskRetainedSI = 0
            r_cRiskTreatySI = 0
            r_cRiskFacSI = 0
            r_cRiskXOLTreatySI = 0
            r_cRiskXOLFacSI = 0
            ' Loop round all bands
            For Each oReinsurance As bSIRReinsuranceRI2007.Reinsurance In m_oReinsurances
                ' Cache array and walk lines

                vRILines = oReinsurance.RILines.Clone()
                If Informations.IsArray(vRILines) Then

                    For lCount As Integer = vRILines.GetLowerBound(1) To vRILines.GetUpperBound(1)
                        ' Increment appropriate counter
                        Select Case vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, lCount)
                            Case "R"

                                r_cRiskRetainedSI += gPMFunctions.ToSafeCurrency(CDbl(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lCount)))
                            Case "T"

                                r_cRiskTreatySI += gPMFunctions.ToSafeCurrency(CDbl(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lCount)))
                            Case "F"

                                r_cRiskFacSI += gPMFunctions.ToSafeCurrency(CDbl(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lCount)))
                            Case "TX"

                                r_cRiskXOLTreatySI += gPMFunctions.ToSafeCurrency(CDbl(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lCount)))
                            Case "FX"

                                r_cRiskXOLFacSI += gPMFunctions.ToSafeCurrency(CDbl(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lCount)))
                        End Select
                    Next
                End If
            Next oReinsurance


        Catch ex As Exception
            'Debugger.Break()
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

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
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", lTreatyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute SQL Statement
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectTreatySQL, sSQLName:=ACSelectTreatyName, bStoredProcedure:=ACSelectTreatyStored, vResultArray:=vArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to select treaty details")
            End If

            If Informations.IsArray(vArray) Then

                sCode = CStr(vArray(2, 0))

                sAgreementCode = CStr(vArray(6, 0))

                dCommissionPercent = gPMFunctions.ToSafeDouble((vArray(11, 0)))

                bIsRetained = gPMFunctions.ToSafeBoolean((vArray(12, 0)))
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch ex As Exception
            'Debugger.Break()
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

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
            m_oReinsurances = New bSIRReinsuranceRI2007.Reinsurances()
            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

            Return result

        Catch excep As System.Exception
            'Debugger.Break()

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
            'Debugger.Break()

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
            'Debugger.Break()



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
        Dim m_iProcessId As Integer

        Dim cPremiumTax, cCommissionTax As Decimal

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Update"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case m_sTransactionType
                Case "NB", "MTA", "REN", "MTC", "MTR"
                    m_iProcessId = 1
                Case "C_CO", "C_CP", "C_CR"
                    m_iProcessId = 2
            End Select

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

            'Gaurav
            If Informations.IsArray(m_vDeletedRIArrangementIds) Then
                For lCount As Integer = m_vDeletedRIArrangementIds.GetLowerBound(0) To m_vDeletedRIArrangementIds.GetUpperBound(0)
                    m_lReturn = CType(DeleteRILines(gPMFunctions.ToSafeLong((m_vDeletedRIArrangementIds(lCount)))), gPMConstants.PMEReturnCode)
                Next
            End If
            ' Loop round Collection
            For Each oReinsurance As bSIRReinsuranceRI2007.Reinsurance In m_oReinsurances
                ' If this line is marked as updated, update it
                If oReinsurance IsNot Nothing Then


                    If oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit Then
                        ' Write modified flag to arrangement
                        bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", oReinsurance.ArrangementID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "is_modified", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateRIArrangementSQL, sSQLName:=ACUpdateRIArrangementName, bStoredProcedure:=ACUpdateRIArrangementStored)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to update ri arrangement")
                        End If


                        ' Cache array and walk lines
                        If oReinsurance.RILines IsNot Nothing Then
                            vRILines = oReinsurance.RILines.Clone()
                            If Informations.IsArray(vRILines) Then

                                For lCount As Integer = vRILines.GetLowerBound(1) To vRILines.GetUpperBound(1)
                                    ' Check line status

                                    If CStr(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, lCount)) <> "FX" Then

                                        If gPMFunctions.ToSafeLong((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LineID, lCount))) > 0 Then
                                            ' Add parameters

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LineID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", gPMFunctions.ToSafeDouble((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007ThisShare, lCount)), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_percent", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007PremiumPercent, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_percent", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007CommPercent, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                            If (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007AgreementCode, lCount) Is DBNull.Value) Then
                                                bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                            Else
                                                bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", CStr(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007AgreementCode, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                            End If


                                            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_value", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Premium, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_value", gPMFunctions.ToSafeCurrency((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Commission, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_tax", gPMFunctions.ToSafeCurrency((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007PremiumTax, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_tax", gPMFunctions.ToSafeCurrency((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007CommissionTax, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "is_commission_modified", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007IsCommissionModified, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "line_limit", gPMFunctions.ToSafeCurrency((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LineLimit, lCount)), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "lower_limit", gPMFunctions.ToSafeCurrency((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LowerLimit, lCount)), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency) 'QBE05

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "retained_percentage", gPMFunctions.ToSafeDecimal((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Retained, lCount)), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) 'QBE05

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "default_share_percent", gPMFunctions.ToSafeDecimal((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007DefaultShare, lCount)), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                            ' Update this line

                                            lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateRIArrangementLineRI2007SQL, sSQLName:=ACUpdateRIArrangementLineRI2007Name, bStoredProcedure:=ACUpdateRIArrangementLineRI2007Stored)

                                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to update ri arrangement lines")
                                            End If
                                        Else
                                            ' Add parameters

                                            'developer guide no. 85
                                            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
                                            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", oReinsurance.ArrangementID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                                            If vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, lCount) Is DBNull.Value Then
                                                bPMAddParameter.AddParameterLite(m_oDatabase, "type", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                            Else
                                                bPMAddParameter.AddParameterLite(m_oDatabase, "type", CStr(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                            End If


                                            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007TreatyID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007PartyCnt, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "default_share_percent", gPMFunctions.ToSafeDouble((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007DefaultShare, lCount)), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", gPMFunctions.ToSafeDouble((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007ThisShare, lCount)), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_percent", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007PremiumPercent, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_percent", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007CommPercent, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                            If vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007AgreementCode, lCount) Is DBNull.Value Then
                                                bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007AgreementCode, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                            Else
                                                bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", CStr(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007AgreementCode, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                            End If


                                            bPMAddParameter.AddParameterLite(m_oDatabase, "priority", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Priority, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "number_of_lines", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Lines, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "line_limit", gPMFunctions.ToSafeCurrency((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LineLimit, lCount)), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_value", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Premium, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_value", gPMFunctions.ToSafeCurrency((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Commission, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_tax", gPMFunctions.ToSafeCurrency((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007PremiumTax, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "commission_tax", gPMFunctions.ToSafeCurrency((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007CommissionTax, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "is_commission_modified", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007IsCommissionModified, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "lower_limit", gPMFunctions.ToSafeCurrency((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LowerLimit, lCount)), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency) 'QBE05

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "retained_percentage", gPMFunctions.ToSafeDecimal((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Retained, lCount)), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) 'QBE05


                                            ' Insert this line

                                            lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsertRIArrangementLineSQL, sSQLName:=ACInsertRIArrangementLineName, bStoredProcedure:=ACInsertRIArrangementLineStored)

                                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to insert ri arrangement lines")
                                            End If

                                            ' Store new ID

                                            vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LineID, lCount) = m_oDatabase.Parameters.Item("ri_arrangement_line_id").Value

                                            ' TO DO DA QBE05 -  Pending Task of Tax Calculation
                                            'To confirm
                                            ' Recalculate taxes now this is a proper line (we have already stored the values :-)

                                            If (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, lCount)) = "F" Or (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, lCount)) = "FX" Then




                                                lReturn = CType(CalculateFacTax((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LineID, lCount)), (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007PartyCnt, lCount)), (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Premium, lCount)), (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Commission, lCount)), cPremiumTax, cCommissionTax), gPMConstants.PMEReturnCode)
                                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    gPMFunctions.RaiseError("CalculateFacultativeTax", "Unable to calculate facultative tax")
                                                End If
                                            Else




                                                lReturn = CType(CalculateTreatyTax((vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LineID, lCount)), (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007TreatyID, lCount)), (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Premium, lCount)), (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Commission, lCount)), cPremiumTax, cCommissionTax), gPMConstants.PMEReturnCode)
                                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    gPMFunctions.RaiseError("CalculateTreatyTax", "Unable to calculate treaty tax")
                                                End If
                                            End If

                                            ' Quick check that taxes match
                                            If (cPremiumTax <> vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007PremiumTax, lCount)) Or (cCommissionTax <> vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007CommissionTax, lCount)) Then
                                                ' Decrement the count so this row get it's totals updated next time through
                                                'lCount = lCount - 1
                                            End If
                                        End If
                                    End If
                                    'Add broker participants if any attached to Fac Prop


                                    If gPMFunctions.ToSafeString(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, lCount)) = "F" And Informations.IsArray(m_vParticipantsArray) And gPMFunctions.ToSafeBoolean(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LineID, lCount)) Then
                                        For iRow As Integer = 0 To m_vParticipantsArray.GetUpperBound(0)

                                            If gPMFunctions.ToSafeLong((m_vParticipantsArray(iRow, ACIBrokerAssociationPartyCnt))) = ToSafeLong(vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007PartyCnt, lCount)) Then
                                                'Add broker

                                                bPMAddParameter.AddParameterLite(m_oDatabase, "Ri_arrangement_line_id", (vRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LineID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                                                bPMAddParameter.AddParameterLite(m_oDatabase, "PartyCnt", (m_vParticipantsArray(iRow, ACIBrokerPartyCnt)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                                                bPMAddParameter.AddParameterLite(m_oDatabase, "Part_percent", (m_vParticipantsArray(iRow, ACIBrokerParticipant_percent)) * 100, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                                                bPMAddParameter.AddParameterLite(m_oDatabase, "ProcessId", m_iProcessId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                                                lReturn = m_oDatabase.SQLAction(sSQL:=ACAddBrokerParticipantsSQL, sSQLName:=ACAddBrokerParticipantsName, bStoredProcedure:=ACAddBrokerParticipantsStored)
                                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to insert Ri Broker Lines")
                                                End If
                                            End If
                                        Next
                                    End If
                                Next
                            End If
                        End If

                        ' Store array back (we may have new id's) and set db status

                        oReinsurance.RILines = vRILines
                        oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                    End If
                    'End If
                    'Update Premium Percent for RI Arrangement Id
                    'PN: 72068
                    m_lReturn = CType(UpdatePremiumPercentForRIArrangement(oReinsurance.ArrangementID), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If
                    'End 72068
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


            'Debugger.Break()
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
            For Each oReinsurance As bSIRReinsuranceRI2007.Reinsurance In m_oReinsurances
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
            'Debugger.Break()



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateBands Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateBands", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '                        PRIVATE METHODS
    ' ***************************************************************** '
    ''' <summary>
    ''' Groups reinsurance lines by RI Band and adds them to the collection.
    ''' </summary>
    ''' <param name="vArrangements"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function StoreRIArrangement(ByVal vArrangements(,) As Object) As Integer

        Dim nResult As Integer = 0

        Dim oRILines(,) As Object = Nothing
        Dim oReinsurance As bSIRReinsuranceRI2007.Reinsurance

        Const kMethodName As String = "StoreRIArrangement"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

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

                    If m_oReinsurances.Count = 0 Then
                        m_oReinsurances.Add(Nothing)
                    End If
                    m_oReinsurances.Add(oReinsurance)
                End If

                ' Get RI Lines

                bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", CDbl(vArrangements(MainModule.RIArrangementEnum.DBRIArrangementID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                nResult = m_oDatabase.SQLSelect(sSQL:=ACSelectRIArrangementLineRI2007SQL, sSQLName:=ACSelectRIArrangementLineRI2007Name, bStoredProcedure:=ACSelectRIArrangementLineRI2007Stored, vResultArray:=oRILines)

                'Sort the RI Lines
                If Informations.IsArray(oRILines) Then

                    nResult = CType(SortRIArray2007(oRILines), gPMConstants.PMEReturnCode)

                    For nCnt As Integer = oRILines.GetLowerBound(1) To oRILines.GetUpperBound(1)

                        If gPMFunctions.ToSafeString(oRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, nCnt)) = "FX" Then
                            oRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, nCnt) = gPMFunctions.ToSafeDouble((oRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, nCnt)), 0) * (1 - gPMFunctions.ToSafeDouble((oRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Retained, nCnt)), 0))
                        End If
                    Next
                End If

                ' Check what type this is

                If gPMFunctions.ToSafeBoolean((vArrangements(MainModule.RIArrangementEnum.DBRIOriginalFlag, lCount))) Then

                    oReinsurance.OriginalSumInsured = CDec(vArrangements(MainModule.RIArrangementEnum.DBRISumInsured, lCount))

                    oReinsurance.OriginalPremium = CDec(vArrangements(MainModule.RIArrangementEnum.DBRIPremium, lCount))

                    oReinsurance.OriginalRILines = oRILines
                Else

                    oReinsurance.ArrangementID = CInt(vArrangements(MainModule.RIArrangementEnum.DBRIArrangementID, lCount))

                    oReinsurance.SumInsured = CDec(vArrangements(MainModule.RIArrangementEnum.DBRISumInsured, lCount))

                    oReinsurance.Premium = CDec(vArrangements(MainModule.RIArrangementEnum.DBRIPremium, lCount))

                    oReinsurance.RIModelID = gPMFunctions.ToSafeLong((vArrangements(MainModule.RIArrangementEnum.DBRIModelID, lCount)), 0)

                    oReinsurance.FacPremiumMethod = CInt(vArrangements(MainModule.RIArrangementEnum.DBRIFacPremiumType, lCount))

                    oReinsurance.RILines = oRILines

                    RIArrangementId = CInt(vArrangements(MainModule.RIArrangementEnum.DBRIArrangementID, lCount)) 'Gaurav

                    oReinsurance.ExtendedLimitAmount = gPMFunctions.ToSafeDecimal(vArrangements(MainModule.RIArrangementEnum.DBRIExtendedLimitAmount, lCount), 0)
                    oReinsurance.IsExtendedLimitApplied = gPMFunctions.ToSafeLong(vArrangements(MainModule.RIArrangementEnum.DBRIIsExtendedLimitApplied, lCount), 0)

                    oReinsurance.XOLRIModelId = gPMFunctions.ToSafeLong(vArrangements(MainModule.RIArrangementEnum.DBRIXOLRIModelId, lCount), 0)


                End If
            Next lCount

            ' We're done, perform a first pass for minor rounding issues
            m_oReinsurances.Round()

        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        End Try

        Return nResult

    End Function

    '*******************************************************************
    'Function              :  SortRIArray2007
    'Description           :  Sort the RI Lines.
    '                      :  Sort order based on RI line Type
    '                         i.e F(Fac Prop), FX(FAC XOl), T( Treaty Prop), TX(Treaty XOL), R(Retained)
    '*********************************************************************
    Private Function SortRIArray2007(ByRef vArrayResult(,) As Object) As Integer

        Dim result As Integer = 0
        'developer guide no.101
        Dim vTemp As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        For iCnt1 As Integer = 0 To vArrayResult.GetUpperBound(1) - 1
            For iCnt As Integer = 0 To vArrayResult.GetUpperBound(1) - iCnt1 - 1


                If IsSortingRequired(CStr(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, iCnt)), CStr(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, iCnt + 1)), gPMFunctions.ToSafeInteger(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007IsObligatory, iCnt)), gPMFunctions.ToSafeInteger(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007IsObligatory, iCnt + 1))) Then
                    For innerCnt As Integer = 0 To vArrayResult.GetUpperBound(0)
                        'developer guide no.101
                        vTemp = vArrayResult(innerCnt, iCnt)


                        vArrayResult(innerCnt, iCnt) = vArrayResult(innerCnt, iCnt + 1)

                        vArrayResult(innerCnt, iCnt + 1) = vTemp
                    Next
                End If
            Next
        Next

        'Shorting Rows for same Type of RI Layers
        For iCnt1 As Integer = 0 To vArrayResult.GetUpperBound(1) - 1

            If (gPMFunctions.ToSafeString(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, iCnt1)) = "TX" Or gPMFunctions.ToSafeString(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, iCnt1)) = "FX") And gPMFunctions.ToSafeString(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, iCnt1)) <> "CAT" Then
                For iCnt As Integer = iCnt1 + 1 To vArrayResult.GetUpperBound(1) - 1

                    If gPMFunctions.ToSafeString(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, iCnt)) = gPMFunctions.ToSafeString(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, iCnt1)) And gPMFunctions.ToSafeCurrency(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LowerLimit, iCnt)) < gPMFunctions.ToSafeCurrency(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007LowerLimit, iCnt1)) Then
                        For innerCnt As Integer = 0 To vArrayResult.GetUpperBound(0)
                            If Trim(ToSafeString(vArrayResult(MainModule.RIArrangementLineEnumRI2007.DBRIL2007ReinsuranceTypeCode, iCnt))) <> "CAT" Then 'E005 Part2
                                vTemp = vArrayResult(innerCnt, iCnt)
                                vArrayResult(innerCnt, iCnt) = vArrayResult(innerCnt, iCnt1)
                                vArrayResult(innerCnt, iCnt1) = vTemp
                            End If
                        Next
                    End If
                Next
            End If
        Next

        Return result
    End Function

    '*************************************************************************************
    '   Function        :        IsSortingRequired
    '   I/P             :        sRIType1,sRIType2  Denotes the RI Type
    '   Return          :        True/False
    '   Description     :        Returns True if sRIType1 priority is greater than sRIType2
    '                            Priority values are Hard Coded and calculated from index of vSortOrder.
    '   Example         :        Priority of 'T' is greater than 'F'
    '**************************************************************************************
    Private Function IsSortingRequired(ByVal sRIType1 As String, ByVal sRIType2 As String, Optional ByVal lObligatoryType1 As Integer = 0, Optional ByVal lObligatoryType2 As Integer = 0) As Boolean

        Dim iIndex1, iIndex2 As Integer

        Dim vSortOrder(6) As String 'E005 Part2
        vSortOrder(0) = "F"
        vSortOrder(1) = "FX"
        vSortOrder(2) = "TFS"
        vSortOrder(3) = "T"
        vSortOrder(4) = "TX"
        vSortOrder(5) = "TC" ' Added for Catastrophe Excess of Loss
        vSortOrder(6) = "R"

        For iCounter As Integer = vSortOrder.GetLowerBound(0) To vSortOrder.GetUpperBound(0)
            If vSortOrder(iCounter) = sRIType1 Then
                iIndex1 = iCounter
            End If
            If vSortOrder(iCounter) = sRIType2 Then
                iIndex2 = iCounter
            End If
        Next
        Return (iIndex1 > iIndex2 And lObligatoryType1 <> 1) Or lObligatoryType2 = 1

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

            ''Start(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.4)
            If r_blsDisplayed Then

                With m_oDatabase

                    'Clear the parameters
                    .Parameters.Clear()
                    bPMAddParameter.AddParameterLite(m_oDatabase, "user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Authority", "display_reinsurance", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                    lReturn = .SQLSelect(sSQL:=ACSelectUserAuthority, sSQLName:=ACSelectUserAuthorityScrName, bStoredProcedure:=ACSelectUserAuthorityStored)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    r_blsDisplayed = Not (.Records.Fields(0) = 0)

                End With

            End If

            ''End(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.4)


            '    lReturn = m_oDatabase.CloseDatabase
            '
            '    Set m_oDatabase = Nothing

            Return result

        Catch excep As System.Exception
            'Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Check if Display RI", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfDisplayedRI", excep:=excep)

            Return result

        End Try
    End Function

    Public Function DeleteRILines(ByVal lDeletedRILineIds As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Const kMethodName As String = "DeleteRILines"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", lDeletedRILineIds, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)


            ' Delete this line
            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRIArrangementLineRI2007SQL, sSQLName:=ACDeleteRIArrangementLineRI2007Name, bStoredProcedure:=ACDeleteRIArrangementLineRI2007Stored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to update ri arrangement lines")
            End If

        Catch ex As Exception
            'Debugger.Break()
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=Update(), excep:=ex)
        Finally
        End Try
        Return result
    End Function


    '*************************************************************************************
    '   Function        :        UpdatePremiumPercent
    '   Parameters      :
    '   Return          :
    '   Description     :        Update the Premium Percent values via SP
    '                            using the follwing formula Premium/TotPremium * 100
    '   Created         :        Gaurav Arora    Dated : 19-Apr-2007
    '**************************************************************************************

    Public Function UpdatePremiumPercent() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Const kMethodName As String = "UpdatePremiumPercent"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", m_lRIArrangement_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)


            ' Update all line
            lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdatePremiumPercentSQL, sSQLName:=ACUpdatePremiumPercentName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to update ri arrangement lines")
            End If
        Catch ex As Exception
            'Debugger.Break()
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=Update(), excep:=ex)

        Finally

        End Try

        Return result
    End Function


    Public Function UpdatePremiumPercentForRIArrangement(ByVal v_lRIArrangement_id As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Const kMethodName As String = "UpdatePremiumPercentForRIArrangement"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", v_lRIArrangement_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)


            ' Update all line
            lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdatePremiumPercentSQL, sSQLName:=ACUpdatePremiumPercentName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to update ri arrangement lines")
            End If
        Catch ex As Exception
            'Debugger.Break()
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=Update(), excep:=ex)

        Finally

        End Try

        Return result
    End Function

    Public Function GetGroupingIDs(ByRef vOriginalGroupingIDArray(,) As Object) As Integer

        Dim result As Integer = 0

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetGroupingIDs"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", m_lRIArrangement_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Get RI Arrangement details
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetGroupingIdSQL, sSQLName:=ACGetGroupingIdName, bStoredProcedure:=True, vResultArray:=vOriginalGroupingIDArray, bKeepNulls:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACSelectRIArrangementSQL)
            End If

        Catch ex As Exception
            'Debugger.Break()
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try

        Return result
    End Function

    Public Function GetInsurerApprovedStatus(ByRef r_sInsurerApprovedStatus As String) As Integer
        Return GetInsurerApprovedStatus(r_sInsurerApprovedStatus:=r_sInsurerApprovedStatus, v_lPartyCnt:=0)
    End Function

    Public Function GetInsurerApprovedStatus(ByRef r_sInsurerApprovedStatus As String, _
                             ByVal v_lPartyCnt As Integer) As Integer

        Dim lReturn As Integer
        Dim dtResult As New DataTable
        Const kMethodName As String = "GetInsurerApprovedStatus"

        Try

            GetInsurerApprovedStatus = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Collection
            m_oReinsurances.Clear()

            If v_lPartyCnt >= 0 Then

                m_oDatabase.Parameters.Clear()
                ' Add Parameter
                bPMAddParameter.AddParameterLite(m_oDatabase, "PartyCnt", v_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACSelPartyInsurerSQL, sSQLName:=ACSelPartyInsurerName, bStoredProcedure:=ACSelPartyInsuredStored, oRecordset:=dtResult)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, ACSelPartyInsurerSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                    If CInt(dtResult.Rows(0).Item("domiciled_for_tax").ToString) = 1 Then
                        r_sInsurerApprovedStatus = "A"
                    Else
                        r_sInsurerApprovedStatus = "U"
                    End If
                Else
                    r_sInsurerApprovedStatus = "U"
                End If

            End If

            Return lReturn
        Catch ex As Exception
            'DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)
            Return lReturn
        End Try
    End Function

    ''' <summary>
    ''' GetRIVersion
    ''' </summary>
    ''' <param name="r_oRIVersion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRIVersion(ByRef r_oRIVersion(,) As Object) As Integer

        Dim nReturn As Integer
        Const kMethodName As String = "GetRIVersion"

        Try

            nReturn = PMEReturnCode.PMTrue

            ' Add parameters
            AddParameterLite(m_oDatabase, "nRisk_cnt", m_lRiskCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)

            ' Get RI Arrangement details
            nReturn = m_oDatabase.SQLSelect( _
                sSQL:=kSelectRIArrangementVersionsSQL, _
                sSQLName:=kSelectRIArrangementVersions, _
                bStoredProcedure:=kSelectRIArrangementVersionsStored, _
                vResultArray:=r_oRIVersion)
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kSelectRIArrangementVersionsSQL & " Failed", PMELogLevel.PMLogError)
                Return nReturn
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(r_oRIVersion) Then
                ' No Records, return PMFalse
                nReturn = PMEReturnCode.PMNotFound
            End If
            Return nReturn
        Catch ex As Exception
            nReturn = PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetRIVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRIVersion", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)

        End Try
        Return nReturn
    End Function

    ''' <summary>
    ''' IsRatingSectionDeleted
    ''' </summary>
    ''' <param name="nRiskCnt"></param>
    ''' <param name="nRIBand"></param>
    ''' <param name="bIsDeletedRatingSection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsRatingSectionDeleted( _
        ByVal nRiskCnt As Integer, _
        ByVal nRIBand As Integer, _
        ByRef bIsDeletedRatingSection As Boolean) As Integer

        Dim nReturn As Integer
        Dim dtResult As New DataTable
        Const kMethodName As String = "IsRatingSectionDeleted"

        Try

            nReturn = PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "nRisk_cnt", nRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "nRi_band_id", nRIBand, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

            ' Get RI Arrangement details
            nReturn = m_oDatabase.ExecuteDataTable(sSQL:=kSelectRatingSectionsSQL, sSQLName:=kSelectRatingSections, bStoredProcedure:=kSelectRatingSectionsStored, oRecordset:=dtResult)
            If nReturn <> PMEReturnCode.PMTrue Then
                nReturn = PMEReturnCode.PMFalse
                RaiseError(kMethodName, kSelectRatingSectionsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Do we have any records ?
            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                bIsDeletedRatingSection = False
            Else
                bIsDeletedRatingSection = True
            End If
            Return nReturn
        Catch ex As Exception
            nReturn = PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="IsRatingSectionDeleted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsRatingSectionDeleted", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
            Return nReturn
        End Try
    End Function

End Class
