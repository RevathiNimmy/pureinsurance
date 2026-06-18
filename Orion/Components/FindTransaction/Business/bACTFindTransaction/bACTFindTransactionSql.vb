Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module FindTransSql
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Class Name: FindTransSQL
    '
    ' Date: 28 August 1997
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Embedded SQL)
    '
    ' Edit History:
    '   PWF 26/09/2002 - Added recoded sql for SelectTransQuery
    '   PWF 30/09/2002 - Enhanced sql for SelectTransQueryFiltered
    '   AMB 11/02/2003 - insured_name, insured_account, insurance_file_cnt,
    '                  - document_id and flag added for IAG PS220 Manage Debtors
    '   CJB 25/03/2004 - Return TransDetail.comment field also (will be viewed via
    '                    tooltips and added/edited via right mouse button menus.
    '   CJB 30/03/2004 - Return new TransDetail.not_reported column field also -
    '                    can be toggled on/off by right mouse button menu.
    ' ***************************************************************** '

    ' Get TransID from parameters SQL
    Public Const ACGetTransIDStored As Boolean = True
    Public Const ACGetTransIDName As String = "GetTransID"
    Public Const ACGetTransIDSQL As String = "spu_ACT_Do_GetTransId"

    ' Select all ledgers for company
    Public Const ACGetLedgersQueryStored As Boolean = True
    Public Const ACGetLedgersQueryName As String = "GetLedgersQuery"
    Public Const ACGetLedgersQuerySQL As String = "spu_ACT_Do_SelectLedgers"

    '
    Public Const ACGetFinancePlanDetailsStored As Boolean = True
    Public Const ACGetFinancePlanDetailsName As String = "GetFinancePlanDetails"
    Public Const ACGetFinancePlanDetailsSQL As String = "spu_ACT_get_fp_dets_from_doc_ref"

    ' AMB 18/02/2003: PS220 - added for Manage Debtors development
    Public Const ACGetAccountAmountsStored As Boolean = True
    Public Const ACGetAccountAmountsName As String = "GetAccountAmounts"
    Public Const ACGetAccountAmountsSQL As String = "spu_ACT_Select_Credit_Amounts"

    ' DD 03/06/2003: Added for performance
    Public Const ACGetAccountDetailsStored As Boolean = True
    Public Const ACGetAccountDetailsName As String = "GetAccountDetails"
    Public Const ACGetAccountDetailsSQL As String = "spu_ACT_Select_Account_Details"

    ' CJB 26/03/2004: Added to allow comments to be added/updated
    Public Const ACUpdateCommentStored As Boolean = True
    Public Const ACUpdateCommentName As String = "UpdateComment"
    Public Const ACUpdateCommentSQL As String = "spu_ACT_Update_TransDetail_Comment"

    ' CJB 30/03/2004: Added to allow not_reported flag to be toggled on/off
    Public Const ACUpdateNotReportedStored As Boolean = True
    Public Const ACUpdateNotReportedName As String = "UpdateNotReported"
    Public Const ACUpdateNotReportedSQL As String = "spu_ACT_Update_TransDetail_NotReported"

    'DC220305 : Get Write off Reasons
    Public Const ACGetWriteOffReasonsSQL As String = "spu_ACT_Get_Write_Off_Reasons"
    Public Const ACGetWriteOffReasonsName As String = "Get Write Off Reasons"
    Public Const ACGetWriteOffReasonsStored As Boolean = True

    Public Const ACGetAccountTypeSQL As String = "spu_ACT_Get_Account_Types"
    Public Const ACGetAccountTypeName As String = "SelectAccountTypes"
    Public Const ACGetAccountTypeStored As Boolean = True

    Public Const ACTransFromQueryStored As Boolean = False
    Public Const ACTransFromQueryName As String = "SelectTransQuery"

    ' PWF 26/09/2002 - Notes on following statement.
    ' - cover_start_date switches based on the from_sirius field.
    ' - fully_matched IS more efficient as a sub-query, don't ask why
    ' - currency_match_amount is LEFT JOINED so default to 0 when NULL
    ' - Fields are ordered for the resultset so no mapping is necessary
    ' AMB 11/02/2003: PS220 - fields added
    ' PSL 01/07/2003 Iss4536
    ' CJB 25/03/2004 Return TransDetail.comment field also (will be viewed via
    ' tooltips and added/edited via right mouse button menus.
    'CJB 30/03/2004 Return TransDetail.not_reported field also (can be toggled via
    'right mouse button menus.
    'RKS 121004 PN15623 Restricted query to 500 records (timed-out error fix)
    ' RDC 04112005 Added tb.code, tg.code, tgtb.allocation_seq, allocation rule, tdt.code
    ' Sankar - (Tech Spec - PGR025 - SAM Bill Enquiry.doc) - (5.1.1.2) - Added dt.description
    ' Prakash Varghese - (Tech Spec - WCR044 - Bill History.doc) - (3.3.1.1)- Added PFI.Transaction_Id related columns
    Public Const ACTransFromQuerySelectUnderwriting As String = "SELECT "
    Public Const ACTransFromQuerySelect500Underwriting As String = "SELECT TOP 500 "
    Public Const ACTransFromQuerySelectListUnderwriting As String = "d.document_ref, " &
                                                                    "cover_start_date = CASE WHEN (dt.from_sirius = 0 OR t.spare LIKE 'Revers%') and not(t.spare = 'DIRECTDEBIT') THEN d.document_date ELSE ISNULL(i.cover_start_date,d.document_date) END, " &
                                                                    "p.period_name, " &
                                                                    "t.currency_amount, " &
                                                                    "fully_matched = (SELECT td2.fully_matched FROM TransDetail td2 WHERE td2.document_id = t.document_id AND td2.document_sequence = 1), " &
                                                                    "0, d.documenttype_id, dt.doctypegroup_id, " &
                                                                    "t.insurance_ref, pmu.username, " &
                                                                    "t.purchase_order_no, t.purchase_invoice_no, " &
                                                                    "t.department, t.spare, a.short_code, a.account_id, " &
                                                                    "t.currency_id, t.transdetail_id, t.amount, t.document_sequence, " &
                                                                    "d.document_date, t.company_id, 0, 0, " &
                                                                    "d.reason, insured_name = acc2.account_name, " &
                                                                    "insured_account = acc2.short_code, flag = ISNULL(audst.[code], ''), " &
                                                                    "d.insurance_file_cnt, d.document_id, auds.auditset_id, auds.user_id, t.currency_base_xrate, " &
                                                                    "payee_name = ISNULL(cli.payment_name, (CASE WHEN d.document_ref Like 'CLP%' THEN (SELECT TOP 1 cp.PayeeName FROM Claim_Payment cp WHERE cp.claim_id = cl1.Claim_id and cp.claim_payment_id=cp.base_claim_payment_id and cp.PayeeName is not null ) " &
                                                                    "WHEN d.document_ref Like 'CLR%' THEN (SELECT TOP 1 cr.PayeeName FROM Claim_Receipt cr WHERE cr.claim_id = cl1.Claim_id and cr.claim_receipt_id=cr.base_claim_receipt_id and cr.PayeeName IS NOT NULL) " &
                                                                    "ELSE '' END)), " &
                                                                    "i.alternate_reference, i.policy_type_id, t.comment, isnull(t.not_reported, 0), uwy.description, " &
                                                                    "description  = ISNULL((Select  Distinct MT.description from CashListItem_Instalments CI " &
                                                                    "RIGHT JOIN CashListItem CLI ON CLI.cashlistitem_id =CI.cashlistitem_id " &
                                                                    "INNER JOIN MediaType MT ON MT.mediatype_id=CLI.mediatype_id " &
                                                                    "Where t.transdetail_id=CLI.transdetail_id), " &
                                                                    "(CASE WHEN d.document_ref Like 'CLP%' THEN (SELECT top 1 MT.description from Claim_Payment cp INNER JOIN MediaType MT On  MT.mediatype_id=cp.PayeeMediaType where cp.claim_id = cl1.Claim_id AND cp.claim_payment_id=cp.base_claim_payment_id and cp.PayeeMediaType is not null ) " &
                                                                    "WHEN d.document_ref Like 'CLR%' THEN (SELECT top 1 MT.description from Claim_Receipt cr INNER JOIN MediaType MT On MT.mediatype_id=cr.PayeeMediaType where cr.claim_id = cl1.Claim_id AND cr.claim_receipt_id=cr.base_claim_receipt_id and cr.PayeeMediaType is not null) ELSE NULL END )), " &
                                                                    "c.description, c2.description, c2.currency_id, " &
                                                                    "t.account_currency_id, t.account_amount, " &
                                                                    "t.outstanding_amount, t.outstanding_account_amount, t.amount_updated, " &
                                                                    "t.outstanding_currency_amount, p.period_end_date, " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                    "tb.code, tg.code, tgtb.allocation_sequence, tgtb.allocation_rule, tdt.code, " &
                                                                    "t.balance_type,Party_type.Code,ISNULL(party_agent.is_float_balance_account,0) is_float_balance_account,ISNULL(party_agent.is_overdraft_account,0)is_overdraft_account,t.Reference, ag.shortname, " &
                                                                    "Bank_Guarantee.BG_ref, dt.description,dtg.description,c2.iso_code,c3.iso_code,c.iso_code, " &
                                                                    "CASE WHEN PFI.PFTransaction_Id IS NULL THEN 0 ELSE 1 End 'Instalment_Collection', " &
                                                                    "cli.cashlist_id, " &
                                                                    "cl.is_split_receipt, " &
                                                                    "cli.is_lead, " &
                                                                    "t.due_date,[case].case_number 'CaseNumber', " &
                                                                    "(Select PP.shortname from Party PP WITH(NOLOCK) LEFT JOIN Party_Agent PPA " &
                                                                    " ON PP.party_cnt = PPA.party_cnt where PPA.party_cnt = (select Top 1 lead_agent_cnt from " &
                                                                    " insurance_file WITH(NOLOCK) where insurance_ref = t.insurance_ref Order By policy_version Desc ))  MTAAgentName " &
                                                                    ",CASE WHEN EXISTS(SELECT TransMatch.CashListItem_ID  FROM TransMatch JOIN transdetail ON transmatch.transdetail_id = transdetail.transdetail_id WHERE t.transdetail_id  = transdetail.transdetail_id  AND IsNull(transmatch.CashListItem_ID,0) <> 0) THEN 1 ELSE 0 End 'IsGoneForApproval' , CASE WHEN EXISTS(SELECT TransDetailEx.transdetailex_id  FROM TransDetailEx join transdetail on transdetailex.transdetail_id = transdetail.transdetail_id  WHERE d.document_id = transdetailex.document_id and a.account_id = transdetailex.account_id) THEN 1 ELSE 0 END 'IsDebitOrderTransDetail',  " &
                                                                    "0 'Include_Tax_In_Instalments', " &
                                                                    "cli.cashlistitem_id, " &
                                                                    "cl.bankaccount_id, " &
                                                                    "cl.cashlist_ref, " &
                                                                    "IsNull(a.account_key,0), " &
                                                                    " c.code As CurrencyCode " &
                                                                    ",pfprem_finance_cnt =(SELECT pfprem_finance_cnt FROM document d1 JOIN transdetail td ON td.document_id = d1.document_id JOIN pfpremiumfinance pf1  ON pf1.plantransaction_id = td.transdetail_id  WHERE d1.document_ref = d.document_ref AND d1.company_id = d.company_id) " &
                                                                    ",pfprem_finance_version =(SELECT pfprem_finance_version FROM document d1 JOIN transdetail td ON td.document_id = d1.document_id JOIN pfpremiumfinance pf2 ON pf2.plantransaction_id = td.transdetail_id  WHERE d1.document_ref = d.document_ref AND d1.company_id = d.company_id) " &
                                                                    ",statusind =(SELECT statusind FROM document d1 JOIN transdetail td ON td.document_id = d1.document_id JOIN pfpremiumfinance pf2 ON pf2.plantransaction_id = td.transdetail_id  WHERE d1.document_ref = d.document_ref AND d1.company_id = d.company_id) " &
                                                                    ",IsNull(party_agent.is_gross_agent,0)" &
                                                                    ",isnull(Case When d.document_ref Like 'IND%' OR d.document_ref Like 'INC%' then (select TOP 1 ppf.clientid from PFPremiumFinance ppf INNER JOIN insurance_file i  ON i.insurance_file_cnt=ppf.Insurance_File_Cnt  INNER JOIN TransDetail TD  ON i.insurance_ref = TD.insurance_ref where TD.document_id = d.document_id)  Else acc2.account_key End,0) " &
                                                                    ",isnull((SELECT top 1 aa.account_key FROM account aa INNER JOIN TransDetail td ON aa.account_id=td.account_id INNER JOIN document dd ON dd.document_id=td.document_id WHERE dd.document_ref=d.document_ref AND ISNULL(aa.account_key,0)<>0),0) " &
                                                                    ",isnull(i.insurance_folder_cnt,0)"


    ' PWF 26/09/2002 - Notes on following statement.
    ' - Insurance_file MUST be LEFT JOINed and doesn't always exist
    ' RDC 04112005 added left joins from tax_band to transdetail_type
    'Prakash Varghese - (Tech Spec - WCR044 - Bill History.doc) - (3.3.1.1)- Added Left join on PFInstalments
    Public Const ACTransFromQueryFrom As String = "FROM Account a WITH(NOLOCK) " &
                                                  "INNER JOIN TransDetail t WITH(NOLOCK) ON t.account_id = a.account_id " &
                                                  "LEFT JOIN transdetail_type tt WITH(NOLOCK) ON tt.transdetail_type_id = t.transdetail_type_id " &
                                                  "LEFT  JOIN CashListItem cli WITH(NOLOCK) ON cli.transdetail_id = t.transdetail_id " &
                                                  "LEFT JOIN CashList cl WITH(NOLOCK) ON cl.cashlist_id = cli.cashlist_id " &
                                                  "LEFT  JOIN mediatype mt WITH(NOLOCK) ON mt.mediatype_id = cli.mediatype_id " &
                                                  "INNER JOIN Document d WITH(NOLOCK) ON d.document_id = t.document_id " &
                                                  "INNER JOIN Period p WITH(NOLOCK) ON p.period_id = t.period_id " &
                                                  "INNER JOIN DocumentType dt WITH(NOLOCK) ON dt.documenttype_id = d.documenttype_id " &
                                                  "INNER JOIN PMUser pmu WITH(NOLOCK) ON pmu.user_id = t.operator_id " &
                                                  "LEFT  JOIN Insurance_file i WITH(NOLOCK) ON i.insurance_file_cnt = d.insurance_file_cnt " &
                                                  "LEFT  JOIN Underwriting_Year uwy WITH(NOLOCK) ON uwy.underwriting_year_id = t.underwriting_year_id " &
                                                  "INNER JOIN Currency c WITH(NOLOCK) ON c.currency_id = t.currency_id " &
                                                  "INNER JOIN Currency c2 WITH(NOLOCK) ON c2.currency_id = t.amount_currency_id " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "LEFT  JOIN tax_band tb WITH(NOLOCK) ON tb.tax_band_id = t.tax_band_id " &
                                                  "LEFT  JOIN tax_group tg WITH(NOLOCK) ON tg.tax_group_id = t.tax_group_id " &
                                                  "LEFT  JOIN tax_group_tax_band tgtb WITH(NOLOCK) ON tgtb.tax_band_id = t.tax_band_id " &
                                                  "                                  AND tgtb.tax_group_id = t.tax_group_id " &
                                                  "LEFT  JOIN transdetail_type tdt WITH(NOLOCK) ON tdt.transdetail_type_id = t.transdetail_type_id " &
                                                  "LEFT JOIN party WITH(NOLOCK) ON Party.Party_cnt=a.Account_key " &
                                                  "LEFT JOIN party_type WITH(NOLOCK) ON party.party_type_id=party_type.party_type_id " &
                                                  "LEFT JOIN Party_agent WITH(NOLOCK) on Party.Party_cnt=Party_agent.Party_cnt " &
                                                  "LEFT JOIN Party ag WITH(NOLOCK) on ag.Party_cnt=i.lead_agent_cnt " &
                                                  "LEFT JOIN Insurance_File_BG_Link on Insurance_File_BG_Link.Insurance_File_Cnt=i.Insurance_File_Cnt " &
                                                  "LEFT JOIN Bank_Guarantee ON Bank_Guarantee.BG_id=Insurance_File_BG_Link.BG_id " &
                                                  "LEFT JOIN PFInstalments PFI WITH(NOLOCK) ON t.TransDetail_id=PFI.PFTransaction_id " '(Prakash Varghese) - (Tech Spec - WCR044 - Bill History.doc) - (3.3.1.1)


    Public Const ACTransFromAdditionalSAMQuery As String = "LEFT JOIN doctypegroup dtg WITH(NOLOCK) ON dtg.doctypegroup_id = dt.doctypegroup_id " &
                                                           "INNER JOIN Currency c3 WITH(NOLOCK) ON c3.currency_id = t.account_currency_id "


    'PSL 25/02/2003 Issue 2443
    'Prakash Varghese - (Tech Spec - WCR044 - Bill History.doc) - (3.3.1.1)- Added Left join on PFInstalments
    Public Const ACTransFromQueryFromNewPF As String = "FROM Account a " &
                                                       "INNER JOIN TransDetail t ON t.account_id = a.account_id " &
                                                       "LEFT JOIN transdetail_type tt ON tt.transdetail_type_id = t.transdetail_type_id " &
                                                       "LEFT  JOIN CashListItem cli WITH(NOLOCK) ON cli.transdetail_id = t.transdetail_id " &
                                                          "LEFT JOIN CashList cl WITH(NOLOCK) ON cl.cashlist_id = cli.cashlist_id " &
                                                       "LEFT  JOIN mediatype mt WITH(NOLOCK) ON mt.mediatype_id = cli.mediatype_id " &
                                                       "INNER JOIN Document d ON d.document_id = t.document_id " &
                                                       "INNER JOIN Period p ON p.period_id = t.period_id " &
                                                       "INNER JOIN DocumentType dt ON dt.documenttype_id = d.documenttype_id " &
                                                       "INNER JOIN PMUser pmu ON pmu.user_id = t.operator_id " &
                                                       "LEFT JOIN Insurance_file i ON i.insurance_file_cnt = d.insurance_file_cnt " &
                                                       "LEFT  JOIN Underwriting_Year uwy WITH(NOLOCK) ON uwy.underwriting_year_id = t.underwriting_year_id " &
                                                       "INNER JOIN Currency c ON c.currency_id = t.currency_id " &
                                                       "INNER JOIN Currency c2 ON c2.currency_id = t.amount_currency_id " &
                                                       "LEFT  JOIN tax_band tb WITH(NOLOCK) ON tb.tax_band_id = t.tax_band_id " &
                                                       "LEFT  JOIN tax_group tg WITH(NOLOCK) ON tg.tax_group_id = t.tax_group_id " &
                                                       "LEFT  JOIN tax_group_tax_band tgtb WITH(NOLOCK) ON tgtb.tax_band_id = t.tax_band_id " &
                                                       " AND tgtb.tax_group_id = t.tax_group_id " &
                                                       "LEFT  JOIN transdetail_type tdt WITH(NOLOCK) ON tdt.transdetail_type_id = t.transdetail_type_id " &
                                                       "LEFT JOIN party WITH(NOLOCK) ON Party.Party_cnt=a.Account_key " &
                                                       "LEFT JOIN party_type WITH(NOLOCK) ON party.party_type_id=party_type.party_type_id " &
                                                       "LEFT JOIN Party_agent WITH(NOLOCK) on Party.Party_cnt=Party_agent.Party_cnt " &
                                                       "LEFT JOIN Party ag on ag.Party_cnt=i.lead_agent_cnt " &
                                                       "LEFT JOIN Insurance_File_BG_Link on Insurance_File_BG_Link.Insurance_File_Cnt=i.Insurance_File_Cnt " &
                                                       "LEFT JOIN Bank_Guarantee ON Bank_Guarantee.BG_id=Insurance_File_BG_Link.BG_id " &
                                                       "LEFT JOIN PFInstalments PFI WITH(NOLOCK) ON t.TransDetail_id=PFI.PFTransaction_id " & Strings.ChrW(13) & Strings.ChrW(10)
    ' AMB 11/02/2003: PS220
    ' Added JOINs for additional listview columns required for IAG Spec 220 Manage Debtors
    'eck PN6169 Added in extra check to ensure that account company matches insurance file company
    Public Const ACTransFromQueryAdditional As String = "LEFT JOIN account AS acc2 WITH(NOLOCK) ON acc2.account_key = i.insured_cnt " &
                                                        "LEFT JOIN AuditSet AS auds WITH(NOLOCK) ON auds.document_id = d.document_id AND ISNULL(auds.rejected,0)=0 " &
                                                        "LEFT JOIN AuditSet_Type AS audst WITH(NOLOCK) ON audst.auditset_type_id = auds.auditset_type_id " &
                                                        " LEFT JOIN Stats_Folder AS sf WITH(NOLOCK) ON sf.document_ref = d.document_ref " &
                                                        "LEFT JOIN Claim AS cl1 WITH(NOLOCK) ON cl1.claim_id = sf.loss_id " &
                                                        "LEFT JOIN [case] WITH(NOLOCK) ON [case].case_id = cl1.base_case_id " & Strings.ChrW(13) & Strings.ChrW(10)



    ' PWF 26/09/2002 - Notes on following statement.
    ' - Simple ordering clause
    Public Const ACTransFromQueryOrderBy As String = " ORDER BY t.company_id, d.document_date DESC" & Strings.ChrW(13) & Strings.ChrW(10)
    Public Const ACTransFromQueryOrderBySpare As String = " ORDER BY t.company_id, spare_type, d.document_date DESC" & Strings.ChrW(13) & Strings.ChrW(10)

    ' PWF 30/09/2002 - Notes on following statement.
    ' - Two additional fields required for filtering where enhanced security enabled
    ' - First field can be retrieved direct from enhanced query to filter records
    ' - Second field may have better permissions for another group so needs to be
    '   retrieved seperately.
    Public Const ACTransFilterFields As String = ", " &
                                                 "uga.has_unrestricted_enquiry, " &
                                                 "has_unrestricted_update = (SELECT MAX(uga.has_unrestricted_update) " &
                                                 "FROM PMUser_Group_Authorities uga " &
                                                 "INNER JOIN #User_Groups ug on ug.pmuser_group_id = uga.pmuser_group_id " &
                                                 "WHERE uga.node_id = st.parent_node_id) " & Strings.ChrW(13) & Strings.ChrW(10)

    ' PWF 30/09/2002 - Notes on following statement.
    ' - Due to nested user groups sp needs to be used.
    '   Load it into a temporary table for use later.
    Public Const ACTransFilterGroups As String = "CREATE TABLE #User_Groups (" &
                                                 "pmuser_group_id int NOT NULL," &
                                                 "code Char(10), " &
                                                 "caption VarChar(255), " &
                                                 "is_supervisor tinyint NOT NULL) " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "INSERT INTO #User_Groups " &
                                                 "EXECUTE spu_pmuser_users_groups_sel " &
                                                 "@user_id = {user_id}, " &
                                                 "@effective_date = {effective_date}, " &
                                                 "@language_id = {language_id} " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "CREATE INDEX Temp_User_Groups_ID ON #User_Groups (pmuser_group_id) " & Strings.ChrW(13) & Strings.ChrW(10)

    ' PWF 30/09/2002 - Notes on following statement.
    ' - Enhance the join conditions to provide access to the the is viewable
    '   authorisation flag
    Public Const ACTransFilterJoin As String = "INNER JOIN StructureTree st ON st.account_id = a.account_id " &
                                               "INNER JOIN PMUser_Group_Authorities uga ON uga.node_id = st.parent_node_id " &
                                               "INNER JOIN #User_Groups ug on ug.pmuser_group_id = uga.pmuser_group_id " & Strings.ChrW(13) & Strings.ChrW(10)

    ' PWF 30/09/2002 - Notes on following statement.
    ' - Drop the temporary user group table from earlier
    Public Const ACTransFilterCleanup As String = "DROP TABLE #User_Groups " & Strings.ChrW(13) & Strings.ChrW(10)

    '26/09/2002 - PWC - 186 - Debt Roll-up
    Public Const ACTransFromQuerySelectSpareType As String = ", " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "CASE t.spare " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "    WHEN 'TAX' THEN 1 " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "    WHEN 'COMM' THEN 2 " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "    WHEN 'GROSS' THEN 3 " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "    ELSE  4 " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "End 'spare_type' "

    Public Const ACRollupTransFilterGroups As String = ACTransFilterGroups &
                                                       "DECLARE @has_unrestricted_enquiry INT " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "DECLARE @has_unrestricted_update INT " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "SELECT " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "    @has_unrestricted_enquiry = MAX(uga.has_unrestricted_enquiry)," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "    @has_unrestricted_update = MAX(uga.has_unrestricted_update)" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "FROM " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "    account a " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "    StructureTree st ON st.account_id = a.account_id " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "    PMUser_Group_Authorities uga ON (uga.node_id = st.parent_node_id) " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "INNER JOIN " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "    #User_Groups ug on ug.pmuser_group_id = uga.pmuser_group_id " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "Where " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                       "    a.account_id = {rollup_account_id} " & Strings.ChrW(13) & Strings.ChrW(10)

    'See notes on ACTransFromQuerySelect for an explanation of this SQL
    ' CJB 25/03/2004 Was going to return '' for TransDetail.comment as rolled-up trans
    ' won't have comments but since this is from 1.9 and isn't used in 1.8 yet and it
    ' already returns less columns than the broking and u/w queries then I won't!!
    ' CJB 30/03/2004 Same as above for not_reported column now returned by the other queries...
    'RKS 121004 PN15623 Restricted query to 500 records (timed-out error fix)
    ' Sankar - Tech Spec - PGR025 - SAM Bill Enquiry.doc - (5.1.1.1) - Added dt.description
    ' Prakash Varghese - (Tech Spec - WCR044 - Bill History.doc) - (3.3.1.1)- Added PFI.Transaction_Id related columns
    Public Const ACRollupTransFromQuerySelect As String = "SELECT "
    Public Const ACRollupTransFromQuerySelect500 As String = "SELECT TOP 500 "
    '--RC
    Public Const ACRollupTransFromQuerySelectList As String = "d.document_ref, " &
                                                              "cover_start_date = CASE WHEN (MIN(dt.from_sirius) = 0 OR MIN(t.spare) LIKE 'Revers%') THEN " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "Min(d.document_date) Else Min (i.cover_start_date) END, " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "MIN(p.period_name), SUM (t.currency_amount) 'currency_amount', " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "fully_matched = (SELECT td2.fully_matched FROM TransDetail td2 WHERE td2.document_id = Min(t.document_id) " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "AND td2.document_sequence = 1 ), " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "0, " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "MIN(d.documenttype_id), MIN(dt.doctypegroup_id), Min (t.insurance_ref) 'insurance_ref', " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "MIN(pmu.username), t.purchase_order_no 'purchase_order_no', t.purchase_invoice_no 'purchase_invoice_no', '' 'department', " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "case when MAX(t.spare)='TAX' and MAX(a.ledger_id) in (2,5) then 'GROSS' else MAX(t.spare) END 'spare', a.short_code, a.account_id, MIN(t.currency_id) 'currency_id', " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "MIN(t.transdetail_id) 'transdetail_id', Sum (t.amount) 'amount', 0 'document_sequence', MIN(d.document_date), " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "Min (t.company_id) 'company_id', 0, 0 'match_date', MIN(d.reason), " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "insured_name = Case When d.document_ref Like 'IND%' OR d.document_ref Like 'INC%' then (select TOP 1 ppf.ClientName from PFPremiumFinance ppf INNER JOIN insurance_file i  ON i.insurance_file_cnt=ppf.Insurance_File_Cnt  INNER JOIN TransDetail TD  ON i.insurance_ref = TD.insurance_ref where TD.document_id = d.document_id)  Else MIN(acc2.account_name) End," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "insured_account = Case When d.document_ref Like 'IND%' OR d.document_ref Like 'INC%' then (select TOP 1 ppf.ClientCode from PFPremiumFinance ppf INNER JOIN insurance_file i  ON i.insurance_file_cnt=ppf.Insurance_File_Cnt  INNER JOIN TransDetail TD  ON i.insurance_ref = TD.insurance_ref  where TD.document_id = d.document_id)  Else MIN(acc2.short_code) End," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "flag = ISNULL(MIN(audst.[code]), ''), " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "MIN(d.insurance_file_cnt), MIN(d.document_id), MIN(auds.auditset_id), MIN(auds.user_id), " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "Min (t.currency_base_xrate) 'currency_base_xrate', " &
                                                              "(CASE WHEN d.document_ref Like 'CLP%' THEN (SELECT TOP 1 cp.PayeeName FROM Claim_Payment cp WHERE cp.claim_id = cl1.Claim_id and cp.claim_payment_id=cp.base_claim_payment_id and cp.PayeeName is not null ) " &
                                                              "WHEN d.document_ref Like 'CLR%' THEN (SELECT TOP 1 cr.PayeeName FROM Claim_Receipt cr WHERE cr.claim_id = cl1.Claim_id and cr.claim_receipt_id=cr.base_claim_receipt_id and cr.PayeeName IS NOT NULL) " &
                                                              "ELSE '' END) 'payee_name', " &
                                                              "MIN(i.alternate_reference), MIN(i.policy_type_id), MIN(t.comment), MIN(isnull(t.not_reported, 0)), MIN(uwy.description), " &
                                                              "CASE WHEN ISNULL(ISNULL(MIN(mt.description),(case when MIN(t.transdetail_id) in(select  innerpfi.PFTransaction_id from PFInstalments innerpfi inner join CashListItem_Instalments innercli " &
                                                              "on innercli.pfinstalments_id = innerpfi.pfinstalments_id inner join CashListItem innerclsli on innerclsli.cashlistitem_id = innercli.cashlistitem_id inner join MediaType innermd  " &
                                                              "on innermd.mediatype_id = innerclsli.mediatype_id where innerpfi.PFTransaction_id = MIN(t.transdetail_id)) then (  " &
                                                              "select MIN(innermd.description) from PFInstalments innerpfi inner join CashListItem_Instalments innercli on innercli.pfinstalments_id = innerpfi.pfinstalments_id  inner join CashListItem innerclsli on innerclsli.cashlistitem_id = innercli.cashlistitem_id inner join MediaType innermd  on innermd.mediatype_id = innerclsli.mediatype_id where innerpfi.PFTransaction_id = MIN(t.transdetail_id)) " &
                                                              "ELSE null end) ),'') = '' THEN (CASE WHEN d.document_ref Like 'CLP%' THEN " &
                                                              "(SELECT top 1 MT.description from Claim_Payment cp INNER JOIN MediaType MT On MT.mediatype_id=cp.PayeeMediaType where cp.claim_id = cl1.Claim_id AND cp.claim_payment_id=cp.base_claim_payment_id and cp.PayeeMediaType is not null ) " &
                                                              "WHEN d.document_ref Like 'CLR%' THEN (SELECT top 1 MT.description from Claim_Receipt cr INNER JOIN MediaType MT On MT.mediatype_id=cr.PayeeMediaType where cr.claim_id = cl1.Claim_id AND cr.claim_receipt_id=cr.base_claim_receipt_id and cr.PayeeMediaType is not null) ELSE NULL END ) ELSE NULL END MediaTypeDescription," &
                                                              "MIN(c.description), " &
                                                              "MIN(c2.description), MIN(c2.currency_id), " &
                                                              "MIN(t.account_currency_id), SUM(t.account_amount), " &
                                                              "SUM(t.outstanding_amount), SUM(t.outstanding_account_amount), MAX(t.amount_updated), " &
                                                              "SUM(t.outstanding_currency_amount), MIN(p.period_end_date), " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "'', '', 0, 0, ''," &
                                                              "t.balance_type,Party_type.Code,ISNULL(party_agent.is_float_balance_account,0) is_float_balance_account,ISNULL(party_agent.is_overdraft_account,0)is_overdraft_account,t.Reference, ag.shortname, Bank_Guarantee.BG_ref, dt.description,dtg.description,c2.iso_code,c3.iso_code,c.iso_code, " &
                                                              "CASE WHEN PFI.PFTransaction_Id IS NULL THEN 0 ELSE 1 End 'Instalment_Collection'," &
                                                              "min(cli.cashlist_id), " &
                                                              "min(cl.is_split_receipt), " &
                                                              "min(cli.is_lead), " &
                                                              "t.due_date,MIN([case].Case_Number) 'CaseNumber', " &
                                                               "(Select PP.shortname from Party PP WITH(NOLOCK) LEFT JOIN Party_Agent PPA " &
                                                                    " ON PP.party_cnt = PPA.party_cnt where PPA.party_cnt = (select Top 1 lead_agent_cnt from " &
                                                                    " insurance_file WITH(NOLOCK) where insurance_ref = t.insurance_ref Order By policy_version Desc ))  MTAAgentName, " &
                                                                    " CASE WHEN EXISTS(SELECT TransMatch.CashListItem_ID  FROM TransMatch JOIN transdetail ON transmatch.transdetail_id = transdetail.transdetail_id WHERE d.document_id = transdetail.document_id AND IsNull(transmatch.CashListItem_ID,0) <> 0) THEN 1 ELSE 0 End 'IsGoneForApproval' , CASE WHEN EXISTS(SELECT TransDetailEx.transdetailex_id  FROM TransDetailEx join transdetail on transdetailex.transdetail_id = transdetail.transdetail_id  WHERE d.document_id = transdetailex.document_id and a.account_id = transdetailex.account_id) THEN 1 ELSE 0 END 'IsDebitOrderTransDetail' ," &
                                                                    " isnull((SELECT TOP 1 TC.include_tax_in_instalments FROM  TransDetail TD INNER JOIN Document D  ON  D.Document_id = TD.Document_id INNER JOIN Tax_Calculation TC  ON  TC.Insurance_File_Cnt = D.Insurance_File_Cnt Where TD.transdetail_id = min(t.transdetail_id )" &
                                                                    "AND TD.SPARE LIKE '%TAX%' AND TC.tax_band_id = td.tax_band_id ),0) 'Include_Tax_In_Instalments', " &
                                                               "min(cli.cashlistitem_id) , " &
                                                               "min(cl.bankaccount_id), " &
                                                               "min(cl.cashlist_ref), " &
                                                               "IsNull(a.account_key,0), " &
                                                               "Min(c.code) As CurrencyCode " &
                                                               ",pfprem_finance_cnt =(SELECT pfprem_finance_cnt FROM document d1 JOIN transdetail td ON td.document_id = d1.document_id JOIN pfpremiumfinance pf1  ON pf1.plantransaction_id = td.transdetail_id  WHERE d1.document_ref = MIN(d.document_ref) AND d1.company_id = MIN(d.company_id)) " &
                                                               ",pfprem_finance_version =(SELECT pfprem_finance_version FROM document d1 JOIN transdetail td ON td.document_id = d1.document_id JOIN pfpremiumfinance pf2 ON pf2.plantransaction_id = td.transdetail_id  WHERE d1.document_ref = MIN(d.document_ref) AND d1.company_id = MIN(d.company_id)) " &
                                                               ",statusind =(SELECT statusind FROM document d1 JOIN transdetail td ON td.document_id = d1.document_id JOIN pfpremiumfinance pf2 ON pf2.plantransaction_id = td.transdetail_id  WHERE d1.document_ref = MIN(d.document_ref) AND d1.company_id = MIN(d.company_id)) " &
                                                               ",IsNull(party_agent.is_gross_agent,0) " &
                                                               ",isnull(Case When d.document_ref Like 'IND%' OR d.document_ref Like 'INC%' then (select TOP 1 ppf.clientid from PFPremiumFinance ppf INNER JOIN insurance_file i  ON i.insurance_file_cnt=ppf.Insurance_File_Cnt  INNER JOIN TransDetail TD  ON i.insurance_ref = TD.insurance_ref where TD.document_id = d.document_id)  Else MIN(acc2.account_key) End,0) " &
                                                               ",isnull((SELECT Min(aa.account_key) FROM account aa INNER JOIN TransDetail td ON aa.account_id=td.account_id INNER JOIN document dd ON dd.document_id=td.document_id WHERE dd.document_ref=d.document_ref AND ISNULL(aa.account_key,0)<>0),0) " &
                                                               ",isnull(Min(i.insurance_folder_cnt),0)"

    Public Const ACRollupTransFilterFields As String = ", " &
                                                       "@has_unrestricted_enquiry, " &
                                                       "@has_unrestricted_update "

    'Modification History:
    'Sankar - Added dt.description - (Tech Spec - PGR025 - SAM Bill Enquiry.doc)
    'Prakash- Added PFT.PFTransaction_Id - (Tech Spec - WCR044 - Bill History.doc) - (3.3.1.1)

    'Amit - Removed the duedate from the group by caluse 
    'Amit - tax due date is Null and and hence grouping is incorrect.
    'Amit - Either Populate the Tax due date field OR remove this from Group By. I prefered the later

    Public Const ACRollupTransFromQueryGroupBy As String = " GROUP BY d.document_id, d.document_ref, a.short_code, a.account_id, d.document_date, t.due_date, t.insurance_ref, " &
                                                           " t.balance_type,Party_type.Code,party_agent.is_float_balance_account,party_agent.is_overdraft_account,t.Reference, Bank_Guarantee.BG_ref, " &
                                                           " ag.shortname, dt.description, PFI.PFTransaction_Id, dtg.description, c2.iso_code,c3.iso_code,c.iso_code,t.company_id,t.purchase_order_no, " &
                                                           "t.purchase_invoice_no,t.insurance_ref,a.account_key, cl1.Claim_id,party_agent.is_gross_agent "


    Public Const ACIncludeExcludedTaxRows As String = " and t.transdetail_id not in (select distinct OTD.transdetail_id from transdetail OTD WITH(NOLOCK)" &
                                                        "JOIN document OD WITH(NOLOCK) ON OD.document_id=OTD.document_id  JOIN (SELECT TD.document_id, " &
                                                        "abs(TC.value)as Amount,td.accounting_date,td.transdetail_type_id,td.period_id,d.insurance_file_cnt " &
                                                        "FROM  TransDetail TD WITH(NOLOCK) INNER JOIN Document D WITH(NOLOCK) ON  D.Document_id = TD.Document_id " &
                                                        "INNER JOIN Tax_Calculation TC WITH(NOLOCK)  ON  TC.Insurance_File_Cnt = D.Insurance_File_Cnt " &
                                                        "where TD.SPARE LIKE '%TAX%' AND TC.tax_band_id = td.tax_band_id and tc.include_tax_in_instalments=0)XTT " &
                                                        "on (OTD.document_id=XTT.document_id and abs(OTD.amount)=XTT.Amount and OTD.accounting_date=XTT.accounting_date and " &
                                                        "OTD.transdetail_type_id=XTT.transdetail_type_id and OTD.period_id=XTT.period_id and OD.insurance_file_cnt=XTT.insurance_file_cnt)) "

    '-------------------------------------------------------------------------------

    Public Const kGetInsuranceFolderDetailsName As String = "Get Insurance Folder Details"
    Public Const kGetInsuranceFolderDetailsSQL As String = "spe_Insurance_Folder_sel"

    Public Const ACGetEventInsuranceFileDocumentName As String = "Get Event Insurance File for Document"
    Public Const ACGetEventInsuranceFileDocumentSQL As String = "spu_TXN_event_insurance_file_sel"
    Public Const ACGetEventInsuranceFileDocumentSp As Boolean = True

    Public Const ACGetCashListTypeCodeName As String = "Get CashListItemType_Code"
    Public Const ACGetCashListTypeCodeSQL As String = "spu_ACT_Get_CashListPaymentType"
    Public Const ACGetCashListTypeCodeSp As Boolean = True

    ' Account Function & CCY Cash Allocation
    Public Const ACGetCurrecnyNotLinkedWithSourceStored As Boolean = True
    Public Const ACGetCurrecnyNotLinkedWithSourceName As String = "Get Currecny Not Linked With Source"
    Public Const ACGetCurrecnyNotLinkedWithSourceSQL As String = "spu_ACT_Get_Currecny_Not_Linked_With_Source"

    'Start - Prakash - WPR85 Parelleling
    Public Const ACSelectLinkedAccountIDsSQL As String = "spu_Get_Linked_CashDeposit_Account_IDs"
    Public Const ACSelectLinkedAccountIDsName As String = "GetLinkedAccountIDs"
    'End - Prakash - WPR85 Parelleling


    Public Const ACSelectSubAgentsSQL As String = "spu_Select_SubAgents"
    Public Const ACSelectSubAgentsName As String = "selectSubAgents"
    Public Const ACSelectSubAgentsStored As Boolean = True

    'WPR07- contra and replace
    Public Const ACGetReceiptReversalSQL As String = "spe_User_Authorities_Sel"
    Public Const ACGetReceiptReversalName As String = "selectUserAuthority"
    Public Const ACGetReceiptReversalStored As Boolean = True

    Public Const ACUpdateCashListItemQueryStored As Boolean = True
    Public Const ACUpdateCashListItemQueryName As String = "UpdateCashListItemQuery"
    Public Const ACUpdateCashListItemQuerySQL As String = "spu_ACT_Set_CashListItem_ReverseAllocation"
    'WPR07- contra and replace

    Public Const ACCheckThirdPartyInstalmentStored As Boolean = True
    Public Const ACCheckThirdPartyInstalmentName As String = "CheckThirdPartyInstalment"
    Public Const ACCheckThirdPartyInstalmentSQL As String = "spu_Check_Third_Party_Instalment"

    Public Const ACPFPremiumFinanceUpdateStatusStored As Boolean = True
    Public Const ACPFPremiumFinanceUpdateStatusName As String = "PFPremiumFinanceUpdateStatusLive"
    Public Const ACPFPremiumFinanceUpdateStatusSQL As String = "spu_PFPremiumFinance_UpdateStatusLive"

    'Update CashListItem for allocation status
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCashListItemForAllocationStatus"
    Public Const ACUpdateSQL As String = "SPU_ACT_UpdateCashListItem_For_AllocationStatus"

End Module
