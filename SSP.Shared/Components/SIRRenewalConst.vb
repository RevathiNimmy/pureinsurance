Option Strict Off
Option Explicit On
Imports System
Public Module RenewalConst
	'***************************************************************************
	' To Access the default renewal settings
	Public Const PMRenewalDefaultSettingId As Integer = -1
	'***************************************************************************
	
	'***************************************************************************
	' Renewal Status Type Codes
	'***************************************************************************
	Public Const PMRenewalStatusTypePreSelection As String = "PRERENSEL"
	Public Const PMRenewalStatusTypeRenewalSelected As String = "RENSEL"
	Public Const PMRenewalStatusTypeRenewalQuoted As String = "RENQUOTED"
	Public Const PMRenewalStatusTypeRenewalInvited As String = "INVITED"
	Public Const PMRenewalStatusTypeRenewalPending As String = "CONFPEND"
	Public Const PMRenewalStatusTypeLapsePending As String = "LAPSEPEND"
	Public Const PMRenewalStatusTypeRenewalConfirmed As String = "RENEWCONF"
	Public Const PMRenewalStatusTypePolicyLapseConfirmed As String = "LAPSECONF"
	Public Const PMRenewalStatusTypePolicyLapsed As String = "LAPSED"
	Public Const PMRenewalStatusTypePolicyRenewed As String = "RENEWED"
	Public Const PMRenewalStatusTypeCompletedAlternateInsurer As String = "COMPALT"
	
	'***************************************************************************
	'Event Type Code
	'***************************************************************************
	Public Const PMRenewalEventType As String = "RENEWAL"
	Public Const PMRenewalPolicyChange As String = "RENPOLCHG"
	
	'***************************************************************************
	'Event Descriptions
	'***************************************************************************
	Public Const PMRenewalEventTypeDescPreRenewalSelect As String = "Policy Pre Selected For Renewal"
	Public Const PMRenewalEventTypeDescRenewalSelect As String = "Policy Selected For Renewal"
	Public Const PMRenewalEventTypeDescQuoteBrokerSelect As String = "Renewal Terms Generated - Broker Led"
	Public Const PMRenewalEventTypeDescQuoteInsurerSelect As String = "Renewal Terms Generated - EDI Renewal"
	Public Const PMRenewalEventTypeInvited As String = "Policy Invited for Renewal"
	'SJ 16/04/2004 - start
	Public Const PMRenewalEventTypeInvitedEdi As String = "Policy Invitation Message Sent To Broker"
	Public Const PMRenewalEventTypeInvitedEdiResend As String = "Policy Invitation Message Re-Sent To Broker"
	Public Const PMRenewalEventTypeRevokeConfirm As String = "Policy confirmation revoked"
	Public Const PMRenewalEventTypeRevokeLapse As String = "Policy lapse revoked"
	'SJ 16/04/2004 - end
	Public Const PMRenewalEventTypeConfirmed As String = "Policy Confirmed for Renewal"
	Public Const PMRenewalEventTypeCompletionLapse As String = "Policy Lapsed"
	Public Const PMRenewalEventTypeAlternateLapse As String = "Policy Lapsed - Alternate Insurer Chosen"
	Public Const PMRenewalEventTypeCompletionHoldingInsurer As String = "Policy Renewed with Holding Insurer"
	Public Const PMRenewalEventTypeCompletionAlternateInsurer As String = "Policy Arranged with Alternate Insurer"
	Public Const PMRenewalEventTypeMtaAtRenewal As String = "MTA At Renewal Transacted"
	'SSL 29/05/2001
	Public Const PMRenewalEventTypeLapseConfirmed As String = "Policy Marked to Lapse at Renewal"
	'sj 11/09/2001 - start
	Public Const PMRenewalEventTypeInvitePreferredQuotes As String = "Preferred Quotes Offered at Invite"
	Public Const PMRenewalEventTypeConfDocsHoldingInsurer As String = "Policy Confirmation Documents Printed"
	'sj 11/09/2001 - end
	'sj 13/09/2001 - start
	Public Const PMRenewalEventTypeReprintInvitation As String = "Policy Invitation Documents - Reprinted"
	'sj 13/09/2001 - end
	'sj 02/10/2001 - start
	Public Const PMRenewalEventTypeInsurerQuotationRebroke As String = "Policy Manually Rebroked - EDI Renewals"
	'sj 02/10/2001 - end
	'SJ 21/04/2004 - start
	Public Const PMRenewalEventTypeTransferPolicyToStandardRenewals As String = "Transfer Policy To Standard Renewals"
	Public Const PMRenewalEventTypeRiskChangeSuspension As String = "Policy Suspended At Confirmation Due To Change Of Risk"
	Public Const PMRenewalEventTypeRiskChangeSuspensionShort As String = "Risk Change"
	'SJ 21/04/2004 - end
	'***************************************************************************
	'CTAF 170401
	'Suspension Levels
	'***************************************************************************
	Public Const PMRenewalSuspensionLevelNone As Integer = 0
	Public Const PMRenewalSuspensionLevelSuspended As Integer = 1
	Public Const PMRenewalSuspensionLevelStopped As Integer = 2
	
	
	'***************************************************************************
	'SSL 26/04/2001
	'Motor & Household constants related to the output generated from RnMsg.exe
	'***************************************************************************
	'Motor constants - Renewal type constants
	Public Const PMMotorRenewalGeneral As String = " "
	Public Const PMMotorRenewalExcess As String = "X"
	Public Const PMMotorRenewalEndorse As String = "E"
	
	'Renewal status constants
	Public Const PMRenewalOkay As Integer = 0
	Public Const PMPolNoNotFound As Integer = 1
	Public Const PMValidationFailure As Integer = 2
	Public Const PMInvalidSecondSegment As Integer = 3 ' Invalid second segment in MHD element
	Public Const PMZeroFileSize As Integer = 4 ' Record this in case the output file is 0KB
	Public Const PMRnMSGFailed As Integer = 5 ' RnMSG is a cobol executable file
	Public Const PMFileNotCreated As Integer = 6
	Public Const PMDecodeError As Integer = 7
	Public Const PMCorruptFile As Integer = 10
	Public Const PMNoInsuranceFileCnt As Integer = 11
	Public Const PMFailedToSetStatusToPreRenSel As Integer = 12
	Public Const PMFailedToGetExcessEndorseCount As Integer = 13
	Public Const PMFailedToProcessMotorRenewalType As Integer = 14
	Public Const PMDirectAddRenEDIHouseholdFailed As Integer = 15
	Public Const PMDirectAddRenEDIHouseholdCyclesFailed As Integer = 16
	Public Const PMDirectAddRenEDIHouseholdPbItemsFailed As Integer = 17
	Public Const PMDirectAddRenEDIHouseholdHrItemsFailed As Integer = 18
	Public Const PMProcessHHRenewalTypeFailed As Integer = 19
	'Dragged from combined sourcesafe eck 10/12/03
	'IJR 2003-04-03 Start
	'Added to give greater detail to Motor PMValidationFailure return value
	Public Const PMValidationFailureVbsInitialisation As Integer = 3
	Public Const PMValidationFailureVbsDataLoad As Integer = 4
	Public Const PMValidationFailureEdiVersion As Integer = 5
	Public Const PMValidationFailureRegistrationNumberMismatch As Integer = 6
	Public Const PMValidationFailureIptNotIncluded As Integer = 7
	Public Const PMValidationFailureIptRateMismatch As Integer = 8
	Public Const PMValidationFailurePolicyStatus As Integer = 9
	'IJR 2003-04-03 End
	Public Const PMRenewalLapsedReasonLapsedByInsurer As String = "INSRLAPSED"
	
	
	Public Const PMRenewalComplete As Integer = 99
	
	'Household constants - Renewal type constants
	Public Const PMHHRenewalGeneral As String = " "
	Public Const PMHHRenewalRisk As String = "R"
	Public Const PMHHRenewalRiskSpi As String = "S"
	
	
	'AK 090501 - Revised the status according to database settings - begin
	'IM 050601 - Added ACStatusConfirmPending, ACStatusLapsePending
	' These are the values from the Renewal_Status_Type table
	Public Const ACStatusPreRenSel As String = "PRERENSEL"
	Public Const ACStatusSelected As String = "RENSEL"
	Public Const ACStatusQuote As String = "RENQUOTED"
	Public Const ACStatusInvite As String = "INVITED"
	Public Const ACStatusConfirm As String = "RENEWCONF"
	Public Const ACStatusConfirmPending As String = "CONFPEND"
	Public Const ACStatusLapsePending As String = "LAPSEPEND"
	Public Const ACStatusLapseConfirmed As String = "LAPSECONF"
	Public Const ACStatusRenewed As String = "RENEWED"
	Public Const ACStatusLapse As String = "LAPSED"
	Public Const ACStatusWhatIf As String = "WHATIF"
	Public Const ACStatusCompAlternate As String = "COMPALT"
	Public Const ACStatusIncomplete As String = "INCOMP"
	'AK 090501 - Revised the status according to database settings - end
	
	
	'AK - 160501 - Different Renewal Actions
	'IM - 050601 - Added DOCONF, DOLAPSE, REVCONF, REVLAPSE
	'IM - 250601 - Added QUOTEVIEW, QUOTEINV, QUOCONF, ACTQUOVW
	'IM - 040701 - Added UNDOLAPSE, UNDOCONFIRM
	'IM - 030801 - Added ALTCONF
	Public Const ACRenAltConfirm As String = "ALTCONF"
	Public Const ACRenMaintClaim As String = "MAINTCLM"
	Public Const ACRenMaintNCD As String = "MAINTNCD"
	Public Const ACRenSelect As String = "RENSELEC"
	Public Const ACRenAdvToQuote As String = "ADVQTE"
	Public Const ACRenManRebroke As String = "MANRBRK"
	Public Const ACRenWhatIfQuote As String = "WIFQTE"
	Public Const ACRenAdvToInvited As String = "ADVINV"
	Public Const ACRenRePrintInvitation As String = "RPINVDOC"
	Public Const ACRenLapseConf As String = "LAPSECONF"
	Public Const ACRenAdvToConfirmed As String = "ADVCFM"
	Public Const ACRenAdvToRenewed As String = "ADVRNW"
	Public Const ACRenRecToConfirmed As String = "PROCFM"
	Public Const ACRenRePrintConfirm As String = "RPCFMDOC"
	Public Const ACRenReSendEDI As String = "RSNDEDI"
	Public Const ACRenLapse As String = "LAPSE"
	Public Const ACRenAltQuote As String = "PROALTQTE"
	Public Const ACRenQuoteIns As String = "QTEINS"
	Public Const ACRenQuoteBrk As String = "QTEBRK"
	Public Const ACRenConfirm As String = "CONFIRM"
	Public Const ACRenDoConf As String = "DOCONF"
	Public Const ACRenDoLapse As String = "DOLAPSE"
	Public Const ACRenRevConf As String = "REVCONF"
	Public Const ACRenRevLapse As String = "REVLAPSE"
	Public Const ACRenQuoteView As String = "QUOTEVIEW"
	Public Const ACRenQuoteViewInvited As String = "QUOTEINV"
	Public Const ACRenQuoteViewConfPending As String = "QUOTECONF"
	Public Const ACRenQuoteViewConfirmed As String = "ACTQUOVW"
	Public Const ACRenUndoLapse As String = "UNDOLAPSE"
	Public Const ACRenUndoConfirm As String = "UNDOCONF"
	'AK 011001
	Public Const ACRenSuspend As String = "SUSPEND"
	'AK 111001
	Public Const ACRenOfferAlternative As String = "OFFERALT"
	
	'sj 11/09/2001 - start
	Public Const ACRenInvitePrefQuotes As String = "INVPREFQTE"
	Public Const ACRenConfDocsHoldingInsurer As String = "PCFMDOC"
	'sj 11/09/2001 - end
	'sj 09/10/2001 - start
	Public Const ACRenConfirmAlternate As String = "CONFALT"
	'sj 09/10/2001 - end
	
	'SJ 21/04/2004 - start
	Public Const ACRenHistory As String = "HISTORY"
	Public Const ACRenTransferPolicy As String = "POLTRAN"
	'SJ 21/04/2004 - end
	Public Const ACRenPrintLinkedDoc As String = "PRLINKDOC"
	'IM 140601
	' Work Manager Key constants
	Public Const ACWMKeyProcess As String = "RENEWAL"
	
	' Insurance File Type Constants
	Public Const ACPMInsFileTypeQuote As String = "QUOTE"
	Public Const ACPMInsFileTypeRenewal As String = "RENEWAL"
	Public Const ACPMInsFileTypeWhatIf As String = "RENEWALWIF"
	Public Const ACPMInsFileTypeLivePolicy As String = "POLICY"
	
	' Work Manager Key value constants
	Public Const ACWMKeyValueRenew As String = "RENEW"
	Public Const ACWMKeyValueLapse As String = "LAPSE"
	
	Public Const ACWMTaskConfirmPending As String = "RENCONPEN"
	Public Const ACWMTaskLapsePending As String = "RENLAPPEN"
	
	'Renewal Group Constants
	Public Const ACRenewalGroupMotor As Integer = 4006
	Public Const ACRenewalGroupHousehold As Integer = 4007
	
	'SSL 03/7/01 - Contants for user based messages
	Public Const ACRenEDIMessage1 As String = "No input files present to process."
	'JSB 02/07/03 - Change following description
	'Public Const ACRenEDIMessage2 As String = "No r-*.* prefix files present to process."
	Public Const ACRenEDIMessage2 As String = "No renewal EDI Invite messages available to process"
	Public Const ACRenEDIMessage3 As String = "File : RenMotorOut.txt does not contain any data to process."
	Public Const ACRenEDIMessage4 As String = "File : RenMotorOut.txt does not exist."
	Public Const ACRenEDIMessage5 As String = "File : RenHHOut.txt does not contain any data to process."
	Public Const ACRenEDIMessage6 As String = "File : RenHHOut.txt does not exist."
	Public Const ACRenEDIMessage7 As String = "Invalid Input directory path."
	Public Const ACRenEDIMessage8 As String = "Invalid Backup directory path."
	Public Const ACRenEDIMessage9 As String = "Policy reference not found."
	
	'Below ACRenEDIMsg* to be used in conjunction with ACRenEDIMessage*
	Public Const ACRenEDIMsg1 As Integer = 191
	Public Const ACRenEDIMsg2 As Integer = 192
	Public Const ACRenEDIMsg3 As Integer = 193
	Public Const ACRenEDIMsg4 As Integer = 194
	Public Const ACRenEDIMsg5 As Integer = 195
	Public Const ACRenEDIMsg6 As Integer = 196
	Public Const ACRenEDIMsg7 As Integer = 197
	Public Const ACRenEDIMsg8 As Integer = 198
	Public Const ACRenEDIMsg9 As Integer = 199
	
	'SSL - End
	
	'AK 300701 - Constants for Datamodel Codes
	Public Const ACDataModelMotor As String = "GIIMotor"
	Public Const ACDataModelHouse As String = "GIIHouse"
	Public Const ACDataModelTruck As String = "GIITruck"
	
	'AK 161001 - Constants for Business types
	Public Const ACBusinessTypeMotor As String = "GIIM"
	Public Const ACBusinessTypeHouse As String = "GIIH"
	Public Const ACBusinessTypeTruck As String = "GIIT"
	
	' IJM 030801 - Constants for renewal business type
	Public Const ACRenewalTypeBoth As Integer = 0
	Public Const ACRenewalTypeBrokerLed As Integer = 1
	Public Const ACRenewalTypeInsurerLed As Integer = 2
	Public Const ACRenewalTypeWhatIf As Integer = 4
	'SJ 21/04/2004 - start
	Public Const ACRenewalTypeInsurerMode As Integer = 5
	'SJ 21/04/2004 - end
	'AK 011101 - Reset Flags
	Public Const ACResetClaims As Integer = 1
	Public Const ACResetMTA As Integer = 2
	Public Const ACResetEDI As Integer = 3
	
	Public Const ACFlagSuspend As String = "S"
	Public Const ACFlagLapse As String = "L"
	Public Const ACFlagReset As String = "R"
End Module