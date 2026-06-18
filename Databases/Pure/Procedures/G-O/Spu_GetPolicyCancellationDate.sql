SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'Spu_GetPolicyCancellationDate'
GO

CREATE PROCEDURE Spu_GetPolicyCancellationDate  
@InsFolderCnt int  
As  

	Select Top 1 inf.cover_start_date,inf.insurance_file_cnt 
from insurance_file inf with(nolock) inner join insurance_file_system ifs with(nolock) 
on inf.insurance_file_cnt = ifs.insurance_file_cnt
where inf.insurance_file_type_id=8  And ISNULL(ifs.last_trans_type_id, 0) <> 22
And inf.Insurance_folder_cnt=@InsFolderCnt And lapsed_date is not null  order by inf.cover_start_date desc
GO

