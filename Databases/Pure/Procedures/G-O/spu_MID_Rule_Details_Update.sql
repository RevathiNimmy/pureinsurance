SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_MID_Rule_Details_Update'
GO

CREATE PROCEDURE spu_MID_Rule_Details_Update
	@MID_Rule_id INT,
	@Code VARCHAR(10),
	@Description VARCHAR(255),
	@Effective_Date DATE,
	@Start_Date DATE,
	@Expiry_Date DATE = NULL,
	@MID_Type VARCHAR(20),
	@Supplier_Type_id INT,
	@Supplier_id INT,
	@Insurer_id INT,
	@Delegated_Authority_id INT = NULL,
	@Site_Number INT = NULL,
	@PMUser_Group_id INT,
	@PMwrk_Task_Group_id INT,
	@FileName VARCHAR(255),
	@Test_Indicator BIT,
	@File_Seq_Num_Start varchar(6)

AS

BEGIN

    IF ((SELECT CODE from Supplier_Type WHERE  Supplier_Type_id =@Supplier_Type_id) = 'INSURER')
    SELECT @Delegated_Authority_id=NULL

	UPDATE [MID_Rule]
	   SET [Description] = @Description,
		  [Effective_Date] = @Effective_Date,
		  [Start_Date] = @Start_Date,
		  [Expiry_Date] = @Expiry_Date,
		  [MID_Type] = @MID_Type,
		  [Supplier_Type_id] = @Supplier_Type_id,
		  [Supplier_id] = @Supplier_id,
		  [Insurer_id] = @Insurer_id,
		  [Delegated_Authority_id] = @Delegated_Authority_id,
		  [Site_Number] = @Site_Number,
		  [PMUser_Group_id] = @PMUser_Group_id,
		  [PMwrk_Task_Group_id] = @PMwrk_Task_Group_id,
		  [FileName] = @FileName,
		  [Test_Indicator] = @Test_Indicator,
		  [File_Seq_Num_Start] = @File_Seq_Num_Start
	 WHERE  MID_Rule_id = @MID_Rule_id

END

GO