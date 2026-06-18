SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Validate_BG_for_Policy'
GO
 CREATE PROCEDURE spu_SAM_Validate_BG_for_Policy       
 @product_id   INT,                  
 @source_id   INT,                  
 @cover_from_date  DATETIME,                  
 @Insurance_File_Cnt  INT,                  
 @Transaction_Curreny_Id  INT,                  
 @Party_Cnt   INT,                  
 @bg_id    INT,                
 @total_inf_premium NUMERIC(20,4),          
 @user INT,      
 @Lead_Agent_Cnt INT           
                  
AS                  
                  
BEGIN                  
DECLARE                   
 @system_currency_id INT,                  
 @bg_currency_id  INT,                         
 @Validate_BG  SMALLINT,                  
 @Validate_Deleted SMALLINT,                  
 @Validate_Bal  SMALLINT,                  
 @Validate_Product_Id SMALLINT,                  
 @Validate_Branch_Id SMALLINT,                  
 @Validate_Cover_From SMALLINT,                  
 @Validate_Currency SMALLINT,                  
 @Validate_Status SMALLINT,                 
 @Validate_Party_Agent SMALLINT,           
 @Validate_User SMALLINT ,          
 @Validate_Product_Access SMALLINT ,          
 @Validate_Party_cnt SMALLINT           
             
                      
  SELECT  @Validate_BG = ISNULL(Count(BG_Id),0)                     
   FROM Bank_Guarantee BG                  
   WHERE (Party_Cnt = @Party_Cnt   Or  Party_cnt=@Lead_Agent_Cnt)             
    AND BG_Id = @Bg_Id                  
            
          
SELECT @Validate_Product_Access=can_make_live_BankGuarantee FROM product WHERE product_id=@product_id               
          
SELECT @Validate_Party_cnt=COUNT(party_cnt) FROM party_agent WHERE party_cnt=@Party_Cnt          
          
if @Validate_Party_cnt =1           
BEGIN          
SELECT @Validate_Party_Agent=can_make_live_BankGuarantee FROM party_agent WHERE party_cnt=@Party_Cnt          
END          
ELSE          
BEGIN          
SELECT @Validate_Party_Agent=1          
END          
            
SELECT @Validate_User=can_make_live_BankGuarantee FROM user_Authorities WHERE user_id=@user          
                      
  IF @Validate_BG = 1                  
   SELECT                       
    @Validate_Bal =                   
     CASE                    
     WHEN (BG.available_bal > @total_inf_premium) THEN                  
     1                  
     ELSE                  
     0                   
     END,                   
          @Validate_Product_Id =                  
     CASE                    
     WHEN (BGPL.Product_ID IS NOT NULL) THEN                  
     1                   
     ELSE                  
     0                  
     END,                  
          @Validate_Branch_Id =                   
     CASE                    
     WHEN (BGBL.Source_Id IS NOT NULL) THEN                  
     1                   
     ELSE                  
     0                  
     END,                  
          @Validate_Cover_From =                   
     CASE                    
     WHEN (@cover_from_date >= BG.Issue_Date AND @cover_from_date <= BG.Expiry_Date) THEN                  
     1                   
     ELSE                  
     0                  
     END,                  
          @Validate_Currency =                   
     CASE                    
     WHEN (@Transaction_Curreny_Id = BG.BG_Currency_Id) THEN                  
     1                   
     ELSE                  
     0                  
     END,                  
          @Validate_Deleted =                   
     CASE                    
     WHEN (BG.Is_Deleted = 0) THEN                  
     1                   
     ELSE                  
     0                  
     END,                  
          @Validate_Status =                   
     CASE                    
     WHEN  (                  
     BG.BG_Status_Id = 2 AND                   
      (                  
       BG.Is_policy_lock = 1                   
       AND                   
       NOT EXISTS(Select bg_id from Insurance_File_BG_Link                  
          Where bg_id = BG.bg_id)                  
      )                  
     ) THEN                  
        1   
      When                  
        (BG.BG_Status_Id = 2  and  BG.Is_policy_lock = 0 )                  
               
      THEN     
       1              
     WHEN (BG.BG_Status_Id = 1) THEN                  
     1                   
     ELSE                  
     0                  
     END                  
   FROM Bank_Guarantee BG                  
                         INNER JOIN Party p                  
     ON p.party_cnt = BG.party_cnt                  
    LEFT JOIN BG_Product_Link BGPL        
     ON BGPL.Bg_Id = BG.Bg_Id                  
     AND BGPL.Product_id = @Product_Id                  
    LEFT JOIN BG_Branch_Link BGBL                  
     ON BGBL.Bg_Id = BG.Bg_Id                  
     AND BGBL.Source_id = @Source_Id                  
    INNER JOIN BG_Status BGS                  
     ON BGS.BG_Status_id = BG.BG_Status_id                  
   WHERE  BG.BG_Id = @BG_Id                  
                  
                  
 SELECT   @Validate_BG AS 'Validate_BG',                  
   @Validate_Bal AS 'Validate_Bal',                   
   @Validate_Product_Id AS 'Validate_Product',                   
   @Validate_Branch_Id AS 'Validate_Branch',                    
   @Validate_Cover_From AS 'Validate_Cover_From',                   
   @Validate_Currency AS 'Validate_Tran_Currency',                  
   @Validate_Deleted AS 'Validate_Deleted',                  
   @Validate_Status AS 'Validate_Status',             
   @Validate_Party_Agent AS 'Validate_Party_Agent',                  
   @Validate_User AS 'Validate_User',                  
   @Validate_Product_Access AS 'Validate_Product_Access'                
              
              
              
 SET NOCOUNT ON              
 END                  
              
GO
      
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  
