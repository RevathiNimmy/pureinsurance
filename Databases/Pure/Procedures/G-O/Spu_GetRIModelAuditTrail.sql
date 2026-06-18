SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'Spu_GetRIModelAuditTrail'
GO

Create Procedure Spu_GetRIModelAuditTrail
@ri_model_id int
As
Select date_amended,username from audit_ri_model
Inner join pmuser on audit_ri_model.pmuser_id=pmuser.user_id
Where ri_model_id=@ri_model_id 
Order by date_amended desc

Go