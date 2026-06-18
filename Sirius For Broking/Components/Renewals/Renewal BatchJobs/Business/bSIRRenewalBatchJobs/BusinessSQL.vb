Option Strict Off
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: SSL 29/03/2001
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRRenewalBatchJobs.Renewals class.
    '
    ' ***************************************************************** '

    ' Pre Renewal Selection
    'SJ 14/4/2004 - Add three extra parameters
    Public Const ACPreRenSelSQL As String = "spu_SirRen_Pre_Renewal_Sel"
    Public Const ACPreRenSelStored As Boolean = True
    Public Const ACPreRenSelName As String = "PreRenSel"

    ' Renewal Selection
    'JSB 09/06/03 - added additional parameter, so that insurance_folder_cnt can be passed through
    'CJB 28/04/04 - added additional parameter, so that source_id can be passed through
    'CJB 05/05/04 - added additional parameter, so that insurer_mode can be passed through
    Public Const ACRenSelSQL As String = "spu_SirRen_Renewal_Sel"
    Public Const ACRenSelStored As Boolean = True
    Public Const ACRenSelName As String = "RenSel"

    ' Broker Lead Quotation
    'JSB 09/06/03 - added additional parameter, so that insurance_folder_cnt can be passed through
    'CJB 28/04/04 - added additional parameter, so that source_id can be passed through
    'CJB 05/05/04 - added additional parameter, so that insurer_mode can be passed through
    Public Const ACQuoteBrokerSQL As String = "spu_SirRen_Quote_Broker_Sel"
    Public Const ACQuoteBrokerStored As Boolean = True
    Public Const ACQuoteBrokerName As String = "QuoteBroker"

    ' Insurer Lead Quotation
    'JSB 09/06/03 - added additional parameter, so that insurance_folder_cnt can be passed through
    'CJB 28/04/04 - added additional parameter, so that source_id can be passed through
    'CJB 05/05/04 - added additional parameter, so that insurer_mode can be passed through
    'developer guide no.39
    'start
    Public Const ACQuoteInsurerSQL As String = "spu_SirRen_Quote_Insurer_Sel"
    Public Const ACQuoteInsurerStored As Boolean = True
    Public Const ACQuoteInsurerName As String = "QuoteInsurer"

    ' Create new policy
    Public Const ACSelectCreatePolicySQL As String = "spu_SirRen_Create_Policy_Sel"
    Public Const ACSelectCreatePolicyName As String = "SelectCreatePolicy"
    Public Const ACSelectCreatePolicyStored As Boolean = True

    ' Invitation
    'JSB 09/06/03 - added additional parameter, so that insurance_folder_cnt can be passed through
    'CJB 28/04/04 - added additional parameter, so that source_id can be passed through
    'CJB 05/05/04 - added additional parameter, so that insurer_mode can be passed through
    Public Const ACInvitationSQL As String = "spu_SirRen_Invitation_Sel"
    Public Const ACInvitationStored As Boolean = True
    Public Const ACInvitationName As String = "Invitation"

    ' Renewal_Control Selection
    Public Const ACSelectRenewalControlStored As Boolean = True
    Public Const ACSelectRenewalControlName As String = "SelectRenewalControl"
    Public Const ACSelectRenewalControlSQL As String = "spu_renewal_control_sel"

    'AK 180501 Renewal Completion - Lapse
    'JSB 09/06/03 - added additional parameter, so that insurance_folder_cnt can be passed through
    'CJB 28/04/04 - added additional parameter, so that source_id can be passed through
    'CJB 05/05/04 - added additional parameter, so that insurer_mode can be passed through
    Public Const ACSelectRenCompSQL As String = "spu_SirRen_Comp_Sel"
    Public Const ACSelectRenCompName As String = "SelectRenComp"
    Public Const ACSelectRenCompStored As Boolean = True



    ' CTAF 13062001 - Auto Renewal
    Public Const ACSelectAutoRenewSQL As String = "spu_SIRREN_Select_AutoRenew"
    Public Const ACSelectAutoRenewName As String = "SelectAutoRenew"
    Public Const ACSelectAutoRenewStored As Boolean = True

    ' CTAF 13062001 - Auto Renewal - Selecting Live Policies
    Public Const ACSelectLivePolicySQL As String = "spu_SirRen_Select_Live_Policy"
    Public Const ACSelectLivePolicyName As String = "SelectLivePolicy"
    Public Const ACSelectLivePolicyStored As Boolean = True

    'TF271101 - Auto Renew Invited (Broker Led)
    'These are more generic than the above (which are for single selection)
    Public Const ACSelectAutoRenInvitedSQL As String = ""
    Public Const ACSelectAutoRenInvitedName As String = "SelectAutoRenewInvited"
    Public Const ACSelectAutoRenInvitedStored As Boolean = True

    ' CTAF 13062001 - Reminder
    'JSB 09/06/03 - added additional parameter, so that insurance_folder_cnt can be passed through
    'CJB 28/04/04 - added additional parameter, so that source_id can be passed through
    'CJB 05/05/04 - added additional parameter, so that insurer_mode can be passed through
    Public Const ACSelectReminderSQL As String = "spu_SirRen_Reminder_Sel"
    Public Const ACSelectReminderName As String = "SelectReminder"
    Public Const ACSelectReminderStored As Boolean = True

    ' IJM 270701 - WhatIf Quote
    Public Const ACWhatIfQuoteSQL As String = "spu_SirRen_WhatIf_Quote_Sel"
    Public Const ACWhatIfQuoteStored As Boolean = True
    Public Const ACWhatIfQuoteName As String = "WhatIfQuote"

    ' SET 25022004 - Auto renewal debit (multiple record select)
    'SJ 26/04/2004 - Add extra parameter
    'CJB 28/04/04 - added additional parameter, so that source_id can be passed through
    'CJB 05/05/04 - added additional parameter, so that insurer_mode can be passed through
    Public Const ACSelectAutoRenDebitSQL As String = "spu_SIRRen_auto_ren_db_sel"
    Public Const ACSelectAutoRenDebitName As String = "SelectAutoRenewDebit"
    Public Const ACSelectAutoRenDebitStored As Boolean = True

    'CJB 22/07/04 - Auto renewal debit (single record select) PN13570
    Public Const ACSelectSingleAutoRenDebitSQL As String = "spu_SIRRen_auto_ren_db_single_sel"
    Public Const ACSelectSingleAutoRenDebitName As String = "SelectAutoRenewDebitSingle"
    Public Const ACSelectSingleAutoRenDebitStored As Boolean = True


    ' ************************************************
    ' Single record selects
    ' ************************************************

    'AK 07062001 - for handling single renewal-selections

    ' Renewal Selection
    Public Const ACSelectSingleSelectionSQL As String = "spu_SirRen_Renewal_Sel"
    Public Const ACSelectSingleSelectionName As String = "SelectSingleSelection"
    Public Const ACSelectSingleSelectionStored As Boolean = True


    ' Renewal Invitation
    Public Const ACSelectSingleInvitationSQL As String = "spu_SirRen_Invitation_Sel"
    Public Const ACSelectSingleInvitationName As String = "SelectSingleInvitation"
    Public Const ACSelectSingleInvitationStored As Boolean = True

    ' Insurer lead Quotation
    Public Const ACSelectSingleQuoteInsurerSQL As String = "spu_SirRen_Quote_Insurer_Sel"
    Public Const ACSelectSingleQuoteInsurerName As String = "SelectSingleQuoteInsurer"
    Public Const ACSelectSingleQuoteInsurerStored As Boolean = True

    'sj 09/10/2001 - start
    ' Preferred Quotes
    Public Const ACSelectSinglePerferredQuotesSQL As String = "spu_SirRen_Preferred_Quotes_Sel"
    Public Const ACSelectSinglePerferredQuotesName As String = "SelectSingleQuoteBroker"
    Public Const ACSelectSinglePerferredQuotesStored As Boolean = True
    'sj 09/10/2001 - end

    ' Broker lead Quotation
    Public Const ACSelectSingleQuoteBrokerSQL As String = "spu_SirRen_Quote_Broker_Sel"
    Public Const ACSelectSingleQuoteBrokerName As String = "SelectSingleQuoteBroker"
    Public Const ACSelectSingleQuoteBrokerStored As Boolean = True

    'AK 180501 Renewal Completion - Lapse
    'CJB 28/04/04 - added additional parameter, so that source_id can be passed through
    'CJB 05/05/04 - added additional parameter, so that insurer_mode can be passed through
    'CLG 15/06/04 - created new single selection SP to cater for just 2 paremeters
    Public Const ACSelectSingleRenCompSQL As String = "spu_SirRen_Comp_Single_Sel"
    Public Const ACSelectSingleRenCompName As String = "SelectRenSingleComp"
    Public Const ACSelectSingleRenCompStored As Boolean = True


    ' CTAF 13062001 - Reminder
    Public Const ACSelectSingleReminderSQL As String = "spu_SirRen_Reminder_Sel"
    Public Const ACSelectSingleReminderName As String = "SelectReminder"
    Public Const ACSelectSingleReminderStored As Boolean = True

    'TF101001 - To get InsuranceFileCnt from GisPolicyLink
    Public Const ACGetInsFileCntStored As Boolean = True
    Public Const ACGetInsFileCntName As String = "ACGetInsFileCnt"
    Public Const ACGetInsFileCntSQL As String = "spu_gis_policy_link_sel"


    'AK 131201 Renewal housekeep - all the due policies
    'CJB 28/04/04 - added additional parameter, so that source_id can be passed through
    'CJB 04/05/04 - added additional parameter, so that insurer_mode can be passed through
    Public Const ACRenHouseKeepAllSQL As String = "spu_SIRRen_HouseKeepAll"
    Public Const ACRenHouseKeepAllName As String = "HouseKeepAll"
    Public Const ACRenHouseKeepAllStored As Boolean = True

    'AK 131201 Renewal housekeep - only for the selected Insurance folder
    Public Const ACRenHouseKeepSQL As String = "spu_SIRRen_HouseKeep"
    Public Const ACRenHouseKeepName As String = "HouseKeep"
    Public Const ACRenHouseKeepStored As Boolean = True

    'IDP Jan 2003 - New SP to switch the scheme on a Fortis Renewal
    Public Const ACRenFortisRenewalSQL As String = "sp_SirRen_FortScheme_Upd"
    'end
    Public Const ACRenFortisRenewalName As String = "FortisRenewal"
    Public Const ACRenFortisRenewalStored As Boolean = True
End Module