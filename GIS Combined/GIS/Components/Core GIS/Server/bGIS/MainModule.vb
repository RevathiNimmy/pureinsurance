Option Strict Off
Option Explicit On
Imports SSP.Shared

Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  07/04/1999
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bGIS"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' UserID

    ' ***********************************************************'
    ' SQL String Constants
    ' ***********************************************************'

    ' Common
    Public Const ACSQLStartCol As String = " ( "
    Public Const ACSQLEndCol As String = " ) "
    Public Const ACSQLStartParam As String = " {"
    Public Const ACSQLEndParam As String = "} "
    Public Const ACSQLSeparator As String = " , "
    Public Const ACSQLAnd As String = " AND "
    Public Const ACSQLEquals As String = " = "
    Public Const ACSQLWhere As String = " WHERE "

    ' Add SQL Constants
    Public Const ACAddSQLStart As String = "INSERT INTO "
    Public Const ACAddSQLValues As String = " VALUES "

    ' Update SQL Constants
    Public Const ACUpdSQLStart As String = "UPDATE "
    Public Const ACUpdSQLSet As String = " SET "

    ' Select SQL Constants
    Public Const ACSelSQLSelect As String = "SELECT "
    Public Const ACSelSQLFrom As String = " FROM "
    Public Const ACSelSQLOrderBy As String = " ORDER BY "
    Public Const ACSelSQLAsc As String = " ASC"

    'RDT 01092003 - Added constant for Additional data array
    Public Const CNSourceOfBusiness As String = "SOB"
    Public Const CNAgentsOnline As String = "AOL"
    'SJ 12/03/2004 - start
    'Folgate Edi Development
    Public Const CNEdiSolution As String = "EDI_SOLUTION"
    Public Const CNPolicyLinkId As String = "POLICY_LINK_ID"
    Public Const CNSchemeId As String = "CONTROL__SCHEME_ID"
    'SJ 12/03/2004 - end
    Public Const CNDataModelType As String = "DATA_MODEL_TYPE"

    Public Const CNCalledFromSTS As String = "CalledFromSTS"

    ' ID Col
    Public Const ACIDCol As String = "_ID"

    Public Const ACOIMGISSubKey As String = "GIS" ' CL230100

    Public Const ACHTMLBodyStartTag As String = "<BODY>"
    Public Const ACHTMLBodyEndTag As String = "</BODY>"

    'Array constants for the Risk
    Public Const ACRRiskId As Integer = 0
    Public Const ACRRiskStatusId As Integer = 1
    Public Const ACRRiskFolderCnt As Integer = 2
    Public Const ACRAccumulationId As Integer = 3
    Public Const ACRRiskTypeId As Integer = 4
    Public Const ACRDescription As Integer = 5
    Public Const ACRSequenceNumber As Integer = 6
    Public Const ACRSumInsuredRequested As Integer = 7
    Public Const ACRInceptionDate As Integer = 8
    Public Const ACRExpiryDate As Integer = 9
    Public Const ACRIsNotIndexLinked As Integer = 10
    Public Const ACRIsAccumulated As Integer = 11
    Public Const ACRLapsedReasonId As Integer = 12
    Public Const ACRLapsedDate As Integer = 13
    Public Const ACRLapsedDescription As Integer = 14
    Public Const ACRVarDataRef As Integer = 15
    Public Const ACRTotalSumInsured As Integer = 16
    Public Const ACRTotalAnnualPremium As Integer = 17
    Public Const ACRTotalThisPremium As Integer = 18
    Public Const ACRIsRiAtRiskLevel As Integer = 19
    Public Const ACRIsAutoReinsured As Integer = 20
    Public Const ACRGISScreenId As Integer = 21
    Public Const ACREMLPercentage As Integer = 22
    Public Const ACRRiskNumber As Integer = 23
    Public Const ACRVariationNumber As Integer = 24
    Public Const ACRIsRiskSelected As Integer = 25
    Public Const ACRCoverage As Integer = 26
    Public Const ACRInsuredItem As Integer = 27
    Public Const ACRExtensions As Integer = 28
    Public Const ACRPackage As Integer = 29
    Public Const ACRRiskLinkStatus As Integer = 30
    Public Const ACRNCD As Integer = 31
    Public Const ACRExcess As Integer = 32
    Public Const ACRIsPrinted As Integer = 33
    Public Const ACRRenewalOriginalRiskCnt As Integer = 34
    Public Const ACRProRataRate As Integer = 35
    Public Const ACRTotalAnnualPremiumTax As Integer = 36
    Public Const ACRIsConverted As Integer = 37 ' RAW 15/11/2004 : Pricing Changes : added
    Public Const ACRRatingRuleSetId As Integer = 38 ' RAW 15/11/2004 : Pricing Changes : added
    Public Const ACRRateCoverStartDate As Integer = 39 ' RAW 15/11/2004 : Pricing Changes : added
    Public Const ACRRateQuoteDate As Integer = 40 ' RAW 15/11/2004 : Pricing Changes : added
    Public Const ACRRenewalRatingRuleSetId As Integer = 41 ' RAW 15/11/2004 : Pricing Changes : added
    Public Const ACRRenewalRateCoverStartDate As Integer = 42 ' RAW 15/11/2004 : Pricing Changes : added
    Public Const ACRRenewalRateQuoteDate As Integer = 43 ' RAW 15/11/2004 : Pricing Changes : added

    Public Const ACRMax As Integer = 43

    ' PW051004 - lifestyle array constants
    Public Const kLSAPartyCnt As Byte = 0
    Public Const kLSAPartyLifestyleID As Byte = 1
    Public Const kLSAName As Byte = 2
    Public Const kLSACategory As Byte = 3
    Public Const kLSADateOfBirth As Byte = 4
    Public Const kLSAGenderCode As Byte = 5
    Public Const kLSAOccupationCode As Byte = 6
    Public Const kLSASecondaryOccupationCode As Byte = 7
    Public Const kLSAIsSmoker As Byte = 8

    ' PW051004 - associates array constants
    Public Const kASARelationCnt As Byte = 0
    Public Const kASAShortName As Byte = 1
    Public Const kASAResolvedName As Byte = 2
    Public Const kASARelationshipTypeID As Byte = 3
    Public Const kASARelationshipDescription As Byte = 4
    Public Const kASARelationshipTypeCode As Byte = 5

    ' PW051004 - address array constants
    Public Const kADAPostalCode As Byte = 0
    Public Const kADAUsageTypeID As Byte = 1
    Public Const kADAAddress1 As Byte = 2
    Public Const kADAAddress2 As Byte = 3
    Public Const kADAAddress3 As Byte = 4
    Public Const kADAAddress4 As Byte = 5
    Public Const kADAAddressCnt As Byte = 6
    Public Const kADACountryDesc As Byte = 7
    Public Const kADAUsageTypeCode As Byte = 8

    ' PW051004 - contact array constants
    Public Const kCOAContactCnt As Byte = 0
    Public Const kCOAAreaCode As Byte = 1
    Public Const kCOANumber As Byte = 2
    Public Const kCOAExtension As Byte = 3
    Public Const kCOAContactTypeID As Byte = 4
    Public Const kCOAContactDescription As Byte = 5
    Public Const kCOAContactTypeCode As Byte = 6

    ' PW051004 - loyalty scheme array constants
    Public Const kLYAPartySchemeID As Byte = 0
    Public Const kLYAPartyCnt As Byte = 1
    Public Const kLYASchemeID As Byte = 2
    Public Const kLYASchemeDescription As Byte = 3
    Public Const kLYAMembershipNumber As Byte = 4
    Public Const kLYAOtherReference As Byte = 5
    Public Const kLYAStartDate As Byte = 6
    Public Const kLYAEndDate As Byte = 7
    Public Const kLYAMainMembershipNumber As Byte = 8
    Public Const kLYAIsActive As Byte = 9
    Public Const kLYASchemeCode As Byte = 10

    Public Const kGisDataModelTypeRISK As String = "RISK"
    Public Const kGisDataModelTypeCLAIM As String = "CLAIM"
    Public Const kGisDataModelTypePOLICY As String = "POLICY"
    Public Const kGisDataModelTypePARTY As String = "PARTY"
    Public Const kGisDataModelTypeCASE As String = "CASE"
    Public nTransactionType As Integer = 0

    Public Function CreateBOM(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sClassName As String, ByRef r_oBOM As Object) As Integer
        Return CreateBOM(sUserName:=sUserName, sPassword:=gpmfunctions.tosafestring(sPassword), iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=v_sClassName, r_oBOM:=r_oBOM, vDatabase:=Nothing)
    End Function

    Public Function CreateBOM(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sClassName As String, ByRef r_oBOM As Object, ByRef vDatabase As Object) As Integer
        Dim result As Integer = 0
        Dim sClassBOMAppName As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sBOMRequired As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        r_oBOM = Nothing

        ' Create the Obj.Class name using Data Model Code
        sClassBOMAppName = "bGISBOM" & v_sGisDataModelCode & "." & v_sClassName

        ' Create the Component
        r_oBOM = gPMFunctions.CreateLateBoundObject(sClassBOMAppName)

        r_oBOM.GISDataModel = v_sGisDataModelCode
        lReturn = r_oBOM.Initialise(gPMFunctions.ToSafeString(sUserName), gPMFunctions.ToSafeString(sPassword), gPMFunctions.ToSafeInteger(iUserID), gPMFunctions.ToSafeInteger(iSourceID), gPMFunctions.ToSafeInteger(iLanguageID), gPMFunctions.ToSafeInteger(iCurrencyID), gPMFunctions.ToSafeInteger(iLogLevel), ACApp, vDatabase)
        Try
            r_oBOM.GISBusinessTypeCode = v_sGisBusinessTypeCode
        Catch
        End Try

        Return lReturn
    End Function
End Module
