Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	' Main public constant for all functions to identify which application this is
	Public Const ACApp As String = "uctCLMListVersionControl"
	
    ' Public instance of the object manager.

    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	' images
    Public Const kImageFolderClosed As Integer = 0
    Public Const kImageFolderOpen As Integer = 1
    Public Const kImageFolderSelected As Integer = 2
	
	' node keys
	Public Const ktvwNodeKeyREALROOT As String = "REALROOT"
	Public Const ktvwNodeKeyALL As String = "ALL"
	Public Const ktvwNodeKeyOPEN As String = "OPEN"
	Public Const ktvwNodeKeyINFOONLY As String = "INFOONLY"
	Public Const ktvwNodeKeySETTLED As String = "SETTLED"
	
	' node tags
	Public Const ktvwNodeTagClaimNode As String = "CLAIM"
	
	' claim status details
	Public Const kClaimStatusIdClosed As Integer = 3
	Public Const kClaimStatusIdReClosed As Integer = 5
	
	
	' claims details data array positions
	Public Const kClaimDetailsClaimId As Integer = 0
	Public Const kClaimDetailsClaimStatusId As Integer = 1
	Public Const kClaimDetailsInfoOnly As Integer = 2
	Public Const kClaimDetailsInsuranceRef As Integer = 3
	Public Const kClaimDetailsDescription As Integer = 4
	Public Const kClaimDetailsClaimNumber As Integer = 5
	Public Const kClaimDetailsProductId As Integer = 6
	Public Const kClaimDetailsProductDescription As Integer = 7
	Public Const kClaimDetailsCaseNumber As Integer = 13
	Public Const kClaimDetailsIsOtherClaim As Integer = 15
	
	' claim version details data array positions
	Public Const kClaimVersionDetailsClaimId As Integer = 0
	Public Const kClaimVersionDetailsVersionId As Integer = 1
	Public Const kClaimVersionDetailsCreateDate As Integer = 2
	Public Const kClaimVersionDetailsTransactionType As Integer = 3
	Public Const kClaimVersionDetailsVersionDescription As Integer = 4
	Public Const kClaimVersionDetailsTotalIncurred As Integer = 5
	Public Const kClaimVersionDetailsTotalPaid As Integer = 6
	Public Const kClaimVersionDetailsThisReserveRevision As Integer = 7
	Public Const kClaimVersionDetailsThisReservePayment As Integer = 8
	Public Const kClaimVersionDetailsThisSalvageRecovery As Integer = 9
	Public Const kClaimVersionDetailsThisThirdPartyRecovery As Integer = 10
	Public Const kClaimVersionDetailsCurrentReserve As Integer = 11
	Public Const kClaimVersionDetailsInsuranceFileCurrency As Integer = 12
	Public Const kClaimVersionDetailsClaimCurrency As Integer = 13
	Public Const kClaimVersionDetailsCreatedBy As Integer = 14
	Public Const kClaimVersionDetailsClaimDescription As Integer = 15
	Public Const kClaimVersionDetailsInsuranceRef As Integer = 16
	Public Const kClaimVersionDetailsInsuranceFileCnt As Integer = 17
	Public Const kClaimVersionDetailsClaimNumber As Integer = 18
	Public Const kClaimVersionDetailsRiskCnt As Integer = 19
	Public Const kClaimVersionDetailsClientShortName As Integer = 20
	Public Const kClaimVersionDetailsLossFromDate As Integer = 21
	Public Const kClaimVersionDetailsInsuranceHolderShortname As Integer = 22
	Public Const kClaimVersionDetailsInsuranceFolderCnt As Integer = 23
	Public Const kClaimVersionDetailsTransactionTypeCode As Integer = 24
	
	
	
	' claim version listview index
	Public Const klvwIndexClaimId As Integer = 1
	Public Const klvwIndexVersion As Integer = 2
	Public Const klvwIndexTransactionDate As Integer = 3
	Public Const klvwIndexTransactionType As Integer = 4
	Public Const klvwIndexVersionDescription As Integer = 5
	Public Const klvwIndexTotalIncurred As Integer = 6
	Public Const klvwIndexTotalPaid As Integer = 7
	Public Const klvwIndexThisRevision As Integer = 8
	Public Const klvwIndexThisPayment As Integer = 9
	Public Const klvwIndexThisThirdPartyRecovery As Integer = 11
	Public Const klvwIndexThisSalvageRecovery As Integer = 10
	Public Const klvwIndexCurrentReserve As Integer = 12
	Public Const klvwIndexPolicyCurrency As Integer = 13
	Public Const klvwIndexLossCurrency As Integer = 14
	Public Const klvwIndexUser As Integer = 15
	
	' claim version listview key
	Public Const klvwKeyClaimId As String = "claimid"
	Public Const klvwKeyVersion As String = "version"
	Public Const klvwKeyTransactionDate As String = "transactiondate"
	Public Const klvwKeyTransactionType As String = "transactiontype"
	Public Const klvwKeyVersionDescription As String = "versiondescription"
	Public Const klvwKeyTotalIncurred As String = "totalincurred"
	Public Const klvwKeyTotalPaid As String = "totalpaid"
	Public Const klvwKeyThisRevision As String = "thisrevision"
	Public Const klvwKeyThisPayment As String = "thispayment"
	Public Const klvwKeyThisThirdPartyRecovery As String = "thisthirdpartyrecovery"
	Public Const klvwKeyThisSalvageRecovery As String = "thissalvagerecovery"
	Public Const klvwKeyCurrentReserve As String = "currentreserve"
	Public Const klvwKeyPolicyCurrency As String = "policycurrency"
	Public Const klvwKeyLossCurrency As String = "losscurrency"
	Public Const klvwKeyUser As String = "user"
	
	' claim version listview text
	Public Const klvwTextClaimId As String = "claimid"
	Public Const klvwTextVersion As String = "Version"
	Public Const klvwTextTransactionDate As String = "Transaction Date"
	Public Const klvwTextTransactionType As String = "Transaction Type"
	Public Const klvwTextVersionDescription As String = "Version Description"
	Public Const klvwTextTotalIncurred As String = "Total Incurred"
	Public Const klvwTextTotalPaid As String = "Total Paid"
	Public Const klvwTextThisRevision As String = "This Revision"
	Public Const klvwTextThisPayment As String = "This Payment"
	Public Const klvwTextThisThirdPartyRecovery As String = "This Third Party Recovery"
	Public Const klvwTextThisSalvageRecovery As String = "This Salvage Recovery"
	Public Const klvwTextCurrentReserve As String = "Current Reserve"
	Public Const klvwTextPolicyCurrency As String = "Policy Currency"
	Public Const klvwTextLossCurrency As String = "Loss Currency"
	Public Const klvwTextUser As String = "User"
End Module