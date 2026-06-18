SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_get_BGs_for_policy'
GO

CREATE PROCEDURE spu_get_BGs_for_policy    
 @product_id                    INT,    
 @source_id                       INT,    
 @cover_from_date       DATETIME,    
 @Insurance_File_Cnt  INT,    
 @Party_Cnt                      INT ,    
 @Total_Premium           MONEY,    
 @TranCurrency_Id         INT,   
 @DueDate                         DATETIME=null OUTPUT    
   
    
AS    
    
BEGIN    
--DECLARE @system_currency_id INT,    
-- @bg_currency_id  INT    
    
  SELECT    
                         0 AS RowIndex,    
                         BG.bg_id,    
                         bg_ref,    
                         BG_Limit,    
                         bank_name_id,    
                         BG_currency_id,    
                         BG.Party_Cnt,    
                         expiry_date,    
                         available_bal,    
                         issue_date,    
                         P.Shortname,    
                         P.Resolved_Name    
                         ,b.bank_name    
           FROM Bank_Guarantee BG    
    
   INNER JOIN Bank b    
    ON BG.Bank_name_id = b.bank_id    
   INNER JOIN Party p    
    ON p.party_cnt = BG.party_cnt    
   INNER JOIN BG_Product_Link BGPL    
    ON BGPL.Bg_Id = BG.Bg_Id    
   INNER JOIN BG_Branch_Link BGBL    
    ON BGBL.Bg_Id = BG.Bg_Id    
   INNER JOIN BG_Status BGS    
    ON BGS.BG_Status_id = BG.BG_Status_id    
  WHERE    
 BG.Party_cnt = @Party_Cnt    
   AND    BG.Is_deleted = 0    
   AND    BG.BG_Currency_Id = @TranCurrency_Id      
   AND    BGPL.Product_id = @Product_Id    
   AND    BGBL.Source_id = @Source_Id    
    
    AND  BG.Issue_Date <= @cover_from_date AND BG.Expiry_Date >= @cover_from_date    
   AND   BGS.code <> 'Invoked' AND BGS.code <> 'Expired'    
                        AND     BG.available_bal >= @total_premium    
                        AND    
   (    
     (BG.Is_policy_lock = 1 AND NOT EXISTS(SELECT bg_id FROM Insurance_File_BG_Link  WHERE bg_id = BG.bg_id))    
   OR    
    (BG.Is_Policy_lock = 0)    
    
    )  
    
    
 END    
    
 SELECT @DueDate=dateadd(mm, datediff(mm, '1/1/1900', @cover_from_date )+2, '1/1/1900') - 1    
 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

