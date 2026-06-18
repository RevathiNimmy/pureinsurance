SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_Banking'
GO

CREATE PROCEDURE spu_ACT_Do_Banking  
    @account_id INT,  
    @date_to DATETIME = NULL,  
    @search_by_effectivedate TINYINT = 0,  
    @marked_status INT = NULL,  
    @receipt_type_id INT = NULL,
    @media_type_id INT = NULL,
    @branch_id INT = 0
 
AS  
SET @date_to = ISNULL(@date_to,GETDATE()) 

CREATE TABLE #temp_transdetail
(
marked_status INT,
branch_desc VARCHAR(255),
account_name VARCHAR(60),
document_ref VARCHAR(25),
document_date DATETIME,
media_type_desc VARCHAR(255),
Amount  MONEY,
media_ref_desc VARCHAR(100),
receipt_type_desc VARCHAR(50), 
short_code VARCHAR(20),
UserName VARCHAR(255),
currency_id INT,
accounting_date DATETIME,
transdetail_id INT,
company_id INT)

INSERT INTO #temp_transdetail
SELECT 
  ( SELECT  
  	ISNULL(SUM(0)+1,0)  
    FROM transmatch tm  
    WHERE tm.allocationdetail_id IS NULL  
    AND tm.transdetail_id = td1.transdetail_id ),
  cmp.description ,
  a.account_name ,
  d.document_ref ,
  d.document_date ,
  mt.description ,
  td1.system_amount  ,
  cli.media_ref ,
  clrt.description , 
  a.short_code ,
  pmu.UserName ,
  t.currency_id,
  t.accounting_date,
  td1.transdetail_id,
  d.company_id
  FROM  account a 
  INNER JOIN TransDetail t 
        ON t.account_id = a.account_id 
  LEFT  JOIN transdetail_type tt 
        ON tt.transdetail_type_id = t.transdetail_type_id 
  LEFT  JOIN CashListItem cli 
        ON cli.transdetail_id = t.transdetail_id 
  LEFT  JOIN cashlistitem_receipt_type clrt 
        ON clrt.cashlistitem_receipt_type_id = cli.cashlistitem_receipt_type_id
  LEFT  JOIN mediatype mt 
        ON mt.mediatype_id = cli.mediatype_id 
  INNER JOIN Document d 
        ON d.document_id = t.document_id 
        AND d.documenttype_id = 22
  INNER JOIN PMUser pmu 
        ON pmu.user_id = t.operator_id 
  LEFT  JOIN company cmp  
        ON d.company_id = cmp.company_id
  JOIN (SELECT transdetail_id FROM transdetail WHERE document_id IN 
       (SELECT document_id FROM transdetail WHERE account_id = @account_id)) ttd
        ON ttd.transdetail_id= t.transdetail_id 
  JOIN  transdetail td1
       ON td1.document_id = t.document_id 
       AND td1.transdetail_id <> t.transdetail_id 
  WHERE t.account_id <> @account_id
      AND ( (ISNULL(@search_by_effectivedate,0)=0 OR t.accounting_date <= @date_to) AND
          ( @search_by_effectivedate =1 OR d.document_date <= @date_to))
      AND (clrt.cashlistitem_receipt_type_id = @receipt_type_id OR ISNULL(@receipt_type_id,0) = 0)
      AND (mt.mediatype_id = @media_type_id OR  ISNULL(@media_type_id,0) = 0)
      AND (cmp.company_id = @branch_id OR @branch_id=0)


 
DELETE #temp_transdetail 
FROM #temp_transdetail ttd
  JOIN AllocationDetail AD  ON ttd.transdetail_id = AD.transdetail_id
  JOIN  
      Allocation A ON (A.allocation_id = AD.allocation_id)  
  JOIN  
      PMUser U ON (U.user_id = A.user_id)  
  JOIN  
      DocumentType DT ON (DT.documenttype_id = AD.documenttype_id)  
  JOIN  
      TransDetail TD ON (TD.transdetail_id = AD.transdetail_id)  
  JOIN  
      Account ACC ON (ACC.account_id = TD.account_id)  
  LEFT OUTER JOIN  
      CashListItem CLI ON (CLI.TransDetail_Id = TD.TransDetail_Id)  
  LEFT JOIN  
      TransMatch TM ON (AD.AllocationDetail_id = TM.AllocationDetail_id  
                    AND AD.transdetail_id = TM.transdetail_id)  
  WHERE  
      AD.allocation_id IN (SELECT  
          allocation_id  
      FROM  
          allocationdetail ad2  
          LEFT JOIN  
              TransMatch tm2 ON (ad2.AllocationDetail_id = tm2.AllocationDetail_id)  
      WHERE  
              (  
                  ad2.transdetail_id = ttd.transdetail_id 
               OR tm2.transdetail_id = ttd.transdetail_id  
              )  
         AND   ISNULL(tm2.is_reversed,0) = 0  
          )  
      AND  
      ISNULL(tm.is_reversed,0) = 0 

IF @marked_status IS NOT NULL
DELETE FROM #temp_transdetail 
WHERE marked_status <> @marked_status

SELECT * FROM #temp_transdetail
ORDER BY company_id, document_date DESC

DROP TABLE #temp_transdetail