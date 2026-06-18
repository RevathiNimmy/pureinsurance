SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Risk_group_sel'
GO

CREATE PROCEDURE spu_Risk_group_sel
AS


SELECT
    [risk_group_id],
    [caption_id],
    [code],
    [description],
    [is_deleted],
    [effective_date],
    [gis_screen_id],
    [abi_code],
    [post_quote_gis_screen_id],
    [FSA_Product_id],
    [Midnight_Renewal],
    [Country_id],
    [Brokerlink_Policy_Type_Id]
FROM
    risk_group


GO