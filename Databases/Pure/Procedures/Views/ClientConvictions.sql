set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClientConvictions'
go
create view ClientConvictions as select
    Party_Conviction.party_cnt                      ClientID,
    Party_Conviction.code                           Code,
    Party_Conviction.description                    Description,
    Party_Conviction.conviction_date                ConvictionDate,
    Party_Conviction.fine_amt                       FineAmount,
    Party_Conviction.sentence_code                  SentenceCode,
    Party_Conviction.sentence_description           SentenceDescription,
    Party_Conviction.sentence_duration              SentenceDuration,
    Party_Conviction.sentence_duration_qualifier    SentenceDurationQualifier,
    Party_Conviction.sentence_effective_date        SentenceEffectiveDate,
    Party_Conviction.status_code                    StatusCode,
    Party_Conviction.alcohol_level                  AlcoholLevel,
    Party_Conviction.alcohol_measurement_method     AlcoholMeasurementMethod,
    Party_Conviction.driving_licence_penalty_pts    DrivingLicencePenaltyPoints
    from Party_Conviction
go
