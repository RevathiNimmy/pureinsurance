SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_policy_shared_premiums_fetch'
GO

CREATE PROCEDURE spu_TXN_policy_shared_premiums_fetch
(
@from_event bit,
@insurance_file_cnt int
)

AS

DECLARE @tax_value numeric(19,4)

IF @from_event=0
BEGIN
	SELECT @tax_value=SUM(value) FROM tax_calculation
	WHERE
	insurance_file_cnt=@insurance_file_cnt AND
	transtype='TTIF'
	
	SELECT
	PCS.party_cnt,
	P.shortname,
	PCS.split_percentage/100,
	PCS.split_value,
	ROUND(PCS.split_percentage*@tax_value/100, 4) AS split_tax,
	PCS.split_value - (ROUND(PCS.split_percentage*@tax_value/100, 4)) AS split_exc_tax
	FROM
	policy_shared_premiums PCS
	INNER JOIN party P ON P.party_cnt=PCS.party_cnt
	WHERE
	PCS.insurance_file_cnt=@insurance_file_cnt
END
ELSE
BEGIN
	SELECT @tax_value=SUM(value) FROM event_tax_calculation
	WHERE
	insurance_file_cnt=@insurance_file_cnt AND
	transtype='TTIF'
	
	SELECT
	PCS.party_cnt,
	P.shortname,
	PCS.split_percentage/100,
	PCS.split_value,
	ROUND(PCS.split_percentage*@tax_value/100, 4) AS split_tax,
	PCS.split_value - (ROUND(PCS.split_percentage*@tax_value/100, 4)) AS split_exc_tax
	FROM
	event_policy_shared_premiums PCS
	INNER JOIN party P ON P.party_cnt=PCS.party_cnt
    WHERE PCS.insurance_file_cnt=@insurance_file_cnt
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

