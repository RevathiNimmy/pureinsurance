SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_conviction_sel'
GO

CREATE PROCEDURE spe_party_conviction_sel
    @party_cnt int,
    @party_conviction_id int,
	@UserId int = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL 
AS
SELECT
    party_cnt,
    party_conviction_id,
    code,
    conviction_date,
    description,
    fine_amt,
    sentence_code,
    sentence_description,
    sentence_duration,
    sentence_duration_qualifier,
    sentence_effective_date,
    status_code,
    alcohol_level,
    alcohol_measurement_method,
    driving_licence_penalty_pts
 FROM party_conviction
WHERE party_cnt = @party_cnt AND party_conviction_id = @party_conviction_id

GO

