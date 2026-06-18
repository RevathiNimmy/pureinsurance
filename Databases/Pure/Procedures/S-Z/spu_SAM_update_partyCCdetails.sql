SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_SAM_update_partyCCdetails'
GO

--Start (girija) - (UIIC WR27 - MTA Amend Client.doc)
CREATE PROCEDURE spu_SAM_update_partyCCdetails
	@party_cnt int,
	@CompanyReg varchar(60),
	@TradeCode varchar(70),
	@SICCode int,
	@TradingSince datetime,
	@WageRoll decimal,
	@TurnoverCode int,
	@FinancialYear datetime,
	@Salutation varchar(255),
	@TPS tinyint,
	@MPS tinyint,
	@eMPS tinyint,
	@Source varchar(255),
	@employeeband_id int=null,
	@no_of_offices int=null,
	@Company_Name varchar(255)=null 

AS
BEGIN
UPDATE Party_Corporate_Client
    SET
	company_reg=@CompanyReg ,
	trade_code=@TradeCode ,
	SIC_code_id=@SICCode ,
	trading_since_date=@TradingSince ,
	wage_roll=@WageRoll,
	turnover=@TurnoverCode,
	financial_year=@FinancialYear,
	salutation=@Salutation,
	tpsind=@TPS,
	mailshot=@MPS,
	empsind=@eMPS,
	source=@Source,
	no_of_employees=@employeeband_id,
	no_of_offices=@no_of_offices
WHERE 	party_cnt = @party_cnt

IF @Company_Name is not null
BEGIN
Update party  
 SET name = @Company_Name, 
 resolved_name = @Company_Name 
WHERE  party_cnt  = @party_cnt  
END 

END
--End (girija) - (UIIC WR27 - MTA Amend Client.doc)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

