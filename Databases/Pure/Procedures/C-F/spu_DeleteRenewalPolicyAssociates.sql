EXECUTE DDLDropProcedure 'spu_DeleteRenewalPolicyAssociates'
GO

CREATE PROCEDURE spu_DeleteRenewalPolicyAssociates
    @nInsuranceFileCnt INT
AS
BEGIN
   DELETE FROM Insurance_File_Associates
   WHERE Insurance_File_cnt = @nInsuranceFileCnt
End

GO
