SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_ValidateRIModelUsage'
GO

CREATE PROCEDURE spu_SIR_ValidateRIModelUsage
@Risktype_id int,
@RIband_id int,
@RIModel_id int,
@effective_date datetime

AS

Select * From Risk_Type_RI_Model_Usage 
Where risk_type_id =@Risktype_id
And ri_band =@RIband_id
And ri_model_id=@RIModel_id
And effective_date=@effective_date 

GO