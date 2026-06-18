SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_wp_CreditControlItem'
GO

CREATE PROCEDURE spu_wp_CreditControlItem  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
    DECLARE @HasOIP tinyint  
    DECLARE @OIPCount int  
  
    SET @OIPCount = 0  
    SET @HasOIP = 0  
   
    EXEC spu_wp_CreditControlOIP @HasOIP OUTPUT, @OIPCount OUTPUT, @InsuranceFileCnt  
  
    SELECT debtor_name = P.resolved_name,  
    debtor_address_line1 = a.address1,  
    debtor_address_line2 = a.address2,  
    debtor_address_line3 = a.address3,  
    debtor_address_line4 = a.address4,  
    debtor_post_code = a.postal_code,  
    debtor_country = c.description,  
    default_date = CCI.created_date,  
    amount_outstanding = CCI.amount,  
    due_date = CCI.due_date,  
    step_number = CCS.step_number,  
    will_auto_cancel = CCI.can_auto_cancel,  
    HasOIP = @HasOIP,  
    OIPCount = @OIPCount,
	credit_control_reason = CCI.credit_control_reason  
      
    FROM credit_control_item CCI,  
           account ACC,  
           party P,  
           party_address_usage PAU,  
           address_usage_type AUT,  
           address A,  
           country C,  
    	   credit_control_step CCS  

    WHERE P.party_cnt = pau.party_cnt  
       AND pau.address_cnt = a.address_cnt  
       AND pau.address_usage_type_id = aut.address_usage_type_id  
       AND aut.code = '3131 XCO'  
       AND a.country_id = c.country_id  
       AND P.party_cnt =  ACC.account_key  
       AND ACC.account_id = CCI.account_id  
       AND CCS.credit_control_step_id = CCI.credit_control_step_id  
       AND CCI.insurance_file_cnt = @InsuranceFileCnt  
       AND CCI.credit_control_item_id = @Instance2  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
