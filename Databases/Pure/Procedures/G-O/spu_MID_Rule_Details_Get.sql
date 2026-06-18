SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_MID_Rule_Details_Get'
GO

CREATE PROCEDURE spu_MID_Rule_Details_Get
AS

BEGIN
	SELECT	MID_Rule_id, R.Code, R.Description, R.Effective_Date, Start_Date, Expiry_Date,
			MID_Type, R.Supplier_Type_id, Supplier_id, Insurer_id, Delegated_Authority_id,
			Site_Number, R.PMUser_Group_id, R.PMwrk_Task_Group_id, FileName,
			Test_Indicator, File_Seq_Num_Start, Current_File_Seq_Num, R.Source_ID,
			S.Code SupplierCode, TG.description TaskGroupCode, UG.description UserGroupCode, R.is_deleted
	FROM MID_Rule R
		Inner join Supplier_Type S on R.Supplier_Type_id = S.Supplier_Type_id
		LEFT JOIN PMWrk_Task_Group TG ON R.PMwrk_Task_Group_id  = TG.pmwrk_task_group_id
		LEFT JOIN PMUser_Group UG ON R.PMUser_Group_id = UG.PMUser_Group_id
	ORDER BY R.Code

END

GO