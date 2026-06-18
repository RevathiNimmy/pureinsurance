SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Rsk_Type_Exp_Ser_add'
GO


CREATE PROCEDURE spu_Rsk_Type_Exp_Ser_add
    @Rsk_Type_Exp_Ser_Id int OUTPUT,
    @Exp_Ser_Id int,
    @Risk_Type_id int,
    @Mode int
AS


BEGIN
if @Mode=0
INSERT INTO RISK_TYPE_EXPERT_SERVICE(Risk_type_id,
                    Expert_Service_Id)
SELECT Risk_code.risk_code_id, Expert_Service.Expert_Service_id
FROM Risk_code, Expert_Service
WHERE (Risk_code.risk_code_id = @Risk_Type_id ) AND
 (Expert_Service.Expert_Service_Id =@Exp_Ser_Id )
else if @Mode=1
INSERT INTO RISK_TYPE_EXPERT_SERVICE(Risk_type_id,
                    Expert_Service_Id)
SELECT Risk_Type.risk_Type_id, Expert_Service.Expert_Service_id
FROM Risk_Type, Expert_Service
WHERE (Risk_Type.risk_Type_id = @Risk_Type_id ) AND
 (Expert_Service.Expert_Service_Id =@Exp_Ser_Id )
END
BEGIN
SELECT @Rsk_Type_Exp_Ser_Id = @@IDENTITY
END
GO


