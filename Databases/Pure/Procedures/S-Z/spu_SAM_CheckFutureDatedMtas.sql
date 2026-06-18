SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_CheckFutureDatedMtas'
GO


CREATE PROCEDURE spu_SAM_CheckFutureDatedMtas
    @Insurance_Folder_Cnt integer,
    @Check_Date Datetime
AS
Begin
	select count(iff.insurance_file_cnt) from insurance_file iff
	join insurance_file_type ift on ift.insurance_file_type_id = iff.insurance_file_type_id
	where ift.code in ('MTA PERM', 'MTA TEMP') and iff.cover_Start_date > @Check_Date
	and iff.insurance_folder_cnt = @Insurance_Folder_Cnt
End
Go