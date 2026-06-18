SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Risk_group_add'
GO

CREATE PROCEDURE spe_Risk_group_add
    @risk_group_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime
AS
BEGIN
INSERT INTO Risk_group (
    risk_group_id ,
    caption_id ,
    code ,
    description ,
    is_deleted ,
    effective_date )
VALUES (
    @risk_group_id,
    @caption_id,
    @code,
    @description,
    @is_deleted,
    @effective_date)
END

GO

