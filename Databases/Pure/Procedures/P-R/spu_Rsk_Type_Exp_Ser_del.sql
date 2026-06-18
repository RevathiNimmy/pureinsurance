SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Rsk_Type_Exp_Ser_del'
GO


CREATE PROCEDURE spu_Rsk_Type_Exp_Ser_del
    @Exp_Ser_Id int,
    @Rsk_Type_Id int
AS


DELETE FROM RISK_TYPE_EXPERT_SERVICE WHERE (risk_type_id = @Rsk_Type_Id ) AND
 (Expert_Service_Id =@Exp_Ser_Id )
GO


