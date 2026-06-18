SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_claim_conviction_add'
GO


CREATE PROCEDURE spu_claim_conviction_add
    @PartyID int,
    @Code varchar(70),
    @ConvictionDate varchar(50),
    @Description varchar(70) = NULL,
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
INSERT INTO claim_conviction (
 party_claim_id ,
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
 driving_licence_penalty_pts )
VALUES (
 @PartyID,
 @Code,
 @ConvictionDate,
 @Description,
 @FineAmt,
 @SentenceCode,
 @SentenceDescription,
 @SentenceDuration,
 @SentenceDurationQualifier,
 @SentenceEffectiveDate,
 @StatusCode,
 @AlcoholLevel,
 @AlcoholMeasurementMethod,
 @DrivingLicencePenaltyPts)
END
GO


