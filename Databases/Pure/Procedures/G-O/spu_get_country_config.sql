SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_country_config'
GO

CREATE PROCEDURE spu_get_country_config
AS

    SELECT  country_id,    
            iso_code,
            address_line_1_caption,
            address_line_2_caption,
            address_line_3_caption,
            address_line_4_caption,
            is_state_lookup,
            postcode_caption,
            postcode_visibility_id
    FROM    country

GO

