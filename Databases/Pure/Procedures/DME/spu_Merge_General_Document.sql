SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_MERGE_GENERAL_DOCUMENT'
GO

CREATE PROCEDURE spu_Merge_General_Document
@NewFolderNum		int	,
@Parent_num		int
AS 
Declare @Parent2	int,
	@fOLDER_NAME	varchar(25),
	@Ex_code	varchar(20),
	@OldFolderNum    int,	
	@Oldparentnum	int
select @Folder_Name = Folder_name,@Ex_code = ex_code,@Parent2= Parent_num from doc_folder with(nolock) where folder_num = @Parent_num and Folder_Level = 1 

select @OldFolderNum = Folder_Num from doc_folder with(nolock) where parent_num in 
(select folder_num from doc_folder with(nolock) where ex_code = @Ex_code and folder_name = @fOLDER_NAME and folder_num != @Parent_num) and ex_code ='general' --and parent_num !=@Parent2

IF (ISNULL(@OldFolderNum, 0) = 0)
BEGIN
	RETURN
END

select @OldParentNum = Parent_Num from doc_Folder with(nolock) where Folder_num = @OldFolderNum

declare @general int
declare @generaln int

select @General = folder_num from doc_folder with(nolock) where parent_num = @OldFolderNum and Ex_Code = 'GENERAL'
select @GeneralN = folder_num from doc_folder with(nolock) where parent_num = @NewFolderNum and Ex_Code = 'GENERAL'

update doc_document set folder_num = @NewFolderNum where folder_num = @OldFolderNum
update doc_folder set parent_num = @GeneralN where parent_num = @General

delete doc_folder where folder_num = @General

IF NOT EXISTS (Select NULL from doc_folder with(nolock) where parent_num = @OldFolderNum)
BEGIN
	delete doc_folder where folder_num =@OldFolderNum 
	IF NOT EXISTS(Select NULL from doc_folder with(nolock) where parent_num =  @OldParentNum)
	BEGIN 
		delete doc_folder where folder_num =@OldParentNum 
	END
	ELSE
	BEGIN
		IF EXISTS(Select NULL From Doc_Folder where Parent_Num = @OldParentNum And (EX_Code ='COMPLAINTS' OR EX_Code = '') )
		BEGIN
			Update Doc_Folder Set Parent_Num = @Parent_num where Parent_Num = @OldParentNum And (EX_Code ='COMPLAINTS' OR EX_Code = '') 
		END
	End
	IF NOT EXISTS(Select NULL from doc_folder where parent_num =  @OldParentNum)
	BEGIN 
		DELETE doc_folder where folder_num =@OldParentNum 
	END
END

GO
