EXECUTE DDLDropProcedure spu_Update_DeferredRI_Renewal_Status
GO
CREATE PROCEDURE spu_Update_DeferredRI_Renewal_Status
    @Insurance_file_cnt INT  ,
    @new_Insurance_file_cnt INT
AS
	IF EXISTS(SELECT * FROM Renewal_Status WHERE insurance_file_cnt=@Insurance_file_cnt)
	UPDATE Renewal_Status SET insurance_file_cnt = @new_Insurance_file_cnt WHERE  insurance_file_cnt = @Insurance_file_cnt

   IF EXISTS(SELECT * FROM Renewal_Status WHERE renewal_insurance_file_cnt=@Insurance_file_cnt)
	UPDATE Renewal_Status SET renewal_insurance_file_cnt = @new_Insurance_file_cnt WHERE renewal_insurance_file_cnt=@Insurance_file_cnt