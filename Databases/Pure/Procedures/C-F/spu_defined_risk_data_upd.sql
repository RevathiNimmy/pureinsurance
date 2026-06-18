SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_defined_risk_data_upd'
GO


CREATE PROCEDURE spu_defined_risk_data_upd
    @defined_risk_data_id int,
    @source_id int,
    @risk_group_id int,
    @level_1 int,
    @level_2 int,
    @display_order int,
    @code varchar(10),
    @description varchar(255),
    @caption varchar(255),
    @type int
AS


BEGIN
UPDATE defined_risk_data
SET
    risk_group_id=@risk_group_id,
    source_id=@source_id,
    level_1=@level_1,
    level_2=@level_2,
    display_order=@display_order,
    code=@code,
    description=@description,
    caption=@caption,
    type=@type
WHERE defined_risk_data_id=@defined_risk_data_id
END
GO


