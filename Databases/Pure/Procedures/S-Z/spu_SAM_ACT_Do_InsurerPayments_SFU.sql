
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_ACT_Do_InsurerPayments_SFU'
GO

--Start (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR63 - Insurer Payments - Get Insurer Payments.doc) - (6.2.3)
CREATE PROCEDURE spu_SAM_ACT_Do_InsurerPayments_SFU  
    @account_id int,  
    @date_to datetime  = NULL,  
    @date_by_trans bit = 0,  
    @marked_status int = -1,  
    @month int         = 0,  
    @alternate_reference varchar(80) = NULL,  
    @company_id int = null ,  
    @YearName varchar(255)=NULL,  
    @PeriodName varchar(255)=NULL,  
    @currencyid INT=0, 
    @policy_number varchar(50) = NULL,  
    @date_from datetime  = NULL, --@due_date_from  
    @reference varchar(100) = NULL,  
    @gross_agent bit = 0,  
    @mediatype varchar(255) = NULL ,
	@excludePendingAuth bit = 0, 
	@onlyPendingAuth bit = 0
AS  
  
    Declare @report_indicator int

    -- Get the report indicator  
    Select  
        @report_indicator = Case  
            When pt.code = 'IN'  
                Then isnull(pi.report_indicator, -1)  
                Else isnull(pa.report_indicator, -1)  
            End  
    From  
        party p  
    Join  
        party_type pt  
        On pt.party_type_id = p.party_type_id  
    Join  
        account a  
        On a.account_key = p.party_cnt  
    Left Join  
        party_agent pa  
        On pa.party_cnt = p.party_cnt  
    Left Join  
        party_insurer pi  
        On pi.party_cnt = p.party_cnt  
    Where  
        a.account_id = @account_id
  
    -- Validate date_to  
    If (@date_to is not null)  
        Select  
            @date_to = dateadd(hh, 23, @date_to),  
            @date_to = dateadd(mi, 59, @date_to),  
            @date_to = dateadd(ss, 59, @date_to)  
  
   If (@date_from IS NOT NULL)  
  SELECT  
     @date_from = dateadd(hh, 00, @date_from),  
     @date_from = dateadd(mi, 00, @date_from),  
     @date_from = dateadd(ss, 00, @date_from)  
  
    -- Validate month  
    If (not isnull(@month, 0) between 1 and 12)  
        Select @month = null  
  
    -- Validate marked status  
    If (@marked_status not in (0, 1))  
        Select @marked_status = null  
  
    -- Validate marked status  PN 33593 (RC)  
    If (@alternate_reference = '')  
        Select @alternate_reference = null  
  
 If (@policy_number = '')  
    Select @policy_number = null  
  
 IF (@reference = '')  
        SELECT @reference = NULL  
  
     IF (@mediatype = '')  
        SELECT @mediatype = NULL;  
  
    -- Perform main query  
    WITH PendingAuth AS (
        SELECT tm.transdetail_id
        FROM TransMatch tm
        INNER JOIN CashListItem cli ON cli.cashlistitem_id = tm.CashListItem_ID
        WHERE ISNULL(cli.transdetail_id, 0) = 0
          AND NOT EXISTS (
              SELECT 1
              FROM Payment_approval pa
              WHERE pa.payment_cnt = cli.cashlistitem_id
                AND pa.payment_type = 2
                AND pa.approved = 0
          )
    )
    Select  
        d.document_id,  
        d.document_ref,  
        case when d.comment = 'Consolidated Binder'  
            then d.comment  
            else td.insurance_ref  
        end [insurer_ref],  
        td_p.currency_amount [fully_paid_amount],  
        td_os.currency_amount [client_outstanding],  
        case when d.comment = 'Consolidated Binder'  
            then 1  
            else 0  
        end [consolidate_binder],  
        -- Policy holder details  
        p_c.shortname,  
        p_c.resolved_name,  
        td.transdetail_id,  
        td.company_id,  
        td.accounting_date,  
        td.currency_amount,  
        td.currency_id,  
        c.code,  
  ISNULL(td.currency_base_xrate,0)'currency_base_xrate' ,  
        -- Get the total amount marked for payment  
        td_m.currency_amount [marked_amount],  
        -- Get the total allocated amount  
        (td.currency_amount - td.outstanding_currency_amount) [paid_amount],  
        td.spare,  
        p.period_name,  
        datepart(mm, dateadd(dd, a.settlement_period, td.accounting_date)) [month],  
        -- Account amounts  
        td.account_currency_id,  
     ca.code,  
  ISNULL(td.account_base_xrate,0)'account_base_xrate',  
        td_p.account_amount [fully_paid_account_amount],  
        td_os.currency_amount [client_outstanding_account_amount],  
        td.account_amount,  
        td_m.account_amount [marked_account_amount],  
        (td.account_amount - td.outstanding_account_amount) [paid_account_amount],  
 td.reference [alternate_reference], --PN 33593 (RC)  
  ifi.cover_start_date [effective_date], --PN 33593 (RC)  
 s.Code as BranchCode ,  
  
  P.year_name,  
  td.due_date,  
  td.system_currency_id,  
  td.system_base_xrate,  
  cs.code SystemCurrencyCode  
    From  
        document d  
    Join  
        transdetail td  
        On td.document_id = d.document_id  
join source s  
    on td.company_id=s.source_id  
    Left Join  
        insurance_file ifi  
      On ifi.insurance_file_cnt = d.insurance_file_cnt  
    Left Join  
        insurance_folder ifo  
        On ifo.insurance_folder_cnt = ifi.insurance_folder_cnt  
    Left Join  
        party p_c -- Client party  
        On p_c.party_cnt = ifo.insurance_holder_cnt  
    Join  
        period p  
        On p.period_id = td.period_id  
    Join  
        account a  
        On a.account_id = td.account_id  
    Join  
        currency c  
        On c.currency_id = td.currency_id  
    Join  
     currency ca  
        On ca.currency_id = td.account_currency_id  
    JOIN  
    currency cs  
      ON cs.currency_id = td.system_currency_id  
    Left Join    -- derived table for marked transaction  
       (Select transdetail_id, sum(currency_match_amount) [currency_amount], sum(account_match_amount) [account_amount]  
            From transmatch  
            Where allocationdetail_id is null  
            Group By transdetail_id) td_m  
        On td_m.transdetail_id = td.transdetail_id  
    Left Join    -- derived table for fully paid totals (need to be added to document totals)  
       (Select document_id, account_id, sum(currency_amount) [currency_amount], sum(account_amount) [account_amount]  
            From transdetail tdi  
            Where outstanding_currency_amount = 0  
            Group By document_id, account_id) td_p  
        On td_p.document_id = d.document_id  
        And td_p.account_id = a.account_id  
    Join        -- derived table for client outstanding amount  
        (Select td.document_id, sum(td_p.outstanding_currency_amount) [currency_amount], sum(td_p.outstanding_account_amount) [account_amount]  
            From  
                transdetail td  
            Join  
                transdetail td_p  
                On td_p.document_id = td.document_id  
                And td_p.account_id = td.account_id  
            Where td.document_sequence = 1  
            Group By td.document_id) td_os  
        On td_os.document_id = d.document_id  
    LEFT JOIN party_agent pa  
    ON a.account_key=pa.party_cnt  
    Where  
        td.account_id = @account_id  
  
    -- Parameterised selection criteria  
  And    -- Date to (and by accounting or transaction date)  
       (@date_to is null  
        Or (@date_to >= td.accounting_date And @date_by_trans = 1)  
        Or (@date_to >= ifi.cover_start_date And @date_by_trans = 0)  
        or (@date_to >= td.accounting_date And d.document_ref like 'JN%'))  
  
  And    -- Date from (and by accounting or transaction date)  
       (@date_from is null  
        Or (@date_from <= td.accounting_date And @date_by_trans = 1)  
        Or (@date_from <= ifi.cover_start_date And @date_by_trans = 0)  
        or (@date_from <= td.accounting_date And d.document_ref like 'JN%'))  
  And    -- Marked status  
       (@marked_status is null  
        Or (@marked_status = 0 And td_m.currency_amount is null)  
        Or (@marked_status != 0 And td_m.currency_amount is not null))  
  And    -- Month  
       (@month is null  
        Or (@month = datepart(mm, dateadd(dd, a.settlement_period, td.accounting_date))))  
  And    -- Alternate Reference  PN 33593 (RC)  
      (@alternate_reference is null  
       OR td.reference LIKE @alternate_reference)  
  AND   --reference  
       (@reference IS NULL  
        OR td.insurance_ref LIKE @reference)  
  AND  
        (td.spare IS NULL  
         OR (td.spare <> 'COMM' AND @gross_agent=1)  
         OR @gross_agent=0)  
  AND  
        (@mediatype IS NULL  
         OR @mediatype = pa.payment_method)  
  AND (td.company_id = @company_id OR @company_id IS NULL)  
  AND ( @YearName IS NULL  OR P.year_name IN (SELECT * FROM UF_StringToTable(@YearName)))  
  AND ( @PeriodName  IS NULL  OR P.period_name  IN (SELECT * FROM UF_StringToTable(@PeriodName) ))  
  AND  -- Ignore fully matched items  
        (( @YearName IS NULL AND @PeriodName IS NULL AND td.outstanding_currency_amount <> 0  )  
  OR  
  (( @YearName IS NOT NULL OR @PeriodName IS NOT NULL) AND  td.outstanding_currency_amount <> 0 AND td.spare NOT LIKE 'Revers%' ))  
  AND (@currencyid =0 OR TD.currency_id =@currencyid)  
  AND     -- Policy Number  
    (@policy_number is null  
  OR td.insurance_ref  LIKE @policy_number) 		
AND (@ExcludePendingAuth = 0 OR TD.transdetail_id NOT IN (SELECT PendingAuth.transdetail_id FROM PendingAuth))
  AND (@OnlyPendingAuth = 0 OR TD.transdetail_id IN (SELECT PendingAuth.transdetail_id FROM PendingAuth))
    Order By  
        Case @report_indicator  
            When 0 Then td.accounting_date  
            When 1 Then td.insurance_ref  
            When 2 Then p_c.shortname  
            Else null  
        End  
--End (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR63 - Insurer Payments - Get Insurer Payments.doc) - (6.2.3)  
Go
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
