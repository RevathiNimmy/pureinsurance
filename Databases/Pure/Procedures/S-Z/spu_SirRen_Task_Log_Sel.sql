SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Task_Log_Sel'
GO


/*
    TF - Created
    DD, 07/12/2001: Added optional filtering
*/

CREATE PROCEDURE spu_SirRen_Task_Log_Sel
(
    @StartDate  DATETIME=NULL,
    @EndDate    DATETIME=NULL,
    @Status INT=NULL,
    @PolicyNo   VARCHAR(255)=NULL
)
AS
BEGIN
    SELECT
        rtl.renewal_task_log_cnt,
        rtl.process_id,
        rtl.process_code,
        rtl.insurance_folder_cnt,
        rtl.insurance_file_cnt,
        rtl.policy_no,
        rtl.party_cnt,
        rtl.resolved_name,
        rtl.date,
        rtl.status,
        rtl.error_log
    FROM
        Renewal_Task_Log rtl
    WHERE
        (rtl.date>=@StartDate OR @StartDate IS NULL)
    AND (rtl.date<=@EndDate OR @EndDate IS NULL)
    AND (rtl.status=@Status OR @Status IS NULL)
    AND (rtl.policy_no=@PolicyNo OR @PolicyNo IS NULL)
    ORDER BY
        date DESC
END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


