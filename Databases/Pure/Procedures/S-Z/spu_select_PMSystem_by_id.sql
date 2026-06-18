SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_select_PMSystem_by_id'
GO

CREATE PROCEDURE spu_select_PMSystem_by_id
	@message_id smallint
AS

SELECT
    system_id,
	product_id,
	system_name,
	default_source_id,
	home_country_id,
	currency_id,
	language_id,
	licence_limit,
	licence_key,
	log_level,
	pool_size,
	timestamp
FROM PMSystem
WHERE (system_id = @message_id OR @message_id = 0)

GO