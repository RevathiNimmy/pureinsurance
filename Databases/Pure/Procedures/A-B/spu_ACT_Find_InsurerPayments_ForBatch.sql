SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_ACT_Find_InsurerPayments_ForBatch
GO

CREATE PROCEDURE spu_ACT_Find_InsurerPayments_ForBatch  
    @nBatchID int
    
AS  
   
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
        td.accounting_date,  
        td.currency_amount,  
        td.currency_id,  
        c.code,  
        ISNULL(td.currency_base_xrate,0),  
        -- Get the total amount marked for payment  
        td_m.currency_amount [marked_amount],  
        -- Get the total allocated amount  
        (td.currency_amount - td.outstanding_currency_amount) [paid_amount],  
        td.spare,  
        p.period_name,  
        datepart(mm, dateadd(dd, a.settlement_period, td.accounting_date)) [month],          
        td.account_currency_id,  
        ca.code,  
        ISNULL(td.account_base_xrate,0),  
        td_p.account_amount [fully_paid_account_amount],  
        td_os.account_amount [client_outstanding_account_amount],  
        td.account_amount,  
        td_m.account_amount [marked_account_amount],  
        (td.account_amount - td.outstanding_account_amount) [paid_account_amount],  
        td.reference [alternate_reference],
        ifi.cover_start_date [effective_date],
        td.Comment, 
        td.due_date,        
        p_c.currency_id,
        P.year_name,
        case when substring(d.document_ref,1,3) = 'IND' OR substring(d.document_ref,1,3) = 'IED' OR substring(d.document_ref,1,3) = 'IRD'
            then (SELECT TOP 1 DueDate FROM pfinstalments pfi with (nolock)
            INNER JOIN pfpremiumfinance pfp with (nolock) ON pfp.pfprem_finance_cnt=pfi.pfprem_finance_cnt AND pfp.pfprem_finance_version=pfi.pfprem_finance_version
            INNER JOIN document d1 with (nolock) ON pfp.insurance_file_cnt=d.insurance_file_cnt
            WHERE d1.document_id=d.document_id AND pfi.status <> 3)        
            else td.accounting_date
        end [DueDate],
        '' AS InstalmentNumber,
        '' AS InsDueDate,
        '' AS Amount,
        '' AS pfprem_finance_cnt,
        '' AS pfprem_finance_version,
        '' AS MediaType,
        '' AS pfinstalments_id,
        '' AS [InstAccountAmount],
        '' AS system_currency_id,
        '' AS system_base_xrate,
        '' AS SystemCurrencyCode,
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
        insurance_folder ifo with (nolock)  
        On ifo.insurance_folder_cnt = ifi.insurance_folder_cnt  
    Left Join  
        party p_c with (nolock)-- Client party  
        On p_c.party_cnt = ifo.insurance_holder_cnt  
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
        (SELECT td.document_id,  
                 SUM(td_p.outstanding_currency_amount) [currency_amount],  
                 SUM(td_p.outstanding_account_amount)  [account_amount],  
                 SUM(td_p.amount)                      [amount]  
          FROM   transdetail td  
                 JOIN transdetail td_p  
                   ON td_p.document_id = td.document_id  
                      AND td_p.account_id = td.account_id  
          WHERE  td.document_sequence = 1  
          GROUP  BY td.document_id) td_os  
           ON td_os.document_id = d.document_id  
 LEFT JOIN -- Find the write-off transaction against the client  
  (SELECT DISTINCT transdetail_id  
   FROM   allocationdetail ad  
   WHERE  allocation_id IN (SELECT allocation_id  
                            FROM   allocationdetail  
                            WHERE  documenttype_id = 14)  
          AND ad.transdetail_id IN (SELECT td_p.transdetail_id  
                                    FROM   transdetail td  
                                           JOIN transdetail td_p  
                                             ON td_p.document_id =td.document_id  
                                                AND td_p.account_id =td.account_id  
                                    WHERE  td.document_sequence = 1)) td_wo  
    ON td.transdetail_id = td_wo.transdetail_id  
    Where  
    Td.TransDetail_id in 
    (
SELECT key_value FROM PMNav_Batch_Key_Value where PMNav_Batch_Set_id= @nBatchID )

And  
       -- Ignore fully matched items  
        td.outstanding_currency_amount <> 0  
  
    -- Parameterised selection criteria  
  and d.documenttype_id not in (select documenttype_id from documenttype where code in ('IDR','ICR','ICA','IND','INC','IED','IEC','IRD','IRC','IID','IIC'))  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

