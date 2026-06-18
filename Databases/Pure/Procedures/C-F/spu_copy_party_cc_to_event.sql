SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_copy_party_cc_to_event'
GO

CREATE PROCEDURE spu_copy_party_cc_to_event  
    @event_cnt int,  
    @party_cnt int  
AS  
  
BEGIN  
  
INSERT INTO event_party_corporate_client (  
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
    vat_code,  
    SIC_code_id)  
select @event_cnt,  
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
    party.tax_number,  
    SIC_code_id  
from    party_corporate_client  

INNER JOIN Party ON 
	party_corporate_client.party_cnt = party.party_cnt

where   party_corporate_client.party_cnt = @party_cnt  
END  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
