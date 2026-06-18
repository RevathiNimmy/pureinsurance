SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_ACT_Select_Trans_For_YearEnd'
GO

CREATE PROCEDURE spu_ACT_Select_Trans_For_YearEnd  
    @period_id INT,      
    @account_id INT,
    @company_id INT
AS      
        
SELECT  
    td.transdetail_id,      
	td.outstanding_amount,
	td.outstanding_currency_amount
FROM transdetail td
JOIN account a
    ON a.account_id = td.account_id
JOIN period p
    ON p.period_id = td.period_id
WHERE td.account_id = @account_id
AND td.company_id = @company_id
AND td.Document_id IS NOT NULL 
AND p.period_end_date <= 
    (
        SELECT 
            period_end_date 
        FROM period 
        WHERE period_id = @period_id
    )
AND td.outstanding_amount <> 0
ORDER BY td.transdetail_id    


GO


