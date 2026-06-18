SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_PartyBG_Details_Add'
GO 
CREATE PROCEDURE spu_PartyBG_Details_Add          
  @BG_id                                INT OUTPUT,          
  @bank_name_id                         INT,          
  @bank_branch                          VARCHAR(50),
  @Party_Cnt                            INT,
  @custody_branch_id                    INT,        
  @BG_ref                               VARCHAR(50),        
  @BG_currency_Id                       SMALLINT,          
  @BG_limit                             NUMERIC(13,2),        
  @issue_date                           DATETIME,        
  @available_bal                        NUMERIC(13,2),       
 --****************************************************************************************      
 --Note:Currency Conversion related codes are commented below since it may be used in future      
 --and it is told by Gaurav      
 --****************************************************************************************        
  --@exchange_rate_override_reason_id     INT,          
  --@base_currency_id                     SMALLINT,          
  --@base_currency_xrate                  FLOAT,          
  --@Account_currency_id                  SMALLINT,          
 -- @account_currency_xrate                   FLOAT,          
 -- @system_currency_xrate                    FLOAT,          
 -- @base_currency_date                   DATETIME,          
 -- @Account_currency_date                    DATETIME,          
 -- @system_currency_date                     DATETIME,          
 --@Effective_Date                       DATETIME,          
  @expiry_date                          DATETIME,          
  @is_policy_lock                       TINYINT,        
  @is_deleted                      TINYINT,        
 @bg_Status_Id                          INT        
             
          
AS          
BEGIN          
          
INSERT INTO bank_guarantee(          
  bank_name_id,          
  bank_branch,          
  Party_Cnt,          
  custody_branch_id,        
  BG_ref,          
 BG_currency_Id,          
  BG_limit,          
  issue_date,        
  available_bal,          
  --****************************************************************************************      
 --Note:Currency Conversion related codes are commented below since it may be used in future      
 --and it is told by Gaurav      
 --****************************************************************************************        
-- exchange_rate_override_reason_id,        
 --base_currency_id,          
 --base_currency_xrate,          
 --Account_currency_id,          
 --account_currency_xrate,          
 --system_currency_xrate,          
 --base_currency_date,          
 --Account_currency_date,          
 --system_currency_date,          
 --Effective_Date,          
  expiry_date,          
  is_policy_lock,          
  is_deleted,        
  bg_Status_Id         
 )          
          
VALUES (          
  @bank_name_id,          
  @bank_branch,          
  @Party_Cnt,          
  @custody_branch_id,        
  @BG_ref,      
  @BG_currency_Id,          
  @BG_limit,         
  @issue_date,        
  @available_bal,          
 --****************************************************************************************      
 --Note:Currency Conversion related codes are commented below since it may be used in future      
 --and it is told by Gaurav      
 --****************************************************************************************          
-- @exchange_rate_override_reason_id,        
 --@base_currency_id,                 
 --@base_currency_xrate,             
 --@Account_currency_id ,            
 --@account_currency_xrate,          
 --@system_currency_xrate ,          
 --@base_currency_date ,             
 --@Account_currency_date,           
 --@system_currency_date,          
 --@Effective_Date,          
  @expiry_date,          
  @is_policy_lock,          
 @is_deleted ,        
@bg_Status_Id        
 )          
SELECT @BG_id = @@Identity          
END          
          
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO                
