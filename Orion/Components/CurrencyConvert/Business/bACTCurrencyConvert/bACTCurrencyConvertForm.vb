Option Strict Off
Option Explicit On
Imports System.Collections.ObjectModel
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 24/07/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Currency Conversion.
    '
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
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

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' Private work objects
    Private m_oCurrency As bACTCurrency.Form
    Private m_oCurrencyRate As bACTCurrencyRate.Form
    Private m_oCompany As bACTCompany.Form

    ' Private work collections
    Private m_DecimalPlaces As DecimalKeyedCollection
    Private m_FormatStrings As FormatStringKeyedCollection

    ' Error Codes (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lInsuranceFileID As Integer

    Private m_lRiskID As Integer

    Private m_vCurrencyRates As Object

    Private Const ACCurrencyRateEffectiveDate As Integer = 0
    Private Const ACCurrencyRateAgainstBase As Integer = 1
    Private Const ACCurrencyRateCurrencyId As Integer = 2



    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

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


            ' Set database


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            m_oCurrency = New bACTCurrency.Form
            m_lReturn = m_oCurrency.Initialise(sUserName:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oCurrencyRate = New bACTCurrencyRate.Form
            m_lReturn = m_oCurrencyRate.Initialise(sUserName:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oCompany = New bACTCompany.Form
            m_lReturn = m_oCompany.Initialise(sUserName:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

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

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oCurrency IsNot Nothing Then
                    m_oCurrency.Dispose()
                    m_oCurrency = Nothing
                End If
                If m_oCurrencyRate IsNot Nothing Then
                    m_oCurrencyRate.Dispose()
                    m_oCurrencyRate = Nothing

                End If
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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ConvertBaseToCurrency (Public)
    '
    ' Description:  Main method to call to perform convertions
    '               between base and currency.
    ' ***************************************************************** '
    'Developer Guide No 101

    Public Function ConvertBaseToCurrency(ByVal lCurrencyID As Integer, ByVal lCompanyID As Integer, ByVal cBaseAmount As Decimal, ByRef cCurrencyAmount As Decimal) As Integer
        Return ConvertBaseToCurrency(lCurrencyID:=lCurrencyID, lCompanyID:=lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cCurrencyAmount, vConversionDate:=Nothing, vConversionRate:=Nothing, vIsMultiplier:=Nothing, vRounded:=Nothing, vBaseRoundingDifference:=Nothing, vCurrencyRoundingDifference:=Nothing, vFormattedBase:=Nothing, vFormattedCurrency:=Nothing, lEuro:=0, cEuroAmount:=0, vEuroCCyXrate:=Nothing, vEuroBaseXRate:=Nothing, vCCyAmountUnRounded:=Nothing, vBaseAmountUnRounded:=Nothing)
    End Function
    Public Function ConvertBaseToCurrency(ByVal lCurrencyID As Integer, ByVal lCompanyID As Integer, ByVal cBaseAmount As Decimal, ByRef cCurrencyAmount As Decimal, ByRef vConversionDate As Object) As Integer
        Return ConvertBaseToCurrency(lCurrencyID:=lCurrencyID, lCompanyID:=lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cCurrencyAmount, vConversionDate:=vConversionDate, vConversionRate:=Nothing, vIsMultiplier:=Nothing, vRounded:=Nothing, vBaseRoundingDifference:=Nothing, vCurrencyRoundingDifference:=Nothing, vFormattedBase:=Nothing, vFormattedCurrency:=Nothing, lEuro:=0, cEuroAmount:=0, vEuroCCyXrate:=Nothing, vEuroBaseXRate:=Nothing, vCCyAmountUnRounded:=Nothing, vBaseAmountUnRounded:=Nothing)
    End Function
    Public Function ConvertBaseToCurrency(ByVal lCurrencyID As Integer, ByVal lCompanyID As Integer, ByVal cBaseAmount As Decimal, ByRef cCurrencyAmount As Decimal, ByRef vConversionDate As Object, ByRef vConversionRate As Object) As Integer
        Return ConvertBaseToCurrency(lCurrencyID:=lCurrencyID, lCompanyID:=lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cCurrencyAmount, vConversionDate:=vConversionDate, vConversionRate:=vConversionRate, vIsMultiplier:=Nothing, vRounded:=Nothing, vBaseRoundingDifference:=Nothing, vCurrencyRoundingDifference:=Nothing, vFormattedBase:=Nothing, vFormattedCurrency:=Nothing, lEuro:=0, cEuroAmount:=0, vEuroCCyXrate:=Nothing, vEuroBaseXRate:=Nothing, vCCyAmountUnRounded:=Nothing, vBaseAmountUnRounded:=Nothing)
    End Function
    Public Function ConvertBaseToCurrency(ByVal lCurrencyID As Integer, ByVal lCompanyID As Integer, ByVal cBaseAmount As Decimal, ByRef cCurrencyAmount As Decimal, ByRef vConversionDate As Object, ByRef vConversionRate As Object, ByRef vIsMultiplier As Object, ByRef vRounded As Object, ByRef vBaseRoundingDifference As Object, ByRef vCurrencyRoundingDifference As Object, ByRef vFormattedBase As Object, ByRef vFormattedCurrency As Object, ByRef lEuro As Integer, ByRef cEuroAmount As Decimal, ByRef vEuroCCyXrate As Object, ByRef vEuroBaseXRate As Object, ByRef vCCyAmountUnRounded As Object, ByRef vBaseAmountUnRounded As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Default the parameters that are not used anymore. They have been kept in for compatibility.
            vIsMultiplier = True
            vBaseRoundingDifference = 0
            lEuro = 0
            cEuroAmount = 0
            vEuroCCyXrate = 0
            vEuroBaseXRate = 0
            vBaseAmountUnRounded = 0

            'developer guide no. 98(Guide)
            m_lReturn = CType(Convert(v_bConvertToBase:=False, v_lCurrencyID:=lCurrencyID, v_lCompanyId:=lCompanyID, r_cOriginalAmount:=cBaseAmount, r_cConvertedAmount:=cCurrencyAmount, r_vConversionDate:=vConversionDate, r_vConversionRate:=vConversionRate, r_vConvertedAmountUnrounded:=vCCyAmountUnRounded, r_vConvertedAmountRoundingDifference:=vCurrencyRoundingDifference, r_vFormattedOriginalAmount:=vFormattedBase, r_vFormattedConvertedAmount:=vFormattedCurrency), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Informations.IsNothing(vRounded) Then
                If Not vRounded Then
                    cCurrencyAmount = vCCyAmountUnRounded
                    vCurrencyRoundingDifference = 0
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertBaseToCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertBaseToCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ConvertCurrencytoBase (Public)
    '
    ' Description:  Main method to call to perform convertions
    '               between currency and base.
    ' ***************************************************************** '
    'Developer Guide No 101


    Public Function ConvertCurrencytoBase(ByVal lCurrencyID As Integer, ByVal lCompanyID As Integer, ByRef cBaseAmount As Decimal, ByVal cCurrencyAmount As Decimal, Optional ByRef vConversionDate As Object = Nothing, Optional ByRef vConversionRate As Object = Nothing, Optional ByRef vIsMultiplier As Boolean = False, Optional ByRef vRounded As Object = Nothing, Optional ByRef vBaseRoundingDifference As Object = Nothing, Optional ByRef vCurrencyRoundingDifference As Object = Nothing, Optional ByRef vFormattedBase As Object = Nothing, Optional ByRef vFormattedCurrency As Object = Nothing, Optional ByRef lEuro As Integer = 0, Optional ByRef cEuroAmount As Decimal = 0, Optional ByRef vEuroCCyXrate As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vCCyAmountUnRounded As Object = Nothing, Optional ByRef vBaseAmountUnRounded As Decimal = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Default the parameters that are not used anymore. They have been kept in for compatibility.
            vIsMultiplier = True
            vCurrencyRoundingDifference = 0
            lEuro = 0
            cEuroAmount = 0
            vEuroCCyXrate = 0
            vEuroBaseXRate = 0
            vCCyAmountUnRounded = 0



            'developer guide no.98
            m_lReturn = CType(Convert(v_bConvertToBase:=True, v_lCurrencyID:=lCurrencyID, v_lCompanyId:=lCompanyID, r_cOriginalAmount:=cCurrencyAmount, r_cConvertedAmount:=cBaseAmount, r_vConversionDate:=vConversionDate, r_vConversionRate:=vConversionRate, r_vConvertedAmountUnrounded:=vBaseAmountUnRounded, r_vConvertedAmountRoundingDifference:=vBaseRoundingDifference, r_vFormattedOriginalAmount:=vFormattedCurrency, r_vFormattedConvertedAmount:=vFormattedBase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Informations.IsNothing(vRounded) Then
                If Not vRounded Then
                    cBaseAmount = vBaseAmountUnRounded
                    vBaseRoundingDifference = 0
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertCurrencytoBase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertCurrencytoBase", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAmountInWords (Public)
    '
    ' Description: Returns the amount in words.
    '
    ' ***************************************************************** '
    Public Function GetAmountInWords(ByVal v_lCurrencyID As Integer, ByVal v_cCurrencyAmount As Decimal, ByRef r_sWords As String) As Integer

        Dim result As Integer = 0
        Dim sMajorWords As String = ""
        Dim sMinorWords As String = ""
        Dim sMajorName As String = ""
        Dim sMinorName As String = ""
        Dim iDecimalPlaces As Integer
        Dim sFormatString As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            AmountInWords(v_cCurrencyAmount, sMajorWords, sMinorWords)

            If v_lCurrencyID <> 0 Then
                m_lReturn = CType(GetCurrencyDetails(v_lCurrencyID, iDecimalPlaces, sFormatString, sMajorName, sMinorName), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
                If sMajorWords.Length > 0 Then
                    sMajorWords = sMajorWords & " " & sMajorName
                End If
                If sMinorWords.Length > 0 Then
                    sMinorWords = sMinorWords & " " & sMinorName
                    sMajorWords = sMajorWords & " and " & sMinorWords
                End If
            End If

            r_sWords = sMajorWords.Trim()

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAmountInWords Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAmountInWords", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: FormatCurrency
    '
    ' Description: Formats the amount into the correct syntax according to the currency passed in.
    '
    ' Note : vConversionDate is no longer required but has been left in as an optional parameter for compatibility.
    '
    ' ***************************************************************** '
    Public Function FormatCurrency(ByVal vCurrencyID As Object, ByVal vCurrencyAmount As Object, ByRef vFormattedCurrency As String) As Integer
        Return FormatCurrency(vCurrencyID:=vCurrencyID, vCurrencyAmount:=vCurrencyAmount, vFormattedCurrency:=vFormattedCurrency, vConversionDate:=Nothing)
    End Function

    Public Function FormatCurrency(ByVal vCurrencyID As Object, ByVal vCurrencyAmount As Object, ByRef vFormattedCurrency As String, ByVal vConversionDate As Object) As Integer
        Dim result As Integer = 0
        Dim sFormatString As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'developer guide no.98
            m_lReturn = CType(GetCurrencyDetails(v_lCurrencyID:=vCurrencyID, r_sFormatString:=sFormatString), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            vFormattedCurrency = StringsHelper.Format(vCurrencyAmount, sFormatString.Trim())

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)


    ' ***************************************************************** '
    ' Name: GetCurrencyDetails (Private Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function GetCurrencyDetails(ByVal v_lCurrencyID As Integer) As Integer
        Return GetCurrencyDetails(v_lCurrencyID:=v_lCurrencyID, r_iDecimalPlaces:=0, r_sFormatString:="", r_vMajorName:="", r_vMinorName:="")
    End Function

    Public Function GetCurrencyDetails(ByVal v_lCurrencyID As Integer, ByRef r_sFormatString As String) As Integer
        Return GetCurrencyDetails(v_lCurrencyID:=v_lCurrencyID, r_iDecimalPlaces:=0, r_sFormatString:=r_sFormatString, r_vMajorName:="", r_vMinorName:="")
    End Function

    Public Function GetCurrencyDetails(ByVal v_lCurrencyID As Integer, ByRef r_iDecimalPlaces As Integer) As Integer
        Return GetCurrencyDetails(v_lCurrencyID:=v_lCurrencyID, r_iDecimalPlaces:=r_iDecimalPlaces, r_sFormatString:="", r_vMajorName:="", r_vMinorName:="")
    End Function

    Public Function GetCurrencyDetails(ByVal v_lCurrencyID As Integer, ByRef r_iDecimalPlaces As Integer, ByRef r_sFormatString As String, ByRef r_vMajorName As String, ByRef r_vMinorName As String) As Integer

        Dim result As Integer = 0
        Dim bFound As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bFound = True
            'developer guide no. 131(Guide)
            If (Not (m_DecimalPlaces Is Nothing)) AndAlso (m_DecimalPlaces.Count > 0 And m_DecimalPlaces.Contains(CStr(v_lCurrencyID))) Then
                r_iDecimalPlaces = CInt(m_DecimalPlaces(CStr(v_lCurrencyID)))
            Else
                bFound = False
            End If
            'developer guide no. 131(Guide)
            If (Not (m_FormatStrings Is Nothing)) AndAlso (m_FormatStrings.Count > 0 And m_FormatStrings.Contains(CStr(v_lCurrencyID))) Then
                r_sFormatString = CStr(m_FormatStrings(CStr(v_lCurrencyID)))
            Else
                bFound = False
            End If

            If Not bFound Then

                m_lReturn = m_oCurrency.GetDetails(vCurrencyID:=v_lCurrencyID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If


                m_lReturn = m_oCurrency.GetNext(vDecimalPlaces:=r_iDecimalPlaces, vFormatString:=r_sFormatString, vDescription:=r_vMajorName, vMinorPart:=r_vMinorName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
                m_DecimalPlaces = New DecimalKeyedCollection(CStr(v_lCurrencyID))
                m_FormatStrings = New FormatStringKeyedCollection(CStr(v_lCurrencyID))
                r_vMajorName = gPMFunctions.ToSafeString(r_vMajorName).Trim()
                r_vMinorName = gPMFunctions.ToSafeString(r_vMinorName).Trim()
                m_DecimalPlaces.Add(r_iDecimalPlaces)
                m_FormatStrings.Add(r_sFormatString)
            Else

                m_lReturn = m_oCurrency.GetDetails(vCurrencyID:=v_lCurrencyID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If


                m_lReturn = m_oCurrency.GetNext(vDecimalPlaces:=r_iDecimalPlaces, vFormatString:=r_sFormatString, vDescription:=r_vMajorName, vMinorPart:=r_vMinorName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                r_vMajorName = gPMFunctions.ToSafeString(r_vMajorName).Trim()
                r_vMinorName = gPMFunctions.ToSafeString(r_vMinorName).Trim()
                'm_DecimalPlaces.Add r_iDecimalPlaces, CStr(v_lCurrencyID)
                'm_FormatStrings.Add r_sFormatString, CStr(v_lCurrencyID)
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            If Informations.Err().Number = 5 Then 'invalid procedure call caused by key absent from collection
                bFound = False


            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve Currency Decimal Places()", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCurrencyRate (Private Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function GetCurrencyRate(ByVal v_lCurrencyID As Integer, ByVal v_lCompanyId As Integer, ByVal v_dtConversionDate As Date, ByRef r_vConversionRate As Object) As Integer
        Dim result As Integer = 0

        Dim sKey As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add("currency_id", CStr(v_lCurrencyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("company_id", CStr(v_lCompanyId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'developer guide no. 40
            m_lReturn = m_oDatabase.Parameters.Add("effective_date", v_dtConversionDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)


            'developer guide no. 86
            m_lReturn = m_oDatabase.Parameters.Add("rate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetCurrencyRateSQL, sSQLName:=ACGetCurrencyRateName, bStoredProcedure:=ACGetCurrencyRateStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("rate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("rate").Value) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else

                r_vConversionRate = m_oDatabase.Parameters.Item("rate").Value
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve Currency Decimal Places()", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise

            m_DecimalPlaces = New DecimalKeyedCollection("")
            m_FormatStrings = New FormatStringKeyedCollection("")

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Public Function DoCurrencyConversion(ByVal v_lAccountID As Integer, ByVal v_lCompanyId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cCurrencyAmountUnrounded As Decimal, ByRef r_iBaseCurrencyID As Integer, ByRef r_cBaseAmount As Decimal, ByRef r_iAccountCurrencyID As Integer, ByRef r_cAccountAmount As Decimal, ByRef r_iSystemCurrencyID As Integer, ByRef r_cSystemAmount As Decimal) As Integer
        Return DoCurrencyConversion(v_lAccountID:=v_lAccountID, v_lCompanyId:=v_lCompanyId, v_iCurrencyID:=v_iCurrencyID, v_cCurrencyAmountUnrounded:=v_cCurrencyAmountUnrounded, r_iBaseCurrencyID:=r_iBaseCurrencyID, r_cBaseAmount:=r_cBaseAmount, r_iAccountCurrencyID:=r_iAccountCurrencyID, r_cAccountAmount:=r_cAccountAmount, r_iSystemCurrencyID:=r_iSystemCurrencyID, r_cSystemAmount:=r_cSystemAmount, r_dCurrencyBaseXrate:=0, r_dtCurrencyBaseDate:=#12/30/1899#, r_dAccountBaseXrate:=0, r_dtAccountBaseDate:=#12/30/1899#, r_dSystemBaseXrate:=0, r_dtSystemBaseDate:=#12/30/1899#)
    End Function

    Public Function DoCurrencyConversion(ByVal v_lAccountID As Integer, ByVal v_lCompanyId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cCurrencyAmountUnrounded As Decimal, ByRef r_iBaseCurrencyID As Integer, ByRef r_cBaseAmount As Decimal, ByRef r_iAccountCurrencyID As Integer, ByRef r_cAccountAmount As Decimal, ByRef r_iSystemCurrencyID As Integer, ByRef r_cSystemAmount As Decimal, ByRef r_dCurrencyBaseXrate As Double, ByVal r_dtCurrencyBaseDate As Date) As Integer
        Return DoCurrencyConversion(v_lAccountID:=v_lAccountID, v_lCompanyId:=v_lCompanyId, v_iCurrencyID:=v_iCurrencyID, v_cCurrencyAmountUnrounded:=v_cCurrencyAmountUnrounded, r_iBaseCurrencyID:=r_iBaseCurrencyID, r_cBaseAmount:=r_cBaseAmount, r_iAccountCurrencyID:=r_iAccountCurrencyID, r_cAccountAmount:=r_cAccountAmount, r_iSystemCurrencyID:=r_iSystemCurrencyID, r_cSystemAmount:=r_cSystemAmount, r_dCurrencyBaseXrate:=r_dCurrencyBaseXrate, r_dtCurrencyBaseDate:=r_dtCurrencyBaseDate, r_dAccountBaseXrate:=0, r_dtAccountBaseDate:=#12/30/1899#, r_dSystemBaseXrate:=0, r_dtSystemBaseDate:=#12/30/1899#)
    End Function

    Public Function DoCurrencyConversion(ByVal v_lAccountID As Integer, ByVal v_lCompanyId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cCurrencyAmountUnrounded As Decimal, ByRef r_iBaseCurrencyID As Integer, ByRef r_cBaseAmount As Decimal, ByRef r_iAccountCurrencyID As Integer, ByRef r_cAccountAmount As Decimal, ByRef r_iSystemCurrencyID As Integer, ByRef r_cSystemAmount As Decimal, ByRef r_dCurrencyBaseXrate As Double, ByVal r_dtCurrencyBaseDate As Date, ByRef r_dAccountBaseXrate As Double, ByVal r_dtAccountBaseDate As Date) As Integer
        Return DoCurrencyConversion(v_lAccountID:=v_lAccountID, v_lCompanyId:=v_lCompanyId, v_iCurrencyID:=v_iCurrencyID, v_cCurrencyAmountUnrounded:=v_cCurrencyAmountUnrounded, r_iBaseCurrencyID:=r_iBaseCurrencyID, r_cBaseAmount:=r_cBaseAmount, r_iAccountCurrencyID:=r_iAccountCurrencyID, r_cAccountAmount:=r_cAccountAmount, r_iSystemCurrencyID:=r_iSystemCurrencyID, r_cSystemAmount:=r_cSystemAmount, r_dCurrencyBaseXrate:=r_dCurrencyBaseXrate, r_dtCurrencyBaseDate:=r_dtCurrencyBaseDate, r_dAccountBaseXrate:=r_dAccountBaseXrate, r_dtAccountBaseDate:=r_dtAccountBaseDate, r_dSystemBaseXrate:=0, r_dtSystemBaseDate:=#12/30/1899#)
    End Function

    Public Function DoCurrencyConversion(ByVal v_lAccountID As Integer, ByVal v_lCompanyId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cCurrencyAmountUnrounded As Decimal, ByRef r_iBaseCurrencyID As Integer, ByRef r_cBaseAmount As Decimal, ByRef r_iAccountCurrencyID As Integer, ByRef r_cAccountAmount As Decimal, ByRef r_iSystemCurrencyID As Integer, ByRef r_cSystemAmount As Decimal, ByRef r_dCurrencyBaseXrate As Double, ByVal r_dtCurrencyBaseDate As Date, ByRef r_dAccountBaseXrate As Double, ByVal r_dtAccountBaseDate As Date, ByRef r_dSystemBaseXrate As Double, ByVal r_dtSystemBaseDate As Date) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear existing parameters, if any.
            m_oDatabase.Parameters.Clear()

            'Add parameter details.
            m_lReturn = m_oDatabase.Parameters.Add("account_id", CStr(v_lAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("company_id", CStr(v_lCompanyId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("currency_id", CStr(v_iCurrencyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("currency_amount_unrounded", CStr(v_cCurrencyAmountUnrounded), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            m_lReturn = m_oDatabase.Parameters.Add("mode", "ALL", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.Parameters.Add("base_currency_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add("base_currency_code", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add("base_amount", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add("base_amount_unrounded", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)


            m_lReturn = m_oDatabase.Parameters.Add("account_currency_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add("account_amount", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add("account_amount_unrounded", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)


            m_lReturn = m_oDatabase.Parameters.Add("system_currency_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add("system_amount", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add("system_amount_unrounded", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add("currency_base_xrate", CStr(r_dCurrencyBaseXrate), gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMDouble)
            If r_dtCurrencyBaseDate = #12/30/1899# Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("currency_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = m_oDatabase.Parameters.Add("currency_base_date", r_dtCurrencyBaseDate, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMDate)
            End If
            m_lReturn = m_oDatabase.Parameters.Add("account_base_xrate", CStr(r_dAccountBaseXrate), gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMDouble)
            If r_dtAccountBaseDate = #12/30/1899# Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("account_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = m_oDatabase.Parameters.Add("account_base_date", r_dtAccountBaseDate, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMDate)
            End If
            m_lReturn = m_oDatabase.Parameters.Add("system_base_xrate", CStr(r_dSystemBaseXrate), gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMDouble)
            If r_dtSystemBaseDate = #12/30/1899# Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("system_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = m_oDatabase.Parameters.Add("system_base_date", r_dtSystemBaseDate, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMDate)
            End If


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add("return_status", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'Call the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDoCurrencyConversionSQL, sSQLName:=ACDoCurrencyConversionName, bStoredProcedure:=ACDoCurrencyConversionStored)

            'Get the return values.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or m_oDatabase.Parameters.Item("return_status").Value <> 1 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If


            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("base_currency_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("base_currency_id").Value) Then
                r_iBaseCurrencyID = 0
            Else
                r_iBaseCurrencyID = m_oDatabase.Parameters.Item("base_currency_id").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("base_amount").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("base_amount").Value) Then
                r_cBaseAmount = 0
            Else
                r_cBaseAmount = m_oDatabase.Parameters.Item("base_amount").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_currency_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_currency_id").Value) Then
                r_iAccountCurrencyID = 0
            Else
                r_iAccountCurrencyID = m_oDatabase.Parameters.Item("account_currency_id").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_amount").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_amount").Value) Then
                r_cAccountAmount = 0
            Else
                r_cAccountAmount = m_oDatabase.Parameters.Item("account_amount").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("system_currency_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_currency_id").Value) Then
                r_iSystemCurrencyID = 0
            Else
                r_iSystemCurrencyID = m_oDatabase.Parameters.Item("system_currency_id").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("system_amount").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_amount").Value) Then
                r_cSystemAmount = 0
            Else
                r_cSystemAmount = m_oDatabase.Parameters.Item("system_amount").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Then
                r_dCurrencyBaseXrate = 0
            Else
                r_dCurrencyBaseXrate = m_oDatabase.Parameters.Item("currency_base_xrate").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_base_xrate").Value) Then
                r_dAccountBaseXrate = 0
            Else
                r_dAccountBaseXrate = m_oDatabase.Parameters.Item("account_base_xrate").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("system_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_base_xrate").Value) Then
                r_dSystemBaseXrate = 0
            Else
                r_dSystemBaseXrate = m_oDatabase.Parameters.Item("system_base_xrate").Value
            End If

            Return result

        Catch excep As System.Exception



            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DoCurrencyConversion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DoCurrencyConversion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetInsuranceFileInformation(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Return GetInsuranceFileInformation(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lCompanyID:=0, r_lAccountID:=0, r_iCurrencyID:=0, r_cPremium:=0, r_dCurrencyBaseXrate:=0, r_dtCurrencyBaseDate:=#12/30/1899#, r_dAccountBaseXrate:=0, r_dtAccountBaseDate:=#12/30/1899#, r_dSystemBaseXrate:=0, r_dtSystemBaseDate:=#12/30/1899#, r_lRateOverrideReasonID:=0)
    End Function

    Public Function GetInsuranceFileInformation(ByVal v_lInsuranceFileCnt As Integer, ByRef r_dCurrencyBaseXrate As Double) As Integer
        Return GetInsuranceFileInformation(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lCompanyID:=0, r_lAccountID:=0, r_iCurrencyID:=0, r_cPremium:=0, r_dCurrencyBaseXrate:=r_dCurrencyBaseXrate, r_dtCurrencyBaseDate:=#12/30/1899#, r_dAccountBaseXrate:=0, r_dtAccountBaseDate:=#12/30/1899#, r_dSystemBaseXrate:=0, r_dtSystemBaseDate:=#12/30/1899#, r_lRateOverrideReasonID:=0)
    End Function

    Public Function GetInsuranceFileInformation(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lAccountID As Integer, ByRef r_dCurrencyBaseXrate As Double, ByRef r_dtCurrencyBaseDate As Date, ByRef r_dAccountBaseXrate As Double, ByRef r_dtAccountBaseDate As Date, ByRef r_dSystemBaseXrate As Double, r_dtSystemBaseDate As Date) As Integer
        Return GetInsuranceFileInformation(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lCompanyID:=0, r_lAccountID:=r_lAccountID, r_iCurrencyID:=0, r_cPremium:=0, r_dCurrencyBaseXrate:=r_dCurrencyBaseXrate, r_dtCurrencyBaseDate:=r_dtCurrencyBaseDate, r_dAccountBaseXrate:=r_dAccountBaseXrate, r_dtAccountBaseDate:=r_dtAccountBaseDate, r_dSystemBaseXrate:=r_dSystemBaseXrate, r_dtSystemBaseDate:=r_dtSystemBaseDate, r_lRateOverrideReasonID:=0)
    End Function

    Public Function GetInsuranceFileInformation(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lCompanyID As Integer, ByRef r_lAccountID As Integer, ByRef r_iCurrencyID As Integer, ByRef r_cPremium As Decimal, ByRef r_dCurrencyBaseXrate As Double, ByRef r_dtCurrencyBaseDate As Date, ByRef r_dAccountBaseXrate As Double, ByRef r_dtAccountBaseDate As Date, ByRef r_dSystemBaseXrate As Double, ByRef r_dtSystemBaseDate As Date, ByRef r_lRateOverrideReasonID As Integer) As Integer
        ' ***************************************************************** '
        ' Name: GetInsuranceFileInformation
        '
        ' Description:  Returns currency information from the policy.
        ' ***************************************************************** '

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add parameter details.
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "source_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "agent_account_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "premium", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "system_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "system_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "exchange_rate_override_reason_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'Call the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetInsuranceFileInformationSQL, sSQLName:=ACGetInsuranceFileInformationName, bStoredProcedure:=ACGetInsuranceFileInformationStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", SQLAction, SQL Call failed.")
            End If

            'Get the return values.

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("source_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("source_id").Value) Then
                r_lCompanyID = 0
            Else
                r_lCompanyID = m_oDatabase.Parameters.Item("source_id").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("agent_account_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("agent_account_id").Value) Then
                r_lAccountID = 0
            Else
                r_lAccountID = m_oDatabase.Parameters.Item("agent_account_id").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_id").Value) Then
                r_iCurrencyID = 0
            Else
                r_iCurrencyID = m_oDatabase.Parameters.Item("currency_id").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("premium").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("premium").Value) Then
                r_cPremium = 0
            Else
                r_cPremium = m_oDatabase.Parameters.Item("premium").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Then
                r_dCurrencyBaseXrate = 0
            Else
                r_dCurrencyBaseXrate = m_oDatabase.Parameters.Item("currency_base_xrate").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_base_date").Value) Then
                r_dtCurrencyBaseDate = #12/30/1899#
            Else
                r_dtCurrencyBaseDate = m_oDatabase.Parameters.Item("currency_base_date").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_base_xrate").Value) Then
                r_dAccountBaseXrate = 0
            Else
                r_dAccountBaseXrate = m_oDatabase.Parameters.Item("account_base_xrate").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_base_date").Value) Then
                r_dtAccountBaseDate = #12/30/1899#
            Else
                r_dtAccountBaseDate = m_oDatabase.Parameters.Item("account_base_date").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("system_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_base_xrate").Value) Then
                r_dSystemBaseXrate = 0
            Else
                r_dSystemBaseXrate = m_oDatabase.Parameters.Item("system_base_xrate").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("system_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_base_date").Value) Then
                r_dtSystemBaseDate = #12/30/1899#
            Else
                r_dtSystemBaseDate = m_oDatabase.Parameters.Item("system_base_date").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value) Then
                r_lRateOverrideReasonID = 0
            Else
                r_lRateOverrideReasonID = m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value
            End If


            Return result

        Catch excep As System.Exception



            result = Informations.Err().Number

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFileInformation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFileInformation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'developer guide no. 101
    Public Function UpdateInsuranceFile(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dCurrencyBaseXrate As Double, ByVal v_dtCurrencyBaseDate As Object, ByVal v_dAccountBaseXrate As Double, ByVal v_dtAccountBaseDate As Object, ByVal v_dSystemBaseXrate As Double, ByVal v_dtSystemBaseDate As Object, ByVal v_lRateOverrideReasonID As Integer, ByVal v_iBaseCurrencyID As Integer, ByVal v_iAccountCurrencyID As Integer) As Integer
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear existing parameters, if any.
            m_oDatabase.Parameters.Clear()

            'Add parameter details.
            m_lReturn = m_oDatabase.Parameters.Add("insurance_file_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If v_dCurrencyBaseXrate = 0 Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("currency_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            Else
                m_lReturn = m_oDatabase.Parameters.Add("currency_base_xrate", v_dCurrencyBaseXrate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            End If
            If v_dtCurrencyBaseDate = #12/30/1899# Or v_dtCurrencyBaseDate = DateTime.MinValue Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("currency_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = m_oDatabase.Parameters.Add("currency_base_date", v_dtCurrencyBaseDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If
            If v_dAccountBaseXrate = 0 Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("account_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            Else
                m_lReturn = m_oDatabase.Parameters.Add("account_base_xrate", v_dAccountBaseXrate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            End If
            If v_dtAccountBaseDate = #12/30/1899# Or v_dtAccountBaseDate = DateTime.MinValue Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("account_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = m_oDatabase.Parameters.Add("account_base_date", v_dtAccountBaseDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If
            If v_dSystemBaseXrate = 0 Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("system_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            Else
                m_lReturn = m_oDatabase.Parameters.Add("system_base_xrate", v_dSystemBaseXrate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            End If
            If v_dtSystemBaseDate = #12/30/1899# Or v_dtSystemBaseDate = DateTime.MinValue Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("system_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = m_oDatabase.Parameters.Add("system_base_date", v_dtSystemBaseDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If
            If v_lRateOverrideReasonID = 0 Then

                'developer guide no. 85                'm_lReturn = m_oDatabase.Parameters.Add("exchange_rate_override_reason_id", CStr(DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add("exchange_rate_override_reason_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add("exchange_rate_override_reason_id", v_lRateOverrideReasonID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If
            If v_iBaseCurrencyID = 0 Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("base_currency_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add("base_currency_id", v_iBaseCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If
            If v_iAccountCurrencyID = 0 Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("agent_account_currency_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add("agent_account_currency_id", v_iAccountCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If

            'Call the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateInsuranceFileSQL, sSQLName:=ACUpdateInsuranceFileName, bStoredProcedure:=ACUpdateInsuranceFileStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetAccountIDFromPartyCnt(ByVal v_lPartyCnt As Integer, ByVal v_lCompanyId As Integer, ByRef r_lAccountID As Integer) As Integer
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear existing parameters, if any.
            m_oDatabase.Parameters.Clear()

            'Add parameter details.
            m_lReturn = m_oDatabase.Parameters.Add("companyid", CStr(v_lCompanyId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("PartyCnt", CStr(v_lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add("AccountID", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add("SubBranchID", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'Call the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetAccountIDFromPartyCntSQL, sSQLName:=ACGetAccountIDFromPartyCntName, bStoredProcedure:=ACGetAccountIDFromPartyCntStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the return values.
            r_lAccountID = m_oDatabase.Parameters.Item("AccountID").Value

            Return result

        Catch excep As System.Exception



            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountIDFromPartyCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDFromPartyCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Convert (Public)
    '
    ' Description:  Main method to call to perform convertions
    '               between base and currency or vice versa.
    ' ***************************************************************** '
    Public Function Convert(ByVal v_bConvertToBase As Boolean, ByVal v_lCurrencyID As Integer, ByVal v_lCompanyId As Integer, ByRef r_cOriginalAmount As Decimal, ByRef r_cConvertedAmount As Decimal, ByRef r_vConversionRate As Object) As Integer
        Return Convert(v_bConvertToBase:=v_bConvertToBase, v_lCurrencyID:=v_lCurrencyID, v_lCompanyId:=v_lCompanyId, r_cOriginalAmount:=r_cOriginalAmount, r_cConvertedAmount:=r_cConvertedAmount, r_vConversionDate:=#12/30/1899#, r_vConversionRate:=r_vConversionRate, r_vConvertedAmountUnrounded:=Nothing, r_vConvertedAmountRoundingDifference:=Nothing, r_vFormattedOriginalAmount:=Nothing, r_vFormattedConvertedAmount:=Nothing)
    End Function
    Public Function Convert(ByVal v_bConvertToBase As Boolean, ByVal v_lCurrencyID As Integer, ByVal v_lCompanyId As Integer, ByRef r_cOriginalAmount As Decimal, ByRef r_cConvertedAmount As Decimal) As Integer
        Return Convert(v_bConvertToBase:=v_bConvertToBase, v_lCurrencyID:=v_lCurrencyID, v_lCompanyId:=v_lCompanyId, r_cOriginalAmount:=r_cOriginalAmount, r_cConvertedAmount:=r_cConvertedAmount, r_vConversionDate:=#12/30/1899#, r_vConversionRate:=Nothing, r_vConvertedAmountUnrounded:=Nothing, r_vConvertedAmountRoundingDifference:=Nothing, r_vFormattedOriginalAmount:=Nothing, r_vFormattedConvertedAmount:=Nothing)
    End Function
    Public Function Convert(ByVal v_bConvertToBase As Boolean, ByVal v_lCurrencyID As Integer, ByVal v_lCompanyId As Integer, ByRef r_cOriginalAmount As Decimal, ByRef r_cConvertedAmount As Decimal, ByRef r_vConversionDate As Date, ByRef r_vConversionRate As Object, ByRef r_vConvertedAmountUnrounded As Object, ByRef r_vConvertedAmountRoundingDifference As Object, ByRef r_vFormattedOriginalAmount As Object, ByRef r_vFormattedConvertedAmount As Object) As Integer

        Dim result As Integer = 0
        Dim iOriginalCurrencyID, iConvertedCurrencyID As Integer
        Dim sFormatString As String = ""
        Dim iDecimalPlaces As gPMConstants.PMEVDecimalNoOfDP
        Dim iBaseCurrencyID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Reject invalid calls
            If v_lCurrencyID = 0 Then
                Return result
            End If

            'Get the base currency of the passed in branch.
            m_lReturn = CType(GetBaseCurrency(v_lCompanyId:=v_lCompanyId, r_iBaseCurrencyID:=iBaseCurrencyID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Default date if not passed in or not valid.

            'developer guide no. 113(Guide)
            If Informations.IsDate(r_vConversionDate) Then
                If r_vConversionDate = #12/30/1899# Or r_vConversionDate = #12:00:00 AM# Then
                    r_vConversionDate = DateTime.Today
                End If
            Else
                r_vConversionDate = DateTime.Today
            End If

            'Default rate if not passed in or not valid.

            If Not Informations.IsNothing(r_vConversionRate) Then
                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(r_vConversionRate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    r_vConversionRate = 0
                End If
            Else
                r_vConversionRate = 0
            End If

            If r_vConversionRate = 0 Then
                m_lReturn = CType(GetCurrencyRate(v_lCurrencyID:=v_lCurrencyID, v_lCompanyId:=v_lCompanyId, v_dtConversionDate:=r_vConversionDate, r_vConversionRate:=r_vConversionRate), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'PN32234
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If v_bConvertToBase Then
                'Currency to Base conversion
                r_vConvertedAmountUnrounded = r_cOriginalAmount * r_vConversionRate
                iOriginalCurrencyID = v_lCurrencyID
                iConvertedCurrencyID = iBaseCurrencyID
            Else
                'Base to Currency conversion
                r_vConvertedAmountUnrounded = r_cOriginalAmount / r_vConversionRate
                iOriginalCurrencyID = iBaseCurrencyID
                iConvertedCurrencyID = v_lCurrencyID
            End If

            'Get number of decimal points.
            m_lReturn = CType(GetCurrencyDetails(v_lCurrencyID:=v_lCurrencyID, r_iDecimalPlaces:=iDecimalPlaces), gPMConstants.PMEReturnCode)

            r_cConvertedAmount = gPMMaths.PMRoundupValueVDecimal(r_vConvertedAmountUnrounded, iDecimalPlaces, gPMConstants.PMERoundupFactor.pmeRFactor50Up)
            r_vConvertedAmountRoundingDifference = r_vConvertedAmountUnrounded - r_cConvertedAmount


            'developer guide no. 67(Guide)
            m_lReturn = CType(FormatCurrency(vCurrencyID:=iOriginalCurrencyID, vCurrencyAmount:=r_cOriginalAmount, vFormattedCurrency:=r_vFormattedOriginalAmount), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'developer guide no. 67(Guide)
            m_lReturn = CType(FormatCurrency(vCurrencyID:=iConvertedCurrencyID, vCurrencyAmount:=r_cConvertedAmount, vFormattedCurrency:=r_vFormattedConvertedAmount), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Convert Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Convert", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetBaseCurrency(ByVal v_lCompanyId As Integer, ByRef r_iBaseCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oCompany.GetDetails(vCompanyID:=v_lCompanyId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = m_oCompany.GetNext(vBaseCurrency:=r_iBaseCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in GetBaseCurrency", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetClaimInformation(ByVal sDocumentRef As String) As Integer
        Return GetClaimInformation(sDocumentRef:=sDocumentRef, r_lCompanyID:=0, r_lAccountID:=0, r_iCurrencyID:=0, r_dCurrencyBaseXrate:=0, r_dtCurrencyBaseDate:=#12/30/1899#, r_dAccountBaseXrate:=0, r_dtAccountBaseDate:=#12/30/1899#, r_dSystemBaseXrate:=0, r_dtSystemBaseDate:=#12/30/1899#, r_lRateOverrideReasonID:=0)
    End Function

    Public Function GetClaimInformation(ByVal sDocumentRef As String, ByRef r_lCompanyID As Integer, ByRef r_lAccountID As Integer, ByRef r_iCurrencyID As Integer, ByRef r_dCurrencyBaseXrate As Double, ByRef r_dtCurrencyBaseDate As Date, ByRef r_dAccountBaseXrate As Double, ByRef r_dtAccountBaseDate As Date, ByRef r_dSystemBaseXrate As Double, ByRef r_dtSystemBaseDate As Date, ByRef r_lRateOverrideReasonID As Integer) As Integer
        ' ***************************************************************** '
        ' Name: GetClaimInformation
        '
        ' Description:  Returns currency information from the posted Claim.
        '
        ' THIS FUNCTION IS FOR FUTURE EXPANSION AND IS NOT CALLED.
        ' CURRENTLY ALL CLAIM RATES ARE BASED ON LIVE CONVERSIONS
        ' AT THE TIME OF TRANSACT
        ' ***************************************************************** '

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add parameter details.
            'develoepr guide no.85
            'bPMAddParameter.AddParameterLite(m_oDatabase, "document_ref", CInt(sDocumentRef), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "document_ref", sDocumentRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "source_id", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "system_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "system_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "exchange_rate_override_reason_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'Call the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetClaimInformationSQL, sSQLName:=ACGetClaimInformationName, bStoredProcedure:=ACGetClaimInformationStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", SQLAction, SQL Call failed.")
            End If

            'Get the return values.

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_id").Value) Then
                r_lAccountID = 0
            Else
                r_lAccountID = m_oDatabase.Parameters.Item("account_id").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_id").Value) Then
                r_iCurrencyID = 0
            Else
                r_iCurrencyID = m_oDatabase.Parameters.Item("currency_id").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Then
                r_dCurrencyBaseXrate = 0
            Else
                r_dCurrencyBaseXrate = m_oDatabase.Parameters.Item("currency_base_xrate").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_base_date").Value) Then
                r_dtCurrencyBaseDate = #12/30/1899#
            Else
                r_dtCurrencyBaseDate = m_oDatabase.Parameters.Item("currency_base_date").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_base_xrate").Value) Then
                r_dAccountBaseXrate = 0
            Else
                r_dAccountBaseXrate = m_oDatabase.Parameters.Item("account_base_xrate").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_base_date").Value) Then
                r_dtAccountBaseDate = #12/30/1899#
            Else
                r_dtAccountBaseDate = m_oDatabase.Parameters.Item("account_base_date").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("system_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_base_xrate").Value) Then
                r_dSystemBaseXrate = 0
            Else
                r_dSystemBaseXrate = m_oDatabase.Parameters.Item("system_base_xrate").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("system_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_base_date").Value) Then
                r_dtSystemBaseDate = #12/30/1899#
            Else
                r_dtSystemBaseDate = m_oDatabase.Parameters.Item("system_base_date").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value) Then
                r_lRateOverrideReasonID = 0
            Else
                r_lRateOverrideReasonID = m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value
            End If


            Return result

        Catch excep As System.Exception



            result = Informations.Err().Number

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimInformation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimInformation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetClaimPaymentInformation(ByVal sDocumentRef As String, ByRef r_lAccountID As Integer, ByRef r_dCurrencyBaseXrate As Double, ByRef r_dtCurrencyBaseDate As Date, ByRef r_dAccountBaseXrate As Double, ByRef r_dtAccountBaseDate As Date, ByRef r_dSystemBaseXrate As Double, ByRef r_dtSystemBaseDate As Date) As Integer
        Return GetClaimPaymentInformation(sDocumentRef:=sDocumentRef, r_lCompanyID:=0, r_lAccountID:=r_lAccountID, r_iCurrencyID:=0, r_dCurrencyBaseXrate:=r_dCurrencyBaseXrate, r_dtCurrencyBaseDate:=r_dtCurrencyBaseDate, r_dAccountBaseXrate:=r_dAccountBaseXrate, r_dtAccountBaseDate:=r_dtAccountBaseDate, r_dSystemBaseXrate:=r_dSystemBaseXrate, r_dtSystemBaseDate:=r_dtSystemBaseDate, r_lRateOverrideReasonID:=0)
    End Function

    Public Function GetClaimPaymentInformation(ByVal sDocumentRef As String, ByRef r_lCompanyID As Integer, ByRef r_lAccountID As Integer, ByRef r_iCurrencyID As Integer, ByRef r_dCurrencyBaseXrate As Double, ByRef r_dtCurrencyBaseDate As Date, ByRef r_dAccountBaseXrate As Double, ByRef r_dtAccountBaseDate As Date, ByRef r_dSystemBaseXrate As Double, ByRef r_dtSystemBaseDate As Date, ByRef r_lRateOverrideReasonID As Integer) As Integer
        ' ***************************************************************** '
        ' Name: GetClaimPaymentInformation
        '
        ' Description:  Returns currency information from the posted Claim.
        ' ***************************************************************** '

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add parameter details.
            'developer guide no.85
            bPMAddParameter.AddParameterLite(m_oDatabase, "document_ref", sDocumentRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "source_id", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "system_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "system_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "exchange_rate_override_reason_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'Call the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetClaimPaymentInformationSQL, sSQLName:=ACGetClaimPaymentInformationName, bStoredProcedure:=ACGetClaimPaymentInformationStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", SQLAction, SQL Call failed.")
            End If

            'Get the return values.

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_id").Value) Then
                r_lAccountID = 0
            Else
                r_lAccountID = m_oDatabase.Parameters.Item("account_id").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_id").Value) Then
                r_iCurrencyID = 0
            Else
                r_iCurrencyID = m_oDatabase.Parameters.Item("currency_id").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Then
                r_dCurrencyBaseXrate = 0
            Else
                r_dCurrencyBaseXrate = m_oDatabase.Parameters.Item("currency_base_xrate").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_base_date").Value) Then
                r_dtCurrencyBaseDate = #12/30/1899#
            Else
                r_dtCurrencyBaseDate = m_oDatabase.Parameters.Item("currency_base_date").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_base_xrate").Value) Then
                r_dAccountBaseXrate = 0
            Else
                r_dAccountBaseXrate = m_oDatabase.Parameters.Item("account_base_xrate").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_base_date").Value) Then
                r_dtAccountBaseDate = #12/30/1899#
            Else
                r_dtAccountBaseDate = m_oDatabase.Parameters.Item("account_base_date").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("system_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_base_xrate").Value) Then
                r_dSystemBaseXrate = 0
            Else
                r_dSystemBaseXrate = m_oDatabase.Parameters.Item("system_base_xrate").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("system_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_base_date").Value) Then
                r_dtSystemBaseDate = #12/30/1899#
            Else
                r_dtSystemBaseDate = m_oDatabase.Parameters.Item("system_base_date").Value
            End If

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value) Then
                r_lRateOverrideReasonID = 0
            Else
                r_lRateOverrideReasonID = m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value
            End If


            Return result

        Catch excep As System.Exception



            result = Informations.Err().Number

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimPaymentInformation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimPaymentInformation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CurrencyToCurrencyConversion
    '
    ' Parameters: n/a
    '
    ' Description: Converts currency amount in currency 1 (from) to
    '               currency amount in currency 2 (to).
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function CurrencyToCurrencyConversion(ByVal v_lCurrencyIdFrom As Integer, ByVal v_crCurrencyAmountFrom As Decimal, ByVal v_lCompanyId As Integer, ByVal v_lCurrencyIdTo As Integer, ByRef r_crCurrencyAmountTo As Decimal) As Integer
        Return CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=v_lCurrencyIdFrom, v_crCurrencyAmountFrom:=v_crCurrencyAmountFrom, v_lCompanyId:=v_lCompanyId, v_lCurrencyIdTo:=v_lCurrencyIdTo, r_crCurrencyAmountTo:=r_crCurrencyAmountTo, dt_EffectiveDate:=Nothing)
    End Function

    Public Function CurrencyToCurrencyConversion(ByVal v_lCurrencyIdFrom As Integer, ByVal v_crCurrencyAmountFrom As Decimal, ByVal v_lCompanyId As Integer, ByVal v_lCurrencyIdTo As Integer, ByRef r_crCurrencyAmountTo As Decimal, ByVal dt_EffectiveDate As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CurrencyToCurrencyConversion"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' currency id - from
            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id_from", v_lCurrencyIdFrom, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_amount_from", v_crCurrencyAmountFrom, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "company_id", v_lCompanyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id_to", v_lCurrencyIdTo, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_amount_to", r_crCurrencyAmountTo, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            If Not dt_EffectiveDate Is Nothing Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "effective_date", ToSafeDate(dt_EffectiveDate), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kCurrencyToCurrencyConversionSQL, sSQLName:=kCurrencyToCurrencyConversionName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kCurrencyToCurrencyConversionSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            r_crCurrencyAmountTo = gPMFunctions.ToSafeCurrency(m_oDatabase.Parameters.Item("currency_amount_to").Value)


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

    '***************************************************************** '
    'Name:  GetClaimReceiptInformation
    '
    'Parameters: n/a
    '
    'Description:
    '
    'History:
    '           Created : MEvans : 21-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function GetClaimReceiptInformation(ByVal sDocumentRef As String, ByRef r_lAccountID As Integer, ByRef r_dCurrencyBaseXrate As Double, ByRef r_dtCurrencyBaseDate As Date, ByRef r_dAccountBaseXrate As Double, ByRef r_dtAccountBaseDate As Date, ByRef r_dSystemBaseXrate As Double, ByRef r_dtSystemBaseDate As Date) As Integer
        Return GetClaimReceiptInformation(sDocumentRef:=sDocumentRef, r_lCompanyID:=0, r_lAccountID:=r_lAccountID, r_iCurrencyID:=0, r_dCurrencyBaseXrate:=r_dCurrencyBaseXrate, r_dtCurrencyBaseDate:=r_dtCurrencyBaseDate, r_dAccountBaseXrate:=r_dAccountBaseXrate, r_dtAccountBaseDate:=r_dtAccountBaseDate, r_dSystemBaseXrate:=r_dSystemBaseXrate, r_dtSystemBaseDate:=r_dtSystemBaseDate, r_lRateOverrideReasonID:=0)
    End Function

    Public Function GetClaimReceiptInformation(ByVal sDocumentRef As String, ByRef r_lCompanyID As Integer, ByRef r_lAccountID As Integer, ByRef r_iCurrencyID As Integer, ByRef r_dCurrencyBaseXrate As Double, ByRef r_dtCurrencyBaseDate As Date, ByRef r_dAccountBaseXrate As Double, ByRef r_dtAccountBaseDate As Date, ByRef r_dSystemBaseXrate As Double, ByRef r_dtSystemBaseDate As Date, ByRef r_lRateOverrideReasonID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimReceiptInformation"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Add parameter details.
            'Developer guide no. 85
            bPMAddParameter.AddParameterLite(m_oDatabase, "document_ref", sDocumentRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "source_id", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "system_base_xrate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "system_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "exchange_rate_override_reason_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'Call the stored procedure.
            lReturn = m_oDatabase.SQLAction(sSQL:=ACGetClaimReceiptInformationSQL, sSQLName:=ACGetClaimReceiptInformationName, bStoredProcedure:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetClaimReceiptInformationSQL & "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the return values.

            ' account id

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_id").Value) Then
                r_lAccountID = 0
            Else
                r_lAccountID = m_oDatabase.Parameters.Item("account_id").Value
            End If

            ' currency id

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_id").Value) Then
                r_iCurrencyID = 0
            Else
                r_iCurrencyID = m_oDatabase.Parameters.Item("currency_id").Value
            End If

            ' currency to base xrate

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_base_xrate").Value) Then
                r_dCurrencyBaseXrate = 0
            Else
                r_dCurrencyBaseXrate = m_oDatabase.Parameters.Item("currency_base_xrate").Value
            End If

            ' currency to base date

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("currency_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("currency_base_date").Value) Then
                r_dtCurrencyBaseDate = #12/30/1899#
            Else
                r_dtCurrencyBaseDate = m_oDatabase.Parameters.Item("currency_base_date").Value
            End If

            ' account to base xrate

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_base_xrate").Value) Then
                r_dAccountBaseXrate = 0
            Else
                r_dAccountBaseXrate = m_oDatabase.Parameters.Item("account_base_xrate").Value
            End If

            ' account to base date

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("account_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("account_base_date").Value) Then
                r_dtAccountBaseDate = #12/30/1899#
            Else
                r_dtAccountBaseDate = m_oDatabase.Parameters.Item("account_base_date").Value
            End If

            ' system to base xrate

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("system_base_xrate").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_base_xrate").Value) Then
                r_dSystemBaseXrate = 0
            Else
                r_dSystemBaseXrate = m_oDatabase.Parameters.Item("system_base_xrate").Value
            End If

            ' system to base date

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("system_base_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("system_base_date").Value) Then
                r_dtSystemBaseDate = #12/30/1899#
            Else
                r_dtSystemBaseDate = m_oDatabase.Parameters.Item("system_base_date").Value
            End If

            ' exchange rate override reason id

            If System.Convert.IsDBNull(m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value) Then
                r_lRateOverrideReasonID = 0
            Else
                r_lRateOverrideReasonID = m_oDatabase.Parameters.Item("exchange_rate_override_reason_id").Value
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function FindCurrencyBaseRateByAccount(ByVal v_lAccountID As Integer, ByVal v_lCompanyId As Integer, ByRef r_dAccountBaseXrate As Double, ByVal r_dtAccountBaseDate As Date) As Integer

        Dim nResult As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const kMethodName As String = "FindCurrencyBaseRateByAccount"
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Clear existing parameters, if any.
            m_oDatabase.Parameters.Clear()

            'Add parameter details.
            nResult = m_oDatabase.Parameters.Add("account_id", CStr(v_lAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            nResult = m_oDatabase.Parameters.Add("company_id", CStr(v_lCompanyId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            nResult = m_oDatabase.Parameters.Add("account_base_xrate", CStr(r_dAccountBaseXrate), gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMDouble)
            If r_dtAccountBaseDate = #12/30/1899# Then
                m_lReturn = m_oDatabase.Parameters.Add("account_base_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            Else
                m_lReturn = m_oDatabase.Parameters.Add("account_base_date", r_dtAccountBaseDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If

            'Call the stored procedure.
            nResult = m_oDatabase.SQLAction(sSQL:=ACCurrencyBaseRateByAccountSQL, sSQLName:=ACCurrencyBaseRateByAccounttName, bStoredProcedure:=ACCurrencyBaseRateByAccountStored)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACCurrencyBaseRateByAccountSQL & "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



            r_dAccountBaseXrate = m_oDatabase.Parameters.Item("account_base_xrate").Value

            Return nResult

        Catch excep As System.Exception



            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindCurrencyBaseRateByAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindCurrencyBaseRateByAccount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
