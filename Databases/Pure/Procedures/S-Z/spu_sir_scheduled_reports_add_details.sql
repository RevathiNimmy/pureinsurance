SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_sir_scheduled_reports_add_details'
GO
CREATE  PROCEDURE spu_sir_scheduled_reports_add_details	
	@report_scheduler_id INT,
	@parameter_name VARCHAR(50) = NULL,	
	@default_value VARCHAR(50) = NULL,
	@data_type VARCHAR(50) = NULL,
	@prompt VARCHAR(50) = NULL,
	@currentid_value VARCHAR(50) = NULL,
	@party_search VARCHAR(50) = NULL,
	@empty VARCHAR(50) = NULL
AS

	INSERT INTO Report_Scheduler_Parameters 
	(report_scheduler_id, parameter_name, default_value, data_type, prompt, currentid_value, party_search, empty)
	VALUES(@report_scheduler_id, @parameter_name,@default_value,@data_type,@prompt,@currentid_value,@party_search,@empty)
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
