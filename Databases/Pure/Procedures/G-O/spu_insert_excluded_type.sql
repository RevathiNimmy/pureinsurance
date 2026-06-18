SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_insert_excluded_type'
GO


CREATE PROCEDURE spu_insert_excluded_type
    @unique_report_name varchar(300),
    @ExcludedType varchar(2),
    @ExcludedType_Id int

AS
BEGIN
        Insert into Temp_Report_Exclude(unique_report_name, type, id )
    values(@unique_report_name, @ExcludedType, @ExcludedType_Id)
End

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


