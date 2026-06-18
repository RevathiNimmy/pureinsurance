/* AK 08082002 - add Ledger data for each sub branch */

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tI_Sub_Branch'
go

create trigger tI_Sub_Branch on Sub_Branch for INSERT as
-- INSERT trigger on Sub_Branch
begin
    DECLARE @source_id int
    DECLARE @sub_branch_id int
    DECLARE @Value VARCHAR(20)
    DECLARE @period_id int

    /* DD 28/08/2002 */
    /* Get the Product Option for multi-tree accounting */
    SELECT
        @Value=Value
    FROM
        Hidden_options
    WHERE
        option_number=16

    /*
        Only duplicate the Ledgers when Multi-Tree Accounting is enabled
    */
    IF @Value = '1'
    BEGIN
        select @source_id = inserted.source_id, @sub_branch_id = inserted.sub_branch_id from inserted
        if @sub_branch_id > 1
        BEGIN
            /*
               DD 30/08/2002: Ensure that a set of periods exist and the new ledgers have the
               correct current period set
            */
            EXEC spu_ACT_Do_Period_GenerateDefaultYear @company_id=@source_id,
                @sub_branch_id=@sub_branch_id, @current_period_id=@period_id OUTPUT
            EXEC spe_Ledger_add @source_id, @sub_branch_id
            UPDATE Ledger SET current_period_id=@period_id WHERE sub_branch_id=@sub_branch_id
        END
    END

end

GO

