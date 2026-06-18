SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Party_Name'
GO
create PROCEDURE spu_Get_Party_Name  
  @party_code CHAR(10),
  @Branchcode int =0,
  @option_number int=0 
AS

    --Start (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (6.2.1.2)
    Declare @ReportOnDeleted bit 
    if @Branchcode <> 0 and @option_number <> 0
    Begin
    exec spu_Get_Reporting_On_Deleted_Data_Option @Branchcode, @option_number, @ReportOnDeleted out
    end
    --End (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (6.2.1.2)
	
    SELECT p.party_cnt, p.shortname, p.resolved_name
    FROM party p join party_type pt ON
	 p.party_type_id=pt.party_type_id
    WHERE 1= 1
    AND
        (
	 (
   (@party_code ='CO' OR @party_code ='AH') AND (pt.code =  'CO'  OR pt.code ='HC')  
	 )
  	 OR 
	 (
   pt.code = 'CO'  
	 )
        )
    
    --Start (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (6.2.1.2)
    AND (p.is_deleted=0 or @ReportOnDeleted=1)
    --End (Girija chokkalingam) - (Tech Spec - LOA009 - Hide Inactive Users.doc) - (6.2.1.2)
    ORDER BY p.resolved_name
Go
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
