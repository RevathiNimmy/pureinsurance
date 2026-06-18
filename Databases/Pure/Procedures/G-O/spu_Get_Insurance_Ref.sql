SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_Insurance_Ref'
GO


CREATE PROCEDURE spu_Get_Insurance_Ref
    @Insurance_File_Cnt int
AS

/****** Stored Procedure to extract Insurance Ref for Insurance File_Cnt   ******/
/****** Created by  : Ajit Kumar                   ******/
/****** Date        : 10/01/2001                   ******/
select insurance_ref,insurance_file_type_id, insurance_folder_cnt from insurance_file where insurance_file_cnt = @insurance_file_cnt 
GO


