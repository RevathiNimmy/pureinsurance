SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'Spu_Info_Checklist_upd'
GO

CREATE PROCEDURE Spu_Info_Checklist_upd
    @risk_type_id int,
    @show_info_checklist int	

AS

    UPDATE  Risk_Type
    SET     show_information_checklist = @show_info_checklist
    WHERE   risk_type_id = @risk_type_id
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

