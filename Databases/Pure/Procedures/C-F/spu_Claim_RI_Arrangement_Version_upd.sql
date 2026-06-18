SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_Version_upd'
GO

CREATE Procedure spu_Claim_RI_Arrangement_Version_upd
    @claim_id int,
	@RI_Arrangement_id int
AS
	Update claim_ri_arrangement
	Set ri_arrangement_version = ri_arrangement_version + 1
    	Where   claim_id = @claim_id 
	And ri_arrangement_id = @RI_Arrangement_id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
