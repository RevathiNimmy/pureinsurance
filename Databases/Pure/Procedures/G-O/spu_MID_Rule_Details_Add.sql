SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_MID_Rule_Details_Add'
GO

CREATE PROCEDURE spu_MID_Rule_Details_Add
	@SourceID INT,
	@Code VARCHAR(10),
	@Description varchar(255),
	@Effective_Date DATE,
	@Start_Date date,
	@Expiry_Date date = NULL,
	@MID_Type VARCHAR(20),
	@Supplier_Type_id int,
	@Supplier_id INT,
	@Insurer_id INT,
	@Delegated_Authority_id INT = NULL,
	@Site_Number INT = NULL,
	@PMUser_Group_id INT,
	@PMwrk_Task_Group_id INT,
	@FileName VARCHAR(255),
	@Test_Indicator BIT,
	@File_Seq_Num_Start varchar(6),
	@Current_File_Seq_Num varchar(6)
AS

BEGIN

	INSERT INTO [MID_Rule]
			   ([Code],
			   [Description],
			   [Effective_Date],
			   [Start_Date],
			   [Expiry_Date],
			   [MID_Type],
			   [Supplier_Type_id],
			   [Supplier_id],
			   [Insurer_id],
			   [Delegated_Authority_id],
			   [Site_Number],
			   [PMUser_Group_id],
			   [PMwrk_Task_Group_id],
			   [FileName],
			   [Test_Indicator],
			   [File_Seq_Num_Start],
			   [Current_File_Seq_Num],
			   [is_Deleted],
			   Source_ID)
		 VALUES
			   (@Code,
			   @Description,
			   @Effective_Date,
			   @Start_Date,
			   @Expiry_Date,
			   @MID_Type,
			   @Supplier_Type_id,
			   @Supplier_id,
			   @Insurer_id,
			   @Delegated_Authority_id,
			   @Site_Number,
			   @PMUser_Group_id,
			   @PMwrk_Task_Group_id,
			   @FileName,
			   @Test_Indicator,
			   RIGHT(RTRIM('000000'+ CAST(ISNULL(@File_Seq_Num_Start,'') AS VARCHAR)),6),
			   RIGHT(RTRIM('000000'+ CAST(ISNULL(@Current_File_Seq_Num,'') AS VARCHAR)),6),
			   0,
			   @SourceID)

END

GO