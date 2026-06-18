
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SetRenewalStatusTypeID'
GO

CREATE PROCEDURE spu_SetRenewalStatusTypeID
	@RenewalInsuranceFileCnt INT,
	@RenewalStatusTypeID INT,
	@IsInvitePrinted INT
AS

UPDATE Renewal_Status
SET renewal_status_type_id = @RenewalStatusTypeID,
	is_invite_printed = @IsInvitePrinted
WHERE renewal_insurance_file_cnt = @RenewalInsuranceFileCnt
	
GO