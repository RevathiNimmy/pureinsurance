SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_fee_amounts_u_sel'
GO


CREATE PROCEDURE spu_fee_amounts_u_sel          
    @party_cnt int          
          
AS          
          
SELECT      
 fa.fee_amount_id,          
 fa.product_id,         
 fa.risk_type_group_id,         
 fa.peril_group_id,         
 p.description AS ProductDescription,          
 pg.description as PerilGroupDescription,        
 rtg.description as RiskGroupDescription,        
 fa.transaction_sub_type,          
 fa.fee_percentage,          
 fa.fee_amount,          
 fa.effective_date,          
 tg.description,       
 c.format_string,
 fa.include_fee_in_instalments,
fa.spread_fee_across_instalments
        
FROM Fee_amounts fa          
        
LEFT JOIN Product p ON         
 p.product_id = fa.product_id          
        
LEFT JOIN Risk_Type_Group rtg ON         
 rtg.risk_Type_Group_id = fa.risk_Type_group_id        
        
LEFT JOIN Peril_Group pg ON         
 pg.peril_group_id = fa.peril_group_id        
      
LEFT JOIN Currency c ON      
 c.currency_id = fa.currency_id      
      
LEFT JOIN Tax_Group tg ON      
 tg.tax_Group_id = fa.tax_group_id      
      
WHERE party_cnt = @party_cnt   
AND fa.is_deleted = 0         
        
    
  





GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
