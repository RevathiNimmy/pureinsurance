SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_conviction_add'
GO

CREATE PROCEDURE spe_party_conviction_add
    @party_cnt int ,
    @party_conviction_id int OUTPUT ,
    @code varchar(70) ,
    @conviction_date varchar(40) ,
    @description varchar(70) ,
    @fine_amt numeric(19,4) ,
    @sentence_code varchar(70) ,
    @sentence_description varchar(70) ,
    @sentence_duration decimal(14,2) ,
    @sentence_duration_qualifier varchar(70) ,
    @sentence_effective_date varchar(40) ,
    @status_code varchar(70) ,
    @alcohol_level decimal(14,2) ,
    @alcohol_measurement_method varchar(70) ,
    @driving_licence_penalty_pts decimal(14,2),
	@UserId int = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS
BEGIN
IF @party_conviction_id = 0
                SELECT @party_conviction_id = NULL
IF @party_conviction_id = NULL
                SELECT @party_conviction_id = MAX(party_conviction_id) + 1
    FROM party_conviction
                WHERE party_cnt = @party_cnt

DECLARE @SentenceEffectiveDateFinal varchar(30);

SET @SentenceEffectiveDateFinal =
CASE
    WHEN @sentence_effective_date IS NULL THEN NULL
    WHEN LTRIM(RTRIM(@sentence_effective_date)) IN ('12:00:00 AM','12:00 AM','00:00:00','00:00:00.000')
        THEN CONVERT(varchar(10), GETDATE(), 101)   
    ELSE
        @sentence_effective_date
END;

IF @party_conviction_id = NULL
    SELECT @party_conviction_id = 1
INSERT INTO party_conviction (
    party_cnt ,
    party_conviction_id ,
    code ,
    conviction_date ,
    description ,
    fine_amt ,
    sentence_code ,
    sentence_description ,
    sentence_duration ,
    sentence_duration_qualifier ,
    sentence_effective_date ,
    status_code ,
    alcohol_level ,
    alcohol_measurement_method ,
    driving_licence_penalty_pts,
	UserId,
	UniqueId,
	ScreenHierarchy )
VALUES (
    @party_cnt,
    @party_conviction_id,
    @code,
    @conviction_date,
    @description,
    @fine_amt,
    @sentence_code,
    @sentence_description,
    @sentence_duration,
    @sentence_duration_qualifier,
    @SentenceEffectiveDateFinal,
    @status_code,
    @alcohol_level,
    @alcohol_measurement_method,
    @driving_licence_penalty_pts,
	@UserId,
	@UniqueId,
	@ScreenHierarchy)
END

GO

