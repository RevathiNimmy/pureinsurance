SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_RI_Arrangement_Line_Del'
GO


CREATE PROCEDURE spu_RI_Arrangement_Line_Del      
	@ri_arrangement_line_id INT      
AS      
    
	DELETE FROM ri_arrangement_line WHERE ri_arrangement_line_id = @ri_arrangement_line_id      

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
