SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Party_Corporate_Client_upd'
GO

CREATE PROCEDURE spe_Party_Corporate_Client_upd  
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
  
    UPDATE  
        Party_Corporate_Client  
    SET  
        company_reg = @company_reg,  
        trading_since_date = @trading_since_date,  
        party_business_id = @party_business_id,  
        location = @location,  
        no_of_offices = @no_of_offices,  
        no_of_employees = @no_of_employees,  
        financial_year = @financial_year,  
        trade_code = @trade_code,  
        wage_roll = @wage_roll,  
        turnover = @turnover,  
        SIC_code_id = @SIC_code_id,  
        salutation = @salutation,  
        trade_id = @trade_id,  
        source = @source,  
        tpsind = @tpsind,  
        empsind = @empsind,  
        tp_password = @tp_password,  
        mailshot = @mailshot,
        is_fee_client = @is_fee_client  
    WHERE  
        party_cnt = @party_cnt  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
