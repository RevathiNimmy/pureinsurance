SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Event_Party_CC_sel'
GO


CREATE PROCEDURE spu_Event_Party_CC_sel
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
    vat_code,
    seasonal_gift_id,
    strength_code_id,
    SIC_code_id,
    previous_insurer_cnt,
    previous_broker_cnt
 FROM Event_Party_Corporate_Client
WHERE party_cnt = @party_cnt
GO


