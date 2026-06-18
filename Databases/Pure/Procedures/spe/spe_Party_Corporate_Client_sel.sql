SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Party_Corporate_Client_sel'
GO

CREATE PROCEDURE spe_Party_Corporate_Client_sel  
    @party_cnt int  
AS  
SELECT  
    party_cnt,  
    company_reg,  
    trading_since_date,  
    party_business_id,  
    location,  
    no_of_offices,  
    no_of_employees,  
    financial_year,  
    trade_code,  
    wage_roll,  
    turnover,  
    SIC_code_id,  
    salutation,  
    trade_id,  
    source,  
    tpsind,  
    mailshot,  
    empsind,  
    tp_password,
    is_fee_client  
FROM  
    Party_Corporate_Client  
WHERE  
    party_cnt = @party_cnt  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
