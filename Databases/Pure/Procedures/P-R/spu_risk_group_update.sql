SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_risk_group_update'
GO

CREATE PROCEDURE spu_risk_group_update
    @risk_group_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @gis_screen_id int,
    @abi_code varchar(2),
    @post_quote_gis_screen_id int,
    @FSA_Product_id int,
    @Midnight_Renewal tinyint,
    @Country_id int,
    @Brokerlink_Policy_Type_Id int
AS

/* Update the values */
UPDATE  risk_group
SET 
    [caption_id] = @caption_id,
    [code] = @code,
    [description] = @description,
    [is_deleted] = @is_deleted,
    [effective_date] = @effective_date,
    [gis_screen_id] = @gis_screen_id,
    [abi_code] = @abi_code,
    [post_quote_gis_screen_id] = @post_quote_gis_screen_id,
    [FSA_Product_id] = @FSA_Product_id,
    [Midnight_Renewal] = @Midnight_Renewal,
    [Country_id] = @Country_id,
    [Brokerlink_Policy_Type_Id] = @Brokerlink_Policy_Type_Id
WHERE
    [risk_group_id] = @risk_group_id
GO
