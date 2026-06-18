SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spe_BackdatedMTAs_Allowed'
GO

Create procedure spe_BackdatedMTAs_Allowed 
@insurance_file_cnt int
AS  
BEGIN 
select p.allow_backdated_mtas from insurance_file i join  Product p on p.product_id=i.product_id where i.insurance_file_cnt=@insurance_file_cnt
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

