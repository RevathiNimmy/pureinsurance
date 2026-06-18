SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Period'
GO


CREATE PROCEDURE spu_ACT_Add_Period
    @period_id int OUTPUT,
    @company_id int,
    @sub_branch_id int,
    @year_name varchar(20),
    @period_name varchar(15),
    @period_end_date datetime,
    @period_end_complete smallint
AS


BEGIN
INSERT INTO Period (
    company_id ,
    sub_branch_id ,
    year_name ,
    period_name ,
    period_end_date,
    period_end_complete )
VALUES (
    @company_id,
    @sub_branch_id,
    @year_name,
    @period_name,
    @period_end_date,
    @period_end_complete )
END
SELECT @period_id=@@IDENTITY
GO


