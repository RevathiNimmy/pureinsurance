SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_TXN_is_party_fee_client'
GO

CREATE PROCEDURE spu_TXN_is_party_fee_client
(
	@insurance_file_cnt int
)

AS

DECLARE @is_fee_client bit
DECLARE @party_cnt int
DECLARE @party_type_code char(10)

SELECT
@party_cnt=IFL.insured_cnt,
@party_type_code=PT.code
FROM
event_insurance_file IFL
JOIN
party P ON IFL.insured_cnt=P.party_cnt
JOIN
party_type PT ON p.party_type_id=PT.party_type_id

IF @party_type_code='PC'
BEGIN
	SELECT @is_fee_client=ISNULL(is_fee_client,0) FROM party_personal_client WHERE party_cnt=@party_cnt
	RETURN @is_fee_client
END

IF @party_type_code='CC'
BEGIN
	SELECT @is_fee_client=ISNULL(is_fee_client,0) FROM party_corporate_client WHERE party_cnt=@party_cnt
	RETURN @is_fee_client
END

IF @party_type_code='GC'
BEGIN
	SELECT @is_fee_client=ISNULL(is_fee_client,0) FROM party_group_client WHERE party_cnt=@party_cnt
	RETURN @is_fee_client
END

RETURN 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

