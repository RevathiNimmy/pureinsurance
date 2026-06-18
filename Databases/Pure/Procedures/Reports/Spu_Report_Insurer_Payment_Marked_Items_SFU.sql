/**********************************************************************************************************************************
** Created by Gaurav Arora
** Created On 08 Mar 2006 At 4:30 PM
** Created For S4I Reports - Insurer_Payment_Marked_Items_Report.rpt**
**********************************************************************************************************************************
***********************************************************************************************************************************/

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'Spu_Report_Insurer_Payment_Marked_Items_SFU'
GO


CREATE PROCEDURE Spu_Report_Insurer_Payment_Marked_Items_SFU  
    @account_id int,  
    @date_to NVARCHAR(50),
    @date_by_trans int = 0,  
    @marked_status int = -1,  
    @month int         = 0  
AS  
  
SELECT @date_to= CONVERT(DATETIME,@date_to,103)

   -- Validate date_to  
    IF (@date_to IS NOT NULL)  
        SELECT  
            @date_to = DATEADD(hh, 23, @date_to),  
            @date_to = DATEADD(mi, 59, @date_to),  
            @date_to = DATEADD(ss, 59, @date_to)  
  
    -- Validate month  
    IF (NOT ISNULL(@month, 0) BETWEEN 1 AND 12)  
        SELECT @month = NULL  
  
    -- Validate marked status  
    IF (@marked_status NOT IN (0, 1))  
        SELECT @marked_status = NULL  
  
 CREATE TABLE #MarkedItems  
 (  
  document_id   int,  
  document_ref   varchar(25),  
  insurance_ref   varchar(30),  
  shortName   char(30),  
  accounting_Date  datetime,  
  Premium   numeric(11,2),  
  tax_amount   numeric(11,2),  
  commission_amount  numeric(11,2),  
  insurance_file_cnt  int,  
  descr                varchar(255),  
  account_name   varchar(60),  
  add1    varchar(40),  
  add2    varchar(40),
  add3    varchar(40),  
  add4    varchar(40),  
  marked_amount   numeric(11,2),  
         currency_base_xrate  numeric(4,2),  
  reference  varchar(50)  
 )  
  
    --Inserting Data into Temp Table  
    INSERT INTO #MarkedItems  
    SELECT  
        DISTINCT d.document_id,  
        d.document_ref,  
        CASE  
     WHEN d.comment = 'Consolidated Binder'  
            THEN d.comment  
            ELSE ifi.insurance_ref  
        END [insurer_ref],  
        p_c.shortname,  
        td.accounting_date,  
 (ifi.this_premium * td.currency_base_xrate) Premium,  
  
 (SELECT SUM(account_amount)  
     FROM transDetail  
     WHERE transdetail.document_id=d.document_id  
     AND transDetail.spare LIKE '%TAX%'  
            AND transdetail.account_id = a.account_id) tax_amount,  
  
 (SELECT SUM(account_amount)  
     FROM transDetail  
     WHERE transdetail.document_id=d.document_id  
     AND transDetail.spare LIKE '%COMM%'  
     AND transdetail.account_id = a.account_id) commission_amount,  
  
 ifi.insurance_file_cnt,  
 dt.description document_desc,  
 a.account_name account_name,  
 a.address1 add1,  
 a.address2 add2,  
 a.address3 add3,  
 a.address4 add4,  
  
 (SELECT Sum(account_match_amount)  
     FROM transmatch TM  
     WHERE allocationdetail_id IS NULL  
     AND TM.transdetail_id = TD.transdetail_id) marked_amount,
 
 td.currency_base_xrate,  
 (SELECT cp.thirdpartyreference FROM stats_folder SF
 INNER JOIN claim_payment CP ON CP.claim_payment_id = SF.payment_id  
  AND LEFT(document_ref, 3) like 'CLP'  
 AND d.document_ref = SF.document_ref)  
    FROM  
        transdetail td  
    JOIN  
        document d  
        ON td.document_id = d.document_id  
    JOIN  
 DocumentType dt  
 ON dt.documentType_id = d.documentType_id  
    LEFT JOIN  
        insurance_file ifi  
        ON ifi.insurance_file_cnt = d.insurance_file_cnt  
    LEFT JOIN  
        insurance_folder ifo  
        ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt  
    LEFT JOIN  
        party p_c -- Client party  
        ON p_c.party_cnt = ifo.insurance_holder_cnt  
    JOIN  
        account a  
        ON a.account_id = td.account_id  
  
    WHERE  
        td.account_id = @account_id  
    AND  
        -- Ignore fully matched items  
        td.outstanding_currency_amount <> 0  
  
    -- Parameterised selection criteria  
    AND    -- Date to (and by accounting or transaction date)  
       (@date_to IS NULL  
        OR (@date_to >= td.accounting_date AND @date_by_trans = 0)  
        OR (@date_to >= d.created_date AND @date_by_trans = 1)  
		OR (@date_to >= td.due_date AND @date_by_trans = 2))
    AND    -- Marked status  
       (@marked_status IS NULL  
        OR (@marked_status = 0)  
        OR (@marked_status != 0))  
    AND    -- Month  
       (@month IS NULL  
        OR (@month = datepart(mm, dateadd(dd, a.settlement_period, td.accounting_date))))  
  
    -- Perform main query  
    SELECT      document_id,  
          document_ref,  
      insurance_ref,  
      shortName,  
      accounting_Date,  
      ISNULL(Premium,0) Premium,  
      ISNULL(tax_amount,0) tax_amount,  
      ISNULL(commission_amount,0) commission_amount,  
      insurance_file_cnt,  
                    descr,  
      account_name,  
      add1,  
      add2,  
      add3,  
      add4,  
      ISNULL(SUM(marked_amount),0.00) Marked_Amount,  
        reference  
    FROM #MarkedItems  
    WHERE      Marked_Amount <> 0  
    GROUP BY      document_id,  
      document_ref,  
      insurance_ref,  
      shortName,  
      accounting_Date,  
      Premium,  
      tax_amount,  
      commission_amount,  
      insurance_file_cnt,  
      descr,  
      account_name,  
      add1,  
      add2,  
      add3,  
      add4,  
      reference  


    DROP TABLE #MarkedItems


GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

