SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_copy_insurance_file_agent'
GO


CREATE PROCEDURE spu_copy_insurance_file_agent
    @OldInsuranceFileCnt int,
    @NewInsuranceFileCnt int
AS

/*
    1.0 Created to copy agemt commission for renewal and MTA.       Tomo    16/08/01

*/
INSERT INTO insurance_file_agent (
        insurance_file_cnt,
        party_cnt,
        percentage,
        amount)
    SELECT  @NewInsuranceFileCnt,
        party_cnt,
        percentage,
        amount
    FROM    insurance_file_agent
    WHERE   insurance_file_cnt = @OldInsuranceFileCnt
GO


