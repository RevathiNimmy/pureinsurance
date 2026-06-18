SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_ACT_Do_InsurerPayments_SFU
GO

CREATE PROCEDURE spu_ACT_Do_InsurerPayments_SFU
	@account_id int,
	--@date_to datetime  = NULL,
	@date_by_trans int = 0,
	@marked_status int = -1,
	@month int         = 0,
	@alternate_reference varchar(80) = NULL, --PN 33593 (RC)
	@due_date_from datetime = NULL,
	@due_date_to datetime = NULL,
	@YearName varchar(255)=NULL,
	@PeriodName varchar(255)=NULL,
	@currencyid INT=0 ,
	@instalment_by_duedate bit = 0 ,
	@reference varchar(100) = NULL,
	@gross_agent bit = 0,
	@mediatype int = 0
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
  party p with (nolock)
     Join
         party_type pt with (nolock)
         On pt.party_type_id = p.party_type_id
     Join
  account a with (nolock)
  On a.account_key = p.party_cnt
     Left Join
         party_agent pa with (nolock)
         On pa.party_cnt = p.party_cnt
     Left Join
         party_insurer pi with (nolock)
         On pi.party_cnt = p.party_cnt
 WHERE a.account_id = @account_id

    -- Validate date_to
    --If (@date_to is not null)
    --    Select
    --        @date_to = dateadd(hh, 23, @date_to),
    --        @date_to = dateadd(mi, 59, @date_to),
    --        @date_to = dateadd(ss, 59, @date_to)

 If (@due_date_from is not null)
  SELECT
   @due_date_from = dateadd(hh, 00, @due_date_from),
   @due_date_from = dateadd(mi, 00, @due_date_from),
   @due_date_from = dateadd(ss, 00, @due_date_from)

 -- validate due_date_to
 If (@due_date_to is not null)
  SELECT
   @due_date_to = dateadd(hh, 23, @due_date_to),
   @due_date_to = dateadd(mi, 59, @due_date_to),
   @due_date_to = dateadd(ss, 59, @due_date_to)

    -- Validate month
    If (not isnull(@month, 0) between 1 and 12)
        Select @month = null

    -- Validate marked status
    If (@marked_status not in (0, 1))
        Select @marked_status = null

    -- Validate marked status  PN 33593 (RC)
    If (@alternate_reference = '')
        Select @alternate_reference = null

		    -- Validate reference
    If (@reference = '')
        Select @reference = null
    -- Validate mediatype
    If (@mediatype = 0)
        Select @mediatype = null

    -- Perform main query
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
        d.document_date,
        td.currency_amount,
        td.currency_id,
        c.code TransactionCurrencyCode,
 ISNULL(td.currency_base_xrate,0) AS currency_base_xrate,
        -- Get the total amount marked for payment
        case when substring(d.document_ref,1,3) = 'IND' OR substring(d.document_ref,1,3) = 'IED' OR substring(d.document_ref,1,3) = 'IRD'
         then td_mi.currency_amount
        else td_m.currency_amount
        end [marked_amount],
        -- Get the total allocated amount
        (td.currency_amount - td.outstanding_currency_amount) [paid_amount],
        td.spare,
        p.period_name,
        datepart(mm, dateadd(dd, a.settlement_period, td.accounting_date)) [month],
        -- Account amounts
        td.account_currency_id,
        ca.code AccountCurrencyCode,
 ISNULL(td.account_base_xrate,0) account_base_xrate,
        td_p.account_amount [fully_paid_account_amount],
        td_os.account_amount [client_outstanding_account_amount],
       td.account_amount,
                 case when substring(d.document_ref,1,3) = 'IND' OR substring(d.document_ref,1,3) = 'IED' OR substring(d.document_ref,1,3) = 'IRD'
            then td_mi.account_amount
            else td_m.account_amount
        end [marked_account_amount],
        (td.account_amount - td.outstanding_account_amount) [paid_account_amount],
  td.reference [alternate_reference], --PN 33593 (RC)
   --ifi.cover_start_date [effective_date], --PN 33593 (RC)
   ISNULL(ifi.cover_start_date,td.accounting_date) [effective_date],
  td.comment,
  td.due_date,
  ISNULL(p_c.currency_id, a.currency_id) as currency_id,
  P.year_name,
    case when substring(d.document_ref,1,3) = 'IND' OR substring(d.document_ref,1,3) = 'IED' OR substring(d.document_ref,1,3) = 'IRD'
            then (SELECT TOP 1 DueDate FROM pfinstalments pfi with (nolock)
            INNER JOIN pfpremiumfinance pfp with (nolock) ON pfp.pfprem_finance_cnt=pfi.pfprem_finance_cnt AND pfp.pfprem_finance_version=pfi.pfprem_finance_version
            INNER JOIN document d1 with (nolock) ON pfp.insurance_file_cnt=d.insurance_file_cnt
            WHERE d1.document_id=d.document_id AND pfi.status <> 3)        
            else td.accounting_date
        end [DueDate],
    pfin.InstalmentNumber,
    pfin.duedate as InsDueDate,
    pfin.Amount,
    pfin.pfprem_finance_cnt,
    pfin.pfprem_finance_version,
    mt.description as MediaType,
    pfin.pfinstalments_id,
    pfin.Amount/ISNULL(td.account_base_xrate,0) [InstAccountAmount],
	td.system_currency_id,
	td.system_base_xrate,
	cs.code SystemCurrencyCode,
 -- End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (5.1.1)
	'',
 	CASE WHEN EXISTS(SELECT TransDetailEx.transdetailex_id  
				FROM TransDetailEx 
					JOIN transdetail ON transdetailex.transdetail_id = transdetail.transdetail_id  
						WHERE d.document_id = transdetailex.document_id  
						AND a.account_id = transdetailex.account_id ) 
	THEN 1 
	ELSE 0 
	END 'IsDebitOrderTransDetail'
    From
        document d with (nolock)
    Join
        transdetail td with (nolock)
        On td.document_id = d.document_id
    Left Join
        insurance_file ifi with (nolock)
        On ifi.insurance_file_cnt = d.insurance_file_cnt
    Left Join
        party p_c with (nolock)-- Client party
        On p_c.party_cnt = ifi.insured_cnt
    Join
        period p with (nolock)
        On p.period_id = td.period_id
    Join
        account a  with (nolock)
        On a.account_id = td.account_id
    Join
        currency c with (nolock)
        On c.currency_id = td.currency_id
    Join
     currency ca with (nolock)
      On ca.currency_id = td.account_currency_id
   Join
    currency cs with (nolock)
      On cs.currency_id = td.system_currency_id

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
    Left Join        -- derived table for client outstanding amount
        (Select td.document_id,  sum(td_p.outstanding_currency_amount) [currency_amount], sum(td_p.outstanding_account_amount) [account_amount]      
            From      
                transdetail td      
            Join      
                transdetail td_p      
                On td_p.document_id = td.document_id      
           And td_p.account_id = td.account_id      
            Where td.document_sequence = 1
            OR (td.spare = 'TAX'  --Where no GROSS exists and only tax entry in any MTA
                 AND Exists (Select 1 From transdetail t1 Where t1.document_id = td.document_id 
                               GROUP BY t1.Document_id HAVING MIN(t1.document_sequence) = 2))     
      
            Group By td.document_id ) td_os      
        On td_os.document_id = d.document_id
	 Left Join pfpremiumfinance pfpf with (nolock)
     ON d.insurance_file_cnt=pfpf.insurance_file_cnt AND (substring(d.document_ref,1,3) = 'IND' OR substring(d.document_ref,1,3) = 'IED' OR substring(d.document_ref,1,3) = 'IRD')
    Left Join pfinstalments pfin with (nolock)
    ON pfpf.pfprem_finance_cnt=pfin.pfprem_finance_cnt AND pfpf.pfprem_finance_version=pfin.pfprem_finance_version
    Left Join    -- derived table for marked transaction
       (Select transmatch.transdetail_id, transmatch.currency_match_amount [currency_amount], transmatch.account_match_amount [account_amount],instalmentnumber
            From transmatch
            Where allocationdetail_id is null
            ) td_mi
        On td_mi.transdetail_id = td.transdetail_id AND (td_mi.instalmentnumber= pfin.instalmentnumber OR td_mi.instalmentnumber IS NULL)
    Left Join party_agent pa with (nolock)
    On a.account_key=pa.party_cnt
    Left Join mediatype mt
    On pa.payment_method = mt.mediatype_id
    Where
        td.account_id = @account_id

    -- Parameterised selection criteria
    And    -- Date to (and by accounting or transaction date)
       (@due_date_to is null
        Or (@due_date_to >= td.accounting_date And @date_by_trans = 1)
        Or (@due_date_to >= ifi.cover_start_date And @date_by_trans = 0)
		Or (@due_date_to >= td.due_date  And @date_by_trans = 2)
        or (@due_date_to >= td.accounting_date And d.document_ref like 'JN%'))
 AND
  (@due_date_from is null 
   Or (@due_date_from <= td.accounting_date And @date_by_trans = 1)
        Or (@due_date_from <= ifi.cover_start_date And @date_by_trans = 0)
		Or (@due_date_from <= td.due_date  And @date_by_trans = 2)
  OR (@due_date_from <= td.accounting_date And d.document_ref like 'JN%'))
 AND
  (@due_date_to is null OR @due_date_to >= td.accounting_date)
    And    -- Marked status
       (@marked_status is null
        Or (@marked_status = 0 And td_m.currency_amount is null)
        Or (@marked_status != 0 And td_m.currency_amount is not null))
    And    -- Month
       (@month is null
        Or (@month = datepart(mm, dateadd(dd, a.settlement_period, td.accounting_date))))
 And    -- Alternate Reference  PN 33593 (RC)
    (@alternate_reference is null
  or td.reference LIKE @alternate_reference)
And d.documenttype_id not in (select documenttype_id from documenttype where code in ('IDR','ICR','ICA','IND','INC','IED','IEC','IRD','IRC','IID','IIC'))
    And   --reference
       (@reference is null
        or td.insurance_ref LIKE @reference)
  AND  (pfin.duedate is null
    Or (@due_date_to is null)
    Or (@due_date_to>=pfin.duedate AND @instalment_by_duedate=1)
    Or @instalment_by_duedate=0)
    AND  (pfin.InstalmentNumber is null or pfin.InstalmentNumber<>0)
    And
        (td.spare is null
         Or (td.spare <> 'COMM' AND @gross_agent=1)
         Or @gross_agent=0)
    And
        (@mediatype is null
         Or @mediatype = pa.payment_method)
   AND (pfin.status is null Or pfin.status <> 3)
AND ( @YearName IS NULL  OR P.year_name IN (SELECT * FROM UF_StringToTable(@YearName)))
  AND ( @PeriodName  IS NULL  OR P.period_name  IN (SELECT * FROM UF_StringToTable(@PeriodName) ))
  AND  -- Ignore fully matched items
        (( @YearName IS NULL AND @PeriodName IS NULL AND td.outstanding_currency_amount <> 0  )
  OR
  (( @YearName IS NOT NULL OR @PeriodName IS NOT NULL)) AND ( td.outstanding_currency_amount <> 0 AND td.spare NOT LIKE 'Revers%' ))
AND (@currencyid =0 OR TD.currency_id =@currencyid)
AND TD.transdetail_id  NOT IN (	SELECT tm.transdetail_id 
								FROM TransMatch tm
								INNER JOIN CashListItem cli ON cli.cashlistitem_id = tm.CashListItem_ID
								WHERE ISNULL(cli.transdetail_id, 0) = 0
								AND NOT Exists(	SELECT Null 
												FROM Payment_approval pa 
												WHERE pa.payment_cnt = cli.cashlistitem_id 
												AND pa.payment_type = 2 
												AND pa.approved = 0))
    Order By
        Case @report_indicator
            When 0 Then td.accounting_date
 End,
 Case @report_indicator
            When 1 Then td.insurance_ref
            When 2 Then p_c.shortname
            Else NULL
        End


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

