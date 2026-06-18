SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Risk_by_source_add'
GO


CREATE PROCEDURE spu_Risk_by_source_add
    @risk_group_id int,
    @source_id int,
    @commission_cnt int
AS


INSERT INTO risk_by_source (
    risk_group_id ,
    source_id ,
    commission_cnt )
VALUES (
    @risk_group_id ,
    @source_id ,
    @commission_cnt )
GO


