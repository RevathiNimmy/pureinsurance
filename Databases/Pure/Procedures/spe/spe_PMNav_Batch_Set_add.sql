SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMNav_Batch_Set_add'
GO

CREATE PROCEDURE spe_PMNav_Batch_Set_add
    @pmnav_batch_set_id int OUTPUT ,
    @pmnav_batch_id int ,
    @created_by_id smallint ,
    @date_created datetime ,
    @started_date datetime ,
    @started_by_id smallint ,
    @completed_date datetime
AS
BEGIN
INSERT INTO PMNav_Batch_Set (
    pmnav_batch_id,
    created_by_id,
    date_created,
    started_date,
    started_by_id,
    completed_date)
VALUES (
    @pmnav_batch_id,
    @created_by_id,
    @date_created,
    @started_date,
    @started_by_id,
    @completed_date)
END
BEGIN
SELECT @pmnav_batch_set_id = @@IDENTITY
END

GO

