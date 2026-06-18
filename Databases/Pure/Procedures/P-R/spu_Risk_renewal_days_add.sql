SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Risk_renewal_days_add'
GO

CREATE PROCEDURE spu_Risk_renewal_days_add
    @risk_group_id int,
    @service_level_id int,
    @renewal_days int
AS
INSERT INTO risk_renewal_days (
    risk_group_id ,
    service_level_id ,
    renewal_days )
VALUES (
    @risk_group_id ,
    @service_level_id ,
    @renewal_days )
GO

