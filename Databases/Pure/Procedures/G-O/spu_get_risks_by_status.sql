SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_risks_by_status'
GO


CREATE PROCEDURE spu_get_risks_by_status
    @insurance_file_cnt integer,
	@is_BackDatedMTA bit=0
AS

BEGIN

IF(@is_BackDatedMTA=1)
	SELECT  ISNULL(r.risk_status_id, 4),
            r.is_risk_selected,
            i.insurance_file_cnt,
            r.risk_cnt,
	    rs.code risk_status_code,
	    ifrl.status_flag,
		ISNULL(r.is_mandatory_risk, 0)
    FROM    insurance_file_risk_link ifrl,
            risk r,
            insurance_file i,
	    risk_status rs
    WHERE   i.Base_Insurance_File_Cnt = @insurance_file_cnt
    AND ifrl.risk_cnt = r.risk_cnt
    AND ifrl.insurance_file_cnt = i.insurance_file_cnt
    AND r.risk_status_id = rs.risk_status_id
    AND ifrl.status_flag <> 'D'
ELSE
    SELECT  ISNULL(r.risk_status_id, 4),
            r.is_risk_selected,
            i.insurance_file_cnt,
            r.risk_cnt,
	    rs.code risk_status_code,
	    ifrl.status_flag,
		ISNULL(r.is_mandatory_risk, 0)
    FROM    insurance_file_risk_link ifrl,
            risk r,
            insurance_file i,
	    risk_status rs
    WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt
    AND ifrl.risk_cnt = r.risk_cnt
    AND ifrl.insurance_file_cnt = i.insurance_file_cnt
    AND r.risk_status_id = rs.risk_status_id
    AND ifrl.status_flag <> 'D'  /* filter out the deleted Risks in Case of MTA */
--    AND ifrl.status_flag <> 'U'

END




GO
