SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_product_trade_rnl_online_option'
GO


CREATE PROCEDURE spu_get_product_trade_rnl_online_option
    @insurance_file_cnt int
AS 

BEGIN

--**********************************************************************************************
-- Author : Pankaj Kaushik
-- 
-- History: 21/02/2008  
--**********************************************************************************************
    SELECT  
   	 ifi.product_id,  
	 p.TradeRNLOnline,
	 p.true_monthly_policy_renewal_communication,  
	 ifi.anniversary_copy 
	 FROM insurance_file ifi  
	 LEFT JOIN Product p ON p.product_id = ifi.product_id  
	 WHERE ifi.insurance_file_cnt = @insurance_file_cnt  
END  
GO






