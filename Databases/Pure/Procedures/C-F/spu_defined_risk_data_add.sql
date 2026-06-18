SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_defined_risk_data_add'
GO


CREATE PROCEDURE spu_defined_risk_data_add
    @defined_risk_data_id int OUTPUT,
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
INSERT INTO defined_risk_data (
    source_id,
    risk_group_id,
    level_1,
    level_2,
    display_order,
    code,
    description,
    caption,
    type)
VALUES (
    @source_id,
    @risk_group_id,
    @level_1,
    @level_2,
    @display_order,
    @code,
    @description,
    @caption,
    @type)
END
BEGIN
SELECT @defined_risk_data_id = @@IDENTITY
END
GO


