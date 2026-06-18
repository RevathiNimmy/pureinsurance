
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_State_Code_For_Party'
GO

CREATE PROCEDURE spu_Get_State_Code_For_Party
@Party_Cnt Int = 0,
@Claim_Id Int = 0 
AS

Declare @sStateCode varchar(10) 

set  @sStateCode = ''

IF @Party_Cnt <> 0 
BEGIN
	Select @sStateCode = ISNULL(Code, '')
	From State
	Inner JOIN Address ADDR On RTRIM(LTRIM(ADDR.address4)) = State.description
	INNER JOIN Party_Address_Usage  PAd On PAd.address_cnt = ADDR.address_cnt 
	Where PAd.party_cnt = @Party_Cnt 
END
ELSE
IF @Claim_Id <> 0 
BEGIN
	Select @sStateCode = ISNULL(Code, '')
	From State
	Inner JOIN Address ADDR On RTRIM(LTRIM(ADDR.address4)) = State.description
	INNER JOIN Party_Address_Usage  PAd On PAd.address_cnt = ADDR.address_cnt 
	INNER JOIN insurance_file InF ON InF.insured_cnt = PAd.party_cnt 
	INNER JOIN claim Clm on Clm.Policy_id = InF.insurance_file_cnt 
	Where Clm.Claim_id = @Claim_Id
END

SELECT @sStateCode

GO
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
  