SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_details_driver'
GO


CREATE PROCEDURE spu_get_details_driver
    @PartyID int
AS


Select
    claim_Conviction.Claim_Conviction_id,Claim_Conviction.conviction_date,Claim_Conviction.description
    from Claim_Conviction where Claim_Conviction.party_claim_id=@PartyID
GO


