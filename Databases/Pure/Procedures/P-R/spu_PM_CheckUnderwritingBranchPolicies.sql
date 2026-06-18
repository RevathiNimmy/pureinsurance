SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_PM_CheckUnderwritingBranchPolicies'
GO


CREATE PROCEDURE spu_PM_CheckUnderwritingBranchPolicies
    @source_id integer
AS

SELECT insurance_file_cnt
    FROM insurance_file
    WHERE source_id = @source_id
    AND alternate_reference IS NOT NULL
GO


