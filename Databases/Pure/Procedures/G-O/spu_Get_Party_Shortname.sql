
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_Party_Shortname'
GO

CREATE PROCEDURE spu_Get_Party_Shortname
    @partycnt INT,
	@insurance_file_cnt INT=0
AS  
  
BEGIN
IF (@insurance_file_cnt <> 0)    
	SELECT inf.insurance_ref,P.shortname FROM insurance_file inf
	INNER JOIN Party P ON inf.insured_cnt=P.party_cnt
	WHERE insurance_file_cnt=@insurance_file_cnt

ELSE
	SELECT shortname
    FROM  party
    WHERE party_cnt = @partycnt
END

GO
