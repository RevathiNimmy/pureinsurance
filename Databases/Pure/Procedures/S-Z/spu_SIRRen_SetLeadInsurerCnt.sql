SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_SetLeadInsurerCnt'
GO


CREATE PROCEDURE spu_SIRRen_SetLeadInsurerCnt
    @gis_scheme_id int,
    @insurance_file_cnt int
AS

/* Get party_cnt for lead_insurer_cnt based on gis_scheme_id */
/* History : SSL 09/08/2001 - Created */
BEGIN
    DECLARE @party_cnt int

    -- Get Party_cnt to be assinged to lead_insurer_cnt of insurance_file
    SELECT @party_cnt = party_cnt
    FROM party
    WHERE abi_code_on_81 = (SELECT abi_81_insurer FROM gis_insurer
    WHERE gis_insurer_id = (SELECT gis_insurer_id FROM gis_scheme
    WHERE gis_scheme_id = @gis_scheme_id))

    -- Assign @party_cnt to lead_insurer_cnt of insurance_file
    UPDATE insurance_file SET lead_insurer_cnt = @party_cnt
    WHERE insurance_file_cnt = @insurance_file_cnt
END
GO


