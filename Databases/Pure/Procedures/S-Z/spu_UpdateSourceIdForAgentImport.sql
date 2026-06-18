
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_UpdateSourceIdForAgentImport'
GO


Create Procedure spu_UpdateSourceIdForAgentImport
	@Source_ID int ,
	@Party_cnt bigint =NULL,
	@Address_cnt bigint =NULL,
	@Contact_cnt bigint =NULL
	
AS
	IF @party_cnt IS NOT NULL 
	BEGIN
		UPDATE party_agent_branch set Source_id=@Source_id where party_cnt=@Party_cnt
                UPDATE Party set Source_id=@Source_id where party_cnt=@Party_cnt
	END
	IF @Address_cnt IS NOT NULL 
	BEGIN
		update address set source_id=@source_id where address_cnt=@Address_cnt
	END
	IF @Contact_cnt IS NOT NULL 
	BEGIN
		update Contact set source_id=@source_id where contact_cnt=@contact_cnt
	END