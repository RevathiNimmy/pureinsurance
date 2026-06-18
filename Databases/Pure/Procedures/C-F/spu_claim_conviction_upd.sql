SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_claim_conviction_upd'
GO


CREATE PROCEDURE spu_claim_conviction_upd
    @PartyID int,
    @ClaimConvictionID int,
    @Code varchar(70),
    @ConvictionDate varchar(20),
    @Description varchar(70),
    @FineAmt numeric,
    @SentenceCode varchar(70),
    @SentenceDescription varchar(70),
    @SentenceDuration numeric,
    @SentenceDurationQualifier varchar(70),
    @SentenceEffectiveDate varchar(20),
    @StatusCode varchar(70),
    @AlcoholLevel numeric,
    @AlcoholMeasurementMethod varchar(70),
    @DrivingLicencePenaltyPts numeric
AS


BEGIN
UPDATE claim_conviction
 SET
 code=@Code,
 conviction_date=@ConvictionDate,
 description=@Description,
 fine_amt=@FineAmt,
 sentence_code=@SentenceCode,
 sentence_description=@SentenceDescription,
 sentence_duration=@SentenceDuration,
 sentence_duration_qualifier=@SentenceDurationQualifier,
 sentence_effective_date=@SentenceEffectiveDate,
 status_code=@StatusCode,
 alcohol_level=@AlcoholLevel,
 alcohol_measurement_method=@AlcoholMeasurementMethod,
 driving_licence_penalty_pts=@DrivingLicencePenaltyPts
WHERE claim_conviction_id = @ClaimConvictionID
END
GO


