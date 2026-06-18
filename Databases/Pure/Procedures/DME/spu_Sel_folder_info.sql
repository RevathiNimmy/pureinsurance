EXECUTE DDLDropProcedure 'spu_Sel_folder_info'

GO
CREATE PROCEDURE spu_Sel_folder_info
	@folderPath Varchar(255) OUTPUT,
    @folder_Num INT
AS  
BEGIN  
Declare 
@folderNum Int,
@parentNum Int

Select @folderPath = ''

Select @folderPath = RTrim(df.folder_name),
@folderNum = df.parent_num
	From DOC_document doc
		Inner Join DOC_page dp ON dp.doc_num = doc.doc_num
		Inner Join DOC_volume vol ON vol.volume_id = dp.volume_id
		Inner Join DOC_device dev ON dev.device_id = vol.device_id
		Inner Join DOC_folder df ON df.folder_num = doc.folder_num
	Where doc.folder_num = @folder_Num

While @folderNum <> 0
Begin
	Select @folderNum = parent_num,
		@folderPath = Rtrim(folder_name) + '/' +  @folderPath 
		From DOC_folder Where folder_num = @folderNum
End

END;
GO 
