SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_Get_Premium_Finance_Balance'
GO

CREATE PROCEDURE spu_Get_Premium_Finance_Balance  
    @lAccount_ID INTEGER = NULL,  
    @lInsurance_File_cnt INTEGER = NULL,
    @daccounting_date DATETIME = NULL ,
    @sInsurance_ref VARCHAR(255) = NULL,
    @mOutstandingFromPF MONEY  = 0 OUTPUT
AS 
BEGIN

DECLARE @lInsurance_Folder_cnt AS INTEGER

if @daccounting_date is NULL 
	SET @daccounting_date = GETDATE()
	IF CAST(@daccounting_date AS TIME) = '00:00:00.000'  
BEGIN  
    SET @daccounting_date = DATEADD(MILLISECOND, -1, DATEADD(DAY, 1, CAST(@daccounting_date AS DATETIME)))  
END  

IF ISNULL(@lAccount_ID,0) <> 0 
BEGIN
	IF LEN(RTRIM(LTRIM(@sInsurance_ref))) > 3
BEGIN    
  SELECT @mOutstandingFromPF =     
   (     
             SELECT      
    ISNULL(SUM(ROUND(td.amount,2)),0)      
              FROM account a      
              JOIN party p      
                  ON p.party_cnt = a.account_key      
              JOIN pfpremiumfinance pfpf      
                  ON pfpf.clientid = p.party_cnt      
              JOIN transdetail td      
                  ON td.transdetail_id = pfpf.plantransaction_id      
              JOIN document d      
                  ON d.document_id = td.document_id      
                  AND d.document_date <= @daccounting_date    
              WHERE a.account_id = @laccount_id    
    AND RTRIM(LTRIM(td.Insurance_Ref)) = @sInsurance_Ref    
   )    
   -    
   (
            ISNULL((SELECT SUM(ROUND(pfpf.AmountToFinance, 2))  
              FROM account a      
              JOIN party p      
                  ON p.party_cnt = a.account_key      
              JOIN pfpremiumfinance pfpf      
                  ON pfpf.clientid = p.party_cnt      
              JOIN transdetail td      
                  ON td.transdetail_id = pfpf.plantransaction_id      
              JOIN document d      
                  ON d.document_id = td.document_id      
                  AND d.document_date <= @daccounting_date    
              WHERE a.account_id = @laccount_id     
    AND RTRIM(LTRIM(td.Insurance_Ref)) = @sInsurance_Ref    
    AND pfpf.statusind = 990
            ), 0) 
            +
            ISNULL((
             SELECT SUM(ROUND(amount, 2))
        	FROM  PFInstalments
        	WHERE pfprem_finance_cnt  IN ( SELECT pfpf.pfprem_finance_cnt FROM account a    
            JOIN pfpremiumfinance pfpf ON pfpf.clientid = a.account_key
            WHERE a.account_id = @laccount_id) AND Status = 3), 0)
   )
  END    
  ELSE    
  BEGIN    
   SELECT @mOutstandingFromPF = 
    	(
		 SELECT    
    ISNULL(SUM(ROUND(td.amount,2)),0)    
              FROM account a    
              JOIN party p    
                  ON p.party_cnt = a.account_key    
              JOIN pfpremiumfinance pfpf    
                  ON pfpf.clientid = p.party_cnt    
              JOIN transdetail td    
                  ON td.transdetail_id = pfpf.plantransaction_id    
              JOIN document d    
                  ON d.document_id = td.document_id    
                  AND d.document_date <= @daccounting_date    
              WHERE a.account_id = @laccount_id    
        )
        -
        (
            ISNULL((SELECT SUM(ROUND(pfpf.AmountToFinance, 2))  
                FROM account a    
                JOIN party p
				 ON p.party_cnt = a.account_key    
                JOIN pfpremiumfinance pfpf
				 ON pfpf.clientid = p.party_cnt
                JOIN transdetail td
				 ON td.transdetail_id = pfpf.plantransaction_id  
                JOIN document d
				 ON d.document_id = td.document_id                  
                AND d.document_date <= @daccounting_date
				WHERE a.account_id = @laccount_id  
				AND pfpf.statusind = 990
            ), 0) 
            +
            ISNULL((SELECT SUM(ROUND(amount, 2))
        	FROM  PFInstalments
        	WHERE pfprem_finance_cnt  IN ( SELECT pfpf.pfprem_finance_cnt FROM account a    
            JOIN pfpremiumfinance pfpf ON pfpf.clientid = a.account_key
            WHERE a.account_id = @laccount_id) AND Status = 3), 0)
        )
		END
END
ELSE
BEGIN
	SELECT @mOutstandingFromPF = 0
END

	SELECT ISNULL(@laccount_id,0) AS Account_id,
	@mOutstandingFromPF AS Premium_finance_balance

END


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

