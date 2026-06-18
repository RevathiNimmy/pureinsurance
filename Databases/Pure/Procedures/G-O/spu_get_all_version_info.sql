SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_all_version_info'
GO


CREATE PROCEDURE spu_get_all_version_info

@insurance_folder_cnt int, 
@insurance_file_cnt int
AS

BEGIN
	Select ifi.insurance_file_cnt, ift.code From insurance_file ifi 
	    Inner join insurance_file_type ift On ift.insurance_file_type_id = ifi.insurance_file_type_id 
	    Where ifi.insurance_folder_cnt = @insurance_folder_cnt AND ifi.insurance_file_cnt <= @insurance_file_cnt
	    Order By ifi.insurance_file_cnt DESC
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


