Option Strict Off
Option Explicit On
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("PBRiskPolicyCurrency_NET.PBRiskPolicyCurrency")>
Public NotInheritable Class PBRiskPolicyCurrency
    Implements IDisposable

    Private Const ACClass As String = "PBRiskPolicyCurrency"

    Private Const ACGetCurrencyDetailsSQL As String = "spu_Get_Policy_Currency_For_Risk"
    Private Const ACGetCurrencyDetailsName As String = "Get_Policy_Currency_For_Risk"
    Private Const ACGetCurrencyDetailsStored As Boolean = True

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean

    Private m_lReturn As Integer

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Private m_oCurrency As bACTCurrency.Form

    'Input Parameters
    Private m_lInsuranceFileCnt As Integer

    'Output parameters
    Private m_lCompanyID As Integer
    Private m_sPreviousCurrencyISOCode As String = ""
    Private m_sPreviousCurrencyName As String = ""
    Private m_sPreviousCurrencySymbol As String = ""
    Private m_sCurrentCurrencyISOCode As String = ""
    Private m_sCurrentCurrencyName As String = ""
    Private m_sCurrentCurrencySymbol As String = ""

    Private m_dCombinedRate As Double

    Private m_bGotCurrencyDetails As Boolean

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public ReadOnly Property CurrentCurrencyISOCode() As String
        Get
            If Not m_bGotCurrencyDetails Then
                m_lReturn = GetCurrencyDetails()
            End If
            Return m_sCurrentCurrencyISOCode
        End Get
    End Property

    Public ReadOnly Property CurrentCurrencyName() As String
        Get
            If Not m_bGotCurrencyDetails Then
                m_lReturn = GetCurrencyDetails()
            End If
            Return m_sCurrentCurrencyName
        End Get
    End Property

    Public ReadOnly Property CurrentCurrencySymbol() As String
        Get
            If Not m_bGotCurrencyDetails Then
                m_lReturn = GetCurrencyDetails()
            End If
            Return m_sCurrentCurrencySymbol
        End Get
    End Property

    Public ReadOnly Property PreviousCurrencyISOCode() As String
        Get
            If Not m_bGotCurrencyDetails Then
                m_lReturn = GetCurrencyDetails()
            End If
            Return m_sPreviousCurrencyISOCode
        End Get
    End Property

    Public ReadOnly Property PreviousCurrencyName() As String
        Get
            If Not m_bGotCurrencyDetails Then
                m_lReturn = GetCurrencyDetails()
            End If
            Return m_sPreviousCurrencyName
        End Get
    End Property

    Public ReadOnly Property PreviousCurrencySymbol() As String
        Get
            If Not m_bGotCurrencyDetails Then
                m_lReturn = GetCurrencyDetails()
            End If
            Return m_sPreviousCurrencySymbol
        End Get
    End Property

    Private Function GetCurrencyDetails() As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add("insurance_file_cnt", CStr(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        'Developer guide no 85. 
        m_lReturn = m_oDatabase.Parameters.Add("source_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add("original_currency_iso_code", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.Parameters.Add("original_currency_name", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.Parameters.Add("original_currency_symbol", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.Parameters.Add("new_currency_iso_code", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.Parameters.Add("new_currency_name", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.Parameters.Add("new_currency_symbol", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.Parameters.Add("combined_rate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetCurrencyDetailsSQL, sSQLName:=ACGetCurrencyDetailsName, bStoredProcedure:=ACGetCurrencyDetailsStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lCompanyID = m_oDatabase.Parameters.Item("source_id").Value
        m_sPreviousCurrencyISOCode = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("original_currency_iso_code").Value).Trim()
        m_sPreviousCurrencyName = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("original_currency_name").Value).Trim()
        m_sPreviousCurrencySymbol = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("original_currency_symbol").Value).Trim()
        m_sCurrentCurrencyISOCode = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("new_currency_iso_code").Value).Trim()
        m_sCurrentCurrencyName = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("new_currency_name").Value).Trim()
        m_sCurrentCurrencySymbol = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("new_currency_symbol").Value).Trim()
        m_dCombinedRate = gPMFunctions.ToSafeDouble(m_oDatabase.Parameters.Item("combined_rate").Value)

        m_bGotCurrencyDetails = True

        Return result

    End Function

    Public Function ConvertToCurrentPolicyCurrency(ByVal v_cOldAmount As Decimal, ByRef r_cNewAmount As Decimal) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        If Not m_bGotCurrencyDetails Then
            m_lReturn = GetCurrencyDetails()
        End If

        'Convert old policy amount to new policy amount
        r_cNewAmount = Math.Round(v_cOldAmount * m_dCombinedRate, 2)

        Return result
    End Function

    Public Function ConvertBetweenCurrencies(ByVal v_sOldCurrencyISOCode As String, ByVal v_cOldAmount As Decimal, ByVal v_sNewCurrencyISOCode As String, ByRef r_cNewAmount As Decimal) As Integer

        Dim result As Integer = 0
        Dim iOldCurrencyID, iNewCurrencyID, iBaseCurrencyID As Integer
        Dim cBaseAmount As Decimal

        result = gPMConstants.PMEReturnCode.PMTrue

        If Not m_bGotCurrencyDetails Then
            m_lReturn = GetCurrencyDetails()
        End If

        m_lReturn = CreateCurrencyConvert()

        'Get currency id's from ISO codes

        m_lReturn = m_oCurrency.GetCurrencyIdFromISO(v_sISOCode:=v_sOldCurrencyISOCode, r_iCurrencyID:=iOldCurrencyID)


        m_lReturn = m_oCurrency.GetCurrencyIdFromISO(v_sISOCode:=v_sNewCurrencyISOCode, r_iCurrencyID:=iNewCurrencyID)

        'Get base currency id.

        m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyID:=m_lCompanyID, r_iBaseCurrencyID:=iBaseCurrencyID)

        'Convert the old amount to the new currency
        If iOldCurrencyID = iNewCurrencyID Then
            r_cNewAmount = v_cOldAmount
        Else
            If iOldCurrencyID <> iBaseCurrencyID Then

                m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=iOldCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=v_cOldAmount)
            Else
                cBaseAmount = v_cOldAmount
            End If
            If iNewCurrencyID <> iBaseCurrencyID Then

                m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=iNewCurrencyID, lCompanyID:=m_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=r_cNewAmount)
            Else
                r_cNewAmount = cBaseAmount
            End If
        End If


        Return result
    End Function

    Private Function CreateCurrencyConvert() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oCurrencyConvert Is Nothing Then


            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        If m_oCurrency Is Nothing Then


            m_oCurrency = New bACTCurrency.Form
            m_lReturn = m_oCurrency.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        Return result

    End Function

    Private Function DestroyCurrencyConvert() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If Not (m_oCurrencyConvert Is Nothing) Then

            m_oCurrencyConvert.Dispose()
            m_oCurrencyConvert = Nothing
        End If

        If Not (m_oCurrency Is Nothing) Then

            m_oCurrency.Dispose()
            m_oCurrency = Nothing
        End If

        Return result

    End Function


    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Dim sValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

