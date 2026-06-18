SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_conviction_upd'
GO

CREATE PROCEDURE spe_party_conviction_upd
    @party_cnt int,
    @party_conviction_id int,
    @code varchar(70),
    @conviction_date varchar(40),
    @description varchar(70),
    @fine_amt numeric(19,4),
    @sentence_code varchar(70),
    @sentence_description varchar(70),
    @sentence_duration decimal(14,2),
    @sentence_duration_qualifier varchar(70),
    @sentence_effective_date varchar(40),
    @status_code varchar(70),
    @alcohol_level decimal(14,2),
    @alcohol_measurement_method varchar(70),
    @driving_licence_penalty_pts decimal(14,2),
	@UserId int = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS
BEGIN
UPDATE party_conviction
    SET
    code=@code,
    conviction_date=@conviction_date,
    description=@description,
    fine_amt=@fine_amt,
    sentence_code=@sentence_code,
    sentence_description=@sentence_description,
    sentence_duration=@sentence_duration,
    sentence_duration_qualifier=@sentence_duration_qualifier,
    sentence_effective_date=@sentence_effective_date,
    status_code=@status_code,
    alcohol_level=@alcohol_level,
    alcohol_measurement_method=@alcohol_measurement_method,
    driving_licence_penalty_pts=@driving_licence_penalty_pts,
	UserId = @UserId,
	UniqueId = @UniqueId,
	ScreenHierarchy = @ScreenHierarchy
WHERE party_cnt = @party_cnt AND party_conviction_id = @party_conviction_id
END

GO

