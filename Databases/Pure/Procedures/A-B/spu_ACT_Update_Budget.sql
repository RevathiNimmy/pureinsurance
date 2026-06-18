SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Budget'
GO


CREATE PROCEDURE spu_ACT_Update_Budget
    @budget_id int,
    @budget_ref varchar(20),
    @period_id int,
    @budget_description varchar(255),
    @period_year_name varchar(20),
    @revises_budget_id int,
    @budget_status_id smallint
AS


BEGIN
UPDATE Budget
    SET
    budget_ref=@budget_ref,
    budget_description=@budget_description,
    period_id=@period_id,
    period_year_name=@period_year_name,
    revises_budget_id=@revises_budget_id,
    budget_status_id=@budget_status_id
WHERE budget_id = @budget_id
END
GO


