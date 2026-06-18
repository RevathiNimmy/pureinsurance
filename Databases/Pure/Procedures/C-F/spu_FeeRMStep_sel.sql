SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_FeeRMStep_sel'
GO


CREATE PROCEDURE spu_FeeRMStep_sel        
(        
	@Transaction_type_id INT,        
	@ProdType INT,        
	@insurance_file_cnt INT        
)        
        
AS      
	DECLARE @base_currency_id SMALLINT        
	DECLARE @currency_id SMALLINT        
	DECLARE @currency_base_xrate FLOAT        
	DECLARE @source_id INT        
	DECLARE @TypeOfRates TINYINT        
	DECLARE @effective_date DATETIME        
	DECLARE @effective_fee_date DATETIME        
	DECLARE @premium MONEY        
	DECLARE @currency_desc VARCHAR(255)    
	DECLARE @currency_isocode varchar(10)  
	DECLARE @transaction_sub_type_id int
 
 	/*Get policy details*/        
	SELECT        
		@effective_date = i.currency_base_date,        
		@effective_Fee_date = i.cover_start_date,        
		@base_currency_id = s.base_currency_id,        
		@currency_id = i.currency_id,        
		@currency_base_xrate = i.currency_base_xrate,        
		@source_id = i.source_id,        
		@premium = i.this_premium,        
		@currency_desc = c.description,   
		@currency_isocode = c.iso_code       
	FROM insurance_file i        
		JOIN source s        
			ON s.source_id = i.source_id        
		JOIN currency c        
			ON c.currency_id = i.currency_id        
	WHERE i.insurance_file_cnt = @insurance_file_cnt        


	--**********************************
	--**********************************

	-- set transaction sub_type so that by default it is ignored
	-- by the main select
	SET @transaction_sub_type_id = -1

	-- if this is an MTA with a positive premium 
	-- this is transaction_sub_type : Additional MTA
	IF @premium >= 0 AND @transaction_type_id = 9 
		SET @transaction_sub_type_id = 1

	-- if this is an MTA with a negative premium 
	-- this is transaction_sub_type : Return MTA
	IF @premium < 0 AND @transaction_type_id = 9
		SET @transaction_sub_type_id = 2

	--**********************************
	--**********************************

	-- if a valid effective date cannot be found then        
	-- use todays date        
	Select @effective_date = ISNULL(@effective_date, GetDAte())        
	              
	/*Get source_id where rates are stored*/        
	EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT        
        
	IF @TypeOfRates = 1        
	BEGIN        
		SELECT @source_id = 1        
	END    
      
	SELECT        
		p.resolved_name,        
		pr.product_id,        
		pr.description 'ProductDesc',        
		tt.transaction_type_id,        
		tt.description 'TransactionTypeDesc',        
		fa.fee_amount_id,        
		fa.party_cnt,        
		fa.fee_percentage        
		*        
		(        
		SELECT        
			CASE        
				WHEN @premium < 0 AND pe.fee_charge = 1 THEN -1        
				WHEN @premium < 0 AND pe.fee_charge = 0 THEN 1        
			ELSE 1        
		END        
		),        
		(        
		SELECT        
			CASE        
				WHEN fa.currency_id = @currency_id THEN fa.fee_amount        
				WHEN fa.currency_id = @base_currency_id THEN fa.fee_amount / ISNULL(@currency_base_xrate, CRFromBase.rate_against_base)        
				WHEN @currency_id = @base_currency_id THEN fa.fee_amount * CRToBase.rate_against_base        
			ELSE (fa.fee_amount * CRToBase.rate_against_base) / ISNULL(@currency_base_xrate, CRFromBase.rate_against_base)        
		END *        
			CASE        
				WHEN @premium < 0 AND pe.fee_charge = 1 THEN 1        
				WHEN @premium < 0 AND pe.fee_charge = 0 THEN -1        
			ELSE 1        
		END        
	FROM CurrencyRate CRToBase        
		JOIN CurrencyRate CRFromBase ON 
			CRFromBase.company_id = CRToBase.company_id        
	WHERE CRToBase.company_id = @source_id        
	AND CRToBase.currency_id = fa.currency_id        
	AND CRToBase.effective_from IN        
	(        
		SELECT MAX(effective_from)        
		FROM CurrencyRate        
		WHERE effective_from <= @effective_date        
		AND currency_id = CRToBase.currency_id        
		AND company_id = CRToBase.company_id        
	)        
	AND CRFromBase.currency_id = @currency_id        
	AND CRFromBase.effective_from IN        
	(        
		SELECT MAX(effective_from)        
		FROM CurrencyRate        
		WHERE effective_from <= @effective_date        
		AND currency_id = CRFromBase.currency_id        
		AND company_id = CRFromBase.company_id        
	)        
	) 'fee_amount',        
	fa.effective_date,        
	fa.tax_group_id,        
	pe.fee_charge,        
	@currency_id,        
	@currency_desc,   
	fa.peril_group_id,   
	pg.description peril_group,   
	fa.risk_group_id,   
	rg.description risk_group,   
	@currency_isocode iso_code, 
	@source_id

	FROM fee_amounts fa      
		LEFT JOIN Product pr ON   
			pr.product_id = fa.product_id        
	
		LEFT JOIN Risk_Group rg ON   
			rg.risk_group_id = fa.risk_group_id  

		LEFT JOIN Peril_Group pg ON  
			pg.peril_group_id = fa.peril_group_id  

		JOIN Transaction_Type tt ON 
			tt.transaction_type_id = fa.transaction_type_id        

		JOIN Party p ON       
			p.party_cnt = fa.party_cnt        

		JOIN Party_Extra pe ON       
			pe.party_cnt = p.party_cnt        

	WHERE  fa.transaction_type_id = @Transaction_Type_id
	AND pr.product_id = @ProdType        
	AND fa.effective_date <= CONVERT(DATETIME, @effective_fee_date, 102)   




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
