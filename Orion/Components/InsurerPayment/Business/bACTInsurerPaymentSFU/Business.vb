Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Business functions of the class
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' Edit History :
    '
    ' PWF 20/11/2003 - Stripped down to function as SFU specific!
    '****************************************************************** '


    Private Const ACClass As String = "Business"

    ' System variables
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iLogLevel As Integer
    Private m_iCurrencyID As Integer

    ' Return value
    Private m_lReturn As Integer

    ' Instance of database object
    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean

    ' Required business objects
    Private m_oAllocate As bACTAllocate.Business
    Private m_oAutoNumber As Object
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Private m_oDocument As bACTDocument.Form
    Private m_oDocumentPost As bACTDocumentPost.Form
    Private m_oExplorer As bACTExplorer.Form
    Private m_oMatchPost As bACTMatchPost.Form
    Private m_oPeriod As bACTPeriod.Form
    Private m_oTransdetail As bACTTransdetail.Form


    ' Underwriting switches
    Private m_sUnderwritingOrAgency As String = ""
    Private m_sUnderwritingType As String = ""



    ' *******************************************************************************
    ' PUBLIC PROPERTIES
    ' *******************************************************************************
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            ' Product family
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property


    Public ReadOnly Property UnderwritingOrAgency() As String
        Get
            ' Read first time through
            If m_sUnderwritingOrAgency.Length = 0 Then
                m_lReturn = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, m_sUnderwritingOrAgency)
            End If

            ' Return
            Return m_sUnderwritingOrAgency
        End Get
    End Property

    Public ReadOnly Property UnderwritingType() As String
        Get
            ' "A" for Underwriting Agency and "U" for Reinsurance
            If m_sUnderwritingType.Length = 0 Then
                m_lReturn = bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, m_sUnderwritingType)
            End If

            ' Return
            Return m_sUnderwritingType
        End Get
    End Property
    ' *******************************************************************************
    ' PUBLIC PROPERTIES
    ' *******************************************************************************


    ' *******************************************************************************
    ' PUBLIC METHODS
    ' *******************************************************************************

    ' Format a value as currency via the currency convert object.
    Public Function FormatCurrency(ByVal v_lCurrencyID As Integer, ByVal v_cCurrencyAmount As Decimal, Optional ByVal v_dtConversionDate As Date = #12/30/1899#) As String

        Dim result As String = String.Empty
        Dim sFormattedCurrency As String = ""

        Try

            ' Pass through to format object

            result = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=v_lCurrencyID, vCurrencyAmount:=v_cCurrencyAmount, vFormattedCurrency:=sFormattedCurrency, vConversionDate:=v_dtConversionDate)

            ' Return the formatted currency

            Return sFormattedCurrency

        Catch excep As System.Exception


            ' Log Error Message (and return empty as result)
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' Return the account id for a give shortcode
    Public Function GetAccountFromShortCode(ByVal v_sShortCode As String, ByRef r_lAccountId As Integer) As Integer

        Dim result As Integer = 0
        Dim vAccountIds(,) As Object = Nothing
        Dim vOption As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Check instance of bACTExplorer
            m_oExplorer = New bACTExplorer.Form
            m_lReturn = m_oExplorer.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=1, r_vUnderwriting:=vOption)
            If vOption = "1" Then

                m_lReturn = m_oExplorer.GetAccountIdFromShort(v_sShortCode:=v_sShortCode, r_vAccountIds:=vAccountIds, v_vCompanyId:=m_iSourceID)
            Else

                m_lReturn = m_oExplorer.GetAccountIdFromShort(v_sShortCode:=v_sShortCode, r_vAccountIds:=vAccountIds)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Check results
            If Informations.IsArray(vAccountIds) Then

                If vAccountIds.GetUpperBound(0) > vAccountIds.GetLowerBound(0) Then
                    ' Multiple results will force a look up
                    result = gPMConstants.PMEReturnCode.PMFalse
                Else
                    ' Found code

                    r_lAccountId = CInt(vAccountIds(0, 0))
                End If
            Else
                ' No match
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get account details from short code", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountFromShort", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)



            Return result
        End Try
    End Function


    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the database

            m_lReturn = gPMComponentServices.CheckDatabase(v_sUsername:=sUsername, v_iSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            ' Set Username, Password etc...
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Currency Convert
            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Initialise, Failed to create instance of bACTCurrencyConvert.Form")
            End If

            'bACTTransDetail
            m_oTransdetail = New bACTTransdetail.Form
            m_lReturn = m_oTransdetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Initialise, Failed to create instance of bACTTransDetail.Form")
            End If

            'bACTDocumentPost
            m_oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = m_oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Initialise, Failed to create instance of bACTTransDetail.Form")
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' Creates a fake TransMatch entry to say that it's ready to be paid.
    ' This'll get over-written when it is actually paid.
    Public Function MarkTransaction(ByVal v_lTransactionID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cPayment As Decimal, Optional ByVal v_iInstalmentNumber As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lMatchID, lAllocationID, lSubBranchID As Integer

        Dim dtAccountingDate As Date
        Dim cBaseAmount, cCurrencyAmount As Decimal
        'developer guide no.101
        Dim vTransactionCurrencyID As Object = Nothing
        Dim vdCurrencyBaseXRate As Object = Nothing
        Dim vAccountCurrencyID As Object = Nothing
        Dim vdAccountBaseXRate As Object = Nothing
        Dim iTransMatchId As Integer
        Dim lCompanyID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Get the business object
            m_oMatchPost = New bACTMatchPost.Form
            m_lReturn = m_oMatchPost.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set the month to now for marking purposes
            dtAccountingDate = DateTime.Now

            ' DD 05/08/2002: Get the SubBranch
            'developer guide no.98
            m_lReturn = bACTFunc.GetSubBranchID(v_oDatabase:=m_oDatabase, r_lSubBranchID:=lSubBranchID, v_vTransDetailID:=v_lTransactionID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Add the match group

            m_lReturn = m_oMatchPost.AddMatchGroup(v_dtMatchDate:=dtAccountingDate, r_vMatchId:=lMatchID, v_lSubBranchID:=lSubBranchID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set up the data
            lAllocationID = 0

            'Get company from transdetail

            m_lReturn = m_oTransdetail.GetDetails(vTransdetailID:=v_lTransactionID)

            m_lReturn = m_oTransdetail.GetNext(vCompanyID:=lCompanyID, vCurrencyID:=vTransactionCurrencyID, vCurrencyBaseXrate:=vdCurrencyBaseXRate, vAccountCurrencyID:=vAccountCurrencyID, vAccountBaseXrate:=vdAccountBaseXRate)

            'Set parameters for currency conversion
            cCurrencyAmount = v_cPayment

            'Convert to base using the original rate
            If v_iCurrencyID = vTransactionCurrencyID Then
                vdCurrencyBaseXRate = vdCurrencyBaseXRate
            ElseIf v_iCurrencyID = vAccountCurrencyID Then
                vdCurrencyBaseXRate = vdAccountBaseXRate
            Else
                vdCurrencyBaseXRate = 0
            End If

            'Convert to base

            m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=v_iCurrencyID, lCompanyID:=lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cCurrencyAmount, vConversionDate:=DateTime.Today, vConversionRate:=vdCurrencyBaseXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans() ' Not 100% sure this is valid
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", MarkTransaction, Unable to Mark Transaction")
            End If

            ' Add the blank match

            m_lReturn = m_oMatchPost.AddMatchTrans(v_lAllocationdetailID:=lAllocationID, v_lTransDetailID:=v_lTransactionID, v_iCurrencyID:=v_iCurrencyID, v_cBaseMatchAmount:=cBaseAmount, v_cCurrencyMatchAmount:=cCurrencyAmount, v_vdCurrencyMatchXRate:=vdCurrencyBaseXRate, r_vTransMatchId:=iTransMatchId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Write the match posts

            m_lReturn = m_oMatchPost.Commit()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Terminate

            m_oMatchPost.Dispose()
            m_oMatchPost = Nothing
            If v_iInstalmentNumber <> 0 Then
                m_lReturn = UpdateInstalmentNumber(v_iTransMatchId:=iTransMatchId, v_iInstalmentNumber:=v_iInstalmentNumber)
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MarkTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MarkTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)



            Return result
        End Try
    End Function

    ' Create Binder Journal and allocate
    Public Function ProcessBinder(ByVal v_lAccountId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cPayment As Decimal, ByRef v_vTransdetailIds() As Object) As Integer

        Dim result As Integer = 0
        Dim bTransactionStarted As Boolean
        Dim vlAccountIds As Object = Nothing
        Dim lSuspenseAccountId As Integer
        Dim lCompanyID As Integer
        Dim lPeriodID, lTransID, lSuspenseID As Integer

        Dim lDocumentID As Integer
        Dim sDocumentRef As String = ""

        Dim sGroupCode, sRangeCode As String

        Dim cBaseAmount, cCurrencyAmount As Decimal
        Dim dConversionRate As Double

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for zero total
            If v_cPayment = 0 Then
                result = gPMConstants.PMEReturnCode.PMInvalidRequest
                Return result
            End If

            ' Check instance of bACTPeriod
            m_oPeriod = New bACTPeriod.Form
            m_lReturn = m_oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Check instance of bACTExplorer
            m_oExplorer = New bACTExplorer.Form
            m_lReturn = m_oExplorer.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Check instance of bACTDocument
            m_oDocument = New bACTDocument.Form
            m_lReturn = m_oDocument.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Check instance of bACTAllocate
            m_oAllocate = New bACTAllocate.Business
            m_lReturn = m_oAllocate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Get the Period

            m_lReturn = m_oPeriod.GetPostingPeriodForDate(dtDateInPeriod:=DateTime.Today, lPeriodID:=lPeriodID, lLedgerID:=gACTLibrary.ACTLedgerTypeCreditor)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Get The Insurer Suspense Account

            m_lReturn = m_oExplorer.GetAccountIdFromShort(v_sShortCode:="INSURERSUSPENSE", r_vAccountIds:=vlAccountIds)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Informations.IsArray(vlAccountIds)) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If

            ' Store suspense account id

            lSuspenseAccountId = CInt(vlAccountIds(0, 0))

            ' Wrap code in transaction (assume transaction has started)
            bTransactionStarted = True
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to begin database transaction")
            End If

            ' Set group and range code
            sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1
            sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeJn

            ' Use company_id from first transaction


            m_lReturn = m_oTransdetail.GetDetails(vTransdetailID:=CInt(v_vTransdetailIds(0)))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to process first transaction")
            End If

            ' Use getnext to get company details


            m_lReturn = m_oTransdetail.GetNext(vTransdetailID:=CInt(v_vTransdetailIds(0)), vCompanyID:=lCompanyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to get company id")
            End If

            ' Generate a document number for a Journal document
            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = m_oDocument.GenerateDocumentReferenceNumber(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sDocumentRef)
            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to generate journal document number")
            End If

            ' Calculate document reference
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            sDocumentRef = sRangeCode & sDocumentRef

            ' Create the document

            m_lReturn = m_oDocument.DirectAdd(vDocumentID:=lDocumentID, vCompanyID:=lCompanyID, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, vDocumenttypeID:=gACTLibrary.ACTDocTypeJournal, vDocumentRef:=sDocumentRef, vDocumentDate:=DateTime.Today, vCreatedDate:=DateTime.Today, vAuthorisedDate:=DateTime.Today, vComment:="")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, " + "Unable to create document: " & sDocumentRef)
            End If


            ' It isn't, set current amount
            cCurrencyAmount = v_cPayment
            dConversionRate = 0

            ' Get base amount

            m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=v_iCurrencyID, lCompanyID:=lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cCurrencyAmount, vConversionDate:=DateTime.Today, vConversionRate:=dConversionRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to convert payment amount to base currency")
            End If

            ' Generate Journal transaction to Insurer Suspense

            m_lReturn = m_oTransdetail.DirectAdd(vTransdetailID:=lTransID, vAccountID:=lSuspenseAccountId, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, vCompanyID:=lCompanyID, vCurrencyID:=v_iCurrencyID, vPeriodID:=lPeriodID, vDocumentID:=lDocumentID, vDocumentSequence:=2, vAccountingDate:=DateTime.Today, vAmount:=cBaseAmount, vBaseAmountUnrounded:=cBaseAmount, vFullyMatched:=-1, vCurrencyAmount:=cCurrencyAmount, vCurrencyAmountUnrounded:=cCurrencyAmount, vCurrencyBaseXrate:=dConversionRate, vOperatorID:=m_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to create journal to insurer suspense account")
            End If

            ' Store suspense trans ID
            lSuspenseID = lTransID

            ' Generate Journal transaction to Insurer

            m_lReturn = m_oTransdetail.DirectAdd(vTransdetailID:=lTransID, vAccountID:=v_lAccountId, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, vCompanyID:=lCompanyID, vCurrencyID:=v_iCurrencyID, vPeriodID:=lPeriodID, vDocumentID:=lDocumentID, vDocumentSequence:=1, vAccountingDate:=DateTime.Today, vAmount:=cBaseAmount * -1, vBaseAmountUnrounded:=cBaseAmount * -1, vFullyMatched:=-1, vCurrencyAmount:=cCurrencyAmount * -1, vCurrencyAmountUnrounded:=cCurrencyAmount * -1, vCurrencyBaseXrate:=dConversionRate, vOperatorID:=m_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to create journal to insurer")
            End If

            ' Mark this transaction so it will be included in our bindering allocation
            m_lReturn = MarkTransaction(v_lTransactionID:=lTransID, v_iCurrencyID:=v_iCurrencyID, v_cPayment:=cCurrencyAmount * -1)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to mark binder journal transaction")
            End If

            ' Add to transaction array
            ReDim Preserve v_vTransdetailIds(v_vTransdetailIds.GetUpperBound(0) + 1)

            v_vTransdetailIds(v_vTransdetailIds.GetUpperBound(0)) = lTransID

            ' Prepare the allocation

            m_oAllocate.AccountID = v_lAccountId

            m_oAllocate.CompanyId = lCompanyID

            ' Process the allocation

            m_lReturn = m_oAllocate.Allocate(v_bInsurerBinder:=True, v_vTransIDs:=v_vTransdetailIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to allocate binder journal transactions")
            End If

            ' Write Reversinging Journal from Suspense Back into Original Insurer
            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = m_oDocument.GenerateDocumentReferenceNumber(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, v_iUserID:=m_iUserID, v_iCompanyID:=lCompanyID, r_sDocumentRef:=sDocumentRef)
            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to generate reversing journal document number")
            End If

            ' Generate document ref
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            sDocumentRef = sRangeCode & sDocumentRef

            ' Create document

            m_lReturn = m_oDocument.DirectAdd(vDocumentID:=lDocumentID, vCompanyID:=lCompanyID, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, vDocumenttypeID:=gACTLibrary.ACTDocTypeJournal, vDocumentRef:=sDocumentRef, vDocumentDate:=DateTime.Today, vCreatedDate:=DateTime.Today, vAuthorisedDate:=DateTime.Today, vComment:="Consolidated Binder")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to create reversing journal document")
            End If

            ' Generate Journal transaction to Insurer

            m_lReturn = m_oTransdetail.DirectAdd(vTransdetailID:=lTransID, vAccountID:=v_lAccountId, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, vCompanyID:=lCompanyID, vCurrencyID:=v_iCurrencyID, vPeriodID:=lPeriodID, vDocumentID:=lDocumentID, vDocumentSequence:=1, vAccountingDate:=DateTime.Today, vAmount:=cBaseAmount, vBaseAmountUnrounded:=cBaseAmount, vFullyMatched:=-1, vCurrencyAmount:=cCurrencyAmount, vCurrencyAmountUnrounded:=cCurrencyAmount, vCurrencyBaseXrate:=dConversionRate, vOperatorID:=m_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to create journal transaction to insurer")
            End If

            ' Generate Journal transaction from Insurer Suspense

            m_lReturn = m_oTransdetail.DirectAdd(vTransdetailID:=lTransID, vAccountID:=lSuspenseAccountId, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, vCompanyID:=lCompanyID, vCurrencyID:=v_iCurrencyID, vPeriodID:=lPeriodID, vDocumentID:=lDocumentID, vDocumentSequence:=2, vAccountingDate:=DateTime.Today, vAmount:=cBaseAmount * -1, vBaseAmountUnrounded:=cBaseAmount * -1, vFullyMatched:=-1, vCurrencyAmount:=cCurrencyAmount * -1, vCurrencyAmountUnrounded:=cCurrencyAmount * -1, vCurrencyBaseXrate:=dConversionRate, vOperatorID:=m_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to create journal transaction to insurer suspense")
            End If

            ' Set up dummy marking records so that allocation will match records on the suspense account
            m_lReturn = MarkTransaction(v_lTransactionID:=lSuspenseID, v_iCurrencyID:=v_iCurrencyID, v_cPayment:=cCurrencyAmount)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to mark suspense transaction")
            End If

            m_lReturn = MarkTransaction(v_lTransactionID:=lTransID, v_iCurrencyID:=v_iCurrencyID, v_cPayment:=cCurrencyAmount * -1)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to mark journal transaction")
            End If

            ' Set up allocation

            m_oAllocate.AccountID = lSuspenseAccountId

            ' Allocate journal

            m_lReturn = m_oAllocate.Allocate(v_bInsurerBinder:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", ProcessBinder, Unable to allocate suspense journal transactions")
            End If

        Catch ex As Exception
            ' Set return
            result = If(Informations.Err().Number = Constants.vbObjectError, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessBinder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally
            ' Are we in a transaction?
            If bTransactionStarted Then
                ' If we are currently returning success commit the transaction
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = m_oDatabase.SQLCommitTrans()
                End If

                ' If we are not returning success (even from above, hence no else!!) rollback the transaction
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = m_oDatabase.SQLRollbackTrans()
                End If
            End If

        End Try
        Return result
    End Function

    Public Function SearchDetails(ByVal v_vAccountID As Integer, Optional ByVal v_vDateTo As Object = Nothing,
                                    Optional ByVal v_bDateByTrans As Object = Nothing, Optional ByVal v_lMarkedStatus As Integer = 0,
                                    Optional ByVal v_lMonth As Integer = 0, Optional ByVal v_sLedgerCode As String = "",
                                    Optional ByVal v_sAlternateRef As String = "", Optional ByRef r_vResultArray(,) As Object = Nothing,
                                    Optional ByVal vQueryStringPeriod As Object = Nothing, Optional ByVal vQueryStringyear As Object = Nothing, Optional ByVal v_iCurrencyId As Integer = 0,
                                    Optional ByVal v_vDueDateFrom As Object = Nothing, Optional ByVal v_vDueDateTo As Object = Nothing,
                                    Optional ByVal v_bInstalmentByDuedate As Object = Nothing,
                                    Optional ByVal v_sReference As String = "",
                                    Optional ByVal v_bGrossAgent As Object = Nothing,
                                    Optional ByVal v_lMediaTypeId As Integer = 0) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get cheked list box item
            Dim sYearName As String = ""
            Dim sPeriodName As String = ""
            Dim iCNT As Integer = 0
            If Informations.IsArray(vQueryStringyear) Then
                For iCNT = 0 To vQueryStringyear.GetUpperbound(0)
                    If iCNT > 0 Then
                        sYearName = sYearName + ","
                    End If
                    sYearName = sYearName + vQueryStringyear(iCNT)
                Next
            End If
            iCNT = 0
            If Informations.IsArray(vQueryStringPeriod) Then
                For iCNT = 0 To vQueryStringPeriod.GetUpperbound(0)
                    If iCNT > 0 Then
                        sPeriodName = sPeriodName + ","
                    End If
                    sPeriodName = sPeriodName + vQueryStringPeriod(iCNT)
                Next
            End If
            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "account_id", v_vAccountID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            'bPMAddParameter.AddParameterLite(m_oDatabase, "date_to", v_vDateTo, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "date_by_Trans", v_bDateByTrans, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "marked_status", v_lMarkedStatus, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "month", v_lMonth, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            If Not String.IsNullOrEmpty(v_sAlternateRef) Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "alternate_reference", v_sAlternateRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) 'PN 33593 (RC)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "alternate_reference", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) 'PN 33593 (RC)
            End If
            If Not String.IsNullOrEmpty(sYearName) Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "yearname", sYearName.Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            End If
            If Not Informations.IsNothing(v_vDueDateFrom) Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "due_date_from", v_vDueDateFrom, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If

            If Not String.IsNullOrEmpty(sPeriodName) Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "periodname", sPeriodName.Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            End If
            If v_iCurrencyId <> 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "currencyid", v_iCurrencyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If

            If Not Informations.IsNothing(v_vDueDateTo) Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "due_date_to", v_vDueDateTo, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If
            bPMAddParameter.AddParameterLite(m_oDatabase, "instalment_by_duedate", v_bInstalmentByDuedate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            bPMAddParameter.AddParameterLite(m_oDatabase, "reference", v_sReference, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "gross_agent", v_bGrossAgent, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            bPMAddParameter.AddParameterLite(m_oDatabase, "mediatype", v_lMediaTypeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' Call the new all in-one procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsurerPaymentsSQL, sSQLName:=ACInsurerPaymentsName, bStoredProcedure:=ACInsurerPaymentsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray, bKeepNulls:=True)

            ' Check result
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Check there's some results
                If Not Informations.IsArray(r_vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        ' Do nothing. Not important
        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Public Function SetStatus(ByVal sProcessStatus As String, ByVal sMapStatus As String, ByVal sStepStatus As String) As Integer

        ' Do nothing. Not important
        Return gPMConstants.PMEReturnCode.PMTrue

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
                ReleaseObject(m_oCurrencyConvert)
                ReleaseObject(m_oMatchPost)
                ReleaseObject(m_oDocumentPost)
                ReleaseObject(m_oAutoNumber)
                ReleaseObject(m_oExplorer)
                ReleaseObject(m_oDocument)
                ReleaseObject(m_oTransdetail)
                ReleaseObject(m_oPeriod)
                ReleaseObject(m_oAllocate)
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' UnMarks a transaction.
    Public Function UnMarkTransaction(ByVal v_lTransDetailId As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Set default records affected
            lRecordsAffected = gPMConstants.PMAllRecords

            ' Clear paramters
            bPMAddParameter.AddParameterLite(m_oDatabase, "transdetail_id", v_lTransDetailId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            ' Perform Query
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUnmarkTransactionSQL, sSQLName:=ACUnmarkTransactionName, bStoredProcedure:=ACUnmarkTransactionStored, lRecordsAffected:=lRecordsAffected)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return PMNotFound if no records were deleted
            If lRecordsAffected = 0 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnMarkTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnMarkTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function AddWriteOffTransaction(ByVal Docid As Integer, ByVal Account_id As Integer, ByVal WriteOffAccID As Integer, ByVal WriteOffamt As Decimal, Optional ByVal v_sAltReferance As String = "", Optional ByRef m_lTransDetailID As Integer = 0) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vResult As Object = Nothing
        Dim vResultPeriod As Object = Nothing
        Dim lTransDetailID As Integer

        'get Currency details and document sequence no. from Transdetail table
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Doc_id", vValue:=CStr(Docid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCurrDetailsSQL, sSQLName:=ACCurrDetailsName, bStoredProcedure:=False, vResultArray:=vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vResult) Then

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="date_in_period", vValue:=DateTime.Now.Date, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPeriodIdSQL,
                                        sSQLName:=ACGetPeriodIdName,
                                        bStoredProcedure:=True,
                                        vResultArray:=vResultPeriod)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get DocumentDetails

            m_lReturn = m_oDocumentPost.GetDocument(Docid)
            'Add Account Transaction Record

            m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=Account_id,
                                                        v_iCurrencyID:=vResult(0, 0),
                                                        v_cAmount:=WriteOffamt, v_cCurrencyAmount:=WriteOffamt,
                                                        v_vdCurrencyBaseXRate:=vResult(2, 0),
                                                        v_vDocumentSequence:=CDbl(vResult(1, 0)) + 1,
                                                        v_vComment:="WRITE OFF", v_vOperatorID:=m_iUserID,
                                                        v_vSpare:="WRITEOFF", v_vAccountingDate:=DateTime.Now,
                                                        v_vInsuranceRef:=vResult(3, 0),
                                                        r_vTransDetailId:=lTransDetailID,
                                                        v_periodID:=vResultPeriod(0, 0),
                                                        v_vReference:=v_sAltReferance)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'mark WriteOff transaction

            m_lReturn = MarkTransaction(lTransDetailID, CInt(vResult(0, 0)), WriteOffamt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError("AddWriteOffTransaction", "Not able to mark writeoff transaction")
                Return result
            End If

            'Add Writeoff Transaction Record


            m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=WriteOffAccID,
                                                        v_iCurrencyID:=vResult(0, 0),
                                                        v_cAmount:=WriteOffamt * -1,
                                                        v_cCurrencyAmount:=WriteOffamt * -1,
                                                        v_vdCurrencyBaseXRate:=vResult(2, 0),
                                                        v_vDocumentSequence:=CDbl(vResult(1, 0)) + 2,
                                                        v_vComment:="WRITE OFF", v_vOperatorID:=m_iUserID, v_vSpare:="WRITEOFF",
                                                        v_vAccountingDate:=DateTime.Now,
                                                        v_vInsuranceRef:=vResult(3, 0),
                                                        v_periodID:=vResultPeriod(0, 0))


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lTransDetailID = lTransDetailID
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result
    End Function

    Public Function DeleteWriteOffTransaction(ByVal Docid As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="nDocumentId", vValue:=CStr(Docid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        'delete from Transdetail
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteWOFFSQL, sSQLName:=ACDeleteWOFFName, bStoredProcedure:=ACDeleteWoOFFStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result
    End Function
    Public Function UpdateComment(ByVal v_lTransDetailId As Integer, ByVal v_sComment As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateComment"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            ' Add parameters as per new Standards
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "transdetail_id", v_lTransDetailId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "comment", v_sComment, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCommentSQL, sSQLName:=ACUpdateCommentName, bStoredProcedure:=ACUpdateCommentStored)
            ' Check return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("UpdateComment", "Update Comment Failed")
            End If

            Return result

        Catch ex As System.Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)
            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function

    Public Function LoadAllocationPeriod(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadAllocationPeriod"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Execute the stored procedure
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACLoadAllocationPeriodSQL, sSQLName:=ACLoadAllocationPeriodName, bStoredProcedure:=ACLoadAllocationPeriodStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray, bKeepNulls:=True)
            ' Check return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("LoadAllocationPeriod", "Load Allocation Period Failed")
            End If

            Return result

        Catch ex As System.Exception

            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

            Return result
        End Try

    End Function

    ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - 4.5.1.1.1

    ' *******************************************************************************
    ' PUBLIC METHODS
    ' *******************************************************************************


    ' *******************************************************************************
    ' PRIVATE METHODS
    ' *******************************************************************************
    ' Macro function for loading business objects
    Private Function GetBusinessObject(ByRef r_oBusiness As Object, ByVal v_sName As String, Optional ByVal v_bForceRefresh As Boolean = False) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Check if object is already set or we are forcing
        If (r_oBusiness Is Nothing) Or v_bForceRefresh Then
            ' Create new instance
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=r_oBusiness, v_sClassName:=v_sName, v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel)

            ' Check return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Return result

    End Function

    ' Safely release an object
    Private Function ReleaseObject(ByRef businessObject As Object) As Integer

        Dim result As Integer = 0
        Try

            ' Terminate the object if it exists
            If businessObject Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMTrue
            Else

                businessObject.Dispose()
                businessObject = Nothing
            End If

            Return result

        Catch

            ' Return error
            Return gPMConstants.PMEReturnCode.PMError

            ' Just exit with current return value
        End Try
    End Function
    ' *******************************************************************************
    ' PRIVATE METHODS
    ' *******************************************************************************


    ' *******************************************************************************
    ' PRIVATE EVENTS
    ' *******************************************************************************
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub
    ' *******************************************************************************
    ' PRIVATE EVENTS
    ' *******************************************************************************

    Public Function GetAgentDetailForAccount(ByVal v_lAccountid As Long, ByRef r_vResultsArray(,) As Object) As Long

        Const kMethodName As String = "GetAgentDetailForAccount"

        Dim lReturn As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            Call AddParameterLite(m_oDatabase, "account_id", v_lAccountid, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(
                                    sSQL:=ACGetAgentDetailForAccountSQL,
                                    sSQLName:=ACGetAgentDetailForAccountName,
                                    bStoredProcedure:=ACGetAgentDetailForAccountStored,
                                    vResultArray:=r_vResultsArray,
                                    lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("spu_Get_Agent_Detail_ForAccount", "Get Agent Detail For Account  Failed")
            End If

            Return result
        Catch ex As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)
        End Try
        Return result
    End Function
    Private Function UpdateInstalmentNumber(ByVal v_iTransMatchId As Integer, _
                                ByVal v_iInstalmentNumber As Integer) As Integer

        ' Const kMethodName As String = "UpdateInstalmentNumber"

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add parameters as per new Standards
        m_oDatabase.Parameters.Clear()

        bPMAddParameter.AddParameterLite(m_oDatabase, "transmatch_id", v_iTransMatchId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        bPMAddParameter.AddParameterLite(m_oDatabase, "InstalmentNumber", v_iInstalmentNumber, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        ' Execute the stored procedure
        m_lReturn = m_oDatabase.SQLAction( _
                        sSQL:=ACUpdateInstalmentNumberSQL, _
                        sSQLName:=ACUpdateInstalmentNumberName, _
                        bStoredProcedure:=ACUpdateInstalmentNumberStored)
        ' Check return code
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            RaiseError("UpdateInstalmentNumber", "Update InstalmentNumber Failed")
        End If

        Return result
    End Function

    ' UnMarks a transaction.
    Public Function UnMarkInstTransaction(ByVal v_iTransdetailid As Integer, ByVal v_iInstalmentNumber As Integer) As Integer

        Dim lRecordsAffected As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Set default records affected
            lRecordsAffected = gPMConstants.PMAllRecords

            ' Clear paramters
            AddParameterLite(m_oDatabase, "transdetail_id", v_iTransdetailid, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "InstalmentNumber", v_iInstalmentNumber, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' Perform Query
            m_lReturn = m_oDatabase.SQLAction( _
                sSQL:=ACUnmarkInstTransactionSQL, _
                sSQLName:=ACUnmarkInstTransactionName, _
                bStoredProcedure:=ACUnmarkInstTransactionStored, _
                lRecordsAffected:=lRecordsAffected)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Return PMNotFound if no records were deleted
            If (lRecordsAffected = 0) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If

            Return result
        Catch ex As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="UnMarkInstTransaction", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)
        End Try
        Return result
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetInstalmentDetailsForInsurerPayment
    ' PURPOSE: gets instalment plan details for a given account
    ' AUTHOR:
    ' DATE:
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function GetInstalmentDetailsForInsurerPayment(ByVal v_iAccountid As Integer, ByVal v_sViewType As String, ByRef r_vInstalArray(,) As Object) As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add accountid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add( _
                    sName:="account_id", _
                    vValue:=v_iAccountid, _
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                    iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            ' Add viewtype as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="viewtype", vValue:=v_sViewType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect( _
                sSQL:=ACSelInstalmentsForPartySettelmentSQL, _
                sSQLName:=ACSelInstalmentsForPartySettelmentName, _
                bStoredProcedure:=True, _
                lNumberRecords:=gPMConstants.PMAllRecords, _
                vResultArray:=r_vInstalArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            Return result
        Catch ex As System.Exception
            ' Log Error Message
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetInstalmentDetailsForInsurerPayment", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)
        End Try
        Return result
    End Function
    Public Function DeleteTransMatchInst(ByVal v_iTransdetailid As Integer) As Integer

        Dim iRecordsAffected As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            ' Set default records affected
            iRecordsAffected = gPMConstants.PMAllRecords
            ' Clear paramters
            AddParameterLite(m_oDatabase, "transdetail_id", v_iTransdetailid, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            ' Perform Query
            m_lReturn = m_oDatabase.SQLAction(
                sSQL:=ACDeleteTransMatchInstSQL,
                sSQLName:=ACDeleteTransMatchInstName,
                bStoredProcedure:=ACDeleteTransMatchInstStored,
                lRecordsAffected:=iRecordsAffected)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Return PMNotFound if no records were deleted
            If (iRecordsAffected = 0) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If

            Return result
        Catch ex As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="DeleteTransMatchInst", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)
        End Try
        Return result
    End Function

    Public Function GetTranDetailContraEntriesForInstalments(ByVal v_sParam As String, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Add parameters as per new Standards
            m_oDatabase.Parameters.Clear()

            Call AddParameterLite(m_oDatabase, "Params", v_sParam, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect( _
                            sSQL:=ACGetTranDetailContraEntriesForInstalmentsSQL, _
                            sSQLName:=ACGetTranDetailContraEntriesForInstalmentsName, _
                            bStoredProcedure:=ACGetTranDetailContraEntriesForInstalmentsStored, _
                            vResultArray:=vResultArray)
            ' Check return code
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("GetTranDetailContraEntriesForInstalments", "GetTranDetailContraEntriesForInstalments failed.")
            End If

            Return result
        Catch ex As System.Exception
            ' Log Error Message
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetInstalmentDetailsForInsurerPayment", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)
        End Try
        Return result
    End Function

    Public Function GetTransDetailIdForSetteledPremium(ByVal v_iTransdetailid As Long, ByRef r_vResultsArray(,) As Object) As Integer

        Dim lReturn As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            Call AddParameterLite(m_oDatabase, "transdetail_id", v_iTransdetailid, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(
                                    sSQL:=ACGetTransDetailIdForSetteledPremiumSQL,
                                    sSQLName:=ACGetTransDetailIdForSetteledPremiumName,
                                    bStoredProcedure:=ACGetTransDetailIdForSetteledPremiumStored,
                                    vResultArray:=r_vResultsArray,
                                    lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetTransDetailIdForSetteledPremium", "Get TransDetailId For SetteledPremium  Failed")
            End If

            Return result
        Catch ex As System.Exception
            ' Log Error Message
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetInstalmentDetailsForInsurerPayment", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)
        End Try
        Return result
    End Function

    Public Function GetTransDetailsFromBatch(ByVal v_lBatchID As Long, _
                                            ByRef r_vResultArray(,) As Object) As Long

        Const kMethodName As String = "GetTransDetailsFromBatch"

        GetTransDetailsFromBatch = gPMConstants.PMEReturnCode.PMTrue
        Dim nReturn As Integer
        Try
            AddParameterLite(m_oDatabase, "Batch_set_id", v_lBatchID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            nReturn = m_oDatabase.SQLSelect( _
                                    sSQL:=KGetPMNavXMBatchTransactionDetailSQL, _
                                    sSQLName:=KGetPMNavXMBatchTransactionDetailName, _
                                    bStoredProcedure:=True, _
                                    vResultArray:=r_vResultArray, _
                                    lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to execute spu_Get_PMNav_Batch_Transaction_Details", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetTransDetailsFromBatch, excep:=ex)
        End Try
    End Function

    Public Function UpdateWriteOffDocumentRef(ByVal v_lOldDocumentId As Integer, _
                                            ByVal v_lNewDocumentId As Integer) As Integer


        Const kMethodName As String = "UpdateWriteOffDocumentRef"
        UpdateWriteOffDocumentRef = gPMConstants.PMEReturnCode.PMTrue
        Dim nReturn As Integer
        Try
            AddParameterLite(m_oDatabase, "iOldDocumentId", v_lOldDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "iNewDocumentID", v_lNewDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            nReturn = m_oDatabase.SQLAction( _
                                    sSQL:=KTUpdateWriteOffDocumentSQL, _
                                    sSQLName:=KTUpdateWriteOffDocumentSQL, _
                                    bStoredProcedure:=True)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to execute spu_Get_PMNav_Batch_Transaction_Details", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdateWriteOffDocumentRef, excep:=ex)
        End Try

    End Function

    ''' <summary>
    ''' SearchDetailsForBatch
    ''' </summary>
    ''' <param name="nBatchID"></param>
    ''' <param name="r_oaResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SearchDetailsForBatch(ByVal nBatchID As Integer, _
                                            ByRef r_oaResultArray As Object(,)) As Integer

        Try
            m_oDatabase.Parameters.Clear()
            Call AddParameterLite(m_oDatabase, _
                                    "nBatchID", nBatchID, _
                                    PMEParameterDirection.PMParamInput, _
                                    PMEDataType.PMInteger, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kInsurerPaymentsForBatchSQL, _
                                                sSQLName:=kInsurerPaymentsForBatchName, _
                                                bStoredProcedure:=kInsurerPaymentsForBatchStored, _
                                                lNumberRecords:=gPMConstants.PMAllRecords, _
                                                vResultArray:=r_oaResultArray, _
                                                bKeepNulls:=True)


            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Return PMEReturnCode.PMFalse
            Else
                If (Informations.IsArray(r_oaResultArray) = False) Then
                    Return PMEReturnCode.PMNotFound
                End If
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="SearchDetailsForBatch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchDetailsForBatch", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)
            Return PMEReturnCode.PMError
        End Try
    End Function
End Class
