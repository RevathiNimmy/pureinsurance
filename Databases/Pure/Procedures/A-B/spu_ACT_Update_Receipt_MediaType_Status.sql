SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_ACT_Update_Receipt_MediaType_Status'
GO

--Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
CREATE  PROCEDURE spu_ACT_Update_Receipt_MediaType_Status
	@CashListItem_Id INT,
	@MediaType_Id INT,
	@MediaTypeStatus_Id INT,
	@Comments VARCHAR(255),
	@User_Id SMALLINT,
	@Date_Modified DATETIME,
	@Insurance_File_Cnt INT=NULL,
	@Document_Ref VARCHAR(25) = NULL
AS
BEGIN
	IF ISNULL(@Date_Modified,0)=0 
		SET @Date_Modified=GETDATE()

	UPDATE 
		CashListItem
	SET
		MediaType_Status_Id=@MediaTypeStatus_Id
	WHERE
		CashListItem_Id=@CashListItem_Id

	INSERT INTO
		Receipt_MediaType_Status_History(
						 CashListItem_Id,
						 Document_Ref,
						 Insurance_File_Cnt,
						 MediaType_Id,
						 MediaType_Status_Id,
						 Comments,
						 User_Id,
						 Date_Modified)
	VALUES(
		@CashListItem_Id,
		@Document_Ref,
		@Insurance_File_Cnt,
		@MediaType_Id,
		@MediaTypeStatus_Id,
		@Comments,
		@User_Id,
		@Date_Modified)
		 
END
--End - Sankar - (WPRvb64 Media Type Status) - Paralleling
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

