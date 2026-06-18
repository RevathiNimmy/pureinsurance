SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_party_corporate_client'
GO

CREATE PROCEDURE spu_wp_party_corporate_client  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @ClaimCnt INT,  
    @company_reg VARCHAR(60) OUTPUT,  
    @trading_since_date DATETIME OUTPUT,  
    @party_business VARCHAR(255) OUTPUT,  
    @location INT OUTPUT,  
    @no_of_offices INT OUTPUT,  
    @no_of_employees VARCHAR(255) OUTPUT,  
    @financial_year DATETIME OUTPUT,  
    @trade_code VARCHAR(70) OUTPUT,  
    @wage_roll NUMERIC(19,4) OUTPUT,  
    @turnover VARCHAR(255) OUTPUT,  
    @SIC_code VARCHAR(255) OUTPUT,  
    @salutation VARCHAR(255) OUTPUT,  
    @contact VARCHAR(255) OUTPUT  
AS  
  
DECLARE @AONPRFormat VARCHAR(20)  
DECLARE @trade_from_lookup VARCHAR(20)


SELECT 
    @AONPRFormat = ISNULL(MAX(value), '0')
FROM hidden_options
WHERE option_number = 59
AND branch_id = 1

SELECT 
    @trade_from_lookup = ISNULL(MAX(value), '0') 
FROM system_options
WHERE option_number = 91
AND branch_id = 1  

SELECT  
    @company_reg = pcc.company_reg,  
    @trading_since_date = pcc.trading_since_date,  
    @party_business = pcc.party_business_id,  
    @location = pcc.location,  
    @no_of_offices = pcc.no_of_offices,  
    @no_of_employees = (SELECT CASE @AONPRFormat WHEN '1' THEN CAST(pcc.no_of_employees AS VARCHAR(255)) ELSE eb.description END),  
    @financial_year = pcc.financial_year,  
    @trade_code = (SELECT CASE @trade_from_lookup WHEN '1' THEN ISNULL(t.description, '(Not Known)') ELSE pcc.trade_code END),  
    @wage_roll = pcc.wage_roll,  
    @turnover = (SELECT CASE @AONPRFormat WHEN '1' THEN CAST(pcc.turnover AS VARCHAR(255)) ELSE tb.description END),  
    @SIC_code = sc.description,  
    @salutation = pcc.salutation  
FROM party_corporate_client pcc  
LEFT JOIN SIC_code sc  
    ON sc.SIC_code_id = pcc.SIC_code_id
LEFT JOIN turnoverband tb  
    ON tb.turnoverband_id = pcc.turnover  
LEFT JOIN employeeband eb  
    ON eb.employeeband_id = pcc.no_of_employees  
LEFT JOIN trade t
    ON t.trade_id = pcc.trade_id
WHERE pcc.party_cnt = @PartyCnt  

SELECT @contact = c.Description  
FROM contact c  
JOIN party_contact_usage pcu  
 ON pcu.contact_cnt = c.contact_cnt  
JOIN contact_type ct  
 ON ct.contact_type_id = c.contact_type_id  
WHERE pcu.party_cnt = @PartyCnt  
AND ct.code = 'MAIN'  


GO
