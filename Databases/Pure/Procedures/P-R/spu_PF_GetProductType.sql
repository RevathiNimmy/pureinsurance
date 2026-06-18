SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PF_GetProductType'
GO

Create Procedure spu_PF_GetProductType  
@pfprem_finance_cnt int  
As  
select is_true_monthly_policy from insurance_file ifile  
inner join Product on ifile.product_id=product.product_id  
Where insurance_file_cnt = (Select min(insurance_file_cnt) from pfpremiumfinance  
    Where pfprem_finance_cnt = @pfprem_finance_cnt AND pfprem_finance_version=1)  

GO
