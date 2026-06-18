SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Budget'
GO


CREATE PROCEDURE spu_ACT_Add_Budget
    @budget_id int OUTPUT,
    @budget_ref varchar(20),
    @period_id int,
    @budget_description varchar(255),
    @period_year_name varchar(20),
    @revises_budget_id int,
    @budget_status_id smallint
AS

BEGIN
INSERT INTO Budget (
    budget_ref ,
    period_id ,
    budget_description ,
    period_year_name ,
    revises_budget_id ,
    budget_status_id )
VALUES (
    @budget_ref,
    @period_id,
    @budget_description,
    @period_year_name,
    @revises_budget_id,
    @budget_status_id)
END
SELECT @budget_id=@@IDENTITY
GO


