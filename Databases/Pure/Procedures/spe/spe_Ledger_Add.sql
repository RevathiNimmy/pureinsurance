/* AK 080802 - add ledger for the company/branch and subbranch */
/* CMG/PB - If no ledger is found for the company and default subbranch then use 1 and 1*/
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Ledger_add'
GO

CREATE PROCEDURE spe_Ledger_add
    @source_id int = 0,
    @sub_branch_id int = 0
AS
BEGIN
    DECLARE @default_sub_branch_id INT

    If  @source_id = 0
        select @source_id = 1
    If  @sub_branch_id = 0
        select @sub_branch_id = 1
    PRINT @sub_branch_id
    PRINT @source_id
    IF @sub_branch_id>1
    BEGIN
        EXEC spu_sub_branch_default @source_id=@source_id, @sub_branch_id=@default_sub_branch_id OUTPUT

        INSERT into Ledger([ledgertype_id], [sequence], [company_id], [sub_branch_id], [ledger_name], [ledger_short_name], [current_period_id], [mapping_id], [is_deletable])
                SELECT ledgertype_id, sequence, @source_id, @sub_branch_id , ledger_name, ledger_short_name, NULL, mapping_id, is_deletable
                FROM Ledger
                WHERE company_id = @source_id and sub_branch_id = @default_sub_branch_id

	If @@ROWCOUNT=0
	BEGIN
        	INSERT into Ledger([ledgertype_id], [sequence], [company_id], [sub_branch_id], [ledger_name], [ledger_short_name], [current_period_id], [mapping_id], [is_deletable])
                	SELECT ledgertype_id, sequence, @source_id, @sub_branch_id , ledger_name, ledger_short_name, NULL, mapping_id, is_deletable
                	FROM Ledger
                	WHERE company_id = 1 and sub_branch_id = 1
	END
    END
END