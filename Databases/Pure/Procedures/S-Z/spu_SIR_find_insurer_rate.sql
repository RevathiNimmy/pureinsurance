SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIR_find_insurer_rate'
GO


CREATE PROCEDURE spu_SIR_find_insurer_rate
    @party_cnt int = NULL,
    @scheme int = NULL,
    @risk_from varchar(255) = NULL,
    @risk_to varchar(255) = NULL
AS

/************************************************************************************************/
/*                                              */
/* Stored Procedure: sp_SIR_find_insurer_rate                           */
/*                                              */
/* Edit History: CTAF 12/07/99 - Created                            */
/*       CTAF 04/08/99 - Returns Party_Cnt as well now.                 */
/*       CTAF 06/08/99 - Returns risk_code now as well.                 */
/*       CTAF 10/08/99 - Fixed risk_from and risk_to to work properly.          */
/*                                              */
/************************************************************************************************/
BEGIN
    DECLARE @risk_from_id int,
        @risk_to_id int
    SELECT  @risk_from_id =
        (SELECT risk_code_id FROM risk_code WHERE code = @risk_from)
    SELECT  @risk_to_id =
        (SELECT risk_code_id FROM risk_code WHERE code = @risk_to)
    SELECT  p.name,
        ir.party_cnt,
        ir.Scheme,
        ir.risk_code_id,
        ir.effective_date,
        ir.rate1,
        ir.value1,
        ir.minimum_total1,
        ir.rate2,
        ir.value2,
        ir.minimum_total2,
        ir.rate3,
        ir.value3,
        ir.minimum_total3,
        rc.code
    FROM    Insurer_Rate ir, Party p, Risk_Code rc
    WHERE   (ir.party_cnt = @party_cnt OR @party_cnt IS NULL)
    AND (ir.Scheme = @scheme OR @scheme IS NULL)
    AND (ir.risk_code_id >= @risk_from_id OR @risk_from_id IS NULL)
    AND     (ir.risk_code_id <= @risk_to_id OR @risk_to_id IS NULL)
    AND     (p.party_cnt = ir.party_cnt)
    AND (ir.risk_code_id = rc.risk_code_id)
    ORDER BY p.name
END
GO


