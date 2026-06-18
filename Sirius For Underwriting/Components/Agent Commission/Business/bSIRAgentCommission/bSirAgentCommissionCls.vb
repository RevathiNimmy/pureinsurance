Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 16/09/2000

    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a SirAgentCommission.
    '
    ' Edit History: SR16092000 - Created
    ' 19/10/2005 RKS Commission Override
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
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date
    Private m_lNavigate As Integer

    ' Primary Keys to work with
    ' Insurance File Cnt
    Private m_lInsuranceFileCnt As Integer

    ' PRIVATE Data Members (End)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property
    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

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
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
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


            ' Check that we have the right Database for our
            ' product Family

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level


            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
     Public Function Terminate() As Integer

        Dim result As Integer = 0
        Static bTerminated As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we have already Terminated then exit

            If bTerminated Then
                Return result
            Else
                bTerminated = True
            End If

            ' If this class opened the database, close it
            If m_bCloseDatabase Then
                ' Close the Database
                m_lReturn = m_oDatabase.CloseDatabase()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Release reference to PM Data Access Object
            m_oDatabase = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
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


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
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

                m_lProcessMode = CInt(vProcessMode)
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
    ' Name: CheckDisplayCommission (Public)
    '
    ' Description: should we display this screen?
    '
    ' ***************************************************************** '
    Public Function CheckDisplayCommission(ByRef r_bDisplayScreen As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bDisplayScreen = False

            m_oDatabase.Parameters.Clear()

            'Add the parameters
            If Informations.IsNothing(m_lInsuranceFileCnt) Or CStr(m_lInsuranceFileCnt) = "" Or m_lInsuranceFileCnt = 0 Then
                m_lInsuranceFileCnt = 0
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=m_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Call the Stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckDisplayCommissionSQL, sSQLName:=ACCheckDisplayCommissionName, bStoredProcedure:=ACCheckDisplayCommissionStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return result
            End If


            If CDbl(vArray(0, 0)) = 1 Then
                r_bDisplayScreen = True
            End If

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDisplayCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDisplayCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAgentCommission (Public)
    '
    ' Description: gets the Agentcommission for the insurance file
    '
    ' ***************************************************************** '
    Public Function GetAgentCommission(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vntResult(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            'Call the Stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAgentCommissionSQL, sSQLName:=ACSelectAgentCommissionName, bStoredProcedure:=ACSelectAgentCommisionStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vntResult)


            Return m_lReturn

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgentCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Public Function CalculateAgentTax(ByVal lInsuranceFileCnt As Integer, ByVal iSourceID As Integer, ByVal lTaxGroupID As Integer, ByVal lCurrencyID As Integer, ByVal cAmount As Decimal, ByRef r_cTax As Decimal) As Integer
        ' ---------------------------------------------------------------------------
        ' NAME: CalculateAgentTax
        ' DESCRIPTION: Calculates the amount of Tax on a specific Commission Amount
        ' returns the tax in the currency amount of the policy.
        ' AUTHOR: Danny Davis
        ' DATE: 09 May 2005, 11:22:42
        ' HISTORY:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const kMethodName As String = "CalculateAgentTax"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                'Add the parameters
                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add the parameters

                m_lReturn = .Parameters.Add(sName:="risk_cnt", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add the parameters
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="tax_group_id", vValue:=If(lTaxGroupID = 0, (DBNull.Value), CStr(lTaxGroupID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Tax on Agent Commission
                m_lReturn = .Parameters.Add(sName:="transtype", vValue:="TTAC", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(lCurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="amount", vValue:=CStr(cAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)


                m_lReturn = .Parameters.Add(sName:="tax_currency_amount", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMCurrency)


                m_lReturn = .Parameters.Add(sName:="tax_base_amount", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                'Tell SP to not save results
                m_lReturn = .Parameters.Add(sName:="calculate_only", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'Call the Stored procedure
                m_lReturn = .SQLSelect(sSQL:="spu_SIR_Calculate_Tax_Amounts", sSQLName:="spu_SIR_Calculate_Tax_Amounts", bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to run spu_SIR_Calculate_Tax_Amounts")
                End If

                'Just return the currency tax
                r_cTax = gPMFunctions.ToSafeCurrency(.Parameters.Item("tax_currency_amount").Value, 0)
            End With


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
    ' Name: CalculateAgentCommission (Public)
    '
    ' Description: calculates the Agentcommission for the insurance file
    ' DN 21/02/03 - ISS 2274:Pass the transaction type as an extra parameter
    ' ***************************************************************** '
    Public Function CalculateAgentCommission(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, ByRef r_vntResult(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'DN 21/02/03 - ISS 2274:Pass the transaction type as an extra parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type", vValue:=v_sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Call the Stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCalculateAgentCommissionSQL, sSQLName:=ACCalculateAgentCommissionName, bStoredProcedure:=ACCalculateAgentCommisionStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vntResult)


            Return m_lReturn

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateAgentCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateLeadCommission(Public)
    '
    ' Description: update the lead commission for the insurance file
    '
    ' ***************************************************************** '
    Public Function UpdateLeadCommission(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            'Call The Stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateLeadCommissionSQL, sSQLName:=ACUpdateLeadCommissionName, bStoredProcedure:=ACUpdateLeadCommissionStored)


            Return m_lReturn

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLeadCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLeadCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteAgentCommission
    '
    ' Description:
    '
    ' History: 20/08/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteAgentCommission(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteAgentCommissionSQL, sSQLName:=ACDeleteAgentCommissionName, bStoredProcedure:=ACDeleteAgentCommissionStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAgentCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '-------------------------------------------------------------------------------------------------------------------------------------------------
    ' Function : AddAgentCommission
    '
    ' Person : S.Rajan
    ' Date   : 19th September 2000
    ' Start Renuka - (WPR64 Paralleling)
    ' Added two optional paramaters - v_cMaximumRate and v_iIsValue
    ' End Renuka - (WPR64 Paralleling)
    '-------------------------------------------------------------------------------------------------------------------------------------------------
    'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.2.1.1.1)
    ' Added 2 optional parameters : v_iIsTaxAmended and v_cAmendedTaxValue
    Public Function AddAgentCommission(ByVal v_lInsuranceFileCnt As Integer, ByVal v_nIsLeadAgent As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRisktypeID As Integer, ByVal v_lcommissionbandid As Integer, ByVal v_cPremium As Decimal, ByVal v_cCommissionRate As Double, ByVal v_cCommissionValue As Decimal, ByVal v_nIsAmended As Integer, ByVal v_lTaxGroupID As Integer, Optional ByVal v_cCalculatedCommissionValue As Decimal = 0, Optional ByVal v_sOverrideReason As String = "", Optional ByVal v_cMaximumRate As Decimal = 0, Optional ByVal v_iIsValue As Integer = 0, Optional ByVal v_iIsTaxAmended As Integer = 0, Optional ByVal v_cAmendedTaxValue As Decimal = 0, Optional ByVal v_iPerilTypeId As Integer = 0, Optional ByVal v_iClassOfBusiness As Integer = 0) As Integer
        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.2.1.1.1)

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_lead_Agent", vValue:=CStr(v_nIsLeadAgent), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRisktypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="commission_band_id", vValue:=CStr(v_lcommissionbandid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Premium", vValue:=CStr(v_cPremium), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Commission_percentage", vValue:=CStr(v_cCommissionRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Commission_value", vValue:=CStr(v_cCommissionValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_amended", vValue:=CStr(v_nIsAmended), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            'Add the parameters

            m_lReturn = m_oDatabase.Parameters.Add(sName:="tax_group_id", vValue:=If(v_lTaxGroupID = 0, (DBNull.Value), CStr(v_lTaxGroupID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="calculated_commission_value", vValue:=CStr(v_cCalculatedCommissionValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="override_reason", vValue:=v_sOverrideReason, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            'Start - Renuka - (WPR64 Paralleling)
            If v_cMaximumRate > 0 Then
                'Add the parameters
                m_lReturn = m_oDatabase.Parameters.Add(sName:="maximum_rate", vValue:=CStr(v_cMaximumRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            End If
            'Add the parameters
            If v_iIsValue > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_value", vValue:=CStr(v_iIsValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If
            'End - Renuka - (WPR64 Paralleling)
            'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.2.1.1.1)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_tax_amended", vValue:=CStr(v_iIsTaxAmended), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="amended_tax_value", vValue:=CStr(v_cAmendedTaxValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.2.1.1.1)
            'Call the SP to add the details
            m_lReturn = m_oDatabase.Parameters.Add(sName:="class_of_business_id", vValue:=If(v_iClassOfBusiness = 0, (DBNull.Value), CStr(v_iClassOfBusiness)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="peril_type_id", vValue:=If(v_iPerilTypeId = 0, (DBNull.Value), CStr(v_iPerilTypeId)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddAgentCommisionSQL, sSQLName:=ACAddAgentCommissionName, bStoredProcedure:=ACAddAgentCommissionStored)


            Return m_lReturn

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAgentCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' Function : EditAgentCommission
    '
    ' Person : S.Rajan
    ' Date   : 19th September 2000
    '-------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function EditAgentCommission(ByVal v_lInsuranceFileCnt As Integer, ByVal v_nIsLeadAgent As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRisktypeID As Integer, ByVal v_lcommissionbandid As Integer, ByVal v_cCommissionRate As Decimal, ByVal v_cCommissionValue As Decimal, ByVal v_nIsAmended As Integer, ByVal v_lTaxGroupID As Integer, ByVal v_cTaxAmount As Decimal, Optional ByVal v_cCalculatedCommissionValue As Decimal = 0, Optional ByVal v_sOverrideReason As String = "") As Integer


        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_lead_Agent", vValue:=CStr(v_nIsLeadAgent), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRisktypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="commission_band_id", vValue:=CStr(v_lcommissionbandid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Commission_percentage", vValue:=CStr(v_cCommissionRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Commission_value", vValue:=CStr(v_cCommissionValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_amended", vValue:=CStr(v_nIsAmended), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="tax_group_id", vValue:=CStr(v_lTaxGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="tax_amount", vValue:=CStr(v_cTaxAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="calculated_commission_value", vValue:=CStr(v_cCalculatedCommissionValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="override_reason", vValue:=v_sOverrideReason, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Call the SP to add the details
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACEditAgentCommissionSQL, sSQLName:=ACEditAgentCommissionName, bStoredProcedure:=ACEditAgentCommissionStored)


            Return m_lReturn

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAgentCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' Function : GetallParties
    '
    ' Person : S.Rajan
    ' Date   : 7th September 2000
    '----------------------------------------------------------------------
    Public Function GetallParties(ByRef vntResult(,) As Object) As Integer
        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllPartiesSQL, sSQLName:=ACGetAllPatiesName, bStoredProcedure:=ACGetAllPartiesStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vntResult)

            'Return the result

            Return m_lReturn

        Catch excep As System.Exception


            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllParies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllParties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInsuranceHeaderDetails (Public)
    '
    ' Description: gets the insurance header details
    '
    ' ***************************************************************** '
    Public Function GetInsuranceHeaderDetails(ByRef r_sInsuranceHolderShortName As String, ByRef r_sInsuranceHolderName As String, ByRef r_sInsuranceHolderResolvedName As String, ByRef r_sInsuranceRef As String, ByRef r_sInsuranceFolderDescription As String, ByRef r_sInsuranceCurrencyCode As String, ByRef r_sInsuranceCurrencyCaption As String, ByRef r_iInsuranceSourceID As Integer, ByRef r_iInsuranceCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Dim oInsuranceFile As bSIRInsuranceFile.Services
        Dim oParty As bSIRParty.Services
        Dim oPMLookup As bPMLookup.Business
        Dim lInsuranceHolderCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim lInsuranceFolderCnt As Integer
        Dim iCurrencyID As Integer
        Dim vLookupResult(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lInsuranceFileCnt < 1 Then
                ' no ins file cnt supplied, log error and exit
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Insurance File Cnt supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceHeaderDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' set the required components

            oPMLookup = New bPMLookup.Business()

            m_lReturn = oPMLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            oParty = New bSIRParty.Services()

            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            oInsuranceFile = New bSIRInsuranceFile.Services()

            m_lReturn = oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            oInsuranceFile.InsuranceFileCnt = m_lInsuranceFileCnt

            m_lReturn = oInsuranceFile.GetDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' get the party details

            oParty.PartyCnt = oInsuranceFile.InsuranceHolderCnt

            m_lReturn = oParty.GetDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' set the required details
            r_sInsuranceHolderShortName = CStr(oParty.Shortname).Trim()
            r_sInsuranceHolderName = CStr(oParty.Name).Trim()
            r_sInsuranceHolderResolvedName = CStr(oParty.ResolvedName).Trim()


            lInsuranceHolderCnt = oInsuranceFile.InsuranceHolderCnt
            sInsuranceRef = oInsuranceFile.InsuranceRef

            lInsuranceFolderCnt = oInsuranceFile.InsuranceFolderCnt

            iCurrencyID = oInsuranceFile.CurrencyID

            r_iInsuranceSourceID = oInsuranceFile.SourceID

            ' set the referenced parameter
            r_sInsuranceRef = sInsuranceRef

            ' set the table array
            Dim vTableArray(3, 0) As Object

            ' Column positions for output array.

            ' table name

            vTableArray(0, 0) = "Currency"
            ' key

            vTableArray(1, 0) = iCurrencyID
            ' start position

            vTableArray(2, 0) = 1
            ' number of items

            vTableArray(3, 0) = 1

            ' get the code and caption for currency
            m_lReturn = oPMLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTableArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=m_dtEffectiveDate, vResultArray:=vLookupResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set the returned values to the referenced variables

            r_sInsuranceCurrencyCode = CStr(vLookupResult(2, 0)).Trim()

            r_sInsuranceCurrencyCaption = CStr(vLookupResult(1, 0)).Trim()
            r_iInsuranceCurrencyID = iCurrencyID

            ' Clean up
            oInsuranceFile.Dispose()
            oInsuranceFile = Nothing
            oPMLookup.Dispose()
            oPMLookup = Nothing
            oParty.Dispose()
            oParty = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceHeaderDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceHeaderDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ''' <summary>
    ''' Copies the commission between two insurance files
    ''' </summary>
    ''' <param name="v_lSourceInsuranceFileCnt"></param>
    ''' <param name="v_lTargetInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyPolicyCommission(ByVal v_lSourceInsuranceFileCnt As Long, ByVal v_lTargetInsuranceFileCnt As Long) As Long
        Const kMethodName As String = "CopyPolicyCommission"

        Dim nReturn As Integer = 0

        Try
            CopyPolicyCommission = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_oDatabase.Parameters.Add("insurance_file_cnt", v_lTargetInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add("source_insurance_file_cnt", v_lSourceInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            nReturn = m_oDatabase.SQLAction(sSQL:=kCopyPolicyCommissionSQL, sSQLName:=kCopyPolicyCommissionName, bStoredProcedure:=kCopyPolicyCommissionStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "CopyPolicyCommissionFailed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return nReturn

        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicyCommissionFailed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Terminate", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nReturn
        End Try
    End Function
End Class
