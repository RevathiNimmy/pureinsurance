SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

DDLDropProcedure 'spu_PFEDI_GetAdditionalInformation'
GO

CREATE PROCEDURE spu_PFEDI_GetAdditionalInformation
(
    @pfrf_id INT,
    @party_cnt INT
)
AS

DECLARE @mnemonic AS VARCHAR(35)
DECLARE @depositpc AS NUMERIC(19,4)
DECLARE @title AS VARCHAR(20)
DECLARE @forenames AS VARCHAR(20)
DECLARE @surname AS VARCHAR(20)
DECLARE @dob AS DATETIME
DECLARE @companyreg AS VARCHAR(20)

SELECT
    @mnemonic=mnemonic,
    @depositpc=depositpc
FROM
    PFRF P
WHERE
    pfrf_id=@pfrf_id
    
SELECT
	@title=PPC.party_title_code,
	@forenames=PPC.forename,
	@surname=P.name,
	@dob=PL.date_of_birth
FROM
	Party P
INNER JOIN Party_Personal_Client PPC ON PPC.party_cnt=P.party_cnt
INNER JOIN Party_Lifestyle PL ON PL.party_cnt=P.party_cnt
WHERE
	P.party_cnt=@party_cnt

SELECT
	@companyreg=company_reg
FROM
	Party_Corporate_Client
WHERE
	party_cnt=@party_cnt
	
SELECT
    @mnemonic,
    @depositpc,
	@title,
	@forenames,
	@surname,
	@dob,
	@companyreg

GO