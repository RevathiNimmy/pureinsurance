SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_update_PMSystem'
GO

CREATE PROCEDURE spu_update_PMSystem
	@system_id SMALLINT,
	@product_id SMALLINT,
	@system_name CHAR(40),
	@default_source_id INT,
	@home_country_id SMALLINT,
	@currency_id SMALLINT,
	@language_id SMALLINT,
	@licence_limit SMALLINT,
	@licence_key CHAR(30),
	@log_level SMALLINT,
	@pool_size SMALLINT
AS

UPDATE PMSystem
SET	product_id=@product_id,
	system_name=@system_name,
	default_source_id=@default_source_id,
	home_country_id=@home_country_id,
	currency_id=@currency_id,
	language_id=@language_id,
	licence_limit=@licence_limit,
	licence_key=@licence_key,
	log_level=@log_level,
	pool_size=@pool_size
WHERE system_id = @system_id

GO