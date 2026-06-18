SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PMWrk_Workflow_Step_Instance_del'
GO

--***************************************************************************
--  Revision    Description of Modification     Date        Who
--  --------    ---------------------------     ----        ---
--  1.0         Created                         17/01/2003  AMB
--
--***************************************************************************

CREATE PROCEDURE spu_PMWrk_Workflow_Step_Instance_del

(
    @pmwrk_workflow_step_instance_cnt int)
AS 
DELETE 

FROM 
    PMWrk_Workflow_Step_Instance
WHERE
    pmwrk_workflow_step_instance_cnt = @pmwrk_workflow_step_instance_cnt
GO

