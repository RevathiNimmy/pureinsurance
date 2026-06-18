SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_partyid'
GO


CREATE PROCEDURE spu_get_partyid
AS


Select max(party_claim_id) from Party_Claim
GO


