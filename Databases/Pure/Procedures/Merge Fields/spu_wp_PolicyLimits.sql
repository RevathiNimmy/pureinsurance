SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PolicyLimits'
GO


CREATE PROCEDURE spu_wp_PolicyLimits
    @PartyCnt Int,
    @InsuranceFileCnt Int,
    @RiskId Int,
    @ClaimCnt Int,
    @DocumentRef Varchar(25),
    @Instance1 Int,
    @Instance2 Int,
    @Instance3 Int
AS

Select Pl.Description EventDesc
		From Insurance_File iFile
	Inner Join Policy_Limits pl ON iFile.Policy_Limits_id=pl.Policy_Limits_Id
	Where iFile.Insurance_file_Cnt=@InsuranceFileCnt
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
