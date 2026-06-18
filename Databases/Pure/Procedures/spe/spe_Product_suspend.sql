SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Product_suspend'
GO


--********************************************************************************************************************************  
--To Check if there is any entry in suspended_accounts_transactions corresponding to insurance file 
--*********************************************************************************************************************************  
CREATE PROCEDURE spe_Product_suspend  
    @product_id int  
AS  
  
SELECT  * from suspended_accounts_transactions sat
join insurance_file i 
on i.insurance_file_cnt=sat.insurance_file_cnt
where i.product_id=@product_id and sat.is_deleted=0

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
