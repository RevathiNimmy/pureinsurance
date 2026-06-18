SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Party_Corporate_Client_add'
GO

CREATE PROCEDURE spe_Party_Corporate_Client_add  
    @party_cnt int,  
    @company_reg varchar(60),  
    @trading_since_date datetime,  
    @party_business_id varchar(70),  
    @location int,  
    @no_of_offices int,  
    @no_of_employees int,  
    @financial_year datetime,  
    @trade_code varchar(70),  
    @wage_roll numeric(19, 4),  
    @turnover numeric(19, 4),  
    @SIC_code_id int,  
    @salutation varchar(255),  
    @trade_id int,  
    @source VARCHAR(255),  
    @tpsind tinyint,  
    @empsind tinyint,  
    @tp_password VARCHAR(255),  
    @mailshot tinyint,
    @is_fee_client bit  
AS  
BEGIN  
INSERT INTO Party_Corporate_Client  
(  
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
    empsind,  
    tp_password,  
    mailshot,
    is_fee_client  
)  
VALUES  
(  
    @party_cnt,  
    @company_reg,  
    @trading_since_date,  
    @party_business_id,  
    @location,  
    @no_of_offices,  
    @no_of_employees,  
    @financial_year,  
    @trade_code,  
    @wage_roll,  
    @turnover,  
    @SIC_code_id,  
    @salutation,  
    @trade_id,  
    @source,  
    @tpsind,  
    @empsind,  
    @tp_password,  
    @mailshot,
    @is_fee_client  
)  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
