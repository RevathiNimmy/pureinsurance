--Start (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (6.2.1.1)
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Reporting_On_Deleted_Data_Option'
GO
create procedure spu_Get_Reporting_On_Deleted_Data_Option 
			@branchCode int,  
 			@option_number int, 				
			@ReportOnDeleted bit out
as
Begin
	declare @value varchar(255)
	
	SELECT @value = value from system_options  
	WHERE branch_id = @branchCode and option_number = @Option_number  

	If @value = '1' 
	  Begin
	    set @ReportOnDeleted=1
	  end
	else
	  Begin
	    set @ReportOnDeleted=0
	  End
	
end
Go
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
--End (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (6.2.1.1)