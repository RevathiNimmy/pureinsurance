SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Adjust'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Adjust          
    @insurance_file_cnt int,          
    @discount_adjustment money      
AS          
          
-- ******************************************************************************************************          
-- Stored Procedure spu_SIR_Policy_Discount_Adjust          
-- ******************************************************************************************************          
-- Revision             Description of Modification                                     Date        Who          
-- --------             ---------------------------                                     ----        ---          
-- 1.0                  Created           02-12-2005  MEvans      
-- ******************************************************************************************************          
          
BEGIN          
          
    DECLARE @max_risk_cnt int          
    DECLARE @max_rating_section_id int          
          
    SELECT @max_risk_cnt = MAX(rsec.risk_cnt),          
           @max_rating_section_id = MAX(rsec.rating_section_id)          
    FROM risk rsk          
        JOIN rating_section rsec          
            	ON rsk.risk_cnt=rsec.risk_cnt          
        JOIN insurance_file_risk_link ifrl          
            	ON rsk.risk_cnt = ifrl.risk_cnt        
 	JOIN POLICY_DISCOUNT_VALID_RISK_STATUSES pdvrs  
     		ON pdvrs.risk_status_id = rsk.risk_status_id    
    WHERE ifrl.insurance_file_cnt=@insurance_file_cnt         
    AND rsk.is_risk_selected = 1       
          
    UPDATE rating_section          
        SET this_premium = this_premium + @discount_adjustment          
    WHERE rating_section_id = @max_rating_section_id          
    AND risk_cnt = @max_risk_cnt          

    UPDATE peril 
	SET this_premium = this_premium + @discount_adjustment
    WHERE  peril_id = 1
    AND rating_section_id =@max_rating_section_id
    AND risk_cnt = @max_risk_cnt
          
END          
        


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
