SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_risk_group_add'
GO

CREATE PROCEDURE spu_risk_group_add
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

/* Insert the values */
INSERT INTO risk_group (
    risk_group_id, caption_id, code, [description],
    is_deleted, effective_date, gis_screen_id, abi_code,
    post_quote_gis_screen_id,FSA_Product_id,Midnight_Renewal,Country_id,Brokerlink_Policy_Type_Id )
VALUES (
    @risk_group_id, @caption_id, @code, @description,
    @is_deleted, @effective_date, @gis_screen_id, @abi_code,
    @post_quote_gis_screen_id,@FSA_Product_id,@Midnight_Renewal,@Country_id,@Brokerlink_Policy_Type_Id )
GO


