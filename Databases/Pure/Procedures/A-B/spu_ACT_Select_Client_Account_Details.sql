SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Client_Account_Details'
GO
 
CREATE PROCEDURE spu_ACT_Select_Client_Account_Details  
    @account_key INT ,  
    @company_id INT,  
    @include_tax_on_ytd_turnover INT,  
    @include_tax_on_ytd_income INT  
AS  
  
DECLARE   
    @current_year VARCHAR(20),  
    @previous_year VARCHAR(20),  
    @year_to_date_turnover MONEY,  
    @last_year_turnover MONEY,  
    @client_balance MONEY,  
    @year_to_date_income MONEY,  
    @year_to_date_net_income MONEY,  
    @ledger_id INT,  
    @underwritingoragency VARCHAR(1),  
    @tax MONEY,  
    @Gross_Amt MONEY,  
    @SubAgent_Amt MONEY,  
    @ytd_turnover_inc_tax MONEY

SELECT @year_to_date_turnover = 0  
SELECT @last_year_turnover = 0  
SELECT @client_balance = 0  
SELECT @ytd_turnover_inc_tax =0
SELECT @year_to_date_income = 0
SELECT @year_to_date_net_income = 0
  
SELECT @year_to_date_income = 0  
SELECT @year_to_date_net_income = 0  
  
--Set company to 1 for period calculations, may need update for multi-branch  
SELECT @company_id=1  
  
SELECT   
    @current_year = year_name   
FROM period   
WHERE company_id = @company_id  
AND period_end_date =   
    (  
        SELECT   
            MIN(period_end_date)   
        FROM period  
        WHERE company_id = @company_id  
        AND period_end_complete = 0  
    )  
  
SELECT   
    @previous_year = year_name   
FROM period  
WHERE company_id = @company_id  
AND period_end_date =   
    (  
        SELECT   
            MAX(period_end_date)   
        FROM period  
        WHERE company_id = @company_id  
        AND period_end_date <  
            (  
                SELECT   
                    MIN(period_end_date)   
                FROM period  
                WHERE company_id = @company_id  
                AND year_name = @current_year  
            )  
    )  
IF EXISTS(SELECT 1 FROM account a INNER JOIN TransDetail td ON a.account_id = TD.account_id WHERE a.account_key = @account_key)
BEGIN 
          
SELECT   
    @last_year_turnover = ISNULL(SUM(td.account_amount), 0)   
FROM account a  
JOIN transdetail td   
    ON a.account_id = td.account_id  
JOIN document d   
    ON d.document_id = td.document_id  
    AND d.documenttype_id IN (4,5,15,16,17,18,30,35,36)  
JOIN period p   
    ON p.period_id = td.period_id  
WHERE a.account_key = @account_key
AND p.year_name = @previous_Year
AND p.company_id = @company_id  
  
SELECT   
    @client_balance = ISNULL(SUM(td.outstanding_account_amount), 0)   
FROM account a  
JOIN transdetail td 
    ON a.account_id = td.account_id  
WHERE a.account_key = @account_key  
AND td.postingstatus_id = 3  
  
                 
SELECT   
    @year_to_date_turnover = ISNULL(SUM(td.account_amount), 0)   
FROM account a  
JOIN transdetail td   
    ON a.account_id = td.account_id  
JOIN document d   
    ON d.document_id = td.document_id  
    AND D.documenttype_id IN (4,5,15,16,17,18,31,32,35,36)  
JOIN period p   
    ON p.period_id = td.period_id  
WHERE a.account_key = @account_key 
AND p.year_name = @current_year  
AND p.company_id = @company_id  
END
SELECT   
    @year_to_date_turnover,   
    @last_year_turnover,   
    @client_balance,   
    @year_to_date_income,   
    @year_to_date_net_income   
  
GO