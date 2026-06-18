SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Ledger'
GO


CREATE PROCEDURE spu_ACT_Add_Ledger
    @ledger_id smallint OUTPUT,
    @sequence smallint,
    @company_id int,
    @sub_branch_id int,
    @ledger_name varchar(30),
    @ledger_short_name varchar(2),
    @mapping_id int,
    @ledgertype_id smallint,
    @current_period_id int,
    @is_deletable bit
AS

INSERT INTO Ledger (
    sequence ,
    company_id ,
    sub_branch_id ,
    ledger_name ,
    ledger_short_name ,
    mapping_id ,
    ledgertype_id ,
    current_period_id ,
    is_deletable )
VALUES (
    @sequence,
    @company_id,
    @sub_branch_id,
    @ledger_name,
    @ledger_short_name,
    @mapping_id,
    @ledgertype_id,
    @current_period_id,
    @is_deletable)

SELECT @ledger_id=@@IDENTITY

GO


