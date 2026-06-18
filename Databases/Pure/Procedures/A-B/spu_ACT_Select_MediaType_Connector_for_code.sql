EXEC DDLDropProcedure 'spu_ACT_Select_MediaType_Connector_for_code'
GO

CREATE PROCEDURE spu_ACT_Select_MediaType_Connector_for_code
	@mediatype_connector_code VARCHAR(10)
AS

	SELECT
		mediatype_connector_id,
		code,
		description,
		connector_address,
		connector_port,
		connector_timeout_seconds
	FROM
		MediaType_Connector
	WHERE
		code=@mediatype_connector_code

GO
		
		