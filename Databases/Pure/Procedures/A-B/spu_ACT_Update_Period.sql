SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Period'
GO


CREATE PROCEDURE spu_ACT_Update_Period
    @period_id int,
    @company_id int,
    @sub_branch_id int,
    @year_name varchar(20),
    @period_name varchar(15),
    @period_end_date datetime,
    @period_end_complete smallint
AS


UPDATE Period SET
    company_id=@company_id,
    sub_branch_id=@sub_branch_id,
    year_name=@year_name,
    period_name=@period_name,
    period_end_date=@period_end_date,
    period_end_complete=@period_end_complete
WHERE period_id = @period_id

GO


