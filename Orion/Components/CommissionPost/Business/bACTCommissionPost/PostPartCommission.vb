Option Strict Off
Option Explicit On
Imports SSP.Shared
'Developer Guide No 129
Friend NotInheritable Class PostPartCommission
    '====================================================================
    '   Class/Module: PostPartCommission
    '   Description : Class implementation of the PostPartCommission use case
    '
    '====================================================================
    '   Maintenance History
    '
    '    25 November 2002    Paul Cunnigham    Created.
    '
    '====================================================================

    ' ************************************************
    ' Added to replace global variables 24/10/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    Private Const ACClass As String = "PostPartCommission"

    Private m_oDatabase As dPMDAO.Database
    Private m_oBusiness As Business

    Private Const ACPFSelectAgentAccountName As String = "GetCommisionAccount"
    'developer guide no. 39
    Private Const ACPFSelectAgentAccountSQL As String = "spu_ACT_PFSelect_Agent_Account"
    Private Const ACPFSelectAgentAccountStored As Boolean = True

    Private Const ACSelectCommissionAmountRemainingName As String = "GetCommissionAmountRemaining"
    'developer guide no. 39
    Private Const ACSelectCommissionAmountRemainingSQL As String = "spu_ACT_Select_Commission_Amount_Remaining"
    Private Const ACSelectCommissionAmountRemainingStored As Boolean = True

    Friend WriteOnly Property Business() As Business
        Set(ByVal Value As Business)

            m_oBusiness = Value

        End Set
    End Property

    Friend WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property

    Friend Function Start(ByRef r_lCommissionSuspendedTransDetailId As Integer, ByRef r_dPercentage As Double, ByRef r_bLastInstalment As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Start
        ' PURPOSE: Routine to handle the PostPartCommission use case
        '          (Post a debit to the suspense account that commission was initially credited to
        '          and a credit to the agent account)
        ' AUTHOR: Paul Cunnigham
        ' DATE: 25 November 2002, 17:17:23
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lAgentAccountId As Integer 'Id of the agent account that the commission will be paid to
        Dim crAmountToRelease As Decimal 'Amount of commission that will be posted to the agent account this will be a credit amount (i.e. negative)
        Dim lCommissionSuspenseAccountId As Integer 'Id of the suspence account that full commission amount was initially credited to
        Dim lDebitTransDetailId As Integer 'TransDetailId of the new DEBIT record created when commission has been posted (used for allocation)


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Must have business and database objects
            If m_oBusiness Is Nothing Or m_oDatabase Is Nothing Then
                Return result
            End If

            'Get the agent account - this is the account that the commission instalment
            'is to be posted to
            If GetAgentAccount(r_lTransDetailId:=r_lCommissionSuspendedTransDetailId, r_lAccount:=lAgentAccountId) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Get the amount of commission to release and the account that the commission
            'was originally credited to
            If GetCommissionDetails(r_lTransDetailId:=r_lCommissionSuspendedTransDetailId, r_bLastInstalment:=r_bLastInstalment, r_dPercentage:=r_dPercentage, r_crAmountToRelease:=crAmountToRelease, r_lCommissionSuspenseAccountId:=lCommissionSuspenseAccountId) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Post the commission entries...
            '(post a credit to lAgentAccountId and a debit to lCommissionSuspenseAccountId)
            '...return the Id of the TransDetail record (debit) we have just posted
            'so that we can allocate against orignal credit amount on the commission suspense account
            If PostCommissionInstalment(r_lCommissionSuspenseAccountId:=lCommissionSuspenseAccountId, r_lAgentAccountId:=lAgentAccountId, r_crAmount:=crAmountToRelease, r_lDebitTransDetailId:=lDebitTransDetailId) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Allocate the new commission instalment DEBIT (hence * -1) against the original credit amount
            If AllocateCommision(r_lCommissionSuspenseAccountId:=lCommissionSuspenseAccountId, r_lCommissionSuspendedTransDetailId:=r_lCommissionSuspendedTransDetailId, r_crAmount:=crAmountToRelease * -1, r_lDebitTransDetailId:=lDebitTransDetailId) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Start"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function GetAgentAccount(ByRef r_lTransDetailId As Integer, ByRef r_lAccount As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetAgentAccount
        ' PURPOSE: Get the commission account from a PFPremiumFinance record
        ' AUTHOR: Paul Cunnigham
        ' DATE: 25 November 2002, 17:13:09
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const klColAccountId As Integer = 0
        Const klFirstRow As Integer = 0



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the ComponentNameID INPUT parameter
        If m_oDatabase.Parameters.Add(sName:="lTransDetailId", vValue:=CStr(r_lTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACPFSelectAgentAccountSQL, sSQLName:=ACPFSelectAgentAccountName, bStoredProcedure:=ACPFSelectAgentAccountStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        Else
            'Populate the params

            r_lAccount = CInt(vResultArray(klColAccountId, klFirstRow))
        End If

        result = gPMConstants.PMEReturnCode.PMTrue

        Return result

    End Function

    Private Function GetCommissionDetails(ByRef r_lTransDetailId As Integer, ByRef r_dPercentage As Double, ByRef r_bLastInstalment As Boolean, ByRef r_crAmountToRelease As Decimal, ByRef r_lCommissionSuspenseAccountId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetCommissionDetails
        ' PURPOSE: Get details of the commission
        ' AUTHOR: Paul Cunningham
        ' DATE: 26 November 2002, 10:44:31
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oTransDetail As bACTTransdetail.Form



        result = gPMConstants.PMEReturnCode.PMFalse

        'Use the bACTTransDetail component to get the commission details

        'Create and initialise the business class
        oTransDetail = New bACTTransdetail.Form
        If oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        'Get details of the Amount and Account from the TransDetail record
        With oTransDetail

            If .GetDetails(vTransdetailID:=r_lTransDetailId) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If


            If .GetNext(vTransdetailID:=r_lTransDetailId, vCurrencyAmount:=r_crAmountToRelease, vAccountID:=r_lCommissionSuspenseAccountId) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If


            .Dispose()
        End With
        oTransDetail = Nothing

        'If it's the last instalment then we'll need to release the amount of commission
        'remaing - if its not then we release a percentage of the amount
        If r_bLastInstalment Then
            If GetCommissionAmountRemaining(r_lTransDetailId:=r_lTransDetailId, r_crAmount:=r_crAmountToRelease) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If
        Else
            r_crAmountToRelease *= r_dPercentage
        End If

        result = gPMConstants.PMEReturnCode.PMTrue

        Return result


    End Function

    Private Function GetCommissionAmountRemaining(ByRef r_lTransDetailId As Integer, ByRef r_crAmount As Decimal) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetCommissionAmountRemaining
        ' PURPOSE: Get the amount of commission that remains unpaid for a transaction
        ' AUTHOR: Paul Cunnigham
        ' DATE: 25 November 2002, 17:13:09
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const klColAmountId As Integer = 0
        Const klFirstRow As Integer = 0



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the ComponentNameID INPUT parameter
        If m_oDatabase.Parameters.Add(sName:="lTransDetailId", vValue:=CStr(r_lTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACSelectCommissionAmountRemainingSQL, sSQLName:=ACSelectCommissionAmountRemainingName, bStoredProcedure:=ACSelectCommissionAmountRemainingStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        If Not Informations.IsArray(vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        Else
            'Populate the params
            r_crAmount = gPMFunctions.NullToLong(Expression:=vResultArray(klColAmountId, klFirstRow))
        End If

        result = gPMConstants.PMEReturnCode.PMTrue

        Return result

    End Function

    Private Function PostCommissionInstalment(ByRef r_lCommissionSuspenseAccountId As Integer, ByRef r_lAgentAccountId As Integer, ByRef r_crAmount As Decimal, ByRef r_lDebitTransDetailId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PostCommissionInstalment
        ' PURPOSE: Posts the commission instalment credit) to the agent account and the balancing
        '          entry (debit) to the commission suspense account
        ' AUTHOR: Paul Cunningham
        ' DATE: 26 November 2002, 12:24:15
        ' OUT: r_lPaymentTransId - The Id of the newly created debit TransDetail record so that
        '                          allocation against the original credit can occur
        ' RETURNS: PMTrue for success
        '
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oDocumentPost As New bACTDocumentPost.Form
        Dim oPMAutoNumber As bACTAutoNumber.Business
        Dim sGroupCode As String = ""
        Dim sRangeCode As String = ""
        Dim sDocumentRef As String = ""
        Dim lDocumentId, lNumberRangeId, lDocumentSequence As Integer
        Dim dtAccountingDate As Date
        Dim lTransDetailId As Integer

        Dim cBaseAmount, cCurrencyAmount As Decimal
        Dim lEuroCurrencyID As Integer
        Dim cEuroAmount As Decimal
        Dim vdEuroBaseXrate As Object = Nothing
        Dim vdEuroCcyXrate As Object = Nothing
        Dim vdCurrencyBaseXRate As Object = Nothing
        Dim vdBaseAmountUnrounded As Object = Nothing
        Dim vdCurrencyAmountUnrounded As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1
            sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeJn

            'sw 03/06/2003 Added reference to database object to stop transaction problem
            oPMAutoNumber = New bACTAutoNumber.Business
            If oPMAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            With oPMAutoNumber

                If .GetNumberRange(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, r_lNumberRangeID:=lNumberRangeId) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return result
                End If
                'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

                If .GenerateDocumentReferenceNumber(v_sRangeCode:=sRangeCode, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sDocumentRef, v_lNumberRangeID:=lNumberRangeId) <> gPMConstants.PMEReturnCode.PMTrue Then
                    'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    Return result
                End If


                .Dispose()
            End With
            oPMAutoNumber = Nothing
            oDocumentPost = New bACTDocumentPost.Form
            If oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If
            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If sDocumentRef.Trim() <> "" Then
                sDocumentRef = sRangeCode & sDocumentRef
            End If
            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            dtAccountingDate = DateTime.Now


            If oDocumentPost.AddDocument(v_lDocumentTypeId:=gACTLibrary.ACTDocTypeJournal, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtAccountingDate, v_sComment:="Write Off", r_vDocumentID:=lDocumentId, r_vDocSourceID:=m_iSourceID) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Get the base amount to post
            '(we pass in r_crAmount as currency amount and routine returns amount to post in cBaseAmount)
            cCurrencyAmount = r_crAmount


            If GetBaseAmountFromCurrency(v_iCurrencyID:=m_iCurrencyID, v_iCompanyID:=m_iSourceID, r_cBaseAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, r_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_dtAccountingDate:=dtAccountingDate, r_lEuro:=lEuroCurrencyID, r_cEuroAmount:=cEuroAmount, r_vEuroCCyXrate:=vdEuroCcyXrate, r_vEuroBaseXRate:=vdEuroBaseXrate, r_vCCyAmountUnrounded:=vdCurrencyAmountUnrounded, r_vBaseAmountUnrounded:=vdBaseAmountUnrounded) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Post the credit to the agent account and get the TransDetailId (r_lPaymentTransId)
            'so that we can allocate it against the original commission amount
            lDocumentSequence = 1

            If oDocumentPost.AddTransaction(v_lAccountID:=r_lAgentAccountId, v_vDocumentSequence:=lDocumentSequence, v_iCurrencyID:=m_iCurrencyID, v_cAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vComment:="", r_vTransDetailId:=lTransDetailId, v_vAccountingDate:=dtAccountingDate) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Post the matching debit to the commission suspense account
            lDocumentSequence += 1

            If oDocumentPost.AddTransaction(v_lAccountID:=r_lCommissionSuspenseAccountId, v_vDocumentSequence:=lDocumentSequence, v_iCurrencyID:=m_iCurrencyID, v_cAmount:=cBaseAmount * -1, v_cCurrencyAmount:=cCurrencyAmount * -1, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vComment:="", r_vTransDetailId:=r_lDebitTransDetailId, v_vAccountingDate:=dtAccountingDate) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If


            If oDocumentPost.Commit() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------
        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostCommissionInstalment"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select

        Finally
            If Not (oDocumentPost Is Nothing) Then

                oDocumentPost.Dispose()
                oDocumentPost = Nothing
            End If
        End Try


        Return result

    End Function

    Private Function AllocateCommision(ByRef r_lCommissionSuspenseAccountId As Integer, ByRef r_lCommissionSuspendedTransDetailId As Integer, ByRef r_crAmount As Decimal, ByRef r_lDebitTransDetailId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AllocateCommision
        ' PURPOSE: Allocate the commission instalment against the original amount
        ' AUTHOR: Paul Cunningham
        ' DATE: 26 November 2002, 12:05:02
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        'Set module level vars and use existing routine to alocate the new debit posting
        'against the orignal instalment credit
        With m_oBusiness
            .CommissionAccountId = r_lCommissionSuspenseAccountId
            .OriginalTransDetailId = r_lCommissionSuspendedTransDetailId
            'This will be a debit amount (i.e. positive)
            .CommissionAmount = r_crAmount
            .PaymentTransactionId = r_lDebitTransDetailId

            If .AllocateCommission() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
        End With

        result = gPMConstants.PMEReturnCode.PMTrue


        Return result


    End Function

    'developer guide no. 101
    'Private Function GetBaseAmountFromCurrency(ByVal v_iCurrencyID As Integer, ByVal v_iCompanyID As Integer, ByRef r_cBaseAmount As Decimal, ByVal v_cCurrencyAmount As Decimal, ByRef r_vdCurrencyBaseXRate As Byte, ByVal v_dtAccountingDate As Date, ByRef r_lEuro As Integer, ByRef r_cEuroAmount As Decimal, ByRef r_vEuroCCyXrate As Object, ByRef r_vEuroBaseXRate As Object, ByRef r_vCCyAmountUnrounded As Object, ByRef r_vBaseAmountUnrounded As Object) As Integer
    Private Function GetBaseAmountFromCurrency(ByVal v_iCurrencyID As Integer, ByVal v_iCompanyID As Integer, ByRef r_cBaseAmount As Decimal, ByVal v_cCurrencyAmount As Decimal, ByRef r_vdCurrencyBaseXRate As Object, ByVal v_dtAccountingDate As Date, ByRef r_lEuro As Integer, ByRef r_cEuroAmount As Decimal, ByRef r_vEuroCCyXrate As Object, ByRef r_vEuroBaseXRate As Object, ByRef r_vCCyAmountUnrounded As Object, ByRef r_vBaseAmountUnrounded As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetBaseAmountFromCurrency
        ' PURPOSE: Routine to get the currency amount - blagged this from CashListPost
        ' AUTHOR: Paul Cunningham
        ' DATE: 27 November 2002, 11:19:20
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oCurrencyConvert As bACTCurrencyConvert.Form
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMFalse
        oCurrencyConvert = New bACTCurrencyConvert.Form
        lReturn = CType(oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBaseAmountFromCurrency Failed To create bACTCurrencyConvert.Form", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetBaseAmountFromCurrency"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            Return result
        End If

        r_vdCurrencyBaseXRate = 0


        If oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=v_iCurrencyID, lCompanyID:=v_iCompanyID, cBaseAmount:=r_cBaseAmount, cCurrencyAmount:=v_cCurrencyAmount, vConversionDate:=v_dtAccountingDate, vConversionRate:=r_vdCurrencyBaseXRate, vIsMultiplier:=False, vRounded:=Nothing, vBaseRoundingDifference:=Nothing, vCurrencyRoundingDifference:=Nothing, vFormattedBase:=Nothing, vFormattedCurrency:=Nothing, lEuro:=r_lEuro, cEuroAmount:=r_cEuroAmount, vEuroCCyXrate:=r_vEuroCCyXrate, vEuroBaseXRate:=r_vEuroBaseXRate, vCCyAmountUnRounded:=r_vCCyAmountUnrounded, vBaseAmountUnRounded:=r_vBaseAmountUnrounded) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If


        oCurrencyConvert.Dispose()
        oCurrencyConvert = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue


        Return result

    End Function

    Protected Overrides Sub Finalize()

        'Free object variales
        m_oBusiness = Nothing
        m_oDatabase = Nothing

    End Sub
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
