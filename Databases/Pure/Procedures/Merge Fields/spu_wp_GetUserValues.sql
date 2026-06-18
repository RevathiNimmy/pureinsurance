if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_wp_GetUserValues]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_wp_GetUserValues]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [spu_wp_GetUserValues] 
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskCnt INT= 0,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT,
	@RiskId INT =0
AS
SELECT U.username,U.email_address,U.initials,U.full_name,U.title,U.telephone_number,U.mobile_number,U.extension_number,U.fax_number,J.description 'job_title'
FROM PMUser U
LEFT OUTER JOIN  Job_Title J on U.job_title_id = J.job_title_id 
WHERE [user_id] = @Instance1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

