SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_check_COB_Rating_Section_Usage'
GO


CREATE PROCEDURE spu_check_COB_Rating_Section_Usage 
@risk_code_id int,
@COB_rating_section_id int,
@section_used tinyint OUTPUT
AS    
    
BEGIN    
DECLARE @insurance_file_cnt int
SELECT @insurance_file_cnt = 0

SELECT  @insurance_file_cnt = (SELECT ISNULL(MAX(ICS.insurance_file_cnt), 0)
				FROM Insurance_COB_section ICS
				JOIN Insurance_File I
				ON I.insurance_File_Cnt = ICS.Insurance_File_Cnt
				WHERE ICS.COB_rating_section_id = @COB_rating_section_id
				AND I.risk_code_id = @risk_code_id)
END
IF @Insurance_file_cnt = 0
	SELECT @section_used = 0
ELSE
	SELECT @section_used = 1

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

 