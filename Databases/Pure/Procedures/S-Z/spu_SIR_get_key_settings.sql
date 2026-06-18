SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'Spu_sir_get_key_settings'
GO

CREATE PROCEDURE Spu_sir_get_key_settings
	@InputXML varchar(5000)
AS
BEGIN

DECLARE @Input XML = @InputXML;
WITH KeyNameValueCTE (KeyName, KeyValue)
AS
(
	SELECT
		KeyName = nodes.value('local-name(.)', 'varchar(30)') ,
		KeyValue = nodes.value('(.)[1]', 'varchar(255)')
	FROM
		@input.nodes('/start/*') AS Tbl(nodes)

)


SELECT KeyName,KeyValue from KeyNameValueCTE  where UPPER(RTRIM(LTRIM(KeyName))) = (
																					SELECT UPPER(RTRIM(LTRIM(name))) FROM PMNav_Key 
																					WHERE UPPER(RTRIM(LTRIM(name))) = UPPER(RTRIM(LTRIM(KeyName)))
																					AND ISNULL(is_deleted,0) = 0
																					AND effective_date < GETDATE()
																					AND ISNULL(Is_External_WorkItem,0) > 0
																				)



END
GO



