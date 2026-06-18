SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetHiddenOption'
GO


CREATE PROCEDURE spu_GetHiddenOption
	@BranchID smallint,
	@OptionNo int

 AS

SELECT value FROM Hidden_Options WHERE branch_id = @BranchID AND option_number = @OptionNo
GO

