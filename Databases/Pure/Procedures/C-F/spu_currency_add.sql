SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_currency_add'
GO

CREATE PROCEDURE spu_currency_add  
    @currency_id smallint OUTPUT,  
    @caption_id int,  
    @iso_code char(4),  
    @description varchar(255),  
    @minor_part varchar(30),  
    @code char(10),  
    @symbol char(4),  
    @alignment char(1),  
    @decimal_places tinyint,  
    @is_deleted tinyint,  
    @effective_date datetime,  
    @format_string varchar(255),  
    @round_to_places tinyint,  
    @is_base bit,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null  
AS 
 
BEGIN

DECLARE @value int

IF @currency_id = 0 OR @currency_id IS NULL  
BEGIN  
	SELECT @currency_id = MAX(currency_id) + 1  
	FROM Currency  

	IF @currency_id IS NULL  
	BEGIN  
		SELECT @currency_id = 1  
	END  
END  
 

INSERT INTO Currency  
(  
    currency_id,  
    caption_id,  
    iso_code,  
    description,  
    minor_part,  
    code,  
    symbol,  
    alignment,  
    decimal_places,  
    is_deleted,  
    effective_date,  
    format_string,  
    round_to_places,  
    is_base,
	UserId,
	UniqueId,
	ScreenHierarchy  
)  
VALUES  
(  
    @currency_id,  
    @caption_id,  
    @iso_code,  
    @description,  
    @minor_part,  
    @code,  
    @symbol,  
    @alignment,  
    @decimal_places,  
    @is_deleted,  
    @effective_date,  
    @format_string,  
    @round_to_places,  
    @is_base,
	@user_id,
	@unique_id,
	@screen_hierarchy  
)  

-- There are 3 Type of Rates  
-- 1 = Single Ledger, One Rate For All Branches.  
-- 2 = Single Ledger, One Rate For Each Branch.  
-- 3 = Multi Ledger, One Rate For Each Branch.  
-- if rate type = 1 then automatically add the currency against head office / admin branch (company 1)
-- so it will be picked up in the maintain currency rates screen.

-- determine if this is type 2
Select @value = value from system_options Where option_number =154 and branch_id = 1
-- this isnt type 2
If ISNULL(@value,0) = 0
BEGIN
	-- determine if it is type 3
	SELECT @value = value from hidden_options where option_number = 16 and branch_id = 1  
	-- this isnt type 3
	If ISNULL(@value,0) = 0
	BEGIN	
		-- so this is type 1
		-- so automatically add currency to head office / admin branch
		Insert into companycurrency (currency_id, company_id) values (@currency_id, 1)
	END 
END 
END 


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
