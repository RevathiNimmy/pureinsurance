SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_claim_party_link_dar'
GO


CREATE PROCEDURE spu_claim_party_link_dar
    @claim_id int,
    @code varchar(20),
    @risk_type_id int,
    @peril_type_id int    
AS

IF @code=''
BEGIN
	IF @peril_type_id > 0
	BEGIN
		DELETE
		FROM    claim_party_link
		WHERE   claim_id = @claim_id
		AND peril_type_id=@peril_type_id
	END
	ELSE
	BEGIN
		DELETE
		FROM    claim_party_link
		WHERE   claim_id = @claim_id
		AND risk_type_id=@risk_type_id
	END
END
ELSE
BEGIN
	IF @peril_type_id > 0
	BEGIN
		DELETE
		FROM    claim_party_link
		WHERE   claim_id = @claim_id
		AND peril_type_id=@peril_type_id
		AND party_cnt IN (
			SELECT  p.party_cnt
			FROM    party p,
			party_type pt
			WHERE   p.party_type_id = pt.party_type_id
			AND pt.code = @code)
	END
	ElSE
	BEGIN
		DELETE
		FROM    claim_party_link
		WHERE   claim_id = @claim_id
		AND risk_type_id=@risk_type_id
		AND party_cnt IN (
			SELECT  p.party_cnt
			FROM    party p,
			party_type pt
			WHERE   p.party_type_id = pt.party_type_id
			AND pt.code = @code)
	END
END
GO


